using UnityEngine;
using Zenject;

public abstract class PopupBase : MonoBehaviour
{
    [Inject] protected LevelManager _levelManager;

    public class ModelBase
    {
        public string PrefabName { get; private set; }
        public ModelBase(string prefabName)
        {
            PrefabName = prefabName;
        }
    }

    private ModelBase _model;

    public virtual void Initialize(ModelBase model)
    {
        _model = model;
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide() 
    {
        gameObject.SetActive(false);
    }
}
