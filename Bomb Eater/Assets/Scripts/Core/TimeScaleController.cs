using DG.Tweening;
using UnityEngine;

/// <summary>
/// The only place that changes Time.timeScale. Overlapping hit-stops merge (slowest scale, latest end)
/// instead of resetting each other.
/// </summary>
public static class TimeScaleController
{
    static Tween activeTween;
    static float activeScale = 1f;
    static float activeUntil;

    public static void Hitstop(float scale, float duration, bool easeIn = false)
    {
        float now = Time.unscaledTime;
        bool running = activeTween != null && activeTween.IsActive();
        if (running && activeScale <= scale && activeUntil >= now + duration)
        {
            return;
        }

        activeTween?.Kill();
        activeScale = running ? Mathf.Min(activeScale, scale) : scale;
        activeUntil = Mathf.Max(running ? activeUntil : 0f, now + duration);
        float remaining = activeUntil - now;

        if (easeIn)
        {
            activeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, activeScale, remaining)
                .SetUpdate(true)
                .OnComplete(Restore);
        }
        else
        {
            Time.timeScale = activeScale;
            activeTween = DOVirtual.DelayedCall(remaining, Restore, true);
        }
    }

    public static void ResetTimeScale()
    {
        activeTween?.Kill();
        Restore();
    }

    static void Restore()
    {
        activeTween = null;
        activeScale = 1f;
        Time.timeScale = 1f;
    }
}
