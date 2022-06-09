using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly Iperson _person;
        public PersonController(Iperson _iperson)
        {
            _person = _iperson;
        }
        public IActionResult Index()
        {

            return View();
        }
    }
}
