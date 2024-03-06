using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    public GameObject quiz;
    public GameObject pauseButton;
    public GameObject homeButton;
    [SerializeField]
    private AudioSource audioSource;
    public AudioClip audioClip;

    private int passedFinishLine;

    // Start is called before the first frame update
    void Start()
    {
        passedFinishLine = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetType().ToString() == "UnityEngine.BoxCollider2D"){
            passedFinishLine += 1; // ako je neparan, onda je s desne strane cilja, kada se ponovno ukljuci trigger pri povratku kroz cilj ne zelimo da svira zvuk ni da se okine kviz
            if (col.gameObject.tag == "Player" && passedFinishLine % 2 == 1)
            {
                FindObjectOfType<GameManagerScript>().Pause();
                quiz.SetActive(true);
                pauseButton.SetActive(false);
                homeButton.SetActive(true);
                if (passedFinishLine == 1) // zvuk svira samo prvi put
                {
                    // ne zelim da svaki put bude zvuk
                    audioSource.clip = audioClip;
                    audioSource.Play();

                }
            }
        }
    }
}
