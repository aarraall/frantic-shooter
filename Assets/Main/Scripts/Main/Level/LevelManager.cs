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

    private void Awake()
    {
        Instance = this;
        Subscribe();
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

    }

    private void OnPlayerWin(object obj)
    {
        // Show win popup and load new level
    }

    private void LoadLevel()
    {
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

    }
}
