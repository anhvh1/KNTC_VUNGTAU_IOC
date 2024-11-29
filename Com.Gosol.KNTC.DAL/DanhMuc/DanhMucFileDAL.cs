using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//Create by TienKM 17/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucFileDAL
    {
        #region Variable

        private string pNhomFileID = "NhomFileID";
        private string pTenNhomFile = "TenNhomFile";
        private string pThuTuHienThi = "ThuTuHienThi";
        private string pTrangThaiSuDung = "TrangThaiSuDung";
        private const string pFileID = "FileID";
        private const string pTenFile = "TenFile";
        private string pTenChucNang = "TenChucNang";
        private string pChucNangID = "ChucNangID";
        private const string pLimit = "Limit";
        private const string pOffset = "Offset";
        private const string pTotalRow = "TotalRow";


        private string sDanhSachNhomFile = "v2_DanhMuc_NhomFile_GetAll";
        private string sThemNhomFile = "v2_DanhMuc_NhomFile_ThemMoi";
        private string sXoaNhomFile = "v2_DanhMuc_NhomFile_Xoa";
        private string sNhomFileCheckName = "v2_DanhMuc_NhomFile_KiemTenTraTonTai";
        private string sNhomFileUpdate = "v2_DanhMuc_NhomFile_Sua";
        private string sNhomFileChiTiet = "v2_DanhMuc_NhomFile_ChiTiet";
        private string sChucNangFile = "v2_DanhMuc_HT_ChucNang_GetAll";
        private string sTenFile = "v2_DanhMuc_File_GetAll";
        private string sFileChiTiet = "v2_DanhMuc_File_ChiTiet";
        private string sFileKiemTraTonTai = "v2_DanhMuc_File_KiemTraTonTai";
        private string sChucNangApDungCheck = "v2_DanhMuc_ChucNangApDung_KiemTenTraTonTai";
        private string sChucNangApDungInsert = "v2_DanhMuc_ChucNangApDung_Insert";
        private string sFileInsert = "v2_DanhMuc_File_Insert";
        private string sKiemTraIDNhomFile = "v2_DanhMuc_NhomFile_KiemTraID";
        private string sKiemTraIDChucNang = "v2_DanhMuc_ChucNang_KiemTraID";
        private string sXoaChucNangApDung = "v2_DanhMuc_File_ChucNangApDung_Xoa";
        private string sXoaFile = "v2_DanhMuc_File_Xoa";
        private string sUpdateFile = "v2_DanhMuc_File_Update";

        #endregion

        #region Function

        public BaseResultModel DanhSachNhomFile(ThamSoLocDanhMuc thamSo)
        {
            var Result = new BaseResultModel();
            List<DanhMucNhomFileModel> Data = new();
            SqlParameter[] parameters =
            {
                new SqlParameter(pTenNhomFile, SqlDbType.NVarChar),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };

            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;

            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = offset;
            parameters[2].Value = thamSo.PageSize;
            parameters[3].Direction = ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sDanhSachNhomFile, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucNhomFileModel
                        {
                            NhomFileID = Utils.ConvertToInt32(dr[pNhomFileID], 0),
                            TenNhomFile = Utils.ConvertToString(dr[pTenNhomFile], string.Empty),
                            ThuTuHienThi = Utils.ConvertToInt32(dr[pThuTuHienThi], 0),
                            TrangThaiSuDung = Utils.ConvertToBoolean(dr[pTrangThaiSuDung], false)
                        });
                    }

                    dr.Close();
                }

                var totalCount = Utils.ConvertToInt32(parameters[3].Value, 0);
                Result.TotalRow = totalCount;
                Result.Status = 1;
                Result.Message = totalCount == 0 || Data.Count == 0 ? Constant.NO_DATA :  "Thành công";
                Result.Data = Data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public BaseResultModel DanhSachFile(ThamSoFileModel thamSo)
        {
            var Result = new BaseResultModel();

            List<DanhMucFileModel> Data = new();
            SqlParameter[] parameters =
            {
                new SqlParameter(pTenFile, SqlDbType.NVarChar),
                new SqlParameter(pChucNangID, SqlDbType.NVarChar),
                new SqlParameter(pNhomFileID, SqlDbType.Int),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };

            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;

            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSo.ChucNangID ?? Convert.DBNull;
            parameters[2].Value = thamSo.NhomFileID ?? Convert.DBNull;
            parameters[3].Value = offset;
            parameters[4].Value = thamSo.PageSize;
            parameters[5].Direction = ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings,
                           CommandType.StoredProcedure, this.sTenFile, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucFileModel
                        {
                            FileID = Utils.ConvertToInt32(dr[pFileID], 0),
                            TenFile = Utils.ConvertToString(dr[pTenFile], string.Empty),
                            ThuTuHienThi = Utils.ConvertToInt32(dr[pThuTuHienThi], 0),
                            TenNhomFile = Utils.ConvertToString(dr[pTenNhomFile], string.Empty),
                            TrangThaiSuDung = Utils.ConvertToBoolean(dr[pTrangThaiSuDung], false),
                            NhomFileID = Utils.ConvertToInt32(dr[pNhomFileID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], String.Empty)
                        });
                    }

                    dr.Close();
                }

                var totalCount = Utils.ConvertToInt32(parameters[5].Value, 0);
                Result.TotalRow = totalCount;
                Result.Status = 1;
                Result.Message = totalCount == 0 || Data.Count == 0 ? Constant.NO_DATA :  "Thành công";
                Result.Data = Data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public List<DanhMucFileModel> GetDanhSachByNhomFileID(ThamSoFileModel thamSo)
        {
            List<DanhMucFileModel> Data = new List<DanhMucFileModel>();
            SqlParameter[] parameters =
            {
                new SqlParameter(pTenFile, SqlDbType.NVarChar),
                new SqlParameter(pChucNangID, SqlDbType.NVarChar),
                new SqlParameter(pNhomFileID, SqlDbType.Int),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };

            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;

            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSo.ChucNangID ?? Convert.DBNull;
            parameters[2].Value = thamSo.NhomFileID ?? Convert.DBNull;
            parameters[3].Value = offset;
            parameters[4].Value = thamSo.PageSize;
            parameters[5].Direction = ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings,
                           CommandType.StoredProcedure, this.sTenFile, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucFileModel
                        {
                            FileID = Utils.ConvertToInt32(dr[pFileID], 0),
                            TenFile = Utils.ConvertToString(dr[pTenFile], string.Empty),
                            ThuTuHienThi = Utils.ConvertToInt32(dr[pThuTuHienThi], 0),
                            TenNhomFile = Utils.ConvertToString(dr[pTenNhomFile], string.Empty),
                            TrangThaiSuDung = Utils.ConvertToBoolean(dr[pTrangThaiSuDung], false),
                            NhomFileID = Utils.ConvertToInt32(dr[pNhomFileID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], String.Empty)
                        });
                    }

                    dr.Close();
                }

                var totalCount = Utils.ConvertToInt32(parameters[5].Value, 0);
              
            }
            catch (Exception ex)
            {
                throw;
            }

            return Data;
        }

        public BaseResultModel DanhSachChucNang()
        {
            var Result = new BaseResultModel();

            List<ChucNangFileModel> Data = new();
            SqlParameter[] parameters =
            {
            };


            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings,
                           CommandType.StoredProcedure, this.sChucNangFile, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new ChucNangFileModel
                        {
                            ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], String.Empty)
                        });
                    }

                    dr.Close();
                }

                Result.TotalRow = Data.Count;
                Result.Status = 1;
                Result.Data = Data;
                Result.Message = "Thành công";
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public BaseResultModel ChiTietFile(int fileID)
        {
            BaseResultModel Result = new BaseResultModel();

            FileChiTietModel File = new FileChiTietModel();

            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pFileID, SqlDbType.Int),
            };
            parameters[0].Value = fileID;

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings,
                    CommandType.StoredProcedure, this.sFileChiTiet, parameters);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        File = new FileChiTietModel
                        {
                            FileID = Utils.ConvertToInt32(dr[pFileID], 0),
                            TenFile = Utils.ConvertToString(dr[pTenFile], string.Empty),
                            ThuTuHienThi = Utils.ConvertToInt32(dr[pThuTuHienThi], 0),
                            TenNhomFile = Utils.ConvertToString(dr[pTenNhomFile], string.Empty),
                            TrangThaiSuDung = Utils.ConvertToBoolean(dr[pTrangThaiSuDung], false),
                            NhomFileID = Utils.ConvertToInt32(dr[pNhomFileID], 0),
                        };
                        break;
                    }

                    if (dr.NextResult())
                        while (dr.Read())
                        {
                            File.ChucNangApDung.Add(new DMChucNangModel
                            {
                                ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                                TenChucNang = Utils.ConvertToString(dr[pTenChucNang], String.Empty)
                            });
                        }

                    Result.Message = "Thành công";
                    Result.Status = 1;
                    Result.Data = File;
                    Result.TotalRow = 1;
                }
                else
                {
                    Result.Message = "Không có dữ liệu";
                    Result.Status = 0;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Result;
        }

        public bool KiemTraTenTonTaiNhomFile(string tenVanBan, int? LoaiVanBanID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pNhomFileID, SqlDbType.Int),
                new SqlParameter(pTenNhomFile, SqlDbType.NVarChar),
            };

            parameters[0].Value = LoaiVanBanID ?? Convert.DBNull;
            parameters[1].Value = tenVanBan;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure,
                    sNhomFileCheckName, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool KiemTraTenTonTaiFile(string tenFile, int? FileID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pTenFile, SqlDbType.NVarChar),
                new SqlParameter(pFileID, SqlDbType.Int),
            };

            parameters[0].Value = tenFile ?? Convert.DBNull;
            parameters[1].Value = FileID ?? Convert.DBNull;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure,
                    sFileKiemTraTonTai, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public bool KiemTraIDNhomFile(int nhomFileID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pNhomFileID, SqlDbType.Int),
            };

            parameters[0].Value = nhomFileID;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure,
                    sKiemTraIDNhomFile, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public bool KiemTraIDChucNang(int chucNangID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pChucNangID, SqlDbType.Int),
            };

            parameters[0].Value = chucNangID;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure,
                    sKiemTraIDChucNang, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public bool KiemTraTenTonTaiChucNang(int FileID, int ChucNangID)
        {
            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pFileID, SqlDbType.Int),
                new SqlParameter(pChucNangID, SqlDbType.Int),
            };

            parameters[0].Value = FileID;
            parameters[1].Value = ChucNangID;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure,
                    sChucNangApDungCheck, parameters);
                if (Convert.ToInt32(rowCount) > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public BaseResultModel ChiTietNhomFile(int nhomFileID)
        {
            BaseResultModel Result = new BaseResultModel();

            DanhMucNhomFileModel nhomFile = null;

            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pNhomFileID, SqlDbType.Int),
            };
            parameters[0].Value = nhomFileID;

            try
            {
                using SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sNhomFileChiTiet, parameters);
                while (dr.Read())
                {
                    nhomFile = new DanhMucNhomFileModel
                    {
                        NhomFileID = Utils.ConvertToInt32(dr[pNhomFileID], 0),
                        TenNhomFile = Utils.ConvertToString(dr[pTenNhomFile], string.Empty),
                        ThuTuHienThi = Utils.ConvertToInt32(dr[pThuTuHienThi], 0),
                        TrangThaiSuDung = Utils.ConvertToBoolean(dr[pTrangThaiSuDung], false)
                    };
                    break;
                }

                if (nhomFile == null)
                {
                    Result.Message = "Không có dữ liệu";
                    Result.Status = 0;
                }
                else
                {
                    Result.Message = "Thành công";
                    Result.Status = 1;
                    Result.Data = nhomFile;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Result;
        }

        public BaseResultModel ThemMoiNhomFile(ThemNhomFileModel nhomFile)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTaiNhomFile(nhomFile.TenNhomFile.Trim(), null))
            {
                Result.Status = -1;
                Result.Message = "Tên nhóm file đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pTenNhomFile, SqlDbType.NVarChar),
                new SqlParameter(pThuTuHienThi, SqlDbType.Int),
                new SqlParameter(pTrangThaiSuDung, SqlDbType.Bit),
            };

            parameters[0].Value = nhomFile.TenNhomFile;
            parameters[1].Value = nhomFile.ThuTuHienThi;
            parameters[2].Value = nhomFile.TrangThaiSuDung;

            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sThemNhomFile,parameters);
                        Result.Data = effected;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            Result.Status = 1;
            Result.Message = Constant.MSG_SUKNTCESS;
            return Result;
        }

        public BaseResultModel ThemMoiFile(ThemFileModel file)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTaiFile(file.TenFile, null))
            {
                Result.Message = $"Ten File : '{file.TenFile}' đã tồn tại";
                Result.Status = -1;
                return Result;
            }

            if (!KiemTraIDNhomFile(file.NhomFileID))
            {
                Result.Message = $"IDNhomFILE : {file.NhomFileID} không tồn tại";
                Result.Status = -1;
                return Result;
            }

            SqlParameter[] parameters = new[]
            {
                new SqlParameter(pTenFile, file.TenFile),
                new SqlParameter(pThuTuHienThi, file.ThuTuHienThi),
                new SqlParameter(pTrangThaiSuDung, file.TrangThaiSuDung),
                new SqlParameter(pNhomFileID, file.NhomFileID),
            };


            using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
            connection.Open();
            using var trans = connection.BeginTransaction();
            try
            {
                var id = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sFileInsert, parameters);

                foreach (var chucnangID in file.ChucNangApDungID)
                {
                    if (!KiemTraIDChucNang(chucnangID))
                    {
                        Result.Message = $"ID chức năng: {chucnangID} không tồn tại";
                        Result.Status = -1;
                        trans.Rollback();
                        return Result;
                    }

                    SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sChucNangApDungInsert, new[]
                    {
                        new SqlParameter(pFileID, id),
                        new SqlParameter(pChucNangID, chucnangID),
                    });
                }

                Result.Status = 1;
                Result.Message = Constant.MSG_SUKNTCESS;
                Result.Data = id;
                trans.Commit();
            }
            catch (Exception e)
            {
                Result.Status = -1;
                Result.Message = e.Message;
                trans.Rollback();
                throw;
            }

            return Result;
        }

        public BaseResultModel UpdateNhomFile(DanhMucNhomFileModel nhomFile)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTaiNhomFile(nhomFile.TenNhomFile.Trim(), nhomFile.NhomFileID))
            {
                Result.Status = -1;
                Result.Message = "Tên nhóm file đã tồn tại";
                return Result;
            }


            SqlParameter[] parameters =
            {
                new SqlParameter(pNhomFileID, SqlDbType.BigInt),
                new SqlParameter(pTenNhomFile, SqlDbType.NVarChar),
                new SqlParameter(pThuTuHienThi, SqlDbType.Int),
                new SqlParameter(pTrangThaiSuDung, SqlDbType.Bit),
            };

            parameters[0].Value = nhomFile.NhomFileID;
            parameters[1].Value = nhomFile.TenNhomFile;
            parameters[2].Value = nhomFile.ThuTuHienThi;
            parameters[3].Value = nhomFile.TrangThaiSuDung;

            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure,
                            sNhomFileUpdate,
                            parameters);
                        Result.Data = effected;
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            Result.Status = 1;
            Result.Message = Constant.MSG_SUKNTCESS;
            return Result;
        }

        public BaseResultModel UpdateFile(UpdateFileModel File)
        {
            var Result = new BaseResultModel();

            if (!KiemTraTenTonTaiFile(null, File.FileID))
            {
                Result.Status = -1;
                Result.Message = "ID file không tồn tại";
                return Result;
            }

            if (!KiemTraIDNhomFile(File.NhomFileID))
            {
                Result.Status = -1;
                Result.Message = $"ID nhom :{File.NhomFileID} không tồn tại";
                return Result;
            }

            if (KiemTraTenTonTaiFile(File.TenFile.Trim(), File.FileID))
            {
                Result.Status = -1;
                Result.Message = "Tên file đã tồn tại";
                return Result;
            }

            foreach (var chucnangID in File.ChucNangApDungID)
            {
                if (!KiemTraIDChucNang(chucnangID))
                {
                    Result.Message = $"ID chức năng: {chucnangID} không tồn tại";
                    Result.Status = -1;
                    return Result;
                }
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pFileID, SqlDbType.Int),
                new SqlParameter(pTenFile, SqlDbType.NVarChar),
                new SqlParameter(pThuTuHienThi, SqlDbType.Int),
                new SqlParameter(pTrangThaiSuDung, SqlDbType.Bit),
                new SqlParameter(pNhomFileID, SqlDbType.Int),
            };

            parameters[0].Value = File.FileID;
            parameters[1].Value = File.TenFile;
            parameters[2].Value = File.ThuTuHienThi;
            parameters[3].Value = File.TrangThaiSuDung;
            parameters[4].Value = File.NhomFileID;

            using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
            connection.Open();
            using (var trans = connection.BeginTransaction())
            {
                try
                {
                    SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sUpdateFile, parameters);
                    // Xóa chuc năng trước khi insert lại
                    SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaChucNangApDung, new[]
                    {
                        new SqlParameter(pFileID, File.FileID)
                    });
                    foreach (var chucnangID in File.ChucNangApDungID)
                    {
                        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sChucNangApDungInsert, new[]
                        {
                            new SqlParameter(pFileID, File.FileID),
                            new SqlParameter(pChucNangID, chucnangID),
                        });
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }

            Result.Status = 1;
            Result.Message = Constant.MSG_SUKNTCESS;
            Result.Data = File.FileID;
            return Result;
        }

        public BaseResultModel XoaNhomFile(int nhomFileID)
        {
            var Result = new BaseResultModel();

            ThamSoFileModel thamSoFileModel = new ThamSoFileModel();
            thamSoFileModel.NhomFileID = nhomFileID;
            var nhom = GetDanhSachByNhomFileID(thamSoFileModel);
            if(nhom.Count > 0)
            {
                Result.Message = "Nhóm file đang được sử dụng";
                Result.Status = 0;
                return Result;
            }

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pNhomFileID, SqlDbType.Int),
            };
            parameters[0].Value = nhomFileID;

            using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
            connection.Open();
            using var trans = connection.BeginTransaction();
            try
            {
                var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaNhomFile,
                    parameters);
                trans.Commit();

                if (effected == 0)
                {
                    Result.Message = "ID không tồn tại";
                    Result.Status = 0;
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            return Result;
        }

        public BaseResultModel XoaFile(int FileID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pFileID, SqlDbType.Int),
            };
            parameters[0].Value = FileID;

            using var connection = new SqlConnection(SQLHelper.appConnectionStrings);
            connection.Open();
            using var trans = connection.BeginTransaction();
            try
            {
                var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaFile,
                    parameters);
                trans.Commit();

                if (effected == 0)
                {
                    Result.Message = "ID không tồn tại";
                    Result.Status = 0;
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            return Result;
        }

        #endregion
    }
}