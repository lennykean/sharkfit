using System;
using System.Security.Claims;

using AspNetCore.ClaimsValueProvider;

using LiteDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharkFit.Data.Model;

namespace SharkFit.Web.Controllers
{
    [Authorize, Route("Challenge/{challengeId:int}")]
    public class CheckinController : Controller
    {
        private readonly LiteCollection<Challenge> _challengeCollection;
        private readonly LiteCollection<Checkin> _checkinCollection;

        public CheckinController(LiteCollection<Challenge> challengeCollection, LiteCollection<Checkin> checkinCollection)
        {
            _challengeCollection = challengeCollection;
            _checkinCollection = checkinCollection;
        }

        [HttpGet("Checkin")]
        public IActionResult Checkin(int challengeId)
        {
            var challenge = _challengeCollection.FindById(challengeId);
            if (challenge == null)
                return NotFound();

            return View(challenge);
        }

        [HttpPost("Checkin")]
        public IActionResult Checkin(int challengeId, Checkin checkin, [FromClaim(ClaimTypes.NameIdentifier)]string userId)
        {
            var challenge = _challengeCollection.FindById(challengeId);
            if (challenge == null)
                return NotFound();

            checkin.UserId = userId;
            checkin.ChallengeId = challengeId;
            checkin.CheckinDate = DateTime.Now;
            _checkinCollection.Insert(checkin);

            return RedirectToAction("Detail", "Challenge", new { id = challengeId });
        }
    }
}