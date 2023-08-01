namespace FirstProject___Test.Models
{
    public class User
    {
        public Guid userToken { get; set; }
        public int userId { get; set; }

        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
