using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Security;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Grpc.Core;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using System.ComponentModel;

namespace Com.Gosol.KNTC.DAL.DanhMuc
{
    public class DanhMucCoQuanDAL_v2
    {
        private const string FILTERBYNAME = @"v2_DanhMuc_CoQuanDonVi_GetAllByCap";
        private const string GETALL = @"v2_DanhMuc_CoQuanDonVi_DanhSach";
        private const string GETALLBYCAP = @"v2_DanhMuc_CoQuanDonVi_GetAllByCap";
        private const string Checkbool = @"v2_DanhMucCoQuan_KiemTraTonTai";
        private const string AddCap = @"v2_DanhMuc_CoQuanDonVi_ThemMoi";
        private const string Delete = @"v2_DanhMuc_CoQuanDonVi_Delete";
        private const string UpdateCoQuan = @"v2_DanhMuc_CoQuanDonVi_Update";
        private const string sChiTiet = @"v2_DanhMuc_CoQuanDonVi_ChiTiet";
        //----------
        private const string pGhiChu = "@GhiChu";
        private const string pTrangThai = "@TrangThai";
        private const string pKeyword = "@Keyword";
        private const string pType = "@Type";



        private const string pLimit = "@Limit";
        private const string pOffset = "@Offset";
        private const string pTotalRow = "@TotalRow";


        // param constant
        private const string PARAM_CoQuanID = "@CoQuanID";
        private const string PARAM_TenCoQuan = "@TenCoQuan";
        private const string PARAM_CoQuanChaID = "@CoQuanChaID";
        private const string PARAM_CapID = "@CapID";
        private const string PARAM_ThamQuyenID = "@ThamQuyenID";
        private const string PARAM_TinhID = "@TinhID";
        private const string PARAM_HuyenID = "@HuyenID";
        private const string PARAM_XaID = "@XaID";
        private const string PARAM_CapUBND = "@CapUBND";
        private const string PARAM_CapThanhTra = "@CapThanhTra";
        private const string PARAM_CQCoHieuLuc = "@CQCoHieuLuc";
        private const string PARAM_SuDungPM = "@SuDungPM";
        private const string PARAM_MaCQ = "@MaCQ";
        private const string PARAM_SuDungQuyTrinh = "@SuDungQuyTrinh";
        private const string PARAM_MappingCode = "@MappingCode";
        private const string PARAM_IsDisable = "@IsDisable";
        private const string PARAM_TTChiaTachSapNhap = "@TTChiaTachSapNhap";
        private const string PARAM_ChiaTachSapNhapDenCQID = "@ChiaTachSapNhapDenCQID";


        public BaseResultModel DanhSach(ThamSoLocDanhMuc_v2 p)
        {
            var Result = new BaseResultModel();
            List<DanhMucCoQuanDonViMODPartial> Data = new List<DanhMucCoQuanDonViMODPartial>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,255),
                new SqlParameter(pLimit,SqlDbType.Int),
                new SqlParameter(pOffset,SqlDbType.Int),
                new SqlParameter(pTotalRow,SqlDbType.Int),
                new SqlParameter(PARAM_CapID,SqlDbType.Int),

            };
            parameters[0].Value = p.Keyword ?? Convert.DBNull;
            parameters[1].Value = (p.PageSize == 0 ? 10 : p.PageSize);
            parameters[2].Value = (p.PageSize == 0 ? 10 : p.PageSize) * ((p.PageNumber == 0 ? 1 : p.PageNumber) - 1);
            parameters[3].Direction = ParameterDirection.Output;
            parameters[3].Size = 8;
            parameters[4].Value = p.Cap ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, GETALL, parameters))
                {
                    while (dr.Read())
                    {

                        DanhMucCoQuanDonViMODPartial DonVi = new DanhMucCoQuanDonViMODPartial();
                        DonVi.ID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        DonVi.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        DonVi.CoQuanChaID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                        DonVi.MaCQ = Utils.ConvertToString(dr["MaCQ"], string.Empty);
                        DonVi.Cap = Utils.ConvertToInt32(dr["CapID"], 0);
                        DonVi.Highlight = Utils.ConvertToInt32(dr["Highlight"], 0);
                        DonVi.ParentID = Utils.ConvertToInt32(dr["ParentID"], 0);
                        Data.Add(DonVi);
                    }
                    dr.Close();
                }

                Data.ForEach(x => x.Children = Data.Where(y => y.CoQuanChaID == x.ID).ToList());
                Data.RemoveAll(x => x.CoQuanChaID > 0);
                var TotalRow = Utils.ConvertToInt32(parameters[3].Value, 0);
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
        //-----------------------



        //-------kiem tra 
        /*public bool KiemTraTonTai(string keyword, int type, int? CoQuanID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter(PARAM_CoQuanID,SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, Checkbool, parameters))
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
        }*/

        public bool KiemTraTonTai(string keyword, int type, int? CoQuanID)
        {
            var Result = true;

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter(pKeyword,SqlDbType.NVarChar,50),
                new SqlParameter(pType,SqlDbType.Int),
                new SqlParameter("CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = keyword;
            parameters[1].Value = type;
            parameters[2].Value = CoQuanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, Checkbool, parameters))
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

        //---- them 
        public BaseResultModel ThemMoi(AddDanhMucCoQuanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("TenCoQuan", SqlDbType.NVarChar),
                    new SqlParameter("CoQuanChaID", SqlDbType.Int),
                    new SqlParameter("CapID", SqlDbType.Int),
                    new SqlParameter("ThamQuyenID" , SqlDbType.Int),
                    new SqlParameter("TinhID",SqlDbType.Int),
                    new SqlParameter("HuyenID",SqlDbType.Int),
                    new SqlParameter("XaID",SqlDbType.Int),
                    new SqlParameter("CapUBND",SqlDbType.Bit),
                    new SqlParameter("CapThanhTra", SqlDbType.Bit),
                    new SqlParameter("CQCoHieuLuc", SqlDbType.Bit),
                    new SqlParameter("SuDungPM", SqlDbType.Bit),
                    new SqlParameter("MaCQ", SqlDbType.NVarChar),

                    new SqlParameter("SuDungQuyTrinh",SqlDbType.Bit),
                    new SqlParameter("SuDungQuyTrinhGQ",SqlDbType.Bit),
                    new SqlParameter("QTVanThuTiepNhanDon",SqlDbType.Bit),
                    new SqlParameter("QTVanThuTiepDan",SqlDbType.Bit),
                    //new SqlParameter("QuyTrinhGianTiep",SqlDbType.Int),


                };
                parameters[0].Value = item.TenCoQuan.Trim();
                parameters[1].Value = item.CoQuanChaID ?? Convert.DBNull;
                parameters[2].Value = item.CapID ?? Convert.DBNull;
                parameters[3].Value = item.ThamQuyenID ?? Convert.DBNull;
                parameters[4].Value = item.TinhID ?? Convert.DBNull; ;
                parameters[5].Value = item.HuyenID ?? Convert.DBNull;
                parameters[6].Value = item.XaID ?? Convert.DBNull;
                parameters[7].Value = item.CapUBND ?? Convert.DBNull;
                parameters[8].Value = item.CapThanhTra ?? Convert.DBNull;
                parameters[9].Value = item.CQCoHieuLuc ?? Convert.DBNull;
                parameters[10].Value = item.SuDungPM ?? Convert.DBNull;
                parameters[11].Value = item.MaCQ ?? Convert.DBNull;
                parameters[12].Value = item.SuDungQuyTrinh ?? Convert.DBNull; 
                parameters[13].Value = item.SuDungQuyTrinhGQ ?? Convert.DBNull; 
                parameters[14].Value = item.QTVanThuTiepNhanDon ?? Convert.DBNull; 
                parameters[15].Value = item.QTVanThuTiepDan ?? Convert.DBNull;
                //parameters[13].Value = item.QuyTrinhGianTiep;


                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, CommandType.StoredProcedure, AddCap, parameters).ToString(), 0);
                            trans.Commit();
                            Result.Message = "Thêm mới thành công!";
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
        // xoa
        public BaseResultModel Xoa(int CoQuanID)
        {
            var Result = new BaseResultModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("CoQuanID", SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var qr = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, Delete, parameters);
                        trans.Commit();
                        if (qr < 0)
                        {
                            Result.Status = 0;
                            Result.Message = "Không thể xóa cơ quan !";
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
            Result.Message = "Xóa danh mục cơ quan thành công!";
            return Result;
        }

        // tim kiem 
        public BaseResultModel SearchName(string Name)
        {
            var Result = new BaseResultModel();
            DanhMucCoQuanDonViMODPartial data = new DanhMucCoQuanDonViMODPartial();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("TenCoQuan",SqlDbType.NVarChar,(50)),
            };
            parameters[0].Value = Name;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_CoQuanDonVi_GetByName", parameters))
                {
                    while (dr.Read())
                    {
                       
                        data.ID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        data.Ten = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        data.MaCQ = Utils.ConvertToString(dr["MaCQ"], string.Empty);
                        data.Cap = Utils.ConvertToInt32(dr["CapID"], 0);
                        data.CoQuanChaID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                        data.Highlight = Utils.ConvertToInt32(dr["Highlight"], 0);
                        
                        break;
                    }
                    dr.Close();
                }
                
                Result.Status = 1;
                Result.Data = data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString(); 
            }
            return Result;
        }

        // update
        public BaseResultModel CapNhat(UpdateDanhMucCoQuanMOD item)
        {
            var Result = new BaseResultModel();
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("CoQuanID",SqlDbType.Int),
                    new SqlParameter("TenCoQuan", SqlDbType.NVarChar),
                    new SqlParameter("CoQuanChaID", SqlDbType.Int),
                    new SqlParameter("CapID", SqlDbType.Int),
                    new SqlParameter("ThamQuyenID" , SqlDbType.Int),
                    new SqlParameter("TinhID",SqlDbType.Int),
                    new SqlParameter("HuyenID",SqlDbType.Int),
                    new SqlParameter("XaID",SqlDbType.Int),
                    new SqlParameter("CapUBND",SqlDbType.Bit),
                    new SqlParameter("CapThanhTra", SqlDbType.Bit),
                    new SqlParameter("CQCoHieuLuc", SqlDbType.Bit),
                    new SqlParameter("SuDungPM", SqlDbType.Bit),
                    new SqlParameter("MaCQ", SqlDbType.NVarChar),

                    new SqlParameter("SuDungQuyTrinh",SqlDbType.Bit),
                    new SqlParameter("SuDungQuyTrinhGQ",SqlDbType.Bit),
                    new SqlParameter("QTVanThuTiepNhanDon",SqlDbType.Bit),
                    new SqlParameter("QTVanThuTiepDan",SqlDbType.Bit),
                };
                parameters[0].Value = item.CoQuanID;
                parameters[1].Value = item.TenCoQuan.Trim();
                parameters[2].Value = item.CoQuanChaID ?? Convert.DBNull;
                parameters[3].Value = item.CapID ?? Convert.DBNull;
                parameters[4].Value = item.ThamQuyenID ?? Convert.DBNull;
                parameters[5].Value = item.TinhID ?? Convert.DBNull;
                parameters[6].Value = item.HuyenID ?? Convert.DBNull;
                parameters[7].Value = item.XaID ?? Convert.DBNull; ;
                parameters[8].Value = item.CapUBND ?? Convert.DBNull;
                parameters[9].Value = item.CapThanhTra ?? Convert.DBNull;
                parameters[10].Value = item.CQCoHieuLuc ?? Convert.DBNull;
                parameters[11].Value = item.SuDungPM ?? Convert.DBNull; ;
                parameters[12].Value = item.MaCQ ?? Convert.DBNull;
                parameters[13].Value = item.SuDungQuyTrinh ?? Convert.DBNull;
                parameters[14].Value = item.SuDungQuyTrinhGQ ?? Convert.DBNull;
                parameters[15].Value = item.QTVanThuTiepNhanDon ?? Convert.DBNull;
                parameters[16].Value = item.QTVanThuTiepDan ?? Convert.DBNull;
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Result.Status = SQLHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, UpdateCoQuan, parameters);
                            trans.Commit();
                            Result.Message = "Cập nhật cơ quan thành công!";
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
                Result.Message = Constant.ERR_UPDATE;
            }
            return Result;
        }
        // chi tiet 
        public BaseResultModel ChiTiet(int? ID)
        {
            var Result = new BaseResultModel();
            DanhMucCoQuanDonViMOD data = new DanhMucCoQuanDonViMOD();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("CoQuanID",SqlDbType.Int),
            };
            parameters[0].Value = ID.Value;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, sChiTiet, parameters))
                {
                    while (dr.Read())
                    {

                        data.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        data.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        data.CoQuanChaID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                        data.CapID = Utils.ConvertToInt32(dr["CapID"], 0);
                        data.ThamQuyenID = Utils.ConvertToInt32(dr["ThamQuyenID"], 0);
                        data.TinhID = Utils.ConvertToInt32(dr["TinhID"], 0);
                        data.HuyenID = Utils.ConvertToInt32(dr["HuyenID"], 0);
                        data.XaID = Utils.ConvertToInt32(dr["XaID"], 0);
                        data.CapUBND = Utils.ConvertToBoolean(dr["CapUBND"], false);
                        data.CQCoHieuLuc = Utils.ConvertToBoolean(dr["CQCoHieuLuc"], false);
                        data.CapThanhTra = Utils.ConvertToBoolean(dr["CapThanhTra"], false);
                        data.SuDungPM = Utils.ConvertToBoolean(dr["SuDungPM"], false);
                        data.MaCQ = Utils.ConvertToString(dr["MaCQ"], string.Empty);
                        data.SuDungQuyTrinh = Utils.ConvertToBoolean(dr["SuDungQuyTrinh"], false);
                        data.SuDungQuyTrinhGQ = Utils.ConvertToBoolean(dr["SuDungQuyTrinhGQ"], false);
                        data.QTVanThuTiepNhanDon = Utils.ConvertToBoolean(dr["QTVanThuTiepNhanDon"], false);
                        data.QTVanThuTiepDan = Utils.ConvertToBoolean(dr["QTVanThuTiepDan"], false);
                        
                        break;
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = data;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                Result.Message = ex.ToString();
            }
            return Result;
        }


        // cap id
        public BaseResultModel DanhSachCap(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<NameCapCoQuanID> Data = new List<NameCapCoQuanID>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_CoQuanDonVi_CapDonVi"))
                {
                    while (dr.Read())
                    {

                        NameCapCoQuanID DonVi = new NameCapCoQuanID();

                        DonVi.Cap = Utils.ConvertToInt32(dr["CapID"], 0);
                        DonVi.TenCap = Utils.ConvertToString(dr["TenCap"], string.Empty);
                        DonVi.ThuTu = Utils.ConvertToInt32(dr["ThuTu"], 0);

                        Data.Add(DonVi);
                    }
                    dr.Close();
                }


                Result.Status = 1;
                Result.Data = Data;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }
        //--------------------

        // cap id
        public static List<NameCapCoQuanID> ConvertEnumToList()
        {
            var enumType = typeof(EnumCapHanhChinhHDSD);
            if (!enumType.IsEnum)
                throw new ArgumentException("Lỗi không thể convert enum");

            var list = new List<NameCapCoQuanID>();

            foreach (EnumCapHanhChinhHDSD value in Enum.GetValues(enumType))
            {
                var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(
                    enumType.GetField(value.ToString()), typeof(DescriptionAttribute));

                var nameCapCoQuan = new NameCapCoQuanID
                {
                    Cap = (int)value,
                    TenCap = descriptionAttribute != null ? descriptionAttribute.Description : value.ToString()
                };

                list.Add(nameCapCoQuan);
            }

            return list;
        }
        public BaseResultModel DanhSachCap_HDSD(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();

            try
            {
                List<NameCapCoQuanID> list = ConvertEnumToList();
                Result.Status = 1;
                Result.Data = list;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }
        //--------------------


        public BaseResultModel CacCapCoQuan(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<CapBac> Data = new List<CapBac>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_CoQuanDonVi_ThamQuyen"))
                {
                    while (dr.Read())
                    {
                        CapBac item = new CapBac();
                        item.ThamQuyenID = Utils.ConvertToInt32(dr["ThamQuyenID"], 0);
                        item.TenThamQuyen = Utils.ConvertToString(dr["TenThamQuyen"], string.Empty);
                        item.MaThamQuyen = Utils.ConvertToInt32(dr["MaThamQuyen"], 0);
                        Data.Add(item);
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = Data;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

        // co quan cha 
        public BaseResultModel DanhSachCoQuanCha(ThamSoLocDanhMuc p)
        {
            var Result = new BaseResultModel();
            List<CoQuanCha> Data = new List<CoQuanCha>();

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_DanhMuc_CoQuanDonVi_CoQuanCha"))
                {
                    while (dr.Read())
                    {
                        CoQuanCha item = new CoQuanCha();

                        item.CQCha = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);

                        Data.Add(item);
                    }
                    dr.Close();
                }

                Result.Status = 1;
                Result.Data = Data;

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }

    }


}
