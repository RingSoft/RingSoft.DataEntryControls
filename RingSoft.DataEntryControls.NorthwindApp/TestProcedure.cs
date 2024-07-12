using System;
using System.Windows;
using RingSoft.DataEntryControls.WPF;
using System.Windows.Threading;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.NorthwindApp
{
    public class TestProcedure : TwoTierProcessingProcedure
    {
        public string Name { get; set; }

        public TestProcedure(Window ownerWindow, string windowText) : base(ownerWindow, windowText)
        {
            Name = "Test";
        }

        public override bool DoProcedure()
        {
            if (Name.IsNullOrEmpty())
            {
                throw new Exception("Bad");
            }
            var max = 10000;

            for (int current = 0; current < max; current++)
            {
                if (current < max)
                {
                    SetProgress(max, current, "Test1", 100,  50, "Test2");
                }
            }

            return true;
        }
    }
}
