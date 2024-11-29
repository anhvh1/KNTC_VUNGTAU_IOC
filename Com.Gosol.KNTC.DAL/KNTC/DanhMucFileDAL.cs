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
    public class DanhMucFileDAL
    {
        #region Nhóm file
        /// <summary>
        /// lấy danh sách nhóm file phân trang và tìm kiếm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="keyword"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public IList<DanhMucNhomFileInfo> NhomFile_GetBySearch(int start, int end, string keyword, ref int TotalRow)
        {
            IList<DanhMucNhomFileInfo> result = new List<DanhMucNhomFileInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Keyword", SqlDbType.NVarChar, 200),
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),
            };
            if (keyword == "null")
            {
                keyword = "";
            }
            parameters[0].Value = keyword;
            parameters[1].Value = start;
            parameters[2].Value = end;
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_NhomFile_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucNhomFileInfo Info = new DanhMucNhomFileInfo();
                        Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        Info.TenNhomFile = Utils.GetString(dr["TenNhomFile"], String.Empty);
                        Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        result.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[3].Value, 0);
            }
            catch { throw; }
            return result;
        }

        /// <summary>
        /// lấy tất cả nhóm file trong hệ thống
        /// </summary>
        /// <returns></returns>
        public IList<DanhMucNhomFileInfo> NhomFile_GetAll()
        {
            IList<DanhMucNhomFileInfo> result = new List<DanhMucNhomFileInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_NhomFile_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucNhomFileInfo Info = new DanhMucNhomFileInfo();
                        Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        Info.TenNhomFile = Utils.GetString(dr["TenNhomFile"], String.Empty);
                        Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        result.Add(Info);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }


        /// <summary>
        /// lấy dánh sách nhóm file đang được sử dụng
        /// </summary>
        /// <returns></returns>
        public IList<DanhMucNhomFileInfo> NhomFile_Get_DanhSachDangSuDung()
        {
            IList<DanhMucNhomFileInfo> result = new List<DanhMucNhomFileInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_NhomFile_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucNhomFileInfo Info = new DanhMucNhomFileInfo();
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        if (Info.TrangThaiSuDung == true)
                        {
                            Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                            Info.TenNhomFile = Utils.GetString(dr["TenNhomFile"], String.Empty);
                            Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                            Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                            result.Add(Info);
                        }
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return result;
        }

        /// <summary>
        /// lấy thông tin nhóm file theo id
        /// </summary>
        /// <param name="NhomFileID"></param>
        /// <returns></returns>
        public DanhMucNhomFileInfo NhomFile_GetByID(int NhomFileID)
        {
            DanhMucNhomFileInfo Info = new DanhMucNhomFileInfo();
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@NhomFileID", SqlDbType.Int) };
            parameters[0].Value = NhomFileID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_NhomFile_GetByID", parameters))
                {
                    if (dr.Read())
                    {
                        Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        Info.TenNhomFile = Utils.GetString(dr["TenNhomFile"], String.Empty);
                        Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return Info;
                throw ex;
            }
            return Info;
        }

        /// <summary>
        /// xoá nhóm file -- chú ý kiểm tra sự tồn tại của file đang có nhóm cần xoá
        /// </summary>
        /// <param name="NhomFileID"></param>
        /// <returns></returns>
        public int NhomFile_Delete(int NhomFileID)
        {
            int val = 0;
            // kiểm tra sự tồn tại của file trong nhóm đang muốn xoá
            if (File_GetBy_NhomFileID(NhomFileID).Count > 0)
            {
                return 0;
            }
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@NhomFileID", SqlDbType.Int)
            };
            parameters[0].Value = NhomFileID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_NhomFile_Delete", parameters);
                        trans.Commit();
                        val = 1;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return val;
        }


        /// <summary>
        /// sửa nhóm file
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int NhomFile_Update(DanhMucNhomFileInfo info)
        {
            int val = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@NhomFileID", SqlDbType.Int),
                new SqlParameter("@TenNhomFile", SqlDbType.NVarChar, 2000),
                new SqlParameter("@ThuTuHienThi", SqlDbType.Int),
                new SqlParameter("@TrangThaiSuDung", SqlDbType.Bit),
            };
            parms[0].Value = info.NhomFileID;
            parms[1].Value = info.TenNhomFile;
            parms[2].Value = info.ThuTuHienThi;
            parms[3].Value = info.TrangThaiSuDung;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_NhomFile_Update", parms);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return val;
        }

        /// <summary>
        /// thêm nhóm file
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int NhomFile_Insert(DanhMucNhomFileInfo info)
        {
            object val = null;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@TenNhomFile", SqlDbType.NVarChar,2000),
                new SqlParameter("@ThuTuHienThi", SqlDbType.Int),
                new SqlParameter("@TrangThaiSuDung", SqlDbType.Bit),
            };
            parms[0].Value = info.TenNhomFile;
            parms[1].Value = info.ThuTuHienThi;
            parms[2].Value = info.TrangThaiSuDung;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, "DM_NhomFile_Insert", parms);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return Convert.ToInt32(val);
        }
        #endregion

        #region file

        /// <summary>
        /// lấy danh sách phân trang file theo bộ lọc, tìm kiếm
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="keyword"></param>
        /// <param name="chucNangID"></param>
        /// <param name="nhomFileID"></param>
        /// <param name="TotalRow"></param>
        /// <returns></returns>
        public List<DanhMucFileInfo> File_GetBySearch(int start, int end, string keyword, int chucNangID, int nhomFileID, ref int TotalRow)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Keyword", SqlDbType.NVarChar, 200),
                new SqlParameter("@ChucNangID", SqlDbType.Int),
                new SqlParameter("@NhomFileID", SqlDbType.Int),
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),
            };
            if (keyword == "null")
            {
                keyword = "";
            }
            parameters[0].Value = keyword;
            parameters[1].Value = chucNangID;
            parameters[2].Value = nhomFileID;
            parameters[3].Value = start;
            parameters[4].Value = end;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            List<DanhMucFileInfo> DanhSachFile = new List<DanhMucFileInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_GetBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucFileInfo Info = new DanhMucFileInfo();
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        Info.FileID = Utils.GetInt32(dr["FileID"], 0);
                        Info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        Info.TenNhomFile = Utils.ConvertToString(dr["TenNhomFile"], string.Empty);
                        var dsChucNang = FileChucNangApDung_GetBy_FileID(Info.FileID);
                        Info.DanhSachChucNangID = dsChucNang.Select(x => x.ChucNangID).ToList();
                        Info.DanhSachTenChucNang = dsChucNang.Select(x => x.TenChucNang).ToList();
                        DanhSachFile.Add(Info);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch { throw; }
            return DanhSachFile;
        }


        /// <summary>
        /// lấy tất cả file trong hệ thống
        /// </summary>
        /// <returns></returns>
        public List<DanhMucFileInfo> File_GetAll()
        {
            List<DanhMucFileInfo> result = new List<DanhMucFileInfo>();
            List<DanhMucFileInfo> DanhSachFile = new List<DanhMucFileInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucFileInfo Info = new DanhMucFileInfo();
                        Info.FileID = Utils.GetInt32(dr["FileID"], 0);
                        Info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        Info.ChucNangID = Utils.GetInt32(dr["ChucNangID"], 0);
                        DanhSachFile.Add(Info);
                    }
                    dr.Close();
                }
                if (DanhSachFile != null && DanhSachFile.Count > 0)
                {
                    result = (from m in DanhSachFile
                              group m by new { m.FileID, m.TenFile, m.ThuTuHienThi, m.TrangThaiSuDung, m.NhomFileID } into file
                              select new DanhMucFileInfo
                              {
                                  FileID = file.Key.FileID,
                                  TenFile = file.Key.TenFile,
                                  ThuTuHienThi = file.Key.ThuTuHienThi,
                                  TrangThaiSuDung = file.Key.TrangThaiSuDung,
                                  NhomFileID = file.Key.NhomFileID,
                                  DanhSachChucNangID = DanhSachFile.Select(x => x.ChucNangID).ToList()
                              }
                            ).ToList();
                }
            }
            catch { throw; }
            return result;
        }

        /// <summary>
        /// Lấy danh sách file đang được sử dụng
        /// </summary>
        /// <returns></returns>
        public List<DanhMucFileInfo> File_Get_DanhSachDangSuDung()
        {
            List<DanhMucFileInfo> result = new List<DanhMucFileInfo>();
            List<DanhMucFileInfo> DanhSachFile = new List<DanhMucFileInfo>();
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_GetAll"))
                {
                    while (dr.Read())
                    {
                        DanhMucFileInfo Info = new DanhMucFileInfo();
                        Info.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        if (Info.TrangThaiSuDung == true)
                        {
                            Info.FileID = Utils.GetInt32(dr["FileID"], 0);
                            Info.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                            Info.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                            Info.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                            Info.ChucNangID = Utils.GetInt32(dr["ChucNangID"], 0);
                            DanhSachFile.Add(Info);
                        }
                    }
                    dr.Close();
                }
                if (DanhSachFile != null && DanhSachFile.Count > 0)
                {
                    result = (from m in DanhSachFile
                              group m by new { m.FileID, m.TenFile, m.ThuTuHienThi, m.TrangThaiSuDung, m.NhomFileID } into file
                              select new DanhMucFileInfo
                              {
                                  FileID = file.Key.FileID,
                                  TenFile = file.Key.TenFile,
                                  ThuTuHienThi = file.Key.ThuTuHienThi,
                                  TrangThaiSuDung = file.Key.TrangThaiSuDung,
                                  NhomFileID = file.Key.NhomFileID,
                                  DanhSachChucNangID = DanhSachFile.Where(x => x.FileID == file.Key.FileID).Select(x => x.ChucNangID).ToList()
                              }
                            ).ToList();
                }
            }
            catch { throw; }
            return result;
        }


        /// <summary>
        /// láy thông tin file theo id
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public DanhMucFileInfo File_GetByID(int FileID)
        {
            List<DanhMucFileInfo> DanhSachFile = new List<DanhMucFileInfo>();
            var Info = new DanhMucFileInfo();
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@FileID", SqlDbType.Int) };
            parameters[0].Value = FileID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        var item = new DanhMucFileInfo();
                        item.FileID = Utils.GetInt32(dr["FileID"], 0);
                        item.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        item.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        item.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        item.ChucNangID = Utils.GetInt32(dr["ChucNangID"], 0);
                        DanhSachFile.Add(item);
                    }
                    dr.Close();
                }
                if (DanhSachFile != null && DanhSachFile.Count > 0)
                {
                    Info = (from m in DanhSachFile
                            group m by new { m.FileID, m.TenFile, m.ThuTuHienThi, m.TrangThaiSuDung, m.NhomFileID } into file
                            select new DanhMucFileInfo
                            {
                                FileID = file.Key.FileID,
                                TenFile = file.Key.TenFile,
                                ThuTuHienThi = file.Key.ThuTuHienThi,
                                TrangThaiSuDung = file.Key.TrangThaiSuDung,
                                NhomFileID = file.Key.NhomFileID,
                                DanhSachChucNangID = DanhSachFile.Select(x => x.ChucNangID).ToList()
                            }
                            ).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                return Info;
                throw ex;
            }
            return Info;
        }


        /// <summary>
        /// lấy danh sách file theo nhomfileid
        /// có thể dùng để check sự tồn tại của file khi xoá nhóm file
        /// </summary>
        /// <param name="NhomFileID"></param>
        /// <returns></returns>
        public List<DanhMucFileInfo> File_GetBy_NhomFileID(int NhomFileID)
        {
            List<DanhMucFileInfo> DanhSachFile = new List<DanhMucFileInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@NhomFileID", SqlDbType.Int) };
            parameters[0].Value = NhomFileID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_GetBy_NhomFileID", parameters))
                {
                    while (dr.Read())
                    {
                        var item = new DanhMucFileInfo();
                        item.FileID = Utils.GetInt32(dr["FileID"], 0);
                        item.TenFile = Utils.GetString(dr["TenFile"], String.Empty);
                        item.ThuTuHienThi = Utils.ConvertToInt32(dr["ThuTuHienThi"], 0);
                        item.TrangThaiSuDung = Utils.ConvertToBoolean(dr["TrangThaiSuDung"], false);
                        item.NhomFileID = Utils.GetInt32(dr["NhomFileID"], 0);
                        DanhSachFile.Add(item);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                return DanhSachFile;
                throw ex;
            }
            return DanhSachFile;
        }


        /// <summary>
        /// xoá file 
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public int File_Delete(int FileID)
        {
            int val = 0;
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@FileID", SqlDbType.Int)
            };
            parameters[0].Value = FileID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_File_Delete", parameters);
                        trans.Commit();
                        val = 1;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return val;
        }


        /// <summary>
        /// sửa file
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int File_Update(DanhMucFileInfo info)
        {
            int val = 0;
            var pList = new SqlParameter("DanhSachChucNangID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbChucNangID = new DataTable();
            tbChucNangID.Columns.Add("n", typeof(int));
            if (info.DanhSachChucNangID.Count > 0)
            {
                info.DanhSachChucNangID.ForEach(x => tbChucNangID.Rows.Add(x));
            }
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@FileID", SqlDbType.Int),
                new SqlParameter("@TenFile", SqlDbType.NVarChar,2000),
                new SqlParameter("@ThuTuHienThi", SqlDbType.Int),
                new SqlParameter("@TrangThaiSuDung", SqlDbType.Bit),
                new SqlParameter("@NhomFileID", SqlDbType.Int),
                pList,
            };
            parms[0].Value = info.FileID;
            parms[1].Value = info.TenFile;
            parms[2].Value = info.ThuTuHienThi;
            parms[3].Value = info.TrangThaiSuDung;
            parms[4].Value = info.NhomFileID;
            parms[5].Value = tbChucNangID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_File_Update", parms);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return val;
        }


        /// <summary>
        /// thêm file
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int File_Insert(DanhMucFileInfo info)
        {
            object val = null;
            var pList = new SqlParameter("DanhSachChucNangID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbChucNangID = new DataTable();
            tbChucNangID.Columns.Add("n", typeof(int));
            if (info.DanhSachChucNangID.Count > 0)
            {
                info.DanhSachChucNangID.ForEach(x => tbChucNangID.Rows.Add(x));
            }
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@TenFile", SqlDbType.NVarChar,2000),
                new SqlParameter("@ThuTuHienThi", SqlDbType.Int),
                new SqlParameter("@TrangThaiSuDung", SqlDbType.Bit),
                new SqlParameter("@NhomFileID", SqlDbType.Int),
                pList,
            };
            parms[0].Value = info.TenFile;
            parms[1].Value = info.ThuTuHienThi;
            parms[2].Value = info.TrangThaiSuDung;
            parms[3].Value = info.NhomFileID;
            parms[4].Value = tbChucNangID;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_File_Insert", parms);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            return -1;
                            throw ex;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
            return Convert.ToInt32(val);
        }
        public int File_MultilInsert(List<DanhMucFileInfo> DanhSachFile)
        {
            int val = 0;
            try
            {
                if (DanhSachFile != null && DanhSachFile.Count > 0)
                {
                    foreach (var item in DanhSachFile)
                    {
                        var query = File_Insert(item);
                        if (query < 1)
                        {
                            return query;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
            return val;
        }

        #endregion

        #region file_ChucNangApDung

        /// <summary>
        /// lấy danh sách chức năng áp dụng cho file theo fileid
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>
        public List<DanhMucFileChucNangApDungInfo> FileChucNangApDung_GetBy_FileID(int FileID)
        {
            List<DanhMucFileChucNangApDungInfo> Result = new List<DanhMucFileChucNangApDungInfo>();
            SqlParameter[] parameters = new SqlParameter[] {
            new SqlParameter("@FileID", SqlDbType.Int) };
            parameters[0].Value = FileID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "DM_File_Get_DanhSachChucNang_ByFileID", parameters))
                {
                    while (dr.Read())
                    {
                        var item = new DanhMucFileChucNangApDungInfo();
                        item.FileID = Utils.GetInt32(dr["FileID"], 0);
                        item.FileChucNangID = Utils.GetInt32(dr["FileChucNangID"], 0);
                        item.ChucNangID = Utils.GetInt32(dr["ChucNangID"], 0);
                        item.TenChucNang = Utils.ConvertToString(dr["TenChucNang"], string.Empty);
                        Result.Add(item);
                    }
                    dr.Close();
                }
            }
            catch { throw; }
            return Result;
        }
        #endregion

        public int File_UpdateTenFile(int ID, int FileID, int LoaiFile)
        {
            int val = 0;
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter("@ID", SqlDbType.Int),
                new SqlParameter("@FileID", SqlDbType.Int),
                new SqlParameter("@LoaiFile", SqlDbType.Int),
            };
            parms[0].Value = ID;
            parms[1].Value = FileID;
            parms[2].Value = LoaiFile;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, "DM_File_UpdateFileID", parms);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -1;
                        throw ex;
                    }
                }
                conn.Close();
            }
            return val;
        }
    }
}
