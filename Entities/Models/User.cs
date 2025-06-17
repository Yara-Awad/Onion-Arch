using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class User :IdentityUser
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
    }
}
