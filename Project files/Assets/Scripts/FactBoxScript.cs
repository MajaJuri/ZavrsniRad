using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactBoxScript : MonoBehaviour
{
    public GameObject FactBox;
    public Text FactBoxTitle;
    public Text FactBoxText;
    public Button okButton;
    public string title;
    public string[] facts;
    public string[] sazetiFacts;
    public float xPos;
    public float yPos;
    public float zPos;
    public float waiting = 0.08F;
    [SerializeField]
    private AudioSource audioSource;
    public AudioClip audioClip;
    public float timeBetweenChars = 0.02f;
    public float timeBetweenLines = 1f;


    // Use this for initialization
    void Start()
    {
        xPos = transform.position.x;
        yPos = transform.position.y;
        zPos = transform.position.z;
    }

    void Update()
    {
        if (FactBox.active)
        {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                okButton.onClick.Invoke();
            }
        }
    }

    IEnumerator OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.GetType().ToString() == "UnityEngine.BoxCollider2D" || col.collider.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            if (col.gameObject.tag == "Player")
            {
                FindObjectOfType<GameManagerScript>().Pause();
                this.transform.position = new Vector3(xPos, yPos + 0.2F, zPos);
                yield return new WaitForSeconds(waiting);
                this.transform.position = new Vector3(xPos, yPos, zPos);
                audioSource.clip = audioClip;
                audioSource.Play();
                yield return new WaitForSeconds(0.25F);
                FactBoxText.text = "";
                FactBox.SetActive(true);
                FactBoxTitle.text = title;
                if(PlayerPrefs.GetInt("sazeti") == 0)
                {
                    for (int i = 0; i < facts.Length; i++)
                    {
                        FactBoxText.text += facts[i];
                        if (i < facts.Length - 1)
                        {
                            FactBoxText.text += "\n\n";
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < sazetiFacts.Length; i++)
                    {
                        FactBoxText.text += sazetiFacts[i];
                        if (i < sazetiFacts.Length - 1)
                        {
                            FactBoxText.text += "\n\n";
                        }
                    }
                }


                /*foreach (string fact in facts)
                {
                    Debug.Log(fact);
                    foreach (char letter in fact.ToCharArray())
                    {
                        FactBoxText.text += letter;
                        yield return new WaitForSeconds(timeBetweenChars);
                    }

                }*/

            }
        }
    }
}
