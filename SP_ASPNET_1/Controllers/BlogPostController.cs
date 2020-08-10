using SP_ASPNET_1.DbFiles.Operations;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using System.Web.Mvc;
using System.Web.Routing;
using SP_ASPNET_1.BusinessLogic;
using System;
using System.Web.Caching;
using System.Linq;
using System.Threading.Tasks;
using WebGrease.Css.Extensions;
using SP_ASPNET_1.DbFiles.Contexts;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Collections.Generic;

namespace SP_ASPNET_1.Controllers
{
    [RoutePrefix("Blog")]
    public class BlogPostController : Controller
    {
        private readonly BlogPostOperations _blogPostOperations;
        private readonly IceCreamBlogContext _db;

        public BlogPostController()
        {
            _blogPostOperations = new BlogPostOperations();
            _db = _blogPostOperations.GetDbContext();
        }

        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            Session["numOfPosts"] = 10;
            //return this.View();
            BlogIndexViewModel result = this._blogPostOperations.GetBlogIndexViewModel();
            //result.BlogPosts = result.BlogPosts.Take(numOfPosts);
            ViewBag.Title = "Blog";
            return this.View(result);
        }


        [Route("Detail/{id:int?}")]
        [HttpGet]
        public ActionResult SinglePost(int? id)
        {
            ViewBag.Title = "single post";

            
            BlogSinglePostViewModel modelView;

            if (id == null)
            {
                modelView = this._blogPostOperations.GetLatestBlogPost();
            }
            else
            {
                modelView = this._blogPostOperations.GetBlogPostByIdFull((int)id);
            }

            return View(modelView);
        }

        [Route("Detail/Random")]
        [HttpGet]
        public ActionResult RandomPost()
        {
            ViewBag.Title = "Random post";

            var viewModel = this._blogPostOperations.GetRandomBlogPost();

            return View(viewModel);
        }

        [Route("LatestPost")]
        [HttpGet]
        public ActionResult LatestPost()
        {
            var t1 = DateTime.Now;
            var viewModel = this._blogPostOperations.GetLatestBlogPost();
            var t2 = DateTime.Now;
            var d = (t2 - t1).TotalSeconds;
            return this.PartialView("~/Views/BlogPost/_BlogPostRecentPartialView.cshtml", viewModel);

        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(BlogPost blogPost)
        {
            try
            {
                this._blogPostOperations.Create(blogPost);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Route("Edit/{id:int?}")]
        [HttpGet]
        public ActionResult EditBlogPost(int id)
        {
            BlogPost blogPost;

            blogPost = this._blogPostOperations.GetBlogPostByIdD((int)id);

            return View(blogPost);
        }

        [Route("Edit/{id:int}")]
        [HttpPost]
        public ActionResult Edit(/*int id, FormCollection collection*/BlogPost blogPost,HttpPostedFileBase imgFile)
        {
 
                // TODO: Add update logic here
                // TODO: Return to detail
                if (ModelState.IsValid)
                {
                    if (imgFile != null)
                    {
                        string[] arr = imgFile.FileName.Split('.');
                        string newImgName = Guid.NewGuid().ToString() + "." + arr[arr.Length - 1];
                        if (blogPost.ImageUrl != null)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/images/posts/")+ blogPost.ImageUrl);
                        }
                        imgFile.SaveAs(Server.MapPath("~/Content/images/posts/") + newImgName);
                        blogPost.ImageUrl = newImgName;
                    }
                    _db.Entry(blogPost).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    return RedirectToAction("SinglePost",new {id=blogPost.BlogPostID });
                }
          

                return View();
   
        }

        [Route("Delete/{id:int}")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                this._blogPostOperations.Delete(id);

                //CHECK: should return to blogs
                return RedirectToAction("Index");
            }
            catch
            {
                return this.HttpNotFound();
            }
        }

        public async Task<ActionResult> AddLikes(int id,string requestIP)
        {
         
            var post = _blogPostOperations.GetBlogPostByIdD(id);
            var recentVisitor = _db.Visitors.SingleOrDefault(v => v.IPAdress == requestIP);
            if (post != null)
            {
                if (recentVisitor != null)
                {
                    var isLike = post.Likes.Contains(recentVisitor);
                    if (!isLike)
                    {
                        post.Likes.Add(recentVisitor);
                    }
                    else
                    {
                        post.Likes.Remove(recentVisitor);
                    }
                }
                else
                {
                    post.Likes.Add(new Visitor() { IPAdress = requestIP });
                }
                await _db.SaveChangesAsync();
            }
         
            ViewBag.likesCount = post.Likes.Count();
            return PartialView();
        }

        public ActionResult LoadMore()
        {
            int x;
            if (Session["numOfPosts"] == null)
            {
                Session["numOfPosts"] = 10;
                x = (int)Session["numOfPosts"];
            }
            else
            {
                 x = (int)Session["numOfPosts"];
                x += 10;
                Session["numOfPosts"] = x;
            }
           // var result2 = this._blogPostOperations.GetBlogIndexViewModel().BlogPosts.ToList();
            //var result = this._db.BlogPosts.OrderByDescending(p=>p.DateTime).ToList();
            if (! (x-10 >= this._db.BlogPosts.Count()))
            {
                var result = this._db.BlogPosts.OrderByDescending(p => p.DateTime).Skip(x-10).Take(10).ToList();
               // result= result.Take(x).Except(result.Take(x - 10)).ToList();
                return PartialView(result);
            }
            return null;
        }
        public ActionResult GetCommentsById(int id)
        {
            var blogPost = _blogPostOperations.GetBlogPostByIdD(id);
            List<Comment> comments = blogPost.Comments.OrderByDescending(a=>a.dateTime).ToList();
            return PartialView(comments);
        }
        public ActionResult AddNewComment(int id,Comment comment)
        {
            var blogPost = _blogPostOperations.GetBlogPostByIdD(id);
            if (ModelState.IsValid)
            {
                if (comment != null)
                {
                    comment.dateTime = DateTime.Now;
                    blogPost.Comments.Add(comment);
                    this._db.SaveChanges();
                }
            }
            return PartialView(comment);
        }
        public ActionResult RemoveComment(int id,int blogId)
        {
             var blogPost = _blogPostOperations.GetBlogPostByIdD(blogId);
             var comment=blogPost.Comments.SingleOrDefault(c => c.Id == id);
            _db.Comments.Remove(comment);
            this._db.SaveChanges();

            return PartialView(blogPost);
        }
    }
}
