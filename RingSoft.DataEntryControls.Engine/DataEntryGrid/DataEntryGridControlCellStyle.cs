namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridCheckBoxCellStyle : DataEntryGridControlCellStyle
    {
    }

    public class DataEntryGridButtonCellStyle : DataEntryGridControlCellStyle
    {
    }

    public class DataEntryGridControlCellStyle : DataEntryGridCellStyle
    {
        private bool _controlVisible = true;

        public bool ControlVisible
        {
            get => _controlVisible;
            set
            {
                if (_controlVisible == value)
                    return;

                _controlVisible = value;

                if (!_controlVisible && CellStyle == DataEntryGridCellStyles.Enabled)
                    CellStyle = DataEntryGridCellStyles.ReadOnly;
            }
        }
    }
}
