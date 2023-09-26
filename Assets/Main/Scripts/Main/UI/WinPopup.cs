using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : PopupBase
{
    public void LoadNextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }
}
