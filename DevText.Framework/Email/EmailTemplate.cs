using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DevText.Framework.Email
{
    public class EmailTemplate
    {
        
        private FileInfo _template;     
        private Dictionary<string,string> _variables = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        string pattern = "{(.|\n)+?}";
        private MailMessage _message;

        //constants for template names
        public const string AccountActivation = "Activated";
        public const string CommentFlagged = "CommentFlagged";
        public const string ForgotUsername = "ForgotUsername";
        public const string PasswordReset = "PasswordReset";
        public const string PostFlagged = "PostFlagged";
        public const string PostPublished = "PostPublished";
        public const string Registered = "Register";

        public EmailTemplate(FileInfo template)
        {                       
            _template = template;
            Parse();
        }

        private void AddVariable(string key, string value)
        {
            if (_variables.ContainsKey(key))
                _variables[key] = value;
            else
                _variables.Add(key, value);
        }        

        public void AssignVariable(string key, string value)
        {
            var realKey = "{" + key + "}";
            if (_variables.ContainsKey(realKey))
                _variables[realKey] = value;
        }
       
        private void Parse() {
            _message = new MailMessage();
            string[] lines = System.IO.File.ReadAllLines(_template.FullName);            
            StringBuilder sb = new StringBuilder();                    
            foreach (var line in lines)
            {
                if (line.ToUpper().StartsWith("@"))
                {
                    string[] tokens = line.Split('=');
                    if (tokens.Length == 2)
                    {
                        AddVariable(tokens[0].Trim(), tokens[1]);
                    }
                }
                else
                {
                    var matches = Regex.Matches(line, pattern);                    
                    foreach (Match match in matches)
                    {                        
                        AddVariable( match.Groups[0].Value, String.Empty);
                    }
                    sb.AppendLine(line);
                }
            }

            _message.Body = sb.ToString();                        
        }

        public MailMessage Execute()
        {
            ReplaceVariables();
            return _message;
        }

        private void ReplaceVariables()
        {
            if (_variables.ContainsKey("@Subject"))
                _message.Subject = _variables["@Subject"];

            var keys = _variables.Where(x => x.Key.StartsWith("{") && x.Key.EndsWith("}")).Select( x => x.Key );
            foreach (var key in keys)
            {
                _message.Body = _message.Body.Replace(key, _variables[key]);
                _message.Subject = _message.Subject.Replace(key, _variables[key]);
            }
    }
}
}
