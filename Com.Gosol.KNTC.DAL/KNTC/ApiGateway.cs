using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.KNTC.DAL.KNTC
{
    public class ApiGateway
    {
        public class objMapping
        {
            public class ObjMapInfo
            {
                public int Ma { get; set; }

                public string Ten { get; set; }

                public string MappingCode { get; set; }
            }

            public class ObjMap
            {
                [JsonProperty("Ma")]
                public string Ma { get; set; }

                [JsonProperty("Ten")]
                public string Ten { get; set; }
            }
        }

        public static string GovSyncConfig(string key)
        {
            try
            {
                //return ConfigurationManager.get_AppSettings()[key].ToString();
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        //public static void InitGrid(ref Repeater rpt, object obj)
        //{
        //    try
        //    {
        //        rpt.set_DataSource(obj);
        //        ((Control)rpt).DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static objMapping.ObjMap AddMap(string T, string M)
        {
            objMapping.ObjMap objMap = new objMapping.ObjMap();
            objMap.Ma = M;
            objMap.Ten = T;
            return objMap;
        }

        public static List<T> GovApi_TraDanhMuc<T>(string LoaiDanhMuc, string MaTinh) where T : class, new()
        {
            try
            {
                string jSOContent = "{" + $"LoaiDanhMuc: \"{LoaiDanhMuc}\", MaTinh: \"{MaTinh}\", MaBoNganh: \"\", MaLoaiKhieuTo: \"\"" + ",}";
                string text = ApiPost("TraDanhMuc", jSOContent);
                return JToken.Parse(JsonConvert.DeserializeObject(text.ToString())!.ToString()).ToObject<List<T>>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string ApiPost(string ApiMethodAction, string JSOContent)
        {
            try
            {
                if (string.IsNullOrEmpty(ApiMethodAction))
                {
                    return string.Empty;
                }

                string MappingID = string.Empty;
                return ApiPost(ApiMethodAction, ref MappingID, JSOContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string ApiPost(string ApiMethodAction, ref string MappingID, string JSOContent)
        {
            //IL_0028: Unknown result type (might be due to invalid IL or missing references)
            //IL_002e: Expected O, but got Unknown
            //IL_0073: Unknown result type (might be due to invalid IL or missing references)
            //IL_007d: Expected O, but got Unknown
            //IL_0089: Unknown result type (might be due to invalid IL or missing references)
            //IL_0090: Expected O, but got Unknown
            string empty = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(ApiMethodAction) || string.IsNullOrEmpty(JSOContent))
                {
                    return string.Empty;
                }

                HttpClient val = new HttpClient();
                try
                {
                    string s = GovSyncConfig("GovApiUser") + ":" + GovSyncConfig("GovApiPass");
                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                    string text = Convert.ToBase64String(bytes);
                    //val.get_DefaultRequestHeaders().set_Authorization(new AuthenticationHeaderValue("Basic", text));
                    StringContent val2 = new StringContent(JSOContent, Encoding.UTF8, "application/json");
                    HttpResponseMessage result = val.PostAsync(GovSyncConfig("GovApiSync") + ApiMethodAction, (HttpContent)(object)val2).Result;
                    //if (!result.get_IsSuccessStatusCode())
                    //{
                    //    throw new Exception(((object)result.get_RequestMessage().get_Content()).ToString());
                    //}

                    //Task<string> task = result.get_Content().ReadAsStringAsync();
                    //task.Wait();
                    //return task.Result.ToString();

                    return string.Empty;
                }
                finally
                {
                    ((IDisposable)val)?.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class DuLieuMapping
    {
        public string TypeApi { get; set; }
        public List<ApiGateway.objMapping.ObjMapInfo> DuLieu { get; set; }
    }
}
