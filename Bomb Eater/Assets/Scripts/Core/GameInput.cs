using UnityEngine;

/// <summary>
/// The only place that reads input, so moving to the Input System later means changing this file only.
/// </summary>
public static class GameInput
{
    public static Vector2 Move => Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);

    public static bool DashPressed => Input.GetButtonDown("Jump");

    public static bool AnyKeyPressed => Input.anyKeyDown;

    public static bool ToggleFullscreenPressed => Input.GetKeyDown(KeyCode.F11);
}
