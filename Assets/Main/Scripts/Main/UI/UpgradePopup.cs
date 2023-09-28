using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class UpgradePopup : PopupBase
{
    [SerializeField] List<UpgradeButton> upgradeButtons = new List<UpgradeButton>();

    public override void Show()
    {
        base.Show();
        Initialize();
    }
    public void Initialize()
    {
        var upgradeOffers = _levelManager.PlayerController.Weapon.ReturnThreeUpgrades();

        var noUpgrades = upgradeOffers == null || upgradeOffers.Count == 0;

        if (noUpgrades)
        {
            Hide();
            _levelManager.StartLevel();
            return;
        }
        var idleButtonCount = upgradeButtons.Count - upgradeOffers.Count;


        for (int i = 0; i < upgradeButtons.Count; i++)
        {
            var button = upgradeButtons[i];

            var isButtonIdle = i > upgradeOffers.Count - 1;

            if (isButtonIdle)
            {
                button.gameObject.SetActive(false);
                continue;
            }

            var upgradeOffer = upgradeOffers[i];
            button.gameObject.SetActive(true);
            button.Initialize(upgradeOffer, OnUpdateButtonClick, null);
        }


    }

    private void OnUpdateButtonClick(WeaponUpgradeType type)
    {
        _levelManager.PlayerController.Weapon.UpgradeLevel(type);
        Hide();
        _levelManager.StartLevel();
    }

    private void Reset()
    {
        upgradeButtons = GetComponentsInChildren<UpgradeButton>(true).ToList();
    }
}
