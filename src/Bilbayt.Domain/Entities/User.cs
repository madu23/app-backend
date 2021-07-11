using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bilbayt.Domain.Entities
{
        [CollectionName("User")]
        public class AppUser : MongoIdentityUser
        {
            public string FullName { get; set; }
        }
}
