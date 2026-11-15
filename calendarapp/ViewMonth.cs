using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calendarapp
{
    public partial class ViewMonth : UserControl
    {
        // The view keeps the SAME Manager as the main window, so delete and "completed" work on the real list
        private Manager manager;
        private DateTime currentDate;

        public ViewMonth()
        {
            InitializeComponent();

        }
        public void Populate(DateTime selectedDate, Manager manager)
        {
            this.manager = manager;
            this.currentDate = selectedDate;
            List<Event> allEvents = manager.GetAllEvents();

            // Clears the old month visuals
            monthPnl.Controls.Clear();

            // Sets up the month math
            int year = selectedDate.Year;
            int month = selectedDate.Month;
            DateTime firstOfMonth = new DateTime(year, month, 1);
            int daysInMonth = DateTime.DaysInMonth(year, month);

            // Calculates Monday start offset
            int dayOfWeek = (int)firstOfMonth.DayOfWeek;
            int offset = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;

            // The Loop
            for (int day = 1; day <= daysInMonth; day++)
            {
                int column = (offset + day - 1) % 7;
                int row = (offset + day - 1) / 7;

                FlowLayoutPanel dayBox = new FlowLayoutPanel();
                dayBox.Dock = DockStyle.Fill;
                dayBox.FlowDirection = FlowDirection.TopDown;
                dayBox.BorderStyle = BorderStyle.FixedSingle;

                // Adds the day number
                dayBox.Controls.Add(new Label { Text = day.ToString(), AutoSize = true });

                // Filters the events for this specific day
                DateTime currentDay = new DateTime(year, month, day);
                var dailyEvents = allEvents.Where(e => e.StartDate.Date == currentDay.Date).ToList();

                foreach (var ev in dailyEvents)
                {
                    Button btn = new Button { Text = ev.Name, BackColor = Color.LightBlue, FlatStyle = FlatStyle.Flat };
                    dayBox.Controls.Add(btn);
                    // Puts the full event in the button's "pocket"
                    btn.Tag = ev;
                    if (ev.IsDone)
                    {
                        btn.BackColor = Color.LightGray; // The grey look
                        btn.ForeColor = Color.DimGray;  // Faded text
                        btn.Text = "✓ " + ev.Name;       // Add a checkmark if you like
                    }
                    else
                    {
                        btn.BackColor = Color.LightBlue; // The "active" look
                        btn.ForeColor = Color.Black;
                    }

                    // Tells the button to open the details window when clicked
                    btn.Click += (sender, args) => {
                        // Pulls the event back out
                        Button clickedBtn = (Button)sender;
                        Event selected = (Event)clickedBtn.Tag;

                        // Creates and shows the details window
                        details infoWindow = new details(selected);
                        var result = infoWindow.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            // Handle Completion (Refresh)
                            this.Populate(currentDate, this.manager);
                        }
                        else if (result == DialogResult.Abort)
                        {
                            // Handle Deletion
                            this.manager.DeleteEvent(selected);
                            this.Populate(currentDate, this.manager);
                        }
                    };
                }

                monthPnl.Controls.Add(dayBox, column, row);
            }
        }
    }
}
