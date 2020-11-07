using RingSoft.DataEntryControls.Engine;

namespace TestDummyApp
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
