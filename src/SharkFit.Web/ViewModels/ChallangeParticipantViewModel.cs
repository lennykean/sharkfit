using System;

namespace SharkFit.Web.ViewModels
{
    public class ChallangeParticipantViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime Joined { get; set; }
        public DateTime? LastCheckin { get; set; }
        public decimal? StartingWeight { get; set; }
        public decimal? LastWeight { get; set; }
        public decimal? Weightloss { get; set; }
    }
}
