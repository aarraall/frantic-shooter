using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UpgradeButton : MonoBehaviour
{
    [field: SerializeField] public TMP_Text upgradeLabel;
    [field: SerializeField] public Image upgradeImage;

    Action<WeaponUpgradeType> buttonCallback;
    WeaponUpgradeType _upgradeType;
    public void Initialize(WeaponUpgradeType upgradeType,Action<WeaponUpgradeType> buttonCall, Sprite upgradeImage = null)
    {
        _upgradeType = upgradeType;
        buttonCallback = null;
        buttonCallback = buttonCall;
        this.upgradeImage.sprite = upgradeImage;
        upgradeLabel.text = _upgradeType.ToString();
    }

    public void OnClick()
    {
        buttonCallback?.Invoke(_upgradeType);
    }

    private void Reset()
    {
        upgradeImage = GetComponentInChildren<Image>(true);
        upgradeLabel = GetComponentInChildren<TMP_Text>(true);
    }
}
