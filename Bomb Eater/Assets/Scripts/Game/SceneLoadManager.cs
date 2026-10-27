using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Slides the transition panels in and out around scene (re)loads.
/// </summary>
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    public GameObject EnterScene;
    public GameObject ExitScene;

    [SerializeField] float transitionTime = .7f;
    [Tooltip("Local X the enter panel slides to when the scene starts.")]
    [SerializeField] float enterPanelExitX = 1600f;

    void Start()
    {
        OnLoadScene();
    }

    public void OnLoadScene()
    {
        EnterScene.SetActive(true);
        EnterScene.GetComponent<RectTransform>().DOLocalMoveX(enterPanelExitX, transitionTime).SetEase(Ease.OutQuart)
            .OnComplete(() => EnterScene.SetActive(false));
    }

    public void OnReloadScene()
    {
        ExitScene.SetActive(true);
        ExitScene.GetComponent<RectTransform>().DOLocalMoveX(0f, transitionTime).SetEase(Ease.OutQuart).SetUpdate(true)
            .OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
