using System;
using System.ComponentModel.DataAnnotations;

namespace SharkFit.Data.Model
{
    public class Checkin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ChallengeId { get; set; }
        [Required]
        public decimal? Weight { get; set; }
        public DateTime CheckinDate { get; set; }
    }
}
