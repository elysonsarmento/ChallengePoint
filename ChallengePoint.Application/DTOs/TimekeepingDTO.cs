namespace ChallengePoint.Domain.DTO
{
    public class TimekeepingDTO
    {
        public int Id { get; set; }

        public DateTime? ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public DateTime? ServerTimestampIn { get; set; }
        public DateTime? ServerTimestampOut { get; set; }
        public int CollaboratorId { get; set; }

        public string? Enrollment { get; set; }
        public string? Name { get; set; }
    }
}