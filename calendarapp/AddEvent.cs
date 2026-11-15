using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace calendarapp
{
    public partial class AddEvent : Form
    {

        public Event NewEvent { get;  set; }

        public AddEvent()
        {
            InitializeComponent();
        }

        // Pre-fills a new event for the date chosen in the mini calendar (09:00 - 10:00)
        public void SetDefaultDate(DateTime date)
        {
            dtpStart.Value = date.Date.AddHours(9);
            dtpEnd.Value = date.Date.AddHours(10);
        }

        public void LoadEventData(Event ev)
        {
            //Fill the UI controls with the existing data
            this.txtName.Text = ev.Name;
            this.dtpStart.Value = ev.StartDate;
            this.dtpEnd.Value = ev.EndDate;
            this.txtNote.Text = ev.Note;

            //Changes the button text so the user knows they are saving changes
            this.btnSave.Text = "Update Event";
            this.Text = "Edit Event"; // Changes the window title
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Checks the input before anything is saved, so the program does not crash
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name for the event.");
                return;
            }
            if (dtpEnd.Value <= dtpStart.Value)
            {
                MessageBox.Show("The event cannot end before it starts!");
                return;
            }

            if (NewEvent != null)
            {
                NewEvent.Name = txtName.Text;
                NewEvent.StartDate = dtpStart.Value;
                NewEvent.EndDate = dtpEnd.Value;
                NewEvent.Note = txtNote.Text;
            }
            else
            {
                // Only creates a NEW event if the user is adding
                NewEvent = new Event(txtName.Text, dtpStart.Value, dtpEnd.Value, txtNote.Text);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();

        }

    }
}
