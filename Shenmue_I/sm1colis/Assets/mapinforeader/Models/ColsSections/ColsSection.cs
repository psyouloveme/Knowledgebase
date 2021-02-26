using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace mapinforeader.Models.ColsSections {

    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiType1"/>
    ///<seealso cref="ColiType2"/>
    public class ColsSection {
        public long HeaderOffset { get; set; }
        public long SizeOffset { get; set; }   
        public uint Size { get; set; }
        public long ContentOffset { get; set; }
    }
}