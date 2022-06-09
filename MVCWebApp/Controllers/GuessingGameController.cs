using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCWebApp.Models.GuessingGame;
using MVCWebApp.Models;
using System.Diagnostics;

namespace MVCWebApp.Controllers
{
    public class GuessingGameController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("randomlyGeneratedNumber") == null)
            {
                //generate random number 
                int generatedrn = Models.GuessingGame.comparePlyerRandomNum.GenerateRandomNumber();
                //random number in a session
                HttpContext.Session.SetInt32("randomlyGeneratedNumber", generatedrn);
                //Number of guesses is so far zero
                HttpContext.Session.SetInt32("NumberOfGuesses", 0);
            }
            
            ViewBag.Message = "The secret Number is generated";
            if (!HttpContext.Request.Cookies.ContainsKey("LeadingScore"))
            {
                //default highscore = 1000000
                SetHighscoreCookie(1000000);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(int HiScoreSofar)
        {

            
            if (HiScoreSofar > 0)
            {
                int _highscoreTest;

                int tries = (int)HttpContext.Session.GetInt32("NumberOfGuesses") + 1;
                HttpContext.Session.SetInt32("NumberOfGuesses", tries);
                int.TryParse(HttpContext.Request.Cookies["highscore"], out _highscoreTest);
                //getting the random numer
                int randomNumberGenerated = (int)HttpContext.Session.GetInt32("randomlyGeneratedNumber");


                ViewBag.Message = Models.GuessingGame.comparePlyerRandomNum.Compare((int)HttpContext.Session.GetInt32("GuessingNumber"), randomNumberGenerated);

                bool Sucess = Models.GuessingGame.comparePlyerRandomNum.CheckSucess((int)HttpContext.Session.GetInt32("GuessingNumber"), randomNumberGenerated);
                //perfect guess
                if (Sucess == true)
                {

                    ViewBag.Message = "Congratulations!Your guess was sucessful";
                    //generate random number 
                    int generatedrn = Models.GuessingGame.comparePlyerRandomNum.GenerateRandomNumber();
                    //random number in a session
                    HttpContext.Session.SetInt32("randomlyGeneratedNumber", generatedrn);
                    //Number of guesses is so far zero
                    HttpContext.Session.SetInt32("NumberOfGuesses", 0);
                    SetHighscoreCookie(tries);
                }
            }
            else

                ViewBag.Message = "Not valid guess! guess again between 1 and 100!";



                return View();


        }
        private void ResetCookieCont()
        {
            CookieOptions cookieOptionReset = new CookieOptions();
            cookieOptionReset.Expires = DateTime.Now.AddDays(-1);

            //default highscore = 1000000
            HttpContext.Response.Cookies.Append("highscore", "1000000", cookieOptionReset);
        }
        private void SetHighscoreCookie(int LeadingScore)
        {
            CookieOptions cookieOption = new CookieOptions();
            cookieOption.Expires = DateTime.Now.AddDays(1);

            HttpContext.Response.Cookies.Append("highscore", LeadingScore.ToString(), cookieOption);
        }
    }
}
