using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class Book : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        [ForeignKey("Genre")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        public int Amount { get; set; }
    }
}