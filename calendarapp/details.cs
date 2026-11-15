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
    public partial class details : Form
    {
        private Event _currentEvent;
        public details(Event ev)
        {
            InitializeComponent();

            // This fills the labels made with the data
            lblName.Text = ev.Name;
            lblStart.Text = ev.StartDate.ToString("dd/MM/yyyy HH:mm");
            lblEnd.Text = ev.EndDate.ToString("dd/MM/yyyy HH:mm");
            lblNote.Text = ev.Note;
            _currentEvent = ev;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Flip the hidden switch to true
            _currentEvent.IsDone = true;

            // Gives the user a visual confirmation
            MessageBox.Show("Event marked as finished.");

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this?",
                                 "Confirm Delete", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Abort; // We use Abort as a signal for 'Delete'
                this.Close();
            }
        }
    }
}
