using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class Visitor
    {
        public Visitor()
        {
            BlogPosts = new List<BlogPost>();
        }
        [Key]
        public int ID { get; set; }
        public string IPAdress { get; set; }
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

    }
}