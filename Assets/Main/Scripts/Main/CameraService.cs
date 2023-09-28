using Cinemachine;
using UnityEngine;
using Zenject;

/// <summary>
/// We can add more functionality here like animated camera, zoom in target, zoom out target etc.
/// </summary>
/// <param name="target"></param>
public class CameraService : MonoBehaviour
{
    CinemachineVirtualCameraBase _followCam;
    [Inject] Camera _mainCamera;

    private void Awake()
    {
        CreateFollowCamera();
    }

    public void CreateFollowCamera()
    {
        if (_followCam != null)
        {
            return;
        }

        _followCam = Instantiate(Resources.Load<CinemachineVirtualCameraBase>(PrefabDB.k_follow_cam_prefab), transform);
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
