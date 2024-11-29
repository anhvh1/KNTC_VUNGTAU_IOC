using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class BuocXacMinh
    {
        #region Database query string
        private const string SELECT_ALL = @"DM_BuocXuLy_GetAll";
        private const string SELECT_BY_ID = @"DM_BuocXuLy_GetByID";
        private const string DELETE = @"DM_BuocXacMinh_Delete";
        private const string UPDATE = @"DM_BuocXacMinh_Update";
        private const string INSERT = @"DM_BuocXacMinh_Insert";
        private const string GET_BYLOAIKHIEUTOID = @"DM_BuocXacMinh_GetByLoaiKhieuToID";
        private const string GET_FILEDMBUOCXACMINH = @"DM_BuocXacMinh_GetFileDanhMucBuocXacMinh";

        private const string GET_BY_LOAIKHIEUTO = "DM_BuocXacMinh_GetByLoaiKhieuTo";
        private const string GET_BY_SEARCH = "DM_BuocXacMinh_GetBySearch";
        private const string COUNT_SEARCH = "DM_BuocXacMinh_CountSearch";
        #endregion

        #region paramaters constant
        private const string PARM_BUOCXACMINHID = @"BuocXacMinhID";
        private const string PARM_TENBUOC = @"TenBuoc";
        private const string PARM_LOAIDON = @"LoaiDon";
        private const string PARM_LOAIKHIEUTOID = @"LoaiKhieuToID";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_LOAIFILE = @"LoaiFile";

        private const string PARM_ISDINHKEMFILE = "@IsDinhKemFile";
        private const string PARM_GHICHU = "@GhiChu";
        private const string PARM_FILEURL = "@FileUrl";
        private const string PARM_ORDERBY = "@OrderBy";

        private const string PARM_START = "@Start";
        private const string PARM_END = "@End";
        private const string PARM_KEYWORD = "@Keyword";

        private const string PARM_SUDUNG = @"SuDung";
        #endregion

        private BuocXacMinhInfo GetData(SqlDataReader rdr)
        {
            BuocXacMinhInfo buocXacMinhInfo = new BuocXacMinhInfo();
            buocXacMinhInfo.BuocXacMinhID = Utils.GetInt32(rdr["BuocXacMinhID"], 0);
            buocXacMinhInfo.TenBuoc = Utils.GetString(rdr["TenBuoc"], String.Empty);
            buocXacMinhInfo.LoaiDon = Utils.GetInt32(rdr["LoaiDon"], 0);
            buocXacMinhInfo.IsDinhKemFile = Utils.GetBoolean(rdr["IsDinhKemFile"], false);
            buocXacMinhInfo.SuDung = Utils.GetBoolean(rdr["SuDung"], false);
            buocXacMinhInfo.GhiChu = Utils.GetString(rdr["GhiChu"], String.Empty);
            buocXacMinhInfo.FileUrl = Utils.GetString(rdr["FileUrl"], String.Empty);
            buocXacMinhInfo.OrderBy = Utils.ConvertToInt32(rdr["OrderBy"], 0);
            buocXacMinhInfo.SuDung = Utils.GetBoolean(rdr["SuDung"], false);
            return buocXacMinhInfo;
        }

        private BuocXacMinhInfo GetFileData(SqlDataReader rdr)
        {
            BuocXacMinhInfo buocXacMinhInfo = new BuocXacMinhInfo();
            buocXacMinhInfo.FileDanhMucBuocXacMinhID = Utils.GetInt32(rdr["FileDanhMucBuocXacMinhID"], 0);
            buocXacMinhInfo.BuocXacMinhID = Utils.GetInt32(rdr["DMBuocXacMinhID"], 0);
            buocXacMinhInfo.TenFile = Utils.GetString(rdr["TenFile"], String.Empty);
            buocXacMinhInfo.NguoiUp = Utils.GetInt32(rdr["NguoiUp"], 0);
            buocXacMinhInfo.FileUrl = Utils.GetString(rdr["FileURL"], String.Empty);
            buocXacMinhInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            buocXacMinhInfo.NgayUp = Utils.GetDateTime(rdr["NgayUp"], DateTime.MinValue);
            buocXacMinhInfo.NgayUp_Str = Format.FormatDate(buocXacMinhInfo.NgayUp);
            if (buocXacMinhInfo.IsBaoMat == false)
            {
                buocXacMinhInfo.IsBaoMatInt = 1;
            }
            else
            {
                buocXacMinhInfo.IsBaoMatInt = 2;
            }
            return buocXacMinhInfo;
        }
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_TENBUOC, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_LOAIDON, SqlDbType.TinyInt),
                new SqlParameter(PARM_ISDINHKEMFILE, SqlDbType.Bit),
                new SqlParameter(PARM_GHICHU, SqlDbType.NVarChar),
                new SqlParameter(PARM_FILEURL, SqlDbType.NVarChar, 2000),
                new SqlParameter(PARM_ORDERBY, SqlDbType.Int),
                new SqlParameter(PARM_SUDUNG, SqlDbType.Bit),
            };
            return parms;
        }
        private void SetInsertParms(SqlParameter[] parms, BuocXacMinhInfo buocXacMinhInfo)
        {
            parms[0].Value = buocXacMinhInfo.TenBuoc;
            parms[1].Value = buocXacMinhInfo.LoaiDon;
            parms[2].Value = buocXacMinhInfo.IsDinhKemFile;
            parms[3].Value = buocXacMinhInfo.GhiChu;
            parms[4].Value = buocXacMinhInfo.FileUrl;
            parms[5].Value = buocXacMinhInfo.OrderBy;
            parms[6].Value = buocXacMinhInfo.SuDung;
        }
        private SqlParameter[] GetUpdateParms()
        {
            List<SqlParameter> parms = GetInsertParms().ToList();
            parms.Insert(0, new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int));
            return parms.ToArray();
        }
        private void SetUpdateParms(SqlParameter[] parms, BuocXacMinhInfo buocXacMinhInfo)
        {
            parms[0].Value = buocXacMinhInfo.BuocXacMinhID;
            parms[1].Value = buocXacMinhInfo.TenBuoc;
            parms[2].Value = buocXacMinhInfo.LoaiDon;
            parms[3].Value = buocXacMinhInfo.IsDinhKemFile;
            parms[4].Value = buocXacMinhInfo.GhiChu;
            parms[5].Value = buocXacMinhInfo.FileUrl;
            parms[6].Value = buocXacMinhInfo.OrderBy;
            parms[7].Value = buocXacMinhInfo.SuDung;
        }
        public BuocXacMinhInfo GetByID(int trangthaixulyID)
        {
            BuocXacMinhInfo buocXacMinhInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int) };
            parameters[0].Value = trangthaixulyID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_ID, parameters))
            {
                if (dr.Read())
                {
                    buocXacMinhInfo = GetData(dr);
                    buocXacMinhInfo.listFileDMBuocXacMinh = new List<BuocXacMinhInfo>();
                    buocXacMinhInfo.listFileDMBuocXacMinh = GetListFileByID(trangthaixulyID, (int)EnumLoaiFile.FileBCXM);
                }
                dr.Close();
            }
            return buocXacMinhInfo;
        }
        public List<BuocXacMinhInfo> GetListFileByID(int trangthaixulyID, int fileType)
        {
            List<BuocXacMinhInfo> buocXacMinhInfo = new List<BuocXacMinhInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int),
            new SqlParameter(PARM_LOAIFILE, SqlDbType.Int),

            };
            parameters[0].Value = trangthaixulyID;
            parameters[1].Value = fileType;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_FILEDMBUOCXACMINH, parameters))
            {
                while (dr.Read())
                {
                    BuocXacMinhInfo item = new BuocXacMinhInfo();
                    item = GetFileData(dr);
                    buocXacMinhInfo.Add(item);
                }
                dr.Close();
            }
            return buocXacMinhInfo;
        }
        public int CountSearch(String keyword)
        {
            int result = 0;
            SqlParameter parm = new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200);
            parm.Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parm))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();

                }
            }
            catch (Exception e)
            {
                throw;
            }
            return result;
        }

        public IList<BuocXacMinhInfo> GetBySearch(int start, int end, String keyword)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200)
            };

            parms[0].Value = start;
            parms[1].Value = end;
            parms[2].Value = keyword;

            IList<BuocXacMinhInfo> trangthaixulys = new List<BuocXacMinhInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parms))
                {
                    while (dr.Read())
                    {
                        BuocXacMinhInfo trangThaiXuLyInfo = GetData(dr);
                        trangThaiXuLyInfo.TenLoaiDon = Utils.ConvertToString(dr["TenLoaiKhieuTo"], String.Empty);
                        if (trangThaiXuLyInfo.IsDinhKemFile == true)
                        {
                            trangThaiXuLyInfo.IsDinhKemFile_str = "Có";
                        }
                        else
                        {
                            trangThaiXuLyInfo.IsDinhKemFile_str = "Không";
                        }
                        trangthaixulys.Add(trangThaiXuLyInfo);
                        trangThaiXuLyInfo.OrderBy_Str = trangThaiXuLyInfo.OrderBy == 0 ? "" : trangThaiXuLyInfo.OrderBy.ToString();
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return trangthaixulys;
        }
        public IList<BuocXacMinhInfo> GetByLoaiKhieuTo(int loaidon)
        {
            SqlParameter parm = new SqlParameter(PARM_LOAIDON, SqlDbType.Int);
            parm.Value = loaidon;
            IList<BuocXacMinhInfo> trangthaixulys = new List<BuocXacMinhInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_LOAIKHIEUTO, parm))
            {
                while (dr.Read())
                {
                    BuocXacMinhInfo buocXacMinhInfo = GetData(dr);
                    trangthaixulys.Add(buocXacMinhInfo);
                }
                dr.Close();
            }
            return trangthaixulys;
        }
        public int Delete(int buocxacminhID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_BUOCXACMINHID, SqlDbType.Int) };
            parameters[0].Value = buocxacminhID;
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
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }
        public int Update(BuocXacMinhInfo buocXacMinhInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, buocXacMinhInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE, parameters);
                        trans.Commit();
                        val = 1;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        val = 0;
                        throw;
                    }
                }
                conn.Close();
            }
            return val;
        }
        public int Insert(BuocXacMinhInfo buocXacMinhInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, buocXacMinhInfo);
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
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }

        public IList<BuocXacMinhInfo> GetByLoaiKhieuToID(int xuLyDonID)
        {
            SqlParameter parm = new SqlParameter(PARM_XULYDONID, SqlDbType.Int);
            parm.Value = xuLyDonID;
            IList<BuocXacMinhInfo> lstItem = new List<BuocXacMinhInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BYLOAIKHIEUTOID, parm))
            {
                while (dr.Read())
                {
                    BuocXacMinhInfo buocXacMinhInfo = GetData(dr);
                    lstItem.Add(buocXacMinhInfo);
                }
                dr.Close();
            }
            return lstItem;
        }

    }
}
