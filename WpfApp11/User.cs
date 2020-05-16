using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp11
{
    public class User
    {
        public string name { get; set; }
        private string password;
        public int port;


        public void SetPassword(string pass)
        {
            password = pass;
        }

        public string GetPassword()
        {
            return password;
        }
    }
}
