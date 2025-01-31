using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerGraphics : MonoBehaviour
{


    public GameObject graphicsObject;


    [SerializeField] GameObject healthAnimation;


    [SerializeField] float moveAmount;
    [SerializeField] float animationTime;

    [SerializeField] AudioClip healSound;
    [SerializeField] AudioSource audioSource;




    public void PlayHealthAnimation(){
        healthAnimation.SetActive(true);
        healthAnimation.transform.position = transform.position;
        healthAnimation.transform.DOMoveY(transform.position.y+ moveAmount,animationTime).SetEase(Ease.OutQuint).OnComplete( () => healthAnimation.SetActive(false));
        audioSource.PlayOneShot(healSound);
        
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
