using AutoMapper;
using Com.Gosol.KNTC.DAL.KNTC;
using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.KNTC;
using Com.Gosol.KNTC.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.BUS.KNTC
{
    public class DongBoDuLieuBUS
    {
        public List<ApiGateway.objMapping.ObjMap> GetLoaiDanhMucAnhXa()
        {
            List<ApiGateway.objMapping.ObjMap> lsMap = new List<ApiGateway.objMapping.ObjMap>();
            lsMap.Add(ApiGateway.AddMap("Cơ quan hành chính trong Tỉnh", "CQ"));
            lsMap.Add(ApiGateway.AddMap("Danh mục đơn vị trực thuộc bộ ngành", "BN"));
            lsMap.Add(ApiGateway.AddMap("Danh mục loại khiếu tố", "KT"));
            lsMap.Add(ApiGateway.AddMap("Danh mục kết quả giải quyết", "KQ"));
            lsMap.Add(ApiGateway.AddMap("Danh mục nguồn đơn", "ND"));
            lsMap.Add(ApiGateway.AddMap("Danh mục địa giới h.chính Tỉnh", "DMT"));
            lsMap.Add(ApiGateway.AddMap("Danh mục địa giới h.chính Huyện", "DMH"));
            lsMap.Add(ApiGateway.AddMap("Danh mục địa giới h.chính Xã", "DMX"));
            lsMap.Add(ApiGateway.AddMap("Danh mục hướng xử lý", "GQ"));
            lsMap.Add(ApiGateway.AddMap("Danh mục dân tộc", "DT"));
            lsMap.Add(ApiGateway.AddMap("Danh mục quốc gia", "QG"));
            return lsMap;
        }
        public DuLieuDongBoModel GetBySearch(BasePagingParams p)
        {
            DuLieuDongBoModel DuLieuDongBo = new DuLieuDongBoModel();
            QueryFilterInfo queryFilter = new QueryFilterInfo();
            int _currentPage = p.PageNumber;
            queryFilter.Start = (_currentPage - 1) * p.PageSize;
            queryFilter.End = _currentPage * p.PageSize;

            DuLieuDongBo.DanhSachDonThu = new DonThuDAL().GetDonThuChuaDongBoBySearch("", 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, -1, 0, Int16.MaxValue, 0).ToList();
            DuLieuDongBo.LichSuDongBo = new DonThuDAL().GetDonThuDaDongBoBySearch("", 0, p.TuNgay ?? DateTime.Now, p.DenNgay ?? DateTime.Now, -1, 0, Int16.MaxValue, 0).ToList();

            return DuLieuDongBo;
        }

        public BaseResultModel TaiDuLieuVaCallAPI(string TypeApi)
        {
            var Result = new BaseResultModel();
            try
            {
                //(new System.Threading.Tasks.Task(() => InitDataMapping(TypeApi))).Start();
                try
                {
                    //string TypeApi = ddlListDM.SelectedValue;
                    var lsCQ = new CoQuan().GetForMapping(TypeApi).ToList<ApiGateway.objMapping.ObjMapInfo>();
                    Result.Data = lsCQ;
                    Result.Status = 1;
                }
                catch (Exception ex)
                {
                    Result.Status = 0;
                    Result.Message = "Có lỗi xảy ra trong quá trình xử lý InitDataMapping Chi tiết:" + ex.Message;
                }

                InitDataApi(TypeApi);
                Result.Status = 1;
                Result.Message = "Quá trình tải dữ liệu thành công !";
            }
            catch (Exception ex)
            {
                Result.Status = 0;
                Result.Message = "Có lỗi xảy ra trong quá trình xử lý Chi tiết:" + ex.Message;
            }

            return Result;
        }

        public BaseResultModel DanhDauDongBo(DongBo_LogInfo LogInfo, int CanBoID)
        {
            var Result = new BaseResultModel();
            try
            {
                //DongBo_LogInfo dongBoInfo = new DongBo_LogInfo();
                //dongBoInfo.URl = txtDuongDan.Text;
                //dongBoInfo.Password = txtMatKhau.Text;
                //int loaiDongBo = (int)LoaiDongBo.TheoNgayGio;
                //if (rdoLoaiDongBoTheoNgay.Checked == true)
                //{
                //    loaiDongBo = (int)LoaiDongBo.TheoThu;
                //}
                //dongBoInfo.LoaiDongBo = loaiDongBo;
                //string ngayTrongTuan = "";
                //foreach (ListItem item in cbThu.Items)
                //{
                //    if (item.Selected)
                //    {
                //        ngayTrongTuan += item.Value + ";";
                //    }
                //}
                //dongBoInfo.NgayTrongTuan = ngayTrongTuan;
                //dongBoInfo.GioDongBo = txtGioPhut.Text;
                //var dongBoID = new DongBo_Log_Dal().DongBo_Update(LogInfo);
                var dongBoID = 1;// fix ID dong bo =1
                if (dongBoID > 0 && LogInfo.DanhSachDonThu != null)
                {
                    //Lay danh sach don thu da chon
                    List<int> selectedDonThuIDList = new List<int>();
                    foreach (DonThuInfo item in LogInfo.DanhSachDonThu)
                    {
                        //CheckBox cbxCheck = item.FindControl("cbxCheck") as CheckBox;

                        int xuLyDonID = item.XuLyDonID;

                        int trangThaiDon1 = new DongBo_Log_Dal().Count_XuLyDon_By_XuLyDonID(xuLyDonID);
                        int trangThaiDon2 = new DongBo_Log_Dal().Count_ThaoLuanGiaiQuyet_By_XuLyDonID(xuLyDonID);
                        int trangThaiDon3 = new DongBo_Log_Dal().Count_KetQua_By_XuLyDonID(xuLyDonID);
                        int trangThaiDon4 = new DongBo_Log_Dal().Count_ThiHanh_By_XuLyDonID(xuLyDonID);

                        int trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatVuViec;
                        if (trangThaiDon1 != 0)
                        {
                            trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatKetQuaXuLy;
                        }
                        if (trangThaiDon2 != 0)
                        {
                            trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatThongTinGiaiQuyet;
                        }
                        if (trangThaiDon3 != 0)
                        {
                            trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatKetQuaGiaiQuyet;
                        }
                        if (trangThaiDon4 != 0)
                        {
                            trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatThiHanhGiaiQuyet;
                        }

                        DongBo_LogInfo dongBo_LogInfo = new DongBo_LogInfo();
                        dongBo_LogInfo.XyLyDonID = xuLyDonID;
                        dongBo_LogInfo.TrangThaiDonThu = trangThaiDonThu;
                        dongBo_LogInfo.CanBoID = CanBoID;
                        dongBo_LogInfo.CreateDate = DateTime.Now;
                        dongBo_LogInfo.TenBuocDongBo = "";
                        dongBo_LogInfo.TrangThai = (int)TrangThaiDongBo.New;

                        new DongBo_Log_Dal().Insert(dongBo_LogInfo);
                    }
                }
                Result.Status = 1;
                Result.Message = "Dữ liệu đã được lưu trữ thành công";
            }
            catch
            {
                Result.Message = "Xảy ra lỗi trong quá trình lưu dữ liệu!,Xin vui lòng thử lại";
                Result.Status = 0;
            }

            return Result;
        }

        public BaseResultModel CapNhatDuLieuMapping(DuLieuMapping DuLieuMapping)
        {
            var Result = new BaseResultModel();
            try
            {
                //foreach (object obj in rptCoQuan.Items)
                //{
                //    TextBox txtCode = ((RepeaterItem)obj).FindControl("txtMappingCode") as TextBox;
                //    if (txtCode != null && !string.IsNullOrEmpty(txtCode.Text))
                //    {
                //        HiddenField hdfID = ((RepeaterItem)obj).FindControl("hdfMaID") as HiddenField;
                //        (new DongBo_Log_Dal()).DongBo_Log_UpdateForMapping(ddlListDM.SelectedValue, int.Parse(hdfID.Value), txtCode.Text);
                //        //new System.Threading.Tasks.Task(() => (new DongBo_Log_Dal()).DongBo_Log_UpdateForMapping(ddlListDM.SelectedValue, int.Parse(hdfID.Value), txtCode.Text)).Start();
                //    }
                //}

                (new System.Threading.Tasks.Task(() => InitDataMapping(DuLieuMapping.TypeApi))).Start();
                InitDataApi(DuLieuMapping.TypeApi);
                Result.Message = "Quá trình cập nhật thành công !";
                Result.Status = 1;
            }
            catch (Exception ex)
            {
                Result.Message = "Có lỗi xảy ra trong quá trình xử lý<br/> Chi tiết:" + ex.Message;
                Result.Status = 0;
            }
            return Result;
        }

        public BaseResultModel CapNhatTrangThaiDon(DongBo_LogInfo LogInfo)
        {
            var Result = new BaseResultModel();
            try
            {
                foreach (DonThuInfo item in LogInfo.DanhSachDonThu)
                {
                    int xuLyDonID = item.XuLyDonID;

                    int trangThaiDon1 = new DongBo_Log_Dal().Count_XuLyDon_By_XuLyDonID(xuLyDonID);
                    int trangThaiDon2 = new DongBo_Log_Dal().Count_ThaoLuanGiaiQuyet_By_XuLyDonID(xuLyDonID);
                    int trangThaiDon3 = new DongBo_Log_Dal().Count_KetQua_By_XuLyDonID(xuLyDonID);
                    int trangThaiDon4 = new DongBo_Log_Dal().Count_ThiHanh_By_XuLyDonID(xuLyDonID);

                    int trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatVuViec;
                    if (trangThaiDon1 != 0)
                    {
                        trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatKetQuaXuLy;
                    }
                    if (trangThaiDon2 != 0)
                    {
                        trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatThongTinGiaiQuyet;
                    }
                    if (trangThaiDon3 != 0)
                    {
                        trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatKetQuaGiaiQuyet;
                    }
                    if (trangThaiDon4 != 0)
                    {
                        trangThaiDonThu = (int)TrangThaiDonThuDongBo.CapNhatThiHanhGiaiQuyet;
                    }

                    DongBo_LogInfo dongBoInfo = new DongBo_Log_Dal().getTrangThaiDonThu_HienTai(xuLyDonID);

                    DongBo_LogInfo dongBo_LogInfo = new DongBo_LogInfo();
                    dongBo_LogInfo.XyLyDonID = xuLyDonID;
                    if (dongBoInfo.TrangThaiDonThu != trangThaiDonThu)
                    {
                        dongBo_LogInfo.TrangThai = 0;
                    }
                    else
                    {
                        dongBo_LogInfo.TrangThai = dongBoInfo.TrangThai;
                    }
                    dongBo_LogInfo.TrangThaiDonThu = trangThaiDonThu;
                    new DongBo_Log_Dal().DongBo_Log_UpdateTrangThaiDon(dongBo_LogInfo);
                }

                Result.Status = 1;
                Result.Message = "Cập nhật trạng thái đơn thành công";
            }
            catch (Exception)
            {
                Result.Message = "Xảy ra lỗi trong quá trình lưu dữ liệu!,Xin vui lòng thử lại";
                Result.Status = 0;
            }

            return Result;
        }

        private void InitDataMapping(string TypeApi)
        {
            try
            {
                //string TypeApi = ddlListDM.SelectedValue;
                var lsCQ = new CoQuan().GetForMapping(TypeApi).ToList<ApiGateway.objMapping.ObjMapInfo>();
                //ApiGateway.InitGrid(ref rptCoQuan, lsCQ);
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Có lỗi xảy ra trong quá trình xử lý InitDataMapping<br/> Chi tiết:" + ex.Message;
            }
        }

        private void InitDataApi(string TypeApi)
        {
            try
            {
                //string TypeApi = string.Empty;

                switch (TypeApi)
                {
                    case "CQ":
                        TypeApi = "2";
                        break;
                    case "BN":
                        TypeApi = "7";
                        break;
                    case "KT":
                        TypeApi = "6";
                        break;
                    case "QD":
                        TypeApi = "8";
                        break;
                    case "TQ":
                        TypeApi = "9";
                        break;
                    case "KQ":
                        TypeApi = "10";
                        break;
                    case "ND":
                        TypeApi = "5";
                        break;
                    case "DMT":
                    case "DMH":
                    case "DMX":
                        TypeApi = "1";
                        break;
                    case "DT":
                        TypeApi = "4";
                        break;
                    case "QG":
                        TypeApi = "3";
                        break;
                    case "GQ":
                        TypeApi = "13";
                        break;
                }
                List<ApiGateway.objMapping.ObjMap> lsApi;
                var objApi = ApiGateway.GovApi_TraDanhMuc<ApiGateway.objMapping.ObjMap>(TypeApi, ApiGateway.GovSyncConfig("DicApi"));
                if (objApi != null)
                {
                    lsApi = ((List<ApiGateway.objMapping.ObjMap>)objApi).OrderBy(x => x.Ten).ToList();

                }
                //ApiGateway.InitGrid(ref rptAPICQ, lsApi);
            }
            catch (Exception ex)
            {
                //lblMessage.Text = "Có lỗi xảy ra trong quá trình xử lý InitDataApi<br/> Chi tiết:" + ex.Message;
            }
        }
    }
}
