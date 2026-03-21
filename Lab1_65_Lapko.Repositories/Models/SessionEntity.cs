namespace Lab1_65_Lapko.Repositories.Models
{
    public class SessionEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid SubjectId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Topic { get; set; }
        public SessionType Type { get; set; }
    }
}
