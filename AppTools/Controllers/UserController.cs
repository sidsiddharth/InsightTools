using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppTools.Model;
using AppTools.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppTools.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        [HttpGet("{userName}", Name = "GetUser")]
        public IActionResult GetByUserName(string userName)
        {
            var user = _userRepository.Find(userName);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            _userRepository.Add(user);
            return CreatedAtRoute("GetUser", new { UserName = user.UserName }, user);
        }

        [HttpPut("{userName}")]
        public IActionResult Update(string userName, [FromBody] User user)
        {
            if (user==null || user.UserName != userName)
            {
                return BadRequest();
            }

            var insightUser = _userRepository.Find(userName);
            if (insightUser == null)
            {
                return NotFound();
            }
            insightUser.FirstName = user.FirstName;
            insightUser.LastName = user.LastName;

            _userRepository.Update(insightUser);
            return new NoContentResult();
        }

        [HttpDelete("{userName}")]
        public IActionResult Delete(string userName)
        {
            var user = _userRepository.Find(userName);
            if (user==null)
            {
                return NotFound();
            }

            _userRepository.Remove(userName);
            return new NoContentResult();
        }

    }
}
