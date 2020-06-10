using System;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Net;

namespace Server
{
    class Server
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            TcpClient clientSocket = default(TcpClient);
            int counter = 0;

            serverSocket.Start();
            Console.WriteLine("Server Started...");
            while (true)
            {
                counter += 1;
                clientSocket = serverSocket.AcceptTcpClient();

                byte[] bytesFrom = new byte[1024];
                string dataFromClient = null;

                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, 1024);
                dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                Console.Write(dataFromClient);

                try
                {
                    clientsList.Add(dataFromClient, clientSocket);
                }
                catch
                {
                    Console.WriteLine("Client already added");
                }
                HandleClient client = new HandleClient();
                client.StartClient(clientSocket, dataFromClient, clientsList);
            }
        }

        public static void Broadcast(string msg, string uName, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();

                byte[] broadcastBytes = Encoding.ASCII.GetBytes(msg);
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }
    }
}