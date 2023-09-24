using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlayPopup : PopupBase
{
    public void OnClickTap()
    {
        LevelManager.Instance.StartLevel();
        Hide();
    }
}
