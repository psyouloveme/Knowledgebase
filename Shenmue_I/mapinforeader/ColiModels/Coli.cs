using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.ColiModels {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiDataType1"/>
    ///<seealso cref="ColiDataType2"/>
    public class ColiData {