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

            SaveButton.ToolTip.HeaderText = "Save Record (Ctrl + R, Ctrl + S)";
            SaveButton.ToolTip.DescriptionText = "Save this record to the database.";

            DeleteButton.ToolTip.HeaderText = "Delete Record (Ctrl + R, Ctrl + D)";
            DeleteButton.ToolTip.DescriptionText = "Delete this record from the database.";
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
        }
    }
}
