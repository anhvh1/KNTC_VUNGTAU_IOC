using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Create by AnhVH - 12/10/2022
//Edit by AnhVH - 13/10/2022:
//  + Sửa lại hàm thêm mới, 
//  + 


namespace Com.Gosol.KNTC.DAL.DanhMuc
{

    public class DanhMucQuocTichDAL
    {
        #region Variable
        private const string pQuocTichID = "@QuocTichID";
        private const string pMaQuocTich = "@MaQuocTich";
        private const string pTenQuocTich = "@TenQuocTich";
        private const string pGhiChu = "@GhiChu";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_QuocTich_DanhSach";
        private const string sThemMoi = "v2_DM_QuocTich_ThemMoi";
        private const string sChiTiet = "v2_DM_QuocTich_ChiTiet";
        private const string sCapNhat = "v2_DM_QuocTich_CapNhat";
        private const string sXoa = "v2_DM_QuocTich_Xoa";
        private const string sKiemTraTonTai = "v2_DM_QuocTich_KiemTraTonTai";

        #endregion


        #region Function

        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên quốc tịch
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<DanhMucQuocTichMOD> Data = new List<DanhMucQuocTichMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pTrangThai,SqlDbType.Bit),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),
                new SqlParameter(pTotalRow,SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.Status ?? Convert.DBNull;
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[3].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[4].Direction = ParameterDirection.Output;
            parameters[4].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSach, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucQuocTichMOD item = new DanhMucQuocTichMOD();
                        item.QuocTichID = Utils.ConvertToInt32(dr["QuocTichID"], 0);
                        item.MaQuocTich = Utils.ConvertToString(dr["MaQuocTich"], string.Empty);
                        item.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"], string.Empty);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.TrangThai = Utils.ConvertToBoolean(dr["TrangThai"], false);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                var TotalRow = Utils.ConvertToInt32(parameters[4].Value, 0);
                Result.Status = 1;
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


        /// <summary>
        /// lấy chi tiết thông tin của 1 danh mục quốc tịch theo id
        /// </summary>
        /// <param name="QuocTichID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? QuocTichID)
        {
            var Result = new BaseResultModel();
            DanhMucQuocTichMOD data = new DanhMucQuocTichMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pQuocTichID,SqlDbType.Int,25),
            };
            parameters[0].Value = QuocTichID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        data.QuocTichID = Utils.ConvertToInt32(dr["QuocTichID"], 0);
                        data.MaQuocTich = Utils.ConvertToString(dr["MaQuocTich"], string.Empty);
                        data.TenQuocTich = Utils.ConvertToString(dr["TenQuocTich"], string.Empty);
                        data.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        data.TrangThai = Utils.ConvertToBoolean(dr["TrangThai"], false);
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

        /// <summary>
        /// kiểm tra tồn tại của danh mục quốc tịch thông qua mã hoặc tên (trùng mã hoặc trùng tên)
        /// , nếu Type=1 và QuocTichID = null thì kiểm tra trùng mã cho trường hợp thêm mới
        /// , nếu Type=2 và QuocTichID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và QuocTichID != null thì kiểm tra trùng mã cho trường hợp Cập nhật
        /// , nếu Type=2 và QuocTichID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và QuocTichID != null thì kiểm tra tồn tại theo QuocTichID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? QuocTichID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pQuocTichID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = QuocTichID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraTonTai, parameters))
                {
                    while (dr.Read())
                    {
                        var TonTai = Utils.ConvertToInt32(dr["TonTai"], 0);
                        Result = TonTai > 0 ? true : false;
                        break;
                    }
                    dr.Close();
                }
            }
            catch (Exception)
            {
            }
            return Result;
        }

        /// <summary>
        /// Thêm mới quốc tịch
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhMucQuocTichThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pMaQuocTich, SqlDbType.VarChar),
                    new SqlParameter(pTenQuocTich, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.MaQuocTich.Trim();
                parameters[1].Value = item.TenQuocTich.Trim();
                parameters[2].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                parameters[3].Value = item.TrangThai;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoi, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới quốc tịch thành công!";
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
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;
            }
            return Result;
        }


        /// <summary>
        /// cập nhật thông tin danh mục quốc tịch
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucQuocTichMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pQuocTichID, SqlDbType.Int),
                    new SqlParameter(pMaQuocTich, SqlDbType.VarChar),
                    new SqlParameter(pTenQuocTich, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.QuocTichID;
                parameters[1].Value = item.MaQuocTich.Trim();
                parameters[2].Value = item.TenQuocTich.Trim();
                parameters[3].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                parameters[4].Value = item.TrangThai;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhat, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật quốc tịch thành công!";
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
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_UPDATE;
            }
            return Result;
        }


        /// <summary>
        /// xóa danh mục quốc tịch
        /// </summary>
        /// <param name="QuocTichID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int QuocTichID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pQuocTichID, SqlDbType.Int)
            };
            parameters[0].Value = QuocTichID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, sXoa, parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa danh mục quốc tịch!";
                            return Result;
                        }
                    }
                    catch
                    {
                        Result.Status = -1;
                        Result.Message = Constant.ERR_DELETE;
                        trans.Rollback();
                        return Result;
                        throw;
                    }
                }
            }
            Result.Status = 1;
            Result.Message = "Xóa danh mục quốc tịch thành công!";
            return Result;
        }

        #endregion

    }
}
