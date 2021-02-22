using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using mapinforeader.Models.ColsSections;

namespace mapinforeader.Models.MapinfoSections {
    ///<summary>Models collision data from a <c>MAPINFO.BIN</c> files's <c>COLI</c> structure.
    ///This is a generic <c>COLI</c>, prefer using a specific type class.</summary>
    ///<seealso cref="ColiType1"/>
    ///<seealso cref="ColiType2"/>
    public class Cols : MapinfoSection {
        public static readonly string Identifier = "COLS";
        public List<Coli> Colis { get; set; }
        public List<Hght> Hghts { get; set; }
        public List<Evnt> Evnts { get; set; }
        public List<Sond> Sonds { get; set; }
        public List<Undu> Undus { get; set; }
        public Cols() : base() {
            this.Colis = new List<Coli>();
            this.Hghts = new List<Hght>();
            this.Evnts = new List<Evnt>();
            this.Sonds = new List<Sond>();
            this.Undus = new List<Undu>();
        }
    }
}