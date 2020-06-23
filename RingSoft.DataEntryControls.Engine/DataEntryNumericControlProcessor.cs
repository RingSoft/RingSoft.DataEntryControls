namespace RingSoft.DataEntryControls.Engine
{
    public class DataEntryNumericControlProcessor
    {
        public DataEntryNumericEditSetup Setup { get; }

        public INumericControl Control { get; }

        public DataEntryNumericControlProcessor( INumericControl control, DataEntryNumericEditSetup setup)
        {
            Control = control;
            Setup = setup;
        }

        public bool IsValidChar(char keyChar)
        {
            switch (keyChar)
            {
                case '\b':
                case '\u001b':  //Escape
                case '\t':
                case '\r':
                case '\n':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                case '.':
                    if (Setup.DecimalCount <= 0)
                        break;

                    if (!Control.Text.IsNullOrEmpty())
                    {
                        if (Control.Text.IndexOf(keyChar) >= 0)
                        {
                            var selectedText = Control.Text.MidStr(Control.SelectionStart, Control.SelectionLength);
                            if (selectedText.IndexOf(keyChar) < 0)
                                break;
                        }
                    }

                    return true;
            }

            Control.OnInvalidChar();
            return false;
        }
    }
}
