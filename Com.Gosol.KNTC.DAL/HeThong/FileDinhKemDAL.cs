using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Com.Gosol.KNTC.Models.KNTC;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class FileDinhKemDAL
    {
        //store procedure
        private const string FILEDINHKEM_GET_BY_FILEID = @"v2_FileDinhKem_GetByID";
        private const string INSERT_FILEDINHKEM = @"v2_FileDinhKem_Insert";
        private const string FILEDINHKEM_GET_BY_NGHIEPVUID = @"v2_FileDinhKem_GetByNghiepVuID";
        private const string DELETE_FILEDINHKEM = @"v2_FielDinhKem_Delete";


        private const string INSERT_FILE_DMBXM = @"FileDMBXM_Insert";
        private const string INSERT_FILE_DMBXM_V2 = @"v2_FileDMBXM_Insert";
        private const string INSERT_FILE_LOG = @"FileLog_Insert";

        //params
        private const string FILE_ID = "FileID";
        private const string TEN_FILE = "TenFile";
        private const string TEN_FILE_HE_THONG = "TenFileHeThong";
        private const string NOI_DUNG = "NoiDung";
        private const string NGAY_CAP_NHAT = "NgayCapNhat";
        private const string NGUOI_CAP_NHAT = "NguoiCapNhat";
        private const string NGHIEP_VU_ID = "NghiepVuID";
        private const string FILE_TYPE = "FileType";
        private const string FILE_URL = "FileUrl";
        private const string DM_TEN_FILEID = "DMTenFileID";
        private const string ISMAHOA = "IsMaHoa";
        private const string ISBAOMAT = "IsBaoMat";

        private const string PARAM_TEN_FILE = "@TenFile";
        private const string PARAM_TOMTAT = "@TomTat";
        private const string PARAM_FILE_URL = "@FileURL";
        private const string PARAM_NGUOIUP = "@NguoiUp";
        private const string PARAM_NGAYUP = "@NgayUp";
        private const string PARAM_DMBuocXacMinhID = "@DMBuocXacMinhID";
        private const string PARAM_DM_TEN_FILEID = "@DMTenFileID";

        private const string PARAM_FILEID = "@FileID";
        private const string PARAM_LOAILOG = "@LoaiLog";
        private const string PARAM_LOAIFILE = "@LoaiFile";
        private const string PARAM_ISMAHOA = "@IsMaHoa";
        private const string PARAM_ISBAOMAT = "@IsBaoMat";

        private const string PARAM_THI_HANH_ID = "@ThiHanhID";
        private const string PARAM_DONTHU_ID = "@DonThuID";
        private const string PARAM_FILE_ID = "@FileID";
        private const string PARAM_XULYDON_ID = "@XuLyDonID";
        private const string PARAM_YKIENGIAIQUYET_ID = "@YKienGiaiQuyetID";
        private const string PARAM_CHUYENGIAIQUYET_ID = "@ChuyenGiaiQuyetID";
        private const string PARM_THEODOIXULYID = @"TheoDoiXuLyID";
        private const string PARM_NOIDUNG = @"NoiDung";
        private const string PARM_NGAYCAPNHAT = @"NgayCapNhat";
        private const string PARM_DUONGDANFILE = @"DuongDanFile";
        private const string PARM_TENFILE = @"TenFile";
        private const string PARAM_KET_QUA_ID = "@KetQuaID";

        private const string PARM_KETQUATRANHCHAPID = @"KetQuaTranhChapID";
        private const string PARM_CANBOUPID = @"CanBoUpID";
        private const string PARM_TOMTAT = @"TomTat";

        public FileDinhKemModel GetByID(int FileDinhKemID, int FileType)
        {
            FileDinhKemModel crFile = new FileDinhKemModel();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter(FILE_ID, SqlDbType.Int),
                new SqlParameter(FILE_TYPE,SqlDbType.Int)
             };
            parameters[0].Value = FileDinhKemID;
            parameters[1].Value = FileType;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, FILEDINHKEM_GET_BY_FILEID, parameters))
                {
                    while (dr.Read())
                    {
                        crFile = new FileDinhKemModel();
                        crFile.FileID = Utils.ConvertToInt32(dr[FILE_ID], 0);
                        crFile.NguoiCapNhat = Utils.ConvertToInt32(dr[NGUOI_CAP_NHAT], 0);
                        crFile.NgayCapNhat = Utils.ConvertToDateTime(dr[NGAY_CAP_NHAT], DateTime.Now);
                        crFile.TenFile = Utils.ConvertToString(dr[TEN_FILE], string.Empty);
                        crFile.NoiDung = Utils.ConvertToString(dr[NOI_DUNG], string.Empty);
                        crFile.FileType = Utils.ConvertToInt32(dr[FILE_TYPE], 0);
                        crFile.NghiepVuID = Utils.ConvertToInt32(dr[NGHIEP_VU_ID], 0);
                        crFile.DMTenFileID = Utils.ConvertToInt32(dr[DM_TEN_FILEID], 0);
                        crFile.FileUrl = Utils.ConvertToString(dr[FILE_URL], string.Empty);
                        crFile.IsBaoMat = Utils.ConvertToBoolean(dr[ISBAOMAT], false);
                        crFile.IsMaHoa = Utils.ConvertToBoolean(dr[ISMAHOA], false);
                        // bổ sung tên File hệ thống
                        crFile.TenFileHeThong = Utils.ConvertToString(dr[TEN_FILE_HE_THONG], string.Empty);
                        break;
                    }
                    dr.Close();
                }
                return crFile;
            }
            catch (Exception ex)
            {
                return new FileDinhKemModel();
                throw ex;
            }
        }

        public List<FileDinhKemModel> GetByNgiepVuID(int NghiepVuID, int FileType)
        {
            List<FileDinhKemModel> listFile = new List<FileDinhKemModel>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter(NGHIEP_VU_ID, SqlDbType.Int),
                new SqlParameter(FILE_TYPE,SqlDbType.Int)
             };
            parameters[0].Value = NghiepVuID;
            parameters[1].Value = FileType;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, FILEDINHKEM_GET_BY_NGHIEPVUID, parameters))
                {
                    while (dr.Read())
                    {
                        var crFile = new FileDinhKemModel();
                        crFile.FileID = Utils.ConvertToInt32(dr[FILE_ID], 0);
                        crFile.NguoiCapNhat = Utils.ConvertToInt32(dr[NGUOI_CAP_NHAT], 0);
                        crFile.NgayCapNhat = Utils.ConvertToDateTime(dr[NGAY_CAP_NHAT], DateTime.Now);
                        crFile.TenFile = Utils.ConvertToString(dr[TEN_FILE], string.Empty);
                        crFile.NoiDung = Utils.ConvertToString(dr[NOI_DUNG], string.Empty);
                        crFile.FileType = Utils.ConvertToInt32(dr[FILE_TYPE], 0);
                        crFile.NghiepVuID = Utils.ConvertToInt32(dr[NGHIEP_VU_ID], 0);
                        crFile.DMTenFileID = Utils.ConvertToInt32(dr[DM_TEN_FILEID], 0);
                        crFile.FileUrl = Utils.ConvertToString(dr[FILE_URL], string.Empty);
                        crFile.IsBaoMat = Utils.ConvertToBoolean(dr[ISBAOMAT], false);
                        crFile.IsMaHoa = Utils.ConvertToBoolean(dr[ISMAHOA], false);
                        listFile.Add(crFile);
                    }
                    dr.Close();
                }
                return listFile;
            }
            catch (Exception ex)
            {
                return new List<FileDinhKemModel>();
                throw ex;
            }
        }

        public BaseResultModel Insert(FileDinhKemModel FileDinhKemModel)
        {
            var Result = new BaseResultModel();
            try
            {
                if (FileDinhKemModel == null || FileDinhKemModel.TenFile == null || FileDinhKemModel.TenFile.Trim().Length < 1)
                {
                    Result.Status = 0;
                    Result.Message = "Tên file gốc không được trống";
                    return Result;
                }
                else
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                         new SqlParameter(TEN_FILE_HE_THONG, SqlDbType.NVarChar),
                         new SqlParameter(NGAY_CAP_NHAT, SqlDbType.DateTime),
                         new SqlParameter(NGUOI_CAP_NHAT, SqlDbType.Int),
                         new SqlParameter(NGHIEP_VU_ID, SqlDbType.Int),
                         new SqlParameter(FILE_TYPE, SqlDbType.NVarChar),
                         new SqlParameter(FILE_URL, SqlDbType.NVarChar),
                    };
                    parameters[0].Value = FileDinhKemModel.TenFile.Trim();
                    parameters[1].Value = FileDinhKemModel.NgayCapNhat;
                    parameters[2].Value = FileDinhKemModel.NguoiCapNhat;
                    parameters[3].Value = FileDinhKemModel.NghiepVuID ?? Convert.DBNull;
                    parameters[4].Value = FileDinhKemModel.FileType;
                    parameters[5].Value = FileDinhKemModel.FileUrl ?? Convert.DBNull;

                    using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                    {
                        conn.Open();
                        using (SqlTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                var query = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILEDINHKEM, parameters), 0);
                                trans.Commit();
                                if (query > 0)
                                {
                                    Result.Status = 1; Result.Data = query;
                                    Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                                }
                            }
                            catch (Exception ex)
                            {
                                Result.Status = -1;
                                Result.Message = ConstantLogMessage.API_Error_System;
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        public BaseResultModel UpdateNghiepVuID(List<int> ListFileDinhKemID, int NghiepVuID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("NghiepVuID", SqlDbType.Int),
                      pList,
                };
                parameters[0].Value = NghiepVuID;
                parameters[1].Value = tbFileID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileDinhKem_UpdateNghiepVuID", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        public BaseResultModel UpdateNghiepVuID_New(List<int> ListFileDinhKemID, int NghiepVuID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("NghiepVuID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = NghiepVuID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileDinhKem_UpdateNghiepVuID_New", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }

        public BaseResultModel UpdateFileHoSo(List<int> ListFileDinhKemID, int XuLyDonID, int DonThuID, string? gropID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {

                      new SqlParameter("XuLyDonID", SqlDbType.Int),
                      new SqlParameter("DonThuID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = XuLyDonID;
                parameters[1].Value = DonThuID;
                parameters[2].Value = tbFileID;
                parameters[3].Value = string.IsNullOrEmpty(gropID) ? DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid() : gropID;

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileHoSo_Update_XuLyDonID_DonThuID", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileXuLyDon(List<int> ListFileDinhKemID, int XuLyDonID, int YKienXuLyID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("XuLyDonID", SqlDbType.Int),
                      new SqlParameter("YKienXuLyID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = XuLyDonID;
                parameters[1].Value = YKienXuLyID;
                parameters[2].Value = tbFileID;
                parameters[3].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileXuLy_Update_XuLyDonID_YKienXuLyID", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileGiaoXacMinh(List<int> ListFileDinhKemID, int ChuyenGiaiQuyetID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("ChuyenGiaiQuyetID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = ChuyenGiaiQuyetID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileDonThuCPGQ_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileGiaiQuyet(List<int> ListFileDinhKemID, int TheoDoiXuLyID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("TheoDoiXuLyID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = TheoDoiXuLyID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileGiaiQuyet_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileVBDonDoc(List<int> ListFileDinhKemID, int DonDocID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("DonDocID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = DonDocID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileDonDoc_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileThiHanh(List<int> ListFileDinhKemID, int ThiHanhID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("ThiHanhID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = ThiHanhID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileThiHanh_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileTranhChap(List<int> ListFileDinhKemID, int KetQuaTranhChapID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("KetQuaTranhChapID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = KetQuaTranhChapID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileTranhChap_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileGiaHanGiaiQuyet(List<int> ListFileDinhKemID, int GiaHanGiaiQuyetID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("TheoDoiXuLyID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = GiaHanGiaiQuyetID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileGiaiQuyet_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel UpdateFileBanHanhQDGQ(List<int> ListFileDinhKemID, int KetQuaID)
        {
            var Result = new BaseResultModel();
            try
            {
                var listID = ListFileDinhKemID.Distinct().ToList();

                var pList = new SqlParameter("@ListID", SqlDbType.Structured);
                pList.TypeName = "dbo.list_ID";
                var tbFileID = new DataTable();
                tbFileID.Columns.Add("ID", typeof(string));
                listID.ForEach(x => tbFileID.Rows.Add(x));

                SqlParameter[] parameters = new SqlParameter[]
                {
                      new SqlParameter("KetQuaID", SqlDbType.Int),
                      pList,
                      new SqlParameter("GroupUID", SqlDbType.NVarChar),
                };
                parameters[0].Value = KetQuaID;
                parameters[1].Value = tbFileID;
                parameters[2].Value = DateTime.Now.ToString("ddMMyyHHmmss") + Guid.NewGuid();

                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var query = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "v2_FileBanHanhQDGQ_Update", parameters);
                            trans.Commit();
                            if (query > 0)
                            {
                                Result.Status = 1; Result.Data = query;
                                Result.Message = ConstantLogMessage.Alert_Insert_Success("File đính kèm");
                            }
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                throw;
            }
            return Result;
        }
        public BaseResultModel Delete(List<FileDinhKemModel> ListFileDinhKem)
        {
            var Result = new BaseResultModel();
            if (ListFileDinhKem.Count <= 0)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng chọn dữ liệu trước khi xóa";
                return Result;
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in ListFileDinhKem)
                            {
                                SqlParameter[] parameters = new SqlParameter[]
                                {
                                    new SqlParameter(FILE_ID, SqlDbType.Int),
                                    new SqlParameter(FILE_TYPE,SqlDbType.Int)
                                };
                                parameters[0].Value = item.FileID;
                                parameters[1].Value = item.FileType;
                                var val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, DELETE_FILEDINHKEM, parameters);
                            }

                            trans.Commit();
                            Result.Status = 1;
                        }
                        catch
                        {
                            Result.Status = -1;
                            Result.Message = ConstantLogMessage.API_Error_System;
                            trans.Rollback();
                            return Result;
                            throw;
                        }
                    }
                }
                if (Result.Status == 1)
                {
                    try
                    {
                        foreach (var item in ListFileDinhKem)
                        {
                            File.Delete(item.FileUrl);
                        }
                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                }
                Result.Message = ConstantLogMessage.Alert_Delete_Success("File đính kèm");
                return Result;
            }
        }

        public BaseResultModel CopyFile(string Url, string UrlNew)
        {
            var Result = new BaseResultModel();
            try
            {
                File.Copy(Url, UrlNew, true);
                Result.Status = 1;
            }
            catch (Exception)
            {
                Result.Status = 0;
                throw;
            }

            return Result;
        }

        public void ClearFolderFileTemp(string FolderPath)
        {
            //File.
        }

        public int InsertDMBuocXacMinh(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_DMBuocXacMinhID, SqlDbType.Int),
                new SqlParameter(PARAM_DM_TEN_FILEID, SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull; ;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? Convert.DBNull;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_DMBXM_V2, parms);
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
        public int InsertFileHoSo(FileDinhKemModel info)
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
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int),

            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? 0;
            parms[6].Value = info.NghiepVuID ?? 0;
            if (info.NgayCapNhat == Constant.DEFAULT_DATE) parms[2].Value = DBNull.Value;
            parms[7].Value = info.DMTenFileID ?? Convert.DBNull;

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
        public int InsertFileXuLyDon(FileDinhKemModel info)
        {
            object val;
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_XULYDON_ID, SqlDbType.Int),
                new SqlParameter("YKienXuLyID", SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? 0;
            parms[6].Value = info.NghiepVuID ?? 0;
            parms[7].Value = info.DMTenFileID ?? Convert.DBNull;
            if (info.NgayCapNhat == Constant.DEFAULT_DATE) parms[2].Value = DBNull.Value;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, @"FileYKienXuLy_Insert_New", parms);
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
        public int InsertFileGiaoXacMinh(FileDinhKemModel info)
        {
            object val;
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

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? 0;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;
            if (info.NgayCapNhat == Constant.DEFAULT_DATE) parms[2].Value = DBNull.Value;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, @"FileDonThuCPGQ_Insert_New", parms);
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
        public int InsertFileGiaiQuyet(FileDinhKemModel info)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARM_THEODOIXULYID, SqlDbType.Int),
                new SqlParameter(PARM_NOIDUNG, SqlDbType.NText),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(PARM_DUONGDANFILE, SqlDbType.NVarChar,2000),
                new SqlParameter(PARM_TENFILE,SqlDbType.NVarChar,100),
                new SqlParameter(PARAM_FILEID,SqlDbType.Int)
            };

            parms[0].Value = info.NghiepVuID ?? 0;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.FileUrl ?? Convert.DBNull;
            parms[4].Value = info.TenFile ?? Convert.DBNull;
            parms[5].Value = info.DMTenFileID ?? Convert.DBNull;

            object val;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, @"FileGiaiQuyet_Insert_New", parms);
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
        public int InsertFileVBDonDoc(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TenFile", SqlDbType.NVarChar),
                new SqlParameter("@NgayUp", SqlDbType.DateTime),
                new SqlParameter("@NguoiUp", SqlDbType.Int),
                new SqlParameter("@FileURL", SqlDbType.NVarChar),
                new SqlParameter("@DonDocID", SqlDbType.Int),
                new SqlParameter("@FileHoSoID", SqlDbType.Int),
                new SqlParameter("@FileID", SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NgayCapNhat;
            parms[2].Value = info.NguoiCapNhat ?? Convert.DBNull; ;
            parms[3].Value = info.FileUrl ?? Convert.DBNull;
            parms[4].Value = info.NghiepVuID ?? Convert.DBNull;
            parms[5].Direction = ParameterDirection.Output;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "Insert_FileDonDoc_New", parms);
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
            return Utils.ConvertToInt32(parms[5].Value, 0);
        }
        public int InsertFileThiHanh(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_THI_HANH_ID, SqlDbType.Int),
                new SqlParameter("@FileID", SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? 0;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "FileThiHanh_Insert_New", parms);
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
        public int InsertFileTranhChap(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARM_KETQUATRANHCHAPID, SqlDbType.Int),
                new SqlParameter(PARM_CANBOUPID, SqlDbType.Int),
                new SqlParameter(PARM_TENFILE, SqlDbType.NVarChar, 200),
                new SqlParameter(PARM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARM_NGAYCAPNHAT, SqlDbType.DateTime),
                new SqlParameter(FILE_URL, SqlDbType.VarChar, 200),
                new SqlParameter("@FileID", SqlDbType.Int)
            };

            parms[0].Value = info.NghiepVuID ?? 0;
            parms[1].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[2].Value = info.TenFile ?? Convert.DBNull;
            parms[3].Value = info.NoiDung ?? Convert.DBNull;
            parms[4].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[5].Value = info.FileUrl ?? Convert.DBNull;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "FileKQTranhChap_Insert_New", parms);
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
        public int InsertFileGiaHanGiaiQuyet(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter("@TenFile", SqlDbType.NVarChar),
                new SqlParameter("@NgayUp", SqlDbType.DateTime),
                new SqlParameter("@NguoiUp", SqlDbType.Int),
                new SqlParameter("@FileURL", SqlDbType.NVarChar),
                new SqlParameter("@GiaHanGiaiQuyetID", SqlDbType.Int),
                new SqlParameter("@FileHoSoID", SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[2].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[3].Value = info.FileUrl ?? Convert.DBNull;
            parms[4].Value = info.NghiepVuID ?? 0;
            parms[5].Direction = ParameterDirection.Output;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "Insert_FileGiaHanGiaiQuyet", parms);
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
        public int InsertFileBanHanhQDGQ(FileDinhKemModel info)
        {
            object val;

            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_TEN_FILE, SqlDbType.NVarChar),
                new SqlParameter(PARAM_TOMTAT, SqlDbType.NVarChar),
                new SqlParameter(PARAM_NGAYUP, SqlDbType.DateTime),
                new SqlParameter(PARAM_NGUOIUP, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_URL, SqlDbType.NVarChar),
                new SqlParameter(PARAM_KET_QUA_ID, SqlDbType.Int),
                new SqlParameter(PARAM_FILE_ID, SqlDbType.Int)
            };

            parms[0].Value = info.TenFile ?? Convert.DBNull;
            parms[1].Value = info.NoiDung ?? Convert.DBNull;
            parms[2].Value = info.NgayCapNhat ?? Convert.DBNull;
            parms[3].Value = info.NguoiCapNhat ?? Convert.DBNull;
            parms[4].Value = info.FileUrl ?? Convert.DBNull;
            parms[5].Value = info.NghiepVuID ?? Convert.DBNull;
            parms[6].Value = info.DMTenFileID ?? Convert.DBNull;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "FileKetQua_Insert_New", parms);
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
        public int InsertFileLog(FileLogInfo DTInfo)
        {
            object val;
            SqlParameter[] parms = new SqlParameter[]{
                new SqlParameter(PARAM_FILEID, SqlDbType.Int),
                new SqlParameter(PARAM_LOAILOG, SqlDbType.Int),
                new SqlParameter(PARAM_LOAIFILE, SqlDbType.Int),
                new SqlParameter(PARAM_ISMAHOA, SqlDbType.Int),
                new SqlParameter(PARAM_ISBAOMAT, SqlDbType.Int)
            };
            parms[0].Value = DTInfo.FileID;
            parms[1].Value = DTInfo.LoaiLog;
            parms[2].Value = DTInfo.LoaiFile;
            if (DTInfo.IsBaoMat == true)
            {
                parms[3].Value = 1;
                parms[4].Value = 1;
            }
            else
            {

                parms[3].Value = 0;
                parms[4].Value = 0;
            }

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, INSERT_FILE_LOG, parms);
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
    }
}
