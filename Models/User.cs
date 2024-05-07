using System.Text.Json.Serialization;

namespace Todo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Slug { get; set; }
        [JsonIgnore] 
        public string PasswordHash { get; set; }
        public IList<TodoModel> Todos { get; set; }
    }
}