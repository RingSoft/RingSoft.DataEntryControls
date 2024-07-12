using System.Windows;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    public abstract class TwoTierProcessingProcedure : ITwoTierProcessingProcedure
    {
        public TwoTierProcessingWindow ProcessingWindow { get; }

        public Window OwnerWindow { get; }

        public int TopMax { get; private set; }

        public int TopValue { get; private set; }

        public string TopText { get; private set; }

        public int BottomMax { get; private set; }

        public int BottomValue { get; private set; }

        public string BottomText { get; private set; }

        public TwoTierProcessingProcedure(Window ownerWindow, string windowText)
        {
            OwnerWindow = ownerWindow;
            ProcessingWindow = new TwoTierProcessingWindow(this);
            ProcessingWindow.Title = windowText;
        }

        public virtual void Start()
        {
            ProcessingWindow.Process();
        }

        public abstract bool DoProcedure();

        public void SetProgress(int topMax = 0
            , int topValue = 0
            , string topText = ""
            , int bottomMax = 0
            , int bottomValue = 0
            , string bottomText = "")
        {
            if (topMax == 0)
            {
                topMax = TopMax;
            }
            if (topValue ==  0)
            {
                topValue = TopValue;
            }

            if (bottomMax == 0)
            {
                bottomMax = BottomMax;
            }

            if (bottomValue == 0)
            {
                bottomValue = BottomValue;
            }

            if (topText.IsNullOrEmpty())
            {
                topText = TopText;
            }

            if (bottomText.IsNullOrEmpty())
            {
                bottomText = BottomText;
            }

            TopMax = topMax;
            TopValue = topValue;
            BottomMax = bottomMax;
            BottomValue = bottomValue;
            TopText = topText;
            BottomText = bottomText;

            ProcessingWindow.SetProgress(topMax, topValue, topText, bottomMax, bottomValue, bottomText);
        }
    }
}
