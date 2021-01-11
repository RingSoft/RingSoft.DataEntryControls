using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;

namespace TestDummyApp
{
    public class CustomContent
    {
        public int Id { get; set; }

        public ContentPresenter Content { get; set; }
    }

    public static class Globals
    {
        public const int CommentDisplayStyleId = 100;
        public const int NonInventoryDisplayStyleId = 101;

        public const int LineTypeControlId = 100;

        public static string CultureId { get; set; }

        public static int Precision { get; set; }

        public static ObservableCollection<CustomContent> GetLineTypeContents()
        {
            var lineTypeContents = new ObservableCollection<CustomContent>();

            var contentPresenter = new ContentPresenter { ContentTemplate = Application.Current.Resources["InventoryPanel"] as DataTemplate };
            var content = new CustomContent { Id = (int)AppGridLineTypes.Inventory, Content = contentPresenter };
            lineTypeContents.Add(content);

            contentPresenter = new ContentPresenter { ContentTemplate = Application.Current.Resources["NonInventoryPanel"] as DataTemplate };
            content = new CustomContent { Id = (int)AppGridLineTypes.NonInventory, Content = contentPresenter };
            lineTypeContents.Add(content);

            contentPresenter = new ContentPresenter { ContentTemplate = Application.Current.Resources["CommentPanel"] as DataTemplate };
            content = new CustomContent { Id = (int)AppGridLineTypes.Comment, Content = contentPresenter };
            lineTypeContents.Add(content);

            return lineTypeContents;
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
