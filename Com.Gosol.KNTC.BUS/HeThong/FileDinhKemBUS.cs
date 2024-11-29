using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Gosol.KNTC.Ultilities;

namespace Com.Gosol.KNTC.BUS.HeThong
{
    public class FileDinhKemBUS
    {
        private FileDinhKemDAL _FileDinhKemDAL;
        public FileDinhKemBUS()
        {
            this._FileDinhKemDAL = new FileDinhKemDAL();
        }
        public BaseResultModel Delete(List<FileDinhKemModel> ListFileDinhKem)
        {
            return _FileDinhKemDAL.Delete(ListFileDinhKem);
        }

        public FileDinhKemModel GetByID(int FileDinhKemID, int FileType)
        {
            return _FileDinhKemDAL.GetByID(FileDinhKemID, FileType);
        }

        public List<FileDinhKemModel> GetByNgiepVuID(int NghiepVuID, int FileType)
        {
            return _FileDinhKemDAL.GetByNgiepVuID(NghiepVuID, FileType);
        }

        public BaseResultModel Insert(FileDinhKemModel FileDinhKemModel)
        {
            BaseResultModel baseResultModel = new BaseResultModel();
            FileLogInfo infoFileLog = new FileLogInfo();
            infoFileLog.LoaiLog = (int)EnumLoaiLog.Them;
            infoFileLog.LoaiFile = FileDinhKemModel.FileType;
            if (FileDinhKemModel.IsBaoMat == true)
            {
                infoFileLog.IsBaoMat = true;
                infoFileLog.IsMaHoa = true;
            }
            else
            {
                infoFileLog.IsBaoMat = false;
                infoFileLog.IsMaHoa = false;
            }

            int FileID = 0;
            if (FileDinhKemModel.FileType == EnumLoaiFile.FileHoSo.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileKQGiaiQuyet.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileKQTiep.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileCQGiaiQuyet.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileHoSo(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileKQXL.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileYKienXuLy.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileTrinhDuThao.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileDuyetDuThao.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileBanHanhGiaiQuyet.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileXuLyDon(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileDTCPGQ.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileGiaoXacMinh(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileBanHanhQD.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileBanHanhQDGQ(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileGiaiQuyet.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileGiaiQuyet(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileDMBXM.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertDMBuocXacMinh(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileVBDonDoc.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileVBDonDoc(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileThiHanh.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileThiHanh(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileKetQuaTranhChap.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileTranhChap(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileGiaHanGiaiQuyet.GetHashCode())
            {
                FileID = _FileDinhKemDAL.InsertFileGiaHanGiaiQuyet(FileDinhKemModel);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileBieuMau.GetHashCode() || FileDinhKemModel.FileType == EnumLoaiFile.FileHuongDanSuDung.GetHashCode())
            {
                var Result = _FileDinhKemDAL.Insert(FileDinhKemModel);
                FileID = Utils.ConvertToInt32(Result.Data, 0);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileRutDon.GetHashCode()|| FileDinhKemModel.FileType == EnumLoaiFile.FileDBCXM.GetHashCode())
            {
                var Result = _FileDinhKemDAL.Insert(FileDinhKemModel);
                FileID = Utils.ConvertToInt32(Result.Data, 0);
            }
            else if (FileDinhKemModel.FileType == EnumLoaiFile.FileThumbnail.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileTrinhTuThuTuc.GetHashCode()
                || FileDinhKemModel.FileType == EnumLoaiFile.FileQuyTrinhNghiepVu.GetHashCode()
                //|| FileDinhKemModel.FileType == EnumLoaiFile.FileKQGiaiQuyet.GetHashCode()
                //|| FileDinhKemModel.FileType == EnumLoaiFile.FileKQTiep.GetHashCode()
                //|| FileDinhKemModel.FileType == EnumLoaiFile.FileCQGiaiQuyet.GetHashCode()
                )
            {
                var Result = _FileDinhKemDAL.Insert(FileDinhKemModel);
                FileID = Utils.ConvertToInt32(Result.Data, 0);
            }

            if (FileID > 0)
            {
                infoFileLog.FileID = FileID;
            }


            _FileDinhKemDAL.InsertFileLog(infoFileLog);
            baseResultModel.Status = 1;
            baseResultModel.Data = infoFileLog.FileID;

            return baseResultModel;
        }

        public BaseResultModel UpdateFileHoSo(List<int> ListFileDinhKemID, int XuLyDonID, int DonThuID)
        {
            return _FileDinhKemDAL.UpdateFileHoSo(ListFileDinhKemID, XuLyDonID, DonThuID, string.Empty);
        }
    }

}
