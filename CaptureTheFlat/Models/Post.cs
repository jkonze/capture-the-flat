using System;
using System.ComponentModel.DataAnnotations;

namespace CaptureTheFlat.Models
{
    public class Post
    {
        [Required] public string PostId { get; set; }

        [Required] public string Title { get; set; }

        [Required] public DateTime PublishDate { get; set; }

        [Required] public string Link { get; set; }

        public bool HasPhoto { get; set; }
        
        public DateTime FoundAt { get; set; }
    }
}