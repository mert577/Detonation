using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct ShakeSettings
{
    public float duration;
    public float strength;
    public int vibrato;

    public ShakeSettings(float duration, float strength, int vibrato = 10)
    {
        this.duration = duration;
        this.strength = strength;
        this.vibrato = vibrato;
    }
}

/// <summary>
/// Shakes the main camera around the position it had before the shake.
/// A new shake restarts from that rest position instead of drifting or snapping to a hard-coded point.
/// </summary>
public static class CameraShaker
{
    static Tween activeShake;
    static Transform shakenCamera;
    static Vector3 restPosition;

    public static void Shake(ShakeSettings settings)
    {
        Shake(settings.duration, settings.strength, settings.vibrato);
    }

    public static void Shake(float duration, float strength, int vibrato = 10)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null || duration <= 0f)
        {
            return;
        }

        Transform cameraTransform = mainCamera.transform;
        bool shaking = activeShake != null && activeShake.IsActive() && shakenCamera == cameraTransform;
        if (shaking)
        {
            activeShake.Kill();
        }
        else
        {
            restPosition = cameraTransform.position;
        }

        shakenCamera = cameraTransform;
        activeShake = cameraTransform.DOShakePosition(duration, strength, vibrato <= 0 ? 10 : vibrato)
            .OnKill(() =>
            {
                if (shakenCamera != null)
                {
                    shakenCamera.position = restPosition;
                }
            });
    }
}
