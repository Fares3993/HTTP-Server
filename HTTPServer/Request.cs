using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {
        string[] rLines;
        RequestMethod method;
        public string relativeURI;
        //Dictionary<string, string> headerLines;
        Dictionary<string, string> headerLines = new Dictionary<string, string> { { "", "" } };
        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //TODO: parse the receivedRequest using the \r\n delimeter  
            // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
            // Parse Request line
            // Validate blank line exists
            // Load header lines into HeaderLines dictionary

            if (!ParseRequestLine() || !ValidateBlankLine() || !LoadHeaderLines())
                return false;
            else
                return true;


        }

        private bool ParseRequestLine()
        {
            string[] stringSeparators = new string[] { "\r\n" };

            contentLines = requestString.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
            if (contentLines.Length <= 3)
            {
                return false;
            }
            rLines = contentLines[0].Split(' ');

            if (rLines.Length != 3)
                return false;

            if (rLines[0] == "GET")
                method = RequestMethod.GET;
            else if (rLines[0] == "POST")
                method = RequestMethod.POST;
            else
                method = RequestMethod.HEAD;

            if (!ValidateIsURI(rLines[1]))
                return false;

            relativeURI = rLines[1].Remove(0, 1);

            if (rLines[2] == "HTTP/1.1")
                httpVersion = HTTPVersion.HTTP11;
            else if (rLines[2] == "HTTP/1.0")
                httpVersion = HTTPVersion.HTTP10;
            else
                httpVersion = HTTPVersion.HTTP09;

            return true;
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            string[] stringSeparators = new string[] { ": " };
            headerLines = new Dictionary<string, string>();
            for (int i = 1; i < contentLines.Length; i++)
            {
                string[] headers = contentLines[i].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

                headerLines.Add(headers[0], headers[1]);
            }

            if (headerLines.Count < 1)
                return false;

            return true;
        }

        private bool ValidateBlankLine()
        {
            if (requestString.EndsWith("\r\n\r\n"))
                return true;
            else
                return false;
        }

    }
}