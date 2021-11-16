using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable



namespace W3SHARE.Models
{
    public partial class File
    {
        public Guid FileId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? AlbumId { get; set; }
        public string Name { get; set; }
        public Guid? LastModifiedBy { get; set; }
        public string Url { get; set; }
        public DateTime? DateModified { get; set; }
        public string ContentType { get; set; }
        public DateTime? DateCreated { get; set; }

       

    }
}
