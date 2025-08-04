using System.Windows.Forms;

namespace PrivilegeUI.MyControls
{
    class DoubleBufferedDataGridView : DataGridView
    {
        public DoubleBufferedDataGridView()
        {
            DoubleBuffered = true;
        }
    }
}
