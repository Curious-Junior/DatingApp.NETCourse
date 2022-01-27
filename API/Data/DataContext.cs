using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        // we just need this constructor
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // this for creating a new table in the database which will store values of type AppUser
        public DbSet<AppUser> Users { get; set; }
        

    }
}