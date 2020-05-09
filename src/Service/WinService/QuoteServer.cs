using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace WService
{
    public class QuoteServer
    {
        private TcpListener listener;
        private int port;
        private string filename;
        private List<string> quoteList;
        private Random random;
        private Task listenerTask;

        public event EventHandler OnEvent;

        public QuoteServer(string fname, int port)
        {
            filename = fname;
            this.port = port;
        }

        public QuoteServer()
            :this("Process.txt",10023)
        {
            
        }

        protected void ReadQuotes()
        {
            try
            {
                quoteList = File.ReadAllLines(filename).ToList();
                if (quoteList.Count==0)
                {
                    throw new QuoteException("quote file is empty");
                }
                random = new Random();
            }
            catch (IOException ex)
            {
                
                throw new QuoteException("I/O error",ex);
            }
        }

        protected string GetRandomQuoteOfThDay()
        {
            int index = random.Next(0, quoteList.Count);
            return quoteList[index];
        }

        private string ReceiveCmd(Socket socket)
        {
            if (socket == null)
            {
                return null;
            }
            NetworkStream nsStream = new NetworkStream(socket);
            using (StreamReader srReader = new StreamReader(nsStream))
            {
                return srReader.ReadToEnd();
            }
        }
        protected void Listener()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                while (true)
                {
                    Socket cSocket = listener.AcceptSocket();
                    //var cmd = ReceiveCmd(cSocket);
                    OnEvent?.Invoke(cSocket.RemoteEndPoint.ToString(), null);
                    string msg = GetRandomQuoteOfThDay();
                    byte[] bufferBytes = new UnicodeEncoding().GetBytes(msg);
                    cSocket.Send(bufferBytes, 0, bufferBytes.Length, SocketFlags.None);
                    cSocket.Close();
                }
            }
            catch (SocketException ex)
            {

                throw new QuoteException("socket error", ex);
            }

        }

        public void StInfo()
        {
            StringInfo.GetNextTextElement("");
            var cul = CultureInfo.CurrentCulture;
        }


        #region Controlor

        public void Start()
        {
            ReadQuotes();
            listenerTask = Task.Factory.StartNew(Listener, TaskCreationOptions.LongRunning);
            while (listenerTask.Status == TaskStatus.Running)
            {
                Thread.Sleep(10);
            }
        }
        
        public void Stop()
        {
            listener?.Stop();
        }

        public void Suspend()
        {
            listener?.Stop();
        }

        public void Resume()
        {
            Start();
        }

        public void RefreshQuotes()
        {
            ReadQuotes();
        }
        #endregion


    }
}
