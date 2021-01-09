using System;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.Engine.DataEntryGrid;

// ReSharper disable once CheckNamespace
namespace RingSoft.DataEntryControls.WPF.DataEntryGrid
{
    public class ControlValueChangedArgs
    {
        public string ControlValue { get; }

        public ControlValueChangedArgs(string controlValue)
        {
            ControlValue = controlValue;
        }
    }

    public class DataEntryGridControlColumnProcessor
    {
        public DataEntryGridDisplayStyle DisplayStyle { get; private set; } = new DataEntryGridDisplayStyle();

        public event EventHandler<ControlValueChangedArgs> ControlValueChanged;

        private Control _control;

        public DataEntryGridControlColumnProcessor(Control control)
        {
            _control = control;

            //This is to avoid random showing when grid is being scrolled and DataEntryControlCellValue sets IsVisible to false.
            _control.Visibility = Visibility.Collapsed;
        }

        public void SetDataValue(string dataValue, string trace = "")
        {
            var dataValueObj = new DataEntryGridDataValue(dataValue);
            
            _control.Visibility = dataValueObj.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            _control.IsEnabled = dataValueObj.IsEnabled;

            if (dataValueObj.DisplayStyleId > 0)
            {
                var dataEntryGrid = _control.GetParentOfType<DataEntryGrid>();
                if (dataEntryGrid != null)
                    DisplayStyle = dataEntryGrid.GetDisplayStyle(dataValueObj.DisplayStyleId);
            }

            if (!dataValueObj.ControlValue.IsNullOrEmpty())
                ControlValueChanged?.Invoke(this, new ControlValueChangedArgs(dataValueObj.ControlValue));
        }
    }
}
