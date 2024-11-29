using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Com.Gosol.KNTC.Model.HeThong;
using Com.Gosol.KNTC.Models;
using static Com.Gosol.KNTC.Model.HeThong.ChucNangInfo;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class ChucNangDAL 
    {
        #region Variable

        #region Params

        private readonly string pChucNangID = "ChucNangID";
        private readonly string pChucNangChaID = "ChucNangChaID";
        private readonly string pTenChucNang = "TenChucNang";
        private readonly string pMaChucNang = "MaChucNang";
        private readonly string pThuTuSapXep = "ThuTuSapXep";
        private readonly string pIcon = "Icon";
        private readonly string pHienThiTrenMenu = "HienThiTrenMenu";
        private readonly string pKeyWord = "KeyWord";
        private readonly string pLimit = "Limit";
        private readonly string pOffset = "Offset";
        private readonly string pTotalRow = "TotalRow";

        #endregion

        #region StoreProcedure

        private readonly string sDanhSachChucNang = "v2_DanhMuc_ChucNang_GetAll";
        private readonly string sDanhSachCapCha = "v2_DanhMuc_ChucNang_DanhSachCapCha";
        private readonly string sKiemTraCapCha = "v2_DanhMuc_ChucNang_KiemTraCapCha";
        private readonly string sKiemTraChucNang = "v2_DanhMuc_ChucNang_KiemTraChucNang";
        private readonly string sKiemTraCapCha_v2 = "v2_DanhMuc_ChucNang_KiemTraCapCha_v2";
        private readonly string sChucNang_ThemMoi = "v2_DanhMuc_ChucNang_ThemMoi";
        private readonly string sChucNang_Xoa = "v2_DanhMuc_ChucNang_Xoa";
        private readonly string sChucNang_ChiTiet = "v2_DanhMuc_ChucNang_ChiTiet";
        private readonly string sChucNang_Sua = "v2_DanhMuc_ChucNang_Sua";
        private readonly string sChucNang_FK_PhanQuyen = "v2_DanhMuc_ChucNang_FK_PhanQuyen";
        private readonly string sChucNang_FK_Menu = "v2_DanhMuc_ChucNang_FK_Menu";
        private readonly string sChucNang_FK_ChucNangApDung= "v2_DanhMuc_ChucNang_FK_ChucNangApDung";
        private readonly string sChucNang_FK_HuongDanSuDung= "v2_DanhMuc_ChucNang_FK_HuongDanSuDung";

        #endregion

        #endregion
        public List<ChucNangModel> GetListChucNangByNguoiDungID(int NguoidungID)
        {
            List<ChucNangModel> Result = new List<ChucNangModel>();
            List<ChucNangModel> list = new List<ChucNangModel>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(@"NguoiDungID", SqlDbType.Int),
            };
            parameters[0].Value = NguoidungID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HT_ChucNang_GetByNguoidungID", parameters))
                {

                    while (dr.Read())
                    {
                        ChucNangModel item = new ChucNangModel(Utils.ConvertToInt32(dr["ChucNangID"], 0)
                            , Utils.ConvertToString(dr["TenChucNang"], String.Empty)
                             , Utils.ConvertToString(dr["MaChucNang"], String.Empty)
                            , Utils.ConvertToInt32(dr["Quyen"], 0))
                            ;

                        list.Add(item);
                    }
                    dr.Close();
                    Result = (from m in list
                              group m by new { m.ChucNangID, m.TenChucNang, m.MaChucNang } into chucNang
                              select new ChucNangModel(chucNang.Key.ChucNangID, chucNang.Key.TenChucNang, chucNang.Key.MaChucNang, list.Where(x => x.ChucNangID == chucNang.Key.ChucNangID).ToList().Max(x => x.Quyen))
                              ).ToList();

                }
            }
            catch
            {
                throw;
            }
            return Result;
        }
        public List<MenuModel> GetListMenuByNguoiDungID(int NguoidungID)
        {
            List<MenuModel> Result = new List<MenuModel>();
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter(@"NguoiDungID", SqlDbType.Int),
            };
            parameters[0].Value = NguoidungID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HT_Menu_GetByNguoiDungID", parameters))
                {

                    while (dr.Read())
                    {
                        MenuModel item = new MenuModel();
                        item.MenuID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        item.TenMenu = Utils.ConvertToString(dr["TenChucNang"], string.Empty);
                        item.MaMenu = Utils.ConvertToString(dr["MaChucNang"], String.Empty);
                        item.MenuChaID = Utils.ConvertToInt32(dr["ChucNangChaID"], 0);
                        item.ThuTuSapXep = Utils.ConvertToInt32(dr["ThuTuSapXep"], 0);
                        item.Icon = Utils.ConvertToString(dr["Icon"], String.Empty);
                        item.HienThi = Utils.ConvertToBoolean(dr["HienThiTrenMenu"], false);
                        item.isHover = Utils.ConvertToBoolean(dr["isHover"], false);
                        //item.ViewOnly = Utils.ConvertToBoolean(dr["ViewOnly"], false);
                        Result.Add(item);
                    }
                    dr.Close();

                }
                Result.ForEach(x => x.Children = Result.Where(y => y.MenuChaID == x.MenuID).ToList());
                if (Result.Any(x => x.Children.Count > 0))
                {
                    Result.RemoveAll(x => x.MenuChaID != 0);
                }
            }
            catch
            {
                throw;
            }
            return Result;
        }
        public List<ChucNangModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow)
        {
            List<ChucNangModel> list = new List<ChucNangModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("Keyword",SqlDbType.NVarChar,200),
                new SqlParameter("OrderByName",SqlDbType.NVarChar,50),
                new SqlParameter("OrderByOption",SqlDbType.NVarChar,50),
                new SqlParameter("pLimit",SqlDbType.Int),
                new SqlParameter("pOffset",SqlDbType.Int),
                new SqlParameter("TotalRow",SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword != null ? p.Keyword : "";
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_HeThong_ChucNang_GetPagingBySearch", parameters))
                {
                    while (dr.Read())
                    {
                        ChucNangModel item = new ChucNangModel();
                        item.ChucNangID = Utils.ConvertToInt32(dr["ChucNangID"], 0);
                        item.TenChucNang = Utils.ConvertToString(dr["TenChucNang"], string.Empty);
                        item.MaChucNang = Utils.ConvertToString(dr["MaChucNang"], string.Empty);
                        item.ChucNangChaID = Utils.ConvertToInt32(dr["ChucNangChaID"], 0);
                        item.TenChucNangCha = Utils.ConvertToString(dr["TenChucNangCha"], string.Empty);
                        list.Add(item);
                    }
                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        
        //V2
        #region Function V2
        public BaseResultModel DanhSach(ThamSoLocDanhMuc thamSo)
        {
            var Result = new BaseResultModel();

            List<DanhMucChucNangModel> Data = new();
            SqlParameter[] parameters =
            {
                new SqlParameter(pKeyWord, SqlDbType.NVarChar),
                new SqlParameter(pHienThiTrenMenu, SqlDbType.Bit),
                new SqlParameter(pOffset, SqlDbType.Int),
                new SqlParameter(pLimit, SqlDbType.Int),
                new SqlParameter(pTotalRow, SqlDbType.Int),
            };
            int offset = thamSo.PageNumber < 1 ? 0 : (thamSo.PageNumber - 1) * thamSo.PageSize;
            
            parameters[0].Value = thamSo.Keyword ?? Convert.DBNull;
            parameters[1].Value = thamSo.Status ?? Convert.DBNull;
            parameters[2].Value = offset;
            parameters[3].Value = thamSo.PageSize;
            parameters[4].Direction = ParameterDirection.Output;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachChucNang, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucChucNangModel
                        {
                            ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], ""),
                            ChucNangChaID = dr[pChucNangChaID] == DBNull.Value ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0),
                            MaChucNang = Utils.ConvertToString(dr[pMaChucNang], ""),
                            ThuTuSapXep = Utils.ConvertToInt32(dr[pThuTuSapXep], 0),
                            HienThiTrenMenu = Utils.ConvertToBoolean(dr[pHienThiTrenMenu], false),
                            Icon = Utils.ConvertToString(dr[pIcon], ""),
                            TT = Utils.ConvertToString(dr["TT"],"")
                        });
                    }

                    dr.Close();
                }

                var totalCount = Utils.ConvertToInt32(parameters[4].Value, 0);

                Result.TotalRow = totalCount;
                Result.Status = 1;
                Result.Message = totalCount == 0 || Data.Count == 0 ?  Constant.NO_DATA : "Thành công";
                Result.Data = Data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public bool KiemTraCapCha(int? capChaID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangChaID, capChaID ?? Convert.DBNull),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraCapCha, parameters);
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
        public bool KiemTraFKPhanQuyen(int ChucNangID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, ChucNangID),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_FK_PhanQuyen, parameters);
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
        public bool KiemTraFKMenu(int ChucNangID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, ChucNangID),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_FK_Menu, parameters);
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
        
        public bool KiemTraFKChucNangApDung(int ChucNangID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, ChucNangID),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_FK_ChucNangApDung, parameters);
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
        public bool KiemTraFKHuongDanSuDung(int ChucNangID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, ChucNangID),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_FK_HuongDanSuDung, parameters);
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

        public bool KiemTraCapCon(int? capChaID)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, capChaID ?? Convert.DBNull),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraCapCha_v2, parameters);
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

        public bool KiemTraChucNang(int? chucNangID, string tenChucNang, string maChucNang)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter(pChucNangID, chucNangID ?? Convert.DBNull),
                    new SqlParameter(pTenChucNang, tenChucNang ?? Convert.DBNull),
                    new SqlParameter(pMaChucNang, maChucNang ?? Convert.DBNull),
                };
                var count = SQLHelper.ExecuteScalar(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sKiemTraChucNang, parameters);
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

        public BaseResultModel DanhSachCapCha()
        {
            var Result = new BaseResultModel();

            List<DanhMucChucNangModel> Data = new();
            SqlParameter[] parameters =
            {
            };

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sDanhSachCapCha, parameters))
                {
                    while (dr.Read())
                    {
                        Data.Add(new DanhMucChucNangModel
                        {
                            ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], ""),
                            ChucNangChaID = dr[pChucNangChaID] == DBNull.Value ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0),
                            MaChucNang = Utils.ConvertToString(dr[pMaChucNang], ""),
                            ThuTuSapXep = Utils.ConvertToInt32(dr[pThuTuSapXep], 0),
                            HienThiTrenMenu = Utils.ConvertToBoolean(dr[pHienThiTrenMenu], false),
                            Icon = Utils.ConvertToString(dr[pIcon], "")
                        });
                    }
                    dr.Close();
                }
                Result.Message = "Thành công";
                Result.Status = 1;
                Result.Data = Data;
                Result.TotalRow = Data.Count;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }

            return Result;
        }

        public BaseResultModel ChiTiet(int? chucNangID)
        {
            var Result = new BaseResultModel();

            DanhMucChucNangModel Data = null;
            SqlParameter[] parameters =
            {
                new SqlParameter(pChucNangID, SqlDbType.Int)
            };
            parameters[0].Value = chucNangID ?? Convert.DBNull;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChucNang_ChiTiet, parameters))
                {
                    while (dr.Read())
                    {
                        Data = new DanhMucChucNangModel
                        {
                            ChucNangID = Utils.ConvertToInt32(dr[pChucNangID], 0),
                            TenChucNang = Utils.ConvertToString(dr[pTenChucNang], ""),
                            ChucNangChaID = dr[pChucNangChaID] == null ? null : Utils.ConvertToInt32(dr[pChucNangChaID], 0),
                            MaChucNang = Utils.ConvertToString(dr[pMaChucNang], ""),
                            ThuTuSapXep = Utils.ConvertToInt32(dr[pThuTuSapXep], 0),
                            HienThiTrenMenu = Utils.ConvertToBoolean(dr[pHienThiTrenMenu], false),
                            Icon = Utils.ConvertToString(dr[pIcon], "")
                        };
                        break;
                    }

                    dr.Close();
                }

                if (Data == null)
                {
                    Result.Message = Constant.NO_DATA;
                    Result.Status = -1;
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

        public BaseResultModel ThemMoi(DanhMucChucNangThemMoi ChucNang)
        {
            BaseResultModel Result = new BaseResultModel();
            SqlParameter[] parameters =
            {
                new SqlParameter(pTenChucNang, SqlDbType.NVarChar),
                new SqlParameter(pChucNangChaID, SqlDbType.Int),
                new SqlParameter(pMaChucNang, SqlDbType.NVarChar),
                new SqlParameter(pThuTuSapXep, SqlDbType.Int),
                new SqlParameter(pHienThiTrenMenu, SqlDbType.Bit),
                new SqlParameter(pIcon, SqlDbType.NVarChar),
            };

            parameters[0].Value = ChucNang.TenChucNang;
            parameters[1].Value = ChucNang.ChucNangChaID ?? Convert.DBNull;
            parameters[2].Value = ChucNang.MaChucNang ?? Convert.DBNull;
            parameters[3].Value = ChucNang.ThuTuSapXep ?? Convert.DBNull;
            parameters[4].Value = ChucNang.HienThiTrenMenu ?? Convert.DBNull;
            parameters[5].Value = ChucNang.Icon ?? Convert.DBNull;

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sChucNang_ThemMoi, parameters);
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }

            Result.Status = 1;
            Result.Message = "Thêm mới chức năng thành công!";

            return Result;
        }

        public BaseResultModel Sua(DanhMucChucNangSua ChucNang)
        {
            BaseResultModel Result = new BaseResultModel();
            
            SqlParameter[] parameters =
            {
                new SqlParameter(pChucNangID, SqlDbType.Int),
                new SqlParameter(pTenChucNang, SqlDbType.NVarChar),
                new SqlParameter(pChucNangChaID, SqlDbType.Int),
                new SqlParameter(pMaChucNang, SqlDbType.NVarChar),
                new SqlParameter(pThuTuSapXep, SqlDbType.Int),
                new SqlParameter(pHienThiTrenMenu, SqlDbType.Bit),
                new SqlParameter(pIcon, SqlDbType.NVarChar),
            };

            parameters[0].Value = ChucNang.ChucNangID;
            parameters[1].Value = ChucNang.TenChucNang;
            parameters[2].Value = ChucNang.ChucNangChaID ?? Convert.DBNull;
            parameters[3].Value = ChucNang.MaChucNang ?? Convert.DBNull;
            parameters[4].Value = ChucNang.ThuTuSapXep ?? Convert.DBNull;
            parameters[5].Value = ChucNang.HienThiTrenMenu ?? Convert.DBNull;
            parameters[6].Value = ChucNang.Icon ?? Convert.DBNull;

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                var data = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sChucNang_Sua, parameters);

                trans.Commit();
                if (data == 0)
                {
                    Result.Status = -1;
                    Result.Message = "Chức năng không tồn tại";
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = Constant.MSG_SUKNTCESS;
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }


            return Result;
        }

        public BaseResultModel Xoa(int idChucNang)
        {
            BaseResultModel Result = new BaseResultModel();
            
            SqlParameter[] parameters =
            {
                new SqlParameter(pChucNangID, SqlDbType.Int)
            };

            parameters[0].Value = idChucNang;

            using var con = new SqlConnection(SQLHelper.appConnectionStrings);
            con.Open();
            using var trans = con.BeginTransaction();
            try
            {
                var rowEffected = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, sChucNang_Xoa, parameters);

                if (rowEffected == 0)
                {
                    Result.Status = -1;
                    Result.Message = "Chức năng không tồn tại!";
                }
                else
                {
                    Result.Status = 1;
                    Result.Message = "Xóa chức năng thành công";
                }

                trans.Commit();
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
