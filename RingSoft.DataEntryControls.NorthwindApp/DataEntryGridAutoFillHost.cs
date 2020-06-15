using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using RingSoft.DataEntryControls.Engine.DataEntryGrid.CellProps;
using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DataEntryControls.WPF.DataEntryGrid;
using RingSoft.DataEntryControls.WPF.DataEntryGrid.ControlHost;
using RingSoft.DbLookup.AutoFill;
using RingSoft.DbLookup.Controls.WPF;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class DataEntryGridAutoFillHost : DataEntryGridControlHost<AutoFillControl>
    {
        private AutoFillSetup _autoFillSetup;

        public AutoFillSetup AutoFillSetup
        {
            get => _autoFillSetup;
            set
            {
                if (_autoFillSetup == value)
                    return;

                _autoFillSetup = value;
                OnPropertyChanged(nameof(AutoFillSetup));
            }
        }

        private AutoFillValue _autoFillValue;

        public AutoFillValue AutoFillValue
        {
            get => _autoFillValue;
            set
            {
                if (_autoFillValue == value)
                    return;

                _autoFillValue = value;
                OnPropertyChanged(nameof(AutoFillValue));
            }
        }

        public DataEntryGridAutoFillCellProps AutoFillCellProps { get; private set; }

        public AutoFillControl AutoFillControl { get; private set; }

        public DataEntryGridAutoFillHost(DataEntryGrid grid) : base(grid)
        {
        }

        public override DataEntryGridCellProps GetCellValue()
        {
            return new DataEntryGridAutoFillCellProps(AutoFillCellProps.Row, AutoFillCellProps.ColumnId,
                AutoFillControl.Setup, AutoFillControl.Value);
        }

        protected override void SetupFrameworkElementFactory(FrameworkElementFactory factory)
        {
            //var binding = new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(nameof(AutoFillSetup)),
            //    Mode = BindingMode.TwoWay,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //};
            //factory.SetBinding(AutoFillControl.SetupProperty, binding);

            //binding = new Binding()
            //{
            //    Source = this,
            //    Path = new PropertyPath(nameof(AutoFillValue)),
            //    Mode = BindingMode.TwoWay,
            //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            //};
            //factory.SetBinding(AutoFillControl.ValueProperty, binding);

            base.SetupFrameworkElementFactory(factory);
        }

        public override bool HasDataChanged()
        {
            if (AutoFillControl.Value == null && AutoFillCellProps.AutoFillValue != null)
                return true;

            if (AutoFillControl.Value != null && AutoFillCellProps.AutoFillValue == null)
                return true;

            if (AutoFillControl.Value == null && AutoFillCellProps.AutoFillValue == null)
                return false;

            if (AutoFillCellProps.AutoFillValue != null && AutoFillControl.Value != null)
            {
                return !AutoFillControl.Value.PrimaryKeyValue.IsEqualTo(AutoFillCellProps.AutoFillValue
                    .PrimaryKeyValue);
            }

            return false;
        }

        protected override void OnControlLoaded(AutoFillControl control, DataEntryGridCellProps cellProps)
        {
            AutoFillControl = control;
            AutoFillCellProps = (DataEntryGridAutoFillCellProps)cellProps;
            AutoFillControl.Setup = AutoFillCellProps.AutoFillSetup;
            AutoFillControl.Value = AutoFillCellProps.AutoFillValue;


            AutoFillControl.ControlDirty += (sender, args) => OnControlDirty();
        }
    }
}
