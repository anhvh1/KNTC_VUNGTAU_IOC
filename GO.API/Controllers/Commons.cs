using Com.Gosol.KNTC.BUS.HeThong;
using Com.Gosol.KNTC.DAL.HeThong;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using GroupDocs.Viewer;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using RestSharp;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Com.Gosol.KNTC.API.Controllers
{
    public class Commons
    {
        #region Constant
        //path FileUpload
        public static readonly String PATH_FILE_HOSO_ENCRYPT = "UploadFiles/filehoso/encrypt";
        public static readonly String PATH_FILE_HOSO_DECRYPT = "UploadFiles/filehoso/decrypt";
        public static readonly String PATH_FILE_HOSO = "UploadFiles/filehoso";

        public static readonly String PATH_FILE_KQXL_ENCRYPT = "UploadFiles/FileKetQuaXuLy/encrypt/";
        public static readonly String PATH_FILE_KQXL_DECRYPT = "UploadFiles/FileKetQuaXuLy/decrypt/";
        public static readonly String PATH_FILE_KQXL = "UploadFiles/FileKetQuaXuLy/";

        public static readonly String PATH_FILE_DTCPGQ_ENCRYPT = "UploadFiles/FileDonThuCanPhanGiaiQuyet/encrypt/";
        public static readonly String PATH_FILE_DTCPGQ_DECRYPT = "UploadFiles/FileDonThuCanPhanGiaiQuyet/decrypt/";
        public static readonly String PATH_FILE_DTCPGQ = "UploadFiles/FileDonThuCanPhanGiaiQuyet/";

        public static readonly String PATH_FILE_BHGQ_ENCRYPT = "UploadFiles/FileBanHanhQD/encrypt/";
        public static readonly String PATH_FILE_BHGQ_DECRYPT = "UploadFiles/FileBanHanhQD/decrypt/";
        public static readonly String PATH_FILE_BHGQ = "UploadFiles/FileBanHanhQD/";

        public static readonly String PATH_FILE_DonDoc_ENCRYPT = "UploadFiles/FileDonDoc/encrypt/";
        public static readonly String PATH_FILE_DonDoc_DECRYPT = "UploadFiles/FileDonDoc/decrypt/";
        public static readonly String PATH_FILE_DonDoc = "UploadFiles/FileDonDoc/";

        public static readonly String PATH_FILE_HSDS_ENCRYPT = "UploadFiles/FileHoSoDonStep/encrypt/";
        public static readonly String PATH_FILE_HSDS_DECRYPT = "UploadFiles/FileHoSoDonStep/decrypt/";
        public static readonly String PATH_FILE_HSDS = "UploadFiles/FileHoSoDonStep/";

        public static readonly String PATH_FILE_RUTDON_ENCRYPT = "UploadFiles/FileRutDon/encrypt/";
        public static readonly String PATH_FILE_RUTDON_DECRYPT = "UploadFiles/FileRutDon/decrypt/";
        public static readonly String PATH_FILE_RUTDON = "UploadFiles/FileRutDon/";

        public static readonly String PATH_FILE_PHANXULY_ENCRYPT = "UploadFiles/FilePhanXuLy/encrypt/";
        public static readonly String PATH_FILE_PHANXULY_DECRYPT = "UploadFiles/FilePhanXuLy/decrypt/";
        public static readonly String PATH_FILE_PHANXULY = "UploadFiles/FilePhanXuLy/";

        public static readonly String PATH_FILE_GIAIQUYET_ENCRYPT = "UploadFiles/GiaiQuyet/encrypt/";
        public static readonly String PATH_FILE_GIAIQUYET_DECRYPT = "UploadFiles/GiaiQuyet/decrypt/";
        public static readonly String PATH_FILE_GIAIQUYET = "UploadFiles/GiaiQuyet/";

        public static readonly String PATH_FILE_TRANHCHAP_ENCRYPT = "UploadFiles/TranhChap/encrypt/";
        public static readonly String PATH_FILE_TRANHCHAP_DECRYPT = "UploadFiles/TranhChap/decrypt/";
        public static readonly String PATH_FILE_TRANHCHAP = "UploadFiles/TranhChap/";

        public static readonly String PATH_FILE_DONTHUCANDUYETGIAIQUYET_ENCRYPT = "UploadFiles/FileDonThuCanDuyetGiaiQuyet/encrypt/";
        public static readonly String PATH_FILE_DONTHUCANDUYETGIAIQUYET_DECRYPT = "UploadFiles/FileDonThuCanDuyetGiaiQuyet/decrypt/";
        public static readonly String PATH_FILE_DONTHUCANDUYETGIAIQUYET = "UploadFiles/FileDonThuCanDuyetGiaiQuyet/";

        public static readonly String PATH_FILE_VBDONDOC_ENCRYPT = "UploadFiles/VBDonDoc/encrypt/";
        public static readonly String PATH_FILE_VBDONDOC_DECRYPT = "UploadFiles/VBDonDoc/decrypt/";
        public static readonly String PATH_FILE_VBDONDOC = "UploadFiles/VBDonDoc/";

        public static readonly String PATH_FILE_QLHS_ENCRYPT = "UploadFiles/QLHS/encrypt/";
        public static readonly String PATH_FILE_QLHS_DECRYPT = "UploadFiles/QLHS/decrypt/";
        public static readonly String PATH_FILE_QLHS = "UploadFiles/QLHS/";

        public static readonly String PATH_FILE_DMBXM_ENCRYPT = "UploadFiles/DMBuocXacMinh/encrypt";
        public static readonly String PATH_FILE_DMBXM_DECRYPT = "UploadFiles/DMBuocXacMinh/decrypt";
        public static readonly String PATH_FILE_DMBXM = "UploadFiles/DMBuocXacMinh";

        public static readonly String PATH_FILE_BIEUMAU_ENCRYPT = "UploadFiles/FileBieuMau/encrypt";
        public static readonly String PATH_FILE_BIEUMAU_DECRYPT = "UploadFiles/FileBieuMau/decrypt";
        public static readonly String PATH_FILE_BIEUMAU = "UploadFiles/FileBieuMau";

        public static readonly String PATH_FILE_HDSD_ENCRYPT = "UploadFiles/FileHuongDanSuDung/encrypt";
        public static readonly String PATH_FILE_HDSD_DECRYPT = "UploadFiles/FileHuongDanSuDung/decrypt";
        public static readonly String PATH_FILE_HDSD = "UploadFiles/FileHuongDanSuDung";

        public static readonly String PATH_FILE_DBCXM_ENCRYPT = "UploadFiles/FileDuyetBaoCaoXacMinh/encrypt";
        public static readonly String PATH_FILE_DBCXM_DECRYPT = "UploadFiles/FileDuyetBaoCaoXacMinh/decrypt";
        public static readonly String PATH_FILE_DBCXM = "UploadFiles/FileDuyetBaoCaoXacMinh";

        #endregion
        public string passwordForCompress_DecompressFile = "";
        public Commons(string password)
        {
            this.passwordForCompress_DecompressFile = password + "asdfghjkl";
        }

        public Commons()
        {

        }

        public string ConvertFileToBase64(string pathFile)
        {
            try
            {
                var at = System.IO.File.GetAttributes(pathFile);

                byte[] fileBit = System.IO.File.ReadAllBytes(pathFile);
                var file = System.IO.Path.Combine(pathFile);

                string AsBase64String = Convert.ToBase64String(fileBit);
                return AsBase64String;
            }
            catch (Exception ex)
            {
                return string.Empty;
                throw ex;
            }
        }

        //tạo file nén từ byte[] đã được mã hóa
        public void CompressAndSaveFile(byte[] byteArrOrigin, string fullPathFile)
        {
            if (byteArrOrigin == null)
                throw new ArgumentNullException("inputData must be non-null");
            using (MemoryStream originalFileStream = new MemoryStream(byteArrOrigin))
            using (FileStream compressedFileStream = File.Create(fullPathFile))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    originalFileStream.CopyTo(compressionStream);
                }
            }
        }

        public byte[] DecompressFile(string PathFileToDecompress)
        {
            byte[] byteArrDecompress;
            using (FileStream originalFileStream = new FileStream(PathFileToDecompress, FileMode.Open))
            {
                using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressFile = new MemoryStream())
                    {
                        decompressionStream.CopyTo(decompressFile);
                        byteArrDecompress = decompressFile.ToArray();
                    }
                }
            }
            return byteArrDecompress;
        }

        public void CompressAndSaveFileWithPassword(byte[] byteArrOrigin, string fullPathFile, string TenFileGoc)
        {
            try
            {
                using (FileStream fs = File.Create(fullPathFile))
                using (var outStream = new ZipOutputStream(fs))
                {
                    outStream.Password = passwordForCompress_DecompressFile;
                    outStream.SetLevel(9);
                    outStream.PutNextEntry(new ZipEntry(TenFileGoc));
                    using (MemoryStream StreamFileGoc = new MemoryStream(byteArrOrigin))
                    {
                        StreamFileGoc.CopyTo(outStream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExtractZipContentWithPassword(string FileZipPath)
        {
            try
            {
                string base64File = "";
                ICSharpCode.SharpZipLib.Zip.ZipFile file = null;
                FileStream fs = File.OpenRead(FileZipPath);
                file = new ICSharpCode.SharpZipLib.Zip.ZipFile(fs);
                file.Password = passwordForCompress_DecompressFile;
                byte[] fileDecompress;
                foreach (ZipEntry _zipEntry in file)
                {
                    Stream zipStream = file.GetInputStream(_zipEntry);
                    if (zipStream != null)
                    {
                        using (MemoryStream streamFile = new MemoryStream())
                        {
                            zipStream.CopyTo(streamFile);
                            fileDecompress = streamFile.ToArray();
                            base64File = Convert.ToBase64String(fileDecompress);
                        }
                        break;
                    }
                }
                return base64File;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FileDinhKemModel> InsertFileAsync(IFormFile source, FileDinhKemModel FileDinhKem, IHostingEnvironment _host)
        {
            try
            {
                FileDinhKemModel fileInfo = new FileDinhKemModel();
                var crCanBoID = FileDinhKem.NguoiCapNhat ?? 0;
                string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                string TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + TenFileGoc;

                fileInfo.NguoiCapNhat = crCanBoID;
                fileInfo.NgayCapNhat = FileDinhKem.NgayCapNhat ?? DateTime.Now;
                fileInfo.FileType = FileDinhKem.FileType;
                fileInfo.NghiepVuID = FileDinhKem.NghiepVuID;
                fileInfo.NoiDung = FileDinhKem.NoiDung;
                fileInfo.TenFile = FileDinhKem.TenFile;
                if (fileInfo.TenFile == null || fileInfo.TenFile == "")
                {
                    fileInfo.TenFile = TenFileHeThong;
                }
                fileInfo.DMTenFileID = FileDinhKem.DMTenFileID;

                string FolderPath = "UploadFiles";
                string FolderPathEncrypt = "UploadFiles";
                GetFolderPath(ref FolderPath, ref FolderPathEncrypt, FileDinhKem.FileType);

                fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPath);
                if (FileDinhKem.IsBaoMat == true)
                {
                    fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPathEncrypt);
                }

                var ResultFile = new FileDinhKemBUS().Insert(fileInfo);
                if (ResultFile.Status > 0)
                {
                    fileInfo.FileID = Utils.ConvertToInt32(ResultFile.Data, 0);
                    try
                    {
                        BinaryReader binaryFile = new BinaryReader(source.OpenReadStream());
                        byte[] byteArrFile = binaryFile.ReadBytes((int)source.OpenReadStream().Length);
                        CheckAndCreateFolder(_host, FolderPath);

                        using (FileStream output = File.Create(GetSavePathFile(_host, TenFileHeThong, FolderPath)))
                        {
                            output.Write(byteArrFile);
                        }

                        if (FileDinhKem.IsBaoMat == true)
                        {
                            string pathInputFile = GetSavePathFile(_host, TenFileHeThong, FolderPath);
                            string encryptFile = GetSavePathFile(_host, TenFileHeThong, FolderPathEncrypt);
                            EncryptFile(pathInputFile, encryptFile);
                            if (File.Exists(encryptFile))
                            {
                                //xoa file trong org
                                if (File.Exists(fileInfo.FileUrl)) File.Delete(pathInputFile);
                            }
                        }

                        return fileInfo;
                    }
                    catch (Exception ex)
                    {
                        int FileDinhKemID = Utils.ConvertToInt32(ResultFile.Data, 0);
                        if (FileDinhKemID > 0)
                        {
                            List<FileDinhKemModel> listID = new List<FileDinhKemModel>();
                            listID.Add(new FileDinhKemModel(FileDinhKemID));
                            new FileDinhKemBUS().Delete(listID);
                        }

                        return new FileDinhKemModel();
                    }
                }
                else
                {
                    return new FileDinhKemModel();
                }
            }
            catch (Exception ex)
            {
                return new FileDinhKemModel();
                throw;
            }

        }


        public FileDinhKemModel InsertFile(IFormFile source, FileDinhKemModel FileDinhKem, IHostingEnvironment _host)
        {
            try
            {
                FileDinhKemModel fileInfo = new FileDinhKemModel();
                var crCanBoID = FileDinhKem.NguoiCapNhat ?? 0;
                string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                var tentpm = DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss.fffffff");
                string TenFileHeThong = crCanBoID.ToString() + "_" + tentpm + "_" + TenFileGoc;

                fileInfo.NguoiCapNhat = crCanBoID;
                fileInfo.NgayCapNhat = FileDinhKem.NgayCapNhat ?? DateTime.Now;
                fileInfo.FileType = FileDinhKem.FileType;
                fileInfo.NghiepVuID = FileDinhKem.NghiepVuID;
                fileInfo.NoiDung = FileDinhKem.NoiDung;
                fileInfo.TenFile = FileDinhKem.TenFile;
                if (fileInfo.TenFile == null || fileInfo.TenFile == "")
                {
                    fileInfo.TenFile = TenFileHeThong;
                }
                fileInfo.DMTenFileID = FileDinhKem.DMTenFileID;

                string FolderPath = "UploadFiles";
                string FolderPathEncrypt = "UploadFiles";
                GetFolderPath(ref FolderPath, ref FolderPathEncrypt, FileDinhKem.FileType);

                fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPath);
                if (FileDinhKem.IsBaoMat == true)
                {
                    fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPathEncrypt);
                }

                var ResultFile = new FileDinhKemBUS().Insert(fileInfo);
                if (ResultFile.Status > 0)
                {
                    fileInfo.FileID = Utils.ConvertToInt32(ResultFile.Data, 0);
                    try
                    {
                        BinaryReader binaryFile = new BinaryReader(source.OpenReadStream());
                        byte[] byteArrFile = binaryFile.ReadBytes((int)source.OpenReadStream().Length);
                        CheckAndCreateFolder(_host, FolderPath);

                        using (FileStream output = File.Create(GetSavePathFile(_host, TenFileHeThong, FolderPath)))
                        {
                            output.Write(byteArrFile);
                        }

                        if (FileDinhKem.IsBaoMat == true)
                        {
                            string pathInputFile = GetSavePathFile(_host, TenFileHeThong, FolderPath);
                            string encryptFile = GetSavePathFile(_host, TenFileHeThong, FolderPathEncrypt);
                            EncryptFile(pathInputFile, encryptFile);
                            if (File.Exists(encryptFile))
                            {
                                //xoa file trong org
                                if (File.Exists(fileInfo.FileUrl)) File.Delete(pathInputFile);
                            }
                        }

                        return fileInfo;
                    }
                    catch (Exception ex)
                    {
                        int FileDinhKemID = Utils.ConvertToInt32(ResultFile.Data, 0);
                        if (FileDinhKemID > 0)
                        {
                            List<FileDinhKemModel> listID = new List<FileDinhKemModel>();
                            listID.Add(new FileDinhKemModel(FileDinhKemID));
                            new FileDinhKemBUS().Delete(listID);
                        }

                        return new FileDinhKemModel();
                    }
                }
                else
                {
                    return new FileDinhKemModel();
                }
            }
            catch (Exception ex)
            {
                return new FileDinhKemModel();
                throw;
            }

        }




        // update file danh muc buoc xac minh
        public async Task<FileDinhKemModel> UpdateFileAsync(IFormFile source, FileDinhKemModel FileDinhKem, IHostingEnvironment _host)
        {
            try
            {
                FileDinhKemModel fileInfo = new FileDinhKemModel();
                var crCanBoID = FileDinhKem.NguoiCapNhat ?? 0;
                string TenFileGoc = ContentDispositionHeaderValue.Parse(source.ContentDisposition).FileName.Trim('"');
                string TenFileHeThong = crCanBoID.ToString() + "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + "_" + TenFileGoc;

                fileInfo.FileID = FileDinhKem.FileID;
                fileInfo.NguoiCapNhat = crCanBoID;
                fileInfo.NgayCapNhat = FileDinhKem.NgayCapNhat ?? DateTime.Now;
                fileInfo.FileType = FileDinhKem.FileType;
                fileInfo.NghiepVuID = FileDinhKem.NghiepVuID;
                fileInfo.NoiDung = FileDinhKem.NoiDung;
                fileInfo.TenFile = FileDinhKem.TenFile;
                if (fileInfo.TenFile == null || fileInfo.TenFile == "")
                {
                    fileInfo.TenFile = TenFileHeThong;
                }
                fileInfo.DMTenFileID = FileDinhKem.DMTenFileID;

                string FolderPath = "";
                string FolderPathEncrypt = "";
                GetFolderPath(ref FolderPath, ref FolderPathEncrypt, FileDinhKem.FileType);

                fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPath);
                if (FileDinhKem.IsBaoMat == true)
                {
                    fileInfo.FileUrl = GetUrlFile(TenFileHeThong, FolderPathEncrypt);
                }

                //var ResultFile = new FileDinhKemBUS().Update(fileInfo);
                //if (ResultFile.Status > 0)
                //{
                //    fileInfo.FileID = Utils.ConvertToInt32(ResultFile.Data, 0);
                //    try
                //    {
                //        BinaryReader binaryFile = new BinaryReader(source.OpenReadStream());
                //        byte[] byteArrFile = binaryFile.ReadBytes((int)source.OpenReadStream().Length);
                //        CheckAndCreateFolder(_host, FolderPath);

                //        using (FileStream output = File.Create(GetSavePathFile(_host, TenFileHeThong, FolderPath)))
                //        {
                //            output.Write(byteArrFile);
                //        }

                //        if (FileDinhKem.IsBaoMat == true)
                //        {
                //            string pathInputFile = GetSavePathFile(_host, TenFileHeThong, FolderPath);
                //            string encryptFile = GetSavePathFile(_host, TenFileHeThong, FolderPathEncrypt);
                //            EncryptFile(pathInputFile, encryptFile);
                //            if (File.Exists(encryptFile))
                //            {
                //                //xoa file trong org
                //                if (File.Exists(fileInfo.FileUrl)) File.Delete(pathInputFile);
                //            }
                //        }

                //        return fileInfo;
                //    }
                //    catch (Exception ex)
                //    {
                //        int FileDinhKemID = Utils.ConvertToInt32(ResultFile.Data, 0);
                //        if (FileDinhKemID > 0)
                //        {
                //            List<FileDinhKemModel> listID = new List<FileDinhKemModel>();
                //            listID.Add(new FileDinhKemModel(FileDinhKemID));
                //            new FileDinhKemBUS().Delete(listID);
                //        }

                //        return new FileDinhKemModel();
                //    }
                //}
                //else
                //{
                //    return new FileDinhKemModel();
                //}
                return new FileDinhKemModel();
            }
            catch (Exception ex)
            {
                return new FileDinhKemModel();
                throw;
            }

        }

        public void GetFolderPath(ref string FolderPath, ref string FolderPathEncrypt, int FileType)
        {
            if (FileType == EnumLoaiFile.FileHoSo.GetHashCode())
            {
                FolderPath = PATH_FILE_HOSO;
                FolderPathEncrypt = PATH_FILE_HOSO_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileKQXL.GetHashCode())
            {
                FolderPath = PATH_FILE_KQXL;
                FolderPathEncrypt = PATH_FILE_KQXL_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileDTCPGQ.GetHashCode())
            {
                FolderPath = PATH_FILE_DTCPGQ;
                FolderPathEncrypt = PATH_FILE_DTCPGQ_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileBanHanhQD.GetHashCode())
            {
                FolderPath = PATH_FILE_BHGQ;
                FolderPathEncrypt = PATH_FILE_BHGQ_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileTheoDoi.GetHashCode())
            {

            }
            else if (FileType == EnumLoaiFile.FileYKienXuLy.GetHashCode())
            {

            }
            else if (FileType == EnumLoaiFile.FileGiaiQuyet.GetHashCode())
            {
                FolderPath = PATH_FILE_GIAIQUYET;
                FolderPathEncrypt = PATH_FILE_GIAIQUYET_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileKetQuaTranhChap.GetHashCode())
            {
                FolderPath = PATH_FILE_TRANHCHAP;
                FolderPathEncrypt = PATH_FILE_TRANHCHAP_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileRutDon.GetHashCode())
            {
                FolderPath = PATH_FILE_RUTDON;
                FolderPathEncrypt = PATH_FILE_RUTDON_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileThiHanh.GetHashCode())
            {

            }
            else if (FileType == EnumLoaiFile.FilePhanXuLy.GetHashCode())
            {
                FolderPath = PATH_FILE_PHANXULY;
                FolderPathEncrypt = PATH_FILE_PHANXULY_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileVBDonDoc.GetHashCode())
            {
                FolderPath = PATH_FILE_VBDONDOC;
                FolderPathEncrypt = PATH_FILE_VBDONDOC_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileDTCDGQ.GetHashCode())
            {
                FolderPath = PATH_FILE_DTCPGQ;
                FolderPathEncrypt = PATH_FILE_DTCPGQ_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileBCXM.GetHashCode())
            {

            }
            else if (FileType == EnumLoaiFile.FileDMBXM.GetHashCode())
            {
                FolderPath = PATH_FILE_DMBXM;
                FolderPathEncrypt = PATH_FILE_DMBXM_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileGiaHanGiaiQuyet.GetHashCode())
            {

            }
            else if (FileType == EnumLoaiFile.FileBieuMau.GetHashCode())
            {
                FolderPath = PATH_FILE_BIEUMAU;
                FolderPathEncrypt = PATH_FILE_BIEUMAU_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileHuongDanSuDung.GetHashCode())
            {
                FolderPath = PATH_FILE_HDSD;
                FolderPathEncrypt = PATH_FILE_HDSD_ENCRYPT;
            }
            else if (FileType == EnumLoaiFile.FileDBCXM.GetHashCode())
            {
                FolderPath = PATH_FILE_DBCXM;
                FolderPathEncrypt = PATH_FILE_DBCXM_ENCRYPT;
            }
            else
            {
                FolderPath = "UploadFiles";
                FolderPathEncrypt = "UploadFiles";
            }
        }

        public void CheckAndCreateFolder(Microsoft.AspNetCore.Hosting.IHostingEnvironment _host, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            string path = _host.ContentRootPath + "\\" + sysCF.GetByKey("SavePathFile").ConfigValue + "\\" + folderPath;
            bool isFolder = Directory.Exists(path);
            if (!isFolder)
            {
                Directory.CreateDirectory(path);
            }
        }

        public string GetSavePathFile(Microsoft.AspNetCore.Hosting.IHostingEnvironment _host, string filename, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            return _host.ContentRootPath + "\\" + sysCF.GetByKey("SavePathFile").ConfigValue + "\\" + folderPath + "\\" + filename;
        }

        public string GetUrlFile(string filename, string folderPath = "")
        {
            var sysCF = new SystemConfigDAL();
            var pathfile = sysCF.GetByKey("SavePathFile").ConfigValue;

            return pathfile + "\\" + folderPath + "\\" + filename;
        }

        public string EnsureCorrectFilename(string filename)
        {
            if (filename.Contains("\\"))
                filename = filename.Substring(filename.LastIndexOf("\\") + 1);

            return filename;
        }

        public string GetServerPath(HttpContext httpCT)
        {
            return httpCT.Request.Scheme + "://" + httpCT.Request.Host.Value + "\\";
        }

        ///<summary>
        /// Encrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);



                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Encryption failed!", "Error");
            }
        }
        ///<summary>
        /// Decrypts a file using Rijndael algorithm.
        ///</summary>
        ///<param name="inputFile"></param>
        ///<param name="outputFile"></param>
        private void DecryptFile(string inputFile, string outputFile)
        {
            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
        }
    }
}
