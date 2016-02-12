using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using VBlog.Models;

namespace VBlog.Content
{
    public class PostsController : Controller
    {

        private VBlogModel model = new VBlogModel();

        private const int PostPerPage = 4;
        public ActionResult Index(int? id)
        {
            int pageNumber = id ?? 0;
            IEnumerable<Post> posts = (from post in model.Posts
                                       where post.DateTime < DateTime.Now
                                       //order by post.DateTime
                                       orderby post.Id descending
                                       select post).Skip(pageNumber * PostPerPage).Take(PostPerPage + 1);
            ViewBag.IsPreviousLinkVisible = pageNumber > 0;
            ViewBag.IsNextLinkVisible = posts.Count() > PostPerPage;
            ViewBag.PageNumber = pageNumber;
            ViewBag.IsAddmin = IsAdmin;
            return View(posts.Take(PostPerPage));
        }

        public ActionResult Details(int id)
        {
            Post post = GetPost(id);
            ViewBag.IsAdmin = IsAdmin;
            return View(post);
        }

        [ValidateInput(false)]
        public ActionResult Comment(int id, string name, string email, string body)
        {
            Post post = GetPost(id);
            Comment comment = new Comment();
            comment.Id = model.Comments.Max(x => x.Id) + 1;
            comment.Post = post;
            comment.DateTime = DateTime.Now;
            comment.Name = name;
            comment.Email = email;
            comment.Body = body;
            model.Comments.Add(comment);
            model.SaveChanges();
            return RedirectToAction("Details", new { id = id });

        }

        //UPDATE POSTS TABLE
        [ValidateInput(false)]
        public ActionResult Update(int? id, string title, string body, DateTime dateTime, string tags)
        {
            if (!IsAdmin)
            {
                return RedirectToAction("Index");
            }
            Post post = GetPost(id);
            post.Title = title;
            post.Body = body;
            post.DateTime = dateTime;
            post.Tags.Clear();
            tags = tags ?? string.Empty;
            string[] tagNames = tags.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tagName in tagNames)
            {
                post.Tags.Add(GetTag(tagName));
            }
            if (!id.HasValue)
            {
                //bad implementation
                post.Id = model.Posts.Count() + 1;
                model.Posts.Add(post);
            }
            model.SaveChanges();
            return RedirectToAction("Details", new { id = post.Id });

        }

        public ActionResult Tags(string id)
        {
            Tag tag = GetTag(id);
            ViewBag.IsAdmin = IsAdmin;
            return View("Index", tag.Posts);
        }


        //NOT WORK
        public ActionResult Delete(int id)
        {
            if (IsAdmin)
            {
                Post post = GetPost(id);
                model.Posts.Remove(post);               
                model.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult DeleteComment(int id)
        {
            if (IsAdmin)
            {
                Comment comment = model.Comments.First(x => x.Id == id);
                model.Comments.Remove(comment);
                model.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        //EDIT
        public ActionResult Edit(int? id)
        {
            Post post = GetPost(id);
            StringBuilder tagList = new StringBuilder();
            foreach (Tag tag in post.Tags)
            {
                tagList.AppendFormat("{0}", tag.Name);
            }
            ViewBag.Tags = tagList.ToString();
            return View(post);
        }

        private Tag GetTag(string tagName)
        {
            //bad implementation
            return model.Tags.FirstOrDefault(x => x.Name == tagName) ?? new Tag() { Id = model.Tags.Max(x => x.Id) + 1, Name = tagName };
        }

        private Post GetPost(int? id)
        {
            return id.HasValue ? model.Posts.First(x => x.Id == id) : new Post() { Id = -1 };
        }

        public bool IsAdmin
        {
            get
            {
                return true;
                //return Session["IsAdmin"] != null && (bool)Session["IsAdmin"];
            }
        }
    }
}
