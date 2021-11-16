using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace W3SHARE.Models
{
    public partial class FileMetadataViewModel

    {
        //Metadata
        public Guid MetadataId { get; set; } 
        public Guid? FileId { get; set; }
        [DisplayName("Geo Location")]
        public string GeoLocation { get; set; }
        [DisplayName("Tags")]
        public string Tags { get; set; }
        [DisplayName("Captured by")]
        public string CaptureBy { get; set; }
        [DisplayName("Captured Date")]
        public DateTime? CaptureDate { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload file")]
        [Required(ErrorMessage = "Please select file")]
        public IFormFile Image { get; set; }

        //File
        //public Guid FileId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? AlbumId { get; set; }
        [DisplayName("Last Modified by")]
        public Guid? LastModifiedBy { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        [DisplayName("Date Modified")]
        public DateTime? DateModified { get; set; }
        [DisplayName("Content Type")]
        public string ContentType { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }

    }
}
