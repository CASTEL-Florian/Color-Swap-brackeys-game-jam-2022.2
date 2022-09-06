using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private ObjectFade endTextFade;
    [SerializeField] private AudioSource audio;
    [SerializeField] private ObjectFade tutorialFade;
    [SerializeField] private Fader fader;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ObjectFade gameUiFade;
    [SerializeField] private ObjectFade waveTextFade;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private Button difficultyButton;
    [SerializeField] private GameObject spawnerPrefabEasy;
    [SerializeField] private GameObject spawnerPrefabNormal;
    [SerializeField] private GameObject spawnerPrefabHard;
    [SerializeField] private List<UnitHealth> playerHealths;
    [SerializeField] private FadeAudioSource musicFade;
    [SerializeField] private ObjectFade gameTextFade;
    private int difficulty = 1;
    private EnemySpawner spawner;
    private float time = 0;
    private bool isPlaying = false;
    private bool gameEnded = false;
    private int enemiesAlive = 0;
    private int currentWave = 0;
    private int numberOfLetters = 0;
    private bool switchingWave = false;
    void Awake()
    {
        if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
    }
    private void Start()
    {
        StartCoroutine(musicFade.StartFade(2, 0.2f));
        StartCoroutine(fader.FadeIn(1));
        if (PlayerPrefs.HasKey("difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("difficulty");
            if (difficulty == 0)
            {
                difficultyText.text = "Difficulty:Easy";
            }
            if (difficulty == 1)
            {
                difficultyText.text = "Difficulty:Normal";
            }
            if (difficulty == 2)
            {
                difficultyText.text = "Difficulty:Hard";
            }
        }
    }

    private void Update()
    {
        if (!isPlaying || gameEnded)
            return;
        time += Time.deltaTime;
        int minutes = Mathf.FloorToInt(time / 60);
        float secondes = time - (minutes * 60);
        timerText.text = minutes.ToString() + ":" + secondes.ToString("00.00", CultureInfo.InvariantCulture);
        if (enemiesAlive == 0 && !switchingWave)
        {
            if (currentWave == spawner.WaveCount())
            {
                EndGame();
                return;
            }
            else
            {
                switchingWave = true;
                currentWave += 1;
                StartCoroutine(SwitchWave());
                if (currentWave == 2)
                    StartCoroutine(tutorialFade.FadeOut());
            }
            
                
        }
    }

    private IEnumerator SwitchWave()
    {
        yield return StartCoroutine(waveTextFade.FadeOut());
        waveText.text = "Wave " + currentWave.ToString();
        yield return StartCoroutine(waveTextFade.FadeIn());
        enemiesAlive = spawner.SpawnWave();
        switchingWave = false;
    }

    public void StartGame()
    {
        if (difficulty == 0)
        {
            spawner = Instantiate(spawnerPrefabEasy).GetComponent<EnemySpawner>();
            foreach (UnitHealth playerHealth in playerHealths)
            {
                playerHealth.SetMaxHealth(6);
            }
        }
        if (difficulty == 1)
        {
            spawner = Instantiate(spawnerPrefabNormal).GetComponent<EnemySpawner>();
        }
        if (difficulty == 2)
        {
            spawner = Instantiate(spawnerPrefabHard).GetComponent<EnemySpawner>();
        }
        isPlaying = true;
        currentWave = 1;
        waveText.text = "Wave " + currentWave.ToString();
        enemiesAlive = spawner.SpawnWave();
        playerManager.enabled = true;
        StartCoroutine(gameUiFade.FadeIn());
        StartCoroutine(waveTextFade.FadeIn());
        StartCoroutine(difficultyText.GetComponent<ObjectFade>().FadeOut());
        StartCoroutine(tutorialFade.FadeIn());
        StartCoroutine(gameTextFade.FadeOut(0.3f));
        difficultyButton.GetComponent<PlayButton>().enabled = false;
        difficultyButton.enabled = false;
    }

    public void EnemyDead(Vector3 postition)
    {
        enemiesAlive -= 1;
    }

    public void LetterDestroyed()
    {
        numberOfLetters -= 1;
        if (numberOfLetters == 0)
        {
            StartCoroutine(musicFade.StartFade(1, 0));
            StartCoroutine(fader.TransitionToScene(SceneManager.GetActiveScene().buildIndex));
        }
    }

    private void EndGame()
    {
        if (gameEnded)
            return;
        gameEnded = true;
        
        StartCoroutine(waveTextFade.FadeOut());
        StartCoroutine(EndTextRoutine());
        numberOfLetters = 17;

    }

    public void PlayAudio(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }

    public void PlayerDead()
    {
        gameEnded = true;
        StartCoroutine(musicFade.StartFade(0.5f, 0));
        StartCoroutine(fader.TransitionToScene(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator EndTextRoutine()
    {
        yield return new WaitForSeconds(1);
        endTextFade.gameObject.SetActive(true);
        yield return endTextFade.FadeIn();
    }

    public void ChangeDifficulty()
    {
        difficulty += 1;
        if (difficulty > 2)
            difficulty = 0;
        PlayerPrefs.SetInt("difficulty", difficulty);
        if (difficulty == 0)
        {
            difficultyText.text = "Difficulty:Easy";
        }
        if (difficulty == 1)
        {
            difficultyText.text = "Difficulty:Normal";
        }
        if (difficulty == 2)
        {
            difficultyText.text = "Difficulty:Hard";
        }
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
