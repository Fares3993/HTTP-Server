using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{

    public enum StatusCode
    {
        OK = 200,
        InternalServerError = 500,
        NotFound = 404,
        BadRequest = 400,
        Redirect = 301
    }

    class Response
    {
        string responseString;
        public string ResponseString
        {
            get
            {
                return responseString;
            }
        }
        StatusCode code;
        List<string> headerLines = new List<string>();
        public Response(StatusCode code, string contentType, string content, string redirectoinPath)
        {
            //throw new NotImplementedException();
            // TODO: Add headlines (Content-Type, Content-Length,Date, [location if there is redirection])

            headerLines.Add(contentType);
            headerLines.Add(content.Length.ToString());
            headerLines.Add(DateTime.Now.ToString());

            string status = GetStatusLine(code);
            // TODO: Create the request string
            if (code == StatusCode.Redirect)
            {
                headerLines.Add(redirectoinPath);
                responseString = status + "\r\n" + "Conent-type" + headerLines[0] + "\r\n" + "Content-Length" + headerLines[1] + "\r\n" + "Date" + headerLines[2] + "\n\r" + "Location" + headerLines[3] + "\r\n" + "\r\n" + content;
            }
            else
            {
                responseString = status + "\r\n" + "Conent-type" + headerLines[0] + "\r\n" + "Content-Length" + headerLines[1] + "\r\n" + "Date" + headerLines[2] + "\r\n" + "\r\n" + content;
            }






        }

        private string GetStatusLine(StatusCode code)
        {
            // TODO: Create the response status line and return it
            string statusLine = string.Empty;

            switch (code)
            {
                case StatusCode.OK:
                    statusLine = "HTTP/1.1" + " " + code + " " + "OK";
                    break;
                case StatusCode.Redirect:
                    statusLine = "HTTP/1.1" + " " + code + " " + "Redirect";
                    break;
                case StatusCode.BadRequest:
                    statusLine = "HTTP/1.1" + " " + code + " " + "Bad Request";

                    break;
                case StatusCode.InternalServerError:
                    statusLine = "HTTP/1.1" + " " + code + " " + "Internal Server Error";

                    break;
                case StatusCode.NotFound:
                    statusLine = "HTTP/1.1" + " " + code + " " + "Not Found ";
                    break;
                    /*default:
                        Console.WriteLine("not found the code");

                        break;*/

            }

            return statusLine;
        }
    }
}