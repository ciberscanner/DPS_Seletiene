//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SeletieneDPS.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class state
    {
        public state()
        {
            this.productservice = new HashSet<productservice>();
            this.productservice1 = new HashSet<productservice>();
            this.userapp = new HashSet<userapp>();
            this.userapp1 = new HashSet<userapp>();
        }
    
        public int id { get; set; }
        public string value { get; set; }
    
        public virtual ICollection<productservice> productservice { get; set; }
        public virtual ICollection<productservice> productservice1 { get; set; }
        public virtual ICollection<userapp> userapp { get; set; }
        public virtual ICollection<userapp> userapp1 { get; set; }
    }
}