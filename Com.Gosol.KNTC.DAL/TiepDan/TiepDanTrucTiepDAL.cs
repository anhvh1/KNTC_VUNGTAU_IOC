using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.TiepDan;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.TiepDan
{
    public class TiepDanTrucTiepDAL
    {
        //Su dung de goi StoreProcedure
        private const string INSERT_DOITUONGKN = @"DoiTuongKN_Insert_New";
        private const string INSERT_NHOMKN = @"NhomKN_Insert";
        private const string GET_ALL_LOAIDOITUONGKN = @"DM_LoaiDoiTuongKN_GetAll";


        //Ten cac bien dau vao
        // param đối tượng khiếu nại
        private const string PARAM_DoiTuongKNID = "@DoiTuongKNID";
        private const string PARAM_HoTen = "@HoTen";
        private const string PARAM_CMND = "@CMND";
        private const string PARAM_GioiTinh = "@GioiTinh";
        private const string PARAM_NgheNghiep = "@NgheNghiep";
        private const string PARAM_QuocTichID = "@QuocTichID";
        private const string PARAM_DanTocID = "@DanTocID";
        private const string PARAM_TinhID = "@TinhID";
        private const string PARAM_HuyenID = "@HuyenID";
        private const string PARAM_XaID = "@XaID";
        private const string PARAM_DiaChiCT = "@DiaChiCT";
        private const string PARAM_NhomKNID = "@NhomKNID";
        private const string PARAM_SoDienThoai = "@SoDienThoai";
        private const string PARAM_NgayCap = "@NgayCap";
        private const string PARAM_NoiCap = "@NoiCap";

        private const string PARAM_NHOMKN_ID = "@NhomKNID";
        private const string PARAM_SO_LUONG = "@SoLuong";
        private const string PARAM_TENCQ = "@TenCoQuan";
        private const string PARAM_LOAI_DOI_TUONG = "@LoaiDoiTuongKNID";
        private const string PARAM_DIACHI_CQ = "@DiaChiCQ";
        private const string PARAM_DAIDIEN_PHAPLY = "@DaiDienPhapLy";
        private const string PARAM_DUOC_UYQUYEN = "@DuocUyQuyen";

        private const string PARAM_LOAI_DOI_TUONG_KN_ID = "@LoaiDoiTuongKNID";
        private const string PARAM_TEN_LOAI_DOI_TUONG_KN = "@TenLoaiDoiTuongKN";

        /// <summary>
        /// Thêm mới đối tượng khiếu nại
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        public BaseResultModel ThemMoiDoiTuongKN(TiepDanTrucTiepMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                int i = 0;
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(PARAM_HoTen, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CMND, SqlDbType.VarChar),
                    new SqlParameter(PARAM_GioiTinh, SqlDbType.Int),
                    new SqlParameter(PARAM_NgheNghiep, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_QuocTichID, SqlDbType.Int),
                    new SqlParameter(PARAM_DanTocID, SqlDbType.Int),
                    new SqlParameter(PARAM_TinhID, SqlDbType.Int),
                    new SqlParameter(PARAM_HuyenID, SqlDbType.Int),
                    new SqlParameter(PARAM_XaID, SqlDbType.Int),
                    new SqlParameter(PARAM_DiaChiCT, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NhomKNID, SqlDbType.Int),
                    new SqlParameter(PARAM_SoDienThoai, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar)
                };
                parameters[0].Value = item.DoiTuongKN[i].HoTen;
                parameters[1].Value = item.DoiTuongKN[i].CMND;
                parameters[2].Value = item.DoiTuongKN[i].GioiTinh;
                parameters[3].Value = item.DoiTuongKN[i].NgheNghiep;
                parameters[4].Value = item.DoiTuongKN[i].QuocTichID;
                parameters[5].Value = item.DoiTuongKN[i].DanTocID;
                parameters[6].Value = item.DoiTuongKN[i].TinhID;
                parameters[7].Value = item.DoiTuongKN[i].HuyenID;
                parameters[8].Value = item.DoiTuongKN[i].XaID;
                parameters[9].Value = item.DoiTuongKN[i].DiaChiCT;
                parameters[10].Value = item.DoiTuongKN[i].NhomKNID;
                parameters[11].Value = item.DoiTuongKN[i].SoDienThoai;
                parameters[12].Value = item.DoiTuongKN[i].NgayCap;
                parameters[13].Value = item.DoiTuongKN[i].NoiCap;
                if (item.DoiTuongKN[i].NgayCap == null)
                {
                    parameters[12].Value = DBNull.Value;
                }
                if (item.DoiTuongKN[i].NoiCap == null)
                {
                    parameters[13].Value = DBNull.Value;
                }

                i++;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_DOITUONGKN, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới thành công!";
                            Result.Data = Result.Status;
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = Constant.ERR_INSERT;
                            trans.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;
            }
            return Result;
        }

        // Thêm mới nhóm khiếu nại
        public BaseResultModel ThemMoiNhomKN(TiepDanTrucTiepMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                int i = 0;
                SqlParameter[] parameters = new SqlParameter[]
                {
                    //new SqlParameter(PARAM_NHOMKN_ID, SqlDbType.Int),
                    new SqlParameter(PARAM_SO_LUONG, SqlDbType.Int),
                    new SqlParameter(PARAM_TENCQ, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_LOAI_DOI_TUONG, SqlDbType.Int),
                    new SqlParameter(PARAM_DIACHI_CQ, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DAIDIEN_PHAPLY, SqlDbType.Bit),
                    new SqlParameter(PARAM_DUOC_UYQUYEN, SqlDbType.Bit)
                };
                parameters[0].Value = item.NhomKN[i].SoLuong;
                parameters[1].Value = item.NhomKN[i].TenCQ;
                parameters[2].Value = item.NhomKN[i].LoaiDoiTuongKNID;
                parameters[3].Value = item.NhomKN[i].DiaChiCQ;
                parameters[4].Value = item.NhomKN[i].DaiDienPhapLy;
                parameters[5].Value = item.NhomKN[i].DuocUyQuyen;

                i++;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_NHOMKN, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới thành công!";
                            Result.Data = Result.Status;
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = Constant.ERR_INSERT;
                            trans.Rollback();
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;
            }
            return Result;
        }

        // Get danh sách Loại đối tượng khiếu nại
        public BaseResultModel DanhSachLoaiDoiTuongKN()
        {
            var Result = new BaseResultModel();
            List<LoaiDoiTuongKNMOD> Data = new List<LoaiDoiTuongKNMOD>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_LOAIDOITUONGKN, null))
                {
                    while (dr.Read())
                    {
                        LoaiDoiTuongKNMOD item = new LoaiDoiTuongKNMOD();
                        item.LoaiDoiTuongKNID = Utils.ConvertToInt32(dr["LoaiDoiTuongKNID"], 0);
                        item.TenLoaiDoiTuongKN = Utils.ConvertToString(dr["TenLoaiDoiTuongKN"], string.Empty);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                Result.Status = 1;
                Result.Data = Data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }
    }
}
