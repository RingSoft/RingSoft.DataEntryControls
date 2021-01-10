using RingSoft.DataEntryControls.Engine;

namespace TestDummyApp
{
    public static class Globals
    {
        public const int CommentDisplayStyleId = 100;
        public const int NonInventoryDisplayStyleId = 101;

        public const int LineTypeControlId = 100;

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
