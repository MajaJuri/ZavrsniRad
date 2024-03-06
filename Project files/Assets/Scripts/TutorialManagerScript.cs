using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManagerScript : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex;
    public GameObject[] rubbish;
    private bool rubbishVisible = false;
    public GameObject questionBox;
    public GameObject player;
    public GameObject LetsGoBox;
    public GameObject skipButton;
    public float timeBetweenInstructions = 1.5f;
    private int items = 0;
    [SerializeField]
    private AudioSource audioSource;
    public AudioClip audioClip;

    public AudioSource musicSource;

    public AudioSource narationSource;
    public AudioClip[] narationClips;

    private bool canGetNextInstruction = true;

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

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == popupIndex)
            {
                popups[i].SetActive(true);
            }
            else
            {
                popups[i].SetActive(false);
            }
        }

        if (popupIndex == 0)
        {
            if (canGetNextInstruction && (Input.GetButtonDown("Horizontal") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            {
                StartCoroutine(Wait(timeBetweenInstructions));
            }
        }
        else if (popupIndex == 1)
        {
            if (canGetNextInstruction && (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
            {
                StartCoroutine(Wait(timeBetweenInstructions));
            }
        }
        else if (popupIndex == 2)
        {
            if (!rubbishVisible)
            {
                foreach (GameObject g in rubbish)
                {
                    g.SetActive(true);
                }
                rubbishVisible = true;
            }
            Debug.Log("items = " + items);
            if (canGetNextInstruction && items >= 2)
            {
                StartCoroutine(Wait(timeBetweenInstructions));
            }
        }
        else if (popupIndex == 3)
        {
            questionBox.SetActive(true);
            if (LetsGoBox.active)
            {
                skipButton.SetActive(false);
                popupIndex++;
                naracija();
            }
        }
    }

    IEnumerator Wait(float seconds)
    {
        canGetNextInstruction = false;
        yield return new WaitForSeconds(timeBetweenInstructions);
        popupIndex++;
        canGetNextInstruction = true;
        naracija();
    }

    public int GetNoOfItems()
    {
        return this.items;
    }

    public void Collect()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        items += 1;
    }

    private void naracija()
    {
        if (PlayerPrefs.GetInt("naracija") > 0)
        {
            if (popupIndex < narationClips.Length)
            {
                narationSource.clip = narationClips[popupIndex];
                narationSource.Play(0);
            }
        }
    }
}
