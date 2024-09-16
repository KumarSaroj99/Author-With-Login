using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorWithLogin.Models;
using FluentNHibernate.Mapping;

namespace AuthorWithLogin.Mappings
{
    public class BookMap:ClassMap<Book>
    {
        public BookMap()
        {
            Table("Books");
            Id(b => b.Id).GeneratedBy.Identity();
            Map(b => b.Title);
            Map(b => b.Price);
            References(x => x.Author).Column("AuthorId").Not.Nullable();
        }
    }
}