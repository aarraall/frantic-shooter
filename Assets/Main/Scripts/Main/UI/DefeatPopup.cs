using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatPopup : PopupBase
{
    public void RestartLevel()
    {
        _levelManager.RestartLevel();
    }
}
