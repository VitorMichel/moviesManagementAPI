using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieManagementApi.Models
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();
    }
}
