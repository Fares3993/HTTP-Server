using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Call CreateRedirectionRulesFile() function to create the rules of redirection 
            CreateRedirectionRulesFile();
            //Start server
            // 1) Make server object on port 1000
            Console.WriteLine("initialize");
            //Server sserver = new Server(1000, "redirectionrulesfile.txt");
            Server sserver = new Server(1000, "redirectionrulesfile.txt");
            Console.WriteLine("Start");
            sserver.StartServer();
            // 2) Start Server
        }

        static void CreateRedirectionRulesFile()
        {
            // TODO: Create file named redirectionRules.txt
            const String redirectionrulesfilename = "redirectionrulesfile.txt";
            StreamWriter RRF = new StreamWriter(redirectionrulesfilename);
            // each line in the file specify a redirection rule
            // example: "aboutus.html,aboutus2.html"
            RRF.WriteLine("aboutus.html,aboutus2.html");
            // means that when making request to aboustus.html,, it redirects me to aboutus2
            RRF.Close();
        }
         
    }
}
