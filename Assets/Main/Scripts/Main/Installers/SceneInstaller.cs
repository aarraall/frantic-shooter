using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] FrustumService frustumService;
    [SerializeField] LevelManager levelManager;
    [SerializeField] PopupManager popupManager;
    [SerializeField] Joystick joyStick;
    [SerializeField] CameraService cameraService;

    public override void InstallBindings()
    {
        Container.BindInstance(Camera.main).AsSingle().NonLazy();
        Container.BindInstance(joyStick).AsSingle().NonLazy();
        Container.BindInstance(popupManager).AsSingle().NonLazy();
        Container.BindInstance(frustumService).AsSingle().NonLazy();
        Container.BindInstance(cameraService).AsSingle().NonLazy();
        Container.BindInstance(levelManager).AsSingle().NonLazy();
    }
}
