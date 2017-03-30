//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace AppTools.Web.Controllers
//{
//    [Route("api/[controller]")]
//    public class UserController : Controller
//    {
//        // GET: api/values
//        [HttpGet]
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/values/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/values
//        [HttpPost]
//        public void Post([FromBody]string value)
//        {
//        }

//        // PUT api/values/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody]string value)
//        {
//        }

//        // DELETE api/values/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppTools.Model;
using AppTools.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppTools.Web.Controllers
{
    //[Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public IActionResult Index()
        {
            return View();
        }

        public UserController(IUserRepository userRepository)
        {

            _userRepository = userRepository;
        }

        //[HttpGet]
        //public IEnumerable<User> GetAll()
        //{
        //    return _userRepository.GetAll();
        //}

        //[HttpGet("{userName}", Name = "GetUser")]
        //public IActionResult GetByUserName(string userName)
        //{
        //    var user = _userRepository.Find(userName);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return new ObjectResult(user);
        //}

        //[HttpPost]
        //public IActionResult Create([FromBody] User user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }
        //    _userRepository.Add(user);
        //    return CreatedAtRoute("GetUser", new { UserName = user.UserName }, user);
        //}

        //[HttpPut("{userName}")]
        //public IActionResult Update(string userName, [FromBody] User user)
        //{
        //    if (user == null || user.UserName != userName)
        //    {
        //        return BadRequest();
        //    }

        //    var insightUser = _userRepository.Find(userName);
        //    if (insightUser == null)
        //    {
        //        return NotFound();
        //    }
        //    insightUser.FirstName = user.FirstName;
        //    insightUser.LastName = user.LastName;

        //    _userRepository.Update(insightUser);
        //    return new NoContentResult();
        //}

        //[HttpDelete("{userName}")]
        //public IActionResult Delete(string userName)
        //{
        //    var user = _userRepository.Find(userName);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _userRepository.Remove(userName);
        //    return new NoContentResult();
        //}

    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

//namespace AppTools.Web.Controllers
//{
//    public class UserController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult About()
//        {
//            ViewData["Message"] = "Your application description page.";

//            return View();
//        }

//        public IActionResult Contact()
//        {
//            ViewData["Message"] = "Your contact page.";

//            return View();
//        }

//        public IActionResult Error()
//        {
//            return View();
//        }
//    }
//}
