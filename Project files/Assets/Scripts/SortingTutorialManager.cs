using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortingTutorialManager : MonoBehaviour
{
    public GameObject[] tutorialText;
    public GameObject[] kontejneriButtons;
    public GameObject bunny1;
    public GameObject bunny2;

    private int index = 0;
    private bool canSetupButtons = false;

    public AudioSource audioSource;
    public AudioClip clickclip;

    void Update()
    {
        for (int i = 0; i < tutorialText.Length; i++)
        {
            if (i == index)
            {
                tutorialText[i].SetActive(true);
                if (tutorialText[i].name == "clean")
                {
                    bunny2.SetActive(true);
                    bunny1.SetActive(false);
                    disableOtherText(tutorialText[i].name);
                }
                if (tutorialText[i].name == "fin")
                {
                    bunny2.SetActive(false);
                    canSetupButtons = true;
                    disableOtherText(tutorialText[i].name);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            
            index++;
            if (canSetupButtons)
            {
                foreach (GameObject g in kontejneriButtons)
                {
                    g.GetComponent<Button>().interactable = true;
                }
                for (int i = 0; i < tutorialText.Length; i++)
                {
                    tutorialText[i].SetActive(false);
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("zvuk") == 1)
                {
                    audioSource.clip = clickclip;
                    audioSource.Play(0);
                }
            }
        }
    }

    void disableOtherText(string arg)
    {
        for (int i = 0; i < tutorialText.Length; i++)
        {
            if (tutorialText[i].name != arg)
            {
                tutorialText[i].SetActive(false);
            }
        }
    }
}
