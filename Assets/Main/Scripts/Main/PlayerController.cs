using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Idle,
        RunningAndShooting,
        Happy,
        Sad,
        Dead
    }

    public State PlayerState { get; private set; }
    [SerializeField] private MovementHandler movementHandler;
    [SerializeField] private Animator animator;
    [SerializeField] private Rig rig;
    [SerializeField] private TwoBoneIKConstraint leftHandIK, rightHandIk;
    public void Initialize()
    {
        PlayerState = State.Idle;
        movementHandler.PathCreator.path.GetRotationAtDistance(0);
    }

    private void Start()
    {
        Initialize();
        animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
        rig.weight = 1;
        //StartCoroutine(WaitAndStartRunning());
    }

    private IEnumerator WaitAndStartRunning()
    {
        yield return new WaitForSeconds(3);
        PlayerState = State.RunningAndShooting;
        animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
        rig.weight = 1;
    }

    public void Die()
    {
        EventManager.TriggerEvent(EventManager.EventSignature.OnPlayerDefeated, this);
    }

    private void Update()
    {
        if (PlayerState != State.RunningAndShooting) return;

        movementHandler.MoveAlongWithPath();
    }
}
