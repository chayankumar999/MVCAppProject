using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApp.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ArtId { get; set; }

        public string Username { get; set; }

        [Required]
        public string Comments { get; set; }

        [DataType(DataType.DateTime)]
        public string CreationTime { get; set; }
    }
}
