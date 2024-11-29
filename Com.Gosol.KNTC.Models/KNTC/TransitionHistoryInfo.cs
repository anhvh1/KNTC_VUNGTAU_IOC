using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.Models.KNTC
{
    public class TransitionHistoryInfo
    {

        public string BuocThucHien { get; set; }
        public DateTime? ThoiGianThucHien { get; set; }
        public string ThoiGianThucHienStr { get; set; }
        public string CanBoThucHien { get; set; }
        public string YKienCanBo { get; set; }
        public string ThaoTac { get; set; }
        public DateTime DueDate { get; set; }
        public string DueDateStr { get; set; }
        public string TenCoQuan { get; set; }
        public int? StateID { get; set; }
        public int? NextStateID { get; set; }
        public int? TrangThai { get; set; }
        public TransitionHistoryInfo()
        {

        }
        public TransitionHistoryInfo(string BuocThucHien, DateTime? ThoiGianThucHien, string CanBoThucHien, string YKienCanBo, string ThaoTac, string TenCoQuan, int? StateID, int? trangThai)
        {
            this.BuocThucHien = BuocThucHien;
            this.ThoiGianThucHien = ThoiGianThucHien;
            this.CanBoThucHien = CanBoThucHien;
            this.YKienCanBo = YKienCanBo;
            this.ThaoTac = ThaoTac;
            this.TenCoQuan = TenCoQuan;
            this.StateID = StateID;
            this.TrangThai = trangThai;
        }
    }
}
