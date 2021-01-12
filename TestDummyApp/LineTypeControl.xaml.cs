using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RingSoft.DataEntryControls.WPF;

namespace TestDummyApp
{
    /// <summary>
    /// Interaction logic for LineTypeControl.xaml
    /// </summary>
    public partial class LineTypeControl
    {

        private AppGridLineTypes _lineType;

        public AppGridLineTypes LineType
        {
            get => _lineType;
            set
            {
                if (_lineType == value)
                    return;

                _lineType = value;
                SelectItem((int)LineType);
            }
        }

        public ObservableCollection<DataEntryCustomContentTemplateItem> Content { get; private set; }

        private bool _controlLoaded;

        public LineTypeControl()
        {
            InitializeComponent();

            Loaded += (sender, args) => OnLoaded();

            ComboBox.SelectionChanged += (sender, args) => OnSelectionChanged();
        }

        private void OnLoaded()
        {
            Content = GetCustomContent();

            ComboBox.ItemsSource = Content;

            _controlLoaded = true;
            SelectItem(GetSelectedId());
        }

        private void OnSelectionChanged()
        {
            if (ComboBox.SelectedItem is DataEntryCustomContentTemplateItem customContent)
                SetSelectedId(customContent.ItemId);
        }

        protected virtual ObservableCollection<DataEntryCustomContentTemplateItem> GetCustomContent() => Globals.GetLineTypeContents();

        protected virtual void SetSelectedId(int selectedId) => LineType = (AppGridLineTypes) selectedId;

        protected virtual int GetSelectedId() => (int) LineType;

        protected void SelectItem(int itemId)
        {
            if (!_controlLoaded)
                return;

            var selectedItem = Content.FirstOrDefault(f => f.ItemId == itemId);

            ComboBox.SelectedItem = selectedItem;
        }

        protected int GetSelectedItemId()
        {
            if (ComboBox.SelectedItem is DataEntryCustomContentTemplateItem customContent)
                return customContent.ItemId;

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
