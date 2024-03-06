using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public int questionCount = 5;

    public GameObject qnAPanel;
    public Text questionText;
    public Button[] answerButtons;
    public GameObject nextQuestionButton;
    public GameObject exitInstructions;
    public GameObject exitQuizButton;
    public GameObject levelADoneObject;
    public Sprite normalButtonImage;
    public Sprite correctButtonImage;
    public Sprite incorrectButtonImage;

    private List<string> redoviDatoteke;
    private string pitanje;
    private List<string> odgovori;
    private string tocanOdgovor;

    [SerializeField]
    private AudioSource audioSource;
    public AudioClip audioClipCorrect;
    public AudioClip audioClipWrong;
    public AudioClip audioClipEnd;

    private int quiz_correct = 0;
    private Button currectCorrectButton;

    private bool backToQuizInstructionWasVisible = false;
    public GameObject backToQuizInstructionObject;

    // Start is called before the first frame update
    void Start()
    {
        // dohvati datoteku
        string readFromFilePath = Application.streamingAssetsPath + "/" + "kviz" + ".txt";
        // procitaj red po red i dodaj u redoviDatoteke
        redoviDatoteke = File.ReadAllLines(readFromFilePath).ToList();
        // postavi prvo pitanje
        SetupNextQuestion();
        // nextQuestionButton omogucava dodavanje pitanja dok questioncount > 0
        nextQuestionButton.GetComponent<Button>().onClick.AddListener(SetupNextQuestion);
    }

    public void SetupNextQuestion()
    {
        qnAPanel.SetActive(true);
        exitInstructions.SetActive(false);
        exitQuizButton.SetActive(false);
        nextQuestionButton.SetActive(false);
        foreach (Button b in answerButtons)
        {
            b.GetComponent<Image>().sprite = normalButtonImage;
        }

        odgovori = new List<string>();
        // dohvati random red
        int redIndex = Random.Range(0, redoviDatoteke.Count - 1);
        string red = redoviDatoteke.ElementAt(redIndex);
        // razdvoji i pohrani u pitanje, odgovori, tocanOdgovor
        pitanje = red.Split(':')[0];
        //Debug.Log("pitanje " + pitanje);
        tocanOdgovor = red.Split(':')[1];
        //Debug.Log("tocan odgovor = " + tocanOdgovor);
        for (int i = 1; i < red.Split(':').Length; i++)
        {
            string odgovor = red.Split(':')[i];
            odgovori.Add(odgovor);
        }
        // makni taj red iz redoviDatoteke tako da se ne pojavi ponovo isto pitanje
        redoviDatoteke.RemoveAt(redIndex);
        // postavi tekst pitanja na ui
        questionText.text = pitanje;
        // postavi tekst odgovora na buttone
        foreach (Button b in answerButtons)
        {
            string odgovor = odgovori.ElementAt(Random.Range(0, odgovori.Count - 1));
            odgovori.Remove(odgovor);
            b.GetComponentInChildren<Text>().text = odgovor;
            // postavi listenere na gumbe - tocno/netocno
            if (odgovor == tocanOdgovor)
            {
                // postaviti listenera za tocan odgovor
                currectCorrectButton = b;
                b.GetComponent<Button>().onClick.RemoveListener(AnswerIncorrect);
                b.GetComponent<Button>().onClick.AddListener(AnswerCorrect);
                b.GetComponent<Button>().onClick.AddListener(() => { b.GetComponent<Image>().sprite = correctButtonImage; });
            }
            else
            {
                // postaviti listenera za netocan odgovor
                b.GetComponent<Button>().onClick.RemoveListener(AnswerCorrect);
                b.GetComponent<Button>().onClick.AddListener(AnswerIncorrect);
                b.GetComponent<Button>().onClick.AddListener(() => { b.GetComponent<Image>().sprite = incorrectButtonImage; });
            }

        }
    }

    public void AnswerCorrect()
    {
        // zvuk
        audioSource.clip = audioClipCorrect;
        audioSource.Play();
        // smanjiti questioncount
        questionCount--;
        // ako je questioncount dosao na 0, preci na sljedeci dio
        // ako nije, pojavi se gumb za ici na sljedece pitanje + poziv SetupNextQuestion();
        if (questionCount <= 0)
        {
            nextQuestionButton.GetComponent<Button>().onClick.RemoveListener(SetupNextQuestion);
            nextQuestionButton.GetComponent<Button>().onClick.AddListener(LevelADone);
        }
        nextQuestionButton.SetActive(true);
        quiz_correct += 3;
        currectCorrectButton.GetComponent<Button>().onClick.RemoveListener(AnswerCorrect);
    }

    public void AnswerIncorrect()
    {
        //Debug.Log("netocan odgovor");
        // zvuk
        audioSource.clip = audioClipWrong;
        audioSource.Play();
        // mogucnost izlaska iz kviza
        exitQuizButton.SetActive(true);
        exitInstructions.SetActive(true);
        quiz_correct -= 1;
    }

    private void LevelADone()
    {
        levelADoneObject.SetActive(true);
        qnAPanel.SetActive(false);
        audioSource.clip = audioClipEnd;
        audioSource.Play();
        PlayerPrefs.SetInt("trenutniScore", izracunajScore());
    }


    private int izracunajScore()
    {
        int trenutniScore = PlayerPrefs.GetInt("trenutniScore");
        float trenutniTimer = PlayerPrefs.GetFloat("trenutniTimer");

        int rezultat = (int) (trenutniScore * 5 / trenutniTimer*100);
        if (quiz_correct < 0) quiz_correct = 0;
        rezultat += quiz_correct * 10;

        return rezultat;
    }

    public void backToQuizInstruction()
    {
        Debug.Log(backToQuizInstructionWasVisible);
        if(!backToQuizInstructionWasVisible)
        {
            backToQuizInstructionObject.SetActive(true);
            backToQuizInstructionWasVisible = true;
        }
        else
        {
            backToQuizInstructionObject.SetActive(false);
        }
    }

}
