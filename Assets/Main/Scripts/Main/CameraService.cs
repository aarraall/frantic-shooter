using Cinemachine;
using UnityEngine;
using Zenject;

/// <summary>
/// We can add more functionality here like animated camera, zoom in target, zoom out target etc.
/// </summary>
/// <param name="target"></param>
public class CameraService : IInitializable
{
    CinemachineVirtualCameraBase _followCam;

    public void Initialize()
    {
        CreateFollowCamera();
    }

    public void CreateFollowCamera()
    {
        if (_followCam != null)
        {
            return;
        }

        _followCam = Object.Instantiate(Resources.Load<CinemachineVirtualCameraBase>(PrefabDB.k_follow_cam_prefab));
    }


    public void FollowTarget(Transform target)
    {
        _followCam.Follow = target;
    }

    public void LookAtTarget(Transform target)
    {
        _followCam.LookAt = target;
    }

   
}
