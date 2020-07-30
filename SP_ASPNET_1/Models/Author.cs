using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class Author
    {
        public Author()
        {
            blogPosts = new List<BlogPost>();
        }
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual ICollection<BlogPost> blogPosts { get; set; }
        public override string ToString()
        {
            return $"{this.Name} {this.Surname}";
        }
        public int AverageLikes()
        {
            int numberOfLikes=0;
            foreach(var blog in blogPosts)
            {
                numberOfLikes += blog.Likes.Count();
            }
            return numberOfLikes/blogPosts.Count;
        }
    }
}