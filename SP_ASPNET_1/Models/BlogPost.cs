using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class BlogPost
    {
        public BlogPost()
        {
            Likes = new List<Visitor>();
        }
        public int BlogPostID { get; set; }
        public string Title { get; set; }
        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }
        public DateTime DateTime { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Visitor> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}