using Microsoft.EntityFrameworkCore;
using MovieManagementApi.Models;

namespace MovieManagementApi.Data
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MoviesActor> MoviesActor { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActor>()
            .HasKey(ma => ma.Id);

            modelBuilder.Entity<MoviesActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MoviesActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MoviesActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MoviesActors)
                .HasForeignKey(ma => ma.ActorId);

            // Seed data
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Inception", Rating = 8.8 },
                new Movie { Id = 2, Title = "The Matrix", Rating = 8.7 }
            );

            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, Name = "Leonardo DiCaprio" },
                new Actor { Id = 2, Name = "Keanu Reeves" }
            );

            modelBuilder.Entity<MoviesActor>().HasData(
                new MoviesActor { Id = 1, MovieId = 1, ActorId = 1 },
                new MoviesActor { Id = 2, MovieId = 2, ActorId = 2 }
            );
        }
    }
}