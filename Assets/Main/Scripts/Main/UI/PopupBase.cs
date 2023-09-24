using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MVP or MVVM could go better here but no time for it
/// </summary>
public class PopupBase : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide() 
    {
        gameObject.SetActive(false);
    }
}
