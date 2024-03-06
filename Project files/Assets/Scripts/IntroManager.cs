using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public GameObject[] storylinePanels;
    public GameObject skipButton;
    public GameObject nextButton;

    private bool canGetNextPanel = false;
    private int index = 0;

    private GameObject currentActivePanel;
    private Transform[] children;
    private int childrenIndex = 1;

    public AudioSource audioSource;
    public AudioClip clickclip;

    public AudioSource musicSource;

    public AudioSource narationSource;
    public AudioClip[] narationClips;
    private int narationIndex = 0;

    void Start()
    {
        if (PlayerPrefs.GetInt("naracija") > 0)
        {
            musicSource.volume = 0.1f;
        }
        else
        {
            musicSource.volume = 0.6f;
        }
        
        naracija();
    }

    void Update()
    {
        if (nextButton.active)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                nextButton.GetComponent<Button>().onClick.Invoke();
            }
        }

        for (int i = 0; i < storylinePanels.Length; i++)
        {
            if (i == index)
            {
                storylinePanels[i].SetActive(true);
                currentActivePanel = storylinePanels[i];
                children = currentActivePanel.GetComponentsInChildren<Transform>(true);
            }
            else
            {
                storylinePanels[i].SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            naracija();
            if (!canGetNextPanel)
            {
                klik();
                childrenIndex++;
                foreach (Transform t in children)
                {
                    if (t.name == children[childrenIndex].name)
                    {
                        t.gameObject.SetActive(true);
                    }
                }
                if (children[childrenIndex].name == "fin")
                {
                    canGetNextPanel = true;
                }
                else
                {
                    canGetNextPanel = false;
                }
            }
            else
            {
                if(currentActivePanel.name == "FourthSet")
                {
                    skipButton.SetActive(false);
                    nextButton.SetActive(true);
                }
                else
                {
                    klik();
                    index++;
                    canGetNextPanel = false;
                    childrenIndex = 1;
                }
            }
        }
    }

    void klik()
    {
        if (PlayerPrefs.GetInt("zvuk") == 1)
        {
            audioSource.clip = clickclip;
            audioSource.Play(0);
        }
    }

    void naracija()
    {
        if (PlayerPrefs.GetInt("naracija") > 0)
        {
            if (narationIndex < narationClips.Length)
            {
                narationSource.clip = narationClips[narationIndex];
                narationSource.Play(0);
                narationIndex++;
            }
        }
    }


}
