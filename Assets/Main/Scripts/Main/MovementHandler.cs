using UnityEngine;
using PathCreation;


public class MovementHandler : MonoBehaviour
{
    [field : SerializeField] public PathCreator PathCreator { get; private set; }
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
        _transform.position = PathCreator.path.GetPoint(0); ;
        _transform.rotation = PathCreator.path.GetRotation(0);
        joystick.OnFingerTravel += OnJoystickTravel;
    }

    private void OnJoystickTravel()
    {
        _joystickOutput += horizontalSpeed * Time.deltaTime * new Vector3(-joystick.Horizontal, 0, 0);
    }

    public void MoveAlongWithPath()
    {
        _distanceTravelled += speed * Time.deltaTime;
        var targetPos = PathCreator.path.GetPointAtDistance(_distanceTravelled, endOfPathInstruction) + _joystickOutput;
        var targetRot = PathCreator.path.GetRotationAtDistance(_distanceTravelled, endOfPathInstruction);
        _transform.SetPositionAndRotation(targetPos, targetRot) ;
    }
}
