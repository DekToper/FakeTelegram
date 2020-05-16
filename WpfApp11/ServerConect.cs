using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp11
{
    public class ServerConect
    {
        public IPAddress groupAddress = IPAddress.Parse("224.5.5.5");
        public UdpClient udpClient = new UdpClient();
        public int sendPort = 8999;
        public int receivePort;

        public ServerConect(int receive = 8998)
        {
            receivePort = receive;
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));
            udpClient.JoinMulticastGroup(groupAddress);
        }

        public bool SendUserDetails(string msg,char tag)
        {
            IPEndPoint sender = null;
            string text = msg;
            byte[] userData = Encoding.Default.GetBytes(tag + text);
            udpClient.Send(userData, userData.Length, new IPEndPoint(groupAddress, sendPort));
            byte[] d = udpClient.Receive(ref sender);
            string data = Encoding.Default.GetString(d);
            if(data == "true")
            {
                return true;
            }          
            return false;
        }
        public void Close()
        {
            udpClient.Close();
        }


    }
}
