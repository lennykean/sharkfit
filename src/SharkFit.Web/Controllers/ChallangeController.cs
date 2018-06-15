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
    public class ChallangeController : Controller
    {
        private readonly LiteCollection<Challange> _challangeCollection;
        private readonly LiteCollection<Checkin> _checkinCollection;

        public ChallangeController(LiteCollection<Challange> challangeCollection, LiteCollection<Checkin> checkinCollection)
        {
            _challangeCollection = challangeCollection;
            _checkinCollection = checkinCollection;
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var challange = _challangeCollection.FindById(id);
            if (challange == null)
                return NotFound();

            return View(new ChallangeDetailViewModel
            {
                Id = challange.Id,
                Title = challange.Title,
                Description = challange.Description,
                Bet = challange.Bet,
                Start = challange.Start,
                End = challange.End,
                Participants = (
                    from p in challange.Participants
                    let checkins = _checkinCollection.Find(c => c.UserId == p.UserId).ToList()
                    let firstCheckin = checkins.OrderBy(c => c.CheckinDate).FirstOrDefault()
                    let lastCheckin = checkins.OrderByDescending(c => c.CheckinDate).FirstOrDefault()
                    select new ChallangeParticipantViewModel
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
            var challanges = _challangeCollection.FindAll();

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

            _challangeCollection.Insert(model);

            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Join(int id)
        {
            var challange = _challangeCollection.FindById(id);
            if (challange == null)
                return NotFound();
            
            return View(challange);
        }

        [HttpPost]
        public IActionResult Join(int id, Participant participant, [FromClaim(ClaimTypes.NameIdentifier)]string userId)
        {
            var challange = _challangeCollection.FindById(id);
            if (challange == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(challange);
            
            if (!challange.Participants.Any(p => p.UserId == userId))
            {
                participant.UserId = userId;
                participant.Joined = DateTime.Now;
                challange.Participants.Add(participant);
                _challangeCollection.Update(challange);
            }
            return RedirectToAction("Detail", new { id });
        }
    }
}