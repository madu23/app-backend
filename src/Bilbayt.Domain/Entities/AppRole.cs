using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bilbayt.Domain.Entities
{
    [CollectionName("AppRole")]
    public class AppRole : MongoIdentityRole
    {
        public string Description { get; set; }
    }
}
