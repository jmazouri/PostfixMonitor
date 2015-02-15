using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class MailLogEntry
    {
        public string Source { get; set; }
        public string Target { get; set; }

        public string MiscInfo { get; set; }

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
                return (asdf.Target == Target && asdf.MiscInfo == MiscInfo && asdf.Source == Source);

            }

            return false;
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode() + Target.GetHashCode() + MiscInfo.GetHashCode();
        }
    }
}
