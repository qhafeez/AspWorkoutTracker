using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Workout_Tracker.Data;
using Workout_Tracker.Models;

namespace Workout_Tracker.Controllers
{
    public class AuthController : Controller
    {

        private ApplicationDbContext context;




        public AuthController(ApplicationDbContext dbContext)
        {
            context = dbContext;
        }

        string local = "localhost:44327";

        string deployed = "aspworkouttracker.azurewebsites.net";

        public IActionResult Index()
        {
            string parameters = "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&include_granted_scopes=true&redirect_uri=https%3A%2F%2F"+deployed+ "%2FAuth%2FOauthred&response_type=code&&client_id=633847887848-k6033dv1eb2k5418mdabtfkn4n19m278.apps.googleusercontent.com";

            return Redirect("https://accounts.google.com/o/oauth2/v2/auth?"+parameters);
        }


        public string GetToken(string code) {

            string grantType = "client_credentials";

            string clientId = "633847887848-k6033dv1eb2k5418mdabtfkn4n19m278.apps.googleusercontent.com";

            string clientSecret = "9SnPNF4XOjvPFX6UNO0tIAHA";

            string authCode = code;
            RestClient client = new RestClient("https://oauth2.googleapis.com/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "redirect_uri=https%3A%2F%2F" + deployed + "%2FAuth%2FOauthred&code=" + authCode+"&grant_type=authorization_code&client_id=633847887848-k6033dv1eb2k5418mdabtfkn4n19m278.apps.googleusercontent.com&client_secret=9SnPNF4XOjvPFX6UNO0tIAHA", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content; 
        }


        public async Task<IActionResult> Oauthred(string code)
        {

            if (code != "")
            {
                string a;



                a = GetToken(code);
                dynamic b = JsonConvert.DeserializeObject(a);
                var token = b.access_token;


                RestClient client = new RestClient("https://people.googleapis.com/v1/people/me?personFields=names");
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Bearer " + token);

                IRestResponse response = await client.ExecuteTaskAsync(request);

                dynamic userInfo = JsonConvert.DeserializeObject(response.Content);

                string googleID = userInfo.names[0].metadata.source.id;
                string userName = userInfo.names[0].displayName;

                //use googleID to check if the user is in the database
                User user = context.Users.Where(c => c.GoogleIdString == googleID).SingleOrDefault();

                //cookies will be used to store the id of the current user after they are logged in

                //if the user is, log them in
                if (user != null)
                {


                    //HttpContext.Session.SetString("userId", "yessire");
                    //CookieOptions option = new CookieOptions();
                    //option.Expires = DateTime.Now.AddHours(3);
                    //option.IsEssential = true;
                    //string sess = HttpContext.Session.GetString("userId");
                    HttpContext.Session.SetString("userId", user.GoogleIdString);
                  //  Response.Cookies.Append("userId", user.GoogleIdString, option);
                    
                }
                else
                {

                    //else insert them in the database and log them in
                    User newUser = new User
                    {
                        Name = userName,
                        GoogleIdString = googleID
                    };

                    context.Users.Add(newUser);
                    context.SaveChanges();

                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddHours(3);
                    option.IsEssential = true;

                    User justAdded = context.Users.Where(c => c.GoogleIdString == googleID).SingleOrDefault();
                    HttpContext.Session.SetString("userId", justAdded.GoogleIdString);
                    //HttpContext.Session.
                    //Response.Cookies.Append("userId", justAdded.GoogleIdString, option);


                }







                return RedirectToAction("HomePage", "Home");
            }
            else {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult LogOut()
        {

            HttpContext.Session.Remove("userId");
            //Response.Cookies.Delete("userId");
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");

        }
     
    }
}