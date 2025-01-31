using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    public GameObject EnterScene;
    public GameObject ExitScene;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnLoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnLoadScene()
    {
        EnterScene.SetActive(true);
        EnterScene.GetComponent<RectTransform>().DOLocalMoveX(1600, .7f).SetEase(Ease.OutQuart).OnComplete(()=> EnterScene.SetActive(false));
    }

    public void OnReloadScene()
    {
        ExitScene.SetActive(true);
        ExitScene.GetComponent<RectTransform>().DOLocalMoveX(0, .7f).SetEase(Ease.OutQuart).OnComplete(S);


        void S()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
          // ExitScene.SetActive(false);
        }
        

    }

}
