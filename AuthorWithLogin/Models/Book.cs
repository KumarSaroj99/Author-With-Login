using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorWithLogin.Models
{
    public class Book
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual int Price { get; set; }
        public virtual Author Author { get; set; }
    }
}