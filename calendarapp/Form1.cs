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
    public partial class Form1 : Form
    {

        // ONE manager for the whole app. All views receive this same object.
        Manager manager = new Manager();

        public Form1()
        {
            InitializeComponent();
        }

        // ---- Helpers that build a view, fill it and show it ----
        private void ShowDay(DateTime date)
        {
            ViewDay dayView = new ViewDay();
            dayView.Populate(date, manager);
            SwitchView(dayView);
        }

        private void ShowWeek(DateTime date)
        {
            ViewWeek weekView = new ViewWeek();
            weekView.Populate(date, manager);
            SwitchView(weekView);
        }

        private void ShowMonth(DateTime date)
        {
            ViewMonth monthView = new ViewMonth();
            monthView.Populate(date, manager);
            SwitchView(monthView);
        }

        private void ShowYear(DateTime date)
        {
            ViewYear yearView = new ViewYear();
            yearView.Populate(date, manager);
            SwitchView(yearView);
        }

        // Redraws whichever view is open (or the day view if nothing is open yet)
        private void RefreshCurrentView(DateTime date)
        {
            Control current = stage.Controls.Count > 0 ? stage.Controls[0] : null;

            if (current is ViewWeek) ShowWeek(date);
            else if (current is ViewMonth) ShowMonth(date);
            else if (current is ViewYear) ShowYear(date);
            else ShowDay(date);
        }

        private void SwitchView(UserControl newView) // To switch the view
        {
            stage.Controls.Clear(); // Removes the old view
            newView.Dock = DockStyle.Fill; // Makes it fill the panel
            stage.Controls.Add(newView); // Adds the new view
        }

        // ---- Buttons ----
        private void button1_Click(object sender, EventArgs e)// Day button
        {
            ShowDay(miniCal.SelectionStart);
        }

        private void button2_Click(object sender, EventArgs e) // Week view button
        {
            ShowWeek(miniCal.SelectionStart);
        }

        private void button3_Click(object sender, EventArgs e)// Month view button
        {
            ShowMonth(miniCal.SelectionStart);
        }

        private void button4_Click(object sender, EventArgs e)// Year view button
        {
            ShowYear(miniCal.SelectionStart);
        }

        private void button5_Click(object sender, EventArgs e)// Add button
        {
            using (AddEvent addForm = new AddEvent())
            {
                // The form starts on the date selected in the mini calendar.
                addForm.SetDefaultDate(miniCal.SelectionStart);

                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        manager.AddEvent(addForm.NewEvent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    // Jumps to the day of the new event and shows it
                    miniCal.SetDate(addForm.NewEvent.StartDate);
                    RefreshCurrentView(addForm.NewEvent.StartDate);
                }
            }
        }

        private void miniCal_DateSelected(object sender, DateRangeEventArgs e)// Changes the open view to the selected date, does nothing if no view is open
        {
            if (stage.Controls.Count > 0)
            {
                RefreshCurrentView(e.Start);
            }
        }

        private void button6_Click(object sender, EventArgs e)// Edit button
        {
            DateTime selectedDate = miniCal.SelectionStart.Date;

            // Finds all events that happen on that specific date
            List<Event> eventsOnThatDay = manager.GetEventsByDay(selectedDate);

            if (eventsOnThatDay.Count == 0)
            {
                MessageBox.Show("There are no events on this day to edit.");
                return;
            }

            // If there is more than one, the user chooses which one to edit
            Event eventToEdit = eventsOnThatDay.Count == 1
                ? eventsOnThatDay[0]
                : ChooseEvent(eventsOnThatDay);

            if (eventToEdit == null) return;

            using (AddEvent editForm = new AddEvent())
            {
                // Passes the existing event into the form
                editForm.LoadEventData(eventToEdit);

                // This connects the two files together
                editForm.NewEvent = eventToEdit;

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    manager.SortEvents(); // The start time may have changed
                    RefreshCurrentView(selectedDate);
                }
            }
        }

        // Small window with a list, so the user can pick one of several events
        private Event ChooseEvent(List<Event> candidates)
        {
            using (Form picker = new Form())
            using (ListBox list = new ListBox())
            using (Button ok = new Button())
            {
                picker.Text = "Which event do you want to edit?";
                picker.FormBorderStyle = FormBorderStyle.FixedDialog;
                picker.StartPosition = FormStartPosition.CenterParent;
                picker.MinimizeBox = false;
                picker.MaximizeBox = false;
                picker.ClientSize = new Size(320, 220);

                list.Dock = DockStyle.Top;
                list.Height = 170;
                foreach (Event ev in candidates)
                {
                    list.Items.Add($"{ev.StartDate:HH:mm}  {ev.Name}");
                }
                list.SelectedIndex = 0;

                ok.Text = "OK";
                ok.DialogResult = DialogResult.OK;
                ok.SetBounds(120, 180, 80, 30);

                picker.AcceptButton = ok;
                picker.Controls.Add(list);
                picker.Controls.Add(ok);

                if (picker.ShowDialog(this) == DialogResult.OK && list.SelectedIndex >= 0)
                {
                    return candidates[list.SelectedIndex];
                }
                return null;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
