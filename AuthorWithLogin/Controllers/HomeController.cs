using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using AuthorWithLogin.Data;
using AuthorWithLogin.Models;
using Magnum.Cryptography;

namespace AuthorWithLogin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(BookAuthorVM vm)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var author = session.Query<Author>().FirstOrDefault(u => u.Name == vm.UserName);
                    if (author != null)
                    {
                        string hashedPassword = HashingService.HashPassword(vm.Password);

                        if (author.Password == hashedPassword)
                        {
                            FormsAuthentication.SetAuthCookie(vm.UserName, true);

                            return RedirectToAction("Index", "AuthorDetail",new {id=author.Id});
                        }
                    }
                    ModelState.AddModelError("", "Usename/Password does not match");
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    string hashedPassword = HashingService.HashPassword(author.Password);
                    Author newAuthor=new Author()
                    {
                        Name = author.Name,
                        Password= hashedPassword,
                        Email = author.Email,
                        Age = author.Age,


                    };
                    newAuthor.Books = new List<Book>();
                    var book = new Book();
                    book.Author = newAuthor;
                    author.Books.Add(book);
                    
                    //newUser.Role = new Role();
                    //newUser.Role.User = newUser;


                    session.Save(newAuthor);
                    transaction.Commit();
                    return RedirectToAction("Login");
                }
            }

        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }

}
