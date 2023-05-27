using ChessAndAHalf.Data.Model;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace ChessAndAHalf
{
    internal class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        public bool ascult;
        MainWindow game;
        public Thread t;
        string serverIP = "127.0.0.1";
        int serverPort = 3000;

        public Client(MainWindow game)
        {
            client = new TcpClient();
            this.game = game;
            StreamWriter wr = new StreamWriter(Console.OpenStandardOutput());
            wr.AutoFlush = true;
            Console.SetOut(wr);

            Connect(serverIP, serverPort);
        }

        public void Connect(string serverIP, int serverPort)
        {
            client.Connect(serverIP, serverPort);
            ascult = true;
            t = new Thread(new ThreadStart(ReceiveMessages));
            t.Start();
            stream = client.GetStream();

            Console.WriteLine("Connected to the server. Start typing messages:");
        }
        public void SendMessages(string message)
        {
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            if (message != null)
            {
                writer.WriteLine(message);
                writer.Flush();
            }
        }

        public void ReceiveMessages()
        {
            reader = new StreamReader(stream);
            string message;
            while (ascult)
            {
                try
                {
                    message = reader.ReadLine();
                    if (message != null)
                    {
                        ExecuteCommand(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection lost. Server disconected! " + ex.Message);
                }
            }
        }
        public void ExecuteCommand(string message)
        {
            switch (message)
            {
                case "WHITE":
                    game.MyColor = PlayerColor.WHITE;
                    break;
                case "BLACK":
                    game.MyColor = PlayerColor.BLACK;
                    break;
                case "#Gata":
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        game.PrintMessage("Connection Lost! You win");
                    }));
                    ascult = false;
                    t.Abort();
                    break;
                default:
                    var completedMoves = message.Split('/');
                    foreach (var move in completedMoves)
                    {
                        try
                        {
                            var moves = move.Split('#');
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                game.Start(int.Parse(moves[0]), int.Parse(moves[1]), bool.Parse(moves[2]));
                            }));
                        }
                        catch
                        {

                        }
                    }
                    break;
            }
        }
        public void Disconnect()
        {
            writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            writer.WriteLine("#Gata");
            ascult = false;
            t.Abort();
        }
    }
}
