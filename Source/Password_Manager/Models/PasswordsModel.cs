using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager
{
    class PasswordsModel
    {
        public List<PasswordEntity> Data { get; set; } = new List<PasswordEntity>();

        public class PasswordEntity
        {
            public PasswordEntity(string name)
            {
                Name = name;
            }

            public string Name { get; set; }
            public string PrimaryColor { get; set; }

        }
    }
}
