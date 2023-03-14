using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.LSS;
using UnityEngine;

public class MenuScreen : Singleton<MenuScreen>
{
    [Header("Settings")]
    public KeyCode KeyCode = KeyCode.Escape;
    public bool sharpAnimations = false;
    public bool IsOn = false;
    
    private Animator mWindowAnimator;
    private float originalTimeScale = 1;
    
    void Start()
    {
        mWindowAnimator = gameObject.GetComponent<Animator>();
        

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode))
        {
            if (IsOn)
            {
                ModalWindowOut();
            }
            else
            {
                ModalWindowIn();
            }
        }
    }

    public void ModalWindowIn()
    {
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        gameObject.SetActive(true);

        if (IsOn == false)
        {
            if (sharpAnimations == false)
                mWindowAnimator.CrossFade("Window In", 0.1f);
            else
                mWindowAnimator.Play("Window In");

            IsOn = true;
        }
    }

    public void ModalWindowOut()
    {
        Time.timeScale = originalTimeScale;
        
        if (IsOn == true)
        {
            if (sharpAnimations == false)
                mWindowAnimator.CrossFade("Window Out", 0.1f);
            else
                mWindowAnimator.Play("Window Out");

            IsOn = false;
        }

        //StartCoroutine("DisableWindow");
    }

    IEnumerator DisableWindow()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
