using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace _4._01_Hw.Data
{
    public class QAContext : DbContext
    {
        private readonly string _connectionString;

        public QAContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answer { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<QuestionTags> QuestionsTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(s =>
            //{
            //    Console.ForegroundColor = ConsoleColor.Cyan;
            //    Console.WriteLine(s);
            //    Console.ResetColor();
            //});
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //set up composite primary key
            modelBuilder.Entity<QuestionTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

        }

    }
}
