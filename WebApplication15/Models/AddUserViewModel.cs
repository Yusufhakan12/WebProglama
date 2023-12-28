namespace WebApplication15.Models
{
    #nullable disable
    public class AddUserViewModel
    {
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public string Role { get; set; } = "User"; 
        public string ConfirmPassWord { get; set; }
    }
}
