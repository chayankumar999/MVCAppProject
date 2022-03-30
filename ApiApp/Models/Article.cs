using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApp.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Category { get; set; }

        public string ArtBody { get; set; }


        [Required]
        public int Publish { get; set; }

        [DataType(DataType.DateTime)]
        public string CreationTime { get; set; }
    }
}
