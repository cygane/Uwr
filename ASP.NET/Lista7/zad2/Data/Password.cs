using System.ComponentModel.DataAnnotations;

namespace zad2.Data
{
    public class Password
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime PasswordSetDate { get; set; }
        public User User { get; set; }
    }
}