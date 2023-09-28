using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    PopupManager _popupManager;
    Level _activeLevel;
    int _initialLevel = 1;

    public PlayerController PlayerController => _activeLevel == null ? null : _activeLevel.PlayerInstance;
    public Level ActiveLevel => _activeLevel;

    private LevelDB _levelDB;

    [Inject]
    public void Install(PopupManager popupManager)
    {
        _popupManager = popupManager;
        _levelDB = Resources.Load<LevelDB>(PrefabDB.k_config_level_dB);
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
        _popupManager.OpenPopup(new PopupBase.ModelBase(PrefabDB.k_ui_restart_prefab));
    }

    private void OnPlayerWin(object obj)
    {
        // Show win popup and load new level
        _popupManager.OpenPopup(new PopupBase.ModelBase(PrefabDB.k_ui_win_prefab));
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

        //with this was we can load levels endlessly
        if (_levelDB.LevelConfigMap.Count >= _initialLevel)
        {
            _initialLevel = 1;
        }

        var levelToLoad = _levelDB.LevelConfigMap[_initialLevel];
        var levelPref = levelToLoad.LevelPrefab;
        _activeLevel = Instantiate(levelPref);
        _popupManager.OpenPopup(new PopupBase.ModelBase(PrefabDB.k_ui_tapToPlay_prefab));
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
