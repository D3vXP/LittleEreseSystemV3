using System;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Controls
{
    public partial class ucActivityLog : UserControl
    {
        public ucActivityLog()
        {
            InitializeComponent();
        }

        private void ucActivityLog_Load(object sender, EventArgs e)
        {
            // TODO(logic prompt): load the activity log for the selected range.
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt): reload the grid with the chosen filters.
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt): export the visible log rows to CSV.
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO(logic prompt): run the search when Enter is pressed.
        }
    }
}
