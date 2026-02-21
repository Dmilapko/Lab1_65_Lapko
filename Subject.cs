using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_65_Lapko
{
    public class Subject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int EctsCredits { get; set; }
        public KnowledgeArea Area { get; set; }

        public List<Session> Sessions { get; set; } = new List<Session>();

        public TimeSpan TotalDuration
        {
            get
            {
                TimeSpan total = TimeSpan.Zero;
                if (Sessions != null)
                {
                    foreach (var s in Sessions)
                    {
                        total += s.Duration;
                    }
                }
                return total;
            }
        }
    }
}
