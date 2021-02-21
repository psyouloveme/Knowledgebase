using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.ColiModels {

    ///<summary>Models collision data of type <c>0x02</c> from a <c>MAPINFO.BIN</c> files's 
    ///<c>COLI</c> structure.</summary>
    public class ColiDataType2 : ColiDataMultiCoord {

        ///<summary>The size (in bytes) of an X, Z coordinate as stored in the inner data.</summary>
        public static readonly int COORD_SIZE = ColiData.SINGLE_SIZE * 2;
        ///<summary>The count of coordinates, as read from the inner data</summary>
        public uint Count { get; set; }

        ///<summary>Default constructor. Sets ShapeId = 2</summary>
        public ColiDataType2() : base(2) { }

        ///<summary>Constructor for when data will be added later</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        public ColiDataType2(uint layerId) : base(layerId, 2) { }

        ///<summary>Constructor for when data has already been read</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataType2(uint layerId, byte[] data) : base(layerId, 2, data) { 
            this.PopulateCoordinates();
        }

        ///<summary>Constructor to read values directly from the stream reader</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="reader">The reader to populate object data from. 
        /// Position must be at the starting offset of the structure's data.</param>
        public ColiDataType2(uint layerId, BinaryReader reader) : base(layerId, 2) {
            long streamPosition = reader.BaseStream.Position;
            // read the count of coordinates and seek back to capture the count in Data
            this.Count = reader.ReadUInt32();
            reader.BaseStream.Seek(-ColiData.UINT32_SIZE, SeekOrigin.Current);

            // c# has a memory limit for single objects, so data can't have a 
            // long length so this should be a safe conversion. if not it'll throw here
            int dataLength = (Convert.ToInt32(this.Count) * COORD_SIZE) + ColiData.UINT32_SIZE;

            // read the full data length (unless we just threw)
            this.Data = reader.ReadBytes(dataLength);

            this.PopulateCoordinates(reader, streamPosition);
        }

        ///<summary>Populate <c>this.Coordinates</c> with values from a BinaryReader</summary>
        ///<param name="reader">The reader to populate object data from.</param>
        ///<param name="position">The starting offset (from the stream's beginning) to read data from</param>
        private void PopulateCoordinates(BinaryReader reader, long position) {
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            // skip the count, since that's already stored
            reader.BaseStream.Seek(ColiData.UINT32_SIZE, SeekOrigin.Current);
            // read the coordinates
            for (int i = 0; i < this.Count; i++) {
                Vector3 vec = new Vector3();
                vec.X = reader.ReadSingle();
                vec.Z = reader.ReadSingle();
                this.Coordinates.Add(vec);
            }
        }

        ///<summary>Populate <c>this.Coordinates</c> with values from <c>this.Data</c></summary>
        private void PopulateCoordinates() {
            using(MemoryStream m  = new MemoryStream(this.Data)) {
                using (BinaryReader r = new BinaryReader(m)) {
                    PopulateCoordinates(r, 0);
                }
            }
        }
    }
}
