using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models.KNTC;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class PhongBan
    {
        #region Database query string

        private const string COUNT = @"DM_PhongBan_Count";
        private const string GET_ALL = @"DM_PhongBan_GetAll";
        private const string GET_BY_SEARCH = @"DM_PhongBan_GetBySearch";
        private const string GET_BY_ID = @"DM_PhongBan_GetByID";
        private const string DELETE = @"DM_PhongBan_Delete";
        private const string UPDATE = @"DM_PhongBan_Update";
        private const string INSERT = @"DM_PhongBan_Insert";

        private const string GET_BY_COQUAN_ID = "DM_PhongBan_GetByCoQuanID";

        #endregion

        #region paramaters constant

        private const string PARM_PHONGBANID = @"PhongBanID";
        private const string PARM_TENPHONGBAN = @"TenPhongBan";
        private const string PARM_SODIENTHOAI = @"SoDienThoai";
        private const string PARM_GHICHU = @"GhiChu";
        private const string PARM_COQUANID = "@CoQuanID";

        private const string PARM_START = "@Start";
        private const string PARM_END = "@End";
        private const string PARM_KEYWORD = "@Keyword";

        #endregion

        private PhongBanInfo GetData(SqlDataReader rdr)
        {
            PhongBanInfo phongBanInfo = new PhongBanInfo();
            phongBanInfo.PhongBanID = Utils.GetInt32(rdr["PhongBanID"], 0);
            phongBanInfo.TenPhongBan = Utils.GetString(rdr["TenPhongBan"], String.Empty);
            phongBanInfo.SoDienThoai = Utils.GetString(rdr["SoDienThoai"], String.Empty);
            phongBanInfo.GhiChu = Utils.GetString(rdr["GhiChu"], String.Empty);
            phongBanInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            phongBanInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], string.Empty);
            return phongBanInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_TENPHONGBAN, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_SODIENTHOAI, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            }; return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, PhongBanInfo info)
        {
            parms[0].Value = info.TenPhongBan;
            parms[1].Value = info.SoDienThoai;
            parms[2].Value = info.GhiChu;
            parms[3].Value = info.CoQuanID;
        }

        private SqlParameter[] GetUpdateParms()
        {
            //List<SqlParameter> parms = GetInsertParms().ToList();
            //parms.Insert(0, new SqlParameter(PARM_PHONGBANID, SqlDbType.Int));
            //return parms.ToArray();
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_PHONGBANID, SqlDbType.Int),
                new SqlParameter(PARM_TENPHONGBAN, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_SODIENTHOAI, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            }; return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, PhongBanInfo info)
        {
            parms[0].Value = info.PhongBanID;
            parms[1].Value = info.TenPhongBan;
            parms[2].Value = info.SoDienThoai;
            parms[3].Value = info.GhiChu;
            parms[4].Value = info.CoQuanID;
        }

        public int Count(string keyword)
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50);
            parm.Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }

        public IList<PhongBanInfo> GetByCoQuanID(int coquanID)
        {
            SqlParameter parm = new SqlParameter(PARM_COQUANID, SqlDbType.Int);
            parm.Value = coquanID;

            IList<PhongBanInfo> phongbans = new List<PhongBanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_COQUAN_ID, parm))
                {
                    while (dr.Read())
                    {
                        PhongBanInfo phongBanInfo = GetData(dr);
                        phongbans.Add(phongBanInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phongbans;
        }

        public IList<PhongBanInfo> GetAll()
        {
            IList<PhongBanInfo> phongbans = new List<PhongBanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {
                    while (dr.Read())
                    {
                        PhongBanInfo phongBanInfo = GetData(dr);
                        phongbans.Add(phongBanInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phongbans;
        }

        public IList<PhongBanInfo> GetBySearch(int start, int end, string keyword)
        {
            IList<PhongBanInfo> phongbans = new List<PhongBanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = start;
            parameters[1].Value = end;
            parameters[2].Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parameters))
                {
                    while (dr.Read())
                    {
                        PhongBanInfo phongBanInfo = GetData(dr);
                        phongbans.Add(phongBanInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phongbans;
        }

        public PhongBanInfo GetByID(int phongbanID)
        {
            PhongBanInfo phongBanInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_PHONGBANID, SqlDbType.Int) };
            parameters[0].Value = phongbanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {
                    if (dr.Read())
                    {
                        phongBanInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return phongBanInfo;
        }

        public int Delete(int phongbanID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_PHONGBANID, SqlDbType.Int)
            };
            parameters[0].Value = phongbanID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
                        trans.Commit();
                        val = 1;

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }

        public int Update(PhongBanInfo phongBanInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, phongBanInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
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
            return val;
        }


        public int Insert(PhongBanInfo phongBanInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, phongBanInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT, parameters);
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
            return Convert.ToInt32(val);
        }

    }
}
