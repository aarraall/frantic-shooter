using DG.Tweening;
using UnityEngine;
using Zenject;

public class Gate : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    [SerializeField] Weapon weapon;

    [Inject] LevelManager _levelManager;

    private void Start()
    {
        var createdWeapon = Instantiate(weapon);
        createdWeapon.transform.SetParent(weaponTransform);
        createdWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        createdWeapon.transform.localScale = Vector3.one;
        weaponTransform
            .DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        _levelManager.PlayerController.OnTakeWeapon(weapon);
    }
}
