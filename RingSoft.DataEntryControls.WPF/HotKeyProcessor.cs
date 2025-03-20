// ***********************************************************************
// Assembly         : RingSoft.DataEntryControls.WPF
// Author           : petem
// Created          : 10-28-2024
//
// Last Modified By : petem
// Last Modified On : 01-09-2025
// ***********************************************************************
// <copyright file="HotKeyProcessor.cs" company="RingSoft">
//     2024
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Timers;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF
{
    /// <summary>
    /// Class HotKey.
    /// </summary>
    public class HotKey
    {
        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public IReadOnlyList<Key> Keys { get; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public RelayCommand Command { get; }

        /// <summary>
        /// The keys
        /// </summary>
        private List<Key> _keys = new List<Key>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public HotKey(RelayCommand command = null)
        {
            Keys = _keys.AsReadOnly();
            Command = command;
        }

        /// <summary>
        /// Adds the key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void AddKey(Key key)
        {
            _keys.Add(key);
        }
    }

    /// <summary>
    /// Class HotKeyPressedArgs.
    /// </summary>
    public class HotKeyPressedArgs
    {
        /// <summary>
        /// Gets the hot key pressed.
        /// </summary>
        /// <value>The hot key pressed.</value>
        public HotKey HotKeyPressed { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyPressedArgs"/> class.
        /// </summary>
        /// <param name="hotKeyPressed">The hot key pressed.</param>
        public HotKeyPressedArgs(HotKey hotKeyPressed)
        {
            HotKeyPressed = hotKeyPressed;
        }
    }

    /// <summary>
    /// Class HotKeyPressed.
    /// </summary>
    internal class HotKeyPressed
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [key found].
        /// </summary>
        /// <value><c>true</c> if [key found]; otherwise, <c>false</c>.</value>
        public bool KeyFound { get; set; }
    }
    /// <summary>
    /// Class HotKeyProcessor.
    /// </summary>
    public class HotKeyProcessor
    {
        /// <summary>
        /// Gets the hot keys.
        /// </summary>
        /// <value>The hot keys.</value>
        public IReadOnlyList<HotKey> HotKeys { get; }

        /// <summary>
        /// Gets the ignore keys.
        /// </summary>
        /// <value>The ignore keys.</value>
        public IReadOnlyList<Key> IgnoreKeys { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [top level].
        /// </summary>
        /// <value><c>true</c> if [top level]; otherwise, <c>false</c>.</value>
        public bool TopLevel { get; set; } //Peter Ringering - 12/10/2024 04:51:33 PM - E-73

        /// <summary>
        /// Occurs when [hot key pressed].
        /// </summary>
        public event EventHandler<HotKeyPressedArgs> HotKeyPressed;

        /// <summary>
        /// The hot keys
        /// </summary>
        private List<HotKey> _hotKeys = new List<HotKey>();
        /// <summary>
        /// The keys pressed
        /// </summary>
        private List<HotKeyPressed> _keysPressed = new List<HotKeyPressed>();
        /// <summary>
        /// The ignore keys
        /// </summary>
        private List<Key> _ignoreKeys = new List<Key>();
        /// <summary>
        /// The elapsed seconds
        /// </summary>
        private int _elapsedSeconds;
        /// <summary>
        /// The timer
        /// </summary>
        private Timer _timer = new Timer();

        /// <summary>
        /// Initializes a new instance of the <see cref="HotKeyProcessor"/> class.
        /// </summary>
        public HotKeyProcessor()
        {
            HotKeys = _hotKeys.AsReadOnly();
            IgnoreKeys = _ignoreKeys.AsReadOnly();

            AddIgnoreKey(Key.Delete);
            AddIgnoreKey(Key.Insert);
            AddIgnoreKey(Key.F4);
            AddIgnoreKey(Key.D0);
            AddIgnoreKey(Key.D1);
            AddIgnoreKey(Key.D2);
            AddIgnoreKey(Key.D3);
            AddIgnoreKey(Key.D4);
            AddIgnoreKey(Key.D5);
            AddIgnoreKey(Key.D6);
            AddIgnoreKey(Key.D7);
            AddIgnoreKey(Key.D8);
            AddIgnoreKey(Key.D9);

            _timer.Interval = 1000;
            _timer.Elapsed += (sender, args) =>
            {
                _elapsedSeconds += 1;
                if (_elapsedSeconds >= 5)
                {
                    ClearKeyPressed();
                }
            };
        }

        /// <summary>
        /// Adds the hot key.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void AddHotKey(HotKey hotKey)
        {
            _hotKeys.Add(hotKey);
        }

        /// <summary>
        /// Removes the hot key.
        /// </summary>
        /// <param name="hotKey">The hot key.</param>
        public void RemoveHotKey(HotKey hotKey)
        {
            if (_hotKeys.Contains(hotKey))
            {
                _hotKeys.Remove(hotKey);
            }
        }

        /// <summary>
        /// Adds the ignore key.
        /// </summary>
        /// <param name="ignoreKey">The ignore key.</param>
        public void AddIgnoreKey(Key ignoreKey)
        {
            _ignoreKeys.Add(ignoreKey);
        }

        /// <summary>
        /// Removes the ignore key.
        /// </summary>
        /// <param name="ignoreKey">The ignore key.</param>
        public void RemoveIgnoreKey(Key ignoreKey)
        {
            if (_ignoreKeys.Contains(ignoreKey))
            {
                _ignoreKeys.Remove(ignoreKey);
            }
        }

        /// <summary>
        /// Handles the <see cref="E:KeyPressed" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void OnKeyPressed(KeyEventArgs e)
        {
            var maxHotKeyCount = 0;
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                return;
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (_keysPressed.Count == 0)
                {
                    switch (e.Key)
                    {
                        case Key.C:
                        case Key.V:
                        case Key.X:
                        case Key.Z:
                        case Key.Y:
                            return;
                    }
                }

                if (IgnoreKeys.Contains(e.Key))
                {
                    ClearKeyPressed();
                    return;
                }

                _elapsedSeconds = 0;
                _timer.Enabled = true;
                _timer.Start();
                _keysPressed.Add(new HotKeyPressed()
                {
                    Key = e.Key,
                });
                foreach (var hotKey in _hotKeys)
                {
                    if (hotKey.Keys.Count > maxHotKeyCount)
                    {
                        maxHotKeyCount = hotKey.Keys.Count;
                    }
                    var hotKeyKeyIndex = 0;
                    foreach (var hotKeyKey in hotKey.Keys)
                    {
                        if (hotKeyKeyIndex > _keysPressed.Count - 1)
                        {
                            break;
                        }

                        if (hotKey.Keys.Count == _keysPressed.Count)
                        {
                            var hotKeyPressedIndex = 0;
                            foreach (var hotKeyPressed in _keysPressed)
                            {
                                if (hotKey.Keys[hotKeyPressedIndex] == hotKeyPressed.Key)
                                {
                                    hotKeyPressed.KeyFound = true;
                                }
                                hotKeyPressedIndex++;
                            }
                            hotKeyKeyIndex++;

                        }
                        //Peter Ringering - 12/10/2024 08:48:01 PM - E-73
                        else
                        {
                            if (hotKey.Keys[hotKeyKeyIndex] == _keysPressed[hotKeyKeyIndex].Key)
                            {
                                e.Handled = true;
                            }
                        }
                    }

                    if (hotKey.Keys.Count == _keysPressed.Count)
                    {
                        var index = 0;
                        HotKey hotKeyFound = null;
                        foreach (var key in _keysPressed)
                        {
                            if (hotKey.Keys[index] == key.Key)
                            {
                                key.KeyFound = true;
                                if (index == _keysPressed.Count - 1)
                                {
                                    if (_keysPressed
                                            .FirstOrDefault(p => p.KeyFound == false) == null)
                                    {
                                        hotKeyFound = hotKey;
                                    }
                                    else
                                    {
                                        hotKeyFound = null;
                                    }
                                }
                                else
                                {
                                    hotKeyFound = null;
                                }
                            }
                            index++;
                        }

                        if (hotKeyFound != null)
                        {
                            if (hotKeyFound.Command != null)
                            {
                                if (hotKeyFound.Command.IsEnabled)
                                {
                                    hotKeyFound.Command.Execute(null);
                                }
                                else
                                {
                                    SystemSounds.Asterisk.Play();
                                }
                            }

                            var pressedArgs = new HotKeyPressedArgs(hotKeyFound);
                            HotKeyPressed?.Invoke(this, pressedArgs);
                            e.Handled = true;
                            ClearKeyPressed();
                            return;
                        }
                    }
                }

                var hotKeysNotFound = _keysPressed.FirstOrDefault(
                    p => p.KeyFound == false);
                if (hotKeysNotFound != null && _keysPressed.Count == maxHotKeyCount)
                {
                    //Peter Ringering - 12/10/2024 04:51:33 PM - E-73
                    ClearKeyPressed();

                    if (!TopLevel)
                    {
                        SystemSounds.Asterisk.Play();
                        e.Handled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="E:KeyUp" /> event.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        public void OnKeyUp(KeyEventArgs e)
        {

        }

        /// <summary>
        /// Clears the key pressed.
        /// </summary>
        private void ClearKeyPressed()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _elapsedSeconds = 0;
            _keysPressed.Clear();
        }
    }
}
