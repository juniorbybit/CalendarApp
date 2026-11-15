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
    public partial class ViewDay : UserControl
    {
        // The view keeps the SAME Manager as the main window, so delete and "completed" work on the real list
        private Manager manager;
        private DateTime currentDate;

        public ViewDay()
        {
            InitializeComponent();
        }
        public void Populate(DateTime targetDate, Manager manager)
        {
            this.manager = manager;
            this.currentDate = targetDate;
            List<Event> allEvents = manager.GetAllEvents();

            dayPnl.Controls.Clear();
            dayPnl.RowCount = 24;

            // Creates the 24-hour background first
            for (int i = 0; i < 24; i++)
            {
                dayPnl.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));

                Label lblHour = new Label
                {
                    Text = $"{i:D2}:00",
                    TextAlign = ContentAlignment.TopRight,
                    Dock = DockStyle.Fill
                };
                dayPnl.Controls.Add(lblHour, 0, i);
            }

            // Filters events for today
            var todaysEvents = allEvents.Where(e => e.StartDate.Date == targetDate.Date).ToList();

            foreach (var ev in todaysEvents)
            {

                int startHour = ev.StartDate.Hour;
                int endHour = ev.EndDate.Hour;

                // Calculates how many rows to stretch (minimum 1, and never past midnight)
                int duration = endHour - startHour;
                if (duration < 1) duration = 1;
                if (duration > 24 - startHour) duration = 24 - startHour;

                // Creates the "Stretched" Button
                Button btnEvent = new Button
                {
                    Text = $"{ev.Name}\n{ev.StartDate:HH:mm} - {ev.EndDate:HH:mm}",
                    BackColor = Color.FromArgb(200, 173, 216, 230), // Light blue with transparency
                    FlatStyle = FlatStyle.Flat,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.TopLeft
                };
                if (ev.IsDone)
                {
                    btnEvent.BackColor = Color.LightGray; // The grey look
                    btnEvent.ForeColor = Color.DimGray;  // Faded text
                }
                else
                {
                    btnEvent.BackColor = Color.LightBlue; // The "active" look
                    btnEvent.ForeColor = Color.Black;
                }

                // Adds to the table (Column 1, Row startHour)
                dayPnl.Controls.Add(btnEvent, 1, startHour);

                // Applies the stretch
                dayPnl.SetRowSpan(btnEvent, duration);
                btnEvent.Text = ev.IsDone ? "✓ " + ev.Name : ev.Name;

                btnEvent.Tag = ev;

                // Tells the button to open the details window when clicked
                btnEvent.Click += (sender, args) => {
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
