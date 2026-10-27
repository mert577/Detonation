using System.Collections;
using UnityEngine;

/// <summary>
/// One thing a bomb does when it explodes. Patterns are shared assets, so keep all runtime state
/// in locals inside <see cref="Play"/>. Add a new behaviour by subclassing this; every bomb can then use it.
/// </summary>
public abstract class ExplosionPattern : ScriptableObject
{
    public abstract IEnumerator Play(ExplosionContext context);
}
