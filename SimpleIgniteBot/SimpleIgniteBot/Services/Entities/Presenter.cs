using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleIgniteBot.Services.Entities
{
   
    public class Presenter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}