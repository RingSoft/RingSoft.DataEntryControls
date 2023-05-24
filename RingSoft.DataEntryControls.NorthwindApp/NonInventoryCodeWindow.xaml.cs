﻿using RingSoft.DataEntryControls.NorthwindApp.Library;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System.Windows.Controls.Primitives;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    /// <summary>
    /// Interaction logic for NonInventoryCodeWindow.xaml
    /// </summary>
    public partial class NonInventoryCodeWindow
    {
        public override DbMaintenanceViewModelBase ViewModel => NonInventoryCodeViewModel;
        public override DbMaintenanceButtonsControl MaintenanceButtonsControl => ButtonsControl;
        public override DbMaintenanceStatusBar DbStatusBar => StatusBar;

        public NonInventoryCodeWindow()
        {
            InitializeComponent();

            Initialize();
            RegisterFormKeyControl(DescriptionControl);
        }

        public override void ResetViewForNewRecord()
        {
            DescriptionControl.Focus();
            base.ResetViewForNewRecord();
        }

        public override void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            if (fieldDefinition == AppGlobals.LookupContext.NonInventoryCodes.GetFieldDefinition(p => p.Description))
                DescriptionControl.Focus();

            base.OnValidationFail(fieldDefinition, text, caption);
        }
    }
}
