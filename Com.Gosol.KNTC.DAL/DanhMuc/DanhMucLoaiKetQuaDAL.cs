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

//Create by Duc - 17/10/2022


namespace Com.Gosol.KNTC.DAL.DanhMuc
{

    public class DanhMucLoaiKetQuaDAL
    {
        #region Variable
        private const string pLoaiKetQuaID = "@LoaiKetQuaID";    
        private const string pTenLoaiKetQua = "@TenLoaiKetQua";
        private const string pMaLoaiKetQua = "@MaLoaiKetQua";
        private const string pGhiChu = "@GhiChu";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_LoaiKetQua_DanhSach";
        private const string sThemMoi = "v2_DM_LoaiKetQua_ThemMoi";
        private const string sChiTiet = "v2_DM_LoaiKetQua_ChiTiet";
        private const string sCapNhat = "v2_DM_LoaiKetQua_CapNhat";
        private const string sXoa = "v2_DM_LoaiKetQua_Xoa";
        private const string sKiemTraTonTai = "v2_DM_LoaiKetQua_KiemTraTonTai";

        #endregion


        #region Function

        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên loại kết quả
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<DanhMucLoaiKetQuaMOD> Data = new List<DanhMucLoaiKetQuaMOD>();
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
                        DanhMucLoaiKetQuaMOD item = new DanhMucLoaiKetQuaMOD();
                        item.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
                        item.MaLoaiKetQua = Utils.ConvertToString(dr["MaLoaiKetQua"], string.Empty);
                        item.TenLoaiKetQua = Utils.ConvertToString(dr["TenLoaiKetQua"], string.Empty);
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
        /// lấy chi tiết thông tin của 1 danh mục loại kết quả theo id
        /// </summary>
        /// <param name="LoaiKetQuaID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? LoaiKetQuaID)
        {
            var Result = new BaseResultModel();
            DanhMucLoaiKetQuaMOD data = new DanhMucLoaiKetQuaMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pLoaiKetQuaID,SqlDbType.Int,25),
            };
            parameters[0].Value = LoaiKetQuaID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        data.LoaiKetQuaID = Utils.ConvertToInt32(dr["LoaiKetQuaID"], 0);
                        data.MaLoaiKetQua = Utils.ConvertToString(dr["MaLoaiKetQua"], string.Empty);
                        data.TenLoaiKetQua = Utils.ConvertToString(dr["TenLoaiKetQua"], string.Empty);
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
        /// kiểm tra tồn tại của danh mục loại kết quả thông qua mã hoặc tên (trùng mã hoặc trùng tên)
        /// , nếu Type=1 và LoaiKetQuaID = null thì kiểm tra trùng mã cho trường hợp thêm mới
        /// , nếu Type=2 và LoaiKetQuaID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và LoaiKetQuaID != null thì kiểm tra trùng mã cho trường hợp Cập nhật
        /// , nếu Type=2 và LoaiKetQuaID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và LoaiKetQuaID != null thì kiểm tra tồn tại theo LoaiKetQuaID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? LoaiKetQuaID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pLoaiKetQuaID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = LoaiKetQuaID ?? Convert.DBNull;
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
        /// Thêm mới loại kết quả
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhMucLoaiKetQuaThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pTenLoaiKetQua, SqlDbType.NVarChar),
                    new SqlParameter(pMaLoaiKetQua, SqlDbType.VarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.TenLoaiKetQua.Trim();
                parameters[1].Value = item.MaLoaiKetQua.Trim();
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
                            Result.Message = "Thêm mới loại kết quả thành công!";
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
        /// cập nhật thông tin danh mục loại kết quả
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucLoaiKetQuaMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pLoaiKetQuaID, SqlDbType.Int),
                    new SqlParameter(pTenLoaiKetQua, SqlDbType.NVarChar),
                    new SqlParameter(pMaLoaiKetQua, SqlDbType.VarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.LoaiKetQuaID;
                parameters[1].Value = item.TenLoaiKetQua.Trim();
                parameters[2].Value = item.MaLoaiKetQua.Trim();
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
                            Result.Message = "Cập nhật loại kết quả thành công!";
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
        /// xóa danh mục loại kết quả
        /// </summary>
        /// <param name="LoaiKetQuaID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int LoaiKetQuaID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pLoaiKetQuaID, SqlDbType.Int)
            };
            parameters[0].Value = LoaiKetQuaID;
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
                            Result.Message = "Không thể xóa danh mục loại kết quả!";
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
            Result.Message = "Xóa danh mục loại kết quả thành công!";
            return Result;
        }

        #endregion

    }
}
