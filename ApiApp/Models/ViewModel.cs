using System.Collections.Generic;

namespace ApiApp.Models
{
    public class ViewModel
    {
        public Article article { get; set; }
        public Comment comment { get; set; }
        public IEnumerable<Comment> CommentList { get; set; }

        public int Id { get; set; }
        public int ArtId { get; set; }

        public string UserName { get; set; }

        public string Comments { get; set; }

        public string CreationTime { get; set; }

    }
}
