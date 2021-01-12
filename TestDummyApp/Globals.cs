using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RingSoft.DataEntryControls.Engine;
using RingSoft.DataEntryControls.WPF;

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
