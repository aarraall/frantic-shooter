using UnityEngine;
using PathCreation;
using UnityEngine.UIElements;
using System;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] Joystick joystick;
    [SerializeField] float speed;
    [SerializeField] EndOfPathInstruction endOfPathInstruction;

    float _distanceTravelled;
    Transform _transform;

    Vector3 _joystickOutput;
    private float horizontalSpeed = 5;

    private void Start()
    {
        _transform = transform;
        _transform.position = pathCreator.path.GetPoint(0); ;
        _transform.rotation = pathCreator.path.GetRotation(0);
        joystick.OnFingerTravel += OnJoystickTravel;
    }

    private void OnJoystickTravel()
    {
        _joystickOutput += new Vector3(0, 0, -joystick.Horizontal) * Time.deltaTime * horizontalSpeed;
    }

    public void MoveAlongWithPath()
    {
        _distanceTravelled += speed * Time.deltaTime;
        var targetPos = pathCreator.path.GetPointAtDistance(_distanceTravelled, endOfPathInstruction) + _joystickOutput;
        var targetRot = pathCreator.path.GetRotationAtDistance(_distanceTravelled, endOfPathInstruction);
        _transform.SetPositionAndRotation(targetPos, targetRot) ;
    }

    private void Update()
    {
        MoveAlongWithPath();
    }
}
