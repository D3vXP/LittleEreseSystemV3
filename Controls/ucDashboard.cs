using System;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// The Dashboard page: five KPI cards on top, the month calendar on the left and the
    /// "Today" / "Selected Day" lists on the right.
    /// </summary>
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();
        }

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void dgvToday_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // TODO(logic prompt)
        }

        private void dgvDay_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // TODO(logic prompt)
        }

        private void dgvDay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // TODO(logic prompt)
        }

        private void btnOpenInAppointments_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }
    }
}
