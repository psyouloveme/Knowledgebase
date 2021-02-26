using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.Models.ColiObjects {

    ///<summary>Models collision data of type <c>0x05</c> from a <c>MAPINFO.BIN</c> file's
    ///<c>COLI</c> structure.</summary>
    public class ColiType5 : ColiSingleCoord {

        ///<summary>The size (in bytes) of an X, Z coordinate as stored in the inner data.</summary>
        public static readonly int COORD_SIZE = ColiObject.SINGLE_SIZE * 2;

        ///<summary>The size (in bytes) of the mystery data in type 5 colis.</summary>
        public static readonly int MYSTERY_SIZE = 0x10;

        ///<summary>The size (in bytes) of the mystery data in type 5 colis.</summary>
        public static readonly int DATA_SIZE = ColiType5.COORD_SIZE + ColiType5.MYSTERY_SIZE;

        ///<summary>The count of coordinates, as read from the inner data</summary>
        public byte[] MysteryBytes { get; set; }

        ///<summary>Default constructor. Sets ShapeId = 5</summary>
        public ColiType5() : base(5) { }

        ///<summary>Constructor for when data will be added later</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        public ColiType5(uint layerId) : base(layerId, 5) { }

        ///<summary>Constructor for when data has already been read</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiType5(uint layerId, byte[] data) : base(layerId, 5, data) { 
            this.PopulateCoordinate();
        }

        ///<summary>Constructor to read values directly from the stream reader</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="reader">The reader to populate object data from. 
        /// Position must be at the starting offset of the structure's data.</param>
        public ColiType5(uint layerId, BinaryReader reader) : base(layerId, 5) {
            long streamPosition = reader.BaseStream.Position;
            reader.BaseStream.Seek(ColiType5.COORD_SIZE, SeekOrigin.Current);

            // read the unkown bytes and seek back to capture it in Data
            this.MysteryBytes = reader.ReadBytes(ColiType5.MYSTERY_SIZE);
            reader.BaseStream.Seek(streamPosition, SeekOrigin.Begin);

            // read the full data length (unless we just threw)
            this.Data = reader.ReadBytes(ColiType5.DATA_SIZE);

            // store stream position of the end of the data 
            // so we can come back to it after populating the coordinate
            long endOfDataPos = reader.BaseStream.Position;
            this.PopulateCoordinate(reader, streamPosition);
            
            // return to the end of data
            reader.BaseStream.Seek(endOfDataPos, SeekOrigin.Begin);
        }

        ///<summary>Populate <c>this.Coordinates</c> with values from a BinaryReader</summary>
        ///<param name="reader">The reader to populate object data from.</param>
        ///<param name="position">The starting offset (from the stream's beginning) to read data from</param>
        private void PopulateCoordinate(BinaryReader reader, long position) {
            reader.BaseStream.Seek(position, SeekOrigin.Begin);
            // read the coordinates
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            this.Coordinate = vec;
        }

        ///<summary>Populate <c>this.Coordinates</c> with values from <c>this.Data</c></summary>
        private void PopulateCoordinate() {
            using(MemoryStream m  = new MemoryStream(this.Data)) {
                using (BinaryReader r = new BinaryReader(m)) {
                    PopulateCoordinate(r, 0);
                }
            }
        }
    }
}
