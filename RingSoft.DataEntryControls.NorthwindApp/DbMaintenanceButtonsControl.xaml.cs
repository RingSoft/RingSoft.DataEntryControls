using RingSoft.DataEntryControls.WPF;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for DbMaintenanceButtonsControl.xaml
    /// </summary>
    public partial class DbMaintenanceButtonsControl : IReadOnlyControl
    {
        public DbMaintenanceButtonsControl()
        {
            InitializeComponent();
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
        }
    }
}
