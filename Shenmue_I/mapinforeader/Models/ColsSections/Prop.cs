using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.Models.ColsSections {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiType1"/>
    ///<seealso cref="ColiType2"/>
    public class Prop : ColsSection {
        public static readonly string Identifier = "PROP";
        public Prop() : base() {
            
        }
    }
}