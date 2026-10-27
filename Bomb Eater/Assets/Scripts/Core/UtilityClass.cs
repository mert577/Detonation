using UnityEngine;

public static class UtilityClass
{
    /// <summary>Angle in degrees of a direction, measured from the positive X axis.</summary>
    public static float GetRotationFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    /// <summary>Unit direction for an angle in degrees, measured from the positive X axis.</summary>
    public static Vector2 GetDirectionFromRotation(float rotation)
    {
        float angle = rotation * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static Vector2 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
    }
}
