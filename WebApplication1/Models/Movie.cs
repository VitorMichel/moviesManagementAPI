using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace MovieManagementApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public double Rating { get; set; }

        public List<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();
    }
}
