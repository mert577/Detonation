# Bomb Eater – architecture and content guide

Most content is **data**: bombs, explosion patterns, difficulty tiers and acts are ScriptableObject assets under `Assets/Content`. Code is only needed for a genuinely new *kind* of behaviour.

## How a run works

```
GameManager ──StartRun──▶ WaveSource (EndlessWaveSpawner or ActSpawner)
                              │ picks BombDefinition + position
                              ▼
                        BombSpawnService ──Instantiate shared Bomb.prefab──▶ Bomb.Init(definition, modifiers)
                                                                             │ fuse runs out
                                                                             ▼
                                                     plays definition.patterns (ExplosionPattern assets)
```

Systems don't call each other directly; they raise and listen to `GameEvents`:

| Event | Raised by | Used by |
|---|---|---|
| `RunStarted` / `RunEnded` | GameManager | UIManager (health bar, death screen) |
| `ScoreChanged` | GameManager | UIManager (score, highscore, "bombs until heal") |
| `WaveStarted` | wave sources | UIManager (wave label) |
| `BombSpawned` / `BombDefused` / `BombExploded` | BombSpawnService / Bomb | GameManager (score, heal), PlayerDash (dash refund) |
| `PlayerHealthChanged` / `PlayerDied` | PlayerHealth | HealthBar, GameManager |

Shared services: `TimeScaleController` (the only code that touches `Time.timeScale`, so hit-stops merge), `CameraShaker` (shakes around the camera's rest position), `PlayerLocator` (cached player lookup), `GameInput` (the only code that reads input).

## Folders

| Folder | What's in it |
|---|---|
| `Scripts/Core` | events, singleton base, time scale, camera shake, input, player lookup, math helpers |
| `Scripts/Game` | GameManager (flow + score), scene transitions, fullscreen, leaderboard |
| `Scripts/Bombs` | `Bomb` (the one bomb component), `BombDefinition`, `BombMovement`, `Patterns/` |
| `Scripts/Spawning` | `WaveSource` base, `EndlessWaveSpawner` + `DifficultyProfile`, `ActSpawner` + `Acts/`, `BombSpawnService`, `MissileSpawner` |
| `Scripts/Hazards` | projectiles, homing projectiles, `Damager`, damage areas, missiles, saw |
| `Scripts/Player` | movement, dash, health, defuser, graphics |
| `Scripts/UI` | HUD, health bar, progress bar, death and leaderboard panels |
| `Content/Bombs` | one `BombDefinition` per bomb type |
| `Content/Patterns` | `ExplosionPattern` assets, reusable by any bomb |
| `Content/Progression` | `EndlessDifficulty` (tiers) and `Acts_Sample` (acts/encounters) |
| `Prefabs/Bombs` | `Bomb.prefab`, shared by every bomb |

## Recipes

**New bomb (no code).** Create > Bomb Eater > Bomb Definition. Set the sprite, tint, fuse time and particles, then drag one or more patterns into *Patterns*. Add it to a tier's *Bomb Pool* in `EndlessDifficulty` (the weight sets how often it appears) or to an encounter.

**New explosion variant (no code).** Create > Bomb Eater > Explosion Patterns > *Radial Burst / Sweep / Aimed Spread / Hazard Rings / Spawn Hazard*, or duplicate an existing pattern and tweak it. Patterns can be stacked: a bomb with `Radial Burst` + `Spawn Hazard` fires a ring *and* leaves a damage area.

**New explosion behaviour (small code).** Subclass `ExplosionPattern`, add `[CreateAssetMenu]`, and implement `IEnumerator Play(ExplosionContext context)`. Use `context.LaunchProjectile(...)` / `context.Spawn(...)`, and keep runtime state in locals, because pattern assets are shared.

**New projectile or hazard.** Duplicate a prefab in `Prefabs/Projectiles` or `Prefabs/Hazards` (or make a prefab variant), then reference it from a pattern. Combine components: `Projectile`/`HomingProjectile` to move, `Damager` to hurt, `SelfDestruct`/`DOTDamager` for lifetime, `Rotate` for spin.

**Balancing endless mode.** Everything is in `Content/Progression/EndlessDifficulty`: wave interval, jitter, rush waves, and per tier the start wave, bombs per wave, movement chance, projectile speed, interval multiplier, missiles on/off and the weighted pool. Scoring and the heal interval are on the `GameManager` object.

**Designed levels (acts).** Create > Bomb Eater > Encounter Configuration. Each act has encounters; each encounter lists bombs with a position (or *Random Position*), a delay after the previous bomb and a *Moving* flag. Select the `ActSpawner` object to see the bomb positions as gizmos (*Preview Act Index* picks which act). To play acts, drag `ActSpawner` into `GameManager > Wave Source`.

**New game mode.** Subclass `WaveSource` (`StartSpawning`, `StopSpawning`, `WaveLabel`, `WaveProgress`), spawn through `BombSpawnService`, and plug it into `GameManager > Wave Source`.

## Scene notes

- `Player` is a prefab (`Prefabs/Player`). Its heal popup (`HealEffect`) and target marker (`line`) are scene objects, so they're set as overrides on the scene instance. Re-assign them if you place the player in a new scene.
- `EndlessWaveSpawner` and `ActSpawner` both live in the scene; only the one in `GameManager > Wave Source` runs.
