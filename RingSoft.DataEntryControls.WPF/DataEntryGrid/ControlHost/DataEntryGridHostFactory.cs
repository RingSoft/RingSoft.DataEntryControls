using System;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost
{
    public class DataEntryGridHostFactory
    {
        public virtual DataEntryGridControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {

            if (editingControlHostId == DataEntryGridCellProps.TextBoxHostId)
                return new DataEntryGridTextBoxHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.ComboBoxHostId)
                return new DataEntryGridComboBoxHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.CheckBoxHostId)
                return new DataEntryGridCheckBoxHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.ButtonHostId)
                return new DataEntryGridButtonHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.DecimalEditHostId)
                return new DataEntryGridDecimalControlHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.DateEditHostId)
                return new DataEntryGridDateHost(grid);
            if (editingControlHostId == DataEntryGridCellProps.IntegerEditHostId)
                return new DataEntryGridIntegerControlHost(grid);

            throw new ArgumentException($"Data Entry Grid Control Host not found for ID: {editingControlHostId}");
        }
    }
}
