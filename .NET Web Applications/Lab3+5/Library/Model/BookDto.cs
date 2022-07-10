using System.Text.Json.Serialization;
using DAL.Model;

namespace Library.Model
{
    public class BookDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("genre")]
        public string? Genre { get; set; }

        [JsonPropertyName("amount_in_stock")]
        public int AmountInStock { get; set; }

        public BookDto()
        {
        }
        public BookDto(Book book)
        {
            Id = book.Id;
            Name = book.Name;
            Author = book.Author!.Name;
            Genre = book.Genre!.Name;
            AmountInStock = book.Amount;
        }
    }
}