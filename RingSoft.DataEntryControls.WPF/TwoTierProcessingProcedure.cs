// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 07-11-2024
//
// Last Modified By : petem
// Last Modified On : 09-07-2024
// ***********************************************************************
// <copyright file="TwoTierProcessingProcedure.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows;
using RingSoft.DataEntryControls.Engine;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class TwoTierProcessingProcedure.
    /// Implements the <see cref="ITwoTierProcessingProcedure" />
    /// </summary>
    /// <seealso cref="ITwoTierProcessingProcedure" />
    public abstract class TwoTierProcessingProcedure : ITwoTierProcessingProcedure
    {
        /// <summary>
        /// Gets the processing window.
        /// </summary>
        /// <value>The processing window.</value>
        public TwoTierProcessingWindow ProcessingWindow { get; }

        /// <summary>
        /// Gets the owner window.
        /// </summary>
        /// <value>The owner window.</value>
        public Window OwnerWindow { get; }

        /// <summary>
        /// Gets the top maximum.
        /// </summary>
        /// <value>The top maximum.</value>
        public int TopMax { get; private set; }

        /// <summary>
        /// Gets the top value.
        /// </summary>
        /// <value>The top value.</value>
        public int TopValue { get; private set; }

        /// <summary>
        /// Gets the top text.
        /// </summary>
        /// <value>The top text.</value>
        public string TopText { get; private set; }

        /// <summary>
        /// Gets the bottom maximum.
        /// </summary>
        /// <value>The bottom maximum.</value>
        public int BottomMax { get; private set; }

        /// <summary>
        /// Gets the bottom value.
        /// </summary>
        /// <value>The bottom value.</value>
        public int BottomValue { get; private set; }

        /// <summary>
        /// Gets the bottom text.
        /// </summary>
        /// <value>The bottom text.</value>
        public string BottomText { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoTierProcessingProcedure" /> class.
        /// </summary>
        /// <param name="ownerWindow">The owner window.</param>
        /// <param name="windowText">The window text.</param>
        public TwoTierProcessingProcedure(Window ownerWindow, string windowText)
        {
            OwnerWindow = ownerWindow;
            ProcessingWindow = new TwoTierProcessingWindow(this);
            ProcessingWindow.Title = windowText;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public virtual void Start()
        {
            ProcessingWindow.Process();
        }

        /// <summary>
        /// Does the procedure.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public abstract bool DoProcedure();

        /// <summary>
        /// Sets the progress.
        /// </summary>
        /// <param name="topMax">The top maximum.</param>
        /// <param name="topValue">The top value.</param>
        /// <param name="topText">The top text.</param>
        /// <param name="bottomMax">The bottom maximum.</param>
        /// <param name="bottomValue">The bottom value.</param>
        /// <param name="bottomText">The bottom text.</param>
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
