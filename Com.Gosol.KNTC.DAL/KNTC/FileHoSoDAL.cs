using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class FileHoSoDAL
    {
        //Su dung de goi StoreProcedure

        private const string GET_ALL = @"FileHoSo_GetAll";
        private const string GET_BY_ID = @"FileHoSo_GetByID";
        private const string GET_BY_XULYDON_ID = @"FileHoSo_GetByXuLyDonID";
        private const string GET_BY_XULYDON_ID_NEW = @"FileHoSo_GetByXuLyDonID_New";
        private const string GET_BY_DONTHU_ID = @"FileHoSo_GetByDonThuID";
        private const string GET_BY_DONTHU_TYPE = @"FileHoSo_GetDonThuByType";
        private const string GET_BY_DONTHU_TYPE_NEW = @"FileHoSo_GetDonThuByType_New";
        private const string INSERT = @"FileHoSo_Insert";
        private const string UPDATE = @"FileHoSo_Update";
        private const string DELETE = @"FileHoSo_Delete";
        private const string DELETE_BY_TYPE = @"FileHoSoByType_Delete";
        private const string DELETE_FILEYKIENXULY = @"FileYKienXuLyByType_Delete";
        private const string DELETE_FileDMBuocXacMinh = @"FileDMBuocXacMinh_Delete";

        private const string INSERT_FILE_YKIENXL = @"FileYKienXuLy_Insert";
        private const string INSERT_FILE_YKIENXL_New = @"FileYKienXuLy_Insert_New";
        private const string INSERT_FILE_DonThuCPGQ = @"FileDonThuCPGQ_Insert";
        private const string INSERT_FILE_DonThuCPGQ_New = @"FileDonThuCPGQ_Insert_New";
        private const string INSERT_FILE_RutDon = @"FileRutDon_Insert";
        private const string INSERT_FILE_RutDon_New = @"FileRutDon_Insert_New";
        private const string INSERT_FILE_PhanXuLy = @"FilePhanXuLy_Insert";
        private const string INSERT_FILE_PhanXuLy_New = @"FilePhanXuLy_Insert_New";
        private const string INSERT_FILE_DonThuCDGQ = @"FileDonThuCDGQ_Insert";
        private const string INSERT_FILE_DonThuCDGQ_New = @"FileDonThuCDGQ_Insert_New";
        private const string INSERT_FILE_BCXM = @"FileBCXM_Insert";
        private const string INSERT_FILE_BCXM_New = @"FileBCXM_Insert_New";
        private const string INSERT_FILE_DMBXM = @"FileDMBXM_Insert";

        private const string GET_BY_PAGE = @"DM_FileHoSo_GetByPage";
        private const string COUNT_ALL = @"DM_FileHoSo_CountAll";
        private const string COUNT_SEARCH = @"DM_FileHoSo_CountSearch";
        private const string SEARCH = @"DM_FileHoSo_GetBySearch";

        //quanghv: stored
        private const string GET_SO_DON_BY_CO_QUAN = @"NV_FileHoSo_GetSoDonByCoQuan";

        private const string FILEYKIENXL_GET_BY_ID = @"FileYKienXuLy_GetByID";
        private const string DELETE_FILEYKIENXL = @"FileYKienXuLy_Delete";
        private const string FILEYKIENXULY_DELETEBYXLDID = @"FileYKienXuLy_DeleteByXLDID";
        private const string GETFILEPHANXULY_BYXULYDONID = @"FilePhanXuLy_GetByXuLyDonID";
        private const string GETFILEPHANXULY_BYXULYDONID_NEW = @"FilePhanXuLy_GetByXuLyDonID_New";
        private const string GETFILEYKIENXLBYXULYDONID = @"FileYKIENXL_GetByXuLyDonID";
        private const string GETFILEBANHANHQDBYXULYDONID = @"FileBanHanhQD_GetByXuLyDonID";

        private const string GETFILEDONTHUCDGQ_BYXULYDONID = @"FileDonThuCanDuyetGiaiQuyet_GetByXuLyDonID";
        private const string FILEDONTHUCDGQ_DELETEBYXULYDONID = @"FileDonThuCanDuyetGiaiQuyet_DeleteByXLDID";

        //Ten cac bien dau vao
        private const string PARAM_FILEHOSO_ID = "@FileHoSoID";
        private const string PARAM_TEN_FILE = "@TenFile";
        private const string PARAM_TOMTAT = "@TomTat";
        private const string PARAM_FILE_URL = "@FileURL";
        private const string PARAM_NGUOIUP = "@NguoiUp";
        private const string PARAM_NGAYUP = "@NgayUp";
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_YKIENGIAIQUYET_ID = "@YKienGiaiQuyetID";
        private const string PARAM_CHUYENGIAIQUYET_ID = "@ChuyenGiaiQuyetID";
        private const string PARAM_DONTHU_ID = "@DonThuID";
        private const string PARAM_FILE_ID = "@FileID";
        private const string PARAM_FILETYPE = "@FileType";
        private const string PARAM_FILEYKIENXULY_ID = "@FileYKienXuLyID";
        private const string PARAM_YKIENGIAIQUYETID = "@YKienGiaiQuyetID";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private const string PARAM_LOAIFILE_DELETE = "@LoaiFileDelete";
        private const string PARAM_DMBuocXacMinhID = "@DMBuocXacMinhID";
        private FileHoSoInfo GetData(SqlDataReader dr)
        {
            FileHoSoInfo info = new FileHoSoInfo();
            info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
            info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
            info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
            info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
            info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
            if (info.NguoiUp > 0)
            {
                info.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(info.NguoiUp).TenCoQuan;
            }
            info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
            info.NgayUps = string.Empty;
            if (info.NgayUp != DateTime.MinValue)
            {
                info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                info.NgayUp_str = Format.FormatDateTime(info.NgayUp);
            }
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            //info.ChuyenGiaiQuyetID = Utils.GetInt32(dr["ChuyenGiaiQuyetID"], 0);

            return info;
        }
        private FileHoSoInfo GetDataDonDoc(SqlDataReader dr)
        {
            FileHoSoInfo info = new FileHoSoInfo();
            info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
            info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
            //info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
            info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
            info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
            if (info.NguoiUp > 0)
            {
                info.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(info.NguoiUp).TenCoQuan;
            }
            info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
            info.NgayUps = string.Empty;
            if (info.NgayUp != DateTime.MinValue)
            {
                info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                info.NgayUp_str = Format.FormatDateTime(info.NgayUp);
            }
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
            //info.ChuyenGiaiQuyetID = Utils.GetInt32(dr["ChuyenGiaiQuyetID"], 0);

            return info;
        }

        private FileHoSoInfo GetDataFileYKienXuLy(SqlDataReader dr)
        {
            FileHoSoInfo info = new FileHoSoInfo();
            //info.FileYKienXuLyID = Utils.GetInt32(dr["FileYKienXuLyID"], 0);
            info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
            info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
            info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
            info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
            info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
            info.NgayUps = string.Empty;
            if (info.NgayUp != DateTime.MinValue)
            {
                info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
            }
            info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);

            return info;
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int)

                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertParms(SqlParameter[] parms, FileHoSoInfo info)
        {

            //parms[0].Value = info.SoFileHoSo;
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.DonThuID;
            if (info.NgayUp == Constant.DEFAULT_DATE) parms[2].Value = DBNull.Value;
        }

        //get update parms
        private SqlParameter[] GetUpdateParms()
        {
            SqlParameter[] parms = new SqlParameter[]{

                new SqlParameter(PARAM_FILEHOSO_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)

            };
            return parms;
        }

        //set update parms
        private void SetUpdateParms(SqlParameter[] parms, FileHoSoInfo info)
        {

            parms[0].Value = info.FileHoSoID;
            parms[1].Value = info.TenFile;
            parms[2].Value = info.TomTat;
            parms[3].Value = info.NgayUp;
            parms[4].Value = info.NguoiUp;
            parms[5].Value = info.FileURL;
            parms[6].Value = info.XuLyDonID;


        }

        public IList<FileHoSoInfo> GetAll()
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_ALL, null))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = new FileHoSoInfo();
                        info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return ListDT;
        }

        //get filehoso by donid
        public IList<FileHoSoInfo> GetByDonThuID(int donThuID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int)
            };
            parameters[0].Value = donThuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHU_ID, parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = new FileHoSoInfo();
                        info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        info.FileID = Utils.GetInt32(dr["FileID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        /// <summary>
        /// lấy danh sách file hồ sơ theo đơn thư id (lấy cả file trung đơn),
        /// lấy cả nhóm file, fileid mới
        /// </summary>
        /// <param name="donThuID"></param>
        /// <returns></returns>
        public IList<FileHoSoInfo> GetByDonThuID_New(int donThuID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int)
            };
            parameters[0].Value = donThuID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileHoSo_GetByDonThuID_New", parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = GetData(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetDonThuByType(int donThuID, int fileType)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILETYPE, SqlDbType.Int)
            };
            parameters[0].Value = donThuID;
            parameters[1].Value = fileType;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_DONTHU_TYPE_NEW, parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = new FileHoSoInfo();
                        info.FileHoSoID = Utils.GetInt32(dr["FileHoSoID"], 0);
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.GetBoolean(dr["IsBaoMat"], false);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.FileID = Utils.GetInt32(dr["DM_FileID"], 0);
                        if (info.IsBaoMat == false)
                        {
                            info.IsBaoMatString = "1";
                        }
                        else
                        {
                            info.IsBaoMatString = "2";
                        }

                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }
        public IList<FileHoSoInfo> GetByXuLyDonID(int xulydonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_XULYDON_ID_NEW, parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = GetData(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }


        /// <summary>
        /// lấy các file thuộc:
        /// , file đơn vị đã giải quyết
        /// , file kết quả tiếp
        /// , file kết quả giải quyết
        /// </summary>
        /// <param name="xulydonID"></param>
        /// <returns></returns>
        public IList<FileHoSoInfo> FileTaiLieu_GetByXuLyDonID(int xulydonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileTaiLieu_GetByXuLyDonID", parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = GetData(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        info.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        info.GroupUID = Utils.ConvertToString(dr["GroupUID"], string.Empty);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }



        /// <summary>
        /// lấy ds file hồ sơ của xulydonid theo 2 trường hợp:
        ///  1- nếu đơn thư ko trùng đơn thì lấy ds file của đơn thư đó
        ///  , 2- nếu đơn thư là trùng đơn thì lấy ds file của lần tiếp đón đó và lần tiếp đầu tiên
        /// </summary>
        /// <param name="xulydonID"></param>
        /// <returns></returns>
        public IList<FileHoSoInfo> GetByXuLyDonID_TrungDon(int xulydonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileHoSo_GetByXuLyDonID_TrungDon", parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = GetData(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        info.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        info.GroupUID = Utils.ConvertToString(dr["GroupUID"], string.Empty);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }
        public IList<FileHoSoInfo> GetByXuLyDonID_DonDoc(int xulydonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xulydonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "FileHoSoDonDoc_GetByXuLyDonID_New", parameters))
                {

                    while (dr.Read())
                    {

                        FileHoSoInfo info = GetDataDonDoc(dr);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.IsMaHoa = Utils.ConvertToBoolean(dr["IsMaHoa"], false);
                        info.LoaiFile = Utils.ConvertToInt32(dr["LoaiFile"], 0);
                        info.TomTat = "";
                        info.CanBoID = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListDT;
        }

        public FileHoSoInfo GetByID(int DTID)
        {

            FileHoSoInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_FILEHOSO_ID,SqlDbType.Int)
            };
            parameters[0].Value = DTID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        info = GetData(dr);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return info;
        }

        //-----------delete----------------
        public int Delete(int DT_ID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_FILEHOSO_ID,SqlDbType.Int)
            };
            parameters[0].Value = DT_ID;
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

        public int DeleteFileHoSo(int xuLyDonID, int fileType)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE_DELETE,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = fileType;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_BY_TYPE, parameters);
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

        public int DeleteFileYKienXL(int xuLyDonID, int fileType)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE_DELETE,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = fileType;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_FILEYKIENXULY, parameters);
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

        public int DeleteFileDMBuocXacMinh(int buocXacMinhID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_DMBuocXacMinhID,SqlDbType.Int),
            };
            parameters[0].Value = buocXacMinhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_FileDMBuocXacMinh, parameters);
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

        //------------UPDATE---------------------
        public int Update(FileHoSoInfo info)
        {

            object val = null;
            SqlParameter[] parameters = GetUpdateParms();
            SetUpdateParms(parameters, info);

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
            return Utils.ConvertToInt32(val, 0);
        }


        //------------------INSERT-----------------
        public int Insert(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertParms();
            SetInsertParms(parameters, info);

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
            return Utils.ConvertToInt32(val, 0);
        }

        public int Insert_New(FileHoSoInfo info)
        {
            object val;
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_DONTHU_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)
            };

            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.DonThuID;
            if (info.NgayUp == Constant.DEFAULT_DATE) parms[2].Value = DBNull.Value;
            parms[7].Value = info.FileID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, @"FileHoSo_Insert_New", parms);
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
            return Utils.ConvertToInt32(val, 0);
        }

        //Get Insert Parmas
        private SqlParameter[] GetInsertFileYKienXLParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter("YKienXuLyID", SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)

                };
            return parms;
        }

        private SqlParameter[] GetInsertFileDMBuocXacMinhParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DMBuocXacMinhID, SqlDbType.Int)

                };
            return parms;
        }
        private SqlParameter[] GetInsertDonThuCanPhanGiaiQuyetParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_CHUYENGIAIQUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)

                };
            return parms;
        }

        private SqlParameter[] GetInsertRutDonParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)

                };
            return parms;
        }

        private SqlParameter[] GetInsertBCXMParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                //new SqlParameter(PARAM_FileHoSo_ID, SqlDbType.Int),
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_YKIENGIAIQUYET_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int),

                };
            return parms;
        }

        private SqlParameter[] GetInsertDonThuCDGQParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter("@YKienGiaiQuyetID", SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)

                };
            return parms;
        }

        private SqlParameter[] GetInsertPhanXuLyParms()
        {
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int),

                };
            return parms;
        }

        //SET iNSERT PARMS
        private void SetInsertFileYKienXLParms(SqlParameter[] parms, FileHoSoInfo info)
        {

            //parms[0].Value = info.SoFileHoSo;
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.YKienXuLyID;
            parms[7].Value = info.FileID;

        }

        private void SetInsertFileDMBuocXacMinhParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.DMBuocXacMinhID;

        }

        private void SetInsertDonThuCanPhanGiaiQuyetParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.ChuyenGiaiQuyetID;
            parms[6].Value = info.FileID;

        }
        private void SetInsertRutDonParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.FileID;
        }

        private void SetInsertBCXMParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.YKienGiaiQuyetID;
            parms[6].Value = info.FileID;
        }

        private void SetInsertDonThuCDGQParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.YKienGiaiQuyetID;
            parms[7].Value = info.FileID;
        }

        private void SetInsertPhanXuLyParms(SqlParameter[] parms, FileHoSoInfo info)
        {
            parms[0].Value = info.TenFile;
            parms[1].Value = info.TomTat;
            parms[2].Value = info.NgayUp;
            parms[3].Value = info.NguoiUp;
            parms[4].Value = info.FileURL;
            parms[5].Value = info.XuLyDonID;
            parms[6].Value = info.FileID;
        }
        public int InsertFileYKienXL(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertFileYKienXLParms();
            SetInsertFileYKienXLParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_YKIENXL_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertDMBuocXacMinh(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertFileDMBuocXacMinhParms();
            SetInsertFileDMBuocXacMinhParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_DMBXM, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertRutDon(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertRutDonParms();
            SetInsertRutDonParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_RutDon_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertBCXM(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertBCXMParms();
            SetInsertBCXMParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_BCXM_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertDonThuCDGQ(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertDonThuCDGQParms();
            SetInsertDonThuCDGQParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_DonThuCDGQ_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertPhanXuLy(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertPhanXuLyParms();
            SetInsertPhanXuLyParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_PhanXuLy_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public int InsertFileDonThuCanPhanGiaiQuyet(FileHoSoInfo info)
        {

            object val;

            SqlParameter[] parameters = GetInsertDonThuCanPhanGiaiQuyetParms();
            SetInsertDonThuCanPhanGiaiQuyetParms(parameters, info);

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_DonThuCPGQ_New, parameters);
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
            return Utils.ConvertToInt32(val, 0);
        }

        public FileHoSoInfo FileYKienXLGetByID(int ID)
        {

            FileHoSoInfo info = null;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_FILEYKIENXULY_ID,SqlDbType.Int)
            };
            parameters[0].Value = ID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, FILEYKIENXL_GET_BY_ID, parameters))
                {

                    if (dr.Read())
                    {
                        info = GetDataFileYKienXuLy(dr);
                    }
                    dr.Close();
                }
            }
            catch
            {
            }
            return info;
        }

        public int DeleteFileYKienXL(int fileID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_FILEYKIENXULY_ID,SqlDbType.Int)
            };
            parameters[0].Value = fileID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, DELETE_FILEYKIENXL, parameters);
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

        public IList<FileHoSoInfo> GetFilePhanXuLyByXuLyDonID(int xuLyDonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEPHANXULY_BYXULYDONID_NEW, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.FilePhanXuLyID = Utils.ConvertToInt32(dr["FilePhanXuLyID"], 0);
                        //info.TenFile = Utils.ConvertToString(dr["TenFile"], String.Empty);
                        info.TomTat = Utils.ConvertToString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.ConvertToString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        info.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], String.Empty);
                        info.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.TenCoQuanUp = new CoQuan().GetCoQuanByCanBoID(info.NguoiUp).TenCoQuan;
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.TenFile = Utils.ConvertToString(dr["TenFileNew"], string.Empty);
                        info.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        info.NhomFileID = Utils.ConvertToInt32(dr["NhomFileID"], 0);
                        info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        info.ThuTuHienThiNhom = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        info.ThuTuHienThiFile = Utils.ConvertToInt32(dr["ThuTuHienThiFile"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public int DeleteFileYKXLByXLDonID(int xuLyDonID, int loaiFile)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = loaiFile;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, FILEYKIENXULY_DELETEBYXLDID, parameters);
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

        public IList<FileHoSoInfo> GetFileDonThuCGQByXuLyDonID(int xuLyDonID, int loaiFile)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE, SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            parameters[1].Value = loaiFile;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEDONTHUCDGQ_BYXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo cInfo = new FileHoSoInfo();
                        cInfo.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        cInfo.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        cInfo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        cInfo.NgayUp = Utils.ConvertToDateTime(dr["NgayUp"], DateTime.MinValue);
                        cInfo.NgayUp_str = Format.FormatDate(cInfo.NgayUp);
                        cInfo.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        cInfo.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        ListDT.Add(cInfo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public int DeletFileDonThuCDGQByXuLyDonID(int xuLyDonID)
        {

            int val = 0;
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID,SqlDbType.Int),
            };
            parameters[0].Value = xuLyDonID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, FILEDONTHUCDGQ_DELETEBYXULYDONID, parameters);
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

        public IList<ChuyenGiaiQuyetInfo> GetChuyenGQByXuLyDonID(int xuLyDonID)
        {
            IList<ChuyenGiaiQuyetInfo> ListDT = new List<ChuyenGiaiQuyetInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "ChuyenGiaiQuyet_GetByXuLyDonID", parameters))
                {

                    while (dr.Read())
                    {
                        ChuyenGiaiQuyetInfo item = new ChuyenGiaiQuyetInfo();
                        item.ChuyenGiaiQuyetID = Utils.ConvertToInt32(dr["ChuyenGiaiQuyetID"], 0);
                        ListDT.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetFileYKienXuLyByXuLyDonID(int xuLyDonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEYKIENXLBYXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        if (string.IsNullOrEmpty(info.TenFile))
                        {
                            if (!string.IsNullOrEmpty(info.FileURL))
                            {
                                info.TenFile = info.FileURL.ToString().Substring(info.FileURL.ToString().LastIndexOf("/") + 1);
                            }
                            else
                            {
                                info.TenFile = "File đính kèm";
                            }
                        }
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        //info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }
        String GETFILEYKIENXLBY_LISTXULYDONID = "FileYKIENXL_GetByListXuLyDonID";
        public IList<FileHoSoInfo> GetFileYKienXuLyByListXuLyDon(List<TKDonThuInfo> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();
            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x.XuLyDonID));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEYKIENXLBY_LISTXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        if (string.IsNullOrEmpty(info.TenFile)) info.TenFile = "File đính kèm";
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListDT;
        }
        public IList<FileHoSoInfo> GetFileYKienXuLyByListXuLyDon_New(List<int?> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();
            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEYKIENXLBY_LISTXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        if (string.IsNullOrEmpty(info.TenFile)) info.TenFile = "File đính kèm";
                        //info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        //info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        //info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        //info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        //info.NgayUps = string.Empty;
                        //if (info.NgayUp != DateTime.MinValue)
                        //{
                        //    info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        //}
                        //info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetFileYKienXuLyByListXuLyDon1(List<int> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();
            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEYKIENXLBY_LISTXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        if (string.IsNullOrEmpty(info.TenFile))
                        {
                            if (!string.IsNullOrEmpty(info.FileURL))
                            {
                                info.TenFile = info.FileURL.ToString().Substring(info.FileURL.ToString().LastIndexOf("/") + 1);
                            }
                            else
                            {
                                info.TenFile = "File đính kèm";
                            }
                        }
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        //info.FileURL = Utils.GetString(dr["FileURL"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetFileBanHanhQDByXuLyDonID(int xuLyDonID)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int)
            };
            parameters[0].Value = xuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEBANHANHQDBYXULYDONID, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        if (string.IsNullOrEmpty(info.TenFile))
                        {
                            if (!string.IsNullOrEmpty(info.FileURL))
                            {
                                info.TenFile = info.FileURL.ToString().Substring(info.FileURL.ToString().LastIndexOf("/") + 1);
                            }
                            else
                            {
                                info.TenFile = "File đính kèm";
                            }
                        }
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        String GETFILEBANHANHQDBYLISTXULYDON = "FileBanHanhQD_GetByListXuLyDon";
        public IList<FileHoSoInfo> GetFileBanHanhQDByListXuLyDon(List<TKDonThuInfo> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x.XuLyDonID));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEBANHANHQDBYLISTXULYDON, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        if (string.IsNullOrEmpty(info.TenFile)) info.TenFile = "File đính kèm";
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetFileBanHanhQDByListXuLyDon_New(List<int?> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEBANHANHQDBYLISTXULYDON, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        if (string.IsNullOrEmpty(info.TenFile)) info.TenFile = "File đính kèm";
                        //info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        //info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        //info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        //info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        //info.NgayUps = string.Empty;
                        //if (info.NgayUp != DateTime.MinValue)
                        //{
                        //    info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        //}
                        //info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

        public IList<FileHoSoInfo> GetFileBanHanhQDByListXuLyDon1(List<int> listXuLyDon)
        {
            IList<FileHoSoInfo> ListDT = new List<FileHoSoInfo>();

            var pList = new SqlParameter("@ListXuLyDonID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbListXuLyDonID = new DataTable();
            tbListXuLyDonID.Columns.Add("XuLyDonID", typeof(string));
            listXuLyDon.ForEach(x => tbListXuLyDonID.Rows.Add(x));
            SqlParameter[] parameters = new SqlParameter[]{
                pList
            };
            parameters[0].Value = tbListXuLyDonID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETFILEBANHANHQDBYLISTXULYDON, parameters))
                {
                    while (dr.Read())
                    {
                        FileHoSoInfo info = new FileHoSoInfo();
                        info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        if (string.IsNullOrEmpty(info.TenFile))
                        {
                            if (!string.IsNullOrEmpty(info.FileURL))
                            {
                                info.TenFile = info.FileURL.ToString().Substring(info.FileURL.ToString().LastIndexOf("/") + 1);
                            }
                            else
                            {
                                info.TenFile = "File đính kèm";
                            }
                        }
                        info.TomTat = Utils.GetString(dr["TomTat"], String.Empty);
                        //info.FileURL = Utils.GetString(dr["FileURL"], string.Empty);
                        info.NguoiUp = Utils.GetInt32(dr["NguoiUp"], 0);
                        info.NgayUp = Utils.GetDateTime(dr["NgayUp"], DateTime.MinValue);
                        info.IsBaoMat = Utils.ConvertToBoolean(dr["IsBaoMat"], false);
                        info.NgayUps = string.Empty;
                        if (info.NgayUp != DateTime.MinValue)
                        {
                            info.NgayUps = info.NgayUp.ToString("dd/MM/yyyy");
                        }
                        info.XuLyDonID = Utils.GetInt32(dr["XuLyDonID"], 0);
                        ListDT.Add(info);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ListDT;
        }

    }
}
