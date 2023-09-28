using PathCreation.Examples;
using UnityEngine;
using Zenject;

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
    [field: SerializeField] public float[] UpgradeTriggerPercentages { get; set; }
    [field: SerializeField] public RoadMeshCreator RoadMeshCreator { get; set; }

    PlayerController _playerInstance;
    float _distanceBetweenStartAndFinish;
    int _nextUpgradeCheckpointIndex = 0;
    State _state = State.None;

    PopupManager _popupManager;
    CameraService _cameraService;

    public PlayerController PlayerInstance => _playerInstance;

    [Inject]
    public void Initialize(PopupManager popupManager, CameraService cameraService)
    {
        _popupManager = popupManager;
        _cameraService = cameraService;

        _playerInstance = Instantiate(Resources.Load<PlayerController>(PrefabDB.k_char_mousey_prefab), transform);
        _playerInstance.transform.SetPositionAndRotation(playerSpawnPoint.position, Quaternion.identity);
        _playerInstance.Initialize();

        _cameraService.FollowTarget(_playerInstance.transform);
        _cameraService.LookAtTarget(_playerInstance.transform);
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
        _popupManager.OpenPopup(new PopupBase.ModelBase(PrefabDB.k_ui_upgrade_prefab));
    }
    private void FinishLevel()
    {
        _playerInstance.SetState(PlayerController.State.Happy);
        _state = State.Paused;
        _popupManager.OpenPopup(new PopupBase.ModelBase(PrefabDB.k_ui_win_prefab));
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
