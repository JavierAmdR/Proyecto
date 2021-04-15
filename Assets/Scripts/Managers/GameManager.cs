using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum gameResult { Victory, Defeat};
    public gameResult currentResult;
    public enum gameState {Intro ,MainMenu, Gameplay}
    public gameState currentState;

    public GameObject exploringMusic;
    public Animator transition;
    public static GameManager current;
    public float transitionTime = 1f;
    public bool playingIntro = true;
    float counter = 0f;
    public GameObject defeatText;
    public GameObject victoryText;
    public GameObject godModeUI;
    public int roomCounter = 1;

    public float healthLost = 0f;
    public float duration = 0f;
    public int attacksReceived = 0;
    public int upgradesObtained = 0;
    public int enemiesDefeated = 0;

    public int enemiesInRoom = 0;

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
        switch (SceneManager.GetActiveScene().buildIndex) 
        {
            case 0:
                ChangeGameState(gameState.Intro);
                PlayIntro();
                break;
            case 1:
                ChangeGameState(gameState.MainMenu);
                break;
            default: 
                ChangeGameState(gameState.Gameplay);
                break;
        }        
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
        if (SceneManager.GetActiveScene().buildIndex == 2 && PlayerStats.current.currentHealth != PlayerStats.current.health.GetValue()) 
        {
            PlayerStats.current.currentHealth = PlayerStats.current.health.GetValue();
        }
        CalculateDuration();
    }

    public void CalculateDuration()
    {
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            duration += Time.deltaTime;
        }
    }

    public void AddEnemyDefeated()
    {
        enemiesDefeated += 1;
    }

    public void AddUpgradesObtained()
    {
        upgradesObtained += 1;
    }

    public void AddAttacksReceived()
    {
        attacksReceived += 1;
    }

    public void AddHealthLost(float damageReceived)
    {
        healthLost += damageReceived;
    }

    public void PlayIntro() 
    {
        transition.SetTrigger("CrossfadeEnd");
        ChangeGameState(gameState.MainMenu);
    }
    void Start()
    {
        current = this;
    }

    public void AddEnemy() 
    {
        enemiesInRoom += 1;
    }

    public void RemoveEnemy() 
    {
        enemiesInRoom -= 1;
        if (enemiesInRoom <= 0) 
        {
            ActivateExit();
        }
    }

    public void ActivateExit() 
    {
        GameObject.Find("Exit").GetComponent<RoomEntrance>().EnableCollider();
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
        exploringMusic.SetActive(true);
        ChangeGameState(gameState.Gameplay);
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void LoadScene(int newScene) 
    {
        StartCoroutine(LoadCrossfade(newScene));
    }

    public void LoadReset()
    {
        StartCoroutine(ResetGame());
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
        StartCoroutine(LoadCrossfade(Random.Range(3, 8)));
    }

    private void ChangeGameState(gameState newState) 
    {
        currentState = newState;
    }


    IEnumerator LoadCrossfade(int sceneIndex) 
    {
        Debug.Log(sceneIndex);
        if (sceneIndex < 1) 
        {
            exploringMusic.GetComponent<AudioSource>().Stop();
            exploringMusic.SetActive(false);
        }
        transition.SetTrigger("CrossfadeStart");
        yield return new WaitForSeconds(transitionTime);
        if (roomCounter >= 6) 
        {
            sceneIndex = 9;
            SceneManager.LoadSceneAsync(sceneIndex);
            roomCounter++;
        }
        else 
        {
            if (roomCounter % 2 != 0 || roomCounter < 2)
            {
                SceneManager.LoadSceneAsync(sceneIndex);
                roomCounter++;
            }
            else
            {
                sceneIndex = 8;
                SceneManager.LoadSceneAsync(sceneIndex);
                roomCounter++;
            }
        }
        while(SceneManager.GetActiveScene().buildIndex != sceneIndex) 
        {
            yield return null;
        }
        if (sceneIndex > 1) 
        {
            if (PlayerController.current != null)
            {
                PlayerController.current.characterController.enabled = false;
                PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
                PlayerController.current.characterController.enabled = true;
            }
        }
        else 
        {
            Destroy(PlayerController.current.gameObject);
            Destroy(UIManager.ui.gameObject);
            Destroy(CameraManager.current.gameObject);
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

    IEnumerator ResetGame()
    {
        transition.SetTrigger("CrossfadeStart");
        SceneManager.LoadSceneAsync("TitleScreen");
        yield return new WaitForSeconds(3f);
        while (SceneManager.GetActiveScene().name != "TitleScreen")
        {
            yield return null;
        }
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            roomCounter = 1;
            transition.SetTrigger("TitleScreen");
            //GameObject.Find("Defeat").SetActive(true);
            //defeatText.SetActive(false);
        }
    }

    IEnumerator LoadCrossfadeDefeat() 
    {
        currentResult = gameResult.Defeat;
        transition.SetTrigger("CrossfadeStart");
        //defeatText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("EndScreen");
        while (SceneManager.GetActiveScene().name != "EndScreen")
        {
            yield return null;
        }
        if (PlayerController.current != null)
        {
            //PlayerController.current.characterController.enabled = false;
            //PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            //PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            roomCounter = 1;
            transition.SetTrigger("CrossfadeEnd");
            //GameObject.Find("Defeat").SetActive(true);
            //defeatText.SetActive(false);
        }
    }

    IEnumerator LoadCrossfadeVictory()
    {
        currentResult = gameResult.Victory;
        transition.SetTrigger("CrossfadeStart");
        //victoryText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("EndScreen");
        while (SceneManager.GetActiveScene().name != "EndScreen")
        {
            yield return null;
        }
        if (PlayerController.current != null)
        {
            //PlayerController.current.characterController.enabled = false;
            //PlayerController.current.transform.position = GameObject.Find("PlayerSpawn").transform.position;
            //PlayerController.current.characterController.enabled = true;
        }
        if (SceneManager.GetActiveScene().name == "EndScreen")
        {
            roomCounter = 1;
            transition.SetTrigger("CrossfadeEnd");
            //GameObject.Find("Victory").SetActive(true);
            //victoryText.SetActive(false);
        }
    }

}
