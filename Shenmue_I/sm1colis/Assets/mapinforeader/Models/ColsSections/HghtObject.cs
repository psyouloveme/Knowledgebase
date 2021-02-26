using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.Models.ColsSections {

    public class HghtObject {
        ///<summary>The size (in bytes) of a single precision floating point number in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int SINGLE_SIZE = 4;

        ///<summary>The size (in bytes) of an unsigned 32-bit integer in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int UINT32_SIZE = 4;

        ///<summary>The size (in bytes) of the mystery data at the start of HGHT data.</summary>
        public static readonly int MYSTERY_SIZE = 4;

        ///<summary>The size (in bytes) of an X, Z coordinate as stored in the inner data.</summary>
        public static readonly int COORD_SIZE = HghtObject.SINGLE_SIZE * 2;
        
        ///<summary>The count of coordinates, as read from the inner data</summary>
        public uint Count { get; set; }

        ///<summary>A list of plottable coordinates derived from <c>this.Data</c> or
        ///read from a <c>MemoryStream</c> provided to the constructor of a derived class</summary>
        public List<Vector3> Coordinates { get; set; }


        ///<summary>Default constructor.</summary>
        public HghtObject() { 
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public HghtObject(uint shapeId) { 
            this.ShapeId = shapeId;
            this.Coordinates = new List<Vector3>();
        }


        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public HghtObject(uint shapeId, byte[] data) {
            this.ShapeId = shapeId;
            this.Data = data;
            this.Coordinates = new List<Vector3>();
            this.PopulateCoordinates();
        }

        ///<summary>The Shape ID (type of shape) of this collision object</summary>
        public uint ShapeId { get; set; }

        ///<summary>The inner data of this collision object</summary>
        public byte[] Data { get; set; }

        ///<summary>The mystery data</summary>
        public byte[] MysteryBytes { get; set; }

        ///<summary>Constructor to read values directly from the stream reader</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="reader">The reader to populate object data from. 
        /// Position must be at the starting offset of the structure's data.</param>
        public HghtObject(uint shapeId, BinaryReader reader) {
            this.ShapeId = shapeId;
            this.Coordinates = new List<Vector3>();
            long streamPosition = reader.BaseStream.Position;
            // read the count of coordinates and seek back to capture the count in Data
            this.Count = reader.ReadUInt32();
            reader.BaseStream.Seek(-HghtObject.UINT32_SIZE, SeekOrigin.Current);

            // c# has a memory limit for single objects, so data can't have a 
            // long length so this should be a safe conversion. if not it'll throw here
            int dataLength = (Convert.ToInt32(this.Count) * HghtObject.COORD_SIZE) + HghtObject.UINT32_SIZE;

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
            reader.BaseStream.Seek(HghtObject.UINT32_SIZE, SeekOrigin.Current);
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