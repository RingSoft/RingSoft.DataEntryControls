using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for LineTypeControl.xaml
    /// </summary>
    public partial class LineTypeControl
    {

        public AppGridLineTypes LineType
        {
            get => (AppGridLineTypes) GetSelectedItemId();
            set => SelectItem((int)value);
        }

        public ObservableCollection<CustomContent> Content { get; private set; }

        private bool _controlLoaded;

        public LineTypeControl()
        {
            InitializeComponent();

            Loaded += (sender, args) => OnLoaded();
        }

        private void OnLoaded()
        {
            Content = GetCustomContent();

            ComboBox.ItemsSource = Content;

            _controlLoaded = true;
        }

        protected virtual ObservableCollection<CustomContent> GetCustomContent() => Globals.GetLineTypeContents();

        //protected virtual int GetCurrentId() => (int)LineType;

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded)
                return;

            var selectedItem = Content.FirstOrDefault(f => f.Id == itemId);

            ComboBox.SelectedItem = selectedItem;
        }

        protected int GetSelectedItemId()
        {
            if (ComboBox.SelectedItem is CustomContent customContent)
                return customContent.Id;

            return 0;
        }

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    switch (e.Key)
        //    {
        //        case Key.I:
        //            InventoryItem.IsSelected = true;
        //            break;
        //        case Key.N:
        //            NonInventoryItem.IsSelected = true;
        //            break;
        //        case Key.C:
        //            CommentItem.IsSelected = true;
        //            break;
        //    }
        //    base.OnKeyDown(e);
        //}
    }
}
