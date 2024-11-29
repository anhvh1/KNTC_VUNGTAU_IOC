using Com.Gosol.KNTC.DAL.BaoCao;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Create by Nam - 24/10/2022


namespace Com.Gosol.KNTC.DAL.DanhMuc
{

    public class DanhMucBuocXacMinhDAL
    {
        #region Variable
        private const string pBuocXacMinhID = "@BuocXacMinhID";
        private const string pTenBuoc = "@TenBuoc";
        private const string pLoaiDon = "@LoaiDon";
        private const string pFileDinhKem = "@IsDinhKemFile";
        private const string pGhiChu = "@GhiChu";
        private const string pFileUrl = "@TenFile";
        private const string pOrderBy = "@OrderBy";
        private const string pSuDung = "@SuDung";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";

        private const string pFileDanhMucBuocXacMinhID = "@FileDanhMucBuocXacMinhID";
        private const string pTenFile = "@TenFile";
        private const string pTomTat = "@TomTat";
        private const string pNgayUp = "@NgayUp";
        private const string pNguoiUp = "@NguoiUp";
        private const string pFileURL = "@FileURL";
        private const string pDMBuocXacMinhID = "@DMBuocXacMinhID";
        private const string pFileID = "@FileID";


        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        private const string sDanhSach = "v2_DM_BuocXacMinh_DanhSach";
        private const string sThemMoi = "v2_DM_BuocXacMinh_ThemMoi";
        private const string sChiTiet = "v2_DM_BuocXacMinh_ChiTiet";
        private const string sCapNhat = "v2_DM_BuocXacMinh_CapNhat";
        private const string sXoa = "v2_DM_BuocXacMinh_Xoa";
        private const string sKiemTraTonTai = "v2_DM_BuocXacMinh_KiemTraTonTai";

        private const string sDanhSachFile = "v2_FileDanhMucBuocXacMinh_DanhSach";
        private const string sDanhSachFileID = "v2_FileDanhMucBuocXacMinh_DanhSachID";
        private const string sChiTietFile = "v2_FileDanhMucBuocXacMinh_ChiTiet";
        private const string sThemMoiFile = "v2_FileDanhMucBuocXacMinh_ThemMoi";
        private const string sCapNhatFile = "v2_FileDanhMucBuocXacMinh_CapNhat";
        private const string sXoaFile = "v2_FileDanhMucBuocXacMinh_Xoa";

        #endregion


        #region Function

        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên bước xác minh
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSach(ThamSoLocDanhMuc p, int? LoaiDon)
        {
            var Result = new BaseResultModel();
            List<DanhMucBuocXacMinhMOD> Data = new List<DanhMucBuocXacMinhMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pLoaiDon,SqlDbType.Int),
                new SqlParameter(pTrangThai,SqlDbType.Bit),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),
                new SqlParameter(pTotalRow,SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = LoaiDon ?? Convert.DBNull;
            parameters[2].Value = p.Status ?? Convert.DBNull;
            parameters[3].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[4].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSach, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucBuocXacMinhMOD item = new DanhMucBuocXacMinhMOD();
                        item.BuocXacMinhID = Utils.ConvertToInt32(dr["BuocXacMinhID"], 0);
                        item.LoaiDon = Utils.ConvertToInt32(dr["LoaiDon"], 0);
                        item.TenBuoc = Utils.ConvertToString(dr["TenBuoc"], string.Empty);
                        item.IsDinhKemFile = Utils.ConvertToBoolean(dr["ISDinhKemFile"], false);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        item.OrderBy = Utils.ConvertToInt32(dr["OrderBy"], 0);
                        item.SuDung = Utils.ConvertToBoolean(dr["SuDung"], false);
                        Data.Add(item);
                    }
                    dr.Close();
                }

                List<DanhMucBuocXacMinhMOD> NewData = new List<DanhMucBuocXacMinhMOD>();

                string TenFile = "";
                for(int i = 0; i < Data.Count-1; i++)
                {
                    int isOk = 1;
                    TenFile = Data[i].TenFile;
                    for (int j = i+1; j < Data.Count; j++)
                    {
                        if(Data[j].BuocXacMinhID == Data[i].BuocXacMinhID)
                        {
                            TenFile += ", " + Data[j].TenFile;
                        }
                    }

                    foreach(DanhMucBuocXacMinhMOD item in NewData)
                    {
                        if(item.BuocXacMinhID == Data[i].BuocXacMinhID)
                        {
                            isOk = 0;
                            break ;
                        }
                    }
                    
                    if(isOk == 1)
                    {
                        NewData.Add(new DanhMucBuocXacMinhMOD()
                        {
                            BuocXacMinhID = Data[i].BuocXacMinhID,
                            TenBuoc = Data[i].TenBuoc,
                            LoaiDon = Data[i].LoaiDon,
                            IsDinhKemFile = Data[i].IsDinhKemFile,
                            GhiChu = Data[i].GhiChu,
                            OrderBy = Data[i].OrderBy,
                            SuDung = Data[i].SuDung,
                            TenFile = TenFile
                        });
                    }
                    
                }

                var TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
                Result.Status = 1;
                Result.Data = NewData;
                Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }


        /// <summary>
        /// lấy chi tiết thông tin của 1 danh mục bước xác minh theo id
        /// </summary>
        /// <param name="BuocXacMinhID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int? BuocXacMinhID)
        {
            var Result = new BaseResultModel();
            List<DanhMucBuocXacMinhMOD> Data = new List<DanhMucBuocXacMinhMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pBuocXacMinhID,SqlDbType.Int),
            };
            parameters[0].Value = BuocXacMinhID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        DanhMucBuocXacMinhMOD item = new DanhMucBuocXacMinhMOD();
                        item.BuocXacMinhID = Utils.ConvertToInt32(dr["BuocXacMinhID"], 0);
                        item.TenBuoc = Utils.ConvertToString(dr["TenBuoc"], string.Empty);
                        item.LoaiDon = Utils.ConvertToInt32(dr["LoaiDon"], 0);
                        item.IsDinhKemFile = Utils.ConvertToBoolean(dr["ISDinhKemFile"], false);
                        item.GhiChu = Utils.ConvertToString(dr["GhiChu"], string.Empty);
                        item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        item.OrderBy = Utils.ConvertToInt32(dr["OrderBy"], 0);
                        item.SuDung = Utils.ConvertToBoolean(dr["SuDung"], false);
                        Data.Add(item);
                    }
                    dr.Close();
                }

                List<DanhMucBuocXacMinhMOD> NewData = new List<DanhMucBuocXacMinhMOD>();

                string TenFile = "";
                for (int i = 0; i < Data.Count; i++)
                {
                    int isOk = 1;
                    TenFile = Data[i].TenFile;
                    for (int j = i + 1; j < Data.Count; j++)
                    {
                        if (Data[j].BuocXacMinhID == Data[i].BuocXacMinhID)
                        {
                            TenFile += ", " + Data[j].TenFile;
                        }
                    }

                    foreach (DanhMucBuocXacMinhMOD item in NewData)
                    {
                        if (item.BuocXacMinhID == Data[i].BuocXacMinhID)
                        {
                            isOk = 0;
                            break;
                        }
                    }

                    if (isOk == 1)
                    {
                        NewData.Add(new DanhMucBuocXacMinhMOD()
                        {
                            BuocXacMinhID = Data[i].BuocXacMinhID,
                            TenBuoc = Data[i].TenBuoc,
                            LoaiDon = Data[i].LoaiDon,
                            IsDinhKemFile = Data[i].IsDinhKemFile,
                            GhiChu = Data[i].GhiChu,
                            OrderBy = Data[i].OrderBy,
                            SuDung = Data[i].SuDung,
                            TenFile = TenFile
                        });
                    }

                }

                Result.Status = 1;
                Result.Data = NewData;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
                throw;
            }
            return Result;
        }

        /// <summary>
        /// kiểm tra tồn tại của danh mục bước xác minh thông qua mã hoặc tên (trùng mã hoặc trùng tên)
        /// , nếu Type=1 và BuocXacMinhID = null thì kiểm tra trùng mã cho trường hợp thêm mới
        /// , nếu Type=2 và BuocXacMinhID = null thì kiểm tra trùng tên cho trường hợp thêm mới
        /// , nếu Type=1 và BuocXacMinhID != null thì kiểm tra trùng mã cho trường hợp Cập nhật
        /// , nếu Type=2 và BuocXacMinhID != null thì kiểm tra trùng tên cho trường hợp Cập nhật
        /// , nếu Type=3 và BuocXacMinhID != null thì kiểm tra tồn tại theo BuocXacMinhID cho trường hợp Xóa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool KiemTraTonTai(string keyword, int type, int? BuocXacMinhID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(pBuocXacMinhID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = BuocXacMinhID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraTonTai, parameters))
                {
                    while (dr.Read())
                    {
                        var TonTai = Utils.ConvertToInt32(dr["TonTai"], 0);
                        Result = TonTai > 0 ? true : false;
                        break;
                    }
                    dr.Close();
                }
            }
            catch (Exception)
            {
            }
            return Result;
        }

        /// <summary>
        /// Thêm mới bước xác minh
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoi(DanhMucBuocXacMinhThemMoiMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pTenBuoc, SqlDbType.NVarChar),
                    new SqlParameter(pLoaiDon, SqlDbType.TinyInt),
                    new SqlParameter(pFileDinhKem , SqlDbType.Bit),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    //new SqlParameter(pFileUrl , SqlDbType.NVarChar),
                    new SqlParameter(pOrderBy , SqlDbType.Int),
                    new SqlParameter(pSuDung , SqlDbType.Int),
                };
                parameters[0].Value = item.TenBuoc.Trim();
                parameters[1].Value = item.LoaiDon ?? Convert.DBNull;
                parameters[2].Value = item.IsDinhKemFile;
                parameters[3].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                //parameters[4].Value = item.TenFile;
                parameters[4].Value = item.OrderBy ?? Convert.DBNull;
                parameters[5].Value = item.SuDung;


                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var buocXacMinhID = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoi, parameters).ToString(), 0);
                            //if (buocXacMinhID > 0)
                            //{
                            //    foreach (var file in item.FileMau)
                            //    {
                            //        SqlParameter[] prFile = new SqlParameter[]
                            //        {
                            //            new SqlParameter(pTenFile, SqlDbType.NVarChar),
                            //            new SqlParameter(pNgayUp, SqlDbType.DateTime),
                            //            new SqlParameter(pFileURL , SqlDbType.NVarChar),
                            //            new SqlParameter(pDMBuocXacMinhID, SqlDbType.NVarChar),
                            //        };
                            //        prFile[0].Value = file.TenFile.Trim();
                            //        prFile[1].Value = file.NgayCapNhat;
                            //        prFile[2].Value = file.FileUrl;
                            //        prFile[3].Value = buocXacMinhID;
                            //        Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoiFile, prFile).ToString(), 0);
                            //    }
                            //}
                            trans.Commit();
                            Result.Status = buocXacMinhID;
                            Result.Message = "Thêm mới bước xác minh thành công!";
                            Result.Data = buocXacMinhID;
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
                Result.Message = Constant.ERR_INSERT;
            }
            return Result;
        }


        /// <summary>
        /// cập nhật thông tin danh mụcbước xác minh
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhat(DanhMucBuocXacMinhCapNhatMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pBuocXacMinhID, SqlDbType.Int),
                    new SqlParameter(pTenBuoc, SqlDbType.NVarChar),
                    new SqlParameter(pLoaiDon, SqlDbType.TinyInt),
                    new SqlParameter(pFileDinhKem , SqlDbType.Bit),
                    new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                    //new SqlParameter(pFileUrl , SqlDbType.NVarChar),
                    new SqlParameter(pOrderBy , SqlDbType.Int),
                    new SqlParameter(pSuDung , SqlDbType.Int)
                };
                parameters[0].Value = item.BuocXacMinhID;
                parameters[1].Value = item.TenBuoc.Trim();
                parameters[2].Value = item.LoaiDon ?? Convert.DBNull;
                parameters[3].Value = item.IsDinhKemFile;
                parameters[4].Value = item.GhiChu.Trim() ?? Convert.DBNull;
                //parameters[5].Value = item.TenFile;
                parameters[5].Value = item.OrderBy ?? Convert.DBNull;
                parameters[6].Value = item.SuDung;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            var buocXacMinhID = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhat, parameters);
                            //if (buocXacMinhID > 0)
                            //{
                            //    foreach (var file in item.FileMau)
                            //    {
                            //        SqlParameter[] prFile = new SqlParameter[]
                            //        {
                            //            new SqlParameter(pFileDanhMucBuocXacMinhID, SqlDbType.Int),
                            //            new SqlParameter(pTenFile, SqlDbType.NVarChar),
                            //            new SqlParameter(pNgayUp, SqlDbType.DateTime),
                            //            new SqlParameter(pFileURL , SqlDbType.NVarChar),
                            //            //new SqlParameter(pDMBuocXacMinhID, SqlDbType.NVarChar),
                            //        };
                            //        prFile[0].Value = file.FileDanhMucBuocXacMinhID;
                            //        prFile[1].Value = file.TenFile.Trim();
                            //        prFile[2].Value = Utils.ConvertToDateTime(file.NgayUp, DateTime.MinValue);
                            //        prFile[3].Value = file.FileURL;
                            //        //prFile[3].Value = buocXacMinhID;
                            //        SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhatFile, prFile);
                            //    }
                            //}
                            Result.Status = buocXacMinhID;
                            trans.Commit();
                            Result.Message = "Cập nhật bước xác minh thành công!";
                            Result.Data = Result.Status;
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = Constant.ERR_UPDATE;
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

        /// <summary>
        /// xóa danh mục bước xác minh
        /// </summary>
        /// <param name="BuocXacMinhID"></param>
        /// <returns></returns>
        public BaseResultModel Xoa(int BuocXacMinhID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pBuocXacMinhID, SqlDbType.Int)
            };
            parameters[0].Value = BuocXacMinhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, sXoa, parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa danh mục bước xác minh!";
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
            Result.Message = "Xóa danh mục bước xác minh thành công!";
            return Result;
        }

        //===============================================================//
        /// <summary>
        /// lấy danh sách phân trang
        /// , cho phép lọc theo trạng thái và tìm kiếm theo tên file
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public BaseResultModel DanhSachFile(int? DMBuocXacMinhID)
        {
            var Result = new BaseResultModel();
            List<FileMauMOD> Data = new List<FileMauMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                //new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                //new SqlParameter(pTrangThai,SqlDbType.Bit),
                //new SqlParameter(pLimit,SqlDbType.Int),
                //new SqlParameter(pOffset,SqlDbType.Int),
                //new SqlParameter(pTotalRow,SqlDbType.Int),
                new SqlParameter(pDMBuocXacMinhID,SqlDbType.Int),

            };
            /*parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.Status ?? Convert.DBNull;
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[3].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[4].Direction = ParameterDirection.Output;
            parameters[4].Size = 8;*/
            parameters[0].Value = DMBuocXacMinhID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachFile, parameters))
                {
                    while (dr.Read())
                    {
                        FileMauMOD item = new FileMauMOD();
                        item.FileDanhMucBuocXacMinhID = Utils.ConvertToInt32(dr["FileDanhMucBuocXacMinhID"], 0);
                        item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        //item.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        item.NgayUp = Utils.ConvertToString(dr["NgayUp"], string.Empty);
                        //item.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        item.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        //item.DMBuocXacMinhID = Utils.ConvertToInt32(dr["DMBuocXacMinhID"], 0);
                        //item.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                //var TotalRow = Utils.ConvertToInt32(parameters[4].Value, 0);
                Result.Status = 1;
                Result.Data = Data;
                //Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        public BaseResultModel DanhSachFileID(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<FileMauMOD> Data = new List<FileMauMOD>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pTrangThai,SqlDbType.Bit),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),
                new SqlParameter(pTotalRow,SqlDbType.Int),
                //new SqlParameter(pDMBuocXacMinhID,SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.Status ?? Convert.DBNull;
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[3].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[4].Direction = ParameterDirection.Output;
            parameters[4].Size = 8;
            //parameters[0].Value = DMBuocXacMinhID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachFileID, parameters))
                {
                    while (dr.Read())
                    {
                        FileMauMOD item = new FileMauMOD();
                        item.FileDanhMucBuocXacMinhID = Utils.ConvertToInt32(dr["FileDanhMucBuocXacMinhID"], 0);
                        item.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        //item.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        item.NgayUp = Utils.ConvertToString(dr["NgayUp"], string.Empty);
                        //item.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        item.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        //item.DMBuocXacMinhID = Utils.ConvertToInt32(dr["DMBuocXacMinhID"], 0);
                        //item.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        Data.Add(item);
                    }
                    dr.Close();
                }
                var TotalRow = Utils.ConvertToInt32(parameters[4].Value, 0);
                Result.Status = 1;
                Result.Data = Data;
                Result.TotalRow = TotalRow;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        /// <summary>
        /// lấy chi tiết thông tin của 1 file mẫu theo id
        /// </summary>
        /// <param name="FileDanhMucBuocXacMinhID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTietFile(int? FileDanhMucBuocXacMinhID, string rootPath)
        {
            var Result = new BaseResultModel();
            CapNhatFileMOD data = new CapNhatFileMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pFileDanhMucBuocXacMinhID,SqlDbType.Int,25),
            };
            parameters[0].Value = FileDanhMucBuocXacMinhID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTietFile, parameters))
                {
                    while (dr.Read())
                    {
                        data.FileDanhMucBuocXacMinhID = Utils.ConvertToInt32(dr["FileDanhMucBuocXacMinhID"], 0);
                        data.TenFile = Utils.ConvertToString(dr["TenFile"], string.Empty);
                        //data.TomTat = Utils.ConvertToString(dr["TomTat"], string.Empty);
                        data.NgayUp = Utils.ConvertToString(dr["NgayUp"], string.Empty);
                        //data.NguoiUp = Utils.ConvertToInt32(dr["NguoiUp"], 0);
                        data.FileURL = Utils.ConvertToString(dr["FileURL"], string.Empty);
                        //data.DMBuocXacMinhID = Utils.ConvertToInt32(dr["DMBuocXacMinhID"], 0);
                        //data.FileID = Utils.ConvertToInt32(dr["FileID"], 0);
                        break;
                    }
                    dr.Close();
                }
                Result.Status = 1;
                Result.Data = data;
            }
            catch (Exception ex)
            {
                throw;
                Result.Status = -1;
                //Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        /// <summary>
        /// Thêm mới file mẫu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel ThemMoiFile(ThemMoiFileMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pTenFile, SqlDbType.NVarChar),
                    //new SqlParameter(pTomTat, SqlDbType.NVarChar),
                    new SqlParameter(pNgayUp , SqlDbType.DateTime),
                    //new SqlParameter(pNguoiUp, SqlDbType.Int),
                    new SqlParameter(pFileURL , SqlDbType.NVarChar),
                    new SqlParameter(pDMBuocXacMinhID , SqlDbType.Int),
                   // new SqlParameter(pFileID , SqlDbType.Int)
                };
                parameters[0].Value = item.TenFile.Trim();
                //parameters[1].Value = item.TomTat;
                parameters[1].Value = item.NgayUp;
                //parameters[3].Value = item.NguoiUp;
                parameters[2].Value = item.FileURL;
                //parameters[3].Value = item.DMBuocXacMinhID;
                //parameters[6].Value = item.FileID;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sThemMoiFile, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới file thành công!";
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
                Result.Message = Constant.ERR_INSERT;
            }
            return Result;
        }

        /// <summary>
        /// cập nhật thông tin file mẫu
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public BaseResultModel CapNhatFile(CapNhatFileMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter(pFileDanhMucBuocXacMinhID, SqlDbType.Int),
                    new SqlParameter(pTenFile, SqlDbType.NVarChar),
                    new SqlParameter(pNgayUp, SqlDbType.DateTime),
                    new SqlParameter(pFileURL, SqlDbType.NVarChar)
                };
                parameters[0].Value = item.FileDanhMucBuocXacMinhID;
                parameters[1].Value = item.TenFile.Trim();
                parameters[2].Value = Utils.ConvertToDateTime(item.NgayUp, DateTime.MinValue);
                parameters[3].Value = item.FileURL;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sCapNhatFile, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật file thành công!";
                            Result.Data = Result.Status;
                        }
                        catch (Exception ex)
                        {
                            Result.Status = -1;
                            Result.Message = Constant.ERR_UPDATE;
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

        /// <summary>
        /// xóa file mẫu
        /// </summary>
        /// <param name="FileDanhMucBuocXacMinhID"></param>
        /// <returns></returns>
        public BaseResultModel XoaFile(int FileDanhMucBuocXacMinhID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter(pFileDanhMucBuocXacMinhID, SqlDbType.Int)
            };
            parameters[0].Value = FileDanhMucBuocXacMinhID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, sXoaFile, parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa file!";
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
            Result.Message = "Xóa file thành công!";
            return Result;
        }

        #endregion

    }
}
