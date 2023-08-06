using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.DBModels
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
