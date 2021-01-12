using RingSoft.DataEntryControls.Engine.DataEntryGrid;
using System;

namespace RingSoft.DataEntryControls.WPF.DataEntryGrid.EditingControlHost
{
    public class DataEntryGridHostFactory
    {
        public virtual DataEntryGridEditingControlHostBase GetControlHost(DataEntryGrid grid, int editingControlHostId)
        {

            if (editingControlHostId == DataEntryGridEditingCellProps.TextBoxHostId)
                return new DataEntryGridTextBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ComboBoxHostId)
                return new DataEntryGridTextComboBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.CheckBoxHostId)
                return new DataEntryGridCheckBoxHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ButtonHostId)
                return new DataEntryGridButtonHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.DecimalEditHostId)
                return new DataEntryGridDecimalControlHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.DateEditHostId)
                return new DataEntryGridDateHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.IntegerEditHostId)
                return new DataEntryGridIntegerControlHost(grid);
            if (editingControlHostId == DataEntryGridEditingCellProps.ContentControlHostId)
                return new DataEntryGridContentComboBoxControlHost(grid);

            throw new ArgumentException($"Data Entry Grid Control Host not found for ID: {editingControlHostId}");
        }
    }
}
