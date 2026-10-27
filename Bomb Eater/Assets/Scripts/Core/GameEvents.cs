using System;

/// <summary>
/// Game-wide notifications. Systems raise these instead of calling each other directly.
/// Listeners subscribe in OnEnable and unsubscribe in OnDisable.
/// </summary>
public static class GameEvents
{
    public static event Action RunStarted;
    public static event Action RunEnded;
    public static event Action<int> ScoreChanged;
    public static event Action<int> WaveStarted;
    public static event Action<Bomb> BombSpawned;
    public static event Action<Bomb> BombDefused;
    public static event Action<Bomb> BombExploded;
    public static event Action<int, int> PlayerHealthChanged;
    public static event Action PlayerDied;

    public static void RaiseRunStarted() => RunStarted?.Invoke();
    public static void RaiseRunEnded() => RunEnded?.Invoke();
    public static void RaiseScoreChanged(int score) => ScoreChanged?.Invoke(score);
    public static void RaiseWaveStarted(int wave) => WaveStarted?.Invoke(wave);
    public static void RaiseBombSpawned(Bomb bomb) => BombSpawned?.Invoke(bomb);
    public static void RaiseBombDefused(Bomb bomb) => BombDefused?.Invoke(bomb);
    public static void RaiseBombExploded(Bomb bomb) => BombExploded?.Invoke(bomb);
    public static void RaisePlayerHealthChanged(int current, int max) => PlayerHealthChanged?.Invoke(current, max);
    public static void RaisePlayerDied() => PlayerDied?.Invoke();
}
