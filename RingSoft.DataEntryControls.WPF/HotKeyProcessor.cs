using System;
using RingSoft.DataEntryControls.Engine;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

        public event EventHandler<HotKeyPressedArgs> HotKeyPressed;

        private List<HotKey> _hotKeys = new List<HotKey>();
        private List<HotKeyPressed> _keysPressed = new List<HotKeyPressed>();

        public HotKeyProcessor()
        {
            HotKeys= _hotKeys.AsReadOnly();
        }

        public void AddHotKey(HotKey hotKey)
        {
            _hotKeys.Add(hotKey);
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
                        if (hotKeyKey == _keysPressed[hotKeyKeyIndex].Key)
                        {
                            _keysPressed[hotKeyKeyIndex].KeyFound = true;
                        }

                        hotKeyKeyIndex++;
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
                    SystemSounds.Asterisk.Play();
                    ClearKeyPressed();
                    e.Handled = true;
                }
            }
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            //if (e.Key == Key.RightCtrl && !Keyboard.IsKeyDown(Key.LeftCtrl))
            //{
            //    ClearKeyPressed();
            //}
            //if (e.Key == Key.LeftCtrl && !Keyboard.IsKeyDown(Key.RightCtrl))
            //{
            //    ClearKeyPressed();
            //}
        }

        private void ClearKeyPressed()
        {
            _keysPressed.Clear();
        }
    }
}
