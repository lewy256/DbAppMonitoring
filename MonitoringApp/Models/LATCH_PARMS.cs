//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MonitoringApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LATCH_PARMS
    {
        public long SNAP_ID { get; set; }
        public long INST_ID { get; set; }
        public long GETS { get; set; }
        public long MISSES { get; set; }
    
        public virtual SNAPSHOT_PARMS SNAPSHOT_PARMS { get; set; }
    }
}
