using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows the active wave source's progress, e.g. time until the next endless wave.
/// </summary>
public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image image;

    void Update()
    {
        WaveSource source = GameManager.instance != null ? GameManager.instance.WaveSource : null;
        image.fillAmount = source != null ? source.WaveProgress : 0f;
    }
}
