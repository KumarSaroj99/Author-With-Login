using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorWithLogin.Data;
using AuthorWithLogin.Models;

namespace AuthorWithLogin.Controllers
{
    public class AuthorDetailController : Controller
    {
        // GET: AuthorDetail
        public ActionResult Index(Guid id)
        {
            Session["id"] = id;
            using (var session = NHibernateHelper.CreateSession())
            {
                var authorDetail=session.Query<Author>().SingleOrDefault(u=>u.Id==id);
                return View(authorDetail);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {

                using (var transaction = session.BeginTransaction())
                {
                    var auth = (Guid)Session["id"];

                    author.Books=new List<Book>();
                    var book=new Book();
                    book.Author=author;
                    author.Books.Add(book);

                    session.Save(author);
                    transaction.Commit();
                    return RedirectToAction("Index", new { id = auth });

                }

            }
        }
        
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Query<Author>().SingleOrDefault(u => u.Id==id);
                return View(author);
            }
        }
        
        [HttpPost]
        public ActionResult Edit(Author author)
        {
            using(var session= NHibernateHelper.CreateSession())
            {

                using(var transaction = session.BeginTransaction())
                {
                    var auth = (Guid)Session["id"];
                    var books=session.Query<Book>().Where(u=>u.Author.Id==author.Id).ToList();

                    author.Books = books;

                    session.Update(author);
                    transaction.Commit();
                    return RedirectToAction("Index", new { id = auth });
                }
            }
        }
        [HttpGet]
        public ActionResult Delete(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                
                using(var transaction= session.BeginTransaction())
                {
                    var author = session.Query<Author>().SingleOrDefault(u => u.Id==id);
                    session.Delete(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }

            }
        }
    }
}