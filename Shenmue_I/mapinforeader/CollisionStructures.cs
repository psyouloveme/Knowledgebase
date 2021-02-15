using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiDataType1"/>
    ///<seealso cref="ColiDataType2"/>
    public class ColiData {

        ///<summary>The size (in bytes) of a single precision floating point number in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int SINGLE_SIZE = 4;

        ///<summary>The size (in bytes) of an unsigned 32-bit integer in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int UINT32_SIZE = 4;

        ///<summary>Default constructor.</summary>
        public ColiData() { }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiData(uint shapeId) { 
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID and shape ID</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiData(uint layerId, uint shapeId) {
            this.LayerId = layerId;
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiData(uint layerId, uint shapeId, byte[] data) {
            this.LayerId = layerId;
            this.ShapeId = shapeId;
            this.Data = data;
        }

        ///<summary>The Layer ID (?) of this collison object</summary>
        public uint LayerId { get; set; }

        ///<summary>The Shape ID (type of shape) of this collision object</summary>
        public uint ShapeId { get; set; }

        ///<summary>The inner data of this collision object</summary>
        public byte[] Data { get; set; }

    }

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c> that contains a list of 2D coordinates.
    ///Prefer using a specific type class that can initialize the coordinate list.</summary>
    ///<seealso cref="ColiDataType1"/>
    ///<seealso cref="ColiDataType2"/>
    public class ColiDataMultiCoord : ColiData {

        ///<summary>Default constructor.</summary>
        public ColiDataMultiCoord() { 
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataMultiCoord(uint shapeId) { 
            this.Coordinates = new List<Vector3>();
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID and shape ID</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataMultiCoord(uint layerId, uint shapeId) {
            this.Coordinates = new List<Vector3>();
            this.LayerId = layerId;
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataMultiCoord(uint layerId, uint shapeId, byte[] data) {
            this.Coordinates = new List<Vector3>();
            this.LayerId = layerId;
            this.ShapeId = shapeId;
            this.Data = data;
        }
        
        ///<summary>A list of plottable coordinates derived from <c>this.Data</c> or
        ///read from a <c>MemoryStream</c> provided to the constructor of a derived class</summary>
        public List<Vector3> Coordinates { get; set; }
    }

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

    ///<summary>Models collision data of type <c>0x03</c> from a <c>MAPINFO.BIN</c> files's 
    ///<c>COLI</c> structure.</summary>
    public class ColiDataType3 : ColiDataMultiCoord {

        ///<summary>The size (in bytes) of an X, Z coordinate as stored in the inner data.</summary>
        public static readonly int COORD_SIZE = ColiData.SINGLE_SIZE * 2;

        ///<summary>The size (in bytes) of the mystery data in type 3 colis.</summary>
        public static readonly int MYSTERY_SIZE = 4;

        ///<summary>The size (in bytes) of the mystery data in type 3 colis.</summary>
        public static readonly int DATA_SIZE = ColiDataType3.COORD_SIZE + ColiDataType3.MYSTERY_SIZE;

        ///<summary>The count of coordinates, as read from the inner data</summary>
        public byte[] MysteryBytes { get; set; }

        ///<summary>Default constructor. Sets ShapeId = 2</summary>
        public ColiDataType3() : base(3) { }

        ///<summary>Constructor for when data will be added later</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        public ColiDataType3(uint layerId) : base(layerId, 3) { }

        ///<summary>Constructor for when data has already been read</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataType3(uint layerId, byte[] data) : base(layerId, 3, data) { 
            this.PopulateCoordinates();
        }

        ///<summary>Constructor to read values directly from the stream reader</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="reader">The reader to populate object data from. 
        /// Position must be at the starting offset of the structure's data.</param>
        public ColiDataType3(uint layerId, BinaryReader reader) : base(layerId, 3) {
            long streamPosition = reader.BaseStream.Position;
            // read the count of coordinates and seek back to capture the count in Data
            this.MysteryBytes = reader.ReadBytes(ColiDataType3.MYSTERY_SIZE);
            reader.BaseStream.Seek(-ColiDataType3.MYSTERY_SIZE, SeekOrigin.Current);

            // read the full data length (unless we just threw)
            this.Data = reader.ReadBytes(ColiDataType3.DATA_SIZE);

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
            Vector3 vec = new Vector3();
            vec.X = reader.ReadSingle();
            vec.Z = reader.ReadSingle();
            this.Coordinates.Add(vec);
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
