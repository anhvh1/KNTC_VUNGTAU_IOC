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
    public class FileGiaiQuyet
    {
        private const string INSERT = @"FileGiaiQuyet_Insert";
        private const string INSERT_NEW = @"FileGiaiQuyet_Insert_New";
        private const string GET_BY_BUOCGIAIQUYET = @"FileGiaiQuyet_GetByBuocGiaiQuyet";
        private const string DELETE_BY_THEODOIXULYID = "FileGiaiQuyet_DeleteByTheoDoiXuLyID";
        private const string DELETE_BY_XULYDONID = "FileGiaiQuyet_DeleteByXuLyDonID";
        private const string FILEGIAIQUYET_THEODOIXULYID = "FileGiaiQuyet_GetByTheoDoiXuLyID";

        private const string PARM_NOIDUNG = @"NoiDung";
        private const string PARM_NGAYCAPNHAT = @"NgayCapNhat";
        private const string PARM_FILEGIAIQUYETID = @"FileGiaiQuyetID";
        private const string PARM_DUONGDANFILE = @"DuongDanFile";
        private const string PARM_THEODOIXULYID = @"TheoDoiXuLyID";
        private const string PARM_XULYDONID = @"XuLyDonID";
        private const string PARM_TENFILE = @"TenFile";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private const string PARAM_BUOCID = "@BuocID";
        private const string PARAM_FILEID = "@FileID";
        private FileGiaiQuyetInfo GetData(SqlDataReader rdr)
        {
            FileGiaiQuyetInfo info = new FileGiaiQuyetInfo();
            info.TheoDoiXuLyID = Utils.GetInt32(rdr["TheoDoiXuLyID"], 0);
            info.NoiDung = Utils.GetString(rdr["NoiDung"], string.Empty);
            info.DuongDanFile = Utils.GetString(rdr["DuongDanFile"], string.Empty);
            info.NgayCapNhat = Utils.GetDateTime(rdr["NgayCapNhat"], DateTime.MinValue);
            info.TenFile = Utils.ConvertToString(rdr["TenFile"], string.Empty);
            info.NgayCapNhats = "";
            if (info.NgayCapNhat != DateTime.MinValue)
            {
                info.NgayCapNhats = info.NgayCapNhat.ToString("dd/MM/yyyy");
            }

            return info;
        }
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_NOIDUNG, SqlDbType.NText),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_DUONGDANFILE, SqlDbType.NVarChar,2000),
                new SqlParameter(PARM_TENFILE,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_FILEID,SqlDbType.Int)
            }; return parms;
        }

        private void SetInsertParms(SqlParameter[] parms, FileGiaiQuyetInfo info)
        {
            parms[0].Value = info.TheoDoiXuLyID;
            parms[1].Value = info.NoiDung;
            parms[2].Value = info.NgayCapNhat;
            parms[3].Value = info.DuongDanFile;

            if (info.TenFile == null)
            {
                parms[4].Value = DBNull.Value;
            }
            else
            {
                parms[4].Value = info.TenFile;
            }
            parms[5].Value = info.FileID;
        }
        public int Insert(FileGiaiQuyetInfo Fileinfo)
        {
            object val = null;
            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, Fileinfo);
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_NEW, parameters);
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
        public List<FileGiaiQuyetInfo> GetByBuocGiaiQuyet(int theodoixulyid, int loaiFile)
        {
            List<FileGiaiQuyetInfo> ListInfo = new List<FileGiaiQuyetInfo>();
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARM_THEODOIXULYID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int),
            };
            parms[0].Value = theodoixulyid;
            parms[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_BUOCGIAIQUYET, parms))
                {
                    while (dr.Read())
                    {
                        FileGiaiQuyetInfo info = GetData(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        ListInfo.Add(info);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return ListInfo;
        }

        public int Delete(int theodoixulyID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int),

            };
            parameters[0].Value = theodoixulyID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_BY_THEODOIXULYID, parameters);
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

        public int DeleteByXuLyDon(int xuLyDonID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(PARM_XULYDONID, SqlDbType.Int),

            };
            parameters[0].Value = xuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_BY_XULYDONID, parameters);
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
        public List<FileHoSoInfo> GetFileGiaiQuyetByTheoDoiID(int xldID, int loaiFile)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
                  new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int)
            };
            parameters[0].Value = xldID;
            parameters[1].Value = loaiFile;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, FILEGIAIQUYET_THEODOIXULYID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.NguoiUp = 0;
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["DuongDanFile"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        cInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        lst_HoSo.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return lst_HoSo;
        }

        public List<FileHoSoInfo> GetFileGiaiQuyetByBuocXacMinhID(int xldID, int loaiFile, int buocXacMinhID)
        {
            List<FileHoSoInfo> lst_HoSo = new List<FileHoSoInfo>();
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARM_XULYDONID,SqlDbType.Int),
                  new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int),
                  new SqlParameter(PARAM_BUOCID,SqlDbType.Int),
            };
            parameters[0].Value = xldID;
            parameters[1].Value = loaiFile;
            parameters[2].Value = buocXacMinhID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileGiaiQuyet_GetByXuLyDonID_BuocID", parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.NguoiUp = 0;
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayCapNhat"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["DuongDanFile"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        cInfo.TheoDoiXuLyID = Utils.ConvertToInt32(dr["TheoDoiXuLyID"], 0);
                        cInfo.FileHoSoID = Utils.ConvertToInt32(dr["FileGiaiQuyetID"], 0);
                        lst_HoSo.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return lst_HoSo;
        }

    }
}
