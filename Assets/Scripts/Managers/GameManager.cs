using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager current;

    void Awake()
    {
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
    
}
