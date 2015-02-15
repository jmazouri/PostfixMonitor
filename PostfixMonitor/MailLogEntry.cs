using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class MailLogEntry
    {
        private string UUID { get; set; }
        public string Date { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }

        public string MiscInfo { get; set; }

        public MailLogEntry()
        {
            UUID = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return String.Format("From: {0}, To: {1}, Body: {2}", Source, Target, MiscInfo);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() == GetHashCode())
            {
                return true;
            }

            var entry = obj as MailLogEntry;
            if (entry != null)
            {
                MailLogEntry asdf = entry;
                return (asdf.UUID == UUID);

            }

            return false;
        }

        public override int GetHashCode()
        {
            return UUID.GetHashCode();
        }
    }
}
