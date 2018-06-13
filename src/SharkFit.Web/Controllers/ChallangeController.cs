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

        [HttpPost]
        public IActionResult Join(int id)
        {
            var challange = _collection.FindById(id);
            if (challange == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (!challange.ParticipantIds.Contains(userId))
            {
                challange.ParticipantIds.Add(userId);
                _collection.Update(challange);
            }

            return RedirectToAction("List");
        }
    }
}