// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 11-11-2022
//
// Last Modified By : petem
// Last Modified On : 11-11-2022
// ***********************************************************************
// <copyright file="WaitSpinner.xaml.cs" company="Peter Ringering">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleLossOfFraction

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Interaction logic for WaitSpinner.xaml
    /// </summary>
    public partial class WaitSpinner
    {
        /// <summary>
        /// Gets or sets the size of the ellipse.
        /// </summary>
        /// <value>The size of the ellipse.</value>
        public int EllipseSize { get; set; } = 8;
        /// <summary>
        /// Gets or sets the height of the spinner.
        /// </summary>
        /// <value>The height of the spinner.</value>
        public int SpinnerHeight { get; set; }
        /// <summary>
        /// Gets or sets the width of the spinner.
        /// </summary>
        /// <value>The width of the spinner.</value>
        public int SpinnerWidth { get; set; }


        // start positions
        /// <summary>
        /// Gets the ellipse n.
        /// </summary>
        /// <value>The ellipse n.</value>
        public EllipseStartPosition EllipseN { get; private set; }
        /// <summary>
        /// Gets the ellipse ne.
        /// </summary>
        /// <value>The ellipse ne.</value>
        public EllipseStartPosition EllipseNE { get; private set; }
        /// <summary>
        /// Gets the ellipse e.
        /// </summary>
        /// <value>The ellipse e.</value>
        public EllipseStartPosition EllipseE { get; private set; }
        /// <summary>
        /// Gets the ellipse se.
        /// </summary>
        /// <value>The ellipse se.</value>
        public EllipseStartPosition EllipseSE { get; private set; }
        /// <summary>
        /// Gets the ellipse s.
        /// </summary>
        /// <value>The ellipse s.</value>
        public EllipseStartPosition EllipseS { get; private set; }
        /// <summary>
        /// Gets the ellipse sw.
        /// </summary>
        /// <value>The ellipse sw.</value>
        public EllipseStartPosition EllipseSW { get; private set; }
        /// <summary>
        /// Gets the ellipse w.
        /// </summary>
        /// <value>The ellipse w.</value>
        public EllipseStartPosition EllipseW { get; private set; }
        /// <summary>
        /// Gets the ellipse nw.
        /// </summary>
        /// <value>The ellipse nw.</value>
        public EllipseStartPosition EllipseNW { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitSpinner"/> class.
        /// </summary>
        public WaitSpinner()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initials the setup.
        /// </summary>
        private void initialSetup()
        {
            float horizontalCenter = SpinnerWidth / 2;
            float verticalCenter = SpinnerHeight / 2;
            float distance = (float)Math.Min(SpinnerHeight, SpinnerWidth) / 2;

            double angleInRadians = 44.8;
            float cosine = (float)Math.Cos(angleInRadians);
            float sine = (float)Math.Sin(angleInRadians);

            EllipseN = newPos(left: horizontalCenter, top: verticalCenter - distance);
            EllipseNE = newPos(left: horizontalCenter + (distance * cosine), top: verticalCenter - (distance * sine));
            EllipseE = newPos(left: horizontalCenter + distance, top: verticalCenter);
            EllipseSE = newPos(left: horizontalCenter + (distance * cosine), top: verticalCenter + (distance * sine));
            EllipseS = newPos(left: horizontalCenter, top: verticalCenter + distance);
            EllipseSW = newPos(left: horizontalCenter - (distance * cosine), top: verticalCenter + (distance * sine));
            EllipseW = newPos(left: horizontalCenter - distance, top: verticalCenter);
            EllipseNW = newPos(left: horizontalCenter - (distance * cosine), top: verticalCenter - (distance * sine));
        }

        /// <summary>
        /// News the position.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="top">The top.</param>
        /// <returns>EllipseStartPosition.</returns>
        private EllipseStartPosition newPos(float left, float top)
        {
            return new EllipseStartPosition() { Left = left, Top = top };
        }


        /// <summary>
        /// Handles the <see cref="E:PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Height")
            {
                SpinnerHeight = Convert.ToInt32(e.NewValue);
            }

            if (e.Property.Name == "Width")
            {
                SpinnerWidth = Convert.ToInt32(e.NewValue);
            }

            if (SpinnerHeight > 0 && SpinnerWidth > 0)
            {
                initialSetup();
            }

            base.OnPropertyChanged(e);
        }
    }

    /// <summary>
    /// Struct EllipseStartPosition
    /// </summary>
    public struct EllipseStartPosition
    {
        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>The left.</value>
        public float Left { get; set; }
        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>The top.</value>
        public float Top { get; set; }
    }
}
