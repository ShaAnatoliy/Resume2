using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Task01
{
	static class HTTPServer
	{
		static TcpListener Listener = null;

		static void Main(string[] args)
		{
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");

			try
			{
				Listener = new TcpListener(localAddr, 8081);

				if (Listener != null)
				{
					Listener.Start();
					Console.WriteLine($"http://{localAddr.ToString()}:8081");
					while (true)
					{
						//  Принимаем новых клиентов и передаем их на обработку новому экземпляру класса Client
						new Client(Listener.AcceptTcpClient());

					}
				}

			}
			finally
			{
				if (Listener != null)
					Listener.Stop();
				else
					Console.WriteLine("Ошибка запуска (Listener)..");
			}
		}
	}
}
