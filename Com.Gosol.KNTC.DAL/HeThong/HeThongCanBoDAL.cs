using Com.Gosol.KNTC.DAL.DanhMuc;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.DanhMuc;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Security;
using Com.Gosol.KNTC.Ultilities;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using Gosol.Security;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Gosol.KNTC.DAL.HeThong
{
    public class HeThongCanBoDAL
    {
        //private readonly ControllerBase

        //param constant
        private const string PARAM_CanBoID = "@CanBoID";
        private const string PARAM_MaCB = "@MaCB";
        private const string PARAM_TenCanBo = "@TenCanBo";
        private const string PARAM_NgaySinh = "@NgaySinh";
        private const string PARAM_GioiTinh = "@GioiTinh";
        private const string PARAM_DiaChi = "@DiaChi";
        private const string PARAM_ChucVuID = "@ChucVuID";
        private const string PARAM_QuyenKy = "@QuyenKy";
        private const string PARAM_Email = "@Email";
        private const string PARAM_DienThoai = "@DienThoai";
        private const string PARAM_PhongBanID = "@PhongBanID";
        private const string PARAM_CoQuanID = "@CoQuanID";
        private const string PARAM_RoleID = "@RoleID";
        private const string PARAM_QuanTridonVi = "@QuanTridonVi";
        private const string PARAM_CoQuanCuID = "@CoQuanCuID";
        private const string PARAM_CanBoCuID = "@CanBoCuID";
        private const string PARAM_XemTaiLieuMat = "@XemTaiLieuMat";
        private const string PARAM_IsStatus = "@IsStatus";
        private const string PARAM_AnhHoSo = "@AnhHoSo";
        private const string PARAM_HoKhau = "@HoKhau";
        private const string PARAM_MaCQ = "@MaCQ";
        private const string PARAM_CapQuanLy = "@CapQuanLy";
        private const string PARAM_TrangThaiID = "@TrangThaiID";
        private const string PARAM_CMND = "@CMND";
        private const string PARAM_NoiCap = "@NoiCap";
        private const string PARAM_NgayCap = "@NgayCap";
        private const string PARAM_SoLuongAnh = "@SoLuongAnh";
        private const string PARAM_NhomNguoiDungID = "@NhomNguoiDungID";


        #region Cán bộ
        public string GenerationMaCanBo(int CoQuanID)
        {
            string maCanBo = "";
            string maCanBoCurr = "";
            string maCoQuan = "";

            SqlParameter[] parameters1 = new SqlParameter[]
       {
                new SqlParameter(PARAM_CoQuanID, SqlDbType.Int)
       };
            parameters1[0].Value = CoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_DanhMuc_CoQuanDonVi_GetByID", parameters1))
                {
                    while (dr.Read())
                    {
                        maCoQuan = Utils.ConvertToString(dr["MaCQ"], String.Empty);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (string.IsNullOrEmpty(maCoQuan))
            {
                maCoQuan = "CB";
            }
            Random oRandom = new Random();
            int MaVach = oRandom.Next(1000, 99999);
            maCanBo = maCoQuan + MaVach;
            //}
            //else
            //{
            //    string s = maCanBoCurr.Substring(maCanBoCurr.IndexOf("_") + 1).ToString();
            //    int STT = Utils.ConvertToInt32((maCanBoCurr.Substring(maCanBoCurr.IndexOf("_") + 1).ToString()), 0);
            //    STT = STT + 1;
            //}

            return maCanBo;
        }

        public string GenerationTenNguoiDung(string TenCanBo, int? CoQuanDangNhap)
        {
            string result = "";
            string TenCanBo_Unsign = Utils.ConvertToUnSign(TenCanBo);
            string[] temp = TenCanBo_Unsign.Split(' ');
            string ChuCaiDauTrongHoTen = "";
            for (int i = 0; i < temp.Length - 1; i++)
            {
                ChuCaiDauTrongHoTen = ChuCaiDauTrongHoTen + temp[i].Substring(0, 1).ToLower();
            }
            result = temp[temp.Length - 1].ToLower() + ChuCaiDauTrongHoTen;
            return result;
        }

        // Insert Can Bo
        public int Insert_Old(HeThongCanBoModel HeThongCanBoModel, ref int CanBoID, ref string Message, int? CoQuanDangNhapID, int? NguoiDungID, int? CanBoDangNhapID)
        {
            int CoQuanID = HeThongCanBoModel.CoQuanID ?? default(int);
            var CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(CoQuanID).CoQuanSuDungPhanMemID;
            int val = 0;
            if (HeThongCanBoModel.TenCanBo.Trim().Length > 50)
            {
                Message = ConstantLogMessage.API_Error_TooLong;
                return val;
            }
            if (string.IsNullOrEmpty(HeThongCanBoModel.TenCanBo) || HeThongCanBoModel.TenCanBo.Trim().Length <= 0)
            {
                Message = ConstantLogMessage.API_Error_NotFill;
                return val;
            }
            if (!Utils.CheckSpecialCharacter(HeThongCanBoModel.TenCanBo))
            {
                Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
                return val;
            }
            var CanBoByCoQuanAndChucVu = GetAllCanBoByChucVuIDAndCoQuanID(HeThongCanBoModel.ChucVuID, HeThongCanBoModel.CoQuanID);
            if (CanBoByCoQuanAndChucVu.Count > 0)
            {
                Message = "Chức vụ trong cơ quan đã có cán bộ làm việc! Thử lại!";
                return val;
            }
            //var CanBo = GetByMaCB(HeThongCanBoModel.MaCB);
            if (HeThongCanBoModel.CoQuanID == null || HeThongCanBoModel.CoQuanID == 0)
            {
                HeThongCanBoModel.CoQuanID = CoQuanDangNhapID;
            }
            else
            {
                var CoQuan = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                if (CoQuan == null)
                {
                    Message = ConstantLogMessage.Alert_Error_NotExist("Cơ quan");
                    return val;
                }
            }

            SqlParameter[] parameters = new SqlParameter[]
              {
                    new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                    new SqlParameter(PARAM_GioiTinh, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                    new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                    new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                    new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                    new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                    new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                    new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                    new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                    new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                    new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                    new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CanBoID,SqlDbType.Int),
                    new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                    new SqlParameter("VaiTro",SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_SoLuongAnh, SqlDbType.Int),
                    new SqlParameter("NgayBatDauLam", SqlDbType.DateTime),
                    new SqlParameter("NguoiTao", SqlDbType.Int),
                    new SqlParameter("TuLayAnh", SqlDbType.Bit),

              };
            parameters[0].Value = HeThongCanBoModel.TenCanBo;
            parameters[1].Value = HeThongCanBoModel.NgaySinh == null ? Convert.DBNull : HeThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
            parameters[2].Value = HeThongCanBoModel.GioiTinh ?? Convert.DBNull;
            parameters[3].Value = HeThongCanBoModel.DiaChi ?? Convert.DBNull;
            parameters[4].Value = HeThongCanBoModel.ChucVuID ?? Convert.DBNull;
            parameters[5].Value = HeThongCanBoModel.QuyenKy ?? Convert.DBNull;
            parameters[6].Value = HeThongCanBoModel.Email ?? Convert.DBNull;
            parameters[7].Value = HeThongCanBoModel.DienThoai ?? Convert.DBNull;
            parameters[8].Value = HeThongCanBoModel.PhongBanID ?? Convert.DBNull;
            parameters[9].Value = HeThongCanBoModel.CoQuanID ?? Convert.DBNull;
            parameters[10].Value = HeThongCanBoModel.RoleID ?? Convert.DBNull;
            parameters[11].Value = HeThongCanBoModel.QuanTriDonVi ?? Convert.DBNull;
            parameters[12].Value = HeThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
            parameters[13].Value = HeThongCanBoModel.CanBoCuID ?? Convert.DBNull;
            parameters[14].Value = HeThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
            parameters[15].Value = EnumTrangThaiNhanVien.DangLam.GetHashCode();
            parameters[16].Value = HeThongCanBoModel.AnhHoSo ?? Convert.DBNull;
            parameters[17].Value = HeThongCanBoModel.HoKhau ?? Convert.DBNull;
            if (string.IsNullOrEmpty(HeThongCanBoModel.MaCB) || HeThongCanBoModel.MaCB.Trim().Length <= 0)
            {
                parameters[18].Value = GenerationMaCanBo(CoQuanID);
            }
            else
            {
                var CanBoByMa = GetByMaCB(HeThongCanBoModel.MaCB);
                if (CanBoByMa.CanBoID > 0)
                {
                    Message = "Mã cán bộ đã tồn tại!";
                    val = 0;
                    return val;
                }
                parameters[18].Value = HeThongCanBoModel.MaCB;
            }
            //check validate Email/User
            if (HeThongCanBoModel.Email == null || string.IsNullOrEmpty(HeThongCanBoModel.Email))
            {
                //string TenCanBo_Unsign = Utils.ConvertToUnSign(HeThongCanBoModel.TenCanBo);
                //string[] temp = TenCanBo_Unsign.Split(' ');
                ////parameters[6].Value = temp[temp.Length - 1] + parameters[18].Value;
                //string ChuCaiDauTrongHoTen = "";
                //for (int i = 0; i < temp.Length - 1; i++)
                //{
                //    ChuCaiDauTrongHoTen = ChuCaiDauTrongHoTen + temp[i].Substring(0, 1).ToLower();
                //}
                //parameters[6].Value = temp[temp.Length - 1] + ChuCaiDauTrongHoTen;
                //HeThongCanBoModel.Email = parameters[6].Value.ToString();
            }
            else
            {
                if (HeThongCanBoModel.Email.Contains("@") && HeThongCanBoModel.Email.Contains(".com"))
                {
                    if (!Utils.CheckEmail(HeThongCanBoModel.Email))
                    {
                        Message = "Email không đúng định dạng";
                        return val;
                    }
                }

                HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.Email, CoQuanSuDungPhanMemID);
                if (nguoiDung.NguoiDungID > 0)
                {
                    Message = "Email/User đã đựoc sử dụng!";
                    return val;
                }


            }
            parameters[19].Direction = ParameterDirection.Output;
            parameters[20].Value = HeThongCanBoModel.CapQuanLy ?? Convert.DBNull;
            parameters[21].Value = HeThongCanBoModel.VaiTro ?? Convert.DBNull;
            parameters[22].Value = HeThongCanBoModel.CMND ?? Convert.DBNull;
            parameters[23].Value = HeThongCanBoModel.NgayCap == null ? Convert.DBNull : HeThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
            parameters[24].Value = HeThongCanBoModel.NoiCap ?? Convert.DBNull;
            parameters[25].Value = HeThongCanBoModel.SoLuongAnh == 0 ? 0 : HeThongCanBoModel.SoLuongAnh;
            parameters[26].Value = DateTime.Now;
            parameters[27].Value = /*UserRole.CheckAdmin(NguoiDungID.Value) ? 1 : 0*/CanBoDangNhapID ?? Convert.DBNull;
            parameters[28].Value = HeThongCanBoModel.TuLayAnh ?? Convert.DBNull;
            int NguoiDungID_Insert;
            //int NguoiDungID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_Insert_New", parameters), 0);

                        CanBoID = Utils.ConvertToInt32(parameters[19].Value, 0);
                        //var CanBoByID = GetCanBoByID(CanBoID);
                        //val = Update(CanBoByID, ref Message);
                        if (val > 0)
                        {
                            if (HeThongCanBoModel.DanhSachChucVuID != null)
                            {
                                var mess = "";
                                var query = CanBoChucVu_Insert(HeThongCanBoModel.DanhSachChucVuID, val, ref mess);
                                if (query < 0) { Message = mess; return query; }
                            }
                        }
                        //thêm người dùng
                        HeThongNguoiDungModel HeThongNguoiDungModel = new HeThongNguoiDungModel();

                        var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
                        HeThongNguoiDungModel.MatKhau = Cryptor.EncryptPasswordUser(HeThongCanBoModel.Email.Trim().ToLower(), matKhauMacDinh ?? "123456");
                        SqlParameter[] paramrs = new SqlParameter[]
                          {
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Int),
                            new SqlParameter("@CanBoID", SqlDbType.Int),
                            new SqlParameter("@PublicKeys", SqlDbType.NVarChar),

                          };
                        paramrs[0].Value = HeThongCanBoModel.Email.Trim().ToLower();
                        paramrs[1].Value = HeThongNguoiDungModel.MatKhau.Trim();
                        paramrs[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
                        paramrs[3].Value = 1;
                        paramrs[4].Value = Utils.ConvertToInt32(val, 0);
                        paramrs[5].Value = HeThongNguoiDungModel.PublicKeys ?? Convert.DBNull;

                        NguoiDungID_Insert = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_Insert", paramrs), 0);


                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    ////Thêm mnguòi dùng đên nhóm mặc định
                    SystemConfigModel sysConfig = new SystemConfigDAL().GetByKey("Nhom_Nguoi_Dung_Mac_Dinh");
                    if (sysConfig != null)
                    {
                        string strValue = sysConfig.ConfigValue;
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string[] arrValue = strValue.Split(',');
                            foreach (var item in arrValue)
                            {
                                NhomNguoiDungModel nnd = new PhanQuyenDAL().NhomNguoiDung_GetByID(Utils.ConvertToInt32(item.Trim(), 0));
                                if (nnd != null)
                                {
                                    NhomNguoiDungModel nhomNguoiDungModel = new PhanQuyenDAL().NhomNguoiDung_GetByCoQuanIDAndNhomTongID(CoQuanID, Utils.ConvertToInt32(item.Trim(), 0));
                                    NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                                    nd_nnd.NguoiDungID = NguoiDungID_Insert;
                                    if (nhomNguoiDungModel.NhomNguoiDungID != 0)
                                    {
                                        nd_nnd.NhomNguoiDungID = nhomNguoiDungModel.NhomNguoiDungID;
                                    }
                                    else
                                    {
                                        nd_nnd.NhomNguoiDungID = Utils.ConvertToInt32(item.Trim(), 0);
                                    }
                                    BaseResultModel baseResult = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_InsertOne(nd_nnd);
                                }
                            }
                        }
                    }

                    //thêm ca làm việc cho cán bộ
                    if (HeThongCanBoModel.DanhSachCaLamViecID != null && HeThongCanBoModel.DanhSachCaLamViecID.Count > 0)
                    {

                    }
                    Message = ConstantLogMessage.Alert_Insert_Success("nhân viên");
                    return val;
                }
            }

        }

        public int Insert(HeThongCanBoModel HeThongCanBoModel, ref int CanBoID, ref string Message, int? CoQuanDangNhapID, int? NguoiDungID, int? CanBoDangNhapID)
        {
            string TenNguoiDung = "";
            if (string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
            {
                Message = "Tên người dùng không được trống!";
                return 0;
            }
            else
            {
                if (HeThongCanBoModel.TenNguoiDung.Trim().Count() < 3)
                {
                    Message = "Tên người dùng quá ngắn!";
                    return 0;
                }
                HeThongNguoiDungModel nguoiDungTemp = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.TenNguoiDung.Trim().ToLower(), 0);
                if (nguoiDungTemp.NguoiDungID > 0)
                {
                    Message = "Tên người dùng đã được sử dụng!";
                    return 0;
                }
                else
                    TenNguoiDung = HeThongCanBoModel.TenNguoiDung.Trim().ToLower();
            }



            int CoQuanID = HeThongCanBoModel.CoQuanID ?? default(int);
            var CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(CoQuanID).CoQuanSuDungPhanMemID;
            int val = 0;
            if (HeThongCanBoModel.TenCanBo.Trim().Length > 50)
            {
                Message = ConstantLogMessage.API_Error_TooLong;
                return val;
            }
            if (string.IsNullOrEmpty(HeThongCanBoModel.TenCanBo) || HeThongCanBoModel.TenCanBo.Trim().Length <= 0)
            {
                Message = ConstantLogMessage.API_Error_NotFill;
                return val;
            }
            if (!Utils.CheckSpecialCharacter(HeThongCanBoModel.TenCanBo))
            {
                Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
                return val;
            }
            //var CanBoByCoQuanAndChucVu = GetAllCanBoByChucVuIDAndCoQuanID(HeThongCanBoModel.ChucVuID, HeThongCanBoModel.CoQuanID);
            //if (CanBoByCoQuanAndChucVu.Count > 0)
            //{
            //    Message = "Chức vụ trong cơ quan đã có cán bộ làm việc! Thử lại!";
            //    return val;
            //}
            //var CanBo = GetByMaCB(HeThongCanBoModel.MaCB);
            if (HeThongCanBoModel.CoQuanID == null || HeThongCanBoModel.CoQuanID == 0)
            {
                HeThongCanBoModel.CoQuanID = CoQuanDangNhapID;
            }
            else
            {
                var CoQuan = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                if (CoQuan == null)
                {
                    Message = ConstantLogMessage.Alert_Error_NotExist("Cơ quan");
                    return val;
                }
            }
            if (string.IsNullOrEmpty(HeThongCanBoModel.Email) && string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
            {
                Message = "Tài khoản người dùng là bắt buộc!";
                return val;
            }

            SqlParameter[] parameters = new SqlParameter[]
              {
                    new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                    new SqlParameter(PARAM_GioiTinh, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                    new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                    new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                    new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                    new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                    //new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                    //new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                    //new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                    new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                    //new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                    //new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                    //new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                    //new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CanBoID,SqlDbType.Int),
                    new SqlParameter("ChuTichUBND",SqlDbType.Int),
                    //new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                    //new SqlParameter("VaiTro",SqlDbType.NVarChar),
                    //new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                    //new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                    //new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                    //new SqlParameter(PARAM_SoLuongAnh, SqlDbType.Int),
                    //new SqlParameter("NgayBatDauLam", SqlDbType.DateTime),
                    //new SqlParameter("NguoiTao", SqlDbType.Int),
                    //new SqlParameter("TuLayAnh", SqlDbType.Bit),
                    //new SqlParameter("LaCanBo", SqlDbType.Bit),

              };
            parameters[0].Value = HeThongCanBoModel.TenCanBo ?? Convert.DBNull;
            parameters[1].Value = HeThongCanBoModel.NgaySinh == null ? Convert.DBNull : HeThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
            parameters[2].Value = HeThongCanBoModel.GioiTinh ?? Convert.DBNull;
            parameters[3].Value = HeThongCanBoModel.DiaChi ?? Convert.DBNull;
            parameters[4].Value = HeThongCanBoModel.ChucVuID ?? Convert.DBNull;
            parameters[5].Value = HeThongCanBoModel.QuyenKy ?? Convert.DBNull;
            parameters[6].Value = HeThongCanBoModel.Email ?? Convert.DBNull;
            parameters[7].Value = HeThongCanBoModel.DienThoai ?? Convert.DBNull;
            parameters[8].Value = HeThongCanBoModel.PhongBanID ?? Convert.DBNull;
            parameters[9].Value = HeThongCanBoModel.CoQuanID ?? Convert.DBNull;
            parameters[10].Value = HeThongCanBoModel.RoleID ?? Convert.DBNull;
            //parameters[11].Value = HeThongCanBoModel.QuanTridonVi ?? Convert.DBNull;
            //parameters[12].Value = HeThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
            //parameters[13].Value = HeThongCanBoModel.CanBoCuID ?? Convert.DBNull;
            parameters[11].Value = HeThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
            parameters[13].Value = HeThongCanBoModel.ChuTichUBND ?? Convert.DBNull;
            //parameters[15].Value = EnumTrangThaiNhanVien.DangLam.GetHashCode();
            //parameters[16].Value = HeThongCanBoModel.AnhHoSo ?? Convert.DBNull;
            //parameters[17].Value = HeThongCanBoModel.HoKhau ?? Convert.DBNull;
            //if (string.IsNullOrEmpty(HeThongCanBoModel.MaCB) || HeThongCanBoModel.MaCB.Trim().Length <= 0)
            //{
            //    parameters[18].Value = GenerationMaCanBo(CoQuanID);
            //}
            //else
            //{
            //    var CanBoByMa = GetByMaCB(HeThongCanBoModel.MaCB);
            //    if (CanBoByMa.CanBoID > 0)
            //    {
            //        Message = "Mã cán bộ đã tồn tại!";
            //        val = 0;
            //        return val;
            //    }
            //    parameters[18].Value = HeThongCanBoModel.MaCB;
            //}
            //check validate Email/User
            if (HeThongCanBoModel.Email == null || string.IsNullOrEmpty(HeThongCanBoModel.Email))
            {
                //string TenCanBo_Unsign = Utils.ConvertToUnSign(HeThongCanBoModel.TenCanBo);
                //string[] temp = TenCanBo_Unsign.Split(' ');
                ////parameters[6].Value = temp[temp.Length - 1] + parameters[18].Value;
                //string ChuCaiDauTrongHoTen = "";
                //for (int i = 0; i < temp.Length - 1; i++)
                //{
                //    ChuCaiDauTrongHoTen = ChuCaiDauTrongHoTen + temp[i].Substring(0, 1).ToLower();
                //}
                //parameters[6].Value = temp[temp.Length - 1] + ChuCaiDauTrongHoTen;
                //HeThongCanBoModel.Email = parameters[6].Value.ToString();
            }
            else
            {
                if (HeThongCanBoModel.Email.Contains("@") && HeThongCanBoModel.Email.Contains(".com"))
                {
                    if (!Utils.CheckEmail(HeThongCanBoModel.Email))
                    {
                        Message = "Email không đúng định dạng";
                        return val;
                    }
                }

                HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.Email, CoQuanSuDungPhanMemID);
                if (nguoiDung.NguoiDungID > 0)
                {
                    Message = "Email đã đựoc sử dụng!";
                    return val;
                }
            }
            parameters[12].Direction = ParameterDirection.Output;
            //parameters[20].Value = HeThongCanBoModel.CapQuanLy ?? Convert.DBNull;
            //parameters[21].Value = HeThongCanBoModel.VaiTro ?? Convert.DBNull;
            //parameters[22].Value = HeThongCanBoModel.CMND ?? Convert.DBNull;
            //parameters[23].Value = HeThongCanBoModel.NgayCap == null ? Convert.DBNull : HeThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
            //parameters[24].Value = HeThongCanBoModel.NoiCap ?? Convert.DBNull;
            //parameters[25].Value = HeThongCanBoModel.SoLuongAnh == 0 ? 0 : HeThongCanBoModel.SoLuongAnh;
            //parameters[26].Value = HeThongCanBoModel.NgayBatDauLam != null ? HeThongCanBoModel.NgayBatDauLam : DateTime.Now;
            //parameters[27].Value = /*UserRole.CheckAdmin(NguoiDungID.Value) ? 1 : 0*/CanBoDangNhapID ?? Convert.DBNull;
            //parameters[28].Value = HeThongCanBoModel.TuLayAnh ?? Convert.DBNull;
            //parameters[29].Value = HeThongCanBoModel.LaCanBo ?? false;
            int NguoiDungID_Insert;

            //int NguoiDungID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_CanBo_Insert_New", parameters), 0);

                        CanBoID = Utils.ConvertToInt32(parameters[12].Value, 0);

                        //if (val > 0)
                        //{
                        //    if (HeThongCanBoModel.DanhSachChucVuID != null)
                        //    {
                        //        var mess = "";
                        //        var query = CanBoChucVu_Insert(HeThongCanBoModel.DanhSachChucVuID, val, ref mess);
                        //        if (query < 0) { Message = mess; return query; }
                        //    }
                        //}
                        //thêm người dùng
                        HeThongNguoiDungModel HeThongNguoiDungModel = new HeThongNguoiDungModel();
                        //string TenNguoiDung = (string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung) && !string.IsNullOrEmpty(HeThongCanBoModel.Email)) ? HeThongCanBoModel.Email.Trim().ToLower() : HeThongCanBoModel.TenNguoiDung.Trim().ToLower();
                        var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
                        //HeThongNguoiDungModel.MatKhau = Cryptor.EncryptPasswordUser("", matKhauMacDinh ?? "123456");
                        HeThongNguoiDungModel.MatKhau = Utils.HashFile(Encoding.ASCII.GetBytes(matKhauMacDinh ?? "123456")).ToUpper();
                        SqlParameter[] paramrs = new SqlParameter[]
                          {
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Int),
                            new SqlParameter("@CanBoID", SqlDbType.Int),
                            new SqlParameter("@SSOID", SqlDbType.NVarChar),
                            //new SqlParameter("@PublicKeys", SqlDbType.NVarChar),

                          };
                        paramrs[0].Value = TenNguoiDung;
                        paramrs[1].Value = HeThongNguoiDungModel.MatKhau.Trim();
                        paramrs[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
                        paramrs[3].Value = 1;
                        paramrs[4].Value = Utils.ConvertToInt32(val, 0);
                        paramrs[5].Value = HeThongCanBoModel.SSOID ?? Convert.DBNull;
                        //paramrs[5].Value = HeThongNguoiDungModel.PublicKeys ?? Convert.DBNull;

                        NguoiDungID_Insert = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_NguoiDung_Insert_New", paramrs), 0);


                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }

                    if (HeThongCanBoModel.DanhSachNhomNguoiDungID != null && HeThongCanBoModel.DanhSachNhomNguoiDungID.Count > 0)
                    {
                        foreach (var item in HeThongCanBoModel.DanhSachNhomNguoiDungID)
                        {
                            NhomNguoiDungModel nnd = new PhanQuyenDAL().NhomNguoiDung_GetByID(item);
                            if (nnd != null)
                            {
                                NhomNguoiDungModel nhomNguoiDungModel = new PhanQuyenDAL().NhomNguoiDung_GetByCoQuanIDAndNhomTongID(CoQuanID, item);
                                NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                                nd_nnd.NguoiDungID = NguoiDungID_Insert;
                                if (nhomNguoiDungModel.NhomNguoiDungID != 0)
                                {
                                    nd_nnd.NhomNguoiDungID = nhomNguoiDungModel.NhomNguoiDungID;
                                }
                                else
                                {
                                    nd_nnd.NhomNguoiDungID = item;
                                }
                                BaseResultModel baseResult = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_InsertOne(nd_nnd);
                            }
                        }
                    }
                    ////Thêm mnguòi dùng đên nhóm mặc định
                    SystemConfigModel sysConfig = new SystemConfigDAL().GetByKey("Nhom_Nguoi_Dung_Mac_Dinh");
                    if (sysConfig != null)
                    {
                        string strValue = sysConfig.ConfigValue;
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string[] arrValue = strValue.Split(',');
                            foreach (var item in arrValue)
                            {
                                NhomNguoiDungModel nnd = new PhanQuyenDAL().NhomNguoiDung_GetByID(Utils.ConvertToInt32(item.Trim(), 0));
                                if (nnd != null)
                                {
                                    NhomNguoiDungModel nhomNguoiDungModel = new PhanQuyenDAL().NhomNguoiDung_GetByCoQuanIDAndNhomTongID(CoQuanID, Utils.ConvertToInt32(item.Trim(), 0));
                                    NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                                    nd_nnd.NguoiDungID = NguoiDungID_Insert;
                                    if (nhomNguoiDungModel.NhomNguoiDungID != 0)
                                    {
                                        nd_nnd.NhomNguoiDungID = nhomNguoiDungModel.NhomNguoiDungID;
                                    }
                                    else
                                    {
                                        nd_nnd.NhomNguoiDungID = Utils.ConvertToInt32(item.Trim(), 0);
                                    }
                                    BaseResultModel baseResult = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_InsertOne(nd_nnd);
                                }
                            }
                        }
                    }

                    Message = ConstantLogMessage.Alert_Insert_Success("nhân viên");
                    return val;
                }
            }

        }

        public int InsertNguoiDangKy(HeThongCanBoModel HeThongCanBoModel, ref string Message)
        {
            int CoQuanID = HeThongCanBoModel.CoQuanID ?? 1;
            int val = 0;
            if (HeThongCanBoModel.TenCanBo != null && HeThongCanBoModel.TenCanBo.Trim().Length > 50)
            {
                Message = ConstantLogMessage.API_Error_TooLong;
                return val;
            }

            if (string.IsNullOrEmpty(HeThongCanBoModel.Email) && string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
            {
                Message = "Tài khoản người dùng là bắt buộc!";
                return val;
            }

            SqlParameter[] parameters = new SqlParameter[]
              {
                    new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                    new SqlParameter(PARAM_GioiTinh, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                    new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                    new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                    new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                    new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                    new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                    new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                    new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                    new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                    new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                    new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CanBoID,SqlDbType.Int),
                    new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                    new SqlParameter("VaiTro",SqlDbType.NVarChar),
                    new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                    new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                    new SqlParameter(PARAM_SoLuongAnh, SqlDbType.Int),
                    new SqlParameter("NgayBatDauLam", SqlDbType.DateTime),
                    new SqlParameter("NguoiTao", SqlDbType.Int),
                    new SqlParameter("TuLayAnh", SqlDbType.Bit),
                    new SqlParameter("LaCanBo", SqlDbType.Bit),

              };
            parameters[0].Value = HeThongCanBoModel.TenCanBo ?? Convert.DBNull;
            parameters[1].Value = HeThongCanBoModel.NgaySinh == null ? Convert.DBNull : HeThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
            parameters[2].Value = HeThongCanBoModel.GioiTinh ?? Convert.DBNull;
            parameters[3].Value = HeThongCanBoModel.DiaChi ?? Convert.DBNull;
            parameters[4].Value = HeThongCanBoModel.ChucVuID ?? Convert.DBNull;
            parameters[5].Value = HeThongCanBoModel.QuyenKy ?? Convert.DBNull;
            parameters[6].Value = HeThongCanBoModel.Email ?? Convert.DBNull;
            parameters[7].Value = HeThongCanBoModel.DienThoai ?? Convert.DBNull;
            parameters[8].Value = HeThongCanBoModel.PhongBanID ?? Convert.DBNull;
            parameters[9].Value = HeThongCanBoModel.CoQuanID ?? 1;
            parameters[10].Value = HeThongCanBoModel.RoleID ?? Convert.DBNull;
            parameters[11].Value = HeThongCanBoModel.QuanTriDonVi ?? Convert.DBNull;
            parameters[12].Value = HeThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
            parameters[13].Value = HeThongCanBoModel.CanBoCuID ?? Convert.DBNull;
            parameters[14].Value = HeThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
            parameters[15].Value = EnumTrangThaiNhanVien.DangLam.GetHashCode();
            parameters[16].Value = HeThongCanBoModel.AnhHoSo ?? Convert.DBNull;
            parameters[17].Value = HeThongCanBoModel.HoKhau ?? Convert.DBNull;
            parameters[18].Value = Convert.DBNull;

            //check validate Email/User
            if (HeThongCanBoModel.Email == null || string.IsNullOrEmpty(HeThongCanBoModel.Email))
            {

            }
            else
            {
                if (HeThongCanBoModel.Email.Contains("@") && HeThongCanBoModel.Email.Contains(".com"))
                {
                    if (!Utils.CheckEmail(HeThongCanBoModel.Email))
                    {
                        Message = "Email không đúng định dạng";
                        return val;
                    }
                }

                HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.Email, 1);
                if (nguoiDung.NguoiDungID > 0)
                {
                    Message = "Email đã đựoc sử dụng!";
                    return val;
                }
            }
            parameters[19].Direction = ParameterDirection.Output;
            parameters[20].Value = HeThongCanBoModel.CapQuanLy ?? Convert.DBNull;
            parameters[21].Value = HeThongCanBoModel.VaiTro ?? Convert.DBNull;
            parameters[22].Value = HeThongCanBoModel.CMND ?? Convert.DBNull;
            parameters[23].Value = HeThongCanBoModel.NgayCap == null ? Convert.DBNull : HeThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
            parameters[24].Value = HeThongCanBoModel.NoiCap ?? Convert.DBNull;
            parameters[25].Value = HeThongCanBoModel.SoLuongAnh == 0 ? 0 : HeThongCanBoModel.SoLuongAnh;
            parameters[26].Value = HeThongCanBoModel.NgayBatDauLam != null ? HeThongCanBoModel.NgayBatDauLam : DateTime.Now;
            parameters[27].Value = Convert.DBNull;
            parameters[28].Value = HeThongCanBoModel.TuLayAnh ?? Convert.DBNull;
            parameters[29].Value = HeThongCanBoModel.LaCanBo ?? false;

            int NguoiDungID_Insert;
            string TenNguoiDung = (string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung) && !string.IsNullOrEmpty(HeThongCanBoModel.Email)) ? HeThongCanBoModel.Email.Trim().ToLower() : HeThongCanBoModel.TenNguoiDung.Trim().ToLower();
            HeThongNguoiDungModel nguoiDungTemp = new HeThongNguoiDungDAL().GetByName(TenNguoiDung, 0);
            if (nguoiDungTemp.NguoiDungID > 0)
            {
                Message = "Tên người dùng đã đựoc sử dụng!";
                return val;
            }
            //int NguoiDungID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_Insert_New", parameters), 0);

                        //CanBoID = Utils.ConvertToInt32(parameters[19].Value, 0);

                        if (val > 0)
                        {
                            if (HeThongCanBoModel.DanhSachChucVuID != null)
                            {
                                var mess = "";
                                var query = CanBoChucVu_Insert(HeThongCanBoModel.DanhSachChucVuID, val, ref mess);
                                if (query < 0) { Message = mess; return query; }
                            }
                        }
                        //thêm người dùng
                        HeThongNguoiDungModel HeThongNguoiDungModel = new HeThongNguoiDungModel();

                        var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
                        if (HeThongCanBoModel.MatKhau != null && HeThongCanBoModel.MatKhau.Length > 0) matKhauMacDinh = HeThongCanBoModel.MatKhau;
                        HeThongNguoiDungModel.MatKhau = Cryptor.EncryptPasswordUser("", matKhauMacDinh ?? "123456");
                        SqlParameter[] paramrs = new SqlParameter[]
                          {
                            new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                            new SqlParameter("@TrangThai", SqlDbType.Int),
                            new SqlParameter("@CanBoID", SqlDbType.Int),
                            new SqlParameter("@PublicKeys", SqlDbType.NVarChar),

                          };
                        paramrs[0].Value = TenNguoiDung;
                        paramrs[1].Value = HeThongNguoiDungModel.MatKhau.Trim();
                        paramrs[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
                        paramrs[3].Value = 1;
                        paramrs[4].Value = Utils.ConvertToInt32(val, 0);
                        paramrs[5].Value = HeThongNguoiDungModel.PublicKeys ?? Convert.DBNull;

                        NguoiDungID_Insert = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_NguoiDung_Insert", paramrs), 0);


                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    ////Thêm mnguòi dùng đên nhóm mặc định
                    SystemConfigModel sysConfig = new SystemConfigDAL().GetByKey("Nhom_Nguoi_Dung_Mac_Dinh");
                    if (sysConfig != null)
                    {
                        string strValue = sysConfig.ConfigValue;
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string[] arrValue = strValue.Split(',');
                            foreach (var item in arrValue)
                            {
                                NhomNguoiDungModel nnd = new PhanQuyenDAL().NhomNguoiDung_GetByID(Utils.ConvertToInt32(item.Trim(), 0));
                                if (nnd != null)
                                {
                                    NhomNguoiDungModel nhomNguoiDungModel = new PhanQuyenDAL().NhomNguoiDung_GetByCoQuanIDAndNhomTongID(CoQuanID, Utils.ConvertToInt32(item.Trim(), 0));
                                    NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                                    nd_nnd.NguoiDungID = NguoiDungID_Insert;
                                    if (nhomNguoiDungModel.NhomNguoiDungID != 0)
                                    {
                                        nd_nnd.NhomNguoiDungID = nhomNguoiDungModel.NhomNguoiDungID;
                                    }
                                    else
                                    {
                                        nd_nnd.NhomNguoiDungID = Utils.ConvertToInt32(item.Trim(), 0);
                                    }
                                    BaseResultModel baseResult = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_InsertOne(nd_nnd);
                                }
                            }
                        }
                    }

                    Message = "Đăng ký thành công!";
                    return val;
                }
            }

        }


        public int InsertForImportExcel(List<HeThongCanBoPartialModel> DanhSachCanBoImport, ref int CanBoID, ref string Message, int? CoQuanDangNhapID, int? NguoiDungID, int? CanBoDangNhapID)
        {
            int val = 0;
            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                foreach (var _heThongCanBoModel in DanhSachCanBoImport)
                {
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //foreach (var _heThongCanBoModel in DanhSachCanBoImport)
                            //{
                            SqlParameter[] parameters = new SqlParameter[]
                             {
                                       new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                                       new SqlParameter(PARAM_GioiTinh, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                                       new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                                       new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                                       new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                                       new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                                       new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                                       new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                                       new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                                       new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                                       new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                                       new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_CanBoID,SqlDbType.Int),
                                       new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                                       new SqlParameter("VaiTro",SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                                       new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                                       new SqlParameter(PARAM_SoLuongAnh, SqlDbType.Int),
                                       new SqlParameter("NgayBatDauLam", SqlDbType.DateTime),
                                       new SqlParameter("NguoiTao", SqlDbType.Int),
                                       new SqlParameter("TuLayAnh", SqlDbType.Bit),
                                       new SqlParameter("@PasswordMacDinh", SqlDbType.NVarChar),
                                       new SqlParameter("@ĐanhSachChucVuID", SqlDbType.Structured),
                                       new SqlParameter("@DanhSachCaLamViecID", SqlDbType.Structured),
                                       new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),

                             };
                            parameters[0].Value = _heThongCanBoModel.TenCanBo;
                            parameters[1].Value = _heThongCanBoModel.NgaySinh == null ? Convert.DBNull : _heThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
                            parameters[2].Value = _heThongCanBoModel.GioiTinh ?? Convert.DBNull;
                            parameters[3].Value = _heThongCanBoModel.DiaChi ?? Convert.DBNull;
                            parameters[4].Value = _heThongCanBoModel.ChucVuID ?? Convert.DBNull;
                            parameters[5].Value = _heThongCanBoModel.QuyenKy ?? Convert.DBNull;
                            parameters[6].Value = _heThongCanBoModel.Email ?? Convert.DBNull;
                            parameters[7].Value = _heThongCanBoModel.DienThoai ?? Convert.DBNull;
                            parameters[8].Value = _heThongCanBoModel.PhongBanID ?? Convert.DBNull;
                            parameters[9].Value = _heThongCanBoModel.CoQuanID ?? Convert.DBNull;
                            parameters[10].Value = _heThongCanBoModel.RoleID ?? Convert.DBNull;
                            parameters[11].Value = _heThongCanBoModel.QuanTriDonVi ?? Convert.DBNull;
                            parameters[12].Value = _heThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
                            parameters[13].Value = _heThongCanBoModel.CanBoCuID ?? Convert.DBNull;
                            parameters[14].Value = _heThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
                            parameters[15].Value = EnumTrangThaiNhanVien.DangLam.GetHashCode();
                            parameters[16].Value = _heThongCanBoModel.AnhHoSo ?? Convert.DBNull;
                            parameters[17].Value = _heThongCanBoModel.HoKhau ?? Convert.DBNull;
                            if (string.IsNullOrEmpty(_heThongCanBoModel.MaCB) || _heThongCanBoModel.MaCB.Trim().Length <= 0)
                            {
                                parameters[18].Value = GenerationMaCanBo(_heThongCanBoModel.CoQuanID.Value);
                            }
                            else
                            {
                                var CanBoByMa = GetByMaCB(_heThongCanBoModel.MaCB);
                                if (CanBoByMa.CanBoID > 0)
                                {
                                    Message = "Mã cán bộ đã tồn tại!";
                                    val = 0;
                                    return val;
                                }
                                parameters[18].Value = _heThongCanBoModel.MaCB;
                            }
                            parameters[19].Direction = ParameterDirection.Output;
                            parameters[20].Value = _heThongCanBoModel.CapQuanLy ?? Convert.DBNull;
                            parameters[21].Value = _heThongCanBoModel.VaiTro ?? Convert.DBNull;
                            parameters[22].Value = _heThongCanBoModel.CMND ?? Convert.DBNull;
                            parameters[23].Value = _heThongCanBoModel.NgayCap == null ? Convert.DBNull : _heThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
                            parameters[24].Value = _heThongCanBoModel.NoiCap ?? Convert.DBNull;
                            parameters[25].Value = _heThongCanBoModel.SoLuongAnh == 0 ? 0 : _heThongCanBoModel.SoLuongAnh;
                            parameters[26].Value = DateTime.Now;
                            parameters[27].Value = /*UserRole.CheckAdmin(NguoiDungID.Value) ? 1 : 0*/CanBoDangNhapID ?? Convert.DBNull;
                            parameters[28].Value = _heThongCanBoModel.TuLayAnh ?? Convert.DBNull;
                            parameters[29].Value = Cryptor.EncryptPasswordUser(_heThongCanBoModel.TenNguoiDung.Trim().ToLower(), matKhauMacDinh ?? "123456");
                            parameters[32].Value = _heThongCanBoModel.TenNguoiDung.Trim().ToLower();
                            var tbChucVuID = new DataTable();
                            tbChucVuID.Columns.Add("ChucVuID", typeof(string));
                            if (_heThongCanBoModel.DanhSachChucVuID != null)
                            {
                                _heThongCanBoModel.DanhSachChucVuID.ForEach(x => tbChucVuID.Rows.Add(x));
                            }
                            parameters[30].Value = tbChucVuID;
                            var tbCaLamViecID = new DataTable();
                            tbCaLamViecID.Columns.Add("CaLamViecID", typeof(string));
                            if (_heThongCanBoModel.DanhSachCaLamViecID != null)
                            {
                                _heThongCanBoModel.DanhSachCaLamViecID.ForEach(x => tbCaLamViecID.Rows.Add(x));
                            }
                            parameters[31].Value = tbCaLamViecID;
                            val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_Insert_For_ImportExcel", parameters), 0);
                            trans.Commit();
                            //}
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            return val;
        }

        // Update 
        public int Update_Old(HeThongCanBoModel HeThongCanBoModel, ref string Message, int? NguoiDungID)
        {
            var CanBoOld = GetCanBoByID(HeThongCanBoModel.CanBoID);
            var CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID).CoQuanSuDungPhanMemID;
            int val = 0;
            var CanBoByCoQuanAndChucVu = GetAllCanBoByChucVuIDAndCoQuanID(HeThongCanBoModel.ChucVuID, HeThongCanBoModel.CoQuanID);
            if (CanBoByCoQuanAndChucVu.Count > 0)
            {
                Message = "Chức vụ trong cơ quan đã có cán bộ làm việc! Thử lại!";
                return val;
            }
            if (HeThongCanBoModel.CanBoID == 0)
            {
                Message = "Chưa có cán bộ được chọn!";
                return val;
            }
            if (HeThongCanBoModel.TenCanBo.Trim().Length > 50)
            {

                Message = ConstantLogMessage.API_Error_TooLong;
                return val;
            }
            if (string.IsNullOrEmpty(HeThongCanBoModel.TenCanBo) || HeThongCanBoModel.TenCanBo.Trim().Length <= 0)
            {

                Message = ConstantLogMessage.API_Error_NotFill;
                return val;
            }
            if (!Utils.CheckSpecialCharacter(HeThongCanBoModel.TenCanBo))
            {

                Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
                return val;
            }
            var crCanBo = GetCanBoByID(HeThongCanBoModel.CanBoID);
            if (crCanBo == null || crCanBo.CanBoID < 1)
            {
                Message = ConstantLogMessage.Alert_Error_NotExist("Cán bộ");
                return val;
            }
            if (!string.IsNullOrEmpty(HeThongCanBoModel.MaCB))
            {
                var CanBoByMaCb = GetByMaCB(HeThongCanBoModel.MaCB.Trim()); ;
                if (CanBoByMaCb.CanBoID != HeThongCanBoModel.CanBoID && CanBoByMaCb.MaCB == HeThongCanBoModel.MaCB)
                {
                    Message = "Mã cán bộ đã tồn tại";
                    return val;
                }
            }
            else if (string.IsNullOrEmpty(HeThongCanBoModel.MaCB))
            {
                HeThongCanBoModel.MaCB = GenerationMaCanBo(HeThongCanBoModel.CoQuanID.Value);
            }
            //if (HeThongCanBoModel.CapQuanLy == null || HeThongCanBoModel.CapQuanLy < 1)
            //{
            //    Message = "cấp quản lý không được trống";
            //    return val;
            //}
            if (HeThongCanBoModel.CoQuanID != null)
            {
                var CoQuan = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                if (CoQuan == null)
                {
                    Message = ConstantLogMessage.Alert_Error_NotExist("công ty");
                    return val;
                }
                else
                {
                    var CoQuanByID = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                }
            }
            SqlParameter[] parameters = new SqlParameter[]
              {
                            new SqlParameter(PARAM_CanBoID, SqlDbType.Int),
                            new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                            new SqlParameter(PARAM_GioiTinh, SqlDbType.Int),
                            new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                            new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                            new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                            new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                            new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                            new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                            new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                            new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                            new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                            new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                            new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                            new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                            new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                            new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                            new SqlParameter("VaiTro",SqlDbType.NVarChar),
                            new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                            new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                            new SqlParameter("NgayKetThucLam", SqlDbType.DateTime),
                            new SqlParameter("TuLayAnh", SqlDbType.Bit),
                            new SqlParameter("@MatKhau", SqlDbType.NVarChar),


              };
            parameters[0].Value = HeThongCanBoModel.CanBoID;
            parameters[1].Value = HeThongCanBoModel.TenCanBo ?? Convert.DBNull;
            parameters[2].Value = HeThongCanBoModel.NgaySinh == null ? Convert.DBNull : HeThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
            parameters[3].Value = HeThongCanBoModel.GioiTinh ?? Convert.DBNull;
            parameters[4].Value = HeThongCanBoModel.DiaChi ?? Convert.DBNull;
            parameters[5].Value = HeThongCanBoModel.ChucVuID ?? Convert.DBNull;
            parameters[6].Value = HeThongCanBoModel.QuyenKy ?? Convert.DBNull;
            parameters[7].Value = HeThongCanBoModel.Email ?? Convert.DBNull;
            parameters[8].Value = HeThongCanBoModel.DienThoai ?? Convert.DBNull;
            parameters[9].Value = HeThongCanBoModel.PhongBanID ?? Convert.DBNull;
            parameters[10].Value = HeThongCanBoModel.CoQuanID ?? Convert.DBNull;
            parameters[11].Value = HeThongCanBoModel.RoleID ?? Convert.DBNull;
            parameters[12].Value = HeThongCanBoModel.QuanTriDonVi ?? Convert.DBNull;
            parameters[13].Value = HeThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
            parameters[14].Value = HeThongCanBoModel.CanBoCuID ?? Convert.DBNull;
            parameters[15].Value = HeThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
            parameters[16].Value = HeThongCanBoModel.TrangThaiID ?? Convert.DBNull;
            parameters[17].Value = HeThongCanBoModel.AnhHoSo ?? Convert.DBNull;
            parameters[18].Value = HeThongCanBoModel.HoKhau ?? Convert.DBNull;
            parameters[19].Value = HeThongCanBoModel.MaCB;
            parameters[20].Value = HeThongCanBoModel.CapQuanLy ?? Convert.DBNull;
            parameters[21].Value = HeThongCanBoModel.VaiTro ?? Convert.DBNull;
            parameters[22].Value = HeThongCanBoModel.CMND ?? Convert.DBNull;
            parameters[23].Value = HeThongCanBoModel.NgayCap == null ? Convert.DBNull : HeThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
            parameters[24].Value = HeThongCanBoModel.NoiCap ?? Convert.DBNull;
            parameters[25].Value = HeThongCanBoModel.TrangThaiID == (int)EnumTrangThaiNhanVien.DaNghi ? DateTime.Now : Convert.DBNull;
            parameters[26].Value = HeThongCanBoModel.TuLayAnh ?? Convert.DBNull;
            //check validate Email/User
            if (HeThongCanBoModel.Email == null || string.IsNullOrEmpty(HeThongCanBoModel.Email))
            {
                Message = "Email/User không được để trống!";
                return val;
            }
            else
            {
                if (HeThongCanBoModel.Email.Contains("@") && HeThongCanBoModel.Email.Contains(".com"))
                {
                    if (!Utils.CheckEmail(HeThongCanBoModel.Email))
                    {
                        Message = "Email không đúng định dạng";
                        return val;
                    }
                }
                if (HeThongCanBoModel.Email.Trim() != CanBoOld.Email.Trim())
                {
                    HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.Email, CoQuanSuDungPhanMemID);
                    if (nguoiDung.NguoiDungID > 0)
                    {
                        Message = "Email/User đã đựoc sử dụng!";
                        return val;
                    }
                }
            }
            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
            parameters[27].Value = Cryptor.EncryptPasswordUser(HeThongCanBoModel.Email.Trim().ToLower(), matKhauMacDinh ?? "123456");
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string MessageNew = null;
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_Update_New", parameters);
                        trans.Commit();
                        if (val > 0 && !UserRole.CheckAdmin(NguoiDungID.Value))
                        {
                            var mess = "";
                            // xóa các chức vụ của  Cán bộ đã có
                            var qrDelete = CanBoChucVu_Delete_By_CanBoID(HeThongCanBoModel.CanBoID, ref mess);
                            if (qrDelete <= 0) { Message = string.IsNullOrEmpty(mess) ? null : mess; return qrDelete; }
                            // thêm lại cán bộ - chức vụ

                            var query = CanBoChucVu_Insert(HeThongCanBoModel.DanhSachChucVuID, HeThongCanBoModel.CanBoID, ref mess);
                            if (query <= 0) { Message = string.IsNullOrEmpty(mess) ? null : mess; return query; }

                            ////update lại tài khoản và mật khẩu nếu thay đổi email
                            //if (CanBoOld.Email != HeThongCanBoModel.Email)
                            //{
                            //    new HeThongNguoiDungDAL().UpdateTaiKhoanNguoiDung(CanBoOld.NguoiDungID ?? 0, HeThongCanBoModel.Email);
                            //}

                            //Xóa các ca làm việc của cán bộ


                        }
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                    Message = ConstantLogMessage.Alert_Update_Success("cán bộ");
                }
                //var crNguoiDung = new HeThongNguoiDungDAL().GetByCanBoID(HeThongCanBoModel.CanBoID);
                if (HeThongCanBoModel.TrangThaiID == 0) // Nghỉ việc
                {
                    new HeThongNguoiDungDAL().UpdateTrangThai(new List<int>() { HeThongCanBoModel.CanBoID }, (int)EnumTrangThaiNhanVien.DaNghi);
                }
                else if (HeThongCanBoModel.TrangThaiID == 1) // Đang làm
                {
                    new HeThongNguoiDungDAL().UpdateTrangThai(new List<int>() { HeThongCanBoModel.CanBoID }, (int)EnumTrangThaiNhanVien.DangLam);
                }
                return val;
            }
        }

        public int Update(HeThongCanBoModel HeThongCanBoModel, ref string Message, int? NguoiDungID)
        {
            var CanBoOld = GetCanBoByID(HeThongCanBoModel.CanBoID);
            var CoQuanSuDungPhanMemID = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID).CoQuanSuDungPhanMemID;
            int val = 0;
            if (HeThongCanBoModel.CanBoID == 0)
            {
                Message = "Chưa có cán bộ được chọn!";
                return val;
            }
            if (!Utils.CheckSpecialCharacter(HeThongCanBoModel.TenCanBo))
            {
                Message = ConstantLogMessage.API_Error_NotSpecialCharacter;
                return val;
            }
            if (CanBoOld == null || CanBoOld.CanBoID < 1)
            {
                Message = ConstantLogMessage.Alert_Error_NotExist("Cán bộ");
                return val;
            }
            if (!string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
            {
                HeThongNguoiDungModel nguoiDungTemp = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.TenNguoiDung.Trim().ToLower(), 0);
                if (nguoiDungTemp.NguoiDungID > 0 && nguoiDungTemp.NguoiDungID != HeThongCanBoModel.NguoiDungID)
                {
                    Message = "Tên người dùng đã được sử dụng!";
                    return val;
                }
            }
            
            if (!string.IsNullOrEmpty(HeThongCanBoModel.MaCB) && (HeThongCanBoModel.MaCB != CanBoOld.MaCB || string.IsNullOrEmpty(CanBoOld.MaCB)))
            {
                //var CanBoByMaCb = GetByMaCB(HeThongCanBoModel.MaCB.Trim());
                //if (CanBoByMaCb.CanBoID != HeThongCanBoModel.CanBoID && CanBoByMaCb.MaCB == HeThongCanBoModel.MaCB)
                //{
                //    Message = "Mã cán bộ đã tồn tại";
                //    return val;
                //}
            }
            else
            {
                HeThongCanBoModel.MaCB = CanBoOld.MaCB;
            }
            if (HeThongCanBoModel.CoQuanID != null)
            {
                var CoQuan = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                if (CoQuan == null)
                {
                    Message = ConstantLogMessage.Alert_Error_NotExist("công ty");
                    return val;
                }
                else
                {
                    var CoQuanByID = new DanhMucCoQuanDonViDAL().GetByID(HeThongCanBoModel.CoQuanID);
                }
            }
            //if (string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
            //{
            //    Message = "Tài khoản người dùng là bắt buộc!";
            //    return val;
            //}
            SqlParameter[] parameters = new SqlParameter[]
              {
                            new SqlParameter(PARAM_CanBoID, SqlDbType.Int),
                            new SqlParameter(PARAM_TenCanBo, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_NgaySinh, SqlDbType.DateTime),
                            new SqlParameter(PARAM_GioiTinh, SqlDbType.Int),
                            new SqlParameter(PARAM_DiaChi, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_ChucVuID, SqlDbType.Int),
                            new SqlParameter(PARAM_QuyenKy, SqlDbType.Int),
                            new SqlParameter(PARAM_Email, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_DienThoai, SqlDbType.NVarChar),
                            new SqlParameter(PARAM_PhongBanID, SqlDbType.Int),
                            new SqlParameter(PARAM_CoQuanID, SqlDbType.Int),
                            new SqlParameter(PARAM_RoleID, SqlDbType.Int),
                            new SqlParameter(PARAM_QuanTridonVi,  SqlDbType.Int),
                            //new SqlParameter(PARAM_CoQuanCuID, SqlDbType.Int),
                            //new SqlParameter(PARAM_CanBoCuID,  SqlDbType.Int),
                            new SqlParameter(PARAM_XemTaiLieuMat,  SqlDbType.Int),
                            new SqlParameter(PARAM_TrangThaiID,  SqlDbType.Int),
                            new SqlParameter("SSOID", SqlDbType.NVarChar),
                            new SqlParameter("ChuTichUBND",  SqlDbType.Int),

                            //new SqlParameter(PARAM_AnhHoSo,  SqlDbType.NVarChar),
                            //new SqlParameter(PARAM_HoKhau,  SqlDbType.NVarChar),
                            //new SqlParameter(PARAM_MaCB,  SqlDbType.NVarChar),
                            //new SqlParameter(PARAM_CapQuanLy,SqlDbType.Int),
                            //new SqlParameter("VaiTro",SqlDbType.NVarChar),
                            //new SqlParameter(PARAM_CMND, SqlDbType.NVarChar),
                            //new SqlParameter(PARAM_NgayCap, SqlDbType.DateTime),
                            //new SqlParameter(PARAM_NoiCap, SqlDbType.NVarChar),
                            //new SqlParameter("NgayKetThucLam", SqlDbType.DateTime),
                            //new SqlParameter("TuLayAnh", SqlDbType.Bit),
                            //new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                            //new SqlParameter("@TenNguoiDung", SqlDbType.VarChar),
                            //new SqlParameter("@TrangThaiTaiKhoan", SqlDbType.TinyInt),
                            //new SqlParameter("NgayBatDauLam", SqlDbType.DateTime),
              };
            parameters[0].Value = HeThongCanBoModel.CanBoID;
            parameters[1].Value = HeThongCanBoModel.TenCanBo ?? Convert.DBNull;
            parameters[2].Value = HeThongCanBoModel.NgaySinh == null ? Convert.DBNull : HeThongCanBoModel.NgaySinh.Value.ToString("yyyy/MM/dd");
            if (HeThongCanBoModel.NgaySinh != null && HeThongCanBoModel.NgaySinh.Value.Year == 1752)
            {
                parameters[2].Value = Convert.DBNull;
            }
            parameters[3].Value = HeThongCanBoModel.GioiTinh ?? Convert.DBNull;
            parameters[4].Value = HeThongCanBoModel.DiaChi ?? Convert.DBNull;
            parameters[5].Value = HeThongCanBoModel.ChucVuID ?? Convert.DBNull;
            parameters[6].Value = HeThongCanBoModel.QuyenKy ?? Convert.DBNull;
            parameters[7].Value = HeThongCanBoModel.Email ?? Convert.DBNull;
            parameters[8].Value = HeThongCanBoModel.DienThoai ?? Convert.DBNull;
            parameters[9].Value = HeThongCanBoModel.PhongBanID ?? Convert.DBNull;
            parameters[10].Value = HeThongCanBoModel.CoQuanID ?? 1;
            parameters[11].Value = HeThongCanBoModel.RoleID ?? Convert.DBNull;
            parameters[12].Value = HeThongCanBoModel.QuanTriDonVi ?? Convert.DBNull;
            //parameters[13].Value = HeThongCanBoModel.CoQuanCuID ?? Convert.DBNull;
            //parameters[14].Value = HeThongCanBoModel.CanBoCuID ?? Convert.DBNull;
            parameters[13].Value = HeThongCanBoModel.XemTaiLieuMat ?? Convert.DBNull;
            parameters[14].Value = HeThongCanBoModel.TrangThaiID ?? Convert.DBNull;
            parameters[15].Value = HeThongCanBoModel.SSOID ?? Convert.DBNull;
            parameters[16].Value = HeThongCanBoModel.ChuTichUBND ?? Convert.DBNull;
            //parameters[17].Value = HeThongCanBoModel.AnhHoSo ?? Convert.DBNull;
            //parameters[18].Value = HeThongCanBoModel.HoKhau ?? Convert.DBNull;
            //parameters[19].Value = HeThongCanBoModel.MaCB;
            //parameters[20].Value = HeThongCanBoModel.CapQuanLy ?? Convert.DBNull;
            //parameters[21].Value = HeThongCanBoModel.VaiTro ?? Convert.DBNull;
            //parameters[22].Value = HeThongCanBoModel.CMND ?? Convert.DBNull;
            //parameters[23].Value = HeThongCanBoModel.NgayCap == null ? Convert.DBNull : HeThongCanBoModel.NgayCap.Value.ToString("yyyy/MM/dd");
            //parameters[24].Value = HeThongCanBoModel.NoiCap ?? Convert.DBNull;
            //parameters[25].Value = HeThongCanBoModel.NgayKetThucLam == null ? Convert.DBNull : HeThongCanBoModel.NgayKetThucLam.Value.ToString("yyyy/MM/dd");
            //parameters[26].Value = HeThongCanBoModel.TuLayAnh ?? Convert.DBNull;

            var matKhauMacDinh = new SystemConfigDAL().GetByKey("MAT_KHAU_MAC_DINH").ConfigValue;
            //check validate Email/User
            //if (string.IsNullOrEmpty(HeThongCanBoModel.Email))
            //{ 
            //    if (!string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung) && HeThongCanBoModel.TenNguoiDung != CanBoOld.TenNguoiDung)
            //    {
            //        HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.TenNguoiDung, 0);
            //        if (nguoiDung.NguoiDungID > 0)
            //        {
            //            Message = "Tài khoản đã được sử dụng! Thử lại!";
            //            return val;
            //        }
            //        else
            //        {
            //            parameters[27].Value = Cryptor.EncryptPasswordUser("", matKhauMacDinh ?? "123456");
            //            parameters[28].Value = HeThongCanBoModel.TenNguoiDung.Trim().ToLower();
            //        }
            //    }
            //}
            //else
            //{
            //    if (HeThongCanBoModel.Email.Contains("@") && HeThongCanBoModel.Email.Contains(".com"))
            //    {
            //        if (!Utils.CheckEmail(HeThongCanBoModel.Email))
            //        {
            //            Message = "Email không đúng định dạng";
            //            return val;
            //        }
            //    }
            //    if (HeThongCanBoModel.Email.Trim() != CanBoOld.Email.Trim())
            //    {
            //        HeThongNguoiDungModel nguoiDung = new HeThongNguoiDungDAL().GetByName(HeThongCanBoModel.Email, CoQuanSuDungPhanMemID);
            //        if (nguoiDung.NguoiDungID > 0)
            //        {
            //            Message = "Email đã đựoc sử dụng!";
            //            return val;
            //        }
            //    }
            //}
            //parameters[27].Value = HeThongCanBoModel.TenNguoiDung == CanBoOld.TenNguoiDung ? CanBoOld.MatKhau : Cryptor.EncryptPasswordUser("", matKhauMacDinh ?? "123456");
            //parameters[28].Value = HeThongCanBoModel.Email.Trim().ToLower() ?? HeThongCanBoModel.TenNguoiDung.Trim().ToLower();
            //parameters[29].Value = HeThongCanBoModel.TrangThaiTaiKhoan == null ? 1 : HeThongCanBoModel.TrangThaiTaiKhoan;
            //parameters[30].Value = HeThongCanBoModel.NgayBatDauLam != null ? HeThongCanBoModel.NgayBatDauLam : DateTime.Now;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string MessageNew = null;
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_CanBo_Update_1508", parameters);

                        try
                        {
                            //tao nguoi neu chua co
                            if (HeThongCanBoModel.NguoiDungID == null || HeThongCanBoModel.NguoiDungID == 0)
                            {
                                HeThongNguoiDungModel HeThongNguoiDungModel = new HeThongNguoiDungModel();
                                HeThongNguoiDungModel.MatKhau = Utils.HashFile(Encoding.ASCII.GetBytes(matKhauMacDinh ?? "123456")).ToUpper();
                                SqlParameter[] paramrs = new SqlParameter[]
                                {
                                new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                                new SqlParameter("@MatKhau", SqlDbType.NVarChar),
                                new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                                new SqlParameter("@TrangThai", SqlDbType.Int),
                                new SqlParameter("@CanBoID", SqlDbType.Int),
                                new SqlParameter("@SSOID", SqlDbType.NVarChar),
                                };
                                paramrs[0].Value = HeThongCanBoModel.TenNguoiDung;
                                paramrs[1].Value = HeThongNguoiDungModel.MatKhau.Trim();
                                paramrs[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
                                paramrs[3].Value = 1;
                                paramrs[4].Value = HeThongCanBoModel.CanBoID;
                                paramrs[5].Value = HeThongCanBoModel.SSOID ?? Convert.DBNull;

                                HeThongCanBoModel.NguoiDungID = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_NguoiDung_Insert_New", paramrs), 0);
                                trans.Commit();
                            }
                            // bổ sung cho người dùng chỉnh sửa thông tin TenNguoiDung
                            else if(HeThongCanBoModel.NguoiDungID > 0 && !string.IsNullOrEmpty(HeThongCanBoModel.TenNguoiDung))
                            {
                                HeThongNguoiDungModel HeThongNguoiDungModel = new HeThongNguoiDungModel();
                                SqlParameter[] paramrs = new SqlParameter[]
                                {
                                    new SqlParameter("@NguoiDungID",SqlDbType.Int),
                                    new SqlParameter("@TenNguoiDung", SqlDbType.NVarChar),
                                    new SqlParameter("@GhiChu", SqlDbType.NVarChar),
                                    new SqlParameter("@TrangThai", SqlDbType.Int),
                                    new SqlParameter("@CanBoID", SqlDbType.Int),
                                    new SqlParameter("@SSOID", SqlDbType.NVarChar),
                                };
                                paramrs[0].Value = HeThongCanBoModel.NguoiDungID;
                                paramrs[1].Value = HeThongCanBoModel.TenNguoiDung;
                                paramrs[2].Value = HeThongNguoiDungModel.GhiChu ?? Convert.DBNull;
                                paramrs[3].Value = 1;
                                paramrs[4].Value = HeThongCanBoModel.CanBoID;
                                paramrs[5].Value = HeThongCanBoModel.SSOID ?? Convert.DBNull;
                                SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"V2_HeThong_NguoiDung_Update", paramrs);
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                        trans.Commit();
                        //if (val > 0 && !UserRole.CheckAdmin(NguoiDungID.Value))
                        //{
                        //    var mess = "";
                        //    var qrDelete = CanBoChucVu_Delete_By_CanBoID(HeThongCanBoModel.CanBoID, ref mess);
                        //    if (qrDelete <= 0) { Message = string.IsNullOrEmpty(mess) ? null : mess; return qrDelete; }

                        //    var query = CanBoChucVu_Insert(HeThongCanBoModel.DanhSachChucVuID, HeThongCanBoModel.CanBoID, ref mess);
                        //    if (query <= 0) { Message = string.IsNullOrEmpty(mess) ? null : mess; return query; }

                        //}
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                    Message = ConstantLogMessage.Alert_Update_Success("cán bộ");


                }

                try
                {
                    //phan quyen
                    // xử lý lại chỗ này
                    //1. nếu không có nhóm người dùng (có 2 trường hợp sảy ra: từ đầu không có nhóm người dùng hoặc xóa tất cả nhóm người dùng đang có)
                    //xóa người dùng khỏi nhỏm trong bảng nguoidung_nhomnguoidung
                    //2. nếu có danh sách nhóm người dùng. cần xử lý xóa những nhóm không còn, thêm các nhóm mới cho người dùng
                    //2.1. thêm mới các nhóm mới. trong lúc thêm mới lấy ra danh sách nhóm người đùng đúng của người dùng (vì nhóm người dun
                    //2.2 xóa các nhóm không có trong danh sách
                    //--> thực hiện tất cả điều này trong hàm NguoiDung_NhomNguoidung_CapNhatNhomNguoiDungChoNguoiDung
                    var danhSachNhomCuaNguoiDung = new List<NguoiDungNhomNguoiDungModel>();
                    if (HeThongCanBoModel.DanhSachNhomNguoiDungID != null && HeThongCanBoModel.DanhSachNhomNguoiDungID.Count > 0)
                    {
                        foreach (var item in HeThongCanBoModel.DanhSachNhomNguoiDungID)
                        {
                            NhomNguoiDungModel nnd = new PhanQuyenDAL().NhomNguoiDung_GetByID(item);
                            if (nnd != null)
                            {
                                NhomNguoiDungModel nhomNguoiDungModel = new PhanQuyenDAL().NhomNguoiDung_GetByCoQuanIDAndNhomTongID(HeThongCanBoModel.CoQuanID ?? 0, item);
                                NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                                nd_nnd.NguoiDungID = HeThongCanBoModel.NguoiDungID ?? 0;
                                if (nhomNguoiDungModel.NhomNguoiDungID != 0)
                                {
                                    nd_nnd.NhomNguoiDungID = nhomNguoiDungModel.NhomNguoiDungID;
                                }
                                else
                                {
                                    nd_nnd.NhomNguoiDungID = item;
                                }
                                danhSachNhomCuaNguoiDung.Add(nd_nnd);
                                //BaseResultModel baseResult = new PhanQuyenDAL().NguoiDung_NhomNguoiDung_InsertOne(nd_nnd);
                            }
                        }
                    }
                    else
                    {
                        NguoiDungNhomNguoiDungModel nd_nnd = new NguoiDungNhomNguoiDungModel();
                        nd_nnd.NguoiDungID = HeThongCanBoModel.NguoiDungID ?? 0;
                        nd_nnd.NhomNguoiDungID = 0;
                        danhSachNhomCuaNguoiDung.Add(nd_nnd);
                    }
                    var capNhatQuyen = new PhanQuyenDAL().NguoiDung_NhomNguoidung_CapNhatNhomNguoiDungChoNguoiDung(danhSachNhomCuaNguoiDung);
                }
                catch (Exception)
                {
                }

                //if (HeThongCanBoModel.TrangThaiID == 0) // Nghỉ việc
                //{
                //    new HeThongNguoiDungDAL().UpdateTrangThai(new List<int>() { HeThongCanBoModel.CanBoID }, (int)EnumTrangThaiNhanVien.DaNghi);
                //}
                //else if (HeThongCanBoModel.TrangThaiID == 1) // Đang làm
                //{
                //    new HeThongNguoiDungDAL().UpdateTrangThai(new List<int>() { HeThongCanBoModel.CanBoID }, (int)EnumTrangThaiNhanVien.DangLam);
                //}
                return val;
            }
        }

        // Delete
        public List<string> Delete(List<int> ListCanBoID, int? NguoIDungID)
        {

            List<string> dic = new List<string>();
            string message = "";
            if (ListCanBoID.Count <= 0)
            {
                message = ConstantLogMessage.API_Error_NotExist;
                dic.Add(message);
                return dic;
            }
            else
            {
                int val = 0;
                for (int i = 0; i < ListCanBoID.Count; i++)
                {
                    var CanBoCrr = GetCanBoByID(ListCanBoID[i]);
                    //if (CheckRef(ListCanBoID[i]))
                    //{
                    //    dic.Add("Cán bộ " + CanBoCrr.TenCanBo + " đang được sử dụng. Không thể xóa!");
                    //}
                    //else 
                    if (CanBoCrr == null)
                    {
                        message = "Cán bộ " + CanBoCrr.TenCanBo + " không tồn tại!";
                        dic.Add(message);
                    }
                    else if (CanBoCrr != null /*&& ((CanBoCrr.DuocSuperAdminTao && UserRole.CheckAdmin(NguoIDungID.Value)) || !CanBoCrr.DuocSuperAdminTao)*/)
                    {
                        SqlParameter[] parameters = new SqlParameter[]
                          {
                        new SqlParameter(@"CanBoID", SqlDbType.Int)

                          };
                        parameters[0].Value = ListCanBoID[i];
                        using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    val = Utils.ConvertToInt32(SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v2_HeThong_CanBo_Delete", parameters), 0);
                                    trans.Commit();
                                }
                                catch (Exception ex)
                                {
                                    dic.Add("Cán bộ " + CanBoCrr.TenCanBo + " đang được sử dụng!");
                                    trans.Rollback();
                                    //throw ex;
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    dic.Add("Không thể xoá! Nhân viên: " + CanBoCrr.TenCanBo + " là người quản trị của công ty");
                    //}
                }
                return dic;
            }

        }

        //public List<DanhMuKNTChucVuModel> GetAll()

        // Get By id
        public HeThongCanBoModel GetCanBoByID(int? CanBoID)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_CanBo_GetByID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToNullableDateTime(dr["NgaySinh"], null);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        canBo.QuanTriDonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        canBo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canBo.TrangThaiID = canBo.State;
                        //canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        //canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);                   
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        canBo.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        canBo.SSOID = Utils.ConvertToString(dr["SSOID"], string.Empty);
                        canBo.TrangThaiTaiKhoan = Utils.ConvertToNullableInt32(dr["TrangThaiTaiKhoan"], null);
                        canBo.DanhSachNhomNguoiDungID = new PhanQuyenDAL().GetNhomNguoiDungByNguoiDungID(canBo.NguoiDungID ?? 0);
                        canBo.ChuTichUBND = Utils.ConvertToInt32(dr["ChuTichUBND"], 0);
                        break;
                    }
                    dr.Close();

                }

            }
            catch
            {
                throw;
            }
            return canBo;
        }

        public HeThongCanBoModel GetByID(int? CanBoID)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetByID_New", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.IsStatus = Utils.ConvertToInt32(dr["IsStatus"], 0);
                        canBo.AnhHoSo = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        canBo.TuLayAnh = Utils.ConvertToBoolean(dr["TuLayAnh"], false);
                        canBo.LaCanBo = Utils.ConvertToBoolean(dr["LaCanBo"], false);
                        break;
                    }
                    dr.Close();
                }

            }
            catch
            {
                throw;
            }
            return canBo;
        }

        public HeThongCanBoModel GetByCanBoID(int? CanBoID)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_GetByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.AnhDaiDienID = Utils.ConvertToInt32(dr["AnhID"], 0);
                        canBo.AnhHoSo = Utils.ConvertToString(dr["FileUrl"], string.Empty);
                        break;
                    }
                    dr.Close();
                }

            }
            catch
            {
                throw;
            }
            return canBo;
        }


        /// <summary>
        ///  lấy thông tin tên cơ quan và cơ quan cha của cán bộ đang đăng nhập
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <param name="NguoiDungID"></param>
        /// <returns></returns>
        public ThongTinDonViModel HeThongCanBo_GetThongTinCoQuan(int CanBoID, int NguoiDungID)
        {
            ThongTinDonViModel canBo = new ThongTinDonViModel();
            if (UserRole.CheckAdmin(NguoiDungID))
            {
                canBo.TenCoQuan = string.Empty;
                canBo.TenCoQuanCha = string.Empty;
                return canBo;
            }


            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThongCanBo_GetThongTinCoQuan_GetBy_CanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new ThongTinDonViModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.CoQuanChaID = Utils.ConvertToInt32(dr["CoQuanChaID"], 0);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.TenCoQuanCha = Utils.ConvertToString(dr["TenCoQuanCha"], string.Empty);
                        break;
                    }
                    dr.Close();

                }
            }
            catch
            {
                throw;
            }
            return canBo;
        }
        //GetAll
        public List<HeThongCanBoModel> GetAll()
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAll"))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        // canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        //canBo.QuanTridonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        //canBo.CoQuanCuID = Utils.ConvertToInt32(dr["CoQuanCuID"], 0);
                        //canBo.CanBoCuID = Utils.ConvertToInt32(dr["CanBoCuID"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);


                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        public List<HeThongCanBoModel> GetListCanBoThuocNhomAdminCongTy(int CoQuanSuDungPhanMemID)
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@CoQuanSuDungPhanMemID",SqlDbType.Int)
             };
            parameters[0].Value = CoQuanSuDungPhanMemID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_GetByNhomAdmin", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        //canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        //canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        //canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        //canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        //canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        //canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        //canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        //canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        //canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        //canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        //canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        //canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        //canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);


                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        public List<HeThongCanBoModel> GetListCanBoCoChucNangDuyetDonTu(int CoQuanSuDungPhanMemID)
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@CoQuanSuDungPhanMemID",SqlDbType.Int)
             };
            parameters[0].Value = CoQuanSuDungPhanMemID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_GetListCanBoCoChucNangDuyetDonTu", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        // Get All without nguoidungid
        public List<HeThongCanBoModel> GetAllCanBoWithoutNguoiDung()
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_GetAllCanBoWithoutNguoiDung"))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        canBo.QuanTriDonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        canBo.CoQuanCuID = Utils.ConvertToInt32(dr["CoQuanCuID"], 0);
                        canBo.CanBoCuID = Utils.ConvertToInt32(dr["CanBoCuID"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        //canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        //canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        //canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        //canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        //canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        //canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        //canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }
        // Get By Name
        public HeThongCanBoModel GetByMaCB(string MaCB)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter(@"MaCB",SqlDbType.NVarChar)
              };
            parameters[0].Value = MaCB ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetByMaCB", parameters))
                {
                    while (dr.Read())
                    {
                        // canBo = new HeThongCanBoModel(Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["TenCanBo"], string.Empty), Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now), Utils.ConvertToInt32(dr["GioiTinh"], 0), Utils.ConvertToString(dr["DiaChi"], string.Empty), Utils.ConvertToInt32(dr["ChucVuID"], 0), Utils.ConvertToInt32(dr["QuyenKy"], 0), Utils.ConvertToString(dr["Email"], string.Empty), Utils.ConvertToString(dr["DienThoai"], string.Empty), Utils.ConvertToInt32(dr["PhongBanID"], 0), Utils.ConvertToInt32(dr["CoQuanID"], 0), Utils.ConvertToInt32(dr["RoleID"], 0), Utils.ConvertToInt32(dr["QuanTridonVi"], 0), Utils.ConvertToInt32(dr["CoQuanCuID"], 0), Utils.ConvertToInt32(dr["CanBoCuID"], 0), Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0), Utils.ConvertToString(dr["AnhHoSo"], string.Empty),
                        //   Utils.ConvertToString(dr["HoKhau"], string.Empty), Utils.ConvertToString(dr["MaCB"], string.Empty), Utils.ConvertToInt32(dr["CapQuanLy"], 0), Utils.ConvertToInt32(dr["TrangThaiID"], 0), Utils.ConvertToInt32(dr["NguoiDungID"], 0));
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        break;

                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return canBo;
        }
        // Filter By Name
        //public List<HeThongCanBoModel> FilterByName(string TenCanBo, int IsStatus, int CoQuanID)
        //{
        //    List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
        //  SqlParameter[] parameters = new SqlParameter[]
        //    {
        //        new SqlParameter(PARAM_TenCanBo,SqlDbType.NVarChar),
        //        new SqlParameter(PARAM_CoQuanID,SqlDbType.Int),
        //        new SqlParameter(PARAM_IsStatus,SqlDbType.Int)
        //};
        //    parameters[0].Value = TenCanBo;
        //    parameters[1].Value = CoQuanID == 0 ? (int?)null : CoQuanID;
        //    parameters[2].Value = IsStatus == 0 ? (int?)null : IsStatus;
        //    try
        //    {

        //        using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, FILTERBYNAME, parameters))
        //        {
        //            while (dr.Read())
        //            {
        //                HeThongCanBoModel canBo = new HeThongCanBoModel(Utils.ConvertToInt32(dr["CanBoID"], 0), Utils.ConvertToString(dr["TenCanBo"], string.Empty), Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now), Utils.ConvertToInt32(dr["GioiTinh"], 0), Utils.ConvertToString(dr["DiaChi"], string.Empty), Utils.ConvertToInt32(dr["ChucVuID"], 0), Utils.ConvertToInt32(dr["QuyenKy"], 0), Utils.ConvertToString(dr["Email"], string.Empty), Utils.ConvertToString(dr["DienThoai"], string.Empty), Utils.ConvertToInt32(dr["PhongBanID"], 0), Utils.ConvertToInt32(dr["CoQuanID"], 0), Utils.ConvertToInt32(dr["RoleID"], 0), Utils.ConvertToInt32(dr["QuanTridonVi"], 0), Utils.ConvertToInt32(dr["CoQuanCuID"], 0), Utils.ConvertToInt32(dr["CanBoCuID"], 0), Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0), Utils.ConvertToInt32(dr["IsStatus"], 0));

        //                list.Add(canBo);
        //            }
        //            dr.Close();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    return list;
        //}

        // Get list Paging
        public List<HeThongCanBoModel> GetPagingBySearch(BasePagingParams p, ref int TotalRow, int? CoQuanID, int? TrangThaiID, int CoQuan_ID, int NguoiDungID, string host)
        {
            var pList = new SqlParameter("@DanhSachCoQuan", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            var tbCoQuanID = new DataTable();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            var crCoQuan = new DanhMucCoQuanDonViDAL().GetByID(CoQuan_ID);
            List<DanhMucCoQuanDonViModel> listCoQuanCon = new List<DanhMucCoQuanDonViModel>();
            listCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapConByCapCoQuan_New(crCoQuan.CoQuanID).ToList();
            if (UserRole.CheckAdmin(NguoiDungID))
            {
                listCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapConByCapCoQuan(0).ToList();
            }

            listCoQuanCon.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@Keyword",SqlDbType.NVarChar),
                new SqlParameter("@OrderByName",SqlDbType.NVarChar),
                new SqlParameter("@OrderByOption",SqlDbType.NVarChar),
                new SqlParameter("@pLimit",SqlDbType.Int),
                new SqlParameter("@pOffset",SqlDbType.Int),
                new SqlParameter("@TotalRow",SqlDbType.Int),
                 new SqlParameter("@CoQuanID",SqlDbType.Int),
                  new SqlParameter("@TrangThaiID",SqlDbType.Int),
                  pList
              };
            parameters[0].Value = p.Keyword == null ? "" : p.Keyword.Trim();
            parameters[1].Value = p.OrderByName;
            parameters[2].Value = p.OrderByOption;
            parameters[3].Value = p.Limit;
            parameters[4].Value = p.Offset;
            parameters[5].Direction = ParameterDirection.Output;
            parameters[5].Size = 8;
            parameters[6].Value = CoQuanID ?? Convert.DBNull;
            parameters[7].Value = TrangThaiID ?? Convert.DBNull;
            parameters[8].Value = tbCoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v2_HeThong_CanBo_GetPagingBySearch_New", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToNullableDateTime(dr["NgaySinh"], null);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        //canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        //canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        //canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        canBo.State = Utils.ConvertToInt32(dr["State"], 0);
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        canBo.TenNguoiDung = Utils.ConvertToString(dr["TenNguoiDung"], string.Empty);
                        canBo.SSOID = Utils.ConvertToString(dr["SSOID"], string.Empty);
                        //canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        //canBo.CapCoQuanID = Utils.ConvertToInt32(dr["CapID"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        //canBo.DanhSachChucVuID = CanBoChucVu_GetBy_CanBoID(canBo.CanBoID).Select(x => x.ChucVuID).ToList();

                        list.Add(canBo);

                    }

                    dr.Close();
                }
                TotalRow = Utils.ConvertToInt32(parameters[5].Value, 0);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            list = list.OrderBy(x => x.CapCoQuanID).ToList();
            return list;
        }

        public int CheckNhanVienGhiNhanChamCongChua(int? CanBoID)
        {
            var Result = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                 {
                    new SqlParameter("CanBoID",SqlDbType.Int),
                 };
                parameters[0].Value = CanBoID;
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, "v1_ChamCong_CheckCanBoDaghiNhanChamCongChua", parameters))
                {
                    while (dr.Read())
                    {
                        Result = Utils.ConvertToInt32(dr["count"], 0);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        //Chek Reference
        public bool CheckRef(int CanBoID)
        {
            bool result = false;
            try
            {
                if (CheckNhanVienGhiNhanChamCongChua(CanBoID) > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                return true;
                throw ex;
            }
            return result;
        }


        // Convert sang chức vụ id by tên chức vụ
        public int ConvertCoQuanIDByName(string TenCoQuan)
        {
            return new DanhMucCoQuanDonViDAL().GetByName(TenCoQuan).CoQuanID;
        }


        public List<HeThongCanBoModel> GetCanBoByTrangThaiID(int? CanBoID)
        {
            var Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@TrangThaiID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetByTrangThaiID", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        canBo.QuanTriDonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        canBo.CoQuanCuID = Utils.ConvertToInt32(dr["CoQuanCuID"], 0);
                        canBo.CanBoCuID = Utils.ConvertToInt32(dr["CanBoCuID"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        Result.Add(canBo);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return Result;
        }

        /// <summary>
        /// lấy toàn bộ cán bộ thuộc danh sách cơ quan
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> GetAllByListCoQuanID(List<int> CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();
            var table = new DataTable();
            table.Columns.Add("CoQuanID", typeof(string));
            CoQuanID.ForEach(x => table.Rows.Add(x));

            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.id_list";
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = table;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInListCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        /// <summary>
        ///  lấy tất cả cán bộ trong cơ quan và cơ quan con
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> GetAllByCoQuanID(int CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();
            var DanhSachCoQuanID = new DanhMucCoQuanDonViDAL().GetAllCapCon(CoQuanID).Select(x => x.CoQuanID).ToList();

            var table = new DataTable();
            table.Columns.Add("CoQuanID", typeof(string));
            DanhSachCoQuanID.ForEach(x => table.Rows.Add(x));

            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.id_list";
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = table;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInListCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);

                        if (canBo.CanBoID != 1)
                            Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        /// <summary>
        /// lấy toàn bộ cán bộ trong 1 cơ quan 
        /// lấy cả danh sách chức vụ, tên chức vụ
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> GetAllInCoQuanID(int CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CoQuanID", SqlDbType.Int)
        };
            parameters[0].Value = CoQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        if (Utils.ConvertToInt32(dr["TrangThaiID"], 0) == EnumTrangThaiCanBo.DangLamViec.GetHashCode())
                        {
                            HeThongCanBoPartialModel canBo = new HeThongCanBoPartialModel();
                            canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                            canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                            canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                            canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                            canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                            canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                            canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                            Result.Add(canBo);

                        }
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public List<HeThongCanBoShortModel> GetThanNhanByCanBoID(int CanBoID)
        {
            List<HeThongCanBoShortModel> list = new List<HeThongCanBoShortModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CanBoID",SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetThanNhan_ByCanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoShortModel canBo = new HeThongCanBoShortModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.ThanNhanID = Utils.ConvertToInt32(dr["NV01001"], 0);
                        canBo.HoTenThanNhan = Crypt.DecryptString_Aes(Utils.ConvertToString(dr["NV01003"], string.Empty));
                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }


        /// <summary>
        /// Lấy chi tiết thông tin cán bộ cho chi tiết bản kê khai  
        /// </summary>
        /// <param name="CanBoID"></param>
        /// <returns></returns>
        public HeThongCanBoPartialModel GetChiTietCanBoByID(int CanBoID)
        {
            HeThongCanBoPartialModel canBo = new HeThongCanBoPartialModel();
            SqlParameter[] parameters = new SqlParameter[]
              {
                new SqlParameter("@CanBoID",SqlDbType.Int)
              };
            parameters[0].Value = CanBoID;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetChiTiet_ByID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoPartialModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.TenChucVu = Utils.ConvertToString(dr["TenChucVu"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        canBo.CMND = Utils.ConvertToString(dr["CMND"], string.Empty);
                        canBo.NgayCap = Utils.ConvertToNullableDateTime(dr["NgayCap"], null);
                        canBo.NoiCap = Utils.ConvertToString(dr["NoiCap"], string.Empty);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return canBo;
        }

        public List<HeThongCanBoModel> GetAllInCoQuanCha(int CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();

            SqlParameter[] parameters = new SqlParameter[]
             {
                new SqlParameter("@CoQuanID",SqlDbType.Int)
             };
            parameters[0].Value = CoQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInCoQuanCha", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.CMND = Utils.ConvertToString(dr["CMND"], string.Empty);
                        canBo.NgayCap = Utils.ConvertToNullableDateTime(dr["NgayCap"], null);
                        canBo.NoiCap = Utils.ConvertToString(dr["NoiCap"], string.Empty);

                        Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        /// <summary>
        /// lấy tất cả cán bộ thuộc cấp quản lý và là cán bộ của đơn vị hiện tại và các đơn vị con
        /// </summary>
        /// <param name="CapQuanLy"></param>
        /// <param name="CoQuanID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> GetAll_By_CapQuanLy_And_DonViID_And_DonViChaID(int? CapQuanLy, int? CoQuanID)
        {
            List<HeThongCanBoModel> Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CapQuanLy",SqlDbType.Int),
                new SqlParameter("@CoQuanID",SqlDbType.Int)
            };
            parameters[0].Value = CapQuanLy ?? Convert.DBNull;
            parameters[1].Value = CoQuanID ?? Convert.DBNull;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAll_By_CapQuanLy_And_DonViID_And_DonViChaID", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var canbo = Result.Where(x => x.CanBoID == 10).ToList().FirstOrDefault();
            return Result;
        }

        /// <summary>
        /// Lấy cán bộ theo cơ quan và chức năng 
        /// </summary>
        /// <param name="CoQuanID"></param>
        /// <param name="ChucNangID"></param>
        /// <returns></returns>
        public List<HeThongCanBoModel> GetCanBoByChucNang(int? CoQuanID, int? ChucNangID)
        {
            var Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CoQuanID", SqlDbType.Int),
                new SqlParameter("@ChucNangID", SqlDbType.Int)
            };
            parameters[0].Value = CoQuanID ?? 0;
            parameters[1].Value = ChucNangID ?? 0;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllByCoQuanAndChucNang", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        //canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        //var DanhSachChucVu = new HeThongCanBoDAL().CanBoChucVu_GetChucVuCuaCanBo(canBo.CanBoID);
                        //canBo.DanhSachChucVuID = DanhSachChucVu.Select(x => x.ChucVuID).ToList();
                        //canBo.DanhSachTenChucVu = DanhSachChucVu.Select(x => x.TenChucVu).ToList();
                        if (canBo.CanBoID != 1)
                            Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        #endregion

        #region HT_CanBo_Chuc_Vu

        public int CanBoChucVu_Insert(List<int> ListChucVu, int CanBoID, ref string Message)
        {
            int val = 0;
            var Result = 0;
            var table = new DataTable();
            table.Columns.Add("ID1", typeof(string));
            table.Columns.Add("ID2", typeof(string));
            if (ListChucVu != null)
            {
                foreach (var item in ListChucVu)
                {
                    var nrow = table.NewRow();
                    nrow["ID1"] = CanBoID;
                    nrow["ID2"] = item;
                    table.Rows.Add(nrow);
                }
            }
            var pList = new SqlParameter("@list_idCanBo_idChucVu", SqlDbType.Structured);
            pList.TypeName = "dbo.id_id_list";
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = table;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = Utils.ConvertToInt32(SQLHelper.ExecuteScalar(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_ChucVu_Insert", parameters), 0);
                        trans.Commit();
                        Result = 1;
                    }
                    catch (Exception ex)
                    {
                        Result = 0;
                        Message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                    //Message = ConstantLogMessage.Alert_Insert_SuKNTCess("Cán bộ - chức vụ");
                    return Result;
                }
            }

        }

        public List<int> CanBoChucVu_GetBy_CanBoID(int CanBoID)
        {
            List<int> Result = new List<int>();
            SqlParameter[] parameters = new SqlParameter[]
           {
                 new SqlParameter("@CanBoID", SqlDbType.Int)
           };
            parameters[0].Value = CanBoID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_ChucVu_GetBy_CanBoID", parameters))
                {
                    while (dr.Read())
                    {
                        Result.Add(Utils.ConvertToInt32(dr["ChucVuID"], 0));
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        public int CanBoChucVu_Delete_By_CanBoID(int CanBoID, ref string message)
        {
            message = "";
            var val = 0;
            var Result = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
                  new SqlParameter(@"CanBoID", SqlDbType.Int)
            };
            parameters[0].Value = CanBoID;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_ChucVu_Delete_ByCanBoID", parameters);
                        trans.Commit();
                        Result = 1;
                    }
                    catch (Exception ex)
                    {
                        Result = 0;
                        message = ex.Message;
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return Result;

        }

        public int GetCapQuanLyID(string TenCapQuanLy)
        {
            if (TenCapQuanLy == "Toàn tỉnh")
            {
                return 0;
            }
            else if (TenCapQuanLy == "Cấp tỉnh")
            {
                return 1;

            }

            else if (TenCapQuanLy == "Cấp huyện")
            {
                return 2;
            }

            return 0;
        }
        public List<CanBoChuVu> CanBoChucVu_GetAll()
        {
            List<CanBoChuVu> list = new List<CanBoChuVu>();

            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_ChucVu_GetAll"))
                {
                    while (dr.Read())
                    {
                        CanBoChuVu canBo = new CanBoChuVu();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.KeKhaiHangNam = Utils.ConvertToBoolean(dr["KeKhaiHangNam"], false);
                        canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        public int InsertCanBoChucVu(Dictionary<int, int> ListCanBoChucVu)
        {
            var val = 0;
            var pList = new SqlParameter("@list_idCanBo_idChucVu", SqlDbType.Structured);
            pList.TypeName = "dbo.id_id_list";
            var tbChucVuCanBo = new DataTable();
            tbChucVuCanBo.Columns.Add("ChucVuID");
            tbChucVuCanBo.Columns.Add("CanBoID");
            foreach (var item in ListCanBoChucVu)
            {
                var newrow = tbChucVuCanBo.NewRow();
                newrow["ChucVuID"] = item.Key;
                newrow["CanBoID"] = item.Value;
                tbChucVuCanBo.Rows.Add(newrow);
            }
            SqlParameter[] parameters = new SqlParameter[]
           {
           pList
           };
            parameters[0].Value = tbChucVuCanBo;
            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_ChucVu_Insert", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    return val;
                }
            }
        }

        // Get all cán bộ by coquanid
        public List<HeThongCanBoModel> GetAllCanBoByCoQuanID(int CoQuanID, int CoQuan_ID)
        {
            List<DanhMucCoQuanDonViModel> ListCoQuanCon = new List<DanhMucCoQuanDonViModel>();
            if (CoQuanID <= 0)
            {
                ListCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapCon(CoQuan_ID);
            }
            else
            {
                ListCoQuanCon = new DanhMucCoQuanDonViDAL().GetAllCapCon(CoQuanID);
            }
            List<int> list = new List<int>();
            ListCoQuanCon.ForEach(x => list.Add(x.CoQuanID));
            List<HeThongCanBoModel> datas = new HeThongCanBoDAL().GetAllCanBoWithoutNguoiDung().ToList();
            List<HeThongCanBoModel> ListCanBoByCoQuanID = datas.Where(x => list.Contains(x.CoQuanID.Value)).ToList();
            return ListCanBoByCoQuanID;
        }

        // Get All Cán bộ by CoQuanID and ChucVuID
        public List<HeThongCanBoModel> GetAllCanBoByChucVuIDAndCoQuanID(int? ChucVuID, int? CoQuanID)
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@ChucVuID",SqlDbType.Int),
            new SqlParameter("@CoQuanID",SqlDbType.Int)
            };
            parameters[0].Value = ChucVuID ?? Convert.DBNull;
            parameters[1].Value = CoQuanID ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v2_HeThong_CanBo_GetAllCanBoByChucVuAndCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);

                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);

                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThai"], 0);
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        //canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;

        }

        //Get Cán bộ by nguoidungid
        public HeThongCanBoModel GetCanBoByNguoiDungID(int? NguoiDungID)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@NguoiDungID",SqlDbType.Int)

            };
            parameters[0].Value = NguoiDungID ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetCanBoByNguoiDungID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        // canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        //canBo.QuanTridonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        //canBo.CoQuanCuID = Utils.ConvertToInt32(dr["CoQuanCuID"], 0);
                        //canBo.CanBoCuID = Utils.ConvertToInt32(dr["CanBoCuID"], 0);
                        canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return canBo;
        }

        // get cán bộ by chức vụ id
        public HeThongCanBoModel GetCanBoByChucVuID(int? ChucVuID)
        {
            HeThongCanBoModel canBo = new HeThongCanBoModel();
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@ChucVuID",SqlDbType.Int)

            };
            parameters[0].Value = ChucVuID ?? Convert.DBNull;
            try
            {

                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetCanBoByChucVuID", parameters))
                {
                    while (dr.Read())
                    {
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        //canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        //canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        //canBo.GioiTinh = Utils.ConvertToInt32(dr["GioiTinh"], 0);
                        //canBo.DiaChi = Utils.ConvertToString(dr["DiaChi"], string.Empty);
                        canBo.ChucVuID = Utils.ConvertToInt32(dr["ChucVuID"], 0);
                        //canBo.QuyenKy = Utils.ConvertToInt32(dr["QuyenKy"], 0);
                        //canBo.Email = Utils.ConvertToString(dr["Email"], string.Empty);
                        //canBo.DienThoai = Utils.ConvertToString(dr["DienThoai"], string.Empty);
                        //canBo.PhongBanID = Utils.ConvertToInt32(dr["PhongBanID"], 0);
                        //canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        // canBo.RoleID = Utils.ConvertToInt32(dr["RoleID"], 0);
                        //canBo.QuanTridonVi = Utils.ConvertToInt32(dr["QuanTridonVi"], 0);
                        //canBo.CoQuanCuID = Utils.ConvertToInt32(dr["CoQuanCuID"], 0);
                        //canBo.CanBoCuID = Utils.ConvertToInt32(dr["CanBoCuID"], 0);
                        //canBo.XemTaiLieuMat = Utils.ConvertToInt32(dr["XemTaiLieuMat"], 0);
                        //canBo.AnhHoSo = Utils.ConvertToString(dr["AnhHoSo"], string.Empty);
                        //canBo.HoKhau = Utils.ConvertToString(dr["HoKhau"], string.Empty);
                        //canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        //canBo.CapQuanLy = Utils.ConvertToInt32(dr["CapQuanLy"], 0);
                        //canBo.TrangThaiID = Utils.ConvertToInt32(dr["TrangThaiID"], 0);
                        //canBo.NguoiDungID = Utils.ConvertToInt32(dr["NguoiDungID"], 0);
                        //canBo.VaiTro = Utils.ConvertToInt32(dr["VaiTro"], 0);
                        break;
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return canBo;

        }

        public List<HeThongCanBoModel> GetListByCoQuanID(int CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CoQuanID", SqlDbType.Int)
        };
            parameters[0].Value = CoQuanID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        if (Utils.ConvertToInt32(dr["TrangThaiID"], 0) == EnumTrangThaiCanBo.DangLamViec.GetHashCode())
                        {
                            HeThongCanBoPartialModel canBo = new HeThongCanBoPartialModel();
                            canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                            canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                            canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                            canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                            canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                            //var DanhSachChucVu = new HeThongCanBoDAL().CanBoChucVu_GetChucVuCuaCanBo(canBo.CanBoID);
                            //canBo.DanhSachChucVuID = DanhSachChucVu.Select(x => x.ChucVuID).ToList();
                            //canBo.DanhSachTenChucVu = DanhSachChucVu.Select(x => x.TenChucVu).ToList();
                            Result.Add(canBo);

                        }
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        // Lấy danh sách cán bộ đã nghỉ hưu hoặc nghỉ việc
        public int GetListCanBo_Expire(int CanBoID, int CoQuanID)
        {
            var pList = new SqlParameter("@ListCanBoID", SqlDbType.Structured);
            pList.TypeName = "dbo.list_ID";
            // var TrangThai = 400;
            var tbCanBoID = new DataTable();
            tbCanBoID.Columns.Add("ID", typeof(string));
            var crCoQuan = new DanhMucCoQuanDonViDAL().GetByID(CoQuanID);
            var CapQuanLy = 2;
            var CoQuanQuanLy = 0;
            if (UserRole.CheckAdmin(CanBoID))
            {
                // TrangThai = 200;
                CapQuanLy = 0;
                CoQuanQuanLy = 0;
            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapTrungUong.GetHashCode())         // cấp trung ương
            {

            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapTinh.GetHashCode())    // cấp tỉnh   
            {
                //  TrangThai = 200;
                CapQuanLy = EnumCapQuanLyCanBo.CapTinh.GetHashCode();
                CoQuanQuanLy = 0;
            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapSo.GetHashCode())    // cấp sở   
            {
                // TrangThai = 300;
                CapQuanLy = EnumCapQuanLyCanBo.CapTinh.GetHashCode();
                CoQuanQuanLy = 0;
            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapHuyen.GetHashCode())    // cấp huyện   
            {
                // TrangThai = 200;
                CapQuanLy = EnumCapQuanLyCanBo.CapHuyen.GetHashCode();
                CoQuanQuanLy = crCoQuan.CoQuanID;
            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapPhong.GetHashCode())    // cấp phòng  
            {

                //TrangThai = 300;
                CapQuanLy = EnumCapQuanLyCanBo.CapHuyen.GetHashCode();
                CoQuanQuanLy = crCoQuan.CoQuanChaID.Value;
            }
            else if (crCoQuan.CapID == EnumCapCoQuan.CapXa.GetHashCode())      // cấp xã
            {
                //  TrangThai = 200;
                CapQuanLy = EnumCapQuanLyCanBo.CapHuyen.GetHashCode();
                CoQuanQuanLy = crCoQuan.CoQuanID;

            }
            int val = 0;
            var listCanBoAll = new HeThongCanBoDAL().GetAll_By_CapQuanLy_And_DonViID_And_DonViChaID(CapQuanLy, CoQuanQuanLy);
            var list = listCanBoAll.Where(x => x.TrangThaiID == 2).ToList();
            list.ForEach(x => tbCanBoID.Rows.Add(x.CanBoID));
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@TrangThai",SqlDbType.Int),
              pList

           };
            parameters[0].Value = 0;
            parameters[1].Value = tbCanBoID;

            using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        val = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HeThongNguoiDung_UpdateTrangThai", parameters);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw;
                    }
                    return val;
                }
            }
        }

        public List<HeThongCanBoModel> GetListByCaLamViecID(int CaLamViecID)
        {
            var Result = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CaLamViecID", SqlDbType.Int)
             };
            parameters[0].Value = CaLamViecID;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThongCanBo_GetByCaLamViecID", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoPartialModel canBo = new HeThongCanBoPartialModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);
                        Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public List<HeThongCanBoModel> GetDanhSachCanBoByListCoQuanID(List<int> CoQuanID)
        {
            var Result = new List<HeThongCanBoModel>();
            var table = new DataTable();
            table.Columns.Add("CoQuanID", typeof(string));
            CoQuanID.ForEach(x => table.Rows.Add(x));

            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.id_list";
            SqlParameter[] parameters = new SqlParameter[]
            {
                pList
            };
            parameters[0].Value = table;

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllInListCoQuan", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);
                        canBo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        canBo.MaCB = Utils.ConvertToString(dr["MaCB"], string.Empty);
                        canBo.NgaySinh = Utils.ConvertToDateTime(dr["NgaySinh"], DateTime.Now);
                        canBo.TenCoQuan = Utils.ConvertToString(dr["TenCoQuan"], string.Empty);

                        Result.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public List<HeThongCanBoModel> GetListCanBoPhuHopVoiThoiGianCaLamviec(TimeSpan? ThoiGianBatDau, TimeSpan? ThoiGianKetThuc, int CoQuanID, int? CoQuanID_Filter)
        {
            List<HeThongCanBoModel> Result = new List<HeThongCanBoModel>();
            List<HeThongCanBoModel> DanhSachCanBoAll = new List<HeThongCanBoModel>();
            var crrCoQuan = new DanhMucCoQuanDonViDAL().GetByID(CoQuanID);
            List<int> DanhSachCoQuanID = new List<int>();
            List<DanhMucCoQuanDonViModel> DanhSachCoQuan = new List<DanhMucCoQuanDonViModel>();
            if (CoQuanID_Filter == null)
            {
                DanhSachCoQuan = new DanhMucCoQuanDonViDAL().GetAllCapConByCoQuanSuDungPhanMemID(crrCoQuan.CoQuanSuDungPhanMemID);
            }
            else
            {
                DanhSachCoQuan = new DanhMucCoQuanDonViDAL().GetAllCoQuanConDaCap(CoQuanID_Filter);
            }
            DanhSachCoQuan.ForEach(x => DanhSachCoQuanID.Add(x.CoQuanID));
            DanhSachCanBoAll = GetDanhSachCanBoByListCoQuanID(DanhSachCoQuanID);
            bool result = false;
            //foreach (var canBo in DanhSachCanBoAll)
            //{
            //    if (canBo.DanhSachCaLamViec != null && canBo.DanhSachCaLamViec.Count == 0)
            //    {
            //        result = true;
            //    }
            //    else
            //    {
            //        foreach (var CaLamViec in canBo.DanhSachCaLamViec)
            //        {
            //            if (CaLamViec.ThoiGianKetThuc < CaLamViec.ThoiGianBatDau)
            //            {
            //                if ((ThoiGianBatDau <= CaLamViec.ThoiGianBatDau && ThoiGianKetThuc <= CaLamViec.ThoiGianBatDau && ThoiGianBatDau > CaLamViec.ThoiGianKetThuc && ThoiGianKetThuc > ThoiGianBatDau)
            //                    || (ThoiGianBatDau >= CaLamViec.ThoiGianKetThuc && ThoiGianKetThuc <= CaLamViec.ThoiGianBatDau && ThoiGianKetThuc > ThoiGianBatDau)
            //                    )
            //                {
            //                    result = true;
            //                }
            //                else
            //                {
            //                    result = false;
            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                if ((ThoiGianBatDau <= CaLamViec.ThoiGianBatDau && ThoiGianKetThuc <= CaLamViec.ThoiGianBatDau)
            //                    || (ThoiGianBatDau >= CaLamViec.ThoiGianKetThuc && ThoiGianKetThuc <= CaLamViec.ThoiGianBatDau)
            //                    || (ThoiGianBatDau >= CaLamViec.ThoiGianKetThuc && ThoiGianKetThuc >= CaLamViec.ThoiGianKetThuc && ThoiGianKetThuc > ThoiGianBatDau)
            //                    )
            //                {
            //                    result = true;
            //                }
            //                else
            //                {
            //                    result = false;
            //                    break;
            //                }
            //            }
            //        }
            //    }

            //    if (result == true) Result.Add(canBo);
            //}
            return Result;
        }

        public List<HeThongCanBoModel> GetAllForLichSuChamCong(int? CoQuanDangNhapID, int? NguoiDungID)
        {
            List<HeThongCanBoModel> list = new List<HeThongCanBoModel>();
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@CoQuanDangNhapID", SqlDbType.Int)
            };
            parameters[0].Value = UserRole.CheckAdmin(NguoiDungID != null ? NguoiDungID.Value : 0) ? 0 : (CoQuanDangNhapID ?? Convert.DBNull);

            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HeThong_CanBo_GetAllForLichSuChamCong", parameters))
                {
                    while (dr.Read())
                    {
                        HeThongCanBoModel canBo = new HeThongCanBoModel();
                        canBo.CanBoID = Utils.ConvertToInt32(dr["CanBoID"], 0);
                        canBo.TenCanBo = Utils.ConvertToString(dr["TenCanBo"], string.Empty);

                        list.Add(canBo);
                    }
                    dr.Close();
                }
            }
            catch
            {
                throw;
            }
            return list;
        }

        public bool CheckCanBoCoAnhDaiDien(int CanBoID)
        {
            var result = false;
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@CanBoID", SqlDbType.Int)
           };
            parameters[0].Value = CanBoID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_CheckCoAnhDaiDien", parameters))
                {
                    if (dr.Read())
                    {
                        result = Utils.ConvertToInt32(dr["Count"], 0) > 0 ? true : false;
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                result = false;
                throw ex;
            }
            return result;
        }

        public int InsertAnhDaiDienFromAnhTraningModel(string UrlAnh)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@FileUrl", SqlDbType.NVarChar)
            };
            parameters[0].Value = UrlAnh.Trim();
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            result = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_NV_AnhNhanVien_InsertAnhDaiDienFromAnhTraningModel", parameters);
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public double TongSoPhepTheoThangByCanBoID(int CanBoID, int Thang)
        {
            var result = 0.0;
            SqlParameter[] parameters = new SqlParameter[]
           {
                  new SqlParameter(@"CanBoID", SqlDbType.Int),
                  new SqlParameter(@"Thang", SqlDbType.Int),
           };
            parameters[0].Value = CanBoID;
            parameters[1].Value = Thang;
            using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_TongPhep_GetByCanboID_Thang", parameters))
            {
                if (dr.Read())
                {
                    result = Utils.ConvertToIntDouble(dr["TongPhep"], 0);
                }
                dr.Close();
            }
            return result;
        }

        public int UpdateSoNgayPhepTheoThangCuaCanBo(double TongPhep, int CanBoID, int Thang)
        {
            int result = 0;
            SqlParameter[] parameters = new SqlParameter[]
            {
               new SqlParameter("@Thang", SqlDbType.Int),
               new SqlParameter("@TongPhep", SqlDbType.Float),
               new SqlParameter("@CanBoID", SqlDbType.Int),
            };
            parameters[0].Value = Thang;
            parameters[1].Value = TongPhep;
            parameters[2].Value = CanBoID;
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            result = SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HT_CanBo_TongPhep_UpdateTongPhep", parameters);
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public void CongPhepHangThang()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(SQLHelper.appConnectionStrings))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            SQLHelper.ExecuteNonQuery(trans, System.Data.CommandType.StoredProcedure, @"v1_HT_NhanVien_TongPhep_CongPhep", null);
                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #endregion

}
