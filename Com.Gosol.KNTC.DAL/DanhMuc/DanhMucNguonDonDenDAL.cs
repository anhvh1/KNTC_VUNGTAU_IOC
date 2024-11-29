using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

//Create by TienKM 14/10/2022

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucNguonDonDenDAL
    {
        #region Variable

        private readonly string pNguonDonDenID = "NguonDonDenID";
        private readonly string pTenNguonDonDen = "TenNguonDonDen";
        private readonly string pMaNguonDonDen = "MaNguonDonDen";
        private readonly string pGhiChu = "GhiChu";
        private readonly string pTrangThai = "TrangThai";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private const string pTotalRow = "TotalRow";

        private readonly string sDanhSachNguonDonDen = "v2_DanhMuc_NguonDonDen_GetAll";
        private readonly string sKiemTraTenTonTai = "v2_DanhMuc_NguonDonDen_KiemTraTonTai";
        private readonly string sKiemTraMaTonTai = "v2_DanhMuc_NguonDonDen_KiemTraMa";
        private readonly string sThemMoi = "v2_DanhMuc_NguonDonDen_ThemMoi";
        private readonly string sXoaNguonDonDen = "v2_DanhMuc_NguonDonDen_Xoa";
        private readonly string sSuaNguonDonDen = "v2_DanhMuc_NguonDonDen_Sua";
        private readonly string sChiTiet = "v2_DanhMuc_NguonDonDen_ChiTiet";

        #endregion

        #region Function

        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSoLocDanhMuc)
        {
            var Result = new BaseResultModel();
            List<DanhMucNguonDonDenModel> Data = new();

            SqlParameter[] parameters =
            {
                new SqlParameter(pTenNguonDonDen, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.NVarChar),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
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

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sDanhSachNguonDonDen, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucNguonDonDenModel
                        {
                            NguonDonDenID = Utils.ConvertToInt32(dr[pNguonDonDenID], 0),
                            TenNguonDonDen = Utils.ConvertToString(dr[pTenNguonDonDen], string.Empty),
                            MaNguonDonDen = Utils.ConvertToString(dr[pMaNguonDonDen], string.Empty),
                            GhiChu = Utils.ConvertToString(dr[pGhiChu], string.Empty),
                            TrangThai = Utils.ConvertToBoolean(dr[pTrangThai], false)
                        });
                    }

                    dr.Close();
                }

                var total = Convert.ToInt32(parameters[4].Value);
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

        /// <summary>
        /// Chi tiết nguồn đơn đến
        /// </summary>
        /// <param name="NguonDonDenID"></param>
        /// <returns></returns>
        public BaseResultModel ChiTiet(int NguonDonDenID)
        {
            var Result = new BaseResultModel();
            DanhMucNguonDonDenModel Data = null;

            SqlParameter[] parameters =
            {
                new SqlParameter(pNguonDonDenID, SqlDbType.Int)
            };
            parameters[0].Value = NguonDonDenID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, this.sChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        Data = new DanhMucNguonDonDenModel
                        {
                            NguonDonDenID = Utils.ConvertToInt32(dr[pNguonDonDenID], 0),
                            TenNguonDonDen = Utils.ConvertToString(dr[pTenNguonDonDen], string.Empty),
                            MaNguonDonDen = Utils.ConvertToString(dr[pMaNguonDonDen], string.Empty),
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


        private bool KiemTraTenTonTai(string tenNguonDonDen, int? NguonDonDenID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter(pNguonDonDenID, SqlDbType.Int),
                new SqlParameter(pTenNguonDonDen, SqlDbType.NVarChar),
            };

            parameters[0].Value = NguonDonDenID ?? Convert.DBNull;
            parameters[1].Value = tenNguonDonDen;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraTenTonTai, parameters);
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

        private bool KiemTraMaTonTai(string maNguonDonDen, int? NguonDonDenID)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter(pNguonDonDenID, SqlDbType.Int),
                new SqlParameter(pMaNguonDonDen, SqlDbType.NVarChar),
            };

            parameters[0].Value = NguonDonDenID ?? Convert.DBNull;
            parameters[1].Value = maNguonDonDen;

            try
            {
                var rowCount = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraMaTonTai, parameters);
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

        public BaseResultModel XoaNguonDonDen(int NguonDonDenID)
        {
            var Result = new BaseResultModel();

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pNguonDonDenID, SqlDbType.Int),
            };
            parameters[0].Value = NguonDonDenID;

            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sXoaNguonDonDen, parameters);
                        trans.Commit();

                        if (effected == 0)
                        {
                            Result.Message = "ID không tồn tại";
                            Result.Status = 0;
                        }
                        else
                        {
                            Result.Status = 1;
                            Result.Message = "Xóa nguồn đơn đến thành công";
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }

                connection.Close();
            }

            return Result;
        }

        public BaseResultModel ThemMoi(ThemDanhMucNguonDonDenModel NguonDonDen)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(NguonDonDen.TenNguonDonDen.Trim(), null))
            {
                Result.Status = -1;
                Result.Message = "Tên nguồn đơn đến đã tồn tại";
                return Result;
            }

            if (KiemTraMaTonTai(NguonDonDen.MaNguonDonDen, null))
            {
                Result.Status = -1;
                Result.Message = "Mã nguồn đơn đến đã tồn tại";
                return Result;
            }


            SqlParameter[] parameters =
            {
                new SqlParameter(pTenNguonDonDen, SqlDbType.NVarChar),
                new SqlParameter(pMaNguonDonDen, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };

            parameters[0].Value = NguonDonDen.TenNguonDonDen;
            parameters[1].Value = NguonDonDen.MaNguonDonDen.Trim() ?? Convert.DBNull;
            parameters[2].Value = NguonDonDen.GhiChu.Trim() ?? Convert.DBNull;
            parameters[3].Value = NguonDonDen.TrangThai ?? Convert.DBNull;

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
            Result.Message = "Thêm mới nguồn đơn đến thành công";

            return Result;
        }

        public BaseResultModel SuaNguonDonDen(DanhMucNguonDonDenModel NguonDonDen)
        {
            var Result = new BaseResultModel();

            if (KiemTraTenTonTai(NguonDonDen.TenNguonDonDen.Trim(), NguonDonDen.NguonDonDenID))
            {
                Result.Status = -1;
                Result.Message = "Tên nguồn đơn đến đã tồn tại";
                return Result;
            }

            if (KiemTraMaTonTai(NguonDonDen.MaNguonDonDen, NguonDonDen.NguonDonDenID))
            {
                Result.Status = -1;
                Result.Message = "Mã nguồn đơn đến đã tồn tại";
                return Result;
            }

            SqlParameter[] parameters =
            {
                new SqlParameter(pNguonDonDenID, NguonDonDen.NguonDonDenID),
                new SqlParameter(pTenNguonDonDen, SqlDbType.NVarChar),
                new SqlParameter(pMaNguonDonDen, SqlDbType.VarChar),
                new SqlParameter(pGhiChu, SqlDbType.NVarChar),
                new SqlParameter(pTrangThai, SqlDbType.Bit)
            };
            parameters[0].Value = NguonDonDen.NguonDonDenID;
            parameters[1].Value = NguonDonDen.TenNguonDonDen.Trim();
            parameters[2].Value = NguonDonDen.MaNguonDonDen.Trim() ?? Convert.DBNull;
            parameters[3].Value = NguonDonDen.GhiChu.Trim() ?? Convert.DBNull;
            parameters[4].Value = NguonDonDen.TrangThai ?? Convert.DBNull;


            using (var connection = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                connection.Open();
                using (var trans = connection.BeginTransaction())
                {
                    try
                    {
                        var effected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sSuaNguonDonDen, parameters);
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
                            Result.Message = "Cập nhật nguồn đơn đến thành công";
                            Result.Data = NguonDonDen.NguonDonDenID;
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