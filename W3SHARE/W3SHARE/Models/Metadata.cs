using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace W3SHARE.Models
{
    public partial class Metadata
    {
        public Guid MetadataId { get; set; }
        public Guid? FileId { get; set; }
        public string GeoLocation { get; set; }
        public string Tags { get; set; }
        public string CaptureBy { get; set; }
        public DateTime? CaptureDate { get; set; }
    }
}
