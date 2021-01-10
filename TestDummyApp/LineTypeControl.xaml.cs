using System;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for LineTypeControl.xaml
    /// </summary>
    public partial class LineTypeControl
    {
        public AppGridLineTypes LineType
        {
            get
            {
                if (Equals(ComboBox.SelectedItem, NonInventoryItem))
                    return AppGridLineTypes.NonInventory;
                if (Equals(ComboBox.SelectedItem, CommentItem))
                    return AppGridLineTypes.Comment;

                return AppGridLineTypes.Inventory;
            }
            set
            {
                switch (value)
                {
                    case AppGridLineTypes.Inventory:
                        InventoryItem.IsSelected = true;
                        break;
                    case AppGridLineTypes.NonInventory:
                        NonInventoryItem.IsSelected = true;
                        break;
                    case AppGridLineTypes.Comment:
                        CommentItem.IsSelected = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }
        public LineTypeControl()
        {
            InitializeComponent();
        }
    }
}
