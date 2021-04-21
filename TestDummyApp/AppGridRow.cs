using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;


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

        public TextComboBoxControlSetup LineTypeComboBoxSetup { get; } = new TextComboBoxControlSetup();

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
                    return new DataEntryGridCustomControlCellProps(this, columnId, (int)LineType,
                        ComboBoxValueChangedTypes.SelectedItemChanged);
                case AppGridColumns.CheckBox:
                    return new DataEntryGridCheckBoxCellProps(this, columnId, CheckBoxValue);
                case AppGridColumns.Date:
                    return new DataEntryGridDateCellProps(this, columnId, DateSetup, DateValue);
                case AppGridColumns.Integer:
                    return new DataEntryGridIntegerCellProps(this, columnId, IntegerSetup, IntegerValue);
                case AppGridColumns.Button:
                    return new DataEntryGridButtonCellProps(this, columnId);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void SetCellValue(DataEntryGridEditingCellProps value)
        {
            var appGridColumn = (AppGridColumns) value.ColumnId;
            switch (appGridColumn)
            {
                case AppGridColumns.LineType:
                    if (value is DataEntryGridCustomControlCellProps customControlCellProps)
                    {
                        var newLineType = (AppGridLineTypes)customControlCellProps.SelectedItemId;
                        var changeLineType = true;
                        if (!IsNew)
                        {
                            var message =
                                "Changing the line type will erase all the current row's data.  Do you wish to continue?";
                            if (!AppGridManager.UserInterface.ShowYesNoMessage(message, "Change Line Type"))
                            {
                                changeLineType = false;
                                customControlCellProps.OverrideCellMovement = true;
                                customControlCellProps.SelectedItemId = (int)LineType;
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
                    Manager.Grid.RefreshDataSource();
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
                case AppGridColumns.Button:
                    var cellProps = GetCellProps(AppGridManager.StockNumberColumnId);
                    AppGridManager.UserInterface.ShowGridMemoEditor(new DataEntryGridMemoValue(20){Text = cellProps.DataValue});
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
                    return GetDisabledCellStyle();
                case AppGridColumns.LineType:
                    return GetLineTypeCellStyle();
                case AppGridColumns.Button:
                    return GetButtonCellStyle();
                case AppGridColumns.CheckBox:
                    return GetCheckBoxCellStyle();
            }
            return base.GetCellStyle(columnId);
        }

        protected DataEntryGridControlCellStyle GetLineTypeCellStyle()
        {
            return new DataEntryGridControlCellStyle();
        }

        protected DataEntryGridCellStyle GetDisabledCellStyle()
        {
            return new() { State = DataEntryGridCellStates.Disabled };
        }

        protected DataEntryGridControlCellStyle GetCheckBoxCellStyle()
        {
            var state = IsNew ? DataEntryGridCellStates.Disabled : DataEntryGridCellStates.Enabled;

            return new DataEntryGridControlCellStyle { State = state };
        }

        protected DataEntryGridButtonCellStyle GetButtonCellStyle()
        {
            var state = IsNew ? DataEntryGridCellStates.Disabled : DataEntryGridCellStates.Enabled;

            return new DataEntryGridButtonCellStyle
            {
                State = state,
                Content = "Button"
            };
        }

        public virtual void LoadSale_DetailRow(SaleDetail saleDetail)
        {
        }
    }
}
