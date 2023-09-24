using Cinemachine;
using System.Collections;
using UnityEngine;

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

    PlayerController _playerInstance;
    float _distanceBetweenStartAndFinish;
    int _nextUpgradeCheckpointIndex = 0;
    State _state = State.None;

    public void Initialize(Joystick inputController, CinemachineVirtualCameraBase followCam)
    {
        _playerInstance = Instantiate(playerPrefab, transform);
        _playerInstance.transform.SetPositionAndRotation(playerSpawnPoint.position, Quaternion.identity);
        followCam.Follow = _playerInstance.transform;
        followCam.LookAt = _playerInstance.transform;
        _playerInstance.Initialize(inputController);
        _distanceBetweenStartAndFinish = Vector3.Distance(playerSpawnPoint.position, finishPoint.position);
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
    }

    private void Update()
    {
        if (_state != State.Playing) { return; }
        if (_playerInstance == null) { return; }

        var playerFinishLineDistance = Vector3.Distance(_playerInstance.transform.position, finishPoint.position);
        var finishRate = playerFinishLineDistance / _distanceBetweenStartAndFinish;

        if (!Mathf.Approximately(finishRate, UpgradeTriggerPercentages[_nextUpgradeCheckpointIndex]))
        {
            return;
        }

        PauseLevel();
    }

}
