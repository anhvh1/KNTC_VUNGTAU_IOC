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

//Create by NamNH - 14/10/2022


namespace Com.Gosol.KNTC.DAL.DanhMuc
{

    public class DanhMucThamQuyenDAL
    {
        #region Variable
        private const string pThamQuyenID = "@ThamQuyenID";
        private const string pMaThamQuyen = "@MaThamQuyen";
        private const string pTenThamQuyen = "@TenThamQuyen";
        private const string pGhiChu = "@GhiChu";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_ThamQuyen_DanhSach";
        private const string sThemMoi = "v2_DM_ThamQuyen_ThemMoi";
        private const string sChiTiet = "v2_DM_ThamQuyen_ChiTiet";
        private const string sCapNhat = "v2_DM_ThamQuyen_CapNhat";
        private const string sXoa = "v2_DM_ThamQuyen_Xoa";
        private const string sKiemTraTonTai = "v2_DM_ThamQuyen_KiemTraTonTai";

        #endregion


        #region Function

        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên thẩm quyền
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<DanhMucThamQuyenMOD> Data = new List<DanhMucThamQuyenMOD>();
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
                        DanhMucThamQuyenMOD item = new DanhMucThamQuyenMOD();
                        item.ThamQuyenID = Utils.ConvertToInt32(dr["ThamQuyenID"], 0);
                        item.MaThamQuyen = Utils.ConvertToString(dr["MaThamQuyen"], string.Empty);
                        item.TenThamQuyen = Utils.ConvertToString(dr["TenThamQuyen"], string.Empty);
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
        /// lấy chi tiết thông tin của 1 danh mục thẩm quyền theo id
        /// </summary>
        /// <param name="ThamQuyenID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? ThamQuyenID)
        {
            var Result = new BaseResultModel();
            DanhMucThamQuyenMOD data = new DanhMucThamQuyenMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pThamQuyenID,SqlDbType.Int,25),
            };
            parameters[0].Value = ThamQuyenID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        data.ThamQuyenID = Utils.ConvertToInt32(dr["ThamQuyenID"], 0);
                        data.MaThamQuyen = Utils.ConvertToString(dr["MaThamQuyen"], string.Empty);
                        data.TenThamQuyen = Utils.ConvertToString(dr["TenThamQuyen"], string.Empty);
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
        /// kiểm tra tồn tại của danh mục thẩm quyền thông qua mã hoặc tên (trùng mã hoặc trùng tên)
        /// , nếu Type=1 và ThamQuyenID = null thì kiểm tra trùng mã cho trường hợp thêm mới
        /// , nếu Type=2 và ThamQuyenID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và ThamQuyenID != null thì kiểm tra trùng mã cho trường hợp Cập nhật
        /// , nếu Type=2 và ThamQuyenID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và ThamQuyenID != null thì kiểm tra tồn tại theo ThamQuyenID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? ThamQuyenID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pThamQuyenID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = ThamQuyenID ?? Convert.DBNull;
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
        /// Thêm mới thẩm quyền
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhMucThamQuyenThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pMaThamQuyen, SqlDbType.VarChar),
                    new SqlParameter(pTenThamQuyen, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.MaThamQuyen.Trim();
                parameters[1].Value = item.TenThamQuyen.Trim();
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
                            Result.Message = "Thêm mới thẩm quyền thành công!";
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
        /// cập nhật thông tin danh mục thẩm quyền
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucThamQuyenMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pThamQuyenID, SqlDbType.Int),
                    new SqlParameter(pMaThamQuyen, SqlDbType.VarChar),
                    new SqlParameter(pTenThamQuyen, SqlDbType.NVarChar),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.ThamQuyenID;
                parameters[1].Value = item.MaThamQuyen.Trim();
                parameters[2].Value = item.TenThamQuyen.Trim();
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
                            Result.Message = "Cập nhật thẩm quyền thành công!";
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
        /// xóa danh mục thẩm quyền
        /// </summary>
        /// <param name="ThamQuyenID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int ThamQuyenID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pThamQuyenID, SqlDbType.Int)
            };
            parameters[0].Value = ThamQuyenID;
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
                            Result.Message = "Không thể xóa danh mục thẩm quyền!";
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
            Result.Message = "Xóa danh mục thẩm quyền thành công!";
            return Result;
        }

        #endregion

    }
}
