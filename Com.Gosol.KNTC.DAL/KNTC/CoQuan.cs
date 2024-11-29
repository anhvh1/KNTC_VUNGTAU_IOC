using Com.Gosol.KNTC.Models.BaoCao;
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
    public class CoQuan
    {
        #region Database query string

        private const string SELECT_ALL = @"DM_CoQuan_GetAll";
        private const string SELECT_COQUAN_BYCANBOID = @"DM_CoQuan_GetByCanBoID";
        private const string SELECT_ALL_FOR_AJAX = @"DM_CoQuan_GetAllForAjax";
        private const string GETCOQUAN_BY_COQUANID_FOR_AJAX = @"DM_CoQuan_GetCoQuanForAjax";
        private const string SELECT_BY_ID = @"DM_CoQuan_GetByID";
        private const string SELECT_BY_PARENTID = @"DM_CoQuan_GetByParentID";
        private const string GET_ALL_COQUAN = @"DM_CoQuan_GetAll";
        private const string DELETE = @"DM_CoQuan_Delete";
        private const string UPDATE = @"DM_CoQuan_Update";
        private const string INSERT = @"DM_CoQuan_Insert";
        private const string GET_BY_CAP = @"DM_CoQuan_GetByCap";
        private const string GET_BY_CAP_FOR_BC = @"DM_CoQuan_GetByCapForBC";
        private const string GET_LIST_CQGQ = @"DM_CoQuan_GetDSCoQuanGQ";

        private const string SELECT_ALL_HAVE_NULL = @"DM_CoQuan_GetAllHaveNull";

        private const string GET_PARENTS = @"DM_CoQuan_GetParents";
        private const string GET_CHILRENTS = @"DM_CoQuan_GetChilrents";
        private const string GET_PARENTS_BY_TINH = @"DM_CoQuan_GetParentsByTinh";

        private const string GET_BY_HUYEN = @"DM_CoQuan_GetByHuyen";
        private const string GET_BY_XA = @"DM_CoQuan_GetByXa";

        private const string GET_COQUAN_DA_GIAIQUYET = @"DM_CoQuan_GetDaGiaiQuyet";

        // for chia tach, sap nhap co quan
        private const string GET_BY_SEARCH = @"DM_CoQuan_GetBySearch";
        private const string COUNT_SEARCH = @"DM_CoQuan_CountSearch";
        private const string UPDATE_DISABLE = @"DM_CoQuan_UpdateDisable";

        private const string DM_COQUAN_GETALLFORCHART = @"DM_CoQuan_GetAllForChart";
        #endregion

        #region paramaters constant

        private const string PARM_COQUANID = @"CoQuanID";
        private const string PARM_TENCOQUAN = @"TenCoQuan";
        private const string PARM_COQUANCHA = @"CoQuanChaID";
        private const string PARM_THAMQUYENID = @"ThamQuyenID";
        private const string PARM_TINHID = @"TinhID";
        private const string PARM_HUYENID = @"HuyenID";
        private const string PARM_XAID = @"XaID";

        private const string PARM_CAP_ID = @"CapID";

        private const string PARM_CAPUBND = "@CapUBND";
        private const string PARM_CAPTHANHTRA = "@CapThanhTra";
        private const string PARM_SUDUNGPM = "@SuDungPM";
        private const string PARM_MACQ = @"MaCQ";
        private const string PARM_SUDUNGQUYTRINH = @"SuDungQuyTrinh";
        private const string PARM_SUDUNGQUYTRINHGQ = @"SuDungQuyTrinhGQ";
        private const string PARM_QUYTRINHVANTHUTIEPNHAN = @"QuyTrinhVanThuTiepNhan";
        private const string PARM_QUYTRINHVANTHUTIEPDAN = @"@QuyTrinhVanThuTiepDan";
        private const string PARM_CQCO_HIEULUC = @"CQCoHieuLuc";
        private const string PARM_CANBOID = @"CanBoID";
        //private const string PARM_VALID = @"Valid";
        private const string PARM_DISABLE = @"Disable";
        private const string PARM_TT_CHIATACH_SAPNHAP = @"TTChiaTachSapNhap";
        private const string PARM_CHIATACH_SAPNHAP_DENCQID = @"ChiaTachSapNhapDenCQID";

        private const string PARM_KEYWORD = @"KeyWord";
        private const string PARM_START = @"Start";
        private const string PARM_END = @"End";

        #endregion

        private CoQuanInfo GetData(SqlDataReader rdr)
        {
            CoQuanInfo coQuanInfo = new CoQuanInfo();
            coQuanInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            coQuanInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], String.Empty);

            coQuanInfo.CoQuanChaID = Utils.GetInt32(rdr["CoQuanChaID"], 0);
            //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
            coQuanInfo.CapID = Utils.ConvertToInt32(rdr["CapID"], 0);
            coQuanInfo.ThamQuyenID = Utils.GetInt32(rdr["ThamQuyenID"], 0);
            coQuanInfo.TinhID = Utils.GetInt32(rdr["TinhID"], 0);
            coQuanInfo.HuyenID = Utils.GetInt32(rdr["HuyenID"], 0);
            coQuanInfo.XaID = Utils.GetInt32(rdr["XaID"], 0);

            coQuanInfo.CapUBND = Utils.ConvertToBoolean(rdr["CapUBND"], false);
            coQuanInfo.CapThanhTra = Utils.ConvertToBoolean(rdr["CapThanhTra"], false);
            coQuanInfo.SuDungPM = Utils.ConvertToBoolean(rdr["SuDungPM"], false);

            coQuanInfo.MaCQ = Utils.ConvertToString(rdr["MaCQ"], String.Empty);
            //coQuanInfo.Valid = Utils.GetInt32(rdr["Valid"], 0);
            return coQuanInfo;
        }

        private ApiGateway.objMapping.ObjMapInfo GetMapData(SqlDataReader rdr)
        {
            ApiGateway.objMapping.ObjMapInfo coQuanInfo = new ApiGateway.objMapping.ObjMapInfo();
            coQuanInfo.Ma = Utils.GetInt32(rdr["Ma"], 0);
            coQuanInfo.Ten = Utils.GetString(rdr["Ten"], String.Empty);
            coQuanInfo.MappingCode = Utils.ConvertToString(rdr["MappingCode"], String.Empty);
            return coQuanInfo;
        }

        private CoQuanInfo GetDataSearch(SqlDataReader rdr)
        {
            CoQuanInfo coQuanInfo = new CoQuanInfo();
            coQuanInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            coQuanInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], String.Empty);

            coQuanInfo.CoQuanChaID = Utils.GetInt32(rdr["CoQuanChaID"], 0);
            //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
            coQuanInfo.CapID = Utils.GetInt32(rdr["CapID"], 0);
            coQuanInfo.ThamQuyenID = Utils.GetInt32(rdr["ThamQuyenID"], 0);
            coQuanInfo.TinhID = Utils.GetInt32(rdr["TinhID"], 0);
            coQuanInfo.HuyenID = Utils.GetInt32(rdr["HuyenID"], 0);
            coQuanInfo.XaID = Utils.GetInt32(rdr["XaID"], 0);
            return coQuanInfo;
        }

        private CoQuanInfo GetDataForAjax(SqlDataReader rdr)
        {
            CoQuanInfo coQuanInfo = new CoQuanInfo();
            coQuanInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            coQuanInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], String.Empty);

            coQuanInfo.CoQuanChaID = Utils.GetInt32(rdr["CoQuanChaID"], 0);
            //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
            coQuanInfo.CapID = Utils.GetInt32(rdr["CapID"], 0);
            coQuanInfo.ThamQuyenID = Utils.GetInt32(rdr["ThamQuyenID"], 0);
            coQuanInfo.TinhID = Utils.GetInt32(rdr["TinhID"], 0);
            coQuanInfo.HuyenID = Utils.GetInt32(rdr["HuyenID"], 0);
            coQuanInfo.XaID = Utils.GetInt32(rdr["XaID"], 0);

            coQuanInfo.CapUBND = Utils.ConvertToBoolean(rdr["CapUBND"], false);
            coQuanInfo.CapThanhTra = Utils.ConvertToBoolean(rdr["CapThanhTra"], false);
            coQuanInfo.SuDungPM = Utils.ConvertToBoolean(rdr["SuDungPM"], false);

            //coQuanInfo.TenDayDu = Utils.GetString(rdr["TenDayDu"], String.Empty);

            coQuanInfo.hasChild = Utils.GetInt32(rdr["hasChild"], 0);

            //coQuanInfo.Valid = Utils.GetInt32(rdr["Valid"], 0);
            return coQuanInfo;
        }

        #region -- insert param
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                //new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_TENCOQUAN, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_COQUANCHA, SqlDbType.Int),
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int),
                new SqlParameter(PARM_THAMQUYENID, SqlDbType.Int),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter(PARM_HUYENID, SqlDbType.Int),
                new SqlParameter(PARM_XAID, SqlDbType.Int),
                new SqlParameter(PARM_CAPUBND, SqlDbType.Int),
                new SqlParameter(PARM_CAPTHANHTRA, SqlDbType.Int),
                new SqlParameter(PARM_SUDUNGPM, SqlDbType.Int),
                new SqlParameter(PARM_MACQ, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_SUDUNGQUYTRINH, SqlDbType.Bit),
                new SqlParameter(PARM_SUDUNGQUYTRINHGQ, SqlDbType.Bit),
                new SqlParameter(PARM_QUYTRINHVANTHUTIEPNHAN, SqlDbType.Bit),
                new SqlParameter(PARM_QUYTRINHVANTHUTIEPDAN, SqlDbType.Bit),
                new SqlParameter(PARM_CQCO_HIEULUC, SqlDbType.Bit)
                //new SqlParameter(PARM_TENDAYDU, SqlDbType.NVarChar, 150)
                
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, CoQuanInfo coQuanInfo)
        {
            parms[0].Value = coQuanInfo.TenCoQuan;
            if (coQuanInfo.CoQuanChaID != 0) parms[1].Value = coQuanInfo.CoQuanChaID;
            else parms[1].Value = DBNull.Value;
            if (coQuanInfo.CapID == 0)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = coQuanInfo.CapID;
            if (coQuanInfo.ThamQuyenID == 0)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = coQuanInfo.ThamQuyenID;
            if (coQuanInfo.TinhID == 0)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = coQuanInfo.TinhID;
            if (coQuanInfo.HuyenID == 0)
                parms[5].Value = DBNull.Value;
            else parms[5].Value = coQuanInfo.HuyenID;

            if (coQuanInfo.XaID == 0)
                parms[6].Value = DBNull.Value;
            else parms[6].Value = coQuanInfo.XaID;


            parms[7].Value = coQuanInfo.CapUBND;
            parms[8].Value = coQuanInfo.CapThanhTra;
            parms[9].Value = coQuanInfo.SuDungPM;
            parms[10].Value = coQuanInfo.MaCQ;
            parms[11].Value = coQuanInfo.SuDungQuyTrinh;
            parms[12].Value = coQuanInfo.SuDungQuyTrinhGQ;
            parms[13].Value = coQuanInfo.QuyTrinhVanThuTiepNhan;
            parms[14].Value = coQuanInfo.QTVanThuTiepDan;
            parms[15].Value = coQuanInfo.CQCoHieuLuc;

        }
        #endregion

        #region -- update param
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_TENCOQUAN, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_COQUANCHA, SqlDbType.Int),
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int),
                new SqlParameter(PARM_THAMQUYENID, SqlDbType.Int),
                new SqlParameter(PARM_TINHID, SqlDbType.Int),
                new SqlParameter(PARM_HUYENID, SqlDbType.Int),
                new SqlParameter(PARM_XAID, SqlDbType.Int),
                new SqlParameter(PARM_CAPUBND, SqlDbType.Int),
                new SqlParameter(PARM_CAPTHANHTRA, SqlDbType.Int),
                new SqlParameter(PARM_SUDUNGPM, SqlDbType.Int),
                new SqlParameter(PARM_MACQ, SqlDbType.NVarChar,100),
                new SqlParameter(PARM_SUDUNGQUYTRINH, SqlDbType.Bit),
                new SqlParameter(PARM_SUDUNGQUYTRINHGQ, SqlDbType.Bit),
                new SqlParameter(PARM_QUYTRINHVANTHUTIEPNHAN, SqlDbType.Bit),
                new SqlParameter(PARM_QUYTRINHVANTHUTIEPDAN, SqlDbType.Bit),
                new SqlParameter(PARM_CQCO_HIEULUC, SqlDbType.Bit)
            };
            return parms;
        }

        private void SetUpdateParms(SqlParameter[] parms, CoQuanInfo coQuanInfo)
        {
            parms[0].Value = coQuanInfo.CoQuanID;
            parms[1].Value = coQuanInfo.TenCoQuan;
            if (coQuanInfo.CoQuanChaID != 0)
            {
                parms[2].Value = coQuanInfo.CoQuanChaID;
            }
            else parms[2].Value = DBNull.Value;
            if (coQuanInfo.CapID == 0)
                parms[3].Value = DBNull.Value;
            else
                parms[3].Value = coQuanInfo.CapID;
            if (coQuanInfo.ThamQuyenID == 0)
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = coQuanInfo.ThamQuyenID;
            if (coQuanInfo.TinhID == 0)
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = coQuanInfo.TinhID;
            if (coQuanInfo.HuyenID == 0)
                parms[6].Value = DBNull.Value;
            else parms[6].Value = coQuanInfo.HuyenID;

            if (coQuanInfo.XaID == 0)
                parms[7].Value = DBNull.Value;
            else parms[7].Value = coQuanInfo.XaID;

            parms[8].Value = coQuanInfo.CapUBND;
            parms[9].Value = coQuanInfo.CapThanhTra;
            parms[10].Value = coQuanInfo.SuDungPM;
            parms[11].Value = coQuanInfo.MaCQ;
            parms[12].Value = coQuanInfo.SuDungQuyTrinh;
            parms[13].Value = coQuanInfo.SuDungQuyTrinhGQ;
            parms[14].Value = coQuanInfo.QuyTrinhVanThuTiepNhan;
            parms[15].Value = coQuanInfo.QTVanThuTiepDan;
            parms[16].Value = coQuanInfo.CQCoHieuLuc;
        }
        #endregion

        public IList<CoQuanInfo> GetAllForChart()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, DM_COQUAN_GETALLFORCHART, null))
                {
                    while (dr.Read())
                    {
                        CoQuanInfo coQuanInfo = new CoQuanInfo();
                        coQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        coQuanInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], String.Empty);
                        coQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        coQuanInfo.CoQuanChaID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                        coQuanInfo.Level = Utils.ConvertToInt32(dr["Level"], 0);
                        coQuanInfo.CapUBND = Utils.ConvertToBoolean(dr["CapUBND"], false);
                        coQuanInfo.SuDungPM = Utils.ConvertToBoolean(dr["SuDungPM"], false);
                        coQuans.Add(coQuanInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetParents()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_PARENTS, null))
                {
                    while (dr.Read())
                    {
                        CoQuanInfo coQuanInfo = GetData(dr);
                        coQuans.Add(coQuanInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetChilrents()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CHILRENTS, null))
                {
                    while (dr.Read())
                    {
                        CoQuanInfo coQuanInfo = GetData(dr);
                        coQuans.Add(coQuanInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetParents(int tinhID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter parm = new SqlParameter(PARM_TINHID, SqlDbType.Int);
            parm.Value = tinhID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_PARENTS_BY_TINH, parm))
                {
                    while (dr.Read())
                    {
                        CoQuanInfo coQuanInfo = GetData(dr);
                        coQuans.Add(coQuanInfo);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetCoQuanDaGiaiQuyet()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_COQUAN_DA_GIAIQUYET, null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        //check dia chi da ton tai
        private const string CHECK_EXISTS_COQUAN = @"DM_CoQuan_CheckExistsName";
        public Boolean checkExistsCoQuan(string tenCoQuan, int coQuanCha, int coQuanId)
        {
            bool valid = false;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_TENCOQUAN, SqlDbType.NVarChar),
                new SqlParameter(PARM_COQUANCHA, SqlDbType.Int),
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = tenCoQuan;
            parameters[1].Value = coQuanCha;
            parameters[2].Value = coQuanId;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, CHECK_EXISTS_COQUAN, parameters))
            {
                while (dr.Read())
                {
                    valid = Utils.GetInt32(dr["isExists"], 0) > 0 ? true : false;
                }
                dr.Close();
            }
            return valid;
        }

        public IList<CoQuanInfo> GetCoQuans()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL, null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        // tuan them lay all, bo dk disble
        public IList<CoQuanInfo> GetCoQuans_All()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetAll_SNCT", null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetCoQuanForAjax()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL_FOR_AJAX, null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetDataForAjax(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetCoQuanByCoQuanID(int CoQuanID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETCOQUAN_BY_COQUANID_FOR_AJAX, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetDataForAjax(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        public IList<CoQuanInfo> GetCoQuanByCoQuanID_New(int CoQuanID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetByID", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = new CoQuanInfo();
                    coQuanInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                    coQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);

                    coQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    coQuanInfo.CapID = Utils.GetInt32(dr["CapID"], 0);
                    coQuanInfo.ThamQuyenID = Utils.GetInt32(dr["ThamQuyenID"], 0);
                    coQuanInfo.TinhID = Utils.GetInt32(dr["TinhID"], 0);
                    coQuanInfo.HuyenID = Utils.GetInt32(dr["HuyenID"], 0);
                    coQuanInfo.XaID = Utils.GetInt32(dr["XaID"], 0);
                    coQuanInfo.SuDungQuyTrinhGQ = Utils.ConvertToBoolean(dr["SuDungQuyTrinhGQ"], false);
                    coQuanInfo.CapUBND = Utils.ConvertToBoolean(dr["CapUBND"], false);
                    coQuanInfo.CapThanhTra = Utils.ConvertToBoolean(dr["CapThanhTra"], false);
                    coQuanInfo.SuDungPM = Utils.ConvertToBoolean(dr["SuDungPM"], false);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        //Dia Chi Suggestion
        private const string COQUAN_SUGGESTION = @"DM_CoQuan_GetSuggestion";
        public IList<CoQuanInfo> GetCoQuanSuggestion(string tenCoQuan)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_TENCOQUAN, SqlDbType.NVarChar)
            };
            parameters[0].Value = tenCoQuan;

            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COQUAN_SUGGESTION, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        //Dia Chi Search
        private const string COQUAN_SEARCH = @"DM_CoQuan_GetCoQuan_Search";
        public IList<CoQuanInfo> GetCoQuanSearch(string keySearch)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_TENCOQUAN, SqlDbType.NVarChar)
            };
            parameters[0].Value = keySearch;

            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COQUAN_SEARCH, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetDataSearch(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetCoQuanByParentID(int coQuanID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_PARENTID, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetDataForAjax(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        public IList<CoQuanInfo> GetListCoQuanGQbyID(int coQuanID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_LIST_CQGQ, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetAllCoQuan()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_COQUAN, null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuanInfo.BanTiepDan = Utils.ConvertToBoolean(dr["BanTiepDan"], false);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<ApiGateway.objMapping.ObjMapInfo> GetForMapping(string TypeApi)
        {
            string strStore = string.Empty;
            switch (TypeApi)
            {
                case "CQ":
                case "BN":
                    strStore = "SyncApi_CoQuan_GetForMapping";
                    break;
                case "KT":
                    strStore = "SyncApi_LoaiKhieuTo_GetForMapping";
                    break;
                //case "QD":
                //	strStore = "8";
                //	break;
                //case "TQ":
                //	strStore = "9";
                //	break;
                case "KQ":
                    strStore = "SyncApi_LoaiKetQua_GetForMapping";
                    break;
                case "ND":
                    strStore = "SyncApi_NguonDonDen_GetForMapping";
                    break;
                case "DMT":
                    strStore = "SyncApi_Tinh_GetForMapping";
                    break;
                case "DMH":
                    strStore = "SyncApi_Huyen_GetForMapping";
                    break;
                case "DMX":
                    strStore = "SyncApi_Xa_GetForMapping";
                    break;
                case "DT":
                    strStore = "SyncApi_DanToc_GetForMapping";
                    break;
                case "QG":
                    strStore = "SyncApi_QuocTich_GetForMapping";
                    break;
                case "GQ":
                    strStore = "SyncApi_HuongGiaiQuyet_GetForMapping";
                    break;
            }
            IList<ApiGateway.objMapping.ObjMapInfo> coQuans = new List<ApiGateway.objMapping.ObjMapInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, strStore, null))
            {
                while (dr.Read())
                {
                    ApiGateway.objMapping.ObjMapInfo coQuanInfo = GetMapData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public CoQuanInfo GetCoQuanByID(int coQuanID)
        {
            CoQuanInfo coQuanInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DM_CoQuan_GetByID", parameters))
            {
                if (dr.Read())
                {
                    coQuanInfo = GetData(dr);
                    coQuanInfo.QTVanThuTiepDan = Utils.ConvertToBoolean(dr["QTVanThuTiepDan"], false);
                    coQuanInfo.QuyTrinhVanThuTiepNhan = Utils.ConvertToBoolean(dr["QTVanThuTiepNhanDon"], false);
                    coQuanInfo.SuDungQuyTrinh = Utils.ConvertToBoolean(dr["SuDungQuyTrinh"], false);
                    coQuanInfo.SuDungQuyTrinhGQ = Utils.ConvertToBoolean(dr["SuDungQuyTrinhGQ"], false);
                    coQuanInfo.CQCoHieuLuc = Utils.ConvertToBoolean(dr["CQCoHieuLuc"], false);
                    coQuanInfo.Disable = Utils.ConvertToBoolean(dr["Disable"], false);
                    coQuanInfo.TTChiaTachSapNhap = Utils.ConvertToInt32(dr["TTChiaTachSapNhap"], 0);
                    coQuanInfo.ChiaTachSapNhapDenCQID = Utils.ConvertToInt32(dr["ChiaTachSapNhapDenCQID"], 0);
                    coQuanInfo.NgayThucHien = Utils.ConvertToNullableDateTime(dr["NgayThucHien"], null);
                    coQuanInfo.TenNguoiThucHien = Utils.ConvertToString(dr["TenNguoiThucHien"], string.Empty);
                    coQuanInfo.BanTiepDan = Utils.ConvertToBoolean(dr["BanTiepDan"], false);

                }
                dr.Close();
            }
            return coQuanInfo;
        }

        public CoQuanInfo GetCoQuanChaByCoQuanID(int coQuanID)
        {
            CoQuanInfo coQuanInfo = new CoQuanInfo();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int)
            };
            parameters[0].Value = coQuanID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"GetCoQuanCha_ByCoQuanID", parameters))
            {
                if (dr.Read())
                {
                    coQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    coQuanInfo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                    coQuanInfo.CoQuanChaID = coQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                    coQuanInfo.TenCoQuanCha = Utils.ConvertToString(dr["TenCoQuanCha"], string.Empty);
                }
                dr.Close();
            }
            return coQuanInfo;
        }

        public int Delete(int coQuanID)
        {
            //object val = 0;
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_COQUANID, SqlDbType.Int) };
            parameters[0].Value = coQuanID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, DELETE, parameters);
                        result = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE, parameters);
                        if (result > 0)
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch
                    {
                        trans.Rollback();
                        //throw;
                        result = -2;
                    }
                }
                conn.Close();
            }
            //return Utils.ConvertToInt32(val, -1);
            return result;
        }

        public int Update(CoQuanInfo coQuanInfo)
        {
            object val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, coQuanInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE, parameters);
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


        public int Insert(CoQuanInfo coQuanInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, coQuanInfo);
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

        public IList<CoQuanInfo> GetCoQuanByCap(int capID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int)
            };
            parameters[0].Value = capID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_CAP, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetAllHaveNull()
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_ALL_HAVE_NULL, null))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }


        private const string SELECT_BY_TINHID = @"DM_CoQuan_GetByTinhID";
        public IList<CoQuanInfo> GetCoQuanByTinhID(int tinhID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parameters[0].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_TINHID, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public List<CoQuanInfo> GetCoQuanByTinhID1(int tinhID)
        {
            List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parameters[0].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_BY_TINHID, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo CoQuanInfo = new CoQuanInfo();
                    //CoQuanInfo coQuanInfo = GetData(dr);
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    CoQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    CoQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    CoQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                    coQuans.Add(CoQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        public IList<CoQuanInfo> GetCoQuanBySoID(int SoID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@SoID", SqlDbType.Int)
            };
            parameters[0].Value = SoID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetBySoID", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        public List<CoQuanInfo> GetCoQuanByHuyenID(int HuyenID)
        {
            List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@HuyenID", SqlDbType.Int)
            };
            parameters[0].Value = HuyenID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetByHuyenID", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        public List<CoQuanInfo> GetCoQuanByXaID(int XaID)
        {
            List<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@XaID", SqlDbType.Int)
            };
            parameters[0].Value = XaID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetByXaID", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetByHuyen(int huyenID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_HUYENID, SqlDbType.Int)
            };
            parameters[0].Value = huyenID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_HUYEN, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetByXa(int xaID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_XAID, SqlDbType.Int)
            };
            parameters[0].Value = xaID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XA, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public CoQuanInfo GetCoQuanByCanBoID(int canBoID)
        {
            CoQuanInfo coQuanInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CANBOID, SqlDbType.Int)
            };
            parameters[0].Value = canBoID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, SELECT_COQUAN_BYCANBOID, parameters))
            {
                if (dr.Read())
                {
                    coQuanInfo = GetData(dr);
                    coQuanInfo.QTVanThuTiepDan = Utils.ConvertToBoolean(dr["QTVanThuTiepDan"], false);
                    coQuanInfo.QuyTrinhVanThuTiepNhan = Utils.ConvertToBoolean(dr["QTVanThuTiepNhanDon"], false);
                    coQuanInfo.SuDungQuyTrinh = Utils.ConvertToBoolean(dr["SuDungQuyTrinh"], false);
                    coQuanInfo.SuDungQuyTrinhGQ = Utils.ConvertToBoolean(dr["SuDungQuyTrinhGQ"], false);
                    coQuanInfo.CQCoHieuLuc = Utils.ConvertToBoolean(dr["CQCoHieuLuc"], false);
                    coQuanInfo.Disable = Utils.ConvertToBoolean(dr["Disable"], false);
                    coQuanInfo.TTChiaTachSapNhap = Utils.ConvertToInt32(dr["TTChiaTachSapNhap"], 0);
                    coQuanInfo.ChiaTachSapNhapDenCQID = Utils.ConvertToInt32(dr["ChiaTachSapNhapDenCQID"], 0);

                }
                dr.Close();
            }
            return coQuanInfo;
        }


        #region -- chia tach, sap nhap co quan
        public IList<CoQuanInfo> GetBySearch(String keyword, int start, int end)
        {
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int)
            };

            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;

            IList<CoQuanInfo> coQuanList = new List<CoQuanInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parm))
                {
                    while (dr.Read())
                    {
                        CoQuanInfo coQuanInfo = GetData(dr);
                        coQuanInfo.TenCoQuanCha = Utils.ConvertToString(dr["TenCoQuanCha"], String.Empty);
                        coQuanInfo.Disable = Utils.ConvertToBoolean(dr["Disable"], false);
                        coQuanInfo.TTChiaTachSapNhap = Utils.ConvertToInt32(dr["TTChiaTachSapNhap"], 0);
                        coQuanInfo.TenCQChiaTachSapNhapDen = Utils.ConvertToString(dr["TenCQChiaTachSapNhapDen"], String.Empty);
                        coQuanList.Add(coQuanInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return coQuanList;
        }

        public int CountSearch(String keyword)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200)
            };

            parm[0].Value = keyword;

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

        private SqlParameter[] GetUpdateDisableParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                new SqlParameter(PARM_DISABLE, SqlDbType.Bit),
                new SqlParameter(PARM_TT_CHIATACH_SAPNHAP, SqlDbType.Int),
                new SqlParameter(PARM_CHIATACH_SAPNHAP_DENCQID, SqlDbType.Int),
            };
            return parms;
        }

        private void SetUpdateDisableParms(SqlParameter[] parms, CoQuanInfo coQuanInfo)
        {
            parms[0].Value = coQuanInfo.CoQuanID;
            parms[1].Value = coQuanInfo.Disable;
            parms[2].Value = coQuanInfo.TTChiaTachSapNhap;
            parms[3].Value = coQuanInfo.ChiaTachSapNhapDenCQID;
        }

        public int UpdateDisable(CoQuanInfo coQuanInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateDisableParms();
            SetUpdateDisableParms(parameters, coQuanInfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATE_DISABLE, parameters);
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
        #endregion

        #region -- get for bc tinh tinh td,xld,gqd
        public IList<BCTinhHinhTD_XLD_GQInfo> GetCoQuanByCapForBC(int capID, int tinhID)
        {
            IList<BCTinhHinhTD_XLD_GQInfo> coQuans = new List<BCTinhHinhTD_XLD_GQInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int),
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parameters[0].Value = capID;
            parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_CAP_FOR_BC, parameters))
            {
                while (dr.Read())
                {
                    BCTinhHinhTD_XLD_GQInfo coQuanInfo = new BCTinhHinhTD_XLD_GQInfo();
                    coQuanInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                    coQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    coQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    coQuanInfo.HuyenID = Utils.GetInt32(dr["HuyenID"], 0);
                    coQuanInfo.CapID = capID;
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        public IList<CoQuanInfo> GetCoQuanByCapAndTinhID(int capID, int tinhID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int),
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parameters[0].Value = capID;
            parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_CAP_FOR_BC, parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }

        /// <summary>
        /// lấy cả những cơ quan ko trong hệ thống phần mềm
        /// </summary>
        /// <param name="capID"></param>
        /// <param name="tinhID"></param>
        /// <returns></returns>
        public IList<CoQuanInfo> GetCoQuanByCapAndTinhID_All(int capID, int tinhID)
        {
            IList<CoQuanInfo> coQuans = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CAP_ID, SqlDbType.Int),
                new SqlParameter(PARM_TINHID, SqlDbType.Int)
            };
            parameters[0].Value = capID;
            parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CoQuan_GetByCapForBC_All", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo coQuanInfo = GetData(dr);
                    coQuans.Add(coQuanInfo);
                }
                dr.Close();
            }
            return coQuans;
        }
        #endregion
        public List<CoQuanInfo> GetAllCapCon(int? CoQuanID)
        {
            List<CoQuanInfo> List = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("CoQuanID", SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID ?? Convert.DBNull;
            //parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMuc_CoQuanDonVi_GetAllCapCon", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo CoQuanInfo = new CoQuanInfo();
                    //CoQuanInfo coQuanInfo = GetData(dr);
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    CoQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    CoQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    CoQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                    CoQuanInfo.SuDungPM = Utils.ConvertToBoolean(dr["SuDungPM"], false);
                    List.Add(CoQuanInfo);
                }
                dr.Close();
            }
            return List;

        }

        public List<CoQuanInfo> GetAllCoQuanTiepNhan(int? CoQuanID)
        {
            List<CoQuanInfo> List = new List<CoQuanInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("CoQuanID", SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID ?? Convert.DBNull;
            //parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_GetAllCoQuanChuyenDon", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo CoQuanInfo = new CoQuanInfo();
                    //CoQuanInfo coQuanInfo = GetData(dr);
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    CoQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    CoQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    CoQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                    CoQuanInfo.SuDungPM = Utils.ConvertToBoolean(dr["SuDungPM"], false);
                    List.Add(CoQuanInfo);
                }
                dr.Close();
            }
            return List;

        }
        // lấy ra cơ quan  cao nhất chính là TinhID truyền vào
        public CoQuanInfo GetCoQuanByTinhID_New(int? TinhID)
        {
            CoQuanInfo CoQuanInfo = new CoQuanInfo();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("TinhID", SqlDbType.Int)
            };
            parameters[0].Value = TinhID ?? Convert.DBNull;
            //parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMuc_CoQuanDonVi_GetCoQuanbyTinh", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo = new CoQuanInfo();
                    //CoQuanInfo coQuanInfo = GetData(dr);
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    CoQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    CoQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    CoQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                    //List.Add(CoQuanInfo);
                }
                dr.Close();
            }
            return CoQuanInfo;
        }
        public CoQuanInfo GetCoQuanByHuyenID_New(int? HuyenID, int? TinhID, int? CoQuanID)
        {
            CoQuanInfo CoQuanInfo = new CoQuanInfo();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("HuyenID", SqlDbType.Int),
                new SqlParameter("TinhID", SqlDbType.Int),
                 new SqlParameter("CoQuanID", SqlDbType.Int)
            };
            parameters[0].Value = HuyenID ?? Convert.DBNull;
            parameters[1].Value = TinhID ?? Convert.DBNull;
            parameters[2].Value = CoQuanID ?? Convert.DBNull;
            //parameters[1].Value = tinhID;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_DanhMuc_CoQuanDonVi_GetCoQuanbyHuyen", parameters))
            {
                while (dr.Read())
                {
                    CoQuanInfo = new CoQuanInfo();
                    //CoQuanInfo coQuanInfo = GetData(dr);
                    CoQuanInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                    CoQuanInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                    CoQuanInfo.CoQuanChaID = Utils.GetInt32(dr["CoQuanChaID"], 0);
                    //coQuanInfo.Cap = Utils.GetInt32(rdr["Cap"], 0);
                    CoQuanInfo.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                    //List.Add(CoQuanInfo);
                }
                dr.Close();
            }
            return CoQuanInfo;
        }
    }
}
