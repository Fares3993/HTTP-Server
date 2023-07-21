using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            // C: \Users\banot\Desktop\New folder(3)\Template[2021 - 2022]\HTTPServer\bin\Debug\log.txt
            //FileStream file = new FileStream(@"C:Template[2021 - 2022]\HTTPServer\bin\Debug\log.txt", FileMode.OpenOrCreate);

            // Datetime: 
            //message:
            // for each exception write its details associated with datetime
            StreamWriter sr = new StreamWriter("log.txt");
            sr.WriteLine("Datetime: " + DateTime.Now.ToString());
            sr.WriteLine("messge:" + ex.Message);
            sr.Close();

        }
    }
}