using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Task01
{
	static class Server
	{
		static readonly TcpListener Listener = new TcpListener(IPAddress.Any, 8081);

		static void Main(string[] args)
		{
			try
			{
				if (Listener != null)
				{
					Listener.Start();
					while (true)
					{
						// Принимаем новых клиентов
						Listener.AcceptTcpClient();
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
