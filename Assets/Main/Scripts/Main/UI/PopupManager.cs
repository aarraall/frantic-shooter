using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private List<PopupBase> popups;
    public static PopupManager Instance;

    private void Awake()
    {
        Instance = this;
        HideAll();
    }

    public void OpenPopup(Type popupType)
    {
        // TODO : Usually I create a popup system that supports many async popup queries by enqueueing them
        // I can evolve this to smh like that later

        HideAll();
        var popup = popups.FirstOrDefault(popup => popup.GetType() == popupType);
        popup.Show();
    }

    public void ClosePopup(Type popupType)
    {
        var popup = popups.FirstOrDefault(popup => popup.GetType() == popupType);
        popup.Hide();
    }

    public void HideAll()
    {
        foreach (var popup in popups)
        {
            popup.Hide();
        }
    }


    private void Reset()
    {
        popups = GetComponentsInChildren<PopupBase>().ToList();
    }
}
