using Cinemachine;
using PathCreation.Examples;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Level : MonoBehaviour
{
    enum State
    {
        None,
        Initialized,
        Playing,
        Paused
    }

    [SerializeField] Transform playerSpawnPoint, finishPoint;
    [SerializeField] PlayerController playerPrefab; // can be injected through GameManager or LevelManager?
    [field: SerializeField] public float[] UpgradeTriggerPercentages { get; set; }

    [field: SerializeField] public RoadMeshCreator RoadMeshCreator { get; set; }

    PlayerController _playerInstance;
    float _distanceBetweenStartAndFinish;
    int _nextUpgradeCheckpointIndex = 0;
    State _state = State.None;

    public PlayerController PlayerInstance => _playerInstance;

    public void Initialize(Joystick inputController, CinemachineVirtualCameraBase followCam)
    {
        _playerInstance = Instantiate(playerPrefab, transform);
        _playerInstance.transform.SetPositionAndRotation(playerSpawnPoint.position, Quaternion.identity);
        followCam.Follow = _playerInstance.transform;
        followCam.LookAt = _playerInstance.transform;
        _playerInstance.Initialize(inputController);
        _distanceBetweenStartAndFinish = Vector3.Distance(playerSpawnPoint.position, finishPoint.position);
        RoadMeshCreator.TriggerUpdate();
        _state = State.Initialized;
    }

    public void StartLevel()
    {
        _state = State.Playing;
    }

    public void PauseLevel()
    {
        _playerInstance.SetState(PlayerController.State.Idle);
        _state = State.Paused;
        PopupManager.Instance.OpenPopup(typeof(UpgradePopup));
    }
    private void FinishLevel()
    {
        _playerInstance.SetState(PlayerController.State.Happy);
        _state = State.Paused;
        PopupManager.Instance.OpenPopup(typeof(WinPopup));
    }


    private void Update()
    {
        if (_state != State.Playing) { return; }
        if (_playerInstance == null) { return; }

        var playerFinishLineDistance = Vector3.Distance(_playerInstance.transform.position, playerSpawnPoint.position);
        var finishRate = playerFinishLineDistance / _distanceBetweenStartAndFinish;

        if (Approximately(finishRate, 1, .0025f))
        {
            //Level finished
            FinishLevel();
            return;
        }
        if (UpgradeTriggerPercentages.Length <= _nextUpgradeCheckpointIndex)
        {
            return;
        }
        if (!Approximately(finishRate, UpgradeTriggerPercentages[_nextUpgradeCheckpointIndex], .0025f))
        {
            return;
        }

        _nextUpgradeCheckpointIndex++;
        PauseLevel();
    }

   

    private bool Approximately(float a, float b, float tolerance)
    {
        return (Mathf.Abs(a - b) < tolerance);
    }
}
