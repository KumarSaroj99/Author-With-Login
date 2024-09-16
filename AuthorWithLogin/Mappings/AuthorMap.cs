using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorWithLogin.Models;
using FluentNHibernate.Mapping;

namespace AuthorWithLogin.Mappings
{
    public class AuthorMap:ClassMap<Author>
    {
        public AuthorMap()
        {
            Table("Authors");
            Id(a => a.Id).GeneratedBy.GuidComb();
            Map(a => a.Name);
            Map(a=>a.Email);
            Map(a=>a.Age);
            Map(a=>a.Password);
            HasMany(x => x.Books).Cascade.All().Inverse();
        }
    }
}