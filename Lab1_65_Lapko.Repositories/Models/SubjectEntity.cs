namespace Lab1_65_Lapko.Repositories.Models
{
    public class SubjectEntity
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea Area { get; set; }
    }
}
