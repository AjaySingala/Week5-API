using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApiClientSide.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult AddCustomerJS()
        {
            return View();

        }

        [HttpPost]
        [Route("Home/SetSession")]
        public void SetSession(string key, string value)
        {
            Session[key] = value;
        }

        public ActionResult AddCustomerJQ()
        {
            return View();
        }

        public ActionResult GetCustomer()
        {
            ViewBag.fname = Session["fname"];
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<ActionResult> LoginClient()
        {
            var result = await LoginApi();
            ViewBag.result = result.access_token;
            Session["token"] = result.access_token;

            return View();
        }

        private async Task<TokenData> LoginApi()
        {
            var url = "http://localhost:54756/";
            TokenData result = null;
            var user = new
            {
                username = "ajay.singala@gmail.com",
                password = "Password@123"
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Clear();
                    
                    //// Add token to Authorization header.
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    //Define request data format  
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    StringContent content = new StringContent(
                        JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                    //Sending request to find web api REST service resource login using HttpClient  
                    HttpResponseMessage response = await client.PostAsJsonAsync("api/account/login", user);
                    response.EnsureSuccessStatusCode();

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (response.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api   
                        //var data = response.Content.ReadAsStringAsync().Result;
                        var data = await response.Content.ReadAsStringAsync();

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        result = JsonConvert.DeserializeObject<TokenData>(data);
                        //result = data;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return result;
        }

        public async Task<ActionResult> GetValues()
        {
            var result = await GetValuesApi();

            return View(result);
        }

        private async Task<IEnumerable<string>> GetValuesApi()
        {
            var url = "http://localhost:54756/";
            IEnumerable<string> result = null;
            var token = Session["token"] as string;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Clear();


                //// Add token to Authorization header.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource login using HttpClient  
                var response = await client.GetStringAsync("api/values");

                //Deserializing the response recieved from web api and storing into the Employee list  
                result = JsonConvert.DeserializeObject<IEnumerable<string>>(response);
            }
            return result;
        }

    }

    public class TokenData
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string bearer { get; set; }
        public string expires_in { get; set; }
        public string username { get; set; }
        //public string .issued { get; set; }
        //public string .expires { get; set; }

    }
}
