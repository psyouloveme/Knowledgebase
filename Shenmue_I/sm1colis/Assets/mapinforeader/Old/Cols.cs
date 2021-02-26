using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using mapinforeader.Util;
using mapinforeader.Models.ColiObjects;

namespace mapinforeader.Old
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
            public void ParseColiContent () {
                this.ColiObjs = new List<ColiObj>();
                MemoryStream s = new MemoryStream(this.Content);
            }

            public void ReadColiObjs(Stream s)
            {
                using (BinaryReader r = new BinaryReader(s))
                {
                    while (r.BaseStream.Position < this.ContentOffset + this.Size)
                    {
                        // Console.WriteLine("trying to start reading");
                        ColiObj coli;
                        byte[] nextWord = new byte[4],
                            nextNextWord = new byte[4],
                            checkArray = new byte[3],
                            buffer = new byte[4],
                            coliTypeBytes = new byte[4];
                        uint coliType, coliCount;
                        uint? colisubtype;

                        // first 4 bytes are type?, usually 00
                        coliTypeBytes = r.ReadBytes(4);
                        // convert type to int since that's always present
                        coliType = BitConverter.ToUInt32(coliTypeBytes, 0);
                        // next 4 are either the subtype? or count?
                        nextWord = r.ReadBytes(4);
                        // this will either be the first content byte or
                        // the count of (at least for 00 02 header types)
                        nextNextWord = r.ReadBytes(4);

                        Array.Copy(nextNextWord, 1, checkArray, 0, 3);

                        // if the last 3 bytes of the number are all 00, this is a subtype? indicator
                        if (Array.TrueForAll(checkArray, p => p == 0))
                        {
                            // Console.WriteLine("trying to parse a coli with subtype");
                            colisubtype = BitConverter.ToUInt32(nextWord, 0);
                            coliCount = BitConverter.ToUInt32(nextNextWord, 0);
                            if (coliType == 0 && colisubtype.HasValue)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x02:
                                        coli = new ColiType0002(coliCount);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, coliCount);
                                        break;
                                }
                            }
                            else if (coliType == 09 && colisubtype.HasValue)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x02:
                                        coli = new ColiType0902(coliCount);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, coliCount);
                                        break;
                                }
                            }
                            else if (coliType == 07 && colisubtype.HasValue)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x02:
                                        coli = new ColiType0702(coliCount);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, coliCount);
                                        break;
                                }
                            }

                            else if (coliType == 0xC8 && colisubtype.HasValue)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x02:
                                        coli = new ColiTypeC802(coliCount);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, coliCount);
                                        break;
                                }
                            }
                            else
                            {
                                coli = new ColiObj(coliType, colisubtype, coliCount);
                            }
                        }
                        // otherwise this is the first content byte
                        else
                        {
                            colisubtype = BitConverter.ToUInt32(nextWord, 0);
                            if (coliType == 0)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x03:
                                        coli = new ColiType0003();
                                        break;
                                    case 0x01:
                                        coli = new ColiType0001();
                                        break;
                                    case 0x05:
                                        coli = new ColiType0005();
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, null);
                                        break;
                                }
                            }
                            else if (coliType == 0x64)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new ColiType6401();
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, null);
                                        break;
                                }
                            }
                            // else if (coliType == 0x6E)
                            // {
                            //   switch (colisubtype.Value)
                            //   {
                            //     case 0x01:
                            //       coli = new ColiType6E01();
                            //       break;
                            //     default:
                            //       coli = new ColiObj(coliType, colisubtype, null);
                            //       break;
                            //   }
                            // }
                            else if (coliType == 0x07)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new ColiType0701();
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, null);
                                        break;
                                }
                            }
                            else if (coliType == 0x08)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new ColiType0801();
                                        break;
                                    default:
                                        coli = new ColiObj();
                                        break;
                                }
                            }
                            else if (coliType == 0x09)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new ColiType0901();
                                        break;
                                    case 0x03:
                                        coli = new ColiType0903();
                                        break;
                                    case 0x05:
                                        coli = new ColiType0905();
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, null);
                                        break;
                                }
                            }
                            else if (coliType == 0x0A)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new ColiType0A01();
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype, null);
                                        break;
                                }
                            }
                            else if (coliType >= 0x65 && coliType <= 0x6D)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new Coli2d(coliType, 0x01);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype);
                                        break;
                                }
                            }
                            else if (coliType >= 0x6E && coliType <= 0x76)
                            {
                                switch (colisubtype.Value)
                                {
                                    case 0x01:
                                        coli = new Coli2d(coliType, 0x01);
                                        break;
                                    default:
                                        coli = new ColiObj(coliType, colisubtype);
                                        break;
                                }
                            }
                            else
                            {
                                coli = new ColiObj(coliType, colisubtype);
                            }
                            coli.ObjData.Add(BitConverter.ToSingle(nextNextWord, 0));
                        }
                        // read the remaining content until we hit FFFFFFFF
                        buffer = r.ReadBytes(4);
                        while (!Array.TrueForAll(buffer, b => b == 0xFF))
                        {
                            coli.ObjData.Add(BitConverter.ToSingle(buffer, 0));
                            buffer = r.ReadBytes(4);
                        }
                        this.ColiObjs.Add(coli);
                    }
                }
            }

            public void ReadColiObjsNew(Stream s)
            {
                using (BinaryReader r = new BinaryReader(s))
                {
                    while (r.BaseStream.Position < this.ContentOffset + this.Size)
                    {
                        ColiObject coli;
                        uint coliLayer = r.ReadUInt32();
                        uint coliShape = r.ReadUInt32();
                        // Console.WriteLine($"Creating coli with layer {coliLayer.ToString("X2")} shape: {coliShape.ToString("X2")}");
                        switch (coliShape) {
                            case 0x01:
                                coli = new ColiType1(coliLayer, r);
                                break;
                            case 0x02:
                                coli = new ColiType2(coliLayer, r);
                                break;
                            case 0x03:
                                coli = new ColiType3(coliLayer, r);
                                break;
                            case 0x05:
                                coli = new ColiType5(coliLayer, r);
                                break;
                            default:
                                coli = new ColiObject(coliLayer, coliShape);
                                break;
                                //throw new Exception($"Got an unexpected coli type: 0x{coliShape.ToString("X2")}");
                        }
                        this.ColiDatas.Add(coli);
                        // skip the terminator
                        r.BaseStream.Seek(4, SeekOrigin.Current);
                    }
                }
            }

            public ColiInfo() {
                ColiObjs = new List<ColiObj>();
                ColiDatas = new List<ColiObject>();
            }
            
            public List<ColiObj> ColiObjs { get; set; }
            public List<ColiObject> ColiDatas { get; set; }
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
                // SMFileUtils.WriteBytesToFile("D000_MAPINFO_COLS_COLI0_DEBUG.BIN.bytes", coliInfo.Content);
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
            coliInfo.ParseColiContent();
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
        public static List<Cols.ColiInfo> LocateColiOffsets(BinaryReader reader)
        {
            List<Cols.ColiInfo> c = new List<Cols.ColiInfo>();
            bool streamEnded = false;
            while (!streamEnded)
            {
                int i;
                for (i = 0; i < Cols.Headers.COLI.Length && !streamEnded; i++)
                {
                    byte b;
                    try
                    {
                        b = reader.ReadByte();
                    }
                    catch
                    {
                        streamEnded = true;
                        break;
                    }
                    if (b < 0)
                    {
                        streamEnded = true;
                    }
                    if (b != Cols.Headers.COLI[i])
                    {
                        break;
                    }
                }
                if (i == Cols.Headers.COLI.Length)
                {
                    var newcols = new Cols.ColiInfo();
                    newcols.HeaderOffset = reader.BaseStream.Position - i;
                    newcols.SizeOffset = reader.BaseStream.Position;
                    newcols.Size = BitConverter.ToUInt32(reader.ReadBytes(4), 0);
                    newcols.ContentOffset = reader.BaseStream.Position;
                    c.Add(newcols);
                }
            }
            return c;
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
                catch (EndOfStreamException)
                {
                    cols = null;
                    streamEnded = true;
                }
                catch (Exception)
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
                        catch (EndOfStreamException)
                        {
                            cols = null;
                            streamEnded = true;
                            endMismatch = true;
                        }
                        catch (Exception)
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
                        // SMFileUtils.WriteBytesToFile("COLS_D000_DEBUG.BIN.bytes", cols.Content);
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