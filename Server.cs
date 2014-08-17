using System;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;

namespace ServerSide
{
	class Server : ServiceBase
	{
		private TcpListener tcpListener;
		private Thread listenThread;
		private string text;
		private ArrayList connections;
		private Thread clientThread;

		public static void Main ()
		{
			Server serv = new Server();
		}

		public Server ()
		{
			string[] ipconf = File.ReadAllLines("IPsetting.conf");
			IPAddress ipa;
			IPAddress.TryParse (ipconf [0], out ipa);

			this.tcpListener = new TcpListener (new IPEndPoint (ipa, Convert.ToInt32(ipconf[1])));
			
			this.listenThread = new Thread (new ThreadStart (ListenForClients));
			this.listenThread.Start ();


		

			

		
		}




		private void ListenForClients ()
		{
			this.tcpListener.Start ();
			connections = new ArrayList (30);

			while (true) {
				TcpClient client = this.tcpListener.AcceptTcpClient ();

				connections.Add (client);

				clientThread = new Thread (new ParameterizedThreadStart (HandleClientConn));
				clientThread.Start (client);


			}




		}


		private void HandleClientConn (object client)
		{
			TcpClient tcpClient = (TcpClient)client;
			Console.WriteLine ("Accepted connection from " + tcpClient.Client.RemoteEndPoint);
			NetworkStream clientStream = tcpClient.GetStream ();
			ASCIIEncoding encoder = new ASCIIEncoding ();
			byte[] message = new byte[4096];
			clientStream.Read(message, 0, message.Length);
			clientStream.Flush();
			string clientInfo = encoder.GetString(message).TrimEnd(new char[]{ (char)0 });
			string[] allInfo = new string[4];
			allInfo = clientInfo.Split(' ');
			string texts = allInfo[0] + " is now online(" + allInfo[1] + ")";
			foreach (TcpClient slot in connections)
			{
				NetworkStream clientStreams = slot.GetStream();
				encoder = new ASCIIEncoding();
				clientStreams.Write(encoder.GetBytes(texts), 0, encoder.GetBytes(texts).Length);
				clientStreams.Flush();
			}

			int bytesRead;
			
			while (true) {

				bytesRead = 0;

				try {

					message = new byte[4096];

					bytesRead = clientStream.Read (message, 0, message.Length);

					clientStream.Flush ();
					text = encoder.GetString (message).TrimEnd (new char[]{ (char)0 });
					foreach (TcpClient slot in connections) {
					
						NetworkStream clientsStream = slot.GetStream ();
						encoder = new ASCIIEncoding ();

						clientsStream.Write (encoder.GetBytes (text), 0, encoder.GetBytes (text).Length);

						clientsStream.Flush ();

					}
			} catch {
					break;
				}
				if (bytesRead == 0) {

					break;

				}

			}

			Console.WriteLine ("Disconnected from " + tcpClient.Client.RemoteEndPoint);
			connections.Remove (tcpClient);
			tcpClient.Close ();

		}

	}
}

