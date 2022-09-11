﻿using System.Windows.Controls;
using RingSoft.DbLookup.App.WPFCore;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class AppDbMaintenanceButtonsFactory : DbMaintenanceButtonsFactory
    {
        public override Control GetButtonsControl()
        {
            return new DbMaintenanceButtonsControl();
        }

        public override Control GetAdvancedFindButtonsControl(AdvancedFindViewModel viewModel)
        {
            var result = new DbMaintenanceButtonsControl();
            var additionalButtons = new AdvancedFindAdditionalButtonsControl();
            result.AdditionalButtonsPanel.Children.Add(additionalButtons);
            additionalButtons.ImportDefaultLookupButton.Command = viewModel.ImportDefaultLookupCommand;
            additionalButtons.ApplyToLookupButton.Command = viewModel.ApplyToLookupCommand;
            additionalButtons.SqlViewerButton.Command = viewModel.ShowSqlCommand;
            additionalButtons.RefreshSettingsButton.Command = viewModel.RefreshSettingsCommand;
            result.UpdateLayout();
            return result;
        }
    }
}