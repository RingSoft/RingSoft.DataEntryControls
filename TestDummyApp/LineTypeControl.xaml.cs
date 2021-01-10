using System;
using System.Windows.Input;

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.I:
                    InventoryItem.IsSelected = true;
                    break;
                case Key.N:
                    NonInventoryItem.IsSelected = true;
                    break;
                case Key.C:
                    CommentItem.IsSelected = true;
                    break;
            }
            base.OnKeyDown(e);
        }
    }
}
