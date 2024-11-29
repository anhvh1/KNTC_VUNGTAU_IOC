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
    public class KetQuaTranhChapDAL
    {
        #region == Query
        // Kết quả tranh chấp
        private const string INSERT_KETQUATRANHCHAP = @"KetQuaTranhChap_Insert";
        private const string UPDATE_KETQUATRANHCHAP = @"KetQuaTranhChap_Update";
        private const string GET_KQ_BYXULYDONID = @"KetQuaTranhChap_GetByXuLyDonID";

        // Hội đồng hòa giải
        private const string INSERT_HOIDONGHOAGIAI = @"HoiDongHoaGiai_Insert";
        private const string UPDATE_HOIDONGHOAGIAI = @"HoiDongHoaGiai_Update";
        private const string GET_HD_BYKETQUATRANHCHAPID = @"HoiDongHoaGiai_GetByKetQuaTranhChapID";

        // File kết quả tranh chấp 
        private const string INSERT_FILEKQTRANHCHAP = @"FileKQTranhChap_Insert";
        private const string INSERT_FILEKQTRANHCHAP_NEW = @"FileKQTranhChap_Insert_New";
        private const string DELETE_FILEKQTRANHCHAP = @"FileKQTranhChap_Delete";
        private const string GET_FILE_BYKETQUATRANHCHAPID = @"FileKQTranhChap_GetByKetQuaTranhChapID";
        #endregion

        #region == Param
        // Kết quả tranh chấp
        private const string PARM_KETQUATRANHCHAPID = @"KetQuaTranhChapID";
        private const string PARM_COQUANID = @"CoQuanID";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_NDHOAGIAI = @"NDHoaGiai ";
        private const string PARM_KETQUAHOAGIAI = @"KetQuaHoaGiai";
        private const string PARM_FILEURL = @"FileUrl";
        private const string PARM_NGAYRAKQ = @"NgayRaKQ";
        private const string PARM_CANBOID = @"CanBoID";

        // Hội động hòa giải
        private const string PARM_HOIDONGHOAGIAIID = @"HoiDongHoaGiaiID";
        private const string PARM_TENCANBO = @"TenCanBo";
        private const string PARM_NHIEMVU = @"NhiemVu";

        // File kết quả tranh chấp 
        private const string PARM_FILEKQTRANHCHAPID = @"FileKQTranhChapID";
        private const string PARM_CANBOUPID = @"CanBoUpID";
        private const string PARM_TENFILE = @"TenFile";
        private const string PARM_TOMTAT = @"TomTat";
        private const string PARM_NGAYCAPNHAT = @"NgayCapNhat";
        #endregion

        #region == Insert
        public int Insert(KetQuaTranhChapInfo kq_item, List<HoiDongHoaGiaiInfo> hd_item)
        {
            int result;
            int ketQuaTC;
            SqlParameter[] parameters = GetInsertKQParms();
            SetInsertKQParms(parameters, kq_item);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        result = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_KETQUATRANHCHAP, parameters), 0);
                        ketQuaTC = result;
                        if (result > 0)
                        {
                            for (int i = 0; i < hd_item.Count; i++)
                            {
                                hd_item[i].KetQuaTranhChapID = ketQuaTC;
                                parameters = GetInsertHDParms();
                                SetInsertHDParms(parameters, hd_item[i]);

                                result = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_HOIDONGHOAGIAI, parameters), 0);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result = 0;
                        throw;
                    }
                }
                conn.Close();
            }
            return Utils.ConvertToInt32(ketQuaTC, 0);
        }

        #region == Insert KQ - ket qua tranh chap
        private SqlParameter[] GetInsertKQParms()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                    new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                    new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                    new SqlParameter(PARM_NDHOAGIAI, SqlDbType.NVarChar),
                    new SqlParameter(PARM_KETQUAHOAGIAI, SqlDbType.NVarChar),
                    new SqlParameter(PARM_FILEURL, SqlDbType.VarChar, 200),
                    new SqlParameter(PARM_NGAYRAKQ, SqlDbType.DateTime),
                    new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                    };
            return parms;
        }

        private void SetInsertKQParms(SqlParameter[] parms, KetQuaTranhChapInfo item)
        {
            parms[0].Value = item.CoQuanID;
            parms[1].Value = item.XuLyDonID;
            parms[2].Value = item.NDHoaGiai ?? Convert.DBNull;
            parms[3].Value = item.KetQuaHoaGiai ?? Convert.DBNull;
            parms[4].Value = item.FileUrl ?? Convert.DBNull;
            parms[5].Value = item.NgayRaKQ;
            parms[6].Value = item.CanBoID;
        }

        public int Insert_KQ(KetQuaTranhChapInfo item)
        {
            object id;

            SqlParameter[] parameters = GetInsertKQParms();
            SetInsertKQParms(parameters, item);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        id = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_KETQUATRANHCHAP, parameters);

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
            return Utils.ConvertToInt32(id, 0);
        }
        #endregion

        #region == Insert HD - hoi dong hoa giai
        private SqlParameter[] GetInsertHDParms()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                    new SqlParameter(PARM_KETQUATRANHCHAPID, SqlDbType.Int),
                    new SqlParameter(PARM_TENCANBO, SqlDbType.NVarChar, 200),
                    new SqlParameter(PARM_NHIEMVU, SqlDbType.NVarChar, 200),
                    };
            return parms;
        }

        private void SetInsertHDParms(SqlParameter[] parms, HoiDongHoaGiaiInfo item)
        {
            parms[0].Value = item.KetQuaTranhChapID;
            parms[1].Value = item.TenCanBo ?? Convert.DBNull;
            parms[2].Value = item.NhiemVu ?? Convert.DBNull;
        }

        public int Insert_HD(HoiDongHoaGiaiInfo item)
        {
            object id;

            SqlParameter[] parameters = GetInsertHDParms();
            SetInsertHDParms(parameters, item);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        id = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_HOIDONGHOAGIAI, parameters);

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
            return Utils.ConvertToInt32(id, 0);
        }
        #endregion

        #region == Insert File
        private SqlParameter[] GetInsertFileParms()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                    new SqlParameter(PARM_KETQUATRANHCHAPID, SqlDbType.Int),
                    new SqlParameter(PARM_CANBOUPID, SqlDbType.Int),
                    new SqlParameter(PARM_TENFILE, SqlDbType.NVarChar, 200),
                    new SqlParameter(PARM_TOMTAT, SqlDbType.NVarChar),
                    new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                    new SqlParameter(PARM_FILEURL, SqlDbType.VarChar, 200),
                    new SqlParameter("@FileID", SqlDbType.Int)
                    };
            return parms;
        }

        private void SetInsertFileParms(SqlParameter[] parms, FileKetQuaTranhChapInfo item)
        {
            parms[0].Value = item.KetQuaTranhChapID;
            parms[1].Value = item.CanBoUpID;
            parms[2].Value = item.TenFile;
            parms[3].Value = item.TomTat;
            parms[4].Value = item.NgayCapNhat;
            parms[5].Value = item.FileUrl;
            parms[6].Value = item.FileID;
        }

        public int Insert_File(FileKetQuaTranhChapInfo item)
        {
            object id;

            SqlParameter[] parameters = GetInsertFileParms();
            SetInsertFileParms(parameters, item);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        id = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILEKQTRANHCHAP_NEW, parameters);
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
            return Utils.ConvertToInt32(id, 0);
        }
        #endregion
        #endregion

        #region == Update
        private SqlParameter[] GetUpdateKQParms()
        {
            SqlParameter[] parms = new SqlParameter[] { };

            parms = new SqlParameter[]{
                    new SqlParameter(PARM_COQUANID, SqlDbType.Int),
                    new SqlParameter(PARM_XULYDONID, SqlDbType.Int),
                    new SqlParameter(PARM_NDHOAGIAI, SqlDbType.NVarChar),
                    new SqlParameter(PARM_KETQUAHOAGIAI, SqlDbType.NVarChar),
                    new SqlParameter(PARM_FILEURL, SqlDbType.VarChar, 200),
                    new SqlParameter(PARM_CANBOID, SqlDbType.Int),
                    new SqlParameter(PARM_KETQUATRANHCHAPID, SqlDbType.Int),
                    };
            return parms;
        }

        private void SetUpdateKQParms(SqlParameter[] parms, KetQuaTranhChapInfo item)
        {
            parms[0].Value = item.CoQuanID;
            parms[1].Value = item.XuLyDonID;
            parms[2].Value = item.NDHoaGiai ?? Convert.DBNull;
            parms[3].Value = item.KetQuaHoaGiai ?? Convert.DBNull;
            parms[4].Value = item.FileUrl ?? Convert.DBNull;
            parms[5].Value = item.CanBoID;
            parms[6].Value = item.KetQuaTranhChapID;
        }

        public int Update(KetQuaTranhChapInfo kq_item, List<HoiDongHoaGiaiInfo> hd_item)
        {
            int result;

            SqlParameter[] parameters = GetUpdateKQParms();
            SetUpdateKQParms(parameters, kq_item);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        result = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, UPDATE_KETQUATRANHCHAP, parameters), 0);
                        result = 1;
                        if (result > 0)
                        {
                            for (int i = 0; i < hd_item.Count; i++)
                            {
                                hd_item[i].KetQuaTranhChapID = kq_item.KetQuaTranhChapID;
                                parameters = GetInsertHDParms();
                                SetInsertHDParms(parameters, hd_item[i]);

                                result = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_HOIDONGHOAGIAI, parameters), 0);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result = 0;
                        throw;
                    }
                }
                conn.Close();
            }
            return Utils.ConvertToInt32(result, 0);
        }
        #endregion

        #region == Get
        public KetQuaTranhChapInfo GetKQByXuLyDonID(int xuLyDonID)
        {
            KetQuaTranhChapInfo item = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_KQ_BYXULYDONID, parameters))
                {

                    if (dr.Read())
                    {
                        item = new KetQuaTranhChapInfo();
                        item.NDHoaGiai = Utils.ConvertToString(dr["NDHoaGiai"], string.Empty);
                        item.KetQuaHoaGiai = Utils.ConvertToString(dr["KetQuaHoaGiai"], string.Empty);
                        item.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        item.KetQuaTranhChapID = Utils.ConvertToInt32(dr["KetQuaTranhChapID"], 0);
                        item.NgayRaKQ = Utils.ConvertToDateTime(dr["NgayRaKQ"], DateTime.MinValue);
                        item.NgayRaKQ_Str = Format.FormatDate(item.NgayRaKQ);
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.lstHoiDong = GetHDByKetQuaTranhChapID(item.KetQuaTranhChapID);
                        item.lstFileKQ = GetFileByKetQuaTranhChapID(item.KetQuaTranhChapID);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return item;
        }

        public List<HoiDongHoaGiaiInfo> GetHDByKetQuaTranhChapID(int ketQuaTranhChapID)
        {
            List<HoiDongHoaGiaiInfo> lstItem = new List<HoiDongHoaGiaiInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_KETQUATRANHCHAPID,SqlDbType.Int)
            };
            parameters[0].Value = ketQuaTranhChapID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_HD_BYKETQUATRANHCHAPID, parameters))
                {

                    while (dr.Read())
                    {
                        HoiDongHoaGiaiInfo item = new HoiDongHoaGiaiInfo();
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.NhiemVu = Utils.ConvertToString(dr["NhiemVu"], string.Empty);
                        lstItem.Add(item);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return lstItem;
        }

        public List<FileKetQuaTranhChapInfo> GetFileByKetQuaTranhChapID(int ketQuaTranhChapID)
        {
            List<FileKetQuaTranhChapInfo> lstItem = new List<FileKetQuaTranhChapInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_KETQUATRANHCHAPID,SqlDbType.Int)
            };
            parameters[0].Value = ketQuaTranhChapID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_FILE_BYKETQUATRANHCHAPID, parameters))
                {
                    while (dr.Read())
                    {
                        FileKetQuaTranhChapInfo item = new FileKetQuaTranhChapInfo();
                        item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        item.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        item.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        item.NgayCapNhat_Str = Format.FormatDate(item.NgayCapNhat);
                        item.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        item.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        item.CanBoUpID = Utils.ConvertToInt32(dr["CanBoUpID"], 0);
                        lstItem.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItem;
        }

        public List<FileKetQuaTranhChapInfo> GetFileByXuLyDonID(int xuLyDonID)
        {
            List<FileKetQuaTranhChapInfo> lstItem = new List<FileKetQuaTranhChapInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@XuLyDonID",SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileKQTranhChap_GetByXuLyDonID_New", parameters))
                {
                    while (dr.Read())
                    {
                        FileKetQuaTranhChapInfo item = new FileKetQuaTranhChapInfo();
                        item.KetQuaTranhChapID = Utils.ConvertToInt32(dr["FileKQTranhChapID"], 0);
                        item.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        item.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        item.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        item.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        item.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        //item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        item.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        item.NgayCapNhat = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        item.NgayCapNhat_Str = Format.FormatDate(item.NgayCapNhat);
                        item.FileUrl = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        item.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        item.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        item.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        item.CanBoUpID = Utils.ConvertToInt32(dr["CanBoUpID"], 0);
                        lstItem.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return lstItem;
        }
        #endregion

        #region == Delete
        #region == Delete File
        public int Delete_File(int ketQuaTranhChapID)
        {
            object val;

            SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter(PARM_KETQUATRANHCHAPID, SqlDbType.Int)
            };
            parameters[0].Value = ketQuaTranhChapID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, DELETE_FILEKQTRANHCHAP, parameters);
                        val = 1;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        val = 0;
                        throw;
                    }
                }
                conn.Close();
            }
            return Utils.ConvertToInt32(val, 0);
        }
        #endregion
        #endregion
    }
}
