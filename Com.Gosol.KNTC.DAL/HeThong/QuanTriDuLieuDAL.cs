using Com.Gosol.KNTC.Models;
using Com.Gosol.KNTC.Models.HeThong;
using Com.Gosol.KNTC.Ultilities;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;


namespace Com.Gosol.KNTC.DAL.HeThong
{

    public class QuanTriDuLieuDAL
    {
        public int BackupData(string fileName, string filePath)
        {
            var backupPath = new SystemConfigDAL().GetByKey("BackUp_Path").ConfigValue.ToString();
            //var backupService = new BackupService(SQLHelper.appConnectionStrings, SQLHelper.backupPath);
            var backupService = new BackupService(SQLHelper.appConnectionStrings, backupPath);
            try
            {
                return backupService.BackupDatabase(fileName, backupPath);
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }


        public int RestoreData(string fileName)
        {
            var backupPath = new SystemConfigDAL().GetByKey("BackUp_Path").ConfigValue.ToString();
            //var restoreService = new RestoreService(SQLHelper.appConnectionStrings, SQLHelper.backupPath);
            var restoreService = new RestoreService(SQLHelper.appConnectionStrings, backupPath);
            try
            {
                return restoreService.RestoreDatabase(fileName);
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }

        public List<QuanTriDuLieuModel> GetFileInDerectory()
        {
            try
            {
                var backupPath = new SystemConfigDAL().GetByKey("BackUp_Path").ConfigValue.ToString();
                var Result = new List<QuanTriDuLieuModel>();
                //var fullFile = Directory.GetFiles(SQLHelper.backupPath, "*.bak");
                var fullFile = Directory.GetFiles(backupPath, "*.bak");
                if (fullFile.Length > 0)
                {
                    for (int i = 0; i < fullFile.Length; i++)
                    {
                        var fileName = Path.GetFileName(fullFile[i].ToString()).Trim();
                        Result.Add(new QuanTriDuLieuModel(fileName));
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                return new List<QuanTriDuLieuModel>();
                throw ex;
            }
        }

        // ghi dữ liệu vào file GhiLogQuanTriDuLieu.txt
        public BaseResultModel WirteFile(string ThaoTac, string ThoiGian, string NguoiThucHien)
        {
            List<FileTextMOD> preData = (List<FileTextMOD>)ReadFileTXT().Data;
            var Result = new BaseResultModel();
            try
            {
                //Pass the filepath and filename to the StreamWriter Constructor
                using (StreamWriter sw = File.CreateText("E:\\C# API\\KNTC_V2\\SourceCode\\API\\Publish\\Upload\\GhiLogQuanTriDuLieu.txt"))
                {
                    FileTextMOD obj = new FileTextMOD()
                    {
                        ThoiGian = ThoiGian,
                        ThaoTac = ThaoTac,
                        NguoiThucHien = NguoiThucHien

                    };

                    preData.Add(obj);

                    string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(preData);

                    //Write a line of text
                    sw.WriteLine(jsonData);
                    //Close the file
                    sw.Close();
                }

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                return Result;
            }

            return Result;
        }

        /*public BaseResultModel ReadFile()
        {
            var Result = new BaseResultModel();
            List<FileTextMOD> file = new List<FileTextMOD>();
            //string line;
            try
            {
                using (StreamReader sr = new StreamReader("E:\\Works\\KNTC_v2\\SourceCode\\API\\GO.API\\Upload\\GhiLogQuanTriDuLieu.txt"))
                {   
                    
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        FileTextMOD item = new FileTextMOD();
                        item.ThoiGian = "";
                        item.ThaoTac = "";
                        item.NguoiThucHien = "";
                        //Result.Add(new QuanTriDuLieuModel(line));
                        //sr.ReadLine();
                        file.Add(item);
                    }
                    sr.Close();
                }
                Result.Status = 1;
                Result.Data = file;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ex.ToString();
            }
            return Result;
        }*/

        // Đọc file GhiLogQuanTriDuLieu.txt
        public BaseResultModel ReadFileTXT()
        {
            var Result = new BaseResultModel();
            List<FileTextMOD> file = new List<FileTextMOD>();

            try
            {
                string str = File.ReadAllText("E:\\C# API\\KNTC_V2\\SourceCode\\API\\Publish\\Upload\\GhiLogQuanTriDuLieu.txt");
                JArray jarr = JArray.Parse(str);//get 1 mảng list object
                                                //dynamic jarr = JsonConvert.DeserializeObject(str);
                for (int i = 0; i < jarr.Count; i++)
                {
                    JObject obj = (JObject)jarr[i]; //nhận obj thứ i
                    string ThoiGian = obj["ThoiGian"].ToString();
                    string ThaoTac = obj["ThaoTac"].ToString();
                    string NguoiThucHien = obj["NguoiThucHien"].ToString();
                    file.Add(new FileTextMOD { ThoiGian = ThoiGian, ThaoTac = ThaoTac, NguoiThucHien = NguoiThucHien });

                }
                Result.Status = 1;
                Result.Data = file;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                return Result;
                throw ex;
            }
            
        }

        // Tìm kiếm 
        public BaseResultModel TimKiem(string? Keyword)
        {
            var Result = new BaseResultModel();
            try
            {
                string str = File.ReadAllText("E:\\C# API\\KNTC_V2\\SourceCode\\API\\Publish\\Upload\\GhiLogQuanTriDuLieu.txt");
                JArray strArr = JArray.Parse(str);
                List<FileTextMOD> fileTextMODList = strArr.ToObject<List<FileTextMOD>>();
                List<FileTextMOD> newArray = new List<FileTextMOD>();
                if (Keyword == null)
                {
                    Result.Status = 1;
                    Result.Data = fileTextMODList;
                    return Result;
                }

                foreach(FileTextMOD fileTextMod in fileTextMODList)
                {
                    if(fileTextMod.ThaoTac.ToLower().Contains(Keyword.ToLower()) || fileTextMod.NguoiThucHien.ToLower().Contains(Keyword.ToLower()))
                    {
                        newArray.Add(fileTextMod);
                    }
                }

                Result.Status = 1;
                Result.Data = newArray;
                return Result;
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = ConstantLogMessage.API_Error_System;
                return Result;
                throw ex;
            }
        }

    }
}
