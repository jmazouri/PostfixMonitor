using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class Program
    {
        static int topIndex = 0;
        static int maxEntries = 3;
        static List<MailLogEntry> oldEntries = new List<MailLogEntry>();
        static List<MailLogEntry> selectedEntries = new List<MailLogEntry>(); 

        static void Main(string[] args)
        {
            Thread workThread = new Thread(new ThreadStart(delegate()
            {
                while (true)
                {
#if DEBUG
                    List<string> entries = new List<string>
                    {
                        "Feb  8 14:56:51 macchiato postfix/smtpd[8073]: connect from unknown[10.0.0.12]",
                        "Feb  8 14:56:51 macchiato dovecot: auth-worker(8078): mysql(127.0.0.1): Connected to database mailserver",
                        "Feb  8 14:56:51 macchiato postfix/smtpd[8073]: 8C2F31E0EC3: client=unknown[10.0.0.12], sasl_method=PLAIN, sasl_username=jmazouri@local.jmazouri.com",
                        "Feb  8 14:56:51 macchiato postfix/cleanup[8083]: 8C2F31E0EC3: message-id=<em9dea627e-a7fd-4b63-b2b9-623cdc12010e@jmazouri-browsy>",
                        "Feb  8 14:56:51 macchiato postfix/qmgr[5932]: 8C2F31E0EC3: from=<jmazouri@local.jmazouri.com>, size=1540, nrcpt=1 (queue active)",
                        "Feb  8 14:56:51 macchiato postfix/smtpd[8073]: disconnect from unknown[10.0.0.12]",
                        "Feb  8 14:56:51 macchiato dovecot: ssl-params: Generating SSL parameters",
                        "Feb  8 14:56:51 macchiato dovecot: imap-login: Login: user=<jmazouri@local.jmazouri.com>, method=PLAIN, rip=10.0.0.12, lip=10.0.0.15, mpid=8089, TLS, session=<rqAKDpkOfwAKAAAM>",
                        "Feb  8 14:56:52 macchiato postfix/smtp[8085]: connect to gmail-smtp-in.l.google.com[2607:f8b0:4001:c07::1a]:25: Network is unreachable",
                        "Feb  8 14:56:54 macchiato postfix/smtp[8085]: 8C2F31E0EC3: to=<jmazouri@gmail.com>, relay=gmail-smtp-in.l.google.com[74.125.207.26]:25, delay=2.8, delays=0.16/0.02/0.67/2, dsn=2.0.0, status=sent (250 2.0.0 OK 1423425562 n91si6182206ioi.107 - gsmtp)",
                        "Feb  8 14:56:54 macchiato postfix/qmgr[5932]: 8C2F31E0EC3: removed",
                        "Feb  8 14:56:55 macchiato dovecot: ssl-params: SSL parameters regeneration completed",
                        "Feb  8 15:00:11 macchiato postfix/anvil[8075]: statistics: max connection rate 1/60s for (smtp:10.0.0.12) at Feb  8 14:56:51",
                        "Feb  8 15:00:11 macchiato postfix/anvil[8075]: statistics: max connection count 1 for (smtp:10.0.0.12) at Feb  8 14:56:51",
                        "Feb  8 15:00:11 macchiato postfix/anvil[8075]: statistics: max cache size 1 at Feb  8 14:56:51",
                        "Feb  8 15:26:52 macchiato dovecot: imap(jmazouri@local.jmazouri.com): Disconnected for inactivity in=1435 out=391"
                    };
#else
                    List<string> entries = File.ReadAllText("/var/log/mail.log").Split("\n".ToCharArray()).ToList();
    #endif
                    PostfixLogParser parser = new PostfixLogParser(entries);

                    var transitional = new List<MailLogEntry>();
                    transitional.AddRange(parser.Entries);
                    transitional.AddRange(oldEntries);

                    selectedEntries =
                        transitional.Where(d => !oldEntries.Contains(d)).Skip(topIndex).Take(maxEntries).ToList();



                    oldEntries.AddRange(transitional);

                    Thread.Sleep(1500);
                }
            }));
            
            
        }
    }
}
