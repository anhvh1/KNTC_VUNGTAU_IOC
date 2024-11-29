using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Create by NamNH - 13/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucPhongBanDAL
    {
        #region Variable
        private const string pPhongBanID = "@PhongBanID";
        private const string pSoDienThoai = "@SoDienThoai";
        private const string pTenPhongBan = "@TenPhongBan";
        private const string pGhiChu = "@GhiChu";
        private const string pCoQuanID = "@CoQuanID";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_PhongBan_DanhSach";
        private const string sThemMoi = "v2_DM_PhongBan_ThemMoi";
        private const string sChiTiet = "v2_DM_PhongBan_ChiTiet";
        private const string sCapNhat = "v2_DM_PhongBan_CapNhat";
        private const string sXoa = "v2_DM_PhongBan_Xoa";
        private const string sKiemTraTonTai = "v2_DM_PhongBan_KiemTraTonTai";

        #endregion

        #region Function

        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên phòng ban
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(int? CoQuanID)
        {
            var Result = new BaseResultModel();
            List<DanhMucPhongBanMOD> Data = new List<DanhMucPhongBanMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                /*new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),*/
                new SqlParameter(pCoQuanID,SqlDbType.Int),
                //new SqlParameter(pTotalRow,SqlDbType.Int),

            };
            //parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            //parameters[1].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            //parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[0].Value = CoQuanID ?? Convert.DBNull;
            //parameters[4].Direction = ParameterDirection.Output;
            //parameters[4].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSach, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucPhongBanMOD item = new DanhMucPhongBanMOD();
                        item.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        item.TenPhongBan = Utils.ConvertToString(dr["TenPhongBan"], string.Empty);
                        item.SoDienThoai = Utils.ConvertToString(dr["SoDienThoai"], string.Empty);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                //var TotalRow = Utils.ConvertToInt32(parameters[4].Value, 0);
                Result.Status = 1;
                Result.Data = Data;
                //Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }


        /// <summary>
        /// lấy chi tiết thông tin của 1 danh mục phòng ban theo id
        /// </summary>
        /// <param name="PhongBanID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? PhongBanID)
        {
            var Result = new BaseResultModel();
            DanhMucPhongBanMOD data = new DanhMucPhongBanMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pPhongBanID,SqlDbType.Int,25),
            };
            parameters[0].Value = PhongBanID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        data.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        data.SoDienThoai = Utils.ConvertToString(dr["SoDienThoai"], string.Empty);
                        data.TenPhongBan = Utils.ConvertToString(dr["TenPhongBan"], string.Empty);
                        data.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        data.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
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
        /// kiểm tra tồn tại của danh mục phòng ban thông qua sđt hoặc tên (trùng sđt hoặc trùng tên)
        /// , nếu Type=1 và PhongBanID = null thì kiểm tra trùng sđt cho trường hợp thêm mới
        /// , nếu Type=2 và PhongBanID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và PhongBanID != null thì kiểm tra trùng sđt cho trường hợp Cập nhật
        /// , nếu Type=2 và PhongBanID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và PhongBanID != null thì kiểm tra tồn tại theo PhongBanID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? PhongBanID, int? CoQuanID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pPhongBanID,SqlDbType.Int),
                new SqlParameter(pCoQuanID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = PhongBanID ?? Convert.DBNull;
            parameters[3].Value = CoQuanID ?? Convert.DBNull;
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
        /// Thêm mới phòng ban
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhMucPhongBanThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pSoDienThoai, SqlDbType.VarChar),
                    new SqlParameter(pTenPhongBan, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pCoQuanID, SqlDbType.Int),
                };
                parameters[0].Value = item.SoDienThoai.Trim();
                parameters[1].Value = item.TenPhongBan.Trim();
                parameters[2].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                parameters[3].Value = item.CoQuanID;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoi, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới phòng ban thành công!";
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
        /// cập nhật thông tin danh mục phòng ban
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucPhongBanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pPhongBanID, SqlDbType.Int),
                    new SqlParameter(pSoDienThoai, SqlDbType.VarChar),
                    new SqlParameter(pTenPhongBan, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pCoQuanID, SqlDbType.Int),
                };
                parameters[0].Value = item.PhongBanID;
                parameters[1].Value = item.SoDienThoai.Trim();
                parameters[2].Value = item.TenPhongBan.Trim();
                parameters[3].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                parameters[4].Value = item.CoQuanID;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhat, parameters);
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhat, parameters);
                            Console.WriteLine(Result.Status);
                            trans.Commit();
                            Result.Message = "Cập nhật phòng ban thành công!";
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
        /// xóa danh mục phòng ban
        /// </summary>
        /// <param name="PhongBanID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int PhongBanID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pPhongBanID, SqlDbType.Int)
            };
            parameters[0].Value = PhongBanID;
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
                            Result.Message = "Không thể xóa danh mục phòng ban!";
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
            Result.Message = "Xóa danh mục phòng bàn thành công!";
            return Result;
        }

        #endregion
    }
}
