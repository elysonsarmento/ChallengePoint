using System.ComponentModel.DataAnnotations;

namespace ChallengePoint.Application.ViewModel
{
    public class ClockOutViewModel
    {
        [Required]
        public required string clockOut { get; set; }
        [Required]
        public required string Enrollment { get; set; }
    }
}
