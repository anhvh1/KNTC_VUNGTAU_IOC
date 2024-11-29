using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.KNTC.BUS.HeThong
{

    public class CauHinhBUS
    {
        CauHinhDAL _CauHinhDAL;
        public CauHinhBUS()
        {
            _CauHinhDAL = new CauHinhDAL();
        }
        public List<CauHinhModel> DanhSachCauHinhTheoPhanLoai(string phanLoai)
        {
            return _CauHinhDAL.DanhSachCauHinhTheoPhanLoai(phanLoai);
        }


    }
}
