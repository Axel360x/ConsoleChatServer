using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Server
{
	public class FixedServer
	{
		private TcpListener tcpListener;
		private Thread listenThread;


		public static void Main(){



		}



		public FixedServer ()
		{
			this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3000);
			this.listenThread = new Thread (new ThreadStart (ListenForClients));
			this.listenThread.Start ();

		}


		private void ListenForClients(){
			this.tcpListener.Start ();

			while (true) {
				TcpClient client = this.tcpListener.AcceptTcpClient ();
				//Thread clientThread = new Thread (new ParameterizedThreadStart (HandleClientConn));
				//clientThread.Start (client);
				}
			}




	}
}

