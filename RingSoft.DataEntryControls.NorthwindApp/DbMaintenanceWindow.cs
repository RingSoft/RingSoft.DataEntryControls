﻿using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public abstract class DbMaintenanceWindow : BaseWindow, IDbMaintenanceView
    {
        public abstract DbMaintenanceViewModelBase ViewModel { get; }

        public abstract DbMaintenanceButtonsControl MaintenanceButtonsControl { get; }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;

        public AutoFillControl KeyAutoFillControl { get; private set; }

        public DbMaintenanceWindow()
        {
            EnterToTab = true;
        }

        protected void Initialize()
        {
            ShowInTaskbar = false;
            MaintenanceButtonsControl.Margin = new Thickness(0, 0, 0, 2.5);

            MaintenanceButtonsControl.PreviousButton.Command = ViewModel.PreviousCommand;
            MaintenanceButtonsControl.NewButton.Command = ViewModel.NewCommand;
            MaintenanceButtonsControl.SaveButton.Command = ViewModel.SaveCommand;
            MaintenanceButtonsControl.DeleteButton.Command = ViewModel.DeleteCommand;
            MaintenanceButtonsControl.FindButton.Command = ViewModel.FindCommand;
            MaintenanceButtonsControl.SelectButton.Command = ViewModel.SelectCommand;
            MaintenanceButtonsControl.NextButton.Command = ViewModel.NextCommand;
            MaintenanceButtonsControl.CloseButton.Click += (_, _) => CloseWindow();

            Loaded += (_, _) => ViewModel.OnViewLoaded(this);
            PreviewKeyDown += DbMaintenanceWindow_PreviewKeyDown;

            Closing += (_, args) => ViewModel.OnWindowClosing(args);
        }

        protected void RegisterFormKeyControl(AutoFillControl keyAutoFillControl)
        {
            KeyAutoFillControl = keyAutoFillControl;
            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.IsDirtyProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyValueDirty)),
                Mode = BindingMode.TwoWay
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.SetupProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillSetup))
            });

            BindingOperations.SetBinding(keyAutoFillControl, AutoFillControl.ValueProperty, new Binding
            {
                Source = ViewModel,
                Path = new PropertyPath(nameof(ViewModel.KeyAutoFillValue)),
                Mode = BindingMode.TwoWay
            });

            keyAutoFillControl.LostFocus += (_, _) => ViewModel.OnKeyControlLeave();
        }

        private void DbMaintenanceWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.Left:
                        ViewModel.OnGotoPreviousButton();
                        e.Handled = true;
                        break;
                    case Key.Right:
                        ViewModel.OnGotoNextButton();
                        e.Handled = true;
                        break;
                }
            }
        }

        public void InitializeFromLookupData(LookupAddViewArgs e)
        {
            ViewModel.InitializeFromLookupData(e);
        }

        public virtual void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        public virtual void ResetViewForNewRecord()
        {
        }

        public void OnRecordSelected()
        {
            if (FocusManager.GetFocusedElement(this) is TextBox textBox)
                textBox.SelectAll();
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            var lookupWindow =
                new LookupWindow(lookupDefinition, allowAdd, allowView, initialSearchFor)
                    {InitialSearchForPrimaryKeyValue = initialSearchForPrimaryKey};

            lookupWindow.LookupSelect += (_, args) =>
            {
                LookupFormReturn?.Invoke(this, args);
            };
            lookupWindow.Owner = this;
            lookupWindow.ShowDialog();
        }

        public void CloseWindow()
        {
            Close();
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            var result = ControlsGlobals.UserInterface.ShowYesNoCancelMessageBox(text, caption, playSound);
            switch (result)
            {
                case MessageBoxButtonsResult.Yes:
                    return MessageButtons.Yes;
                case MessageBoxButtonsResult.No:
                    return MessageButtons.No;
            }

            return MessageButtons.Cancel;
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            var result = ControlsGlobals.UserInterface.ShowYesNoMessageBox(text, caption, playSound);

            return result == MessageBoxButtonsResult.Yes;
        }

        public void ShowRecordSavedMessage()
        {
            MessageBox.Show("Record Saved!", "Record Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected override void OnReadOnlyModeSet(bool readOnlyValue)
        {
            if (readOnlyValue)
            {
                var focusedElement = FocusManager.GetFocusedElement(this);
                if (focusedElement == null || !focusedElement.IsEnabled)
                    MaintenanceButtonsControl.NextButton.Focus();
            }
            else if (MaintenanceButtonsControl.IsKeyboardFocusWithin)
            {
                WPFControlsGlobals.SendKey(Key.Tab);
            }

            base.OnReadOnlyModeSet(readOnlyValue);
        }

        public override void SetControlReadOnlyMode(Control control, bool readOnlyValue)
        {
            if (control == KeyAutoFillControl)
            {
                return;
            }
            base.SetControlReadOnlyMode(control, readOnlyValue);
        }
    }
}
