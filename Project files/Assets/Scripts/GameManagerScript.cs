using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public float restartDelay = 0.5f;
    public static float timer;
    public static bool timeStarted = false;

    public GameObject player;
    public GameObject soundButton;
    public GameObject musicButton;
    public GameObject noSoundButton;
    public GameObject noMusicButton;

    private Rigidbody2D m_Rigidbody2D;

    public float minY;
    public Text timeText;

    private bool startedLevel = false;

    public int items = 0;

    [SerializeField]
    private Text itemsText;
    [SerializeField]
    public AudioSource audioSource;
    public AudioClip audioClip;

    [SerializeField]
    public AudioSource musicSource;
    public AudioClip music;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PRVI " + PlayerPrefs.GetString("prvi"));
        Debug.Log("DRUGI " + PlayerPrefs.GetString("drugi"));
        Debug.Log("TRECI " + PlayerPrefs.GetString("treci"));
        PlayerPrefs.SetInt("trenutniScore", 0);
        Debug.Log("TRENUTNI SCORE " + PlayerPrefs.GetInt("trenutniScore"));
        
        if (PlayerPrefs.GetInt("timer_zero") == 1)
        {
            timer = 0f;
        }
        else
        {
            timer = PlayerPrefs.GetFloat("trenutniTimer");
        }

        timeStarted = false;
        m_Rigidbody2D = player.GetComponent<Rigidbody2D>();

        DisplayTime(timeText);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && startedLevel==false)
        {
            startedLevel = true;
            timeStarted = true;
            Unpause();
            DisplayTime(timeText);
        }
        if (timeStarted == true)
        {
            timer += Time.deltaTime;
            PlayerPrefs.SetFloat("trenutniTimer", timer);
            DisplayTime(timeText);
        }
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

    public void PlayerFallOff()
    {
        Debug.Log("PlayerFallOff");
        Invoke("PlayFromBeginning", restartDelay);
    }

    public void Pause()
    {
        timeStarted = false;
        (player.GetComponent("Platformer2DUserControl") as Behaviour).enabled = false;
        (player.GetComponent("Animator") as Behaviour).enabled = false;
    }

    public void Unpause()
    {
        timeStarted = true;
        (player.GetComponent("Platformer2DUserControl") as Behaviour).enabled = true;
        (player.GetComponent("Animator") as Behaviour).enabled = true;
    }

    public void RestartGame() // tu zelim da timer pocne od 0
    {
        timer = 0;
        PlayerPrefs.SetInt("timer_zero", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlayFromBeginning() // tu NE zelim da timer pocne od 0
    {
        PlayerPrefs.SetInt("timer_zero", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {
        if (player.transform.position.y < minY)
        {
            PlayerFallOff();
        }
    }

    public void Collect()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        items += 1;
        itemsText.text = "" + items;
    }

    public void Collect(string itemName)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        items += 1;
        itemsText.text = "" + items;
        PlayerPrefs.SetInt("trenutniScore", items);
    }

    public int GetNoOfItems()
    {
        return items;
    }

   public void SetPausePanel()
    {
        if(PlayerPrefs.GetInt("glazba") == 1)
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

        if(PlayerPrefs.GetInt("zvuk") == 1)
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
