namespace WebApplication15.Models
{
    public class AdminPageViewModel
    {   
        public int Id { get; set; }
        public string Role { get; set; } = "Admin";
        public string Email {get;set;}
        public string PassWord { get; set; }
    }
}
