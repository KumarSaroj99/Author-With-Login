using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorWithLogin.Data;
using AuthorWithLogin.Models;

namespace AuthorWithLogin.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index(Guid id)
        {
            TempData["user"] = id;
            Session["id"] = id;
            using(var session = NHibernateHelper.CreateSession())
            {
                //var author=session.Query<Author>().SingleOrDefault(u=>u.Id==id);
                var books=session.Query<Book>().Where(u=>u.Author.Id==id).ToList();

                //var books=session.Query<Book>().ToList();
                return View(books);

                //return Content(id);
            }
            
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book)
        {
            
            using(var session = NHibernateHelper.CreateSession())
            {
                var auth=(Guid)Session["id"];
                using(var transaction=session.BeginTransaction())
                {
                    book.Author = session.Get<Author>(auth);
                    session.Save(book);
                    transaction.Commit();
                    return RedirectToAction("Index",new {id=auth});
                }
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book=session.Query<Book>().SingleOrDefault(u=>u.Id==id);
                return View(book);
            }
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            var auth = (Guid)Session["id"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    book.Author = session.Get<Author>(auth);
                    session.Update(book);
                    transaction.Commit();
                    return RedirectToAction("Index", new { id = auth });
                }
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var user = session.Query<Book>().SingleOrDefault(u => u.Id == id);
                return View(user);
            }
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteUser(int id)
        {
            var auth = (Guid)Session["id"];
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var book= session.Get<Book>(id);

                    session.Delete(book);
                    transaction.Commit();
                    return RedirectToAction("Index", new { id = auth });
                }
            }
        }
    }
}