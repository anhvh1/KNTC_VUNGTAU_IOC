using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class DanhMucChungModel
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public DanhMucChungModel (string Name, int Value)
        { 
            this.Name = Name;
            this.Value = Value;
        }
    }
}
