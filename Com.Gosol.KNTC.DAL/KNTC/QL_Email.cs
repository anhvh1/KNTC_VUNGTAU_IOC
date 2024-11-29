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
    public class QL_Email
    {
        private QL_EmailInfo GetData(SqlDataReader dr)
        {
            QL_EmailInfo info = new QL_EmailInfo();
            var a = dr["Active"];
            info.EmailID = Utils.GetInt32(dr["EmailID"], 0);
            info.LoaiEmailID = Utils.GetInt32(dr["LoaiEmailID"], 0);
            info.NgayTao = Utils.GetDateTime(dr["NgayTao"], DateTime.MinValue);
            info.NoiDungEmail = Utils.GetString(dr["NoiDungEmail"], string.Empty);
            info.Active = Utils.GetBoolean(dr["Active"], false);

            return info;
        }

        public List<QL_EmailInfo> GetBySear(string key, int page, int pagesize, int? LoaiEmailID)
        {
            List<QL_EmailInfo> result = new List<QL_EmailInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pKeyword", SqlDbType.NVarChar),
                new SqlParameter("pPage", SqlDbType.Int),
                new SqlParameter("pPagesize", SqlDbType.Int),
                new SqlParameter("pLoaiEmailID", SqlDbType.Int),
            };
            parameters[0].Value = key;
            parameters[1].Value = page;
            parameters[2].Value = pagesize;
            parameters[3].Value = LoaiEmailID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_QL_Email_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        QL_EmailInfo info = GetData(dr);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        info.TenEmail = Utils.GetString(dr["TenEmail"], string.Empty);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }
        public int CountSear(string key)
        {
            List<QL_EmailInfo> result = new List<QL_EmailInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pKeyword", SqlDbType.NVarChar),
            };
            parameters[0].Value = key;
            int count = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_Email_CountSearch", parameters))
                {
                    while (dr.Read())
                    {
                        QL_EmailInfo info = new QL_EmailInfo();
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            count = result[0].CountTotal;

            return count;
        }

        public QL_EmailInfo GetByID(int ID)
        {
            QL_EmailInfo info = new QL_EmailInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pEmailID", SqlDbType.Int),
            };
            parameters[0].Value = ID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_Email_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        info = GetData(dr);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        info.TenEmail = Utils.GetString(dr["TenEmail"], string.Empty);
                        //result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return info;
        }

        public int Delete(int ID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pEmailID", SqlDbType.Int) };
            parameters[0].Value = ID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_Email_Delete", parameters);
                        trans.Commit();
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

        public int Update(QL_EmailInfo info)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pEmailID", SqlDbType.Int),
                new SqlParameter("pNgayTao", SqlDbType.DateTime),
                new SqlParameter("pNoiDungEmail", SqlDbType.NVarChar),
                new SqlParameter("pActive", SqlDbType.Bit),
                new SqlParameter("pLoaiEmailID", SqlDbType.Int)
            };

            parameters[0].Value = info.EmailID;
            parameters[1].Value = info.NgayTao;
            parameters[2].Value = info.NoiDungEmail;
            parameters[3].Value = info.Active;
            parameters[4].Value = info.LoaiEmailID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_Email_Update", parameters);
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

        public int Insert(QL_EmailInfo info)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("pNgayTao", SqlDbType.DateTime),
                new SqlParameter("pNoiDungEmail", SqlDbType.NVarChar),
                new SqlParameter("pActive", SqlDbType.Bit),
                new SqlParameter("pLoaiEmailID", SqlDbType.Int)
            };

            parameters[0].Value = info.NgayTao;
            parameters[1].Value = info.NoiDungEmail;
            parameters[2].Value = info.Active;
            parameters[3].Value = info.LoaiEmailID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "QL_Email_Insert", parameters);
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

        public QL_EmailInfo GetByLoaiEmail(int ID)
        {
            QL_EmailInfo info = new QL_EmailInfo();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pLoaiEmailID", SqlDbType.Int),
            };
            parameters[0].Value = ID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_Email_GetByLoaiEmail", parameters))
                {
                    while (dr.Read())
                    {
                        info = GetData(dr);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        info.TenEmail = Utils.GetString(dr["TenEmail"], string.Empty);
                        string kk = info.NoiDungEmail.Replace(System.Environment.NewLine, "<br />");
                        info.NoiDungEmail = kk;
                        //result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return info;
        }

        public List<QL_DmEmailInfo> GetLoaiEmail(string key, int page, int pagesize)
        {
            List<QL_DmEmailInfo> result = new List<QL_DmEmailInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("pKeyword", SqlDbType.NVarChar),
                new SqlParameter("pPage", SqlDbType.Int),
                new SqlParameter("pPagesize", SqlDbType.Int),
            };
            parameters[0].Value = key;
            parameters[1].Value = page;
            parameters[2].Value = pagesize;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "QL_DmEmail_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        QL_DmEmailInfo info = new QL_DmEmailInfo();
                        info.LoaiEmailID = Utils.GetInt32(dr["LoaiEmailID"], 0);
                        info.TenEmail = Utils.GetString(dr["TenEmail"], string.Empty);
                        info.CountTotal = Utils.GetInt32(dr["CountTotal"], 0);
                        result.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
    }
}
