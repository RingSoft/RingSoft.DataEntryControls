using System.Windows.Input;

namespace RingSoft.DataEntryControls.Engine.DataEntryGrid
{
    public class DataEntryGridContextMenuItem
    {
        public string Header { get; }

        public ICommand Command { get; }

        public object CommandParameter { get; set; }

        public DataEntryGridContextMenuItem(string header, ICommand command)
        {
            Header = header;
            Command = command;
        }
    }
}
