// Use packet manager to install this !
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    // Inherit from higher level class : DbContext

    // A DbContext instance represents a session with the database and can be used to query and save instances of your entities.
    public class DataContext : DbContext
    {
        // Initializes a new instance of the Microsoft.EntityFrameworkCore.DbContext class using the specified options
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users {get; set;}
    }
}