using System;
using System.ComponentModel.DataAnnotations;

namespace SharkFit.Data.Model
{
    public class Participant
    {
        public string UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime Joined { get; set; }
    }
}