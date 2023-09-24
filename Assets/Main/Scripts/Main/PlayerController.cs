using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SocialPlatforms;

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
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private TwoBoneIKConstraint leftHandIK, rightHandIk;

    [SerializeField] private Weapon initialWeaponPrefab;

    Weapon _currentWeapon;
    float _currentFireRate = 0f;
    float _fireCounter = 0;

    public void Initialize(Joystick inputController)
    {
        SetState(State.Idle);
        OnTakeWeapon(Instantiate(initialWeaponPrefab));
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
        _currentWeapon = weapon;
        _currentWeapon.Initialize();
        SetIK();
        SetWeaponPosition();
        _currentFireRate = 1f / _currentWeapon.
            WeaponAttributesProperty.
            UpgradeLevelMap[WeaponUpgradeType.FireRate].
            UpgradeValue;
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
        animator.SetTrigger(GameConstants.PlayerAnimatorStateMap[PlayerState]);
        rig.weight = 0;

        switch (state)
        {
            case State.Idle:
                break;
            case State.RunningAndShooting:
                rig.weight = 1;
                break;
            case State.Happy:
                break;
            case State.Sad:
                break;
            case State.Dead:
                break;
        }
    }
    private void Fire()
    {
        _fireCounter += Time.deltaTime;

        if (_fireCounter < _currentFireRate)
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
        if (_currentFireRate == 0) return;

        movementHandler.MoveAlongWithPath();
        Fire();
    }
   
}
