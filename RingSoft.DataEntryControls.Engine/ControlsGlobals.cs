using System;

namespace RingSoft.DataEntryControls.Engine
{
    public static class ControlsGlobals
    {
        private static IControlsUserInterface _userInterface;

        public static IControlsUserInterface UserInterface
        {
            get
            {
                if (_userInterface == null)
                    throw new Exception("ControlsGlobals UserInterface not set.  Run WPFControlsGlobals.InitUI");

                return _userInterface;
            }
            set => _userInterface = value;
        }

    }
}
