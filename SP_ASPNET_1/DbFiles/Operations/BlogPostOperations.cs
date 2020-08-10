using SP_ASPNET_1.DbFiles.UnitsOfWork;
using SP_ASPNET_1.Models;
using SP_ASPNET_1.ViewModels;
using SP_ASPNET_1.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SP_ASPNET_1.DbFiles.Contexts;
using System.Data.Entity;

namespace SP_ASPNET_1.DbFiles.Operations
{
    public class BlogPostOperations
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<BlogIndexViewModel> GetBlogIndexViewModelAsync()
        {
            //List<BlogPost> blogPosts = await GetDbContext().BlogPosts.OrderByDescending(d => d.DateTime).ToListAsync();
            List<BlogPost> blogPosts = (await _unitOfWork.BlogPostSchoolRepository.GetAsync(null, b => b.OrderByDescending(d => d.DateTime), "Author")).ToList();

            return new BlogIndexViewModel()
            {
                BlogPosts = blogPosts.GetRange(1, blogPosts.Count - 1),
                BlogPost = blogPosts.Take(1).FirstOrDefault()
            };
        }
        public IceCreamBlogContext GetDbContext()
        {
           return _unitOfWork.GetDbContext();
        }
        public BlogIndexViewModel GetBlogIndexViewModel()
        {
            List<BlogPost> blogPosts = GetDbContext().BlogPosts.Take(10).OrderByDescending(d=>d.DateTime).ToList();
            //_unitOfWork.BlogPostSchoolRepository.Get(null,/* b => b.OrderByDescending(d => d.DateTime)*/null, "Author").ToList();
            if (!blogPosts.Any())
            {
                return new BlogIndexViewModel();
            }
            return new BlogIndexViewModel()
            {
                BlogPosts = blogPosts.GetRange(1, blogPosts.Count - 1),
                BlogPost = blogPosts.Take(1).FirstOrDefault()
            };
        }

        public BlogPost GetBlogPostByIdD(int id)
        {
            return _unitOfWork.BlogPostSchoolRepository.GetByID(id);
        }

        public BlogSinglePostViewModel GetBlogPostByIdFull(int id)
        {
            return _unitOfWork.BlogPostSchoolRepository.Get(filter: x => x.BlogPostID == id,
                    orderBy: null,
                    includeProperties: "Author")
                .FirstOrDefault()
                .ToBlogSinglePostViewModel();
        }

        public BlogSinglePostViewModel GetLatestBlogPost()
        {

         /*   var list = _unitOfWork.BlogPostSchoolRepository.Get(filter: null,*/
         /* orderBy: x => x.OrderByDescending(entity => entity.DateTime)*/
         /*  null,includeProperties: "Author");*/

           // return list.Select(StaticHelpers.ToBlogSinglePostViewModel).FirstOrDefault();
            var list = GetDbContext().BlogPosts.ToList();

             var max=list.Max(a => a.DateTime);
            return list.Where(a=>a.DateTime==max).Select(StaticHelpers.ToBlogSinglePostViewModel).FirstOrDefault();
        }


        public BlogSinglePostViewModel GetRandomBlogPost()
        {
            //TODO: Investigate
            List<BlogPost> posts = _unitOfWork.BlogPostSchoolRepository.Get(null,
                    /*x => x.OrderByDescending(entity => entity.DateTime)*/null,
                    "Author")
                .ToList();

            if(posts.Count is 0)
            {
                return null;
            }

            Random rnd = new Random();
            
            var randomPost = posts[rnd.Next(posts.Count)];
            return randomPost.ToBlogSinglePostViewModel();
        }

        internal void Create(BlogPost blogPost)
        {
            try
            {
                this._unitOfWork.BlogPostSchoolRepository.Insert(blogPost);
                this._unitOfWork.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                BlogPost post = this.GetBlogPostByIdD(id);
                this._unitOfWork.BlogPostSchoolRepository.Remove(post);
                this._unitOfWork.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}