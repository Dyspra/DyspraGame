using UnityEngine;

namespace Dyspra
{
    /// <summary>
    /// Singleton class that will allow binding and calling every input
    /// </summary>
    public class InputHandler : Singleton<InputHandler>
    {
        AbstractCommand _buttonTeleport = new Command_Teleport();
        AbstractCommand _buttonValidate = new Command_Validate();
        AbstractCommand _buttonBack = new Command_Back();

        /// <summary>
        /// Check if any input was triggered
        /// </summary>
        public AbstractCommand HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                return _buttonTeleport;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                return _buttonValidate;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                return _buttonBack;
            return null;
        }
    }
}
