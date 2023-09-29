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
    PathCreator _pathCreator;

    float _initialDistanceTravelledOnPath;

    [Inject]
    public void Initialize(Joystick inputController)
    {
        _joystick = inputController;
        _transform = transform;
        _joystick.OnFingerTravel += OnJoystickTravel;
        _joystick.OnFingerUp += OnFingerUp;
    }

    public void SetPathCreator(PathCreator pathCreator)
    {
        _pathCreator = pathCreator;
        _initialDistanceTravelledOnPath = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
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
        PathMovement();
    }

    private void StraightMove()
    {
        Vector3 distanceTravelled = speed * Time.deltaTime * Vector3.forward;

        _transform.position += distanceTravelled + _joystickOutput;
        var clampedX = _transform.position.x;
        Mathf.Clamp(clampedX, leftClamp, rightClamp);

        _transform.position = new Vector3(clampedX, _transform.position.y, _transform.position.z);
    }

    private void PathMovement()
    {
        if (_pathCreator == null) return;

        _initialDistanceTravelledOnPath -= speed * Time.deltaTime;
        _transform.position -=  _pathCreator.path.GetDirectionAtDistance(_initialDistanceTravelledOnPath, EndOfPathInstruction.Stop) * Time.deltaTime * speed;
        _transform.position += _joystickOutput * speed / 2f;
        _transform.position = new Vector3(Mathf.Clamp(_transform.position.x, leftClamp, rightClamp), _transform.position.y, _transform.position.z);
    }
}
