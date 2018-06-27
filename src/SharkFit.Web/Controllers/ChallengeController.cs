using System;
using System.Linq;
using System.Security.Claims;

using AspNetCore.ClaimsValueProvider;

using LiteDB;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharkFit.Data.Model;
using SharkFit.Web.ViewModels;

namespace SharkFit.Web.Controllers
{
    [Authorize]
    public class ChallengeController : Controller
    {
        private readonly LiteCollection<Challenge> _challengeCollection;
        private readonly LiteCollection<Checkin> _checkinCollection;

        public ChallengeController(LiteCollection<Challenge> challengeCollection, LiteCollection<Checkin> checkinCollection)
        {
            _challengeCollection = challengeCollection;
            _checkinCollection = checkinCollection;
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var challenge = _challengeCollection.FindById(id);
            if (challenge == null)
                return NotFound();

            return View(new ChallengeDetailViewModel
            {
                Id = challenge.Id,
                Title = challenge.Title,
                Description = challenge.Description,
                Bet = challenge.Bet,
                Start = challenge.Start,
                End = challenge.End,
                Participants = (
                    from p in challenge.Participants
                    let checkins = _checkinCollection.Find(c => c.UserId == p.UserId && c.ChallengeId == id).ToList()
                    let firstCheckin = checkins.OrderBy(c => c.CheckinDate).FirstOrDefault()
                    let lastCheckin = checkins.OrderByDescending(c => c.CheckinDate).FirstOrDefault()
                    select new ChallengeParticipantViewModel
                    {
                        UserId = p.UserId,
                        Name = p.Name,
                        Joined = p.Joined,
                        StartingWeight = firstCheckin?.Weight,
                        LastCheckin = lastCheckin?.CheckinDate,
                        LastWeight = lastCheckin?.Weight,
                        Weightloss = firstCheckin?.Weight - lastCheckin?.Weight
                    }).ToList()
            });
        }

        [HttpGet]
        public IActionResult List()
        {
            var challenges = _challengeCollection.FindAll();

            return View(challenges);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Challenge model)
        {
            if (!ModelState.IsValid)
                return View();

            _challengeCollection.Insert(model);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Join(int id)
        {
            var challenge = _challengeCollection.FindById(id);
            if (challenge == null)
                return NotFound();
            
            return View(challenge);
        }

        [HttpPost]
        public IActionResult Join(int id, Participant participant, [FromClaim(ClaimTypes.NameIdentifier)]string userId)
        {
            var challenge = _challengeCollection.FindById(id);
            if (challenge == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(challenge);
            
            if (!challenge.Participants.Any(p => p.UserId == userId))
            {
                participant.UserId = userId;
                participant.Joined = DateTime.Now;
                challenge.Participants.Add(participant);
                _challengeCollection.Update(challenge);
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}
