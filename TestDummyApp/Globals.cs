using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

namespace TestDummyApp
{
    public static class Globals
    {
        public const int CommentDisplayStyleId = 100;
        public const int NonInventoryDisplayStyleId = 101;

        public const int LineTypeControlId = 100;

        public static string CultureId { get; set; }

        public static int Precision { get; set; }

        public static ObservableCollection<CustomContentItem> GetLineTypeContents()
        {
            var lineTypeContents = new ObservableCollection<CustomContentItem>();

            var content = new CustomContentItem
            {
                Id = (int) AppGridLineTypes.Inventory,
                DataTemplate = GetDataTemplateForLineType(AppGridLineTypes.Inventory)
            };
            lineTypeContents.Add(content);

            content = new CustomContentItem
            {
                Id = (int) AppGridLineTypes.NonInventory,
                DataTemplate = GetDataTemplateForLineType(AppGridLineTypes.NonInventory)
            };
            lineTypeContents.Add(content);

            content = new CustomContentItem
            {
                Id = (int) AppGridLineTypes.Comment, 
                DataTemplate = GetDataTemplateForLineType(AppGridLineTypes.Comment)
            };
            lineTypeContents.Add(content);

            return lineTypeContents;
        }

        public static DataTemplate GetDataTemplateForLineType(AppGridLineTypes lineType)
        {
            switch (lineType)
            {
                case AppGridLineTypes.NonInventory:
                    return Application.Current.Resources["NonInventoryPanel"] as DataTemplate;
                case AppGridLineTypes.Comment:
                    return Application.Current.Resources["CommentPanel"] as DataTemplate;
            }
            return Application.Current.Resources["InventoryPanel"] as DataTemplate;
        }

        public static DecimalEditControlSetup GetNumericEditSetup()
        {
            return new DecimalEditControlSetup()
            {
                CultureId = CultureId,
                Precision = Precision
            };
        }
    }
}
