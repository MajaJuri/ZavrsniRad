using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnGameManager : MonoBehaviour
{
    public GameObject instructionText;

    public List<GameObject> prefabsToSpawn;
    public GameObject[] kontejneri;
    public string[] pozitivnePoruke;
    public string[] porukeAkoJeKrivOdgovor;
    public GameObject pauseButton;
    public GameObject endScreen;
    public Color normalColor;

    private Vector2 screenHalfSizeWorldUnits;
    public int zaRazvrstati = 10;
    private bool timeStarted = false;
    private bool gameFinished = false;

    public GameObject soundButton;
    public GameObject musicButton;
    public GameObject noSoundButton;
    public GameObject noMusicButton;

    public static float timer;

    [SerializeField]
    private AudioSource audioSource;
    public AudioClip audioCorrect;
    public AudioClip audioWrong;
    public AudioClip audioFinish;
    [SerializeField]
    private AudioSource musicSource;
    public AudioClip music;
    [SerializeField]
    private Text itemsText;
    public GameObject itemsTextObject;
    [SerializeField]
    private Text timeText;
    public GameObject timeTextObject;
    [SerializeField]
    private Text porukaText;
    public Text scoreText;

    public GameObject inputPanel;
    public InputField inputField;
    public Button okButton;
    private int correct = 0;
    private string input;

    private float gravityScale = 0.1f;

    void Start()
    {
        gravityScale = PlayerPrefs.GetFloat("brzina");
        Debug.Log("PRVI " + PlayerPrefs.GetString("prvi"));
        Debug.Log("DRUGI " + PlayerPrefs.GetString("drugi"));
        Debug.Log("TRECI " + PlayerPrefs.GetString("treci"));
        Debug.Log("TRENUTNI SCORE " + PlayerPrefs.GetInt("trenutniScore"));
        Debug.Log("TRENUTNI TIMER " + PlayerPrefs.GetFloat("trenutniTimer"));
        timer = 0;
        screenHalfSizeWorldUnits = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        SpawnNext();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("CollectibleItem"))
        {
            g.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        SetupPrefs();
    }

    void Update()
    {
        if (timeStarted == true)
        {
            timer += Time.deltaTime;
            DisplayTime(timeText);
        }
        itemsText.text = "" + zaRazvrstati;

        if (Input.anyKey)
        {
            instructionText.SetActive(false);
            timeStarted = true;
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("CollectibleItem"))
            {
                g.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            }
        }
    }

    private void SetupPrefs()
    {
        if (PlayerPrefs.GetInt("glazba") == 1)
        {
            musicSource.clip = music;
            musicSource.mute = false;
        }
        else
        {
            musicSource.mute = true;
        }

        if (PlayerPrefs.GetInt("zvuk") == 1)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;
        }
    }

    public void Correct()
    {
        porukaText.text = pozitivnePoruke[Random.Range(0, pozitivnePoruke.Length-1)];
        foreach(GameObject g in kontejneri)
        {
            g.GetComponent<BoxCollider2D>().enabled = true;
            g.GetComponent<Image>().color = normalColor;
        }
        zaRazvrstati--;
        PlayCorrect();
        SpawnNext();

        correct += 3;
    }

    public void Wrong()
    {
        porukaText.text = porukeAkoJeKrivOdgovor[Random.Range(0, porukeAkoJeKrivOdgovor.Length)];
        correct -= 1;
        PlayWrong();
    }

    public void PlayCorrect()
    {
        audioSource.clip = audioCorrect;
        audioSource.Play();
    }

    public void PlayWrong()
    {
        audioSource.clip = audioWrong;
        audioSource.Play();
    }

    public void PlayFinish()
    {
        audioSource.clip = audioFinish;
        audioSource.Play();
    }

    public void SpawnNext()
    {
        if (zaRazvrstati > 0)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x), screenHalfSizeWorldUnits.y);
            int i = Random.Range(0, prefabsToSpawn.Count - 1);
            GameObject newObject = (GameObject)Instantiate(prefabsToSpawn[i], spawnPosition, Quaternion.Euler(Vector3.forward));
            // postaviti gravity scale
            newObject.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            prefabsToSpawn.RemoveAt(i);
        }
        else
        {
            gameFinished = true;
            FinishGame();
        }
    }

    public void SpawnSame(GameObject g)
    {
        Vector2 spawnPosition = new Vector2(Random.Range(-screenHalfSizeWorldUnits.x, screenHalfSizeWorldUnits.x), screenHalfSizeWorldUnits.y);
        GameObject newObject = (GameObject)Instantiate(g, spawnPosition, Quaternion.Euler(Vector3.forward));
        newObject.GetComponent<Rigidbody2D>().gravityScale = PlayerPrefs.GetFloat("brzina");
    }

    public void Pause()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("CollectibleItem"))
        {
            Destroy(g);
        }
        timeStarted = false;
    }

    public void Unpause()
    {
        SpawnNext();
        timeStarted = true;
    }

    public string GetNiceTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        return niceTime;
    }

    public void DisplayTime(Text timeText)
    {
        timeText.text = GetNiceTime(timer);
    }

    public void RestartGame() // tu zelim da timer pocne od 0
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FinishGame()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("CollectibleItem"))
        {
            Destroy(g);
        }
        Debug.Log("Finish game");
        PlayFinish();
        timeStarted = false;
        itemsTextObject.SetActive(false);
        timeTextObject.SetActive(false);
        porukaText.text = "";
        foreach (GameObject g in kontejneri)
        {
            g.SetActive(false);
        }
        pauseButton.SetActive(false);
        endScreen.SetActive(true);

        int score = izracunajScore();
        scoreText.text = "" + score;
        Debug.Log("SCORE = " + score);
        PlayerPrefs.SetInt("trenutniScore2", score);

        promijeniLjestvicu(score);
    }

    private int izracunajScore()
    {
        int trenutniScore = PlayerPrefs.GetInt("trenutniScore");
        trenutniScore += correct * 5;

        return trenutniScore;
    }

    private void promijeniLjestvicu(int trenutniScore)
    {
        string prvi = PlayerPrefs.GetString("prvi");
        string drugi = PlayerPrefs.GetString("drugi");
        string treci = PlayerPrefs.GetString("treci");
        
        string prviName = prvi.Split('$')[0];
        int prviScore;
        if (prvi.Split('$')[1] == "--")
        {
            prviScore = 0;
        }
        else
        {
            prviScore = int.Parse(prvi.Split('$')[1]);
        }

        string drugiName = drugi.Split('$')[0];
        int drugiScore;
        if (drugi.Split('$')[1] == "--")
        {
            drugiScore = 0;
        }
        else
        {
            drugiScore = int.Parse(drugi.Split('$')[1]);
        }

        string treciName = treci.Split('$')[0];
        int treciScore;
        if (treci.Split('$')[1] == "--")
        {
            treciScore = 0;
        }
        else
        {
            treciScore = int.Parse(treci.Split('$')[1]);
        }

        okButton.onClick.RemoveAllListeners();
        if (trenutniScore >= prviScore)
        {
            otvoriInputPanel();
            okButton.onClick.AddListener(() =>
            {
                input = inputField.text;
                PlayerPrefs.SetString("prvi", input + "$" + trenutniScore);
                PlayerPrefs.SetString("drugi", prviName + "$" + prviScore);
                PlayerPrefs.SetString("treci", drugiName + "$" + drugiScore);
                onOK();
            });
        }else if(trenutniScore >= drugiScore)
        {
            otvoriInputPanel();
            okButton.onClick.AddListener(() =>
            {
                input = inputField.text;
                PlayerPrefs.SetString("drugi", input + "$" + trenutniScore);
                PlayerPrefs.SetString("treci", drugiName + "$" + drugiScore);
                onOK();
            });
        }else if(trenutniScore >= treciScore)
        {
            otvoriInputPanel();
            okButton.onClick.AddListener(() =>
            {
                input = inputField.text;
                PlayerPrefs.SetString("treci", input + "$" + trenutniScore);
                onOK();
            });
        }
    }

    private void otvoriInputPanel()
    {
        inputPanel.SetActive(true);
    }

    public void onOK()
    {
        inputPanel.SetActive(false);
    }

    public void SetPausePanel()
    {
        if (PlayerPrefs.GetInt("glazba") == 1)
        {
            noMusicButton.SetActive(true);
            musicButton.SetActive(false);
            musicSource.mute = false;
        }
        else
        {
            noMusicButton.SetActive(false);
            musicButton.SetActive(true);
            musicSource.mute = true;
        }

        if (PlayerPrefs.GetInt("zvuk") == 1)
        {
            noSoundButton.SetActive(true);
            soundButton.SetActive(false);
            audioSource.mute = false;
        }
        else
        {
            noSoundButton.SetActive(false);
            soundButton.SetActive(true);
            audioSource.mute = true;
        }
    }

    public void muteSound()
    {
        PlayerPrefs.SetInt("zvuk", 0);
        soundButton.SetActive(true);
        noSoundButton.SetActive(false);
        audioSource.mute = true;
    }

    public void unmuteSound()
    {
        PlayerPrefs.SetInt("zvuk", 1);
        soundButton.SetActive(false);
        noSoundButton.SetActive(true);
        audioSource.mute = false;
    }

    public void muteMusic()
    {
        PlayerPrefs.SetInt("glazba", 0);
        musicButton.SetActive(true);
        noMusicButton.SetActive(false);
        musicSource.mute = true;
    }

    public void unmuteMusic()
    {
        PlayerPrefs.SetInt("glazba", 1);
        musicButton.SetActive(false);
        noMusicButton.SetActive(true);
        musicSource.mute = false;
    }
}
