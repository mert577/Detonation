/// <summary>
/// Per-spawn tweaks that come from the game mode (difficulty tier or act), not from the bomb type.
/// </summary>
public readonly struct BombModifiers
{
    public readonly float movementChance;
    public readonly float projectileSpeedMultiplier;

    public BombModifiers(float movementChance, float projectileSpeedMultiplier)
    {
        this.movementChance = movementChance;
        this.projectileSpeedMultiplier = projectileSpeedMultiplier;
    }

    public static BombModifiers Default => new BombModifiers(0f, 1f);
}
