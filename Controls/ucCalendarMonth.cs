using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LittleEreseSystemV3.Controls
{
    /// <summary>
    /// A read-only month calendar used by the Dashboard. The coloured day cells are created at
    /// runtime (they come from the data), so tlpDays stays empty in the Designer.
    /// </summary>
    public partial class ucCalendarMonth : UserControl
    {
        public ucCalendarMonth()
        {
            InitializeComponent();
        }

        private void btnPrevMonth_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }

        private void btnTodayMonth_Click(object sender, EventArgs e)
        {
            // TODO(logic prompt)
        }
    }
}
