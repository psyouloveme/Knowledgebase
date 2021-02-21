using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.ColiModels {

    ///<summary>Models collision data of type <c>0x02</c> from a <c>MAPINFO.BIN</c> files's 
    ///<c>COLI</c> structure.</summary>
    public class ColiDataType1 : ColiDataMultiCoord {

        ///<summary>The size (in bytes) of an X, Z coordinate as stored in the inner data.</summary>
        public static readonly int COORD_SIZE = ColiData.SINGLE_SIZE * 2;

        ///<summary>The static size of a <c>0x01</c> <c>COLI</c>'s inner data.</summary>
        public static readonly int DATA_SIZE = ColiDataType1.COORD_SIZE * 2;

        ///<summary>Default constructor. Sets <c>ShapeId=1</c></summary>
        public ColiDataType1() : base(1) { }

        ///<summary>Constructor for when data will be added later</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        public ColiDataType1(uint layerId) : base(layerId, 1) { }

        ///<summary>Constructor for when data has already been read</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataType1(uint layerId, byte[] data) : base(layerId, 1, data) { 
            this.PopulateCoordinates();
        }

        ///<summary>Constructor to read values directly from a stream reader</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="reader">The reader to populate object data from. 
        /// <paramref name="reader"/>'s position must be at the starting offset of the structure's data.</param>
        public ColiDataType1(uint layerId, BinaryReader reader) : base(layerId,1) {
            long position = reader.BaseStream.Position;
            this.Data = reader.ReadBytes(ColiDataType1.DATA_SIZE);
            this.PopulateCoordinates(reader, position);
        }

        ///<summary>Populate <c>this.Coordinates</c> with values from a BinaryReader</summary>
        ///<param name="reader">The reader to populate object data from.</param>
        ///<param name="position">The starting offset (from the stream's beginning) to read data from</param>
        private void PopulateCoordinates(BinaryReader reader, long position) {
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            for (int i = 0; i < ColiDataType1.DATA_SIZE / ColiDataType1.COORD_SIZE; i++) {
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
                    this.PopulateCoordinates(r, 0);
                }
            }
        }
    }
}
