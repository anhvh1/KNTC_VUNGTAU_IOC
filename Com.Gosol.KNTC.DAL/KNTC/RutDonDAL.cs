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
    public class RutDonDAL
    {
        #region Database query string

        private const string GET_ALL = @"RutDon_GetAll";
        private const string GET_BY_ID = @"RutDon_GetByID";
        private const string DELETE = @"RutDon_Delete";
        private const string UPDATE = @"RutDon_Update";
        private const string INSERT = @"RutDon_Insert";
        private const string GETFILEQD_URL = @"RutDon_GetFileQD";
        private const string GET_BY_COQUAN = "RutDon_GetByCoQuan";
        private const string GET_BY_XULYDONID = "RutDon_GetByXuLyDonID";
        private const string GET_BY_XULYDONID_NEW = "RutDon_GetByXuLyDonID_New";
        private const string HUYRUTDON = @"RutDon_UpdateHuyRutDon";

        private const string GET_BY_SEARCH = "XuLyDon_GetSearch_RutDon";
        private const string COUNT_SEARCH = "XuLyDon_CountSearch_RutDon";

        #endregion

        #region paramaters constant

        private const string PARM_RUTDONID = @"RutDonID";
        private const string PARM_NGAYRUTDON = "@NgayRutDon";
        private const string PARM_LYDO = @"LyDo";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_FILEQD = @"FileQD";

        private const string PARM_STARTDATE = "@StartDate";
        private const string PARM_ENDDATE = "@EndDate";
        private const string PARM_COQUANID = "@CoQuanID";

        private const string PARM_LOAIKHIEUTOID = @"LoaiKhieuToID";
        private const string PARM_TUNGAY = @"TuNgay";
        private const string PARM_DENNGAY = @"DenNgay";
        private const string PARM_START = "@Start";
        private const string PARM_END = "@End";
        private const string PARM_KEYWORD = "@KeyWord";
        private const string PARM_STATENAME = "@StateName";
        private const string PARM_PREVSTATE = "@PrevState";
        private const string PARM_STATE = "@State";

        private const string PARM_COMMAND = @"Command";
        #endregion

        private RutDonInfo GetData(SqlDataReader rdr)
        {
            RutDonInfo rutDonInfo = new RutDonInfo();
            rutDonInfo.RutDonID = Utils.GetInt32(rdr["RutDonID"], 0);
            rutDonInfo.NgayRutDon = Utils.ConvertToDateTime(rdr["NgayRutDon"], DateTime.MinValue);
            rutDonInfo.LyDo = Utils.GetString(rdr["LyDo"], String.Empty);
            rutDonInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            rutDonInfo.FileQD = Utils.GetString(rdr["FileQD"], String.Empty);
            return rutDonInfo;
        }

        private RutDonInfo GetDataRutDon(SqlDataReader rdr)
        {
            RutDonInfo info = new RutDonInfo();

            info.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            info.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            info.TenChuDon = Utils.GetString(rdr["HoTen"], string.Empty);
            info.SoDon = Utils.GetString(rdr["SoDonThu"], string.Empty);
            //info.SoDon = Utils.GetInt32(rdr["SoDonThu"], 0);
            info.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);
            info.TenLoaiKhieuTo = Utils.GetString(rdr["TenLoaiKhieuTo"], String.Empty);
            info.NgayRutDon = Utils.GetDateTime(rdr["NgayRutDon"], DateTime.MinValue);
            info.LyDo = Utils.GetString(rdr["LyDo"], String.Empty);
            info.RutDonID = Utils.GetInt32(rdr["RutDonID"], 0);
            info.StateName = Utils.GetString(rdr["StateName"], String.Empty);
            info.TrangThaiRut = "";

            if (info.RutDonID != 0)
                info.TrangThaiRut = "Đã rút";
            else
                info.TrangThaiRut = "Chưa rút";

            return info;
        }

        private RutDonInfo GetFileQD_URL(SqlDataReader rdr)
        {
            RutDonInfo rutDonInfo = new RutDonInfo();
            rutDonInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            rutDonInfo.FileQD = Utils.GetString(rdr["FileQD"], String.Empty);
            return rutDonInfo;
        }

        private RutDonInfo GetCustomData(SqlDataReader rdr)
        {
            RutDonInfo rutDonInfo = new RutDonInfo();
            rutDonInfo.RutDonID = Utils.GetInt32(rdr["RutDonID"], 0);
            rutDonInfo.DonThuID = Utils.GetInt32(rdr["DonThuID"], 0);
            rutDonInfo.XuLyDonID = Utils.GetInt32(rdr["XuLyDonID"], 0);
            rutDonInfo.NgayRutDon = Utils.ConvertToDateTime(rdr["NgayRutDon"], DateTime.MinValue);
            rutDonInfo.LyDo = Utils.GetString(rdr["LyDo"], String.Empty);
            rutDonInfo.CoQuanID = Utils.ConvertToInt32(rdr["CoQuanID"], 0);
            rutDonInfo.NhomKNID = Utils.ConvertToInt32(rdr["NhomKNID"], 0);
            rutDonInfo.SoDon = Utils.ConvertToString(rdr["SoDonThu"], String.Empty);
            rutDonInfo.TenCoQuan = Utils.ConvertToString(rdr["TenCoQuan"], String.Empty);
            rutDonInfo.TenLoaiKhieuTo1 = Utils.ConvertToString(rdr["TenLoaiKhieuTo1"], String.Empty);
            rutDonInfo.TenLoaiKhieuTo2 = Utils.ConvertToString(rdr["TenLoaiKhieuTo2"], String.Empty);
            rutDonInfo.TenLoaiKhieuTo3 = Utils.ConvertToString(rdr["TenLoaiKhieuTo3"], String.Empty);
            rutDonInfo.NoiDungDon = Utils.GetString(rdr["NoiDungDon"], String.Empty);

            return rutDonInfo;
        }

        public SqlParameter[] GetParam_RutDon()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_STATE, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_COMMAND, SqlDbType.NVarChar,100)

            }; return parms;
        }


        public void SetParam_RutDon(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.Start;
            parms[6].Value = info.End;
            parms[7].Value = info.StateName;
            parms[8].Value = info.StateName2;
            parms[9].Value = info.PrevStateName;


            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        public SqlParameter[] GetParam_CountRutDon()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar,50),
                new SqlParameter(PARM_LOAIKHIEUTOID, SqlDbType.Int),
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_STATE, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_COMMAND, SqlDbType.NVarChar,100)

            }; return parms;
        }


        public void SetParam_CountRutDon(SqlParameter[] parms, QueryFilterInfo info)
        {
            parms[0].Value = info.CoQuanID;
            parms[1].Value = info.KeyWord;
            parms[2].Value = info.LoaiKhieuToID;
            parms[3].Value = info.TuNgay;
            parms[4].Value = info.DenNgay;
            parms[5].Value = info.StateName;
            parms[6].Value = info.StateName2;
            parms[7].Value = info.PrevStateName;

            if (info.TuNgay == DateTime.MinValue) parms[3].Value = DBNull.Value;
            if (info.DenNgay == DateTime.MinValue) parms[4].Value = DBNull.Value;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_NGAYRUTDON, SqlDbType.DateTime),
                new SqlParameter(PARM_LYDO, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                new SqlParameter(PARM_FILEQD, SqlDbType.NVarChar, 200),
            };
            return parms;
        }

        public RutDonInfo GetFileQDURL(int XuLyDonID)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int)

            };
            parm[0].Value = XuLyDonID;
            RutDonInfo rutdons = new RutDonInfo();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEQD_URL, parm))
                {
                    if (dr.Read())
                    {
                        rutdons = GetFileQD_URL(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return rutdons;
        }

        private void SetInsertParms(SqlParameter[] parms, RutDonInfo info)
        {
            parms[0].Value = info.NgayRutDon ?? Convert.DBNull;
            parms[1].Value = info.LyDo ?? Convert.DBNull;
            parms[2].Value = info.XuLyDonID;
            if (info.FileQD == null)
            {
                parms[3].Value = DBNull.Value;
            }
            else
            {
                parms[3].Value = info.FileQD;
            }
        }

        private SqlParameter[] GetUpdateParms()
        {
            List<SqlParameter> parms = GetInsertParms().ToList();
            parms.Insert(0, new SqlParameter(PARM_RUTDONID, SqlDbType.Int));
            return parms.ToArray();
        }

        private void SetUpdateParms(SqlParameter[] parms, RutDonInfo info)
        {
            parms[0].Value = info.RutDonID;
            parms[1].Value = info.LyDo;
            parms[2].Value = info.XuLyDonID;
            parms[3].Value = info.FileQD;
        }

        public IList<RutDonInfo> GetByCoQuan_New(List<int> ListCoQuanID, DateTime startDate, DateTime endDate)
        {
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            ListCoQuanID.ForEach(x => tbCoQuanID.Rows.Add(x));
            SqlParameter[] parm = new SqlParameter[] {
                pList,
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime)
            };
            parm[0].Value = tbCoQuanID;
            parm[1].Value = startDate;
            parm[2].Value = endDate;

            IList<RutDonInfo> rutdons = new List<RutDonInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "RutDon_GetByCoQuan_New", parm))
            {
                while (dr.Read())
                {
                    RutDonInfo rutDonInfo = GetCustomData(dr);
                    rutdons.Add(rutDonInfo);
                }
                dr.Close();
            }
            return rutdons;
        }

        public IList<RutDonInfo> GetByCoQuan(int coquanID, DateTime startDate, DateTime endDate)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_STARTDATE, SqlDbType.DateTime),
                new SqlParameter(PARM_ENDDATE, SqlDbType.DateTime)
            };
            parm[0].Value = coquanID;
            parm[1].Value = startDate;
            parm[2].Value = endDate;

            IList<RutDonInfo> rutdons = new List<RutDonInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_COQUAN, parm))
            {
                while (dr.Read())
                {
                    RutDonInfo rutDonInfo = GetCustomData(dr);
                    rutdons.Add(rutDonInfo);
                }
                dr.Close();
            }
            return rutdons;
        }

        public IList<RutDonInfo> GetAll()
        {
            IList<RutDonInfo> rutdons = new List<RutDonInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {
                    while (dr.Read())
                    {
                        RutDonInfo rutDonInfo = GetData(dr);
                        rutdons.Add(rutDonInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return rutdons;
        }

        public List<RutDonInfo> GetSearch(QueryFilterInfo info)
        {
            List<RutDonInfo> xldList = new List<RutDonInfo>();
            SqlParameter[] parms = GetParam_RutDon();
            SetParam_RutDon(parms, info);
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parms))
                {
                    while (dr.Read())
                    {
                        RutDonInfo rutDonInfo = GetDataRutDon(dr);
                        //if (rutDonInfo.StateName != Constant.Ket_Thuc)
                        xldList.Add(rutDonInfo);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return xldList;
        }

        public int CountSearch(QueryFilterInfo info)
        {
            int Count = 0;

            SqlParameter[] parms = GetParam_CountRutDon();

            SetParam_CountRutDon(parms, info);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH, parms))
                {
                    if (dr.Read())
                    {
                        Count = Utils.ConvertToInt32(dr["CountNum"], 0);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Count;
        }

        public RutDonInfo GetByID(int rutdonID)
        {
            RutDonInfo rutDonInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_RUTDONID, SqlDbType.Int) };
            parameters[0].Value = rutdonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {
                    if (dr.Read())
                    {
                        rutDonInfo = GetData(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return rutDonInfo;
        }

        public List<RutDonInfo> GetByXuLyDonID(int xldID)
        {
            List<RutDonInfo> rutDonInfo = new List<RutDonInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int) };
            parameters[0].Value = xldID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDONID_NEW, parameters))
                {
                    while (dr.Read())
                    {
                        RutDonInfo item = new RutDonInfo();
                        item = GetData(dr);
                        item.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        item.FileRutDonID = Utils.ConvertToInt32(dr["FileRutDonID"], 0);
                        item.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        item.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        item.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        item.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.TenCoQuanUp = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        //item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        item.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        item.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        item.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        item.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        item.NgayUps = Format.FormatDate(item.NgayUp);
                        rutDonInfo.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception xx)
            {
            }
            return rutDonInfo;
        }

        public int Delete(int rutdonID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_RUTDONID, SqlDbType.Int) };
            parameters[0].Value = rutdonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
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



        public int UpdateHuyRutDon(int xulydonID, string state, string command)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
            new SqlParameter(PARM_STATENAME, SqlDbType.NVarChar, 100),
            new SqlParameter(PARM_COMMAND, SqlDbType.NVarChar, 100)
            };
            parameters[0].Value = xulydonID;
            parameters[1].Value = state;
            parameters[2].Value = command;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, HUYRUTDON, parameters);
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

        public int Update(RutDonInfo rutDonInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, rutDonInfo);
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


        public int Insert(RutDonInfo rutDonInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, rutDonInfo);
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
