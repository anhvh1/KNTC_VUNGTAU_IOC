using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Com.Gosol.KNTC.DAL.HeThong
{

    public class CauHinhDAL
    {
        public CauHinhDAL()
        {

        }


        public List<CauHinhModel> DanhSachCauHinhTheoPhanLoai(string phanLoai)
        {

            List<CauHinhModel> list = new List<CauHinhModel>();
            var query = "select * from HT_CauHinh ";
            if (phanLoai != null && phanLoai.Length > 0)
                query = query + " where PhanLoai='" + phanLoai + "'";

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.Text, query))
                {
                    while (dr.Read())
                    {
                        CauHinhModel item = new CauHinhModel();
                        item.id = Utils.ConvertToInt32(dr["id"], 0);
                        item.MaSo = Utils.ConvertToString(dr["MaSo"], string.Empty);
                        item.GiaTri = Utils.ConvertToString(dr["GiaTri"], string.Empty);
                        item.PhanLoai = Utils.ConvertToString(dr["PhanLoai"], string.Empty);
                        item.Nhom = Utils.ConvertToString(dr["Nhom"], string.Empty);
                        list.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


    }
}
