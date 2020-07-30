using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SP_ASPNET_1.Models
{
    public class Comment
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public string body { set; get; }
        public DateTime dateTime { set; get; }
        [ForeignKey("BlogPost")]
        public int BlogPostID { set; get; }
        public virtual BlogPost BlogPost { set; get; }
    }
}