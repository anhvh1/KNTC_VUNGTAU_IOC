using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DocumentModel
    {
        public int DocumentID { get; set; }
        public int WorkflowID { get; set; }
        public int StateID { get; set; }
        public Nullable<DateTime> DueDate { get; set; }
        public int ProcessCount { get; set; }
    }
}
