using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelDB levelDB;
    [SerializeField] Joystick joystick;
    [SerializeField] CinemachineVirtualCameraBase followCam;

    public static LevelManager Instance;

    Level _activeLevel;

    int _initialLevel = 1;

    public PlayerController PlayerController => _activeLevel == null ? null : _activeLevel.PlayerInstance;
    public Level ActiveLevel => _activeLevel;

    private void Awake()
    {
        Instance = this;
        Subscribe();
    }
    private void Start()
    {
        LoadLevel();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        Unsubscribe();
        EventManager.StartListening(EventManager.EventSignature.OnPlayerWin, OnPlayerWin);
        EventManager.StartListening(EventManager.EventSignature.OnPlayerDefeated, OnPlayerLose);
    }

    private void Unsubscribe()
    {
        EventManager.StopListening(EventManager.EventSignature.OnPlayerWin, OnPlayerWin);
        EventManager.StopListening(EventManager.EventSignature.OnPlayerDefeated, OnPlayerLose);
    }

    private void OnPlayerLose(object obj)
    {
        // Show defeat popup and restart level
        PopupManager.Instance.OpenPopup(typeof(DefeatPopup));
    }

    private void OnPlayerWin(object obj)
    {
        // Show win popup and load new level
        PopupManager.Instance.OpenPopup(typeof(WinPopup));
    }

    private void Unload()
    {
        if (_activeLevel == null) return;

        Destroy(_activeLevel.gameObject);
        _activeLevel = null;
    }

    private void LoadLevel()
    {
        Unload();

        var levelToLoad = levelDB.LevelConfigMap[_initialLevel];
        var levelPref = levelToLoad.LevelPrefab;
        _activeLevel = Instantiate(levelPref);
        _activeLevel.Initialize(joystick, followCam);
        PopupManager.Instance.OpenPopup(typeof(TapToPlayPopup));
    }

    public void StartLevel()
    {
        if (_activeLevel == null) return;

        _activeLevel.StartLevel();
        EventManager.TriggerEvent(EventManager.EventSignature.OnLevelStart);
    }

    public void LoadNextLevel()
    {
        _initialLevel++;
        LoadLevel();
    }

    public void RestartLevel()
    {
        LoadLevel();
    }
}
