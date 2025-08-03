using System.Windows.Forms;

namespace Privilege.UI.MyControls
{
    class DoubleBufferedDataGridView : DataGridView
    {
        public DoubleBufferedDataGridView()
        {
            DoubleBuffered = true;
        }
    }
}
