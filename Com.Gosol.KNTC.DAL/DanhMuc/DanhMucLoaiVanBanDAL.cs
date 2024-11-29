using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//Create by TienKM 14/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucLoaiVanBanDAL
    {
        #region Variable

        private readonly string pVanBanID = "LoaiVanBanID";
        private readonly string pTenVanBan = "TenVanBan";
        private readonly string pMaVanBan = "MaVanBan";
        private readonly string pGhiChu = "GhiChu";
        private readonly string pTrangThai = "TrangThai";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private const string pTotalRow = @"TotalRow";


        private readonly string sDanhSachLoaiVanBan = "v2_DanhMuc_LoaiVanBan_GetAll";
        private readonly string sKiemTraTonTai = "v2_DanhMuc_LoaiVanBan_KiemTenTraTonTai";
        private readonly string sThemMoi = "v2_DanhMuc_LoaiVanBan_ThemMoi";
        private readonly string sXoaVanBan = "v2_DanhMuc_LoaiVanBan_Xoa";
        private readonly string sCountFK_VBKetQua = "[v2_DanhMuc_LoaiVanBan_CountFK_VBKetQua]";
        private readonly string sSuaVanBan = "v2_DanhMuc_LoaiVanBan_Sua";
        private readonly string sChiTiet = "v2_DanhMuc_LoaiVanBan_ChiTiet";

        #endregion

        #region Function

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            List<DanhMucLoaiVanBanModel> Data = new();
            SqlParameter[] parameters = {
                new SqlParameter(pTenVanBan, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit),
                new SqlParameter(pOffset, SqlDbType.NVarChar),
                new SqlParameter(pLimit, SqlDbType.NVarChar),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };
            int offset;
            if (thamSoLocDanhMuc.PageNumber < 1)
            {
                offset = 0;
            }
            else
            {
                offset = (thamSoLocDanhMuc.PageNumber - 1) * thamSoLocDanhMuc.PageSize;
            }

            parameters[0].Value = thamSoLocDanhMuc.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSoLocDanhMuc.Status ?? Convert.DBNull;
            parameters[2].Value = offset;
            parameters[3].Value = thamSoLocDanhMuc.PageSize;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[4].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sDanhSachLoaiVanBan, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucLoaiVanBanModel
                        {
                            LoaiVanBanID = Utils.GetInt32(dr["LoaiVanBanID"], 0),
                            TenVanBan = Utils.GetString(dr["TenVanBan"], string.Empty),
                            MaVanBan = Utils.GetString(dr["MaVanBan"], string.Empty),
                            GhiChu = Utils.GetString(dr["GhiChu"], string.Empty),
                            TrangThai = Utils.GetBoolean(dr["TrangThai"], false)
                        });
                    }

                    dr.Close();
                }

                var total = Utils.ConvertToInt32(parameters[4].Value, 0);

                Result.TotalRow = total;
                if (total == 0)
                {
                    Result.Message = Constant.NO_DATA;
                    Result.Status = 1;
                }
                else
                {
                    Result.Message = "Thành công";
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

        public BaseResultModel ChiTiet(int LoaiVanBanID)
        {
            var Result = new BaseResultModel();
            DanhMucLoaiVanBanModel Data = null;

            SqlParameter[] parameters = {
                new SqlParameter(pVanBanID, SqlDbType.Int)
            };
            parameters[0].Value = LoaiVanBanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        Data = new DanhMucLoaiVanBanModel
                        {
                            LoaiVanBanID = Utils.ConvertToInt32(dr[pVanBanID], 0),
                            TenVanBan = Utils.ConvertToString(dr[pTenVanBan], string.Empty),
                            MaVanBan = Utils.ConvertToString(dr[pMaVanBan], string.Empty),
                            GhiChu = Utils.ConvertToString(dr[pGhiChu], string.Empty),
                            TrangThai = Utils.ConvertToBoolean(dr[pTrangThai], false)
                        };
                        break;
                    }

                    dr.Close();
                }

                Result.TotalRow = 1;
                Result.Status = 1;
                if (Data == null)
                {
                    Result.Message = "Không có dữ liệu";
                }
                else
                {
                    Result.Message = "Thành công";
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

        public bool KiemTraTenTonTai(string tenVanBan, string maVanBan, int? LoaiVanBanID)
        {
            SqlParameter[] parameters = {
                new SqlParameter(pVanBanID, SqlDbType.Int),
                new SqlParameter(pTenVanBan, SqlDbType.NVarChar),
                new SqlParameter(pMaVanBan, SqlDbType.NVarChar),
            };

            parameters[0].Value = LoaiVanBanID ?? Convert.DBNull;
            parameters[1].Value = tenVanBan;
            parameters[2].Value = maVanBan;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraTonTai, parameters);
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

        public BaseResultModel ThemMoi(ThemDanhMucLoaiVanBanModel vanBan)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(vanBan.TenVanBan.Trim(), vanBan.MaVanBan.Trim(), null))
            {
                Result.Status = -1;
                Result.Message = "Tên, mã văn bản đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters = {
                new SqlParameter(pTenVanBan, SqlDbType.NVarChar),
                new SqlParameter(pMaVanBan, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };

            parameters[0].Value = vanBan.TenVanBan;
            parameters[1].Value = vanBan.MaVanBan ?? Convert.DBNull;
            parameters[2].Value = vanBan.GhiChu ?? Convert.DBNull;
            parameters[3].Value = vanBan.TrangThai ?? Convert.DBNull;


            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sThemMoi, parameters);
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
            Result.Message = "Thêm mới văn bản thành công";

            return Result;
        }

        public BaseResultModel SuaVanBan(DanhMucLoaiVanBanModel vanBan)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(vanBan.TenVanBan.Trim(),vanBan.MaVanBan.Trim(), vanBan.LoaiVanBanID))
            {
                Result.Status = -1;
                Result.Message = "Tên, mã văn bản đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters = {
                new SqlParameter(pVanBanID, vanBan.LoaiVanBanID),
                new SqlParameter(pTenVanBan, SqlDbType.NVarChar),
                new SqlParameter(pMaVanBan, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };
            parameters[0].Value = vanBan.LoaiVanBanID;
            parameters[1].Value = vanBan.TenVanBan.Trim();
            parameters[2].Value = vanBan.MaVanBan.Trim() ?? Convert.DBNull;
            parameters[3].Value = vanBan.GhiChu.Trim() ?? Convert.DBNull;
            parameters[4].Value = vanBan.TrangThai ?? Convert.DBNull;


            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sSuaVanBan, parameters);
                        Result.Data = effected;
                        trans.Commit();
                        if (effected == 0)
                        {
                            Result.Status = 0;
                            Result.Message = "ID không tồn tại";
                        }
                        else
                        {
                            Result.Status = 1;
                            Result.Message = "Cập nhật văn bản thành công";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        public BaseResultModel XoaVanBan(int VanBanID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = {
                new SqlParameter(pVanBanID, SqlDbType.Int),
            };
            parameters[0].Value = VanBanID;

            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sCountFK_VBKetQua, new[]
                        {
                            new SqlParameter(pVanBanID, VanBanID)
                        });

                        if (Convert.ToInt32(count) > 0)
                        {
                            Result.Message = "Văn bản chứa dữ liệu không được xóa";
                            Result.Status = 0;

                            return Result;
                        }
                        
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaVanBan, parameters);
                        trans.Commit();

                        if (effected == 0)
                        {
                            Result.Message = "ID không tồn tại";
                            Result.Status = 0;
                        }
                        else
                        {
                            Result.Status = 1;
                            Result.Message = "Xóa văn bản thành công";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }

        #endregion
    }
}