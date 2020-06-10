using System;
using System.Threading;
using System.Net.Sockets;
using System.Collections;

namespace Server
{
    public class HandleClient
    {
        TcpClient clientSocket;
        string clNo;
        Hashtable clientsList;

        public void StartClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(Listen);
            ctThread.Start();
        }

        private void Listen()
        {
            byte[] bytesFrom = new byte[1024];
            string dataFromClient;
            bool connected = true;

            while (connected)
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, 1024);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    string[] sData = dataFromClient.Split('-');
                    dataFromClient = sData[0];
                    Console.WriteLine(dataFromClient);

                    Server.Broadcast(dataFromClient, clNo, true);
                    networkStream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    connected = false;
                }
            }
            Server.clientsList.Remove(clNo);
        }
    }
}
