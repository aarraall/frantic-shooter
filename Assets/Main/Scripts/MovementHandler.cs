using UnityEngine;
using PathCreation;
using UnityEngine.UIElements;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] PathCreator pathCreator;
    [SerializeField] Joystick joystick;
    [SerializeField] float speed;
    [SerializeField] float horizontalClamp;
    [SerializeField] EndOfPathInstruction endOfPathInstruction;


    float _distanceTravelled;
    Transform _transform;

    private void Start()
    {
        _transform = transform;
        _transform.position = pathCreator.path.GetPoint(0); ;
        _transform.rotation = pathCreator.path.GetRotation(0);
    }
    public void MoveAlongWithPath()
    {
        _distanceTravelled += speed * Time.deltaTime;
        var targetPos = pathCreator.path.GetPointAtDistance(_distanceTravelled, endOfPathInstruction) + new Vector3(0, 0, -joystick.Horizontal);
        var targetRot = pathCreator.path.GetRotationAtDistance(_distanceTravelled, endOfPathInstruction);
        _transform.SetPositionAndRotation(targetPos, targetRot) ;
    }

    private void Update()
    {
        MoveAlongWithPath();
    }

}
