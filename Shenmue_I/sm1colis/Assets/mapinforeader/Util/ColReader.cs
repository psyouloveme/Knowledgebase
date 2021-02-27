using System;
using System.IO;
using mapinforeader.Models.ColiObjects;
using mapinforeader.Models.ColsSections;
using mapinforeader.Models.MapinfoSections;
using System.Text;

namespace mapinforeader.Util
{
    public class ColsReader : BinaryReader
    {
        public ColsReader(Stream s) : base(s) { }

        public ColsReader(Stream s, Encoding e) : base(s, e) { }

        public ColsReader(Stream s, Encoding e, Boolean b) : base(s, e, b) { }

        public Cols ReadCols() {
            Cols cols = this.ReadColsMetadata();
            if (cols != null) {
                this.ReadColis(cols);
                this.ReadHghts(cols);
            }
            return cols;
        }

        public Cols ReadColsMetadata() {
            Cols cols = null;
            long? position = SMFileUtils.FindNextString(this, Cols.Identifier);
            if (position.HasValue) {
                cols = new Cols();
                cols.HeaderOffset = position.Value;
                cols.SizeOffset = position.Value + Cols.Identifier.Length;
                this.BaseStream.Seek(position.Value + Cols.Identifier.Length, SeekOrigin.Begin);
                cols.Size = BitConverter.ToUInt32(this.ReadBytes(4), 0);
                cols.ContentOffset = this.BaseStream.Position;
            }
            return cols;
        }

        public Coli ReadColiMetadata() {
            Coli coli = null;
            long? position = SMFileUtils.FindNextString(this, Coli.Identifier);
            if (position.HasValue) {
                coli = new Coli();
                coli.HeaderOffset = position.Value;
                coli.SizeOffset = position.Value + Coli.Identifier.Length;
                this.BaseStream.Seek(position.Value + Coli.Identifier.Length, SeekOrigin.Begin);
                coli.Size = BitConverter.ToUInt32(this.ReadBytes(4), 0);
                coli.ContentOffset = this.BaseStream.Position;
            }
            return coli;
        }

        public Hght ReadHghtMetadata() {
            Hght hght = null;
            long? position = SMFileUtils.FindNextString(this, Hght.Identifier);
            if (position.HasValue) {
                hght = new Hght();
                hght.HeaderOffset = position.Value;
                hght.SizeOffset = position.Value + Hght.Identifier.Length;
                this.BaseStream.Seek(position.Value + Hght.Identifier.Length, SeekOrigin.Begin);
                hght.Size = BitConverter.ToUInt32(this.ReadBytes(4), 0);
                hght.ContentOffset = this.BaseStream.Position;
            }
            return hght;
        }

        public Coli ReadColis(Cols cols) {
            this.BaseStream.Seek(cols.HeaderOffset, SeekOrigin.Begin);
            Coli coli = this.ReadColiMetadata();
            while (coli != null) {
                this.BaseStream.Seek(coli.ContentOffset, SeekOrigin.Begin);
                this.ReadColiObjects(coli);
                cols.Colis.Add(coli);
                coli = this.ReadColiMetadata();
            }
            return coli;
        }

        public Hght ReadHghts(Cols cols) {
            this.BaseStream.Seek(cols.HeaderOffset, SeekOrigin.Begin);
            Hght hght = this.ReadHghtMetadata();
            while (hght != null) {
                this.BaseStream.Seek(hght.ContentOffset, SeekOrigin.Begin);
                this.ReadHghtObjects(hght);
                cols.Hghts.Add(hght);
                hght = this.ReadHghtMetadata();
            }
            return hght;
        }

        public void ReadHghtObjects(Hght hght) {
            this.BaseStream.Seek(hght.ContentOffset, SeekOrigin.Begin);
            while (this.BaseStream.Position < (hght.ContentOffset + hght.Size)) {
                HghtObject newHghtObject;
                byte[] mysteryData = this.ReadBytes(HghtObject.MYSTERY_SIZE);
                uint hghtShape = this.ReadUInt32();
                newHghtObject = new HghtObject(hghtShape, this);
                newHghtObject.MysteryBytes = mysteryData;
                hght.HghtDatas.Add(newHghtObject);
                // skip the terminator
                this.BaseStream.Seek(4, SeekOrigin.Current);                
            }
        }

        public void ReadColiObjects(Coli coli) {
            this.BaseStream.Seek(coli.ContentOffset, SeekOrigin.Begin);
            while (this.BaseStream.Position < (coli.ContentOffset + coli.Size)) {
                ColiObject newColiObj;
                uint coliLayer = this.ReadUInt32();
                uint coliShape = this.ReadUInt32();
                switch (coliShape)
                {
                    case 0x01:
                        newColiObj = new ColiType1(coliLayer, this);
                        break;
                    case 0x02:
                        newColiObj = new ColiType2(coliLayer, this);
                        break;
                    case 0x03:
                        newColiObj = new ColiType3(coliLayer, this);
                        break;
                    case 0x05:
                        newColiObj = new ColiType5(coliLayer, this);
                        break;
                    default:
                        newColiObj = new ColiObject(coliLayer, coliShape);
                        break;
                }
                coli.ColiDatas.Add(newColiObj);
                // skip the terminator
                this.BaseStream.Seek(4, SeekOrigin.Current);                
            }
        }
    }
}