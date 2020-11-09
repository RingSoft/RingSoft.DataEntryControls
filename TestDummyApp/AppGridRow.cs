using System;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;


namespace TestDummyApp
{
    public abstract class AppGridRow : DataEntryGridRow
    {
        public abstract AppGridLineTypes LineType { get; }

        public bool CheckBoxValue { get; private set; }

        public DateTime? DateValue { get; private set; }

        public DateEditControlSetup DateSetup { get; }

        public int? IntegerValue { get; private set; }

        public IntegerEditControlSetup IntegerSetup { get; }

        public DataEntryComboBoxSetup LineTypeComboBoxSetup { get; } = new DataEntryComboBoxSetup();

        public AppGridManager AppGridManager { get; }

        public AppGridRow(AppGridManager manager) : base(manager)
        {
            AppGridManager = manager;
            LineTypeComboBoxSetup.LoadFromEnum<AppGridLineTypes>();

            DateSetup = new DateEditControlSetup{AllowNullValue = true};
            IntegerSetup = new IntegerEditControlSetup{AllowNullValue = true};
        }

        public override DataEntryGridCellProps GetCellProps(int columnId)
        {
            var appGridColumn = (AppGridColumns) columnId;
            switch (appGridColumn)
            {
                case AppGridColumns.Disabled:
                    return new DataEntryGridTextCellProps(this, columnId);
                case AppGridColumns.LineType:
                    var selectedLineType = LineTypeComboBoxSetup.GetItem((int) LineType);
                    return new DataEntryGridComboBoxCellProps(this, columnId, LineTypeComboBoxSetup, selectedLineType,
                        ComboBoxValueChangedTypes.SelectedItemChanged);
                case AppGridColumns.CheckBox:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, CheckBoxValue);
                case AppGridColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, DateSetup, DateValue);
                case AppGridColumns.Integer:
                    return new DataEntryGridIntegerCellProps(this, columnId, IntegerSetup, IntegerValue);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridCellProps value)
        {
            var appGridColumn = (AppGridColumns) value.ColumnId;
            switch (appGridColumn)
            {
                case AppGridColumns.LineType:
                    if (value is DataEntryGridComboBoxCellProps comboBoxCellProps)
                    {
                        var newLineType = (AppGridLineTypes) comboBoxCellProps.SelectedItem.NumericValue;
                        var changeLineType = true;
                        if (!IsNew)
                        {
                            var message =
                                "Changing the line type will erase all the current row's data.  Do you wish to continue?";
                            if (!AppGridManager.UserInterface.ShowYesNoMessage(message, "Change Line Type"))
                            {
                                changeLineType = false;
                                comboBoxCellProps.OverrideCellMovement = true;
                                comboBoxCellProps.SelectedItem =
                                    comboBoxCellProps.ComboBoxSetup.GetItem((int) LineType);
                            }

                        }
                        
                        if (changeLineType)
                        {
                            var newRow =
                                AppGridManager.GetNewAppGridRow(newLineType);
                            AppGridManager.ReplaceRow(this, newRow);
                            newRow.IsNew = true;
                        }
                    }
                    break;
                case AppGridColumns.StockNumber:
                    break;
                case AppGridColumns.Location:
                    break;
                case AppGridColumns.CheckBox:
                    if (value is DataEntryGridCheckBoxCellProps checkBoxCellProps)
                        CheckBoxValue = checkBoxCellProps.Value;

                    break;
                case AppGridColumns.Price:
                    break;
                case AppGridColumns.Date:
                    if (value is DataEntryGridDateCellProps dateCellProps)
                        DateValue = dateCellProps.Value;
                    break;
                case AppGridColumns.Integer:
                    if (value is DataEntryGridIntegerCellProps integerCellProps)
                        IntegerValue = integerCellProps.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.SetCellValue(value);
        }

        public override DataEntryGridCellStyle GetCellStyle(int columnId)
        {
            var column = (AppGridColumns) columnId;
            switch (column)
            {
                case AppGridColumns.Disabled:
                    return new DataEntryGridCellStyle() { CellStyle = DataEntryGridCellStyles.Disabled };
            }
            return base.GetCellStyle(columnId);
        }

        public virtual void LoadSale_DetailRow(SaleDetail saleDetail)
        {
        }
    }
}
