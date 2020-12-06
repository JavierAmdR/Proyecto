﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transition;
    public static GameManager current;
    public float transitionTime = 1f;

    void Awake()
    {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        current = this;
    }

    public void ChangeTimeScale (float timeScale) 
    {
        Time.timeScale = timeScale;
    }

    public void StartGame() 
    {
        LoadScene(1);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void LoadScene(int newScene) 
    {
        StartCoroutine(LoadCrossfade(newScene));
    }

    IEnumerator LoadCrossfade(int sceneIndex) 
    {
        transition.SetTrigger("CrossfadeStart");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(sceneIndex);
        while(SceneManager.GetActiveScene().buildIndex != sceneIndex) 
        {
            yield return null;
        }
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex) 
        {
            transition.SetTrigger("CrossfadeEnd");
        }
    }
    
}
