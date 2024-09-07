// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.Engine
// Author           : petem
// Created          : 07-11-2024
//
// Last Modified By : petem
// Last Modified On : 07-11-2024
// ***********************************************************************
// <copyright file="TwoTierProgressViewModel.cs" company="Peter Ringering">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RingSoft.DataEntryControls.Engine
{
    /// <summary>
    /// Class TwoTierProgressViewModel.
    /// Implements the <see cref="INotifyPropertyChanged" />
    /// </summary>
    /// <seealso cref="INotifyPropertyChanged" />
    public class TwoTierProgressViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The top tier text
        /// </summary>
        private string _topTierText;

        /// <summary>
        /// Gets or sets the top tier text.
        /// </summary>
        /// <value>The top tier text.</value>
        public string TopTierText
        {
            get => _topTierText;
            set
            {
                if (_topTierText == value)
                {
                    return;
                }
                _topTierText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The top tier maximum
        /// </summary>
        private int _topTierMaximum;

        /// <summary>
        /// Gets or sets the top tier maximum.
        /// </summary>
        /// <value>The top tier maximum.</value>
        public int TopTierMaximum
        {
            get => _topTierMaximum;
            set
            {
                if (_topTierMaximum == value)
                {
                    return;
                }
                _topTierMaximum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The top tier progress
        /// </summary>
        private int _topTierProgress;

        /// <summary>
        /// Gets or sets the top tier progress.
        /// </summary>
        /// <value>The top tier progress.</value>
        public int TopTierProgress
        {
            get => _topTierProgress;
            set
            {
                if (_topTierProgress == value)
                {
                    return;
                }
                _topTierProgress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The bottom tier text
        /// </summary>
        private string _bottomTierText;

        /// <summary>
        /// Gets or sets the bottom tier text.
        /// </summary>
        /// <value>The bottom tier text.</value>
        public string BottomTierText
        {
            get => _bottomTierText;
            set
            {
                if (_bottomTierText == value)
                {
                    return;
                }
                _bottomTierText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The bottom tier maximum
        /// </summary>
        private int _bottomTierMaximum;

        /// <summary>
        /// Gets or sets the bottom tier maximum.
        /// </summary>
        /// <value>The bottom tier maximum.</value>
        public int BottomTierMaximum
        {
            get => _bottomTierMaximum;
            set
            {
                if (_bottomTierMaximum == value)
                {
                    return;
                }
                _bottomTierMaximum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The bottom tier progress
        /// </summary>
        private int _bottomTierProgress;

        /// <summary>
        /// Gets or sets the bottom tier progress.
        /// </summary>
        /// <value>The bottom tier progress.</value>
        public int BottomTierProgress
        {
            get => _bottomTierProgress;
            set
            {
                if (_bottomTierProgress == value)
                {
                    return;
                }
                _bottomTierProgress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The window text
        /// </summary>
        private string _windowText;

        /// <summary>
        /// Gets or sets the window text.
        /// </summary>
        /// <value>The window text.</value>
        public string WindowText
        {
            get => _windowText;
            set
            {
                if (_windowText == value)
                {
                    return;
                }
                _windowText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
