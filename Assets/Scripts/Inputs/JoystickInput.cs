using UnityEngine;

public class JoystickInput : IInputService {
    private Joystick _joystick;

    public Vector2 Direction => _joystick.Direction;

    public JoystickInput(Joystick joystick) {
        _joystick = joystick;
    }

    public void Enable() {
        _joystick.gameObject.SetActive(true);
    }

    public void Disable() {
        _joystick.gameObject.SetActive(false);
    }

}
