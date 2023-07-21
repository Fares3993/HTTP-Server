using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;

        public Server(int portNumber, string redirectionMatrixPath)
        {
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            this.LoadRedirectionRules(redirectionMatrixPath);
            //TODO: initialize this.serverSocket
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint end_point = new IPEndPoint(IPAddress.Any, portNumber);
            this.serverSocket.Bind(end_point);
        }

        public void StartServer()
        {
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(10000);
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket client_socket = this.serverSocket.Accept();
                Console.WriteLine("New client Accepted: {0}", client_socket.RemoteEndPoint);
                Thread thread = new Thread(new ParameterizedThreadStart(HandleConnection));
                thread.Start(client_socket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket client = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            client.ReceiveTimeout = 0;
            byte[] Received_request = new byte[2000];
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {
                try
                {
                    // TODO: Receive request
                    int receivedLen = client.Receive(Received_request);
                    string receiveRequest = Encoding.ASCII.GetString(Received_request,0,receivedLen);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLen == 0)                             
                    {
                        Console.WriteLine("Client {0} ended connection", client.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request request = new Request(receiveRequest);
                    // TODO: Call HandleRequest Method that returns the response
                    Response response = HandleRequest(request);
                    // TODO: Send Response back to client
                    string RB = response.ResponseString;
                    byte[] Response_back = Encoding.ASCII.GetBytes(RB);
                    client.Send(Response_back);
                }
                catch (Exception ex)
                {
                    // TODO: log exception using Logger class
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            client.Close();
        }

        Response HandleRequest(Request request)
        {
            
            string content;
            Response response;
            try
            {
                //TODO: check for bad request 
                if(!request.ParseRequest())
                {
                    content = LoadDefaultPage(Configuration.BadRequestDefaultPageName);
                    response = new Response(StatusCode.BadRequest, "text/html", content, null);
                    return response;
                }

                //TODO: map the relativeURI in request to get the physical path of the resource.

                //TODO: check for redirect
                if (Configuration.RedirectionRules.ContainsKey(request.relativeURI))
                {
                    string path = Configuration.RedirectionRules[request.relativeURI];
                    content = LoadDefaultPage(GetRedirectionPagePathIFExist(path));
                    response = new Response(StatusCode.Redirect, "text/html", content, path);
                    return response;
                }
                //TODO: check file exists
                else
                {
                    content = LoadDefaultPage(request.relativeURI);
                    if (content != "")
                        return response = new Response(StatusCode.OK, "text/html", content, null);
                    else
                    {
                        content = LoadDefaultPage("NotFound.html");
                        return response = new Response(StatusCode.NotFound, "text/html", content, null);
                    }
                }
               

                    //TODO: read the physical file

                    // Create OK response
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error.
                content = LoadDefaultPage(Configuration.InternalErrorDefaultPageName);
                return new Response(StatusCode.InternalServerError, "text/hrml", content, null);

            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            string path = Configuration.RootPath + "\\" + relativePath;
            if (File.Exists(path))
            {
                return relativePath;
            }
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            StreamReader read;
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            if(!File.Exists(filePath))
            {
                Logger.LogException(new Exception("default page" + defaultPageName + "doesn't exists"));
                return string.Empty;
            }
            // else read file and return its content
            read = new StreamReader(filePath);
            string file = read.ReadToEnd();
            read.Close();
            return file;
           
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                StreamReader read = new StreamReader(filePath);
                Configuration.RedirectionRules = new Dictionary<string, string>();
                while (! read.EndOfStream)
                {
                    string String = read.ReadLine();
                    string[] redirection_rules = String.Split(',');
                    Configuration.RedirectionRules.Add(redirection_rules[0], redirection_rules[1]);

                }
            }
            catch (Exception ex)
            {
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}
