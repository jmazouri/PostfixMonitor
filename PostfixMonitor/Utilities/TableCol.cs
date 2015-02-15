using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class TableCol
    {
        /// <summary>
        /// The width of this column in percentage format. (Ex. 0.5 == 50%). If set to 0, will auto scale.
        /// </summary>
        public double ColumnWidth { get; set; }

        /// <summary>
        /// Whether or not the text for this column will be center aligned.
        /// </summary>
        public bool CenterAlign { get; set; }

        public TableCol()
        {
            ColumnWidth = 0;
            CenterAlign = false;
        }
    }
}