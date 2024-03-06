using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prefs : MonoBehaviour
{
    public GameObject settingsPanel;

    public Text prviNameText;
    public Text prviScoreText;
    public Text drugiNameText;
    public Text drugiScoreText;
    public Text treciNameText;
    public Text treciScoreText;

    public Toggle naracija;
    public Toggle glazba;
    public Toggle zvuk;
    public Toggle sazetiPrikaz;
    public ToggleGroup tezina;
    public Toggle lagano;
    public Toggle srednjeTesko;
    public Toggle tesko;

    public float brzina_lagano = 0.05f;
    public float brzina_srednje = 0.1f;
    public float brzina_tesko = 0.2f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioSource musicSource;
    public AudioClip music;

    // Start is called before the first frame update
    void Start()
    {
        inicijalizacijaKljuceva();
        ispisiPostavke();
    }

    public void DisplayHighScore()
    {
        string prvi = PlayerPrefs.GetString("prvi");
        string drugi = PlayerPrefs.GetString("drugi");
        string treci = PlayerPrefs.GetString("treci");

        string prviName = prvi.Split('$')[0];
        string prviScore = prvi.Split('$')[1];

        string drugiName = drugi.Split('$')[0];
        string drugiScore = drugi.Split('$')[1];

        string treciName = treci.Split('$')[0];
        string treciScore = treci.Split('$')[1];

        prviNameText.text = prviName;
        prviScoreText.text = prviScore;
        drugiNameText.text = drugiName;
        drugiScoreText.text = drugiScore;
        treciNameText.text = treciName;
        treciScoreText.text = treciScore;
    }

    void inicijalizacijaKljuceva()
    {
        PlayerPrefs.SetInt("trenutniScore", 0);
        PlayerPrefs.SetFloat("trenutniTimer", 0f);
        PlayerPrefs.SetInt("timer_zero", 1);
        if (!PlayerPrefs.HasKey("prvi"))
        {
            PlayerPrefs.SetString("prvi", "-----$--");
        }

        if (!PlayerPrefs.HasKey("drugi"))
        {
            PlayerPrefs.SetString("drugi", "-----$--");
        }

        if (!PlayerPrefs.HasKey("treci"))
        {
            PlayerPrefs.SetString("treci", "-----$--");
        }
        // naracija
        if (!PlayerPrefs.HasKey("naracija"))
        {
            PlayerPrefs.SetInt("naracija", 0);
        }

        // glazba
        if (!PlayerPrefs.HasKey("glazba"))
        {
            PlayerPrefs.SetInt("glazba", 1);
        }

        // zvuk
        if (!PlayerPrefs.HasKey("zvuk"))
        {
            PlayerPrefs.SetInt("zvuk", 1);
        }

        // sazeti prikaz
        if (!PlayerPrefs.HasKey("sazeti"))
        {
            PlayerPrefs.SetInt("sazeti", 0);
        }
        // brzina
        if (!PlayerPrefs.HasKey("brzina"))
        {
            PlayerPrefs.SetFloat("brzina", brzina_srednje);
        }
    }

    public void spremiPostavke()
    {
        // naracija
        if (naracija.isOn)
        {
            PlayerPrefs.SetInt("naracija", 1);
        }
        else
        {
            PlayerPrefs.SetInt("naracija", 0);
        }

        // glazba
        if (glazba.isOn)
        {
            PlayerPrefs.SetInt("glazba", 1);
            musicSource.mute = false;
        }
        else
        {
            PlayerPrefs.SetInt("glazba", 0);
            musicSource.mute = true;
        }

        // zvuk
        if (zvuk.isOn)
        {
            PlayerPrefs.SetInt("zvuk", 1);
            audioSource.mute = false;
        }
        else
        {
            PlayerPrefs.SetInt("zvuk", 0);
            audioSource.mute = true;
        }

        // sazeti prikaz
        if (sazetiPrikaz.isOn)
        {
            PlayerPrefs.SetInt("sazeti", 1);
        }
        else
        {
            PlayerPrefs.SetInt("sazeti", 0);
        }
        // brzina
        string activeToggleName = getActiveToggleName(tezina);
        if (activeToggleName == lagano.name)
        {
            PlayerPrefs.SetFloat("brzina", brzina_lagano);
        }
        else if (activeToggleName == srednjeTesko.name)
        {
            PlayerPrefs.SetFloat("brzina", brzina_srednje);
        }
        else if (activeToggleName == tesko.name)
        {
            PlayerPrefs.SetFloat("brzina", brzina_tesko);
        }
        else
        {
            Debug.Log("Nijedan toggle nije selectiran");
        }
        settingsPanel.SetActive(false);
    }

    public void ucitajPostavke()
    {
        ispisiPostavke();
        // naracija
        if(PlayerPrefs.GetInt("naracija") == 1)
        {
            naracija.isOn = true;
        }
        else
        {
            naracija.isOn = false;
        }

        // glazba
        if (PlayerPrefs.GetInt("glazba") == 1)
        {
            glazba.isOn = true;
        }
        else
        {
            glazba.isOn = false;
        }

        // zvuk
        if (PlayerPrefs.GetInt("zvuk") == 1)
        {
            zvuk.isOn = true;
        }
        else
        {
            zvuk.isOn = false;
        }


        // sazeti prikaz
        if (PlayerPrefs.GetInt("sazeti") == 1)
        {
            sazetiPrikaz.isOn = true;
        }
        else
        {
            sazetiPrikaz.isOn = false;
        }
        // brzina
        float brzina = PlayerPrefs.GetFloat("brzina");
        if(brzina == brzina_lagano)
        {
            lagano.isOn = true;
        }else if(brzina == brzina_srednje)
        {
            srednjeTesko.isOn = true;
        }else if(brzina == brzina_tesko)
        {
            tesko.isOn = true;
        }
    }

    private string getActiveToggleName(ToggleGroup myToggleGroup)
    {
        // postoje i drugi nacini za ovo
        // myToggleGroup.ActiveToggles.FirstOrDeafault();
        // first or default su maknuli -> iterirati kroz activetoggles i vratiti prvi za koji vrijedi toggle.isOn == true
        IEnumerator<Toggle> toggleEnum = myToggleGroup.ActiveToggles().GetEnumerator();
        toggleEnum.MoveNext();
        Toggle toggle = toggleEnum.Current;
        return toggle.name;
    }

    public void ispisiPostavke()
    {
        Debug.Log("PRVI " + PlayerPrefs.GetString("prvi"));
        Debug.Log("DRUGI " + PlayerPrefs.GetString("drugi"));
        Debug.Log("TRECI " + PlayerPrefs.GetString("treci"));
        Debug.Log("TRENUTNI SCORE " + PlayerPrefs.GetInt("trenutniScore"));
        Debug.Log("TRENUTNI TIMER " + PlayerPrefs.GetFloat("trenutniTimer"));
        Debug.Log("NARACIJA " + PlayerPrefs.GetInt("naracija"));
        Debug.Log("GLAZBA " + PlayerPrefs.GetInt("glazba"));
        Debug.Log("ZVUK " + PlayerPrefs.GetInt("zvuk"));
        Debug.Log("SAZETI PRIKAZ " + PlayerPrefs.GetInt("sazeti"));
        Debug.Log("BRZINA " + PlayerPrefs.GetFloat("brzina"));
    }

}
