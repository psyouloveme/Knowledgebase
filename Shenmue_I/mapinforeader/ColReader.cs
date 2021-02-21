using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using mapinforeader.Utils;
using mapinforeader.ColiModels;

namespace mapinforeader
{
    public class ColReader
    {
        public long GetColiOffset 
        public List<ColiData> ReadColiObjsNew(Stream s)
        {
            var colis = new List<ColiData>();
            using (BinaryReader r = new BinaryReader(s))
            {
                while (r.BaseStream.Position < this.ContentOffset + this.Size)
                {
                    ColiData coli;
                    uint coliLayer = r.ReadUInt32();
                    uint coliShape = r.ReadUInt32();
                    Console.WriteLine($"Creating coli with layer {coliLayer.ToString("X2")} shape: {coliShape.ToString("X2")}");
                    switch (coliShape)
                    {
                        case 0x01:
                            coli = new ColiDataType1(coliLayer, r);
                            break;
                        case 0x02:
                            coli = new ColiDataType2(coliLayer, r);
                            break;
                        case 0x03:
                            coli = new ColiDataType3(coliLayer, r);
                            break;
                        case 0x05:
                            coli = new ColiDataType5(coliLayer, r);
                            break;
                        default:
                            coli = new ColiData(coliLayer, coliShape);
                            break;
                            //throw new Exception($"Got an unexpected coli type: 0x{coliShape.ToString("X2")}");
                    }
                    colis.Add(coli);
                    // skip the terminator
                    r.BaseStream.Seek(4, SeekOrigin.Current);
                }
            }
            return colis;
        }
    }
}