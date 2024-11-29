using Com.Gosol.KNTC.Model.HeThong;
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
    public class CanBo
    {
        #region Database query string

        private const string GET_ALL = @"DM_CanBo_GetAll";
        private const string GET_BY_ID = @"DM_CanBo_GetByID";
        private const string DELETE = @"DM_CanBo_Delete";
        private const string UPDATE = @"DM_CanBo_Update";
        private const string INSERT = @"DM_CanBo_Insert";
        private const string CANBO_ADD_PHONGBAN = @"DM_CanBo_AddPhongban";
        private const string GET_CB_ALL = @"DM_CanBo_GetAll_CB";

        private const string GET_BY_COQUANID = "DM_CanBo_GetByCoQuanID";
        private const string GET_TRUONGPHONG_BY_COQUANID = "DM_CanBo_GetTruongPhongByCoQuanID";
        private const string GET_CANBOGQ_BY_COQUANID = @"DM_CanBo_GetCanBoGQByCoQuanID";
        private const string GET_CBGQ_BY_COQUANID_QTDONGIAN = @"DM_CanBo_GetCanBoGQByCoQuanID_QTDonGian";


        private const string GET_BY_PAGE = @"DM_CanBo_GetByPage";
        private const string GET_ALL_JOIN = @"DM_CanBo_GetAllJoin";
        private const string GET_BY_SEARCH = @"DM_CanBo_GetBySearch";
        private const string GET_BY_COQUANCHA_ID = @"DM_CanBo_GetByCoQuanCha_ID";

        private const string GET_BY_SEARCH_QTDV = @"DM_CanBo_GetBySearch_QTDV";
        private const string GET_BY_COQUANCHA_ID_QTDV = @"DM_CanBo_GetByCoQuanCha_ID_QTDV";


        private const string COUNT_ALL = @"DM_CanBo_CountAll";
        private const string COUNT_SEARCH = @"DM_CanBo_CountSearch";
        private const string COUNT_BY_COQUANCHA_ID = @"DM_CanBo_CountByCoQuanCha_ID";
        private const string COUNT_SEARCH_QTDV = @"DM_CanBo_CountSearchQTDV";
        private const string COUNT_BY_COQUANCHA_ID_QTDV = @"DM_CanBo_CountByCoQuanCha_ID_QTDV";



        private const string GET_CANBOKY_BY_COQUANID = "DM_CanBo_GetCanBoKyByCoQuanID";
        private const string GET_CANBO_BY_PHONGBAN_ID = "DM_CanBo_GetByPhongBanID";

        private const string GETCANBO_BY_COQUANCHA_ID = @"DM_CanBo_GetCanBoBy_CoQuanChaID";
        private const string GET_LS_LANHDAO_BY_COQUAN = @"DM_CanBo_GetLsLanhDaoByCoQuan";

        #endregion

        #region paramaters constant

        private const string PARM_CANBOID = @"CanBoID";
        private const string PARM_TENCANBO = @"TenCanBo";
        private const string PARM_NGAYSINH = @"NgaySinh";
        private const string PARM_GIOITINH = @"GioiTinh";
        private const string PARM_DIACHI = @"DiaChi";
        private const string PARM_CoQuanID = @"CoQuanID";
        private const string PARM_QUYENKY = @"QuyenKy";
        private const string PARM_EMAIL = @"Email";
        private const string PARM_DIENTHOAI = "@DienThoai";
        private const string PARM_CHUC_VU_ID = @"ChucVuID";
        private const string PARM_PHONGBAN_ID = "PhongBanID";
        private const string PARM_ROLEID = "RoleID";
        private const string PARM_QUANTRIDONVI = @"QuanTriDonVi";
        private const string PARM_START = @"Start";
        private const string PARM_END = @"End";
        private const string PARM_KEYWORD = @"Keyword";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_CQSEARCH_ID = @"CoQuanSearchID";
        private const string PARM_XEMTAILIEUMAT = @"XemTaiLieuMat";
        private const string PARM_STATE = @"State";

        #endregion

        private CanBoInfo GetData(SqlDataReader rdr)
        {
            CanBoInfo canBoInfo = new CanBoInfo();
            canBoInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            canBoInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            canBoInfo.NgaySinh = Utils.GetDateTime(rdr["NgaySinh"], DateTime.Now);
            canBoInfo.GioiTinh = Utils.GetInt32(rdr["GioiTinh"], 0);
            canBoInfo.DiaChi = Utils.GetString(rdr["DiaChi"], String.Empty);
            canBoInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            canBoInfo.QuyenKy = Utils.ConvertToInt32(rdr["QuyenKy"], 0);
            canBoInfo.Email = Utils.ConvertToString(rdr["Email"], String.Empty);
            canBoInfo.DienThoai = Utils.ConvertToString(rdr["DienThoai"], String.Empty);
            canBoInfo.ChucVuID = Utils.ConvertToInt32(rdr["ChucVuID"], 0);
            canBoInfo.RoleID = Utils.ConvertToInt32(rdr["RoleID"], 0);
            canBoInfo.XemTaiLieuMat = Utils.GetBoolean(rdr["XemTaiLieuMat"], false);
            return canBoInfo;
        }


        private CanBoInfo GetAllData(SqlDataReader rdr)
        {
            CanBoInfo canBoInfo = new CanBoInfo();
            canBoInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            canBoInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            canBoInfo.NgaySinh = Utils.GetDateTime(rdr["NgaySinh"], DateTime.Now);
            canBoInfo.GioiTinh = Utils.GetInt32(rdr["GioiTinh"], 0);
            canBoInfo.DiaChi = Utils.GetString(rdr["DiaChi"], String.Empty);
            canBoInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            canBoInfo.QuyenKy = Utils.ConvertToInt32(rdr["QuyenKy"], 0);
            canBoInfo.Email = Utils.ConvertToString(rdr["Email"], String.Empty);
            canBoInfo.DienThoai = Utils.ConvertToString(rdr["DienThoai"], String.Empty);
            canBoInfo.ChucVuID = Utils.ConvertToInt32(rdr["ChucVuID"], 0);
            canBoInfo.RoleID = Utils.ConvertToInt32(rdr["RoleID"], 0);
            canBoInfo.PhongBanID = Utils.ConvertToInt32(rdr["PhongBanID"], 0);
            canBoInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], String.Empty);
            canBoInfo.RoleName = Utils.GetString(rdr["RoleName"], String.Empty);
            canBoInfo.XemTaiLieuMat = Utils.GetBoolean(rdr["XemTaiLieuMat"], false);
            //if (canBoInfo.PhongBanID != 0)
            //    canBoInfo.phongban = canBoInfo.PhongBanID.ToString();
            return canBoInfo;
        }

        private CanBoJoinInfo GetJoinData(SqlDataReader rdr)
        {
            CanBoJoinInfo canBoInfo = new CanBoJoinInfo();
            canBoInfo.CanBoID = Utils.GetInt32(rdr["CanBoID"], 0);
            canBoInfo.TenCanBo = Utils.GetString(rdr["TenCanBo"], String.Empty);
            canBoInfo.NgaySinh = Utils.GetDateTime(rdr["NgaySinh"], DateTime.Now);
            canBoInfo.GioiTinh = Utils.GetInt32(rdr["GioiTinh"], 0);
            canBoInfo.DiaChi = Utils.GetString(rdr["DiaChi"], String.Empty);
            canBoInfo.CoQuanID = Utils.GetInt32(rdr["CoQuanID"], 0);
            canBoInfo.QuyenKy = Utils.ConvertToInt32(rdr["QuyenKy"], 0);
            canBoInfo.TenCoQuan = Utils.GetString(rdr["TenCoQuan"], String.Empty);
            canBoInfo.Email = Utils.ConvertToString(rdr["Email"], String.Empty);
            canBoInfo.DienThoai = Utils.ConvertToString(rdr["DienThoai"], String.Empty);
            canBoInfo.XemTaiLieuMat = Utils.GetBoolean(rdr["XemTaiLieuMat"], false);
            return canBoInfo;
        }

        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_TENCANBO, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_NGAYSINH, SqlDbType.DateTime),
                new SqlParameter(PARM_GIOITINH, SqlDbType.Int),
                new SqlParameter(PARM_DIACHI, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int),
                new SqlParameter(PARM_QUYENKY, SqlDbType.TinyInt),
                new SqlParameter(PARM_EMAIL, SqlDbType.NVarChar, 100),
                new SqlParameter(PARM_DIENTHOAI, SqlDbType.NVarChar, 20),
                new SqlParameter(PARM_CHUC_VU_ID,SqlDbType.Int),
                new SqlParameter(PARM_ROLEID, SqlDbType.Int),
                new SqlParameter(PARM_QUANTRIDONVI, SqlDbType.Int),
                new SqlParameter(PARM_XEMTAILIEUMAT, SqlDbType.TinyInt),
                new SqlParameter(PARM_STATE, SqlDbType.TinyInt),
            };
            return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, CanBoInfo canBoInfo)
        {
            parms[0].Value = canBoInfo.TenCanBo;
            parms[1].Value = canBoInfo.NgaySinh;
            parms[2].Value = canBoInfo.GioiTinh;
            parms[3].Value = canBoInfo.DiaChi;
            parms[4].Value = canBoInfo.CoQuanID;
            parms[5].Value = canBoInfo.QuyenKy;
            parms[6].Value = canBoInfo.Email;
            parms[7].Value = canBoInfo.DienThoai;
            if ((int)parms[4].Value == 0) parms[4].Value = DBNull.Value;
            parms[8].Value = canBoInfo.ChucVuID;
            if ((int)parms[8].Value == 0) parms[8].Value = DBNull.Value;
            parms[9].Value = canBoInfo.RoleID;
            parms[10].Value = canBoInfo.QuanTriDonVi;
            parms[11].Value = canBoInfo.XemTaiLieuMat;
            parms[12].Value = canBoInfo.State;
            if (canBoInfo.State == 0)
            {
                parms[12].Value = DBNull.Value;
            }
        }

        private SqlParameter[] GetUpdateParms()
        {
            List<SqlParameter> parms = GetInsertParms().ToList();
            parms.Insert(0, new SqlParameter(PARM_CANBOID, SqlDbType.Int));
            return parms.ToArray();
        }

        private void SetUpdateParms(SqlParameter[] parms, CanBoInfo canBoInfo)
        {
            parms[0].Value = canBoInfo.CanBoID;
            parms[1].Value = canBoInfo.TenCanBo;
            parms[2].Value = canBoInfo.NgaySinh;
            parms[3].Value = canBoInfo.GioiTinh;
            parms[4].Value = canBoInfo.DiaChi;
            parms[5].Value = canBoInfo.CoQuanID;
            parms[6].Value = canBoInfo.QuyenKy;
            parms[7].Value = canBoInfo.Email;
            parms[8].Value = canBoInfo.DienThoai;
            parms[9].Value = canBoInfo.ChucVuID;
            //check don vi id = null
            if ((int)parms[5].Value == 0) parms[5].Value = DBNull.Value;
            if ((int)parms[9].Value == 0) parms[9].Value = DBNull.Value;
            parms[10].Value = canBoInfo.RoleID;
            parms[11].Value = canBoInfo.QuanTriDonVi;
            parms[12].Value = canBoInfo.XemTaiLieuMat;
            parms[13].Value = canBoInfo.State;
            if (canBoInfo.State == 0)
            {
                parms[13].Value = DBNull.Value;
            }
        }

        public int CountAll()
        {
            int result = 0;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_ALL, null))
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

        public int CountSearch(String keyword, int cqSearchID)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CQSEARCH_ID, SqlDbType.Int)
            };
            parm[0].Value = keyword;
            parm[1].Value = cqSearchID;
            //SqlParameter parm = new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200);
            //parm.Value = keyword;
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

        public int CountByCoQuanChaID(String keyword, int coQuanChaID)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int)
            };
            parm[0].Value = keyword;
            parm[1].Value = coQuanChaID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_BY_COQUANCHA_ID, parm))
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

        public IList<CanBoJoinInfo> GetCanBos()
        {
            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetByCoQuanID(int cqID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            parm.Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        //tuan them 26/09 find can bo chia tach sap nhap
        public IList<CanBoInfo> GetBySNCT(int cqID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            parm.Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_CanBo_GetBySNCT", parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetTruongPhongByCQID(int cqID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            parm.Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_TRUONGPHONG_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetCanBoGQByCQID(int cqID, int xldID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            //SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int),
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int)
            };
            parm[0].Value = cqID;
            parm[1].Value = xldID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CANBOGQ_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetCBGQByCoQuanID_QTDONGIAN(int cqID, int xldID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            //SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            //parm.Value = cqID;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int),
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int)
            };
            parm[0].Value = cqID;
            parm[1].Value = xldID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CBGQ_BY_COQUANID_QTDONGIAN, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canBoInfo.TenCoQuan = Utils.GetString(dr["TenCoQuan"], String.Empty);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetByPhongBanID(int phongbanID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            SqlParameter parm = new SqlParameter(PARM_PHONGBAN_ID, SqlDbType.Int);
            parm.Value = phongbanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CANBO_BY_PHONGBAN_ID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetCanBoKyByCoQuanID(int cqID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            parm.Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CANBOKY_BY_COQUANID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        //internal CanBoInfo GetCanBoByID(object p)
        //{
        //    throw new NotImplementedException();
        //}

        public IList<CanBoJoinInfo> GetBySearch(String keyword, int start, int end, int coQuanSearch)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CQSEARCH_ID, SqlDbType.Int)
            };
            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;
            parm[3].Value = coQuanSearch;

            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);
                        canBoInfo.NguoiDungID = Utils.GetInt32(dr["NguoiDungID"], 0);
                        if (canBoInfo.XemTaiLieuMat == true)
                        {
                            canBoInfo.XemTaiLieuMat_String = "Có";
                        }
                        else
                        {
                            canBoInfo.XemTaiLieuMat_String = "Không";
                        }
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoJoinInfo> GetByCoQuanChaID(String keyword, int start, int end, int coQuanChaID)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int)
            };
            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;
            parm[3].Value = coQuanChaID;

            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_COQUANCHA_ID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);

                        canBoInfo.NguoiDungID = Utils.GetInt32(dr["NguoiDungID"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoJoinInfo> GetCanBosJoin()
        {
            IList<CanBoJoinInfo> canboJoinList = new List<CanBoJoinInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL_JOIN, null))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo cbInfo = GetJoinData(dr);
                        canboJoinList.Add(cbInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canboJoinList;
        }

        public IList<CanBoJoinInfo> GetByPage(int start, int end)
        {
            IList<CanBoJoinInfo> canboJoinList = new List<CanBoJoinInfo>();

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int)
            };
            parm[0].Value = start;
            parm[1].Value = end;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_PAGE, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo cbInfo = GetJoinData(dr);
                        canboJoinList.Add(cbInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canboJoinList;
        }


        public CanBoInfo GetCanBoByID(int canboID)
        {
            CanBoInfo canBoInfo = null;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_CANBOID, SqlDbType.Int) };
            parameters[0].Value = canboID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {
                    if (dr.Read())
                    {
                        canBoInfo = GetData(dr);
                        canBoInfo.TenChucVu = Utils.GetString(dr["TenChucVu"], string.Empty);
                        canBoInfo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBoInfo.QuanTriDonVi = Utils.ConvertToInt32(dr["QuanTriDonVi"], 0);
                        canBoInfo.State = Utils.ConvertToInt32(dr["State"], 0);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return canBoInfo;
        }

        public int Delete(int canboID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
new SqlParameter(PARM_CANBOID, SqlDbType.Int) };
            parameters[0].Value = canboID;
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

        public int Update(CanBoInfo canBoInfo)
        {
            int val = 0;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, canBoInfo);
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


        public int Insert(CanBoInfo canBoInfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, canBoInfo);
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

        public int SetPhongBan(int canboid, int phongbanid)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                new SqlParameter(PARM_PHONGBAN_ID, SqlDbType.Int)
            };

            parms[0].Value = canboid;
            parms[1].Value = phongbanid;
            if ((int)parms[1].Value == 0) parms[1].Value = DBNull.Value;


            int result = 0;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        result = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, CANBO_ADD_PHONGBAN, parms);
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
            return result;

        }


        public IList<CanBoInfo> GetAllCanBo()
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_CB_ALL, null))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetAllData(dr);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoJoinInfo> GetCanBoByCoQuanChaID(int coQuanChaID)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int)
            };
            parm[0].Value = coQuanChaID;

            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETCANBO_BY_COQUANCHA_ID, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        #region -- for email
        public IList<CanBoInfo> GetLanhDaoCoQuan(int cqID)
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            //SqlParameter parm = new SqlParameter(PARM_CoQuanID, SqlDbType.Int);
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int)
            };
            parm[0].Value = cqID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_LS_LANHDAO_BY_COQUAN, parm))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = GetData(dr);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }
        #endregion

        public int CountSearchQuanTriDonVi(String keyword, int cqSearchID, int pQuanTriDonVi)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CQSEARCH_ID, SqlDbType.Int),
                new SqlParameter(PARM_QUANTRIDONVI, SqlDbType.Int),

            };
            parm[0].Value = keyword;
            parm[1].Value = cqSearchID;
            parm[2].Value = pQuanTriDonVi;
            //SqlParameter parm = new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200);
            //parm.Value = keyword;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_SEARCH_QTDV, parm))
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

        public int CountByCoQuanChaID_QuanTriDonVi(String keyword, int coQuanChaID, int pQuanTriDonVi)
        {
            int result = 0;
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int),
                new SqlParameter(PARM_QUANTRIDONVI, SqlDbType.Int),
            };
            parm[0].Value = keyword;
            parm[1].Value = coQuanChaID;
            parm[2].Value = pQuanTriDonVi;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, COUNT_BY_COQUANCHA_ID_QTDV, parm))
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


        public IList<CanBoJoinInfo> GetBySearch_QuanTriDonVi(String keyword, int start, int end, int coQuanSearch, int pQuanTriDonVi)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CQSEARCH_ID, SqlDbType.Int),
                new SqlParameter(PARM_QUANTRIDONVI, SqlDbType.Int)
            };
            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;
            parm[3].Value = coQuanSearch;
            parm[4].Value = pQuanTriDonVi;
            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_SEARCH_QTDV, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);
                        canBoInfo.NguoiDungID = Utils.GetInt32(dr["NguoiDungID"], 0);
                        canBoInfo.QuanTriDonVi = Utils.GetInt32(dr["QuanTriDonVi"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }



        public IList<CanBoJoinInfo> GetByCoQuanChaID_QuanTriDonVi(String keyword, int start, int end, int coQuanChaID, int pQuanTriDonVi)
        {

            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_KEYWORD, SqlDbType.NVarChar, 50),
                new SqlParameter(PARM_START, SqlDbType.Int),
                new SqlParameter(PARM_END, SqlDbType.Int),
                new SqlParameter(PARM_CoQuanID, SqlDbType.Int),
                new SqlParameter(PARM_QUANTRIDONVI, SqlDbType.Int),
            };
            parm[0].Value = keyword;
            parm[1].Value = start;
            parm[2].Value = end;
            parm[3].Value = coQuanChaID;
            parm[4].Value = pQuanTriDonVi;
            IList<CanBoJoinInfo> canbos = new List<CanBoJoinInfo>();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_COQUANCHA_ID_QTDV, parm))
                {
                    while (dr.Read())
                    {
                        CanBoJoinInfo canBoInfo = GetJoinData(dr);

                        canBoInfo.NguoiDungID = Utils.GetInt32(dr["NguoiDungID"], 0);
                        canBoInfo.QuanTriDonVi = Utils.GetInt32(dr["QuanTriDonVi"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetDanhSachLanhDao()
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DM_CanBo_GetDanhSachLanhDao", null))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = new CanBoInfo();
                        canBoInfo.CanBoID = Utils.GetInt32(dr["CanBoID"], 0);
                        canBoInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        canBoInfo.ChuTichUBND = Utils.GetInt32(dr["ChuTichUBND"], 0);
                        canBoInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
                        canBoInfo.TenChucVu = Utils.GetString(dr["TenChucVu"], String.Empty);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }

        public IList<CanBoInfo> GetDanhSachLanhDao_V2()
        {
            IList<CanBoInfo> canbos = new List<CanBoInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DM_CanBo_GetDanhSachLanhDao_V2", null))
                {
                    while (dr.Read())
                    {
                        CanBoInfo canBoInfo = new CanBoInfo();
                        canBoInfo.CanBoID = Utils.GetInt32(dr["CanBoID"], 0);
                        canBoInfo.CoQuanID = Utils.GetInt32(dr["CoQuanID"], 0);
                        canBoInfo.ChuTichUBND = Utils.GetInt32(dr["ChuTichUBND"], 0);
                        canBoInfo.TenCanBo = Utils.GetString(dr["TenCanBo"], String.Empty);
                        canBoInfo.TenChucVu = Utils.GetString(dr["TenChucVu"], String.Empty);
                        canBoInfo.RoleID = Utils.GetInt32(dr["RoleID"], 0);
                        canbos.Add(canBoInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return canbos;
        }
    }
}
