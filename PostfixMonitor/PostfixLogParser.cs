using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class PostfixLogParser
    {
        private static Regex parser;

        public PostfixLogParser(List<string> entries)
        {
            if (parser == null)
            {
                parser = new Regex("^(\\w{3}[^a-zA-Z]+)+", RegexOptions.Compiled);
            }

            foreach (string s in entries)
            {
                var entry = ParseEntry(s);
                if (entry != null)
                {
                    Entries.Add(entry);
                }   
            }
        }

        private static string GetDate(string line)
        {
            var matches = parser.Matches(line);

            if (matches.Count > 0)
            {
                return matches[0].Value;
            }

            return "N/A";
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
                    Date = GetDate(line),
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
                        Date = GetDate(line),
                        Source = "mail server",
                        Target = line.Substring(startOfTarget, targetLength),
                        MiscInfo = "SMTP request to remote server."
                    };
                }

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
                        Date = GetDate(line),
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
                            Date = GetDate(line),
                            Source = "mail server",
                            Target = line.Substring(startOfTarget, targetLength),
                            MiscInfo = String.Format("Relay: {0}", line.Substring(startOfRelay, relayLength))
                        };
                    }
                }
            }

            return new MailLogEntry
            {
                Date = GetDate(line),
                Target = "Unknown",
                Source = "Unknown",
                MiscInfo = line
            };
        }
    }
}
