using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : IInputService {
    private Joystick _joystick;

    public Vector2 Direction => _joystick.Direction;

    public JoystickInput(Joystick joystick) {
        _joystick = joystick;
    }
}
