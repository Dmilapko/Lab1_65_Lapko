namespace Lab1_65_Lapko.Services.DTOs
{
    /// <summary>
    /// Input DTO used for both creating and updating a Session.
    /// Type is a string to keep UI decoupled from the storage enum.
    /// </summary>
    public class SessionInputDto
    {
        public Guid SubjectId { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
