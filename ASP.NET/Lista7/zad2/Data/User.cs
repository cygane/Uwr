using System.ComponentModel.DataAnnotations;

namespace zad2.Data
{
    public class User{
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}