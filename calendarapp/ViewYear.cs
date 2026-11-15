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
    public partial class ViewYear : UserControl
    {
        public ViewYear()
        {
            InitializeComponent();
        }

        public void Populate(DateTime selectedDate, Manager manager)
        {
            List<Event> allEvents = manager.GetAllEvents();

            yearPnl.Controls.Clear();
            int year = selectedDate.Year;

            for (int month = 1; month <= 12; month++)
            {
                // Creates a container for the month
                Panel monthContainer = new Panel { Width = 250, Height = 250, Margin = new Padding(10) };

                // Adds the Month Name
                Label lblMonthName = new Label
                {
                    Text = new DateTime(year, month, 1).ToString("MMMM"),
                    Dock = DockStyle.Top,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                monthContainer.Controls.Add(lblMonthName);

                // Creates a tiny 7x6 grid for the days
                TableLayoutPanel miniGrid = new TableLayoutPanel
                {
                    ColumnCount = 7,
                    RowCount = 6,
                    Dock = DockStyle.Fill
                };

                // Sets columns to 14% each
                for (int i = 0; i < 7; i++) miniGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28f));

                FillMiniGrid(miniGrid, year, month, allEvents);

                monthContainer.Controls.Add(miniGrid);
                monthContainer.Controls.SetChildIndex(lblMonthName, 1); // Keep label at top

                yearPnl.Controls.Add(monthContainer);
            }
        }

        private void FillMiniGrid(TableLayoutPanel grid, int year, int month, List<Event> allEvents)
        {
            DateTime first = new DateTime(year, month, 1);
            int days = DateTime.DaysInMonth(year, month);
            int offset = (first.DayOfWeek == DayOfWeek.Sunday) ? 6 : (int)first.DayOfWeek - 1;

            for (int day = 1; day <= days; day++)
            {
                int col = (offset + day - 1) % 7;
                int row = (offset + day - 1) / 7;

                Label lblDay = new Label
                {
                    Text = day.ToString(),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 7)
                };

                // If there are events on this day, highlights it
                DateTime thisDay = new DateTime(year, month, day);
                if (allEvents.Any(e => e.StartDate.Date == thisDay.Date))
                {
                    lblDay.BackColor = Color.LightBlue; // Highlighted day
                    lblDay.ForeColor = Color.Black;
                }

                grid.Controls.Add(lblDay, col, row);
            }
        }
    }
}
