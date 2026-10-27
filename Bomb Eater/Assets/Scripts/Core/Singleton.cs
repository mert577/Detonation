using UnityEngine;

/// <summary>
/// Scene singleton. A duplicate destroys its own GameObject; derived Awake overrides should
/// call base.Awake() and return early when <c>instance != this</c>.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
