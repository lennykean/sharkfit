using System;
using System.Security.Claims;

using AspNetCore.ClaimsValueProvider;

using LiteDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharkFit.Data.Model;

namespace SharkFit.Web.Controllers
{
    [Authorize, Route("Challange/{challangeId:int}")]
    public class CheckinController : Controller
    {
        private readonly LiteCollection<Challange> _challangeCollection;
        private readonly LiteCollection<Checkin> _checkinCollection;

        public CheckinController(LiteCollection<Challange> challangeCollection, LiteCollection<Checkin> checkinCollection)
        {
            _challangeCollection = challangeCollection;
            _checkinCollection = checkinCollection;
        }

        [HttpGet("Checkin")]
        public IActionResult Checkin(int challangeId)
        {
            var challange = _challangeCollection.FindById(challangeId);
            if (challange == null)
                return NotFound();

            return View(challange);
        }

        [HttpPost("Checkin")]
        public IActionResult Checkin(int challangeId, Checkin checkin, [FromClaim(ClaimTypes.NameIdentifier)]string userId)
        {
            var challange = _challangeCollection.FindById(challangeId);
            if (challange == null)
                return NotFound();

            checkin.UserId = userId;
            checkin.ChallangeId = challangeId;
            checkin.CheckinDate = DateTime.Now;
            _checkinCollection.Insert(checkin);

            return RedirectToAction("Detail", "Challange", new { id = challangeId });
        }
    }
}