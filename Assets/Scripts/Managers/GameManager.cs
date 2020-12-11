using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transition;
    public static GameManager current;
    public float transitionTime = 1f;
    public bool playingIntro = true;
    float counter = 0f;
    public GameObject defeatText;
    public GameObject victoryText;
    public GameObject godModeUI;
    public int roomCounter = 1;

    public int currency = 0;

    void Awake()
    {
        transition = GameObject.Find("Crossfade").GetComponent<Animator>();
        DontDestroyOnLoad(this);

        if (current == null) 
        {
            current = this;
        }
        else 
        {
            Object.Destroy(gameObject);
        }
        PlayIntro();
    }

    public void Update()
    {
        if (playingIntro == true) 
        {
            counter += Time.deltaTime;
            if (counter >= 2f) 
            {
                counter = 0f;
                playingIntro = false;
                StartCoroutine(LoadIntro(1));
            }
        }
    }

    public void PlayIntro() 
    {
        transition.SetTrigger("CrossfadeEnd");
    }
    void Start()
    {
        current = this;
    }

    public void AddCurency(int newCurrency) 
    {
        currency += newCurrency;
        UIManager.ui.CurrencyUpdate();
    }

    public void ChangeTimeScale (float timeScale) 
    {
        Time.timeScale = timeScale;
    }

    public void StartGame() 
    {
        LoadScene(2);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void LoadScene(int newScene) 
    {
        StartCoroutine(LoadCrossfade(newScene));
    }

    public void LoadDefeat() 
    {
        StartCoroutine(LoadCrossfadeDefeat());
    }
    public void LoadVictory()
    {
        StartCoroutine(LoadCrossfadeVictory());
    }

    public void LoadSceneRandomZone1()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        StartCoroutine(LoadCrossfade(Random.Range(3, 14)));
    }


    IEnumerator LoadCrossfade(int sceneIndex) 
    {
        Debug.Log(sceneIndex);
        transition.SetTrigger("CrossfadeStart");
        yield return new WaitForSeconds(transitionTime);
        if (roomCounter >= 10) 
        {
            sceneIndex = 15;
            SceneManager.LoadSceneAsync(sceneIndex);
            roomCounter++;
        }
        else 
        {
            if (roomCounter % 3 != 0 || roomCounter < 3)
            {
                SceneManager.LoadSceneAsync(sceneIndex);
                roomCounter++;
            }
            else
            {
                sceneIndex = 14;
                SceneManager.LoadSceneAsync(sceneIndex);
                roomCounter++;
            }
        }
        while(SceneManager.GetActiveScene().buildIndex != sceneIndex) 
        {
            yield return null;
        }
        if (PlayerController.current != null) 
        {
            PlayerController.current.characterController.enabled = false;
            PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex) 
        {
            transition.SetTrigger("CrossfadeEnd");
        }
    }

    IEnumerator LoadIntro(int sceneIndex) 
    {
        transition.SetTrigger("CrossfadeStart");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(sceneIndex);
        while (SceneManager.GetActiveScene().buildIndex != sceneIndex)
        {
            yield return null;
        }
        if (PlayerController.current != null)
        {
            PlayerController.current.characterController.enabled = false;
            PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            transition.SetTrigger("CrossfadeEnd");
        }
    }

    IEnumerator LoadCrossfadeDefeat() 
    {
        transition.SetTrigger("CrossfadeStart");
        defeatText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(2);
        while (SceneManager.GetActiveScene().buildIndex != 2)
        {
            yield return null;
        }
        if (PlayerController.current != null)
        {
            PlayerController.current.characterController.enabled = false;
            PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            roomCounter = 0;
            transition.SetTrigger("CrossfadeEnd");
            defeatText.SetActive(false);
        }
    }

    IEnumerator LoadCrossfadeVictory()
    {
        transition.SetTrigger("CrossfadeStart");
        victoryText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(2);
        while (SceneManager.GetActiveScene().buildIndex != 2)
        {
            yield return null;
        }
        if (PlayerController.current != null)
        {
            PlayerController.current.characterController.enabled = false;
            PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            roomCounter = 0;
            transition.SetTrigger("CrossfadeEnd");
            victoryText.SetActive(false);
        }
    }

}
