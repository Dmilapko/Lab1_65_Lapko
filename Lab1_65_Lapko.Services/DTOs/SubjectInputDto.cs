namespace Lab1_65_Lapko.Services.DTOs
{
    /// <summary>
    /// Input DTO used for both creating and updating a Subject.
    /// Area is a string to keep UI decoupled from the storage enum.
    /// </summary>
    public class SubjectInputDto
    {
        public string Name { get; set; } = string.Empty;
        public int EctsCredits { get; set; }
        public string Area { get; set; } = string.Empty;
    }
}
