using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Com.Gosol.KNTC.DAL.DanhMuc
{

    public class V2_SystemConFigDAL
    {
        #region Variable
        private const string pSystemConfigID = "@SystemConfigID";    
        private const string pConfigKey = "@ConfigKey";
        private const string pConfigValue = "@ConfigValue";
        private const string pDescription = "@Description";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_SystemConfig_DanhSach";
        private const string sThemMoi = "v2_HT_SystemConfig_Insert";
        private const string sChiTiet = "v2_DM_SystemConfig_ChiTiet";
        private const string sCapNhat = "v2_HT_SystemConfig_Update";
        private const string sXoa = "v2_HT_SystemConfig_Delete";
        private const string sKiemTraTonTai = "";
        private const string sLocTrangThai = "v2_DM_SystemConfig_LocTrangThai";

        #endregion


        #region Function

      
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<V2_SystemConfigMOD> Data = new List<V2_SystemConfigMOD>();
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
                        V2_SystemConfigMOD item = new V2_SystemConfigMOD();
                        item.SystemConfigID = Utils.ConvertToInt32(dr["SystemConfigID"], 0);
                        item.ConfigKey = Utils.ConvertToString(dr["ConfigKey"], string.Empty);
                        item.ConfigValue = Utils.ConvertToString(dr["ConfigValue"], string.Empty);
                        item.Description = Utils.ConvertToString(dr["Description"], string.Empty);
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


        
        public BaseResultModel ChiTiet(int? SystemConfigID)
        {
            var Result = new BaseResultModel();
            V2_SystemConfigMOD data = new V2_SystemConfigMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pSystemConfigID,SqlDbType.Int,25),
            };
            parameters[0].Value = SystemConfigID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        data.SystemConfigID = Utils.ConvertToInt32(dr["SystemConfigID"], 0);
                        data.ConfigKey = Utils.ConvertToString(dr["ConfigKey"], string.Empty);
                        data.ConfigValue = Utils.ConvertToString(dr["ConfigValue"], string.Empty);
                        data.Description = Utils.ConvertToString(dr["Description"], string.Empty);
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
       
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? SystemConfigID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pSystemConfigID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = SystemConfigID ?? Convert.DBNull;
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

        
        public BaseResultModel ThemMoi(V2_SystemConfigMODADD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pConfigKey, SqlDbType.NVarChar),
                    new SqlParameter(pConfigValue, SqlDbType.NVarChar),
                    new SqlParameter(pDescription, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.ConfigKey.Trim();
                parameters[1].Value = item.ConfigValue.Trim();
                parameters[2].Value = item.Description;
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
                            Result.Message = "Thêm mới tham số hệ thống thành công!";
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


        public BaseResultModel CapNhat(V2_SystemConfigMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pSystemConfigID, SqlDbType.Int),
                    new SqlParameter(pConfigKey, SqlDbType.NVarChar),
                    new SqlParameter(pConfigValue, SqlDbType.NVarChar),
                    new SqlParameter(pDescription, SqlDbType.NVarChar),
                    new SqlParameter(pTrangThai, SqlDbType.Bit),
                };
                parameters[0].Value = item.SystemConfigID;
                parameters[1].Value = item.ConfigKey.Trim();
                parameters[2].Value = item.ConfigValue.Trim();
                parameters[3].Value = item.Description;
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
                            Result.Message = "Cập nhật tham số hệ thành công!";
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


        public BaseResultModel Xoa(int SystemConfigID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pSystemConfigID, SqlDbType.Int)
            };
            parameters[0].Value = SystemConfigID;
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
                            Result.Message = "Không thể xóa danh mục tham số hệ thống này !";
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
            Result.Message = "Xóa tham số hệ thống thành công!";
            return Result;
        }

        #endregion

    }
}
