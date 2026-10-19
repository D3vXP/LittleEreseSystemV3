using System;
using System.Windows.Forms;
using LittleEreseSystemV3.Data;

namespace LittleEreseSystemV3.Helpers
{
    /// <summary>
    /// Small helpers for DataGridView selection.
    /// WHY: setting CurrentCell to a cell of a HIDDEN column (for example the id column)
    /// throws InvalidOperationException ("Current cell cannot be set to an invisible cell"),
    /// so every screen selects rows through these two methods instead.
    /// </summary>
    internal static class GridHelper
    {
        /// <summary>
        /// Selects the row whose id column (<paramref name="idColumnName"/>) equals
        /// <paramref name="id"/> and puts the current cell in the first VISIBLE column of that row.
        /// Returns false when the id is not found or the grid has no visible column.
        /// Never throws.
        /// </summary>
        public static bool SelectRowById(DataGridView grid, string idColumnName, int id)
        {
            if (grid == null || string.IsNullOrEmpty(idColumnName) || id <= 0) return false;

            // A cell can only become the CurrentCell when its column is visible,
            // so bail out early instead of letting the grid throw.
            DataGridViewColumn firstVisible =
                grid.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (firstVisible == null) return false;

            DataGridViewRow target = null;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row == null) continue;

                DataGridViewCell cell = row.Cells[idColumnName];
                if (cell == null) continue;

                int rowId = Db.Get<int>(cell.Value);
                if (rowId == id)
                {
                    target = row;
                    break;
                }
            }

            if (target == null) return false;

            grid.ClearSelection();
            target.Selected = true;
            grid.CurrentCell = target.Cells[firstVisible.Index];

            // Scrolling can fail when the row is neither visible nor ready yet; it is only cosmetic.
            try
            {
                grid.FirstDisplayedScrollingRowIndex = target.Index;
            }
            catch (InvalidOperationException)
            {
                // Ignore: the row is already on screen or the grid is still laying itself out.
            }

            return true;
        }

        /// <summary>
        /// Returns the id of the selected (or current) row, or 0 when nothing is selected.
        /// Never throws.
        /// </summary>
        public static int GetSelectedId(DataGridView grid, string idColumnName)
        {
            if (grid == null || string.IsNullOrEmpty(idColumnName)) return 0;

            DataGridViewRow row = grid.CurrentRow;
            if (row == null) return 0;

            DataGridViewCell cell = row.Cells[idColumnName];
            if (cell == null) return 0;

            return Db.Get<int>(cell.Value);
        }
    }
}
