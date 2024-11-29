using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DocumentInfo
    {
        public DateTime DueDate { get; set; }
        public string DueDateStr { get; set; }
        public int StateID { get; set; }
    }
}
