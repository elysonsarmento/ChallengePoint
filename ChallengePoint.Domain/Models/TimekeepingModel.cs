namespace ChallengePoint.Domain.Models
{
    public class TimekeepingModel
    {
        public int Id { get; set; }

        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public DateTime? ServerTimestampIn { get; set; }
        public DateTime? ServerTimestampOut { get; set; }
        public int CollaboratorId { get; set; }
    }
}