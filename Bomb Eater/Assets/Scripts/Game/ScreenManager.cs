using UnityEngine;

/// <summary>
/// Toggles fullscreen with F11. Lives across scene reloads.
/// </summary>
public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] Vector2Int windowedResolution = new Vector2Int(800, 600);
    [Tooltip("Windowed size used on 4K screens.")]
    [SerializeField] Vector2Int windowedResolution4K = new Vector2Int(1200, 900);

    public bool isFullscreen;

    protected override void Awake()
    {
        base.Awake();
        if (instance == this)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if (!GameInput.ToggleFullscreenPressed)
        {
            return;
        }

        if (isFullscreen)
        {
            ReturnFromFullscreen();
        }
        else
        {
            SetFullscreen();
        }
    }

    public void SetFullscreen()
    {
        isFullscreen = true;
        Resolution current = Screen.currentResolution;
        Screen.SetResolution(current.width, current.height, FullScreenMode.ExclusiveFullScreen);
    }

    public void ReturnFromFullscreen()
    {
        isFullscreen = false;
        bool is4K = Screen.currentResolution.width == 3840 && Screen.currentResolution.height == 2160;
        Vector2Int size = is4K ? windowedResolution4K : windowedResolution;
        Screen.SetResolution(size.x, size.y, FullScreenMode.Windowed);
    }
}
