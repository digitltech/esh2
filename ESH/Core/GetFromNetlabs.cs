using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ESH.Core
{
    public class GetFromNetlabs
    {
        public static string GetToken()
        {
            string token = String.Empty;
            var req = WebRequest.Create("http://services.netlab.ru/rest/authentication/token.json?username=*&password=*");

            req.ContentType = "text/json";

            StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
            var responseTokenData = responseReader.ReadToEnd();
            responseTokenData = responseTokenData.Remove(0, 6);

     
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<GetToken>(responseTokenData);

            if (data.TokenResponseJson_GetToken.Status_GetToken.code == "200")
            {
                token = data.TokenResponseJson_GetToken.Data_GetToken.token;
           

            }
            else
            {
                token = " ";
            }

            return token;
        }

        public static  double GetPrice (string token,string idproduct)
        {
            double price = 0;
            var req = WebRequest.Create("http://services.netlab.ru/rest/catalogsZip/goodsByUid/"+idproduct+ ".json?oauth_token="+token);
            req.ContentType = "text/json";
            StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
            var responcePriceData = responseReader.ReadToEnd();
            responcePriceData = responcePriceData.Remove(0, 6);


            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<GetPrice>(responcePriceData);
            if (data.PriceResponceJson_GetPrice.Status_GetToken.code == "200")
            {

                price = data.PriceResponceJson_GetPrice.Data_GetPrice.Property_getPrice.цена_по_категории_D;
            }
            return price;
        }

        public static int GetOstatok(string token, string idproduct)
        {
            int ostatok = 0;
            var req = WebRequest.Create("http://services.netlab.ru/rest/catalogsZip/goodsByUid/" + idproduct + ".json?oauth_token=" + token);
            req.ContentType = "text/json";
            StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
            var responcePriceData = responseReader.ReadToEnd();
            responcePriceData = responcePriceData.Remove(0, 6);


            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<GetPrice>(responcePriceData);
            if (data.PriceResponceJson_GetPrice.Status_GetToken.code == "200")
            {

                ostatok = data.PriceResponceJson_GetPrice.Data_GetPrice.Property_getPrice.количество_на_Курской + data.PriceResponceJson_GetPrice.Data_GetPrice.Property_getPrice.количество_на_Калужской + data.PriceResponceJson_GetPrice.Data_GetPrice.Property_getPrice.количество_на_Лобненской + Convert.ToInt32( data.PriceResponceJson_GetPrice.Data_GetPrice.Property_getPrice.количество_в_транзите);
            }
            return ostatok;
        }

        public static double GetKursRub(string token)
        {
            double kurs = 0;
            var req = WebRequest.Create("http://services.netlab.ru/rest/catalogsZip/info.json?oauth_token="+token);

            req.ContentType = "text/json";

            StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream());
            var responseTokenData = responseReader.ReadToEnd();
            responseTokenData = responseTokenData.Remove(0, 6);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<GetKursRub_json>(responseTokenData);
            foreach (var i in data.GetKursRub.data.ItemKurs)
            {
                kurs = i.properties.usdRateNonCash;
            }
            return kurs;
               
         }
    }



    public class GetToken
    {
        [JsonProperty("tokenResponse")]
        public TokenResponseJson_GetToken TokenResponseJson_GetToken { get; set; }
    }

    public class TokenResponseJson_GetToken
    {
        [JsonProperty("status")]
        public Status_GetToken Status_GetToken { get; set; }
        [JsonProperty("data")]
        public Data_GetToken Data_GetToken { get; set; }
    }
    public class Status_GetToken
    {
        [JsonProperty("code")]
        public string code { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class Data_GetToken
    {
        [JsonProperty("token")]
        public string token { get; set; }
        [JsonProperty("expiredIn")]
        public string expiredIn { set; get; }
    }

    public class GetPrice
    {
        [JsonProperty("goodsResponse")]
        public PriceResponceJson_GetPrice PriceResponceJson_GetPrice { get; set; }
    }

    public class PriceResponceJson_GetPrice
    {
        [JsonProperty("status")]
        public Status_GetToken Status_GetToken { get; set; }
        [JsonProperty("data")]
        public Data_GetPrice Data_GetPrice { get; set; }
    }
    public class Data_GetPrice
    {
        [JsonProperty("id")]
        public string id {get;set;}
        [JsonProperty("properties")]
        public  Property_getPrice Property_getPrice { get; set; }
    }

    public class Property_getPrice
    {
        [JsonProperty(PropertyName = "количество на Курской")]
        public int количество_на_Курской { get; set; }

        [JsonProperty(PropertyName = "цена по категории B")]
        public double цена_по_категории_B { get; set; }

    [JsonProperty(PropertyName = "цена по категории A")]
    public double цена_по_категории_A { get; set; }

[JsonProperty(PropertyName = "цена по категории D")]
public double цена_по_категории_D { get; set; }

        [JsonProperty(PropertyName = "удаленный склад")]
public string удаленный_склад { get; set; }

        [JsonProperty(PropertyName = "цена по категории C")]
public double цена_по_категории_C { get; set; }

        [JsonProperty(PropertyName = "количество на Калужской")]
public int количество_на_Калужской { get; set; }

[JsonProperty(PropertyName = "цена по категории F")]
public double цена_по_категории_F { get; set; }

        [JsonProperty(PropertyName = "цена по категории E")]
public double цена_по_категории_E { get; set; }

        [JsonProperty(PropertyName = "id")]
public int id { get; set; }

[JsonProperty(PropertyName = "количество на Лобненской")]
public int количество_на_Лобненской { get; set; }

[JsonProperty(PropertyName = "количество в транзите")]
public double количество_в_транзите { get; set; }

[JsonProperty(PropertyName = "название")]
public string название { get; set; }

[JsonProperty(PropertyName = "цена по категории N")]
public double цена_по_категории_N { get; set; }






    }

    public class StatusKurs
    {

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }

    public class PropertiesKurs
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("usdRateNonCash")]
        public double usdRateNonCash { get; set; }

        [JsonProperty("usdRateCash")]
        public double usdRateCash { get; set; }
    }

    public class ItemKurs
    {

        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("properties")]
        public PropertiesKurs properties { get; set; }
    }

    public class DataKurs
    {

        [JsonProperty("items")]
        public IList<ItemKurs> ItemKurs { get; set; }
    }

    public class GetKursRub
    {

        [JsonProperty("status")]
        public StatusKurs status { get; set; }

        [JsonProperty("data")]
        public DataKurs data { get; set; }
    }

    public class GetKursRub_json
    {

        [JsonProperty("entityListResponse")]
        public GetKursRub GetKursRub { get; set; }
    }


}
