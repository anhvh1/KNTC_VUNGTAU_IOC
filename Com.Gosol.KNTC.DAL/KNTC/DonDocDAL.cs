using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.KNTC;
using DocumentFormat.OpenXml.EMMA;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class DonDocDAL
    {
        public List<DonThuDonDocInfo> GetDanhSachDonThuCanDonDoc(DateTime? startDate, DateTime? endDate,
            int? CoQuanID, int? HuongGiaiQuyetID, int? LoaiKhieuToID, int? TrangThaiID, string Keyword, int? CoQuanDangNhapID, int? start, int? end)
        {
            List<DonThuDonDocInfo> infoList = new List<DonThuDonDocInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@HuongGiaiQuyetID", SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter("@TrangThaiID", SqlDbType.Int),
                 new SqlParameter("@Keyword", SqlDbType.NVarChar),
                 new SqlParameter("@CoQuanDangNhapID", SqlDbType.Int),
                  new SqlParameter("@Start", SqlDbType.Int),
                  new SqlParameter("@End", SqlDbType.Int)
            };
            parm[0].Value = startDate ?? Convert.DBNull;
            parm[1].Value = endDate ?? Convert.DBNull;
            parm[2].Value = CoQuanID ?? Convert.DBNull;
            parm[3].Value = HuongGiaiQuyetID ?? Convert.DBNull;
            parm[4].Value = LoaiKhieuToID ?? Convert.DBNull;
            parm[5].Value = TrangThaiID ?? Convert.DBNull;
            parm[6].Value = Keyword == "" ? "" : Keyword;
            parm[7].Value = CoQuanDangNhapID ?? Convert.DBNull;
            parm[8].Value = start ?? Convert.DBNull;
            parm[9].Value = end ?? Convert.DBNull;
            try
            {
                //var query = new DataTable();
                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonDoc_DS_CanDonDoc1", parm))
                {
                    while (dr.Read())
                    {
                        DonThuDonDocInfo dtInfo = new DonThuDonDocInfo();
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["TenChuDon"], string.Empty);
                        dtInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        dtInfo.NguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        dtInfo.PhanLoai = Utils.ConvertToString(dr["PhanLoai"], string.Empty);
                        dtInfo.TenHuongXuLy = Utils.ConvertToString(dr["TenHuongXuLy"], string.Empty);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongXuLyID"], 0);
                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        dtInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        dtInfo.NgayDonDocStr = Format.FormatDate(dtInfo.HanXuLy.Value);
                        dtInfo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        dtInfo.CanhBao = Utils.ConvertToInt32(dr["CanhBao"], 0);
                        if (dtInfo.TrangThaiID == 1)
                        {
                            dtInfo.TenTrangThai = "Chưa xử lý";
                        }
                        else if (dtInfo.TrangThaiID == 4)
                        {
                            dtInfo.TenTrangThai = "Đã xử lý";
                        }
                        else if (dtInfo.TrangThaiID == 2)
                        {
                            dtInfo.TenTrangThai = "Chưa giải quyết";
                        }
                        else if (dtInfo.TrangThaiID == 3)
                        {
                            dtInfo.TenTrangThai = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.TenTrangThai = "Đã giải quyết";
                        }
                        dtInfo.IsDonDoc = Utils.ConvertToInt32(dr["DonDocID"], 0);
                        if (dtInfo.IsDonDoc > 0)
                        {
                            dtInfo.TenTrangThaiDonDoc = "Đã đôn đốc";
                        }
                        else
                        {
                            dtInfo.TenTrangThaiDonDoc = "Chưa đôn đốc";
                        }
                        //dtInfo.DonDocID = Utils.ConvertToInt32(dr["DonDocID"], 0);
                        //dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        //dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        if(dtInfo.NhomKNID > 0)
                        {
                            dtInfo.listDoiTuongKN = new DoiTuongKN().GetByNhomKNID(dtInfo.NhomKNID ?? 0).ToList();
                        }
                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        public List<DonThuDonDocInfo> GetDanhSachDonThuCanDonDoc_NotPaging(DateTime? startDate, DateTime? endDate,
    int? CoQuanID, int? HuongGiaiQuyetID, int? LoaiKhieuToID, int? TrangThaiID, string Keyword, int? CoQuanDangNhapID)
        {
            List<DonThuDonDocInfo> infoList = new List<DonThuDonDocInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@HuongGiaiQuyetID", SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter("@TrangThaiID", SqlDbType.Int),
                 new SqlParameter("@Keyword", SqlDbType.NVarChar),
                 new SqlParameter("@CoQuanDangNhapID", SqlDbType.Int)

            };
            parm[0].Value = startDate ?? Convert.DBNull;
            parm[1].Value = endDate ?? Convert.DBNull;
            parm[2].Value = CoQuanID ?? Convert.DBNull;
            parm[3].Value = HuongGiaiQuyetID ?? Convert.DBNull;
            parm[4].Value = LoaiKhieuToID ?? Convert.DBNull;
            parm[5].Value = TrangThaiID ?? Convert.DBNull;
            parm[6].Value = Keyword == "" ? "" : Keyword;
            parm[7].Value = CoQuanDangNhapID ?? Convert.DBNull;
            //parm[8].Value = start ?? Convert.DBNull;
            //parm[9].Value = end ?? Convert.DBNull;
            try
            {
                //var query = new DataTable();
                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonDoc_DS_CanDonDoc_NotPaging", parm))
                {
                    while (dr.Read())
                    {
                        DonThuDonDocInfo dtInfo = new DonThuDonDocInfo();
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["TenChuDon"], string.Empty);
                        dtInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        dtInfo.NguonDonDen = Utils.ConvertToString(dr["TenNguonDonDen"], string.Empty);
                        dtInfo.PhanLoai = Utils.ConvertToString(dr["PhanLoai"], string.Empty);
                        dtInfo.TenHuongXuLy = Utils.ConvertToString(dr["TenHuongXuLy"], string.Empty);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongXuLyID"], 0);
                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        dtInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        dtInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);
                        dtInfo.NgayDonDocStr = Format.FormatDate(dtInfo.HanXuLy.Value);
                        dtInfo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        dtInfo.CanhBao = Utils.ConvertToInt32(dr["CanhBao"], 0);
                        if (dtInfo.TrangThaiID == 1)
                        {
                            dtInfo.TenTrangThai = "Chưa xử lý";
                        }
                        else if (dtInfo.TrangThaiID == 4)
                        {
                            dtInfo.TenTrangThai = "Đã xử lý";
                        }
                        else if (dtInfo.TrangThaiID == 2)
                        {
                            dtInfo.TenTrangThai = "Chưa giải quyết";
                        }
                        else if (dtInfo.TrangThaiID == 3)
                        {
                            dtInfo.TenTrangThai = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.TenTrangThai = "Đã giải quyết";
                        }
                        dtInfo.IsDonDoc = Utils.ConvertToInt32(dr["DonDocID"], 0);
                        if (dtInfo.IsDonDoc > 0)
                        {
                            dtInfo.TenTrangThaiDonDoc = "Đã đôn đốc";
                        }
                        else
                        {
                            dtInfo.TenTrangThaiDonDoc = "Chưa đôn đốc";
                        }
                        //dtInfo.DonDocID = Utils.ConvertToInt32(dr["DonDocID"], 0);
                        //dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        //dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        public void UpdateNhanVanBanDonDoc(int? XulyDonID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                //new SqlParameter("@ViewerVBDonDoc", SqlDbType.Int),
                new SqlParameter("@XulyDonID", SqlDbType.Int)

            };
            //parm[0].Value = 0;
            parm[0].Value = XulyDonID ?? Convert.DBNull;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        var val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "XuLyDon_UpDateVBDonDoc_New", parm);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            //return val;
        }
        private DonThuDonDocInfo GetDataDonDoc(SqlDataReader dr)
        {
            DonThuDonDocInfo dtInfo = new DonThuDonDocInfo();
            //dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
            dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
            //dtInfo.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
            dtInfo.NgayDonDoc = Utils.ConvertToDateTime(dr["NgayDonDoc"], DateTime.MinValue);
            dtInfo.NgayDonDocStr = Format.FormatDate(dtInfo.NgayDonDoc.Value);
            //dtInfo.TenChuDon = Utils.ConvertToString(dr["TenChuDon"], string.Empty);
            dtInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDonDoc"], string.Empty);
            //dtInfo.HanXuLy = Utils.ConvertToDateTime(dr["HanXuLy"], DateTime.MinValue);

            return dtInfo;
        }
        public List<DonThuDonDocInfo> GetDonDocByXLDID(int? XuLyDonID)
        {
            List<DonThuDonDocInfo> infoList = new List<DonThuDonDocInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@XuLyDonID", SqlDbType.Int)

            };
            parm[0].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonDoc_GetDonDocByXLDID", parm))
                {
                    while (dr.Read())
                    {
                        DonThuDonDocInfo dtInfo = GetDataDonDoc(dr);

                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
        public int CountDanhSachDonThuCanDonDoc(DateTime? startDate, DateTime? endDate,
         int? CoQuanID, int? HuongGiaiQuyetID, int? LoaiKhieuToID, int? TrangThaiID, string Keyword, int? CoQuanDangNhapID, int? start, int? end)
        {
            DonThuDonDocInfo infoList = new DonThuDonDocInfo();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter("@StartDate", SqlDbType.DateTime),
                new SqlParameter("@EndDate", SqlDbType.DateTime),
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@HuongGiaiQuyetID", SqlDbType.Int),
                new SqlParameter("@LoaiKhieuToID", SqlDbType.Int),
                new SqlParameter("@TrangThaiID", SqlDbType.Int),
                 new SqlParameter("@Keyword", SqlDbType.NVarChar),
                 new SqlParameter("@CoQuanDangNhapID", SqlDbType.Int)

            };
            parm[0].Value = startDate ?? Convert.DBNull;
            parm[1].Value = endDate ?? Convert.DBNull;
            parm[2].Value = CoQuanID ?? Convert.DBNull;
            parm[3].Value = HuongGiaiQuyetID ?? Convert.DBNull;
            parm[4].Value = LoaiKhieuToID ?? Convert.DBNull;
            parm[5].Value = TrangThaiID ?? Convert.DBNull;
            parm[6].Value = Keyword == "" ? "" : Keyword;
            parm[7].Value = CoQuanDangNhapID ?? Convert.DBNull;

            try
            {
                //var query = new DataTable();
                //SqlDataReader dr1 = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "BaoCaoPhanAnhKienNghi_GetDSChiTietDonThu_New", parm);
                //query.Load(dr1);

                //var l = ListCoQuan.Select(x => x.CoQuanID).ToList();
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DonDoc_DS_CountCanDonDoc", parm))
                {
                    while (dr.Read())
                    {
                        infoList = new DonThuDonDocInfo();
                        infoList.Tong = Utils.ConvertToInt32(dr["Tong"], 0);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList.Tong;
        }

    }
}
