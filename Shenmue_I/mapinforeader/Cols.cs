using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using mapinforeader.Utils;

namespace mapinforeader
{
    public class Cols
    {
        public static class Headers
        {
            public const string COLS = "COLS";
            public const string COLI = "COLI";
            public static readonly string HGHT = "HGHT";
            public static readonly string EVNT = "EVNT";
            public static readonly string UNDU = "UNDU";
            public static readonly string SOND = "SOND";
            public static readonly string PROP = "PROP";
            public static readonly string WALK = "WALK";
        }

        public class ColiInfo
        {
            public class ColiObj {
                public uint ObjType { get; set; }
                public uint? ObjSubTypeOrSomething { get; set; }
                public uint ObjCount { get; set; }
                public List<float> ObjData { get ;set; }
            }

            public void ReadColiObjs() {
                this.ColiObjs = new List<ColiObj>();
                MemoryStream s = new MemoryStream(this.Content);
                using (BinaryReader r = new BinaryReader(s)) {
                    while(r.BaseStream.Position < this.Content.Length) {
                        ColiObj newObj = new ColiObj();
                        newObj.ObjData = new List<float>();
                        
                        byte[] objTypeBytes = r.ReadBytes(4);
                        newObj.ObjType = BitConverter.ToUInt32(objTypeBytes);

                        byte[] nextWord = r.ReadBytes(4), 
                            nextNextWord = r.ReadBytes(4), 
                            checkArray = new byte[3],
                            buffer = new byte[4];
                        Array.Copy(nextNextWord, 1, checkArray, 0, 3);
                        if (Array.TrueForAll(checkArray, p => p == 0)) {
                            newObj.ObjSubTypeOrSomething = BitConverter.ToUInt32(nextWord);
                            newObj.ObjCount = BitConverter.ToUInt32(nextNextWord);
                        } else {
                            newObj.ObjCount = BitConverter.ToUInt32(nextWord);
                            newObj.ObjData.Add(BitConverter.ToSingle(nextNextWord));
                        }
                        buffer = r.ReadBytes(4);
                        while (!Array.TrueForAll(buffer, b => b == 0xFF)) {
                            newObj.ObjData.Add(BitConverter.ToSingle(buffer));
                            buffer = r.ReadBytes(4);
                        }
                        this.ColiObjs.Add(newObj);
                    }
                }
            }

            public List<ColiObj> ColiObjs { get; set; }
            public long HeaderOffset { get; set; }
            public long SizeOffset { get; set; }
            public long ContentOffset { get; set; }
            public uint Size { get; set; }
            public byte[] Content { get; set; }
        }
        public long HeaderOffset { get; set; }
        public long SizeOffset { get; set; }
        public long ContentOffset { get; set; }
        public long IdentifierOffset { get; set; }
        public long Id_MaybeOffset { get; set; }
        public byte[] Content { get; set; }
        public uint Size { get; set; }
        public string Identifier { get; set; }
        public uint Id_Maybe { get; set; }
        public List<ColiInfo> Colis { get; set; }
        public byte[] Hght { get; set; }
        public byte[] Evnt { get; set; }
        public byte[] Undu { get; set; }
        public byte[] Sond { get; set; }
        public byte[] Prop { get; set; }
        public byte[] Walk { get; set; }

        public static IEnumerable<string> HeaderList
        {
            get
            {
                PropertyInfo[] props = typeof(Cols.Headers).GetProperties();
                List<string> headerList = new List<string>();
                foreach (PropertyInfo prop in props)
                {
                    headerList.Add((string)prop.GetValue(null));
                }
                return headerList;
            }
        }

        public void ReadColi(BinaryReader reader)
        {
            ColiInfo coliInfo = new ColiInfo();
            coliInfo.HeaderOffset = reader.BaseStream.Position - 4;
            coliInfo.SizeOffset = reader.BaseStream.Position;

            byte[] coliSizeBytes = reader.ReadBytes(4);
            UInt32 coliSize = BitConverter.ToUInt32(coliSizeBytes, 0);
            coliInfo.Size = coliSize;

            if (coliSize > 0)
            {
                coliInfo.ContentOffset = reader.BaseStream.Position;
                coliInfo.Content = reader.ReadBytes(Convert.ToInt32(coliSize));
                SMFileUtils.WriteBytesToFile("D000_MAPINFO_COLS_COLI0_DEBUG.BIN.bytes", coliInfo.Content);
            }
            else
            {
                coliInfo.ContentOffset = 0;
                coliInfo.Content = null;
            }
            // var floatList = coliInfo.GetContentAsPoints();
            
            if (this.Colis == null)
            {
                this.Colis = new List<ColiInfo>();
            }
            coliInfo.ReadColiObjs();
            this.Colis.Add(coliInfo);
        }

        public void ProcessCOLS(BinaryReader reader)
        {
            // this will start with a section header
            // probably COLS
            char[] header = reader.ReadChars(4);
            string headerStr = new string(header);
            switch (headerStr)
            {
                case Cols.Headers.COLI:
                    this.ReadColi(reader);
                    break;
                default:
                    break;
            }
        }

        public static Cols ReadCols(BinaryReader reader)
        {
            bool streamEnded = false, matched = false;
            Cols cols = new Cols();

            while (!matched && !streamEnded)
            {
                bool foundStart = false, endMismatch = false;
                // find the starting byte
                try
                {
                    byte nextByte = 1;
                    while (nextByte > 0 && !foundStart && !streamEnded)
                    {
                        nextByte = reader.ReadByte();
                        if (nextByte == (int)Cols.Headers.COLS[0])
                        {
                            foundStart = true;
                        }
                        if (nextByte < 0)
                        {
                            streamEnded = true;
                            cols = null;
                        }
                    }
                }
                catch (EndOfStreamException e)
                {
                    cols = null;
                    streamEnded = true;
                }
                catch (Exception e)
                {
                    cols = null;
                }
                if (!streamEnded && foundStart)
                {
                    // check for the subsequent bytes
                    string remainingChars = Cols.Headers.COLS.Substring(1);
                    for (int i = 0; i < remainingChars.Length && !streamEnded && !endMismatch; i++)
                    {
                        try
                        {
                            byte anotherByte = reader.ReadByte();
                            if (anotherByte < 0)
                            {
                                streamEnded = true;
                                endMismatch = true;
                            }
                            else if (anotherByte != (int)remainingChars[i])
                            {
                                endMismatch = true;
                            }
                        }
                        catch (EndOfStreamException e)
                        {
                            cols = null;
                            streamEnded = true;
                            endMismatch = true;
                        }
                        catch (Exception e)
                        {
                            cols = null;
                            endMismatch = true;
                        }
                    }
                    if (foundStart && !streamEnded && !endMismatch)
                    {
                        cols.HeaderOffset = reader.BaseStream.Position - 4;
                        cols.SizeOffset = reader.BaseStream.Position;

                        // this size includes the 4 bytes for the coli data.
                        byte[] colsSizeBytes = reader.ReadBytes(4);
                        UInt32 colsSize = BitConverter.ToUInt32(colsSizeBytes, 0);
                        cols.Size = colsSize;

                        // get the idenfifier??
                        cols.IdentifierOffset = reader.BaseStream.Position;
                        char[] identifier = reader.ReadChars(4);
                        cols.Identifier = new string(identifier);
                        matched = true;

                        cols.Id_MaybeOffset = reader.BaseStream.Position;
                        byte[] coliIdMaybe = reader.ReadBytes(4);
                        UInt32 coliIdMaybeInt = BitConverter.ToUInt32(colsSizeBytes, 0);
                        cols.Id_Maybe = coliIdMaybeInt;
                        cols.ContentOffset = reader.BaseStream.Position;
                        // subtract the 4 bytes for the size, read the remainder of the coli content
                        cols.Content = reader.ReadBytes(Convert.ToInt32(colsSize) - 16);
                        SMFileUtils.WriteBytesToFile("COLS_D000_DEBUG.BIN.bytes", cols.Content);
                    }
                }
            }
            if (cols.Content != null && cols.Content.Length > 0)
            {
                reader.BaseStream.Seek(cols.ContentOffset, SeekOrigin.Begin);
                cols.ProcessCOLS(reader);
            }
            return cols;
        }
    }
}