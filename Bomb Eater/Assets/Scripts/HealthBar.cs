using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{


    public List<Image> heartIcons;
    public List<Vector2> heartIconPositions;

    [SerializeField] PlayerHealth pHealth;


    bool shaking = false;

    public Color activeColor;

    public Color inActiveColor;
    // Start is called before the first frame update
    void Start()
    {
        heartIconPositions = new List<Vector2>();
        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIconPositions.Add(heartIcons[i].rectTransform.anchoredPosition);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        int countToSetInActive = pHealth.MaxHealth - pHealth.GetCurrentHealth();

        if (countToSetInActive <= 1)
        {
            if(!shaking)
                StartShake();
        }
        else
        {
            EndShake();
        }
        for (int i=0;i< heartIcons.Count; i++)
        {

          

            if (i<countToSetInActive)
            {
                heartIcons[i].color = inActiveColor;
            }
            else
            {
                heartIcons[i].color = activeColor;
            }

            
            
        }



    }




    public void StartShake()
    {
        shaking = true;


    }

    public void EndShake()
    {
        shaking = false;
    }
}
