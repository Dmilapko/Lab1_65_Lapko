using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_65_Lapko
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Topic { get; set; }
        public SessionType Type { get; set; }

        public TimeSpan Duration => EndTime - StartTime;

        public override string ToString()
        {
            return $"{Type}: {Topic} ({Date} {StartTime}-{EndTime})";
        }
    }
}
