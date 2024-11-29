using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Constant = Com.Gosol.KNTC.Ultilities.Constant;


//Create by TienKM 17/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucBieuMauDAL
    {
        #region Variable

        #region Param

        private readonly string pBieuMauID = "BieuMauID";
        private readonly string pTenBieuMau = "TenBieuMau";
        private readonly string pMaPhieuIn = "MaPhieuIn";
        private readonly string pDuocSuDung = "DuocSuDung";
        private readonly string pCapID = "CapID";
        private readonly string pLichSuID = "LichSuID";
        private readonly string pCanBoID = "CanBoID";
        private readonly string pTenCanBo = "TenCanBo";
        private readonly string pThoiGianCapNhat = "ThoiGianCapNhat";
        private readonly string pTenCap = "TenCap";
        private readonly string pThuTu = "ThuTu";
        private readonly string pFileUrl = "FileUrl";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private readonly string pTotalRow = "TotalRow";

        #endregion

        #region StoreProcedure

        private readonly string sDanhSachBieuMau = "v2_DanhMuc_BieuMau_GetAll";
        private readonly string sDanhSachBieuMauv2 = "v2_DanhMuc_BieuMau_GetAll_v2";
        private readonly string sDanhSachCap = "v2_DanhMuc_BieuMau_Cap_GetAll";
        private readonly string sKiemTraCap = "v2_DanhMuc_BieuMau_Cap_KiemTraTonTai";
        private readonly string sBieuMauChiTiet = "v2_DanhMuc_BieuMau_ChiTiet";
        private readonly string sBieuMauInsert = "v2_DanhMuc_BieuMau_Insert";
        private readonly string sBieuMauUpdate = "v2_DanhMuc_BieuMau_Update";
        private readonly string sKiemTraBieuMau = "v2_DanhMuc_BieuMau_KiemTraTonTai";
        private readonly string sKiemTraBieuMau_v2 = "v2_DanhMuc_BieuMau_KiemTraTonTai_v2";
        private readonly string sInsertLichSu = "v2_DanhMuc_BieuMau_InsertLichSu";
        private readonly string sXoaBieuMau = "v2_DanhMuc_BieuMau_Xoa";
        private readonly string sChiTietLichSu = "v2_DanhMuc_BieuMau_LichSuChiTiet";

        #endregion

        #endregion

        #region Function

        public BaseResultModel DanhSach(DanhMucBieuMauThamSo thamSo, string root)
        {
            var Result = new BaseResultModel();

            List<DanhMucBieuMauModel> Data = new();
            SqlParameter[] parameters =
            {
                new SqlParameter(pTenBieuMau, SqlDbType.NVarChar),
                new SqlParameter(pCapID, SqlDbType.Int),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pDuocSuDung, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };

            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;

            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSo.CapID ?? Convert.DBNull;
            parameters[2].Value = offset;
            parameters[3].Value = thamSo.PageSize;
            parameters[4].Value = thamSo.Status == null ? Convert.DBNull : (thamSo.Status == true ? 1 : 0);
            parameters[5].Direction = ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachBieuMau, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucBieuMauModel
                        {
                            BieuMauID = Utils.ConvertToInt32(dr[pBieuMauID], 0),
                            TenBieuMau = Utils.ConvertToString(dr[pTenBieuMau], string.Empty),
                            MaPhieuIn = Utils.ConvertToString(dr[pMaPhieuIn], string.Empty),
                            DuocSuDung = Utils.ConvertToInt32(dr[pDuocSuDung], 0) == 1,
                            CapID = Utils.ConvertToInt32(dr[pCapID], 0),
                            FileUrl = Utils.ConvertToString(dr[pFileUrl], "") == "" ? null : root + Utils.ConvertToString(dr[pFileUrl], ""),
                            TenCap = TenCap(Utils.ConvertToInt32(dr[pCapID], 0)),
                            UrlView = Utils.ConvertToString(dr[pFileUrl], "")
                        });
                        //Utils.ConvertToString(dr[pFileUrl], "") == "" ? "" : root + "/XemBieuMau?f=" + Utils.ConvertToString(dr[pFileUrl], "")
                    }
                    ///api/v2/DanhMucBieuMau/d?f=20_2022-11-09-113213_Catalog.docx
                    /// api/v2/DanhMucBieuMau/XemBieuMau?f=20_2022-11-09-095135_Motaduanteamone.docx
                    dr.Close();
                }

                var totalCount = Utils.ConvertToInt32(parameters[5].Value, 0);

                Result.TotalRow = totalCount;
                Result.Status = 1;
                Result.Message = totalCount == 0 || Data.Count == 0 ? Constant.NO_DATA : "Thành công";

                Result.Data = Data;
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
            }

            return Result;
        }


        public BaseResultModel DanhSachCap()
        {
            var Result = new BaseResultModel();
            List<CapBieuMau> Data = new()
            {
                new CapBieuMau
                {
                    TenCap = "Biểu mẫu cấp tỉnh/huyện",
                    CapID = IDCapBieuMau.TinhHuyen
                },
                new CapBieuMau
                {
                    TenCap = "Biểu mẫu cấp xã",
                    CapID = IDCapBieuMau.Xa
                },
            };

            Result.TotalRow = Data.Count;
            Result.Status = 1;
            Result.Data = Data;
            Result.Message = "Thành công";
            return Result;
        }

        public bool KiemTraBieuMauTonTai(int? BieuMauID, string tenBieuMau, string maPhieuIn, int? CapID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pBieuMauID, BieuMauID ?? Convert.DBNull),
                    new SqlParameter(pTenBieuMau, tenBieuMau ?? Convert.DBNull),
                    new SqlParameter(pMaPhieuIn, maPhieuIn ?? Convert.DBNull),
                    new SqlParameter(pCapID, CapID ?? Convert.DBNull),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraBieuMau, parameters);
                if (Convert.ToInt32(count) > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return false;
        }
        public bool KiemTraID(int BieuMauID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pBieuMauID, BieuMauID),

                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_BieuMau_KiemTraID", parameters);
                if (Convert.ToInt32(count) > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return false;
        }

        public BaseResultModel SuaBieuMau(SuaBieuMauModel bieuMau, IFormFile? file)
        {
            BaseResultModel Result = new BaseResultModel();

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {


                //var checkBieuMau = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraBieuMau_v2, new[]
                //{
                //    new SqlParameter(pBieuMauID, bieuMau.BieuMauID),
                //    new SqlParameter(pTenBieuMau, bieuMau.TenBieuMau),
                //    new SqlParameter(pMaPhieuIn, bieuMau.MaPhieuIn),
                //});

                //if (Convert.ToInt32(checkBieuMau) > 0)
                //{
                //    Result.Status = -1;
                //    Result.Message = $"Tên biểu mẫu, mã biểu mẫu đã tồn tại";
                //    return Result;
                //}

                SqlParameter[] parameters =
                {
                    new SqlParameter(pBieuMauID, SqlDbType.Int),
                    new SqlParameter(pTenBieuMau, SqlDbType.NVarChar),
                    new SqlParameter(pMaPhieuIn, SqlDbType.VarChar),
                    new SqlParameter(pCapID, SqlDbType.Int),
                    new SqlParameter(pDuocSuDung, SqlDbType.Bit),
                    new SqlParameter(pFileUrl, SqlDbType.NVarChar),
                };

                parameters[0].Value = bieuMau.BieuMauID;
                parameters[1].Value = bieuMau.TenBieuMau ?? Convert.DBNull;
                parameters[2].Value = bieuMau.MaPhieuIn ?? Convert.DBNull;
                parameters[3].Value = bieuMau.CapID ?? Convert.DBNull;
                parameters[4].Value = bieuMau.DuocSuDung ?? Convert.DBNull;
                parameters[5].Value = bieuMau.FileUrl ?? Convert.DBNull;


                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sBieuMauUpdate, parameters);
                Result.Data = bieuMau.BieuMauID;
                //Cập nhật lịch sử thêm
                SqlParameter[] InsertParam =
                {
                    new SqlParameter(pBieuMauID, SqlDbType.Int),
                    new SqlParameter(pCanBoID, SqlDbType.Int),
                };

                InsertParam[0].Value = bieuMau.BieuMauID;
                InsertParam[1].Value = bieuMau.CanBoID;

                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sInsertLichSu, InsertParam);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            Result.Status = 1;
            Result.Message = Constant.MSG_SUKNTCESS;
            return Result;
        }

        public BaseResultModel XoaBieuMau(int BieuMauID)
        {
            BaseResultModel Result = new BaseResultModel();
            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pBieuMauID, SqlDbType.Int),
                };
                parameters[0].Value = BieuMauID;

                var effectedRows = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaBieuMau, parameters);

                Result.Status = 1;
                Result.Message = "Xóa thành công";

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            con.Dispose();

            return Result;
        }

        public BaseResultModel ThemBieuMau(ThemBieuMauModel bieuMau)
        {
            BaseResultModel Result = new BaseResultModel();

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pTenBieuMau, SqlDbType.NVarChar),
                    new SqlParameter(pMaPhieuIn, SqlDbType.VarChar),
                    new SqlParameter(pDuocSuDung, SqlDbType.Bit),
                    new SqlParameter(pCapID, SqlDbType.Int),
                    new SqlParameter(pFileUrl, SqlDbType.NVarChar),
                };
                parameters[0].Value = bieuMau.TenBieuMau;
                parameters[1].Value = bieuMau.MaPhieuIn;
                parameters[2].Value = bieuMau.DuocSuDung;
                parameters[3].Value = bieuMau.CapID;
                parameters[4].Value = bieuMau.FileUrl;

                var BieuMauID = SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, sBieuMauInsert, parameters);

                Result.Data = Convert.ToInt32(BieuMauID);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            Result.Status = 1;
            Result.Message = Constant.MSG_SUKNTCESS;
            con.Dispose();
            return Result;
        }


        public BaseResultModel ChiTiet(int BieuMauID, string serverPath)
        {
            var Result = new BaseResultModel();

            DanhMucBieuMauChiTietModel Data = null;
            SqlParameter[] parameters =
            {
                new SqlParameter(pBieuMauID, SqlDbType.Int)
            };
            parameters[0].Value = BieuMauID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sBieuMauChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        Data = new DanhMucBieuMauChiTietModel
                        {
                            BieuMauID = Utils.ConvertToInt32(dr[pBieuMauID], 0),
                            TenBieuMau = Utils.ConvertToString(dr[pTenBieuMau], string.Empty),
                            MaPhieuIn = Utils.ConvertToString(dr[pMaPhieuIn], string.Empty),
                            DuocSuDung = Utils.ConvertToInt32(dr[pDuocSuDung], 0) == 1,
                            CapID = Utils.ConvertToInt32(dr[pCapID], 0),
                            TenCap = TenCap(Utils.ConvertToInt32(dr[pCapID], 0)),
                            FileUrl = Utils.ConvertToString(dr[pFileUrl], String.Empty) != "" ? serverPath + Utils.ConvertToString(dr[pFileUrl], String.Empty) : "",
                            UrlView = Utils.ConvertToString(dr[pFileUrl], String.Empty)
                        };

                        break;
                    }

                    dr.Close();
                }

                if (Data == null)
                {
                    Result.Message = "Không có dữ liệu";
                    Result.Status = 0;
                }
                else
                {
                    Result.Message = "Thành công";
                    Result.TotalRow = 1;
                    Result.Status = 1;
                }

                Result.Data = Data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        private string TenCap(int CapID)
        {
            switch (CapID)
            {
                case 3:
                    return "Biểu mẫu cấp Xã";
                case 4:
                case 2:
                    return "Biểu mẫu cấp Tỉnh/Huyện";
                default:
                    return "";
            }
        }

        public BaseResultModel LichSuBieuMauChiTiet(int BieuMauID)
        {
            var Result = new BaseResultModel();

            List<LichSuBieuMauModel> Data = new List<LichSuBieuMauModel>();
            SqlParameter[] parameters =
            {
                new SqlParameter(pBieuMauID, SqlDbType.Int)
            };
            parameters[0].Value = BieuMauID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sChiTietLichSu, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new LichSuBieuMauModel
                        {
                            LichSuID = Utils.ConvertToInt32(dr[pLichSuID], 0),
                            BieuMauID = Utils.ConvertToInt32(dr[pBieuMauID], 0),
                            ThoiGianCapNhat = Utils.ConvertToDateTime_Hour(dr[pThoiGianCapNhat], DateTime.Now),
                            CanBoID = Utils.ConvertToInt32(dr[pLichSuID], 0),
                            TenCanBo = Utils.ConvertToString(dr[pTenCanBo], String.Empty)
                        });
                    }

                    dr.Close();
                }

                if (Data.Count == 0)
                {
                    Result.Message = "Không có dữ liệu";
                    Result.Status = 0;
                }
                else
                {
                    Result.Message = "Thành công";
                    Result.TotalRow = Data.Count;
                    Result.Status = 1;
                    Result.Data = Data;
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        #endregion
    }
}