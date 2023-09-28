using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] PopupManager popupManager;
    [SerializeField] Joystick joyStick;

    public override void InstallBindings()
    {
        Container.BindInstance(Camera.main).AsSingle().NonLazy();
        Container.BindInstance(joyStick).AsSingle().NonLazy();
        Container.BindInstance(popupManager).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<FrustumService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
    }
}
