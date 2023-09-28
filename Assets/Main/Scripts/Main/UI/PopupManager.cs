using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    List<PopupBase> _activePopups = new List<PopupBase> ();
    public void OpenPopup(PopupBase.ModelBase model)
    {
        HideAll();
        PopupBase popup = Instantiate(Resources.Load<PopupBase>(model.PrefabName));
        popup.transform.SetParent(transform);
        popup.Initialize(model);
        popup.Show();
    }

    public void ClosePopup(Type popupType)
    {
        var popup = _activePopups.FirstOrDefault(popup => popup.GetType() == popupType);
        popup.Hide();
        Destroy(popup);
    }

    public void HideAll()
    {
        foreach (var popup in _activePopups)
        {
            popup.Hide();
        }
    }
}
