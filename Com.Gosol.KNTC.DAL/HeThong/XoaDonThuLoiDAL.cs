using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Spire.Pdf.Exporting.XPS.Schema;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.HeThong;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class XoaDonThuLoiDAL
    {

        private const string PARAM_DanhSachDonThuLoi = "DeleteDT_GetSoTiepNhan_GianTiep_GetALL_v2";
        private const string PARAM_XoaDonThuLoi = "DonThu_Delete_Supper_XuLyDon_By_XuLyDonID_v2";
        private const string PARAM_ChiTietDonThu = "SupperDeleteDT_GetAllXuLyDonID_By_DonThuID_v2"; 


        private const string PARAM_SoDonThu = "@SoDonThu";
        private const string PARAM_TenCoQuan = "@TenCoQuan";
        private const string PARAM_NoiDungDon = "@NoiDungDon";
        private const string PARAM_TenLoaiKhieuTo = "@TenLoaiKhieuTo";
        private const string PARAM_NgayNhapDon = "@NgayNhapDon";


        private const string Keyword = "@Keyword";
        private const string pDonThuID = "@DonThuID";
        private const string Limit = "@Limit";
        private const string Offset = "@Offset";
        private const string TotalRow = "@TotalRow";
        private const string Start = "@Start";
        private const string LoaiKhieuToID = "@LoaiKhieuToID";
        private const string CoQuanID = "@CoQuanID";
        private const string TuNgay = "@TuNgay";
        private const string DenNgay = "@DenNgay";
        private const string End = @"End";
        private const string pXuLyDonID = "@XuLyDonID";
        private const string pSoDonThu = "@SoDonThu";


        public BaseResultModel DanhSachDonThuLoi(thamsodonthuloi  p)
        {
            var Result = new BaseResultModel();
            List<XoaDonThuLoiModel> Data = new List<XoaDonThuLoiModel>();
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(Keyword,SqlDbType.NVarChar,50),               
                new SqlParameter(Limit,SqlDbType.Int),
                new SqlParameter(Offset,SqlDbType.Int),
                new SqlParameter(TotalRow,SqlDbType.Int),
                new SqlParameter(LoaiKhieuToID,SqlDbType.Int),
                new SqlParameter(CoQuanID,SqlDbType.Int),
                new SqlParameter(TuNgay,SqlDbType.DateTime),
                new SqlParameter(DenNgay,SqlDbType.DateTime),
               
                

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";           
            parameters[1].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Size = 8;
            parameters[4].Value = p.LoaiKhieuToID ?? Convert.DBNull;
            parameters[5].Value = p.CoQuanID ?? Convert.DBNull;
            parameters[6].Value = p.TuNgay   ??   Convert.DBNull;
            parameters[7].Value = p.DenNgay   ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DeleteDT_GetSoTiepNhan_GianTiep_GetALL_v2", parameters))
                {
                    while (dr.Read())
                    {
                        XoaDonThuLoiModel item = new XoaDonThuLoiModel();
                        item.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        item.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        item.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        item.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        item.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        item.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        item.KhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"],0);
                        item.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        item.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"],DateTime.MinValue);
                        
                        Data.Add(item);
                    }
                    dr.Close();
                }
                var TotalRow = Utils.ConvertToInt32(parameters[3].Value, 0);
                Result.Status = 1;
                Result.Message = "Danh sách đơn thư lỗi ";
                Result.Data = Data;   
                Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }
        
        // chi tiet 
        public BaseResultModel ChiTiet(int? DonThuID , int? XuLyDonID)
        {
            var Result = new BaseResultModel();
            ChiTietDonThuLoi data = null;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pDonThuID,SqlDbType.Int),
                new SqlParameter(pXuLyDonID,SqlDbType.Int)
            };
            parameters[0].Value = DonThuID;
            parameters[1].Value = XuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, PARAM_ChiTietDonThu, parameters))
                {
                    while (dr.Read())
                    {
                        ChiTietDonThuLoi data_1 = new ChiTietDonThuLoi();
                        data_1.SoDonThu = Utils.ConvertToString(dr["SoDonThu"], string.Empty);
                        data_1.HoTen = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        data_1.CMND = Utils.ConvertToString(dr["CMND"], string.Empty);
                        data_1.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"],0);
                        data_1.SoDienThoai = Utils.ConvertToInt32(dr["SoDienThoai"], 0);
                        data_1.TenDanToc = Utils.ConvertToString(dr["TenDanToc"], string.Empty);
                        data_1.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"], string.Empty);
                        data_1.TenThamQuyen = Utils.ConvertToString(dr["TenThamQuyen"], string.Empty);
                        data_1.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        data_1.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        data_1.NgayThuLy = Utils.ConvertToString(dr["NgayThuLy"], string.Empty);
                        data_1.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        data_1.HanGiaiQuyet = Utils.ConvertToString(dr["HanGiaiQuyet"], string.Empty);
                        data_1.SoLan = Utils.ConvertToInt32(dr["SoLan"], 0);
                        data_1.LanGiaiQuyet = Utils.ConvertToInt32(dr["LanGiaiQuyet"], 0);
                        data_1.NoiDungHuongDan = Utils.ConvertToString(dr["NoiDungHuongDan"], string.Empty);
                        data_1.TenCanBoTiepNhan = Utils.ConvertToString(dr["TenCanBoTiepNhan"], string.Empty);
                        data_1.DiaChi = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        data_1.NguonDonDen = Utils.ConvertToString(dr["NguonDonDen"], string.Empty);
                        data_1.TenCQChuyenDonDen = Utils.ConvertToString(dr["TenCQChuyenDonDen"], string.Empty);
                        data_1.MaHoSoMotCua = Utils.ConvertToString(dr["MaHoSoMotCua"], string.Empty);
                        data_1.SoBienNhanMotCua = Utils.ConvertToInt32(dr["SoBienNhanMotCua"],0);
                        data_1.TenTrangThaiDon = Utils.ConvertToString(dr["TenTrangThaiDon"], string.Empty);
                        data_1.TenKhieuTo1 = Utils.ConvertToString(dr["TenLoaiKhieuTo1"], string.Empty);
                        data_1.TenKhieuTo2 = Utils.ConvertToString(dr["TenLoaiKhieuTo2"], string.Empty);
                        data_1.TenKhieuTo3 = Utils.ConvertToString(dr["TenLoaiKhieuTo3"], string.Empty);                   
                        data_1.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        data_1.NgayNhapDon = Utils.ConvertToString(dr["NgayNhapDon"], string.Empty);
                        data_1.TenPhongBanTiepNhan = Utils.ConvertToString(dr["TenPhongBanTiepNhan"], string.Empty);
                        data_1.TenLoaiKetQua = Utils.ConvertToString(dr["TenLoaiKetQua"], string.Empty);
                        data_1.TenCoQuanGQ = Utils.ConvertToString(dr["TenCoQuanGQ"], string.Empty);
                        data_1.TenCoQuanXuLy = Utils.ConvertToString(dr["TenCoQuanXuLy"], string.Empty);
                        data_1.TenCoQuanTiepNhan = Utils.ConvertToString(dr["TenCoQuanTiepNhan"], string.Empty);
                        data_1.TenCoQuanBanHanh = Utils.ConvertToString(dr["TenCoQuanBanHanh"], string.Empty);
                        data_1.TenPhongBanXuLy = Utils.ConvertToString(dr["TenPhongBanXuLy"], string.Empty);
                        data_1.TenCoQuanXuLy = Utils.ConvertToString(dr["TenCoQuanXuLy"], string.Empty);
                        data =data_1;
                        break;
                    } 
                    dr.Close();
                }
                Result.Status = 1;
                Result.Data = data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
            }
            return Result;
        }


        public BaseResultModel XoaDonThu(int DonThuID, int XuLyDonID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pDonThuID, SqlDbType.Int),
               new SqlParameter(pXuLyDonID ,SqlDbType.Int),
            };
            parameters[0].Value = DonThuID;
            parameters[1].Value = XuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var kq = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, PARAM_XoaDonThuLoi, parameters);
                        trans.Commit();
                        if (kq < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa đơn thu!";
                            return Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Result.Status = -1;
                       
                        Result.Message = ex.Message;
                        trans.Rollback();
                        return Result;
                        throw;
                    }
                }
            }
            Result.Status = 1;
            Result.Message = "Xóa danh đơn thu thành công!";
            return Result;
        }
        // Thamsoxoa 


        public BaseResultModel XoaDonThuLoi(Thamsoxoa thamsoxoa)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pDonThuID, SqlDbType.Int),
               new SqlParameter(pXuLyDonID ,SqlDbType.Int),
            };
            parameters[0].Value = thamsoxoa.DonThuID;
            parameters[1].Value = thamsoxoa.XuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var kq = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, PARAM_XoaDonThuLoi, parameters);
                        trans.Commit();
                        if (kq < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa đơn thu!";
                            return Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Result.Status = -1;

                        Result.Message = ex.Message;
                        trans.Rollback();
                        return Result;
                        throw;
                    }
                }
            }
            Result.Status = 1;
            Result.Message = "Xóa đơn thư lỗi thành công!";
            return Result;

        }
    }
    
}
