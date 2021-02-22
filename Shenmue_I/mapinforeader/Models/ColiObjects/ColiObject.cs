using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.Models.ColiObjects {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiType1"/>
    ///<seealso cref="ColiType2"/>
    public class ColiObject {

        ///<summary>The size (in bytes) of a single precision floating point number in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int SINGLE_SIZE = 4;

        ///<summary>The size (in bytes) of an unsigned 32-bit integer in a <c>MAPINFO.BIN</c> file.</summary>
        public static readonly int UINT32_SIZE = 4;

        ///<summary>Default constructor.</summary>
        public ColiObject() { }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiObject(uint shapeId) { 
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID and shape ID</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiObject(uint layerId, uint shapeId) {
            this.LayerId = layerId;
            this.ShapeId = shapeId;
        }

        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiObject(uint layerId, uint shapeId, byte[] data) {
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
}
