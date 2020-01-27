using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Task01
{
	static class HTTPServer
	{
		static TcpListener Listener = null;

		static readonly ReaderWriterLock rwLock = new ReaderWriterLock();

		static int count = 0; // Общий ресурс, защищенный ReaderWriterLock.

		static void ClientThread(Object StateInfo)
		{
			// Создаем новый экземпляр класса Client и передаем ему приведенный к классу TcpClient объект StateInfo
			new Client((TcpClient)StateInfo);
		}

		static void Main(string[] args)
		{
			// Определим нужное максимальное количество потоков
			// Пусть будет по 4 на каждый процессор
			int MaxThreadsCount = Environment.ProcessorCount * 4;
			// Установим максимальное количество рабочих потоков
			ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);
			// Установим минимальное количество рабочих потоков
			ThreadPool.SetMinThreads(2, 2);

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
						// Принимаем новых клиентов. После того, как клиент был принят, он передается в новый поток (ClientThread)
						// с использованием пула потоков.
						ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());

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

		public static string GetCount()
		{
			string retMsg = "";
			try
			{
				rwLock.AcquireReaderLock(10);
				try
				{
					retMsg = "Read. Value: " + count.ToString();
				}
				finally
				{
					rwLock.ReleaseReaderLock();
				}
			}
			catch (ApplicationException)
			{
				retMsg = "Err: Reader time-outs";
			}
			return retMsg;
		}

		public static string AddToCount(int value)
		{
			string retMsg = "";
			try
			{
				rwLock.AcquireWriterLock(100);
				try
				{
					// It's safe for this thread to access from the shared resource.
					count = value;
					retMsg = "Write. Value: " + count.ToString();
				}
				finally
				{
					rwLock.ReleaseWriterLock();
				}
			}
			catch (ApplicationException)
			{
				retMsg = "Err: Writer time-outs";
			}
			return retMsg;
		}
	}
}
