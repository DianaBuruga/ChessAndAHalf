using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        private TcpListener server;
        private bool isRunning;
        private TcpClient client1;
        private TcpClient client2;
        private NetworkStream stream1;
        private NetworkStream stream2;

        public Server()
        {
            server = new TcpListener(IPAddress.Any, 3000);
            isRunning = true;
            client1 = null;
            client2 = null;
            stream1 = null;
            stream2 = null;
        }

        public void Start()
        {
            server.Start();
            Console.WriteLine("Server started. Waiting for clients...");

            client1 = server.AcceptTcpClient();
            stream1 = client1.GetStream();
            Console.WriteLine("Client 1 connected.");

            client2 = server.AcceptTcpClient();
            stream2 = client2.GetStream();
            Console.WriteLine("Client 2 connected.");

            // Start separate threads to handle client interactions
            Thread thread1 = new Thread(HandleClient1);
            Thread thread2 = new Thread(HandleClient2);
            thread1.Start();
            thread2.Start();
        }

        private void HandleClient1()
        {
            StreamReader reader = new StreamReader(stream1);
            StreamWriter writer = new StreamWriter(stream2);
            writer.AutoFlush = true;
            writer.WriteLine("BLACK");
            writer.Flush();

            while (isRunning)
            {
                string message = reader.ReadLine();
                if (message == null) break;

                Console.WriteLine("Client 1: " + message);

                // Forward message to client 2
                writer.WriteLine(message);
                writer.Flush();
            }
            writer.WriteLine("#Gata");
            writer.Flush();
            Console.WriteLine("Client 1 disconnected.");
            Cleanup();
        }

        private void HandleClient2()
        {
            StreamReader reader = new StreamReader(stream2);
            StreamWriter writer = new StreamWriter(stream1);
            writer.AutoFlush = true;
            writer.WriteLine("WHITE");
            writer.Flush();
            while (isRunning)
            {
                string message = reader.ReadLine();
                if (message == null) break;

                Console.WriteLine("Client 2: " + message);

                // Forward message to client 1
                writer.WriteLine(message);
                writer.Flush();
            }
            writer.WriteLine("#Gata");
            writer.Flush();
            Console.WriteLine("Client 2 disconnected.");
            Cleanup();
        }

        private void Cleanup()
        {
            stream1.Close();
            stream2.Close();
            client1.Close();
            client2.Close();
            Console.WriteLine("Finished Connection!");
            server.Stop();
            isRunning = false;
        }
    }
}

