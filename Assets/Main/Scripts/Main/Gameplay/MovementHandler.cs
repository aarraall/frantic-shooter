using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using System;
using Zenject;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] float speed;

    Transform _transform;

    Vector3 _joystickOutput;
    private float horizontalSpeed = 1;
    float leftClamp = -4f;
    float rightClamp = 4f;
    Joystick _joystick;

    [Inject]
    public void Initialize(Joystick inputController)
    {
        _joystick = inputController;
        _transform = transform;
        _joystick.OnFingerTravel += OnJoystickTravel;
        _joystick.OnFingerUp += OnFingerUp;
    }


    private void OnFingerUp()
    {
        _joystickOutput = Vector3.zero;
    }

    private void OnJoystickTravel()
    {
        var joyStickHorizontalOutput = _joystick.Horizontal;
        _joystickOutput = horizontalSpeed * Time.deltaTime * new Vector3(joyStickHorizontalOutput, 0, 0);
    }

    public void MoveAlongWithPath()
    {
        Vector3 distanceTravelled = speed * Time.deltaTime * Vector3.forward;

        _transform.position += distanceTravelled + _joystickOutput;
        var clampedX = transform.position.x;
        clampedX = Mathf.Clamp(clampedX, leftClamp, rightClamp);

    }
}
