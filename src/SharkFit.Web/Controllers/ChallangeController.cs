using System;
using System.Linq;

using AspNetCore.Identity.LiteDB.Models;

using LiteDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SharkFit.Data.Model;

namespace SharkFit.Web.Controllers
{
    [Authorize]
    public class ChallangeController : Controller
    {
        private readonly LiteCollection<Challange> _collection;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChallangeController(LiteCollection<Challange> collection, UserManager<ApplicationUser> userManager)
        {
            _collection = collection;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var challange = _collection.FindById(id);
            if (challange == null)
                return NotFound();

            return View(challange);
        }

        [HttpGet]
        public IActionResult List()
        {
            var challanges = _collection.FindAll();

            return View(challanges);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Challange model)
        {
            if (!ModelState.IsValid)
                return View();

            _collection.Insert(model);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Join(int id)
        {
            var challange = _collection.FindById(id);
            if (challange == null)
                return NotFound();
            
            return View(challange);
        }

        [HttpPost]
        public IActionResult Join(int id, Participant participant)
        {
            var challange = _collection.FindById(id);
            if (challange == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(challange);

            var userId = _userManager.GetUserId(User);

            if (!challange.Participants.Any(p => p.UserId == userId))
            {
                participant.UserId = userId;
                participant.Joined = DateTime.Now;
                challange.Participants.Add(participant);
                _collection.Update(challange);
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}