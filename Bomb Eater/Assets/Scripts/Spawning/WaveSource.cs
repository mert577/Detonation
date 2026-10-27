using UnityEngine;

/// <summary>
/// Decides when and where bombs appear. GameManager starts whichever source it is given,
/// so switching game mode means dragging a different source into its Wave Source slot.
/// </summary>
public abstract class WaveSource : MonoBehaviour
{
    public abstract void StartSpawning();

    public abstract void StopSpawning();

    /// <summary>Text shown on the HUD for the current wave.</summary>
    public abstract string WaveLabel { get; }

    /// <summary>0 to 1 progress shown by the progress bar.</summary>
    public abstract float WaveProgress { get; }
}
