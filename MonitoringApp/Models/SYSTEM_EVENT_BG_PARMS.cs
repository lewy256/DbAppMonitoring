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
    
    public partial class SYSTEM_EVENT_BG_PARMS
    {
        public long SNAP_ID { get; set; }
        public long INST_ID { get; set; }
        public string EVENT { get; set; }
        public long TOTAL_WAITS { get; set; }
        public long TIME_WAITED { get; set; }
    
        public virtual SNAPSHOT_PARMS SNAPSHOT_PARMS { get; set; }
    }
}
