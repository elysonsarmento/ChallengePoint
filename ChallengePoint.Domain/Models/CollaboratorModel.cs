namespace ChallengePoint.Domain.Models
{
    public class CollaboratorModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Position { get; set; }
        public decimal Salary { get; set; }
        public required string Enrollment { get; set; }
    }
}