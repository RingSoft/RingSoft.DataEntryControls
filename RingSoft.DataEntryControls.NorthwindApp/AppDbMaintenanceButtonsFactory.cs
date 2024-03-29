﻿using System.Windows.Controls;
using RingSoft.DbLookup.App.WPFCore;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        public override Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            var result = new DbMaintenanceButtonsControl();
            var additionalButtons = new AdvancedFindAdditionalButtonsControl();
            result.AdditionalButtonsPanel.Children.Add(additionalButtons);
            additionalButtons.ImportDefaultLookupButton.Command = viewModel.ImportDefaultLookupCommand;
            additionalButtons.RefreshSettingsButton.Command = viewModel.RefreshSettingsCommand;
            additionalButtons.PrintLookupOutputButton.Command = viewModel.PrintLookupOutputCommand;
            result.UpdateLayout();
            return result;
        }

        public override Control GetRecordLockingButtonsControl(RecordLockingViewModel viewModel)
        {
            return new DbMaintenanceButtonsControl();
        }
    }
}
