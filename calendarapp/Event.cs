using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calendarapp
{
    public class Event
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Note { get; set; }
        public bool IsDone { get; set; } = false;

        public Event(string name, DateTime startDate, DateTime endDate, string note)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Note = note;
        }

        public override string ToString()
        {
            return $"Event {Name}\n-------------------------\n{StartDate:yyyy-MM-dd HH:mm} - {EndDate:yyyy-MM-dd HH:mm}\n-------------------------\n({Note})";
        }
    }
}
