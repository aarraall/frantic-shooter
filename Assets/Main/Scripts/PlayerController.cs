using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningAndShooting,
        Happy,
        Sad
    }

    public State PlayerState { get; private set; }
    [SerializeField] private MovementHandler movementHandler;
    [SerializeField] private Animator animator;
    [SerializeField] private Rig rig;

    public void Initialize()
    {
        PlayerState = State.Idle;
        movementHandler.PathCreator.path.GetRotationAtDistance(0);
    }

    private void Start()
    {
        Initialize();
        animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
        rig.weight = 0;
        StartCoroutine(WaitAndStartRunning());
    }

    private IEnumerator WaitAndStartRunning()
    {
        yield return new WaitForSeconds(3);
        PlayerState = State.RunningAndShooting;
        animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
        rig.weight = 1;
    }

    private void Update()
    {
        if (PlayerState != State.RunningAndShooting) return;

        movementHandler.MoveAlongWithPath();
    }
}
