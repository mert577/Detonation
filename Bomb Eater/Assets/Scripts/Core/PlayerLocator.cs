using UnityEngine;

/// <summary>
/// Finds the object tagged "Player" once and caches it. Re-finds it automatically after a scene reload.
/// </summary>
public static class PlayerLocator
{
    static Transform player;

    public static Transform Transform
    {
        get
        {
            if (player == null)
            {
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                player = playerObject != null ? playerObject.transform : null;
            }

            return player;
        }
    }
}
