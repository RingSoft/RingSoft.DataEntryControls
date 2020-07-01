using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.App.WPF
{
    public static class Globals
    {
        public static string CultureId { get; set; }

        public static int Precision { get; set; }

        public static DataEntryNumericEditSetup GetNumericEditSetup()
        {
            return new DataEntryNumericEditSetup()
            {
                CultureId = CultureId,
                Precision = Precision
            };
        }
    }
}
