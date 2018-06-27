using System;
using System.Collections.Generic;

namespace SharkFit.Web.ViewModels
{
    public class ChallengeDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Bet { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public IEnumerable<ChallengeParticipantViewModel> Participants { get; set; }
    }
}
