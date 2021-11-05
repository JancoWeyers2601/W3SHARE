using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace W3SHARE.Models
{
    public partial class Access
    {
        public Guid AccessId { get; set; }
        public Guid UserId { get; set; }
        public Guid FileId { get; set; }
    }
}
