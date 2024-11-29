using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.Idics
{
    public class MemberEntry
    {
        public string IP { get; set; }
        public string RemoteIpAddress { get; set; }
        public int? OS { get; set; }
        public string Sign { get; set; }
        public string MeasurementData { get; set; }
        public string Location { get; set; }
        public DateTime? Date { get; set; }
        public ExaminationDataModel ExaminationDataEntity { get; set; }
        public PhysicalBodyCompositionModel PhysicalBodyCompositionEntity { get; set; }
        public List<SuggestModel> SuggestList { get; set; }
        public UserModel UserEntity { get; set; }
    }
}
