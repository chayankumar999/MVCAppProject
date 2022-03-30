using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCApp.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ArtId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Comments { get; set; }

        [DataType(DataType.DateTime)]
        public string CreationTime { get; set; }
    }
}
