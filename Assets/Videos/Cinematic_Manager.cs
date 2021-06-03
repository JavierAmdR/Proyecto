using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cinematic_Manager : MonoBehaviour
{
    public VideoPlayer cinematic;
    private bool LoadedScene = false;

    // Start is called before the first frame update
    void Start()
    {
        cinematic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (cinematic.isPlaying == true)
        {
            Debug.Log("Cinematic playing");
        }
        else
        {
            if (LoadedScene == false)
            {
                GameManager.current.StartGame();
                LoadedScene = true;
                Debug.Log("Scene Loaded");
            }
        }
    }
}
