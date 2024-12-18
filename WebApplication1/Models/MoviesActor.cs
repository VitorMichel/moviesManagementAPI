using System.ComponentModel.DataAnnotations;

namespace MovieManagementApi.Models
{
    public class MoviesActor
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
