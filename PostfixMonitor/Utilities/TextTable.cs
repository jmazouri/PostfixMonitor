using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PostfixMonitor
{
    public class TextTable
    {
        public int TableWidth { get; set; }
        public int AvgColumnWidth { get; set; }

        public string HorizontalLine = "\u2550";
        public string VerticalLine = "\u2551";

        public TextTable(int tableWidth = 79)
        {
            TableWidth = tableWidth;
        }

        static string CenterString(string stringToCenter, int totalLength)
        {
            return stringToCenter.PadLeft(((totalLength - stringToCenter.Length) / 2)
                                          + stringToCenter.Length)
                .PadRight(totalLength);
        }

        static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length);
            }
            return source;
        }

        string Line()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 1; i != TableWidth + 1; i++) { builder.Append(HorizontalLine); }

            return builder.ToString();
        }

        public string FromList<T>(IEnumerable<T> items, List<TableCol> columnList = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine();

            if (columnList == null)
            {
                columnList = new List<TableCol>();
            }

            var props = typeof(T).GetProperties();
            var Columns = new List<string>();

            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                Columns.Add(prop.Name);
            }

            AvgColumnWidth = (TableWidth / Columns.Count);

            builder.AppendLine(Line());

            for (int i = 0; i < Columns.Count; i++)
            {
                int CurColWidth = AvgColumnWidth;

                if (columnList.ElementAtOrDefault(i) != null)
                {
                    if (columnList[i].ColumnWidth > 0)
                    {
                        CurColWidth = (int)(TableWidth * columnList[i].ColumnWidth);
                    }
                }

                builder.Append(String.Format("{0}" + (i < Columns.Count - 1 ? VerticalLine : ""), CenterString(Columns[i], CurColWidth - 1)));
            }

            builder.AppendLine();
            builder.AppendLine(Line());

            foreach (object o in items)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    int CurColWidth = AvgColumnWidth;

                    if (columnList.ElementAtOrDefault(i) != null)
                    {
                        if (columnList[i].ColumnWidth > 0)
                        {
                            CurColWidth = (int)(TableWidth * columnList[i].ColumnWidth);
                        }
                    }

                    var val = props.First(d => d.Name == Columns[i]).GetValue(o, null).ToString();

                    double useless;
                    bool valIsNumeric = double.TryParse(val, out useless);

                    if ((columnList.ElementAtOrDefault(i) != null && columnList[i].CenterAlign) || valIsNumeric)
                    {
                        builder.Append(String.Format("{0, -" + (CurColWidth - 2) + "}" + (i < Columns.Count - 1 ? VerticalLine : ""),
                                CenterString(Truncate(val, CurColWidth), CurColWidth)));
                    }
                    else
                    {
                        builder.Append(String.Format(" {0, -" + (CurColWidth - 2) + "}" + (i < Columns.Count - 1 ? VerticalLine : ""), Truncate(val, CurColWidth - 1)));
                    }

                }

                builder.AppendLine();
            }

            builder.AppendLine(Line());

            return builder.ToString();
        }
    }
}