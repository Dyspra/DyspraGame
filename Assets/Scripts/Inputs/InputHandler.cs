using UnityEngine;

namespace Dyspra
{
    public class InputHandler
    {
        AbstractCommand _buttonTeleport = new Command_Teleport();
        AbstractCommand _buttonValidate = new Command_Validate();
        AbstractCommand _buttonBack = new Command_Back();

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
