using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Com.Gosol.KNTC.DAL.HeThong;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucDiaGioiHanhChinhDAL_v2
    {
        private const string GETBYIDANDCAP = @"v2_DanhMuc_DiaGioiHanhChinh_GetByIDAndCap";
        private const string GetByID = @"v2_DanhMuc_DiaGioiHanhChinh_GetByID";
        private const string FILTERBYNAME = @"v2_DanhMuc_DiaGioiHanhChinh_FilterByName";
        private const string GETALLBYCAP = @"v2_DanhMuc_DiaGioiHanhChinh_GetAllByCap";
        //---------------------
        private const string INSERTTINH = @"v2_DanhMuc_DiaGioiHanhChinh_InsertTinh";
        private const string UPDATETINH = @"v2_DanhMuc_DiaGioiHanhChinh_UpdateTinh";
        private const string DELETETINH = @"v2_DanhMuc_DiaGioiHanhChinh_DeleteTinh";

       


        private const string INSERTHUYEN = @"v2_DanhMuc_DiaGioiHanhChinh_InsertHuyen";
        private const string UPDATEHUYEN = @"v2_DanhMuc_DiaGioiHanhChinh_UpdateHuyen";
        private const string DELETEHUYEN = @"v2_DanhMuc_DiaGioiHanhChinh_DeleteHuyen";



        private const string INSERTXA = @"v2_DanhMuc_DiaGioiHanhChinh_InsertXa";
        private const string UPDATEXA = @"v2_DanhMuc_DiaGioiHanhChinh_UpdateXa";
        private const string DELETEXA = @"v2_DanhMuc_DiaGioiHanhChinh_DeleteXa";

        //-----------------------------------------------------------------------
        private const string GETTINHBYNAME = @"v2_DanhMuc_DiaGioiHanhChinh_GetXaByName";
        private const string GETHUYENBYNAME = @"v2_DanhMuc_DiaGioiHanhChinh_GetHuyenByName";
        private const string GETXABYNAME = @"v2_DanhMuc_DiaGioiHanhChinh_GetXaByName";

        private const string PARAM_TinhID = "@TinhID";
        private const string PARAM_TENTINH = "@TenTinh";
        private const string PARAM_TENDAYDU = "@TenDayDu";
        private const string PARAM_HuyenID = "@HuyenID";
        private const string PARAM_TENHUYEN = "@TenHuyen";
        private const string PARAM_LoaiDiaDanh = "@LoaiDiaDanh";
        private const string PARAM_TENXA = "@TenXa";
        private const string PARAM_XaID = "@XaID";
        private const string PARAM_MappingCode = "@MappingCode";


        //-------------
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";
        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";

        public List<object> GetAllByCap(ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            List<object> list = new List<object>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter("@Cap",SqlDbType.Int),
                new SqlParameter("@Keyword",SqlDbType.NVarChar,(50)),
            
              };
            parameters[0].Value = thamSoLocDanhMuc1.ID;
            parameters[1].Value = thamSoLocDanhMuc1.Cap;
            parameters[2].Value = thamSoLocDanhMuc1.Keyword == null ? "" : thamSoLocDanhMuc1.Keyword;   
            
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETALLBYCAP , parameters))
                {
                    while (dr.Read())
                    {
                        var DiaGioiHanhChinh = new DanhMucDiaGioiHanhChinhMODPartial_v2(
                            Utils.ConvertToInt32(dr["ID"], 0), 
                            Utils.ConvertToString(dr["Ten"], string.Empty), 
                            Utils.ConvertToString(dr["TenDayDu"], string.Empty),
                            Utils.ConvertToInt32(dr["LoaiDiaDanh"], 0),
                            Utils.ConvertToInt32(dr["TotalChildren"], 0), thamSoLocDanhMuc1.Cap, 
                            Utils.ConvertToInt32(dr["Highlight"], 0)
                            );
                        list.Add(DiaGioiHanhChinh);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }

        //--------------------
        
        // get by id 
        public DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2 GetDGHCByIDAndCap(ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            if (thamSoLocDanhMuc1.ID <= 0)
            {
                return new DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2();
            }
            DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2 DiaGioi = new DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@ID",SqlDbType.Int),

                new SqlParameter("@Cap",SqlDbType.Int),
                new SqlParameter("@Keyword",SqlDbType.NVarChar)
             };


            parameters[0].Value = thamSoLocDanhMuc1.ID;
            parameters[1].Value = thamSoLocDanhMuc1.Cap;
            parameters[2].Value = thamSoLocDanhMuc1.Keyword == null ? "" : thamSoLocDanhMuc1.Keyword.Trim();
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, GETBYIDANDCAP , parameters))
                {
                    while (dr.Read())
                    {
                        DiaGioi = new DanhMucDiaGioiHanhChinhMODlUpdatePartial_v2(
                            Utils.ConvertToInt32(dr["ID"], 0),
                            Utils.ConvertToString(dr["Ten"], string.Empty), 
                            Utils.ConvertToString(dr["TenDayDu"], string.Empty),
                            Utils.ConvertToInt32(dr["TinhID"], 0), 
                            Utils.ConvertToInt32(dr["HuyenID"], 0), thamSoLocDanhMuc1.Cap,                           
                            Utils.ConvertToInt32(dr["TotalChildren"], 0), 
                            Utils.ConvertToInt32(dr["Highlight"], 0));
                        break;

                    }

                    dr.Close();
                }

            }
            catch
            {
                throw;
            }
            return DiaGioi;

        }
        // by id 
        public List<DanhMucDiaGioiHanhChinhMOD_v2> GetDGHCByID(int? id)
        {
            if (id <= 0 || id == null)
            {
                return new List<DanhMucDiaGioiHanhChinhMOD_v2>();
            }
            List<DanhMucDiaGioiHanhChinhMOD_v2> list = new List<DanhMucDiaGioiHanhChinhMOD_v2>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@ID",SqlDbType.Int)
             };


            parameters[0].Value = id;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, GetByID, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhModel = new DanhMucDiaGioiHanhChinhMOD_v2();
                        DanhMucDiaGioiHanhChinhModel.TinhID = Utils.ConvertToInt32(dr["TinhID"], 0);
                        DanhMucDiaGioiHanhChinhModel.HuyenID = Utils.ConvertToInt32(dr["HuyenID"], 0);
                        DanhMucDiaGioiHanhChinhModel.XaID = Utils.ConvertToInt32(dr["XaID"], 0);
                        DanhMucDiaGioiHanhChinhModel.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        DanhMucDiaGioiHanhChinhModel.TenHuyen = Utils.ConvertToString(dr["TenHuyen"], string.Empty);
                        DanhMucDiaGioiHanhChinhModel.TenXa = Utils.ConvertToString(dr["TenXa"], string.Empty);
                        DanhMucDiaGioiHanhChinhModel.LoaiDiaDanh = Utils.ConvertToInt32(dr["LoaiDiaDanh"], 0);
                        list.Add(DanhMucDiaGioiHanhChinhModel);
                    }

                    dr.Close();
                }

            }
            catch
            {
                throw;
            }
            return list;

        }
        //----insert tinh---------
        public Dictionary<int, int> InsertTinh(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            int val = 0;
            Dictionary<int, int> dic = new Dictionary<int, int>();
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                dic.Add(0, 0);
                return dic;
            }
            var DiaGioi = GetTinhByName(DanhMucDiaGioiHanhChinhMOD_v2.TenTinh);
            if (DiaGioi.TinhID > 0)
            {
                dic.Add(0, 0);
                return dic;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
            new SqlParameter("@ID",SqlDbType.Int),
            new SqlParameter(PARAM_TENTINH,SqlDbType.NVarChar),
            new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
            new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar),
              };
            parameters[0].Value = ID;
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenTinh;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.MappingCode ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, INSERTTINH, parameters), 0);
                        ID = Utils.ConvertToInt32(parameters[0].Value, 0);
                        dic.Add(val, ID);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return dic;
        }
       
        //---Update tinh --------
        /*public int UpdateTinh(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                return val;
            }
            var DiaGioi = GetTinhByName(DanhMucDiaGioiHanhChinhMOD_v2.TenTinh);
            if (DiaGioi.TinhID != DanhMucDiaGioiHanhChinhMOD_v2.TinhID && DiaGioi.TenTinh == DanhMucDiaGioiHanhChinhMOD_v2.TenTinh)
            {
                return val;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_TinhID,SqlDbType.Int),
                new SqlParameter(PARAM_TENTINH,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
                new SqlParameter("LoaiDiaDanh",SqlDbType.Int),
                new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar),

              };
            parameters[0].Value = DanhMucDiaGioiHanhChinhMOD_v2.TinhID;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenTinh;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.LoaiDiaDanh;
            parameters[4].Value = DanhMucDiaGioiHanhChinhMOD_v2.MappingCode ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, UPDATETINH, parameters), 0);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return val;
        }*/
        
        //--Huyen-----------------------------------------------------------
        // insert Huyen 
        public Dictionary<int, int> InsertHuyen(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            int val = 0;
            if (DanhMucDiaGioiHanhChinhMOD_v2.TinhID != 0)
            {
                ThamSoLocDanhMuc1 p = new ThamSoLocDanhMuc1();
                p.ID = DanhMucDiaGioiHanhChinhMOD_v2.TinhID.Value;
                p.Cap = 1;
                var Tinh = GetDGHCByIDAndCap(p);
                if (Tinh == null)
                {
                    val = 0;
                    dic.Add(val, 0);
                    return dic;
                }
            }
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                dic.Add(0, 0);
                return dic;
            }
            var DiaGioi = GetHuyenByName(DanhMucDiaGioiHanhChinhMOD_v2.TenHuyen);
            if (DiaGioi.HuyenID > 0)
            {
                dic.Add(0, 0);
                return dic;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter(PARAM_TENHUYEN,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
                //new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TinhID,SqlDbType.Int)
              };
            parameters[0].Value = ID;
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenHuyen;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu;
            //parameters[3].Value = DanhMucDiaGioiHanhChinhModel.MappingCode ?? Convert.DBNull;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.TinhID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, INSERTHUYEN, parameters);
                        ID = Utils.ConvertToInt32(parameters[0].Value, 0);
                        dic.Add(val, ID);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return dic;
        }
        // update huyen 
        /*public int UpdateHuyen(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            if (DanhMucDiaGioiHanhChinhMOD_v2.TinhID != 0)
            {
                ThamSoLocDanhMuc1 p = new ThamSoLocDanhMuc1();
                p.ID = DanhMucDiaGioiHanhChinhMOD_v2.TinhID.Value;
                p.Cap = 1;
                var Tinh = GetDGHCByIDAndCap(p);
                if (string.IsNullOrEmpty(Tinh.Ten))
                {
                    val = 0;
                    return val;
                }
            }
            var DiaGioi = GetHuyenByName(DanhMucDiaGioiHanhChinhMOD_v2.TenHuyen.Trim());
            if (DiaGioi.HuyenID != DanhMucDiaGioiHanhChinhMOD_v2.HuyenID && DiaGioi.TenHuyen == DanhMucDiaGioiHanhChinhMOD_v2.TenHuyen)
            {

                return val;
            }
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                return val;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_HuyenID,SqlDbType.Int),
                new SqlParameter(PARAM_TENHUYEN,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TinhID,SqlDbType.Int),
                new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
                //new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar)

              };
            parameters[0].Value = DanhMucDiaGioiHanhChinhMOD_v2.HuyenID;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenHuyen;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TinhID ?? Convert.DBNull;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu;
            //parameters[4].Value = DanhMucDiaGioiHanhChinhModel.MappingCode ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, UPDATEHUYEN, parameters), 0);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return val;
        }*/
        
        // insert xa
        public Dictionary<int, int> InsertXa(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2, ref int ID)
        {
            Dictionary<int, int> dic = new Dictionary<int, int>();
            int val = 0;
            if (DanhMucDiaGioiHanhChinhMOD_v2.HuyenID != 0)
            {
                ThamSoLocDanhMuc1 p = new ThamSoLocDanhMuc1();
                p.ID = DanhMucDiaGioiHanhChinhMOD_v2.HuyenID.Value;
                p.Cap = 2;
                var Tinh = GetDGHCByIDAndCap(p);
                if (Tinh == null)
                {
                    val = 2;
                    dic.Add(val, 2);
                    return dic;
                }
            }
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                dic.Add(0, 2);
                return dic;
            }
            var Huyen = new DanhMucDiaGioiHanhChinhDAL().GetDGHCByIDAndCap(DanhMucDiaGioiHanhChinhMOD_v2.HuyenID.Value, 2, null);
            if (string.IsNullOrEmpty(Huyen.Ten) || string.IsNullOrEmpty(Huyen.TenDayDu))
            {
                dic.Add(0, 2);
                return dic;
            }
            var DiaGioi = GetXaByName(DanhMucDiaGioiHanhChinhMOD_v2.TenXa);
            if (DiaGioi.XaID > 0)
            {
                dic.Add(0, 0);
                return dic;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@ID",SqlDbType.Int),
                new SqlParameter(PARAM_TENXA,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
                new SqlParameter(PARAM_HuyenID,SqlDbType.Int),
                //new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar),
              };
            parameters[0].Value = ID;
            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenXa;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.HuyenID;
            //parameters[4].Value = DanhMucDiaGioiHanhChinhMOD_v2.MappingCode ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, INSERTXA, parameters);
                        ID = Utils.ConvertToInt32(parameters[0].Value, 0);
                        dic.Add(val, ID);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return dic;
        }
        // update xa 
        /*public int UpdateXa(DanhMucDiaGioiHanhChinhMOD_v2 DanhMucDiaGioiHanhChinhMOD_v2)
        {
            int val = 0;
            if (DanhMucDiaGioiHanhChinhMOD_v2.HuyenID != 0)
            {
                ThamSoLocDanhMuc1 p = new ThamSoLocDanhMuc1();
                p.ID = DanhMucDiaGioiHanhChinhMOD_v2.HuyenID.Value;
                p.Cap = 2;
                var Huyen = GetDGHCByIDAndCap(p);
                if (string.IsNullOrEmpty(Huyen.Ten))
                {
                    val = 0;
                    return val;
                }
            }

            //--
            var DiaGioi = GetXaByName(DanhMucDiaGioiHanhChinhMOD_v2.TenXa.Trim());
            if (DiaGioi.XaID != DanhMucDiaGioiHanhChinhMOD_v2.XaID && DiaGioi.TenXa == DanhMucDiaGioiHanhChinhMOD_v2.TenXa)
            {

                return val;
            }
            if (DanhMucDiaGioiHanhChinhMOD_v2 == null)
            {
                return val;
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(PARAM_XaID,SqlDbType.Int),
                new SqlParameter(PARAM_HuyenID,SqlDbType.Int),
                new SqlParameter(PARAM_TENXA,SqlDbType.NVarChar),
                new SqlParameter(PARAM_TENDAYDU,SqlDbType.NVarChar),
                //new SqlParameter(PARAM_MappingCode,SqlDbType.NVarChar),

              };
            parameters[0].Value = DanhMucDiaGioiHanhChinhMOD_v2.XaID;
            parameters[1].Value = DanhMucDiaGioiHanhChinhMOD_v2.HuyenID ?? Convert.DBNull;
            parameters[2].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenXa;
            parameters[3].Value = DanhMucDiaGioiHanhChinhMOD_v2.TenDayDu ?? Convert.DBNull;
            //parameters[4].Value = DanhMucDiaGioiHanhChinhMOD_v2.MappingCode ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, UPDATEXA, parameters), 0);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return val;


        }*/
        

        //   get tinh - huyen - xa by name
        public DanhMucDiaGioiHanhChinhMOD_v2 GetXaByName(string TenXa)
        {
            if (string.IsNullOrEmpty(TenXa))
            {
                return new DanhMucDiaGioiHanhChinhMOD_v2();
            }
            DanhMucDiaGioiHanhChinhMOD_v2 Xa = new DanhMucDiaGioiHanhChinhMOD_v2();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TenXa",SqlDbType.NVarChar)
              };
            parameters[0].Value = TenXa;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_DanhMuc_DiaGioiHanhChinh_GetXaByName", parameters))
                {
                    while (dr.Read())
                    {
                        Xa = new DanhMucDiaGioiHanhChinhMOD_v2(
                            Utils.ConvertToInt32(dr["TinhID"], 0),
                            Utils.ConvertToInt32(dr["HuyenID"], 0),
                            Utils.ConvertToInt32(dr["XaID"], 0),
                            Utils.ConvertToString(dr["TenTinh"], string.Empty), 
                            Utils.ConvertToString(dr["TenHuyen"], string.Empty), 
                            Utils.ConvertToString(dr["TenXa"], string.Empty),null,
                            Utils.ConvertToInt32(dr["LoaiDiaDanh"], 0)
                            );
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return Xa;
        }

        public DanhMucDiaGioiHanhChinhMOD_v2 GetHuyenByName(string TenHuyen)
        {
            if (string.IsNullOrEmpty(TenHuyen))
            {
                return new DanhMucDiaGioiHanhChinhMOD_v2();
            }
            DanhMucDiaGioiHanhChinhMOD_v2 Huyen = new DanhMucDiaGioiHanhChinhMOD_v2();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TenHuyen",SqlDbType.NVarChar)
              };
            parameters[0].Value = TenHuyen;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_DanhMuc_DiaGioiHanhChinh_GetHuyenByName", parameters))
                {
                    while (dr.Read())
                    {
                        Huyen = new DanhMucDiaGioiHanhChinhMOD_v2(
                            Utils.ConvertToInt32(dr["TinhID"], 0),
                            Utils.ConvertToInt32(dr["HuyenID"], 0),
                            Utils.ConvertToInt32(dr["XaID"], 0),
                            Utils.ConvertToString(dr["TenTinh"], string.Empty),
                            Utils.ConvertToString(dr["TenHuyen"], string.Empty), 
                            Utils.ConvertToString(dr["TenXa"], string.Empty), null ,
                            Utils.ConvertToInt32(dr["LoaiDiaDanh"],0));
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return Huyen;
        }

        public DanhMucDiaGioiHanhChinhMOD_v2 GetTinhByName(string TenTinh)
        {
            if (string.IsNullOrEmpty(TenTinh))
            {
                return new DanhMucDiaGioiHanhChinhMOD_v2();
            }
            DanhMucDiaGioiHanhChinhMOD_v2 Tinh = new DanhMucDiaGioiHanhChinhMOD_v2();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TenTinh",SqlDbType.NVarChar)
              };
            parameters[0].Value = TenTinh;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_DiaGioiHanhChinh_GetTinhByName", parameters))
                {
                    while (dr.Read())
                    {
                        Tinh = new DanhMucDiaGioiHanhChinhMOD_v2();
                        Tinh.TinhID = Utils.ConvertToInt32(dr["TinhID"], 0);
                        Tinh.TenTinh = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        Tinh.TenDayDu = Utils.ConvertToString(dr["TenDayDu"], string.Empty);


                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return Tinh;
        }
        //-------------------------

        public BaseResultModel DanhSachTinh(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            List<DanhMucDiaGioiHanhChinhMODPartial_v2> Data = new List<DanhMucDiaGioiHanhChinhMODPartial_v2>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_DiaGioiHanhChinh_DanhSach"))
                {
                    while (dr.Read())
                    {

                        DanhMucDiaGioiHanhChinhMODPartial_v2 DonVi = new DanhMucDiaGioiHanhChinhMODPartial_v2();
                        DonVi.ID = Utils.ConvertToInt32(dr["TinhID"], 0);
                        DonVi.Ten = Utils.ConvertToString(dr["TenTinh"], string.Empty);
                        
                        Data.Add(DonVi);
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = Data;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        // huyen 
        public BaseResultModel DanhSachHuyen(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            List<DanhMucDiaGioiHanhChinhMODPartial_v2> Data = new List<DanhMucDiaGioiHanhChinhMODPartial_v2>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_DiaGioiHanhChinh_DanhSach"))
                {
                    while (dr.Read())
                    {

                        DanhMucDiaGioiHanhChinhMODPartial_v2 DonVi = new DanhMucDiaGioiHanhChinhMODPartial_v2();
                        DonVi.ID = Utils.ConvertToInt32(dr["HuyenID"], 0);
                        DonVi.Ten = Utils.ConvertToString(dr["TenHuyen"], string.Empty);

                        Data.Add(DonVi);
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = Data;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        // get cac cap 

        public List<object> GetByCap(ThamSoLocDanhMuc1 thamSoLocDanhMuc1)
        {
            List<object> list = new List<object>();
            SqlParameter[] parameters = new SqlParameter[]
              {
               
                new SqlParameter("@Keyword",SqlDbType.NVarChar,(50)),

              };
           
            parameters[1].Value = thamSoLocDanhMuc1.Keyword == null ? "" : thamSoLocDanhMuc1.Keyword;

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETALLBYCAP, parameters))
                {
                    while (dr.Read())
                    {

                        var DiaGioiHanhChinh = new DanhMucDiaGioiHanhChinhMODPartial_v2(
                            Utils.ConvertToInt32(dr["ID"], 0),
                            Utils.ConvertToString(dr["Ten"], string.Empty), 
                            Utils.ConvertToString(dr["TenDayDu"], string.Empty),
                            Utils.ConvertToInt32(dr["LoaiDiaDanh"], 0),
                            Utils.ConvertToInt32(dr["TotalChildren"], 0), thamSoLocDanhMuc1.Cap,
                            Utils.ConvertToInt32(dr["Highlight"], 0)
                            );
                        list.Add(DiaGioiHanhChinh);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;

        }
        // test xoa huyen - tinh - xa
        public BaseResultModel XoaTinh(int TinhID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("TinhID", SqlDbType.Int)
            };
            parameters[0].Value = TinhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v2_DanhMuc_DiaGioiHanhChinh_DeleteTinh", parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa  danh mục !";
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
            Result.Message = "Xóa tỉnh thành công!";
            return Result;
        }

        

        //
        public BaseResultModel XoaHuyen(int HuyenID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("HuyenID", SqlDbType.Int)
            };
            parameters[0].Value = HuyenID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v2_DanhMuc_DiaGioiHanhChinh_DeleteHuyen", parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa danh mục!";
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
            Result.Message = "Xóa huyện thành công!";
            return Result;
        }

        // 
        public BaseResultModel XoaXa(int XaID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("XaID", SqlDbType.Int)
            };
            parameters[0].Value = XaID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, "v2_DanhMuc_DiaGioiHanhChinh_DeleteXa", parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa danh mục !";
                            return Result;
                        }
                    }
                    catch
                    {
                        Result.Status = 1;
                        Result.Message = Constant.ERR_DELETE;
                        trans.Rollback();
                        return Result;
                        throw;
                    }
                }
            }
            Result.Status = 1;
            Result.Message = "Xóa thành công!";
            return Result;
        }

        //--------------------------

        public BaseResultModel CapNhatTinh(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(PARAM_TinhID, SqlDbType.Int),
                    new SqlParameter(PARAM_TENTINH, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_TENDAYDU, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_LoaiDiaDanh, SqlDbType.Int)
                   
                };
                parameters[0].Value = item.ID;
                parameters[1].Value = item.Ten.Trim();
                parameters[2].Value = item.TenDayDu.Trim();
                parameters[3].Value = item.LoaiDiaDanh;
                
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATETINH, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật Tỉnh thành công!";
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

        //--
        public BaseResultModel CapNhatHuyen(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(PARAM_HuyenID, SqlDbType.Int),
                    //new SqlParameter(PARAM_TinhID, SqlDbType.Int),
                    new SqlParameter(PARAM_TENHUYEN, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_TENDAYDU, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_LoaiDiaDanh , SqlDbType.Int)
                    
                   
                };
                parameters[0].Value = item.ID;
               
                parameters[1].Value = item.Ten.Trim();
                parameters[2].Value = item.TenDayDu.Trim();
                parameters[3].Value = item.LoaiDiaDanh;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATEHUYEN, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật Huyện thành công!";
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

        //--
        public BaseResultModel CapNhatXa(DanhMucDiaGioiHanhChinhMODPartial_v2 item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(PARAM_XaID, SqlDbType.Int),
                    
                    new SqlParameter(PARAM_TENXA, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_TENDAYDU, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_LoaiDiaDanh , SqlDbType.Int)
                   
                };
                parameters[0].Value = item.ID;
                
                parameters[1].Value = item.Ten.Trim();
                parameters[2].Value = item.TenDayDu.Trim();
                parameters[3].Value = item.LoaiDiaDanh; 

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UPDATEXA, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật Xã thành công!";
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
    }
}

