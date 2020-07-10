using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.App.WPF
{
    public static class Globals
    {
        public static string CultureId { get; set; }

        public static int Precision { get; set; }

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
