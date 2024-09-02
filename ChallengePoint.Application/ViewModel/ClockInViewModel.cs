using System.ComponentModel.DataAnnotations;

namespace ChallengePoint.Application.ViewModel
{
    public class ClockInViewModel

    {
        [Required]
        public required string ClockIn { get; set; }
        [Required]
        public required string Enrollment { get; set; }
    }
}
