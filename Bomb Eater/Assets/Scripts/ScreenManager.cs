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

        startResouliton = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);


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
                Screen.SetResolution((int)startResouliton.x, (int)startResouliton.y, false);
            }
            else
            {
                isFullscreen = true;
                SetFullscreen();
            }
        }
    }

    //sets to full screen and sets the aspect ratio to 4:3
    public void SetFullscreen()
    {
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        Camera.main.aspect = 4f / 3f;

    }


}
