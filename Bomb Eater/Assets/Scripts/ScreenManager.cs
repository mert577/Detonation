using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public static ScreenManager instance;
    public Vector2Int startResouliton;

    bool firstTimeGameOpen = true;

    public bool isFullscreen = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


        DontDestroyOnLoad(this.gameObject);

       

    }
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (isFullscreen)
            {
                isFullscreen = false;
                ReturnFromFullscreen();
            }
            else
            {
                isFullscreen = true;
                SetFullscreen();
            }
        }
    }

    
    public void SetFullscreen()
    {
        Resolution currentRes = Screen.currentResolution;
        FullScreenMode fullScreenMode = FullScreenMode.ExclusiveFullScreen;

        Screen.SetResolution(currentRes.width, currentRes.height, fullScreenMode);
    }


    public void ReturnFromFullscreen()
    {
        isFullscreen = false;
        //if screen is 4K the window size will be 1200x900
        if(Screen.currentResolution.width == 3840 && Screen.currentResolution.height == 2160)
        {
            Screen.SetResolution(1200, 900, FullScreenMode.Windowed);
        }
        else
        {
            Screen.SetResolution(800, 600, FullScreenMode.Windowed);
        }
      
        
    }


}
