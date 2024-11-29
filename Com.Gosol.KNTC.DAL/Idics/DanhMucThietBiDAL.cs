using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.Idics;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.Idics
{
    public class DanhMucThietBiDAL
    {
        public List<DanhMucThietBiModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<DanhMucThietBiModel> Result = new List<DanhMucThietBiModel>();

            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("Keyword",SqlDbType.NVarChar),
                new SqlParameter("OrderByName",SqlDbType.NVarChar),
                new SqlParameter("OrderByOption",SqlDbType.NVarChar),
                new SqlParameter("pLimit",SqlDbType.Int),
                new SqlParameter("pOffset",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),
                new SqlParameter("StartDate",SqlDbType.DateTime),
                new SqlParameter("Status",SqlDbType.Int),
              };

            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName ?? Convert.DBNull;
            parameters[2].Value = p.OrderByOption ?? Convert.DBNull;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = p.TuNgay ?? Convert.DBNull;
            parameters[7].Value = p.Status ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMucThietBi_GetPagingBySearch_New", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucThietBiModel item = new DanhMucThietBiModel();
                        item.MachineId = Utils.ConvertToString(dr["MachineId"], string.Empty);
                        item.AgencyId = Utils.ConvertToString(dr["AgencyId"], string.Empty);
                        item.StaticIP = Utils.ConvertToString(dr["StaticIP"], string.Empty);
                        item.DynamicIP = Utils.ConvertToString(dr["DynamicIP"], string.Empty);
                        item.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        item.StartDate = Utils.ConvertToNullableDateTime(dr["StartDate"], null);
                        item.EditDate = Utils.ConvertToNullableDateTime(dr["EditDate"], null);
                        item.EditBy = Utils.ConvertToString(dr["EditBy"], string.Empty);
                        item.Location = Utils.ConvertToString(dr["Location"], string.Empty);
                        item.SecretKey = Utils.ConvertToString(dr["SecretKey"], string.Empty);
                        Result.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public BaseResultModel Insert(DanhMucThietBiModel DanhMucThietBiModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucThietBiModel == null || DanhMucThietBiModel.MachineId == null || DanhMucThietBiModel.MachineId.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "MachineId không được trống";
                    return Result;
                }
                else
                {
                    var crThietBi = GetByID(DanhMucThietBiModel.MachineId);
                    if (crThietBi != null && crThietBi.MachineId != null && crThietBi.MachineId.Length > 0)
                    {
                        Result.Status = 0;
                        Result.Message = "Thiết bị đã tồn tại";
                        return Result;
                    }
                    else
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("MachineId", SqlDbType.NVarChar),
                            new SqlParameter("AgencyId", SqlDbType.NVarChar),
                            new SqlParameter("StaticIP", SqlDbType.NVarChar),
                            new SqlParameter("DynamicIP", SqlDbType.NVarChar),
                            new SqlParameter("Status", SqlDbType.Bit),
                            new SqlParameter("StartDate", SqlDbType.DateTime),
                            new SqlParameter("EditDate", SqlDbType.DateTime),
                            new SqlParameter("EditBy", SqlDbType.NVarChar),
                            new SqlParameter("Location", SqlDbType.NVarChar),
                            new SqlParameter("SecretKey", SqlDbType.NVarChar),
                        };
                        parameters[0].Value = DanhMucThietBiModel.MachineId.Trim();
                        parameters[1].Value = DanhMucThietBiModel.AgencyId ?? Convert.DBNull;
                        parameters[2].Value = DanhMucThietBiModel.StaticIP ?? Convert.DBNull;
                        parameters[3].Value = DanhMucThietBiModel.DynamicIP ?? Convert.DBNull;
                        parameters[4].Value = DanhMucThietBiModel.Status ?? Convert.DBNull;
                        parameters[5].Value = DanhMucThietBiModel.StartDate ?? Convert.DBNull;
                        parameters[6].Value = DanhMucThietBiModel.EditDate ?? Convert.DBNull;
                        parameters[7].Value = DanhMucThietBiModel.EditBy ?? Convert.DBNull;
                        parameters[8].Value = DanhMucThietBiModel.Location ?? Convert.DBNull;
                        parameters[9].Value = DanhMucThietBiModel.SecretKey ?? Convert.DBNull;

                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMucThietBi_Insert", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Insert_Success("danh mục thiết bị");
                                }
                                catch (Exception ex)
                                {
                                    Result.Status = -1;
                                    Result.Message = ConstantLogMessage.API_Error_System;
                                    trans.Rollback();
                                    throw ex;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
            return Result;
        }
        public BaseResultModel Update(DanhMucThietBiModel DanhMucThietBiModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (DanhMucThietBiModel == null || DanhMucThietBiModel.MachineId == null || DanhMucThietBiModel.MachineId.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "MachineId không được trống";
                    return Result;
                }
                else if (DanhMucThietBiModel.MachineId.Trim().Length > 50)
                {
                    Result.Status = 0;
                    Result.Message = "MachineId không được quá 50 ký tự";
                    return Result;
                }
                else
                {
                    var crObj = GetByID(DanhMucThietBiModel.MachineId);
                    if (crObj != null && crObj.MachineId != null && crObj.MachineId.Length > 0)
                    { 
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                            new SqlParameter("MachineId", SqlDbType.NVarChar),
                            new SqlParameter("AgencyId", SqlDbType.NVarChar),
                            new SqlParameter("StaticIP", SqlDbType.NVarChar),
                            new SqlParameter("DynamicIP", SqlDbType.NVarChar),
                            new SqlParameter("Status", SqlDbType.Bit),
                            new SqlParameter("StartDate", SqlDbType.DateTime),
                            new SqlParameter("EditDate", SqlDbType.DateTime),
                            new SqlParameter("EditBy", SqlDbType.NVarChar),
                            new SqlParameter("Location", SqlDbType.NVarChar),
                            new SqlParameter("SecretKey", SqlDbType.NVarChar),
                        };

                        parameters[0].Value = DanhMucThietBiModel.MachineId.Trim();
                        parameters[1].Value = DanhMucThietBiModel.AgencyId ?? Convert.DBNull;
                        parameters[2].Value = DanhMucThietBiModel.StaticIP ?? Convert.DBNull;
                        parameters[3].Value = DanhMucThietBiModel.DynamicIP ?? Convert.DBNull;
                        parameters[4].Value = DanhMucThietBiModel.Status ?? Convert.DBNull;
                        parameters[5].Value = DanhMucThietBiModel.StartDate ?? Convert.DBNull;
                        parameters[6].Value = DanhMucThietBiModel.EditDate ?? Convert.DBNull;
                        parameters[7].Value = DanhMucThietBiModel.EditBy ?? Convert.DBNull;
                        parameters[8].Value = DanhMucThietBiModel.Location ?? Convert.DBNull;
                        parameters[9].Value = DanhMucThietBiModel.SecretKey ?? Convert.DBNull;

                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    Result.Status = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_DanhMucThietBi_Update", parameters);
                                    trans.Commit();
                                    Result.Message = ConstantLogMessage.Alert_Update_Success("danh mục thiết bị");
                                    return Result;
                                }
                                catch
                                {
                                    Result.Status = -1;
                                    Result.Message = ConstantLogMessage.API_Error_System;
                                    trans.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        Result.Status = 0;
                        Result.Message = "Thiết bị không tồn tại";
                        return Result;

                    }
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw ex;
            }
        }
        public DanhMucThietBiModel GetByID(string MachineId)
        {
            if (MachineId == null || (MachineId != null && MachineId.Length == 0))
            {
                return new DanhMucThietBiModel();
            }
            DanhMucThietBiModel thietBi = null;
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("MachineId",SqlDbType.NVarChar)
              };
            parameters[0].Value = MachineId;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMucThietBi_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        thietBi = new DanhMucThietBiModel();
                        thietBi.MachineId = Utils.ConvertToString(dr["MachineId"], string.Empty);
                        thietBi.AgencyId = Utils.ConvertToString(dr["AgencyId"], string.Empty);
                        thietBi.StaticIP = Utils.ConvertToString(dr["StaticIP"], string.Empty);
                        thietBi.DynamicIP = Utils.ConvertToString(dr["DynamicIP"], string.Empty);
                        thietBi.Status = Utils.ConvertToBoolean(dr["Status"], false);
                        thietBi.StartDate = Utils.ConvertToNullableDateTime(dr["StartDate"], null);
                        thietBi.EditDate = Utils.ConvertToNullableDateTime(dr["EditDate"], null);
                        thietBi.EditBy = Utils.ConvertToString(dr["EditBy"], string.Empty);
                        thietBi.Location = Utils.ConvertToString(dr["Location"], string.Empty);
                        thietBi.SecretKey = Utils.ConvertToString(dr["SecretKey"], string.Empty);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return thietBi;
        }
    }
}
