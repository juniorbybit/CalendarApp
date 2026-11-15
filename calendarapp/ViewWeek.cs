using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace calendarapp
{
    public partial class ViewWeek : UserControl
    {
        // The view keeps the SAME Manager as the main window, so delete and "completed" work on the real list
        private Manager manager;
        private DateTime currentDate;

        public ViewWeek()
        {
            InitializeComponent();
        }
        public void Populate(DateTime selectedDate, Manager manager)
        {
            this.manager = manager;
            this.currentDate = selectedDate;
            List<Event> allEvents = manager.GetAllEvents();

            // Clear both tables (the main table was never cleared before, so labels and events piled up)
            headerTable.Controls.Clear();
            weekPnl.Controls.Clear();

            // Calculates Monday of this week
            int diff = (7 + (selectedDate.DayOfWeek - DayOfWeek.Monday)) % 7;
            DateTime startOfWeek = selectedDate.AddDays(-1 * diff).Date;

            // Fills the Header (Column 0 is empty for the hour space)
            headerTable.Controls.Add(new Label { Text = "Time", Font = new Font("Segoe UI", 9, FontStyle.Bold) }, 0, 0);

            string[] dayNames = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

            for (int i = 0; i < 7; i++)
            {
                DateTime currentDay = startOfWeek.AddDays(i);
                string headerText = $"{dayNames[i]} {currentDay:dd/MM}";

                Label lblHeader = new Label
                {
                    Text = headerText,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                };
                headerTable.Controls.Add(lblHeader, i + 1, 0);
            }

            // Adds the Hour Labels (Column 0)
            for (int hour = 0; hour < 24; hour++)
            {
                weekPnl .Controls.Add(new Label { Text = $"{hour:D2}:00", TextAlign = ContentAlignment.TopRight }, 0, hour);
            }

            // Loops through each of the 7 days
            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                DateTime currentDay = startOfWeek.AddDays(dayIndex);
                var daysEvents = allEvents.Where(e => e.StartDate.Date == currentDay.Date).ToList();

                foreach (var ev in daysEvents)
                {
                    int startHour = ev.StartDate.Hour;
                    int duration = Math.Max(1, ev.EndDate.Hour - startHour);
                    if (duration > 24 - startHour) duration = 24 - startHour; // never past midnight

                    Button btn = new Button
                    {
                        Text = $"{ev.Name}\n{ev.StartDate:HH:mm}",
                        BackColor = Color.LightSkyBlue,
                        FlatStyle = FlatStyle.Flat,
                        Dock = DockStyle.Fill,
                        Margin = new Padding(2)
                    };
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

                    weekPnl.Controls.Add(btn, dayIndex + 1, startHour);
                    weekPnl.SetRowSpan(btn, duration);
                    // Puts the full event in the button's "pocket"
                    btn.Tag = ev;

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
            }
        }
    }
}
