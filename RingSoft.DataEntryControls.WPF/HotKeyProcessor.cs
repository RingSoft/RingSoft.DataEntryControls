using System;
using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Timers;
using System.Windows.Input;

namespace RingSoft.DataEntryControls.WPF
{
    public class HotKey
    {
        public IReadOnlyList<Key> Keys { get; }

        public RelayCommand Command { get; }

        private List<Key> _keys = new List<Key>();

        public HotKey(RelayCommand command = null)
        {
            Keys = _keys.AsReadOnly();
            Command = command;
        }

        public void AddKey(Key key)
        {
            _keys.Add(key);
        }
    }

    public class HotKeyPressedArgs
    {
        public HotKey HotKeyPressed { get; }

        public HotKeyPressedArgs(HotKey hotKeyPressed)
        {
            HotKeyPressed = hotKeyPressed;
        }
    }

    internal class HotKeyPressed
    {
        public Key Key { get; set; }

        public bool KeyFound { get; set; }
    }
    public class HotKeyProcessor
    {
        public IReadOnlyList<HotKey> HotKeys { get; }

        public IReadOnlyList<Key> IgnoreKeys { get; }

        public bool TopLevel { get; set; } //Peter Ringering - 12/10/2024 04:51:33 PM - E-73

        public event EventHandler<HotKeyPressedArgs> HotKeyPressed;

        private List<HotKey> _hotKeys = new List<HotKey>();
        private List<HotKeyPressed> _keysPressed = new List<HotKeyPressed>();
        private List<Key> _ignoreKeys = new List<Key>();
        private int _elapsedSeconds;
        private Timer _timer = new Timer();

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

        public void AddHotKey(HotKey hotKey)
        {
            _hotKeys.Add(hotKey);
        }

        public void RemoveHotKey(HotKey hotKey)
        {
            if (_hotKeys.Contains(hotKey))
            {
                _hotKeys.Remove(hotKey);
            }
        }

        public void AddIgnoreKey(Key ignoreKey)
        {
            _ignoreKeys.Add(ignoreKey);
        }

        public void RemoveIgnoreKey(Key ignoreKey)
        {
            if (_ignoreKeys.Contains(ignoreKey))
            {
                _ignoreKeys.Remove(ignoreKey);
            }
        }

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

        public void OnKeyUp(KeyEventArgs e)
        {

        }

        private void ClearKeyPressed()
        {
            _timer.Stop();
            _timer.Enabled = false;
            _elapsedSeconds = 0;
            _keysPressed.Clear();
        }
    }
}
