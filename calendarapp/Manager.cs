using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace calendarapp
{
    public class Manager
    {
        private List<Event> events = new List<Event>();
        public void AddEvent(Event newEvent)
        {
            if (newEvent.EndDate <= newEvent.StartDate)
                throw new Exception("The event cannot end before it starts!");

            events.Add(newEvent);
            SortEvents();
        }

        // Keeps the list ordered by start time (also needed after an event was edited)
        public void SortEvents()
        {
            events.Sort((x, y) => x.StartDate.CompareTo(y.StartDate));
        }

        // Returns a copy of all events, so the views can read them without printing to the console
        public List<Event> GetAllEvents()
        {
            return events.ToList();
        }

        public Event FindEventByName(string name)
        {
            return events.FirstOrDefault(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public Guid? GetIdByName(string searchName)
        {
            var foundEvent = events.FirstOrDefault(e =>
                e.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase));

            return foundEvent?.Id;
        }
        public void DeleteEvent(Event ev)
        {
            events.Remove(ev);
        }
        public void EditEvent(Event oldEvent)
        {
            if (oldEvent == null)
            {
                Console.WriteLine("Event not found!");
                return;
            }
            int answer;
            do
            {
                Console.WriteLine(oldEvent.ToString());
                Console.WriteLine("What would you like to change? (1:Name, 2:Start, 3:End, 4:Note, 0:Exit)");

                if (!int.TryParse(Console.ReadLine(), out answer)) continue;

                switch (answer)
                {
                    case 1:
                        Console.Write("Enter new name: ");
                        oldEvent.Name = Console.ReadLine();
                        break;
                    case 2:
                        Console.Write("Enter new start date (yyyy-MM-dd HH:mm): ");
                        oldEvent.StartDate = DateTime.Parse(Console.ReadLine());
                        break;
                    case 3:
                        Console.Write("Enter new end date (yyyy-MM-dd HH:mm): ");
                        oldEvent.EndDate = DateTime.Parse(Console.ReadLine());
                        break;
                    case 4:
                        Console.Write("Enter new note: ");
                        oldEvent.Note = Console.ReadLine();
                        break;
                }
            } while (answer != 0);
        }

        public void MarkAsCompleted(Event e)
        {
            if (e != null && !e.Note.Contains("(Completed)"))
                e.Note += " (Completed)";
        }

        public List<Event> DisplayEvents()
        {
            if (events.Count == 0) Console.WriteLine("\nNo events found.");
            foreach (var e in events)
            {
                Console.WriteLine(e);
            }
            return events;
        }
        public List<Event> GetEventsByDay(DateTime date)
        {
            return events.Where(e => e.StartDate.Date == date.Date)
                         .OrderBy(e => e.StartDate).ToList();
        }
        public List<Event> GetEventsByMonth(int year, int month)
        {
            return events.Where(e => e.StartDate.Year == year && e.StartDate.Month == month)
                         .OrderBy(e => e.StartDate).ToList();
        }

        public List<Event> GetEventsByYear(int year)
        {
            return events.Where(e => e.StartDate.Year == year)
                         .OrderBy(e => e.StartDate).ToList();
        }
    }
}
