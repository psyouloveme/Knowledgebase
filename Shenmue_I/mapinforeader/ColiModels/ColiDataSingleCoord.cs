using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.ColiModels {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c> that contains a single 2D coordinate.
    ///Prefer using a specific type class that can initialize the coordinate and collect other data.</summary>
    ///<seealso cref="ColiDataType3"/>
    ///<seealso cref="ColiDataType5"/>
    public class ColiDataSingleCoord : ColiData
    {
        ///<summary>Default constructor.</summary>
        public ColiDataSingleCoord() 
            : base() { }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataSingleCoord(uint shapeId) 
            : base(shapeId) { }

        ///<summary>Constructor that initializes layer ID and shape ID</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataSingleCoord(uint layerId, uint shapeId) 
            : base(layerId, shapeId) { }

        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataSingleCoord(uint layerId, uint shapeId, byte[] data) 
            : base(layerId, shapeId, data) { }

        ///<summary>A plottable coordinate derived from <c>this.Data</c> or
        ///read from a <c>MemoryStream</c> provided to the constructor of a derived class</summary>
        public Vector3 Coordinate { get; set; }
    }
}
