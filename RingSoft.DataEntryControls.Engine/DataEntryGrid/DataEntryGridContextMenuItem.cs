namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridContextMenuItem
    {
        public string Header { get; }

        public RelayCommand Command { get; }

        public object CommandParameter { get; set; }

        public DataEntryGridContextMenuItem(string header, RelayCommand command)
        {
            Header = header;
            Command = command;
        }
    }
}
