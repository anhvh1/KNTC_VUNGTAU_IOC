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
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

//create by NamNH - 17/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucLoaiKhieuToDAL
    {
        #region Variable
        private const string pLoaiKhieuToID = "@LoaiKhieuToID";
        private const string pTenLoaiKhieuTo = "@TenLoaiKhieuTo";
        private const string pLoaiKhieuToCha = "@LoaiKhieuToCha";
        private const string pCap = "@Cap";
        private const string pMappingCode = "@MappingCode";
        private const string pThuTu = "@ThuTu";
        private const string pSuDung = "@SuDung";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_LoaiKhieuTo_DanhSach";
        private const string sDanhSachLoaiCha = "v2_DM_LoaiKhieuTo_DanhSachLoaiCha";
        private const string sThemMoi = "v2_DM_LoaiKhieuTo_ThemMoi";
        private const string sChiTiet = "v2_DM_LoaiKhieuTo_ChiTiet";
        private const string sCapNhat = "v2_DM_LoaiKhieuTo_CapNhat";
        private const string sCapNhatSuDung = "v2_DM_LoaiKhieuTo_CapNhat_TrangThaiSuDung";
        private const string sXoa = "v2_DM_LoaiKhieuTo_Xoa";
        private const string sKiemTraTonTai = "v2_DM_LoaiKhieuTo_KiemTraTonTai";
        private const string sLoaiKhieuToCha = "v2_DM_LoaiKhieuTo_DanhSachLoaiKhieuToCha";

        #endregion

        #region Function


        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên dân tộc
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<DanhMucLoaiKhieuToExtenMOD> Data = new List<DanhMucLoaiKhieuToExtenMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),
                new SqlParameter(pTotalRow,SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSach, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiKhieuToExtenMOD item = new DanhMucLoaiKhieuToExtenMOD();
                        item.LoaiKhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"], 0);
                        item.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        item.LoaiKhieuToCha = Utils.ConvertToInt32(dr["LoaiKhieuToCha"], 0);
                        item.Cap = Utils.ConvertToInt32(dr["Cap"], 0);
                        item.MappingCode = Utils.ConvertToString(dr["MappingCode"], string.Empty);
                        item.ThuTu = Utils.ConvertToInt32(dr["ThuTu"], 0);
                        item.Highlight = Utils.ConvertToInt32(dr["Highlight"], 0);
                        item.SuDung = Utils.ConvertToBoolean(dr["SuDung"], false);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                Data.ForEach(x => x.DanhMucLoaiKhieuToCon = Data.Where(y => y.LoaiKhieuToCha == x.LoaiKhieuToID).ToList());
                Data.RemoveAll(x => x.LoaiKhieuToCha > 0);
                var TotalRow = Utils.ConvertToInt32(parameters[3].Value, 0);
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
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên dân tộc
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSachLoaiCha(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<DanhMucLoaiKhieuToCha> Data = new List<DanhMucLoaiKhieuToCha>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pTotalRow,SqlDbType.Int),

            };
            parameters[0].Direction = ParameterDirection.Output;
            parameters[0].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachLoaiCha, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucLoaiKhieuToCha item = new DanhMucLoaiKhieuToCha();
                        item.LoaiKhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"], 0);
                        item.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        item.LoaiKhieuToCha = Utils.ConvertToInt32(dr["LoaiKhieuToCha"], 0);
                        item.Cap = Utils.ConvertToInt32(dr["Cap"], 0);
                        item.MappingCode = Utils.ConvertToString(dr["MappingCode"], string.Empty);
                        item.SuDung = Utils.ConvertToBoolean(dr["SuDung"], false);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                var TotalRow = Utils.ConvertToInt32(parameters[0].Value, 0);
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
        /// lấy chi tiết thông tin của 1 danh mục loại khiếu tố theo id
        /// </summary>
        /// <param name="LoaiKhieuToID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? LoaiKhieuToID)
        {
            var Result = new BaseResultModel();
            DanhMucLoaiKhieuToExtenMOD item = new DanhMucLoaiKhieuToExtenMOD();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(pLoaiKhieuToID,SqlDbType.Int)
              };
            parameters[0].Value = LoaiKhieuToID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        item.LoaiKhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"], 0);
                        item.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        item.LoaiKhieuToCha = Utils.ConvertToInt32(dr["LoaiKhieuToCha"], 0);
                        item.Cap = Utils.ConvertToInt32(dr["Cap"], 0);
                        item.MappingCode = Utils.ConvertToString(dr["MappingCode"], string.Empty);
                        item.ThuTu = Utils.ConvertToInt32(dr["ThuTu"], 0);
                        item.SuDung = Utils.ConvertToBoolean(dr["SuDung"], false);
                        break;
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = item;
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
        /// kiểm tra tồn tại của danh mục loại khiếu tố thông qua mã hoặc tên (trùng mã hoặc trùng tên)
        /// , nếu Type=1 và LoaiKhieuToID = null thì kiểm tra trùng mã cho trường hợp thêm mới
        /// , nếu Type=2 và LoaiKhieuToID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và LoaiKhieuToID != null thì kiểm tra trùng mã cho trường hợp Cập nhật
        /// , nếu Type=2 và LoaiKhieuToID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và LoaiKhieuToID != null thì kiểm tra tồn tại theo LoaiKhieuToID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? LoaiKhieuToID, int? LoaiKhieuToCha)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pLoaiKhieuToID,SqlDbType.Int),
                new SqlParameter(pLoaiKhieuToCha,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = LoaiKhieuToID ?? Convert.DBNull;
            parameters[3].Value = LoaiKhieuToCha ?? Convert.DBNull;
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
        /// Thêm mới loiaj khiếu tố
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhSachLoaiKhieuToThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pTenLoaiKhieuTo, SqlDbType.NVarChar),
                    new SqlParameter(pLoaiKhieuToCha, SqlDbType.Int),
                    new SqlParameter(pCap, SqlDbType.TinyInt),
                    new SqlParameter(pMappingCode, SqlDbType.NVarChar),
                    new SqlParameter(pThuTu, SqlDbType.Int),
                    new SqlParameter(pSuDung, SqlDbType.Bit)
                };
                parameters[0].Value = item.TenLoaiKhieuTo;
                parameters[1].Value = item.LoaiKhieuToCha;
                parameters[2].Value = item.Cap;
                parameters[3].Value = item.MappingCode;
                parameters[4].Value = item.ThuTu ?? Convert.DBNull;
                parameters[5].Value = item.SuDung;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoi, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới loại khiếu tố thành công!";
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
        /// cập nhật thông tin danh mục loại khiếu tố
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucLoaiKhieuToMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pLoaiKhieuToID, SqlDbType.Int),
                    new SqlParameter(pTenLoaiKhieuTo, SqlDbType.NVarChar),
                    new SqlParameter(pLoaiKhieuToCha, SqlDbType.Int),
                    new SqlParameter(pCap, SqlDbType.TinyInt),
                    new SqlParameter(pMappingCode, SqlDbType.NVarChar),
                    new SqlParameter(pThuTu, SqlDbType.Int),
                    new SqlParameter(pSuDung, SqlDbType.Bit)
                };
                parameters[0].Value = item.LoaiKhieuToID;
                parameters[1].Value = item.TenLoaiKhieuTo.Trim();
                parameters[2].Value = item.LoaiKhieuToCha;
                parameters[3].Value = item.Cap;
                parameters[4].Value = item.MappingCode;
                parameters[5].Value = item.ThuTu ?? Convert.DBNull;
                parameters[6].Value = item.SuDung;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhat, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật loại khiếu tố thành công!";
                            Result.Data = Result.Status;
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = Constant.ERR_UPDATE;
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
        /// cập nhật thông tin sử dụng danh mục loại khiếu tố
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhatSuDung(DanhSachLoaiKhieuToCapNhatSuDungMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pLoaiKhieuToID, SqlDbType.Int),
                    new SqlParameter(pSuDung, SqlDbType.Bit)
                };
                parameters[0].Value = item.LoaiKhieuToID;
                parameters[1].Value = item.SuDung;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhatSuDung, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật thành công!";
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
        /// xóa danh mục loại khiếu tố
        /// </summary>
        /// <param name="LoaiKhieuToID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int LoaiKhieuToID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pLoaiKhieuToID, SqlDbType.Int)
            };
            parameters[0].Value = LoaiKhieuToID;
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
                            Result.Message = "Không thể xóa danh mục loại khiếu tố!";
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
            Result.Message = "Xóa danh mục loại khiếu tố thành công!";
            return Result;
        }


        #endregion
    }
}
