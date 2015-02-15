using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class PostfixLogParser
    {
        public PostfixLogParser(List<string> entries)
        {
            foreach (string s in entries)
            {
                var entry = ParseEntry(s);
                if (entry != null)
                {
                    Entries.Add(entry);
                }   
            }
        }

        public List<MailLogEntry> Entries = new List<MailLogEntry>();

        public MailLogEntry ParseEntry(string line)
        {
            if (line.Contains("Login:"))
            {
                int startOfSource = line.IndexOf("Login: user=<") + 13;
                int sourceLength = line.IndexOf(">,", startOfSource) - startOfSource;

                int startOfIp = line.IndexOf("rip=") + 4;
                int ipLength = line.IndexOf(", lip", startOfIp) - startOfIp;

                return new MailLogEntry()
                {
                    Source = line.Substring(startOfSource, sourceLength),
                    Target = "mail server",
                    MiscInfo = String.Format("Server login from {0}", line.Substring(startOfIp, ipLength))
                };
            }

            if (line.Contains("postfix/smtp"))
            {
                if (line.Contains("connect to"))
                {
                    int startOfTarget = line.IndexOf("connect to") + 11;
                    int targetLength = line.IndexOf("[", startOfTarget) - startOfTarget;

                    return new MailLogEntry()
                    {
                        Source = "mail server",
                        Target = line.Substring(startOfTarget, targetLength),
                        MiscInfo = "SMTP request to remote server."
                    };
                }
                else
                {

                    if (line.Contains("connect from"))
                    {
                        string localmisc = "Connection from {0}";

                        if (line.Contains("disconnect from"))
                        {
                            localmisc = "Disconnection from {0}";
                        }

                        int startOfTarget = line.IndexOf("connect from") + 13;
                        int targetLength = line.IndexOf("]", startOfTarget) - startOfTarget + 1;

                        return new MailLogEntry()
                        {
                            Source = line.Substring(startOfTarget, targetLength),
                            Target = "mail server",
                            MiscInfo = String.Format(localmisc, line.Substring(startOfTarget, targetLength))
                        };
                    }
                    else
                    {
                        if (!line.Contains("sasl"))
                        {
                            int startOfTarget = line.IndexOf("to=<") + 4;
                            int targetLength = line.IndexOf(">,", startOfTarget) - startOfTarget;

                            int startOfRelay = line.IndexOf("relay=") + 6;
                            int relayLength = line.IndexOf("]", startOfRelay) - startOfRelay + 1;

                            return new MailLogEntry()
                            {
                                Source = "mail server",
                                Target = line.Substring(startOfTarget, targetLength),
                                MiscInfo = String.Format("Relay: {0}", line.Substring(startOfRelay, relayLength))
                            };
                        }
                    }
                    
                }
                
            }

            

            return null;
        }
    }
}
