using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharkFit.Data.Model
{
    public class Challange
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal? Bet { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? Start { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime? End { get; set; }

        public List<Participant> Participants { get; set; } = new List<Participant>();
    }
}
