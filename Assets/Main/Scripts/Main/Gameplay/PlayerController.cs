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
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private MovementHandler movementHandler;
    [SerializeField] private Animator animator;
    [SerializeField] private Rig rig;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private TwoBoneIKConstraint leftHandIK, rightHandIk;

    [SerializeField] private Weapon initialWeaponPrefab;

    Weapon _currentWeapon;
    float _fireCounter = 0;

    public Weapon Weapon => _currentWeapon;

    public void Initialize(Joystick inputController)
    {
        SetState(State.Idle);
        OnTakeWeapon(initialWeaponPrefab);
        movementHandler.Initialize(inputController);
        Subscribe();
    }

    public void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        Unsubscribe();
        EventManager.StartListening(EventManager.EventSignature.OnLevelStart, OnLevelStart);
    }

    private void Unsubscribe()
    {
        EventManager.StopListening(EventManager.EventSignature.OnLevelStart, OnLevelStart);
    }

    private void OnLevelStart(object obj)
    {
        SetState(State.RunningAndShooting);
    }

    public void Die()
    {
        SetState(State.Dead);
        EventManager.TriggerEvent(EventManager.EventSignature.OnPlayerDefeated, this);
        //die animation
        // FX etc
    }

    public void OnTakeWeapon(Weapon weapon)
    {
        if (_currentWeapon != null)
        {
            Destroy(_currentWeapon.gameObject);
        }

        _currentWeapon = Instantiate(weapon);
        _currentWeapon.Initialize(shootingPoint);
        SetIK();
        SetWeaponPosition();
    }

    private void SetIK()
    {
        //left hand IK
        leftHandIK.data.target.SetLocalPositionAndRotation(_currentWeapon.WeaponConfigData.WeaponHolderRigDataProperty.LeftHandIKTargetPosition,
            Quaternion.Euler(_currentWeapon.WeaponConfigData.WeaponHolderRigDataProperty.LeftHandIKTargetRotationEuler));

        // right hand IK
        rightHandIk.data.target.SetLocalPositionAndRotation(_currentWeapon.WeaponConfigData.WeaponHolderRigDataProperty.RightHandIKTargetPosition,
            Quaternion.Euler(_currentWeapon.WeaponConfigData.WeaponHolderRigDataProperty.RightHandIKTargetRotationEuler));
    }

    private void SetWeaponPosition()
    {
        _currentWeapon.transform.SetParent(rightHandTransform);
        _currentWeapon.transform.SetLocalPositionAndRotation(_currentWeapon.WeaponConfigData.WeaponPositionDataProperty.Position,
            Quaternion.Euler(_currentWeapon.WeaponConfigData.WeaponPositionDataProperty.RotationEuler));
    }

    public void SetState(State state)
    {
        PlayerState = state;
        rig.weight = 0;
        animator.SetBool(GameConstants.PlayerAnimatorStateMap[State.RunningAndShooting], false);

        switch (state)
        {
            case State.Idle:
                animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
                break;
            case State.RunningAndShooting:
                rig.weight = 1;
                animator.SetBool(GameConstants.PlayerAnimatorStateMap[PlayerState], true);
                break;
            case State.Happy:
                animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
                break;
            case State.Sad:
                animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
                break;
            case State.Dead:
                animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
                break;
        }
    }
    private void Fire()
    {
        _fireCounter += Time.deltaTime;

        if (_fireCounter < Weapon.FireRate)
        {
            return;
        }

        _currentWeapon.Fire();
        _fireCounter = 0;
    }


    private void Update()
    {
        if (PlayerState != State.RunningAndShooting) return;
        if (movementHandler == null) return;
        if (_currentWeapon == null) return;
        if (Weapon.FireRate == 0) return;

        movementHandler.MoveAlongWithPath();
        Fire();
    }

}
