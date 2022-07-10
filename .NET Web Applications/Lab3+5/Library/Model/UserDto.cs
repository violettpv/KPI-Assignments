using System.Text.Json.Serialization;
using DAL.Model;

namespace Library.Model
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("books")]
        public IEnumerable<BookDto>? Books { get; set; }

        public UserDto()
        {
        }
        public UserDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
        }
        public UserDto(User user, IEnumerable<UserBook> books)
        {
            Id = user.Id;
            Name = user.Name;
            Books = books.Select(x => new BookDto(x.Book!));
        }
    }
}