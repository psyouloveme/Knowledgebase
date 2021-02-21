using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.ColiModels {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c> that contains a list of 2D coordinates.
    ///Prefer using a specific type class that can initialize the coordinate list.</summary>
    ///<seealso cref="ColiDataType1"/>
    ///<seealso cref="ColiDataType2"/>
    public class ColiDataMultiCoord : ColiData
    {

        ///<summary>Default constructor.</summary>
        public ColiDataMultiCoord() : base()
        {
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>Constructor that initializes shape ID</summary>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataMultiCoord(uint shapeId) : base(shapeId)
        {
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>Constructor that initializes layer ID and shape ID</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        public ColiDataMultiCoord(uint layerId, uint shapeId) : base(layerId, shapeId)
        {
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>Constructor that initializes layer ID, shape ID and data</summary>
        ///<param name="layerId">The Layer ID(?) for this Coli structure</param>
        ///<param name="shapeId">The Shape ID of the collision object</param>
        ///<param name="data">The inner data of the structure</param>
        public ColiDataMultiCoord(uint layerId, uint shapeId, byte[] data) : base(layerId, shapeId, data)
        {
            this.Coordinates = new List<Vector3>();
        }

        ///<summary>A list of plottable coordinates derived from <c>this.Data</c> or
        ///read from a <c>MemoryStream</c> provided to the constructor of a derived class</summary>
        public List<Vector3> Coordinates { get; set; }
    }
}
