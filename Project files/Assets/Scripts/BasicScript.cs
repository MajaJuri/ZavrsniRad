using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BasicScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource musicSource;
    public AudioClip music;

    public void GoTo(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
    }

    void Start()
    {
        SetupPrefs();
    }

    public void openURL(string url) {
        Debug.Log("opening " + url);
        Application.OpenURL(url);
    }

    private void SetupPrefs()
    {
        if (PlayerPrefs.GetInt("glazba") == 1)
        {
            musicSource.clip = music;
            musicSource.mute = false;
            musicSource.Play(0);
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

    public void GoToNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Debug.Log("Quit game.");
        Application.Quit();
    }
}
