using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AppTools.Model;
using AppTools.Data;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppTools.Web.Controllers
{
    //[Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        //private readonly object routeValues;
        public AppKeyConfig AppConfigs { get; }

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index(string searchString)
        {
            //return View();

            return View(_userRepository.GetAll(searchString));
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Users/Details/testuser
        public IActionResult Details(string userName)
        {
            if (userName is null)
            {
                return NotFound();
            }

            var user =  _userRepository.Find(userName);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,FirstName,LastName,Email,Partner,Telephone,CanCreateCase,Company")] User user)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(movie);
                //await _context.SaveChangesAsync();
                try
                {
                    _userRepository.Add(user.UserName, user);
                
                }
                catch(Exception ex)
                {
                    throw;
                }
                return RedirectToAction("Details/" + user.UserName, "user");
            }
            return View(user);
        }

        // GET: Users/Create
        public IActionResult Add()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add([Bind("UserName,FirstName,LastName,Email,Partner,Telephone,CanCreateCase,Company")] User user)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(movie);
                //await _context.SaveChangesAsync();
                try
                {
                    _userRepository.Add(user.UserName, user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return RedirectToAction("Details/" + user.UserName, "user");

            }
            return View(user);
        }

        // GET: Users/Edit/testuser
        public IActionResult Edit(string userName)
        {
            if (userName is null)
            {
                return NotFound();
            }

            var user = _userRepository.Find(userName);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/testuser
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string userName, [Bind("UserName,FirstName,LastName,Email,Partner,Telephone,CanCreateCase,Company")] User user)
        {
            if (userName != user.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _userRepository.Update(userName, user);
                    //_context.Update(movie);
                    //await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    //if (!MovieExists(movie.ID))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                    throw;
                }
                return RedirectToAction("Details/"+ userName, "user");
            }
            return View(user);
        }

        // GET: Users/Delete/testuser
        public async Task<IActionResult> Delete(string userName)
        {
            if (userName.Trim() == "")
            {
                return NotFound();
            }

            User user = null;
            //var movie = await _context.Movie
            //    .SingleOrDefaultAsync(m => m.ID == id);
            //if (movie == null)
            //{
            //    return NotFound();
            //}

            return View(user);
        }

        // POST: Users/Delete/testuser
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string userName)
        {
            //var movie = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
            //_context.Movie.Remove(movie);
            //await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
