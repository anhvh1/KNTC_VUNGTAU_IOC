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
    public class BaoCaoVuViecDongNguoi_DAL
    {
        private const string BC_THONGKEVUVIECDONGNGUOI_GETBYDATE = "BC_ThongKeVuViecDongNguoi_GetByDate";
        private const string BC_THONGKEVUVIECDONGNGUOI_GETBYDATE_NEW = "BC_ThongKeVuViecDongNguoi_GetByDate_New";
        private const string BC_CHITIETTHONGKEVUVIECDONGNGUOI_GETBYDATE = "BC_ChiTietThongKeVuViecDongNguoi_GetByDate";
        private const string BC_CHITIETTHONGKEVUVIECDONGNGUOI_GETBYDATE_NEW = "BC_ChiTietThongKeVuViecDongNguoi_GetByDate_New";
        private const string PARM_TUNGAY = "@TuNgay";
        private const string PARM_DENNGAY = "@DenNgay";
        public IList<BaoCaoVuViecDongNguoiInfo> GET_DONTHUVUVIECDONGNGUOI(DateTime startDate, DateTime endDate)
        {
            IList<BaoCaoVuViecDongNguoiInfo> infoList = new List<BaoCaoVuViecDongNguoiInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BC_THONGKEVUVIECDONGNGUOI_GETBYDATE_NEW, parm))
                {
                    while (dr.Read())
                    {
                        BaoCaoVuViecDongNguoiInfo info = new BaoCaoVuViecDongNguoiInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.XLDKhieuKienDN = Utils.ConvertToInt32(dr["XLDKhieuKienDN"], 0);
                        info.DonNhieuNguoiDungTen = Utils.ConvertToInt32(dr["DonNhieuNguoiDungTen"], 0);
                        infoList.Add(info);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }

        public IList<ChiTietVuViecDongNguoiInfo> GETCHITIET_DONTHUVUVIECDONGNGUOI(DateTime startDate, DateTime endDate, int Start, int CanBoID, List<CoQuanInfo> List, int End, int Type)
        {
            IList<ChiTietVuViecDongNguoiInfo> infoList = new List<ChiTietVuViecDongNguoiInfo>();
            var pList = new SqlParameter("@ListCoQuanID", SqlDbType.Structured);
            pList.TypeName = "dbo.IntList";
            var tbCoQuanID = new DataTable();
            var tmp = List.Select(x => x.CoQuanID).ToList();
            tbCoQuanID.Columns.Add("CoQuanID", typeof(string));
            List.ForEach(x => tbCoQuanID.Rows.Add(x.CoQuanID));
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime),
                new SqlParameter("@Start", SqlDbType.Int),
                new SqlParameter("@End", SqlDbType.Int),
                new SqlParameter("@Type", SqlDbType.Int),
                pList
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            parm[2].Value = Start;
            parm[3].Value = End;
            parm[4].Value = Type;
            parm[5].Value = tbCoQuanID;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BC_CHITIETTHONGKEVUVIECDONGNGUOI_GETBYDATE_NEW, parm))
                {
                    while (dr.Read())
                    {
                        ChiTietVuViecDongNguoiInfo info = new ChiTietVuViecDongNguoiInfo();
                        info.CoQuanID = Utils.ConvertToInt32(dr["CoQuanID"], 0);
                        info.SoDon = Utils.ConvertToString(dr["SoDon"], string.Empty);
                        info.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        info.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        info.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        info.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        info.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        info.NgayNhapDonStr = Format.FormatDate(info.NgayNhapDon);
                        info.SoLuong = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        info.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        info.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        info.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        info.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        info.StateID = Utils.ConvertToInt32(dr["StateID"], 0);
                        info.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (info.HuongXuLyID == 0)
                        {
                            info.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (info.HuongXuLyID == (int)HuongGiaiQuyetEnum.DeXuatThuLy && info.StateID != 10)
                        {
                            info.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            info.KetQuaID_Str = "Đã giải quyết";
                        }
                        var xemTaiLieuMat = 1;
                        List<FileHoSoInfo> fileYKienXLAll = new List<FileHoSoInfo>();
                        List<FileHoSoInfo> fileBanHanhQDAll = new List<FileHoSoInfo>();
                        // huong giai quyet
                        List<FileHoSoInfo> fileYKienXL = new List<FileHoSoInfo>();
                        if (info.XuLyDonID > 0)
                        {
                            if (xemTaiLieuMat == 1)
                            {

                                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(info.XuLyDonID).ToList();
                            }
                            else
                            {
                                fileYKienXLAll = new FileHoSoDAL().GetFileYKienXuLyByXuLyDonID(info.XuLyDonID).Where(x => x.IsBaoMat != true || x.NguoiUp == CanBoID).ToList();
                            }

                            int step = 0;
                            fileYKienXL = fileYKienXLAll.Where(x => x.XuLyDonID == info.XuLyDonID).ToList();

                            for (int i = 0; i < fileYKienXL.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(fileYKienXL[i].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileYKienXL[i].TenFile))
                                    {
                                        string[] arrtenFile = fileYKienXL[i].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileYKienXL[i].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileYKienXL[i].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileYKienXL[i].FileURL = fileYKienXL[i].FileURL.Replace(" ", "%20");
                                }
                                step++;
                                if (fileYKienXL[i].IsBaoMat == false)
                                {
                                    string sec_false = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a href='" + fileYKienXL[i].FileURL + "' download>" + fileYKienXL[i].TenFile + "</a></li>";
                                    info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_false + "</ul>";
                                }
                                else
                                {
                                    string sec_true = "<li style='margin-bottom: 5px;'>" + step + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileYKienXL[i].FileURL + ">" + fileYKienXL[i].TenFile + "</a></li>";
                                    info.TenHuongGiaiQuyet = "<div style='margin-bottom: 5px;'><span>" + info.TenHuongGiaiQuyet + "</span></div>" + "<ul>" + sec_true + "</ul>";
                                }
                            }
                            if (string.IsNullOrEmpty(info.TenHuongGiaiQuyet))
                            {
                                info.TenHuongGiaiQuyet = "";
                            }
                            // kết quả
                            List<FileHoSoInfo> fileBanHanhQD = new List<FileHoSoInfo>();
                            fileBanHanhQD = fileBanHanhQDAll.Where(x => x.XuLyDonID == info.XuLyDonID).ToList();
                            int steps = 0;
                            for (int j = 0; j < fileBanHanhQD.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(fileBanHanhQD[j].FileURL))
                                {
                                    if (string.IsNullOrEmpty(fileBanHanhQD[j].TenFile))
                                    {
                                        string[] arrtenFile = fileBanHanhQD[j].FileURL.Split('/');
                                        if (arrtenFile.Length > 0)
                                        {
                                            string[] duoiFile = arrtenFile[arrtenFile.Length - 1].Split('_');
                                            if (duoiFile.Length > 0)
                                            {
                                                fileBanHanhQD[j].TenFile = duoiFile[duoiFile.Length - 1];
                                            }
                                            else
                                            {
                                                fileBanHanhQD[j].TenFile = arrtenFile[arrtenFile.Length - 1];
                                            }
                                        }
                                    }
                                    fileBanHanhQD[j].FileURL = fileBanHanhQD[j].FileURL.Replace(" ", "%20");
                                }
                                steps++;
                                if (fileBanHanhQD[j].IsBaoMat == false)
                                {
                                    string sec_false = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a href='" + fileBanHanhQD[j].FileURL + "' download>" + fileBanHanhQD[j].TenFile + "</a></li>";
                                    info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_false + "</ul>";
                                }
                                else
                                {
                                    string sec_true = "<li style='margin-bottom: 5px;'>" + steps + ". " + "<a style='color: #EE7600;' href=" + "/DownloadFileDonThuCT.aspx?url=" + fileBanHanhQD[j].FileURL + ">" + fileBanHanhQD[j].TenFile + "</a></li>";
                                    info.KetQuaID_Str = "<div style='margin-bottom: 5px;'><span>" + info.KetQuaID_Str + "</span></div>" + "<ul>" + sec_true + "</ul>";
                                }
                            }
                            if (string.IsNullOrEmpty(info.KetQuaID_Str))
                            {
                                info.KetQuaID_Str = "";
                            }
                            if (string.IsNullOrEmpty(info.SoDon))
                            {
                                info.SoDon = "";
                            }
                            //info.STT = Utils.ConvertToInt32(dr["RowC"], 0);
                            infoList.Add(info);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            //infoList = infoList.Where(x => x.STT >= Start && x.STT <= (Start + 10)).ToList();
            return infoList;
        }

        public IList<TKDonThuInfo> DB_GETCHITIET_DONTHUVUVIECDONGNGUOI(DateTime startDate, DateTime endDate)
        {
            IList<TKDonThuInfo> infoList = new List<TKDonThuInfo>();
            SqlParameter[] parm = new SqlParameter[] {
                new SqlParameter(PARM_TUNGAY, SqlDbType.DateTime),
                new SqlParameter(PARM_DENNGAY, SqlDbType.DateTime)
            };
            parm[0].Value = startDate;
            parm[1].Value = endDate;
            try
            {
                using (SqlDataReader dr = SQLHelper.ExecuteReader(SQLHelper.appConnectionStrings, CommandType.StoredProcedure, BC_CHITIETTHONGKEVUVIECDONGNGUOI_GETBYDATE, parm))
                {
                    while (dr.Read())
                    {
                        TKDonThuInfo dtInfo = new TKDonThuInfo();
                        dtInfo.DonThuID = Utils.ConvertToInt32(dr["DonThuID"], 0);
                        dtInfo.XuLyDonID = Utils.ConvertToInt32(dr["XuLyDonID"], 0);
                        dtInfo.CoQuanID = Utils.ConvertToInt32(dr["CoQuanXuLyID"], 0);
                        dtInfo.SoLuong = Utils.ConvertToInt32(dr["SoLuong"], 0);
                        dtInfo.GapLanhDao = Utils.ConvertToBoolean(dr["GapLanhDao"], false);
                        dtInfo.LoaiKhieuTo1ID = Utils.ConvertToInt32(dr["LoaiKhieuTo1ID"], 0);
                        dtInfo.LoaiKhieuTo2ID = Utils.ConvertToInt32(dr["LoaiKhieuTo2ID"], 0);
                        dtInfo.LoaiKhieuTo3ID = Utils.ConvertToInt32(dr["LoaiKhieuTo3ID"], 0);
                        dtInfo.LoaiKhieuToID = Utils.ConvertToInt32(dr["LoaiKhieuToID"], 0);
                        dtInfo.VuViecCu = Utils.ConvertToBoolean(dr["VuViecCu"], false);
                        dtInfo.SoDon = Utils.ConvertToString(dr["SoDon"], string.Empty);
                        dtInfo.NoiDungDon = Utils.ConvertToString(dr["NoiDungDon"], string.Empty);
                        dtInfo.NhomKNID = Utils.ConvertToInt32(dr["NhomKNID"], 0);
                        dtInfo.TenChuDon = Utils.ConvertToString(dr["HoTen"], string.Empty);
                        dtInfo.DiaChi = Utils.ConvertToString(dr["DiaChiCT"], string.Empty);
                        dtInfo.NgayNhapDon = Utils.ConvertToDateTime(dr["NgayNhapDon"], DateTime.MinValue);
                        dtInfo.NgayNhapDonStr = Format.FormatDate(dtInfo.NgayNhapDon);
                        dtInfo.TenLoaiKhieuTo = Utils.ConvertToString(dr["TenLoaiKhieuTo"], string.Empty);
                        dtInfo.KetQuaID = Utils.ConvertToInt32(dr["KetQuaID"], 0);
                        dtInfo.HuongXuLyID = Utils.ConvertToInt32(dr["HuongGiaiQuyetID"], 0);
                        dtInfo.TenHuongGiaiQuyet = Utils.ConvertToString(dr["TenHuongGiaiQuyet"], string.Empty);
                        if (dtInfo.HuongXuLyID == 0)
                        {
                            dtInfo.KetQuaID_Str = "Chưa giải quyết";
                        }
                        else if (dtInfo.KetQuaID == 0)
                        {
                            dtInfo.KetQuaID_Str = "Đang giải quyết";
                        }
                        else
                        {
                            dtInfo.KetQuaID_Str = "Đã giải quyết";
                        }
                        infoList.Add(dtInfo);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return infoList;
        }
    }
}
