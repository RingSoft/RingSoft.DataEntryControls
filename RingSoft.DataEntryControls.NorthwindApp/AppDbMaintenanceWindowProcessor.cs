﻿using System.Windows.Controls;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbMaintenance;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class AppDbMaintenanceWindowProcessor : DbMaintenanceWindowProcessor
    {
        public override Button SaveButton => _dbMaintenanceButtonsControl.SaveButton;
        public override Button SelectButton => _dbMaintenanceButtonsControl.SelectButton;
        public override Button DeleteButton => _dbMaintenanceButtonsControl.DeleteButton;
        public override Button FindButton => _dbMaintenanceButtonsControl.FindButton;
        public override Button NewButton => _dbMaintenanceButtonsControl.NewButton;
        public override Button CloseButton => _dbMaintenanceButtonsControl.CloseButton;
        public override Button NextButton => _dbMaintenanceButtonsControl.NextButton;
        public override Button PreviousButton => _dbMaintenanceButtonsControl.PreviousButton;
        public override Button PrintButton => _dbMaintenanceButtonsControl.PrintButton;

        private DbMaintenanceButtonsControl _dbMaintenanceButtonsControl;

        public override void Initialize(BaseWindow window, Control buttonsControl,
            DbMaintenanceViewModelBase viewModel, IDbMaintenanceView view, DbMaintenanceStatusBar statusBar)
        {
            _dbMaintenanceButtonsControl = (DbMaintenanceButtonsControl) buttonsControl;
            base.Initialize(window, buttonsControl, viewModel, view, statusBar);
        }
    }
}
