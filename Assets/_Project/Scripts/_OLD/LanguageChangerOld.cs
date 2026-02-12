using System;
using _Project.Scripts.CommonServices.Localization;
using _Project.Scripts.Configs.Localization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageChangerOld : MonoBehaviour
{
    [SerializeField] private Sprite _russianIcon;
    [SerializeField] private Sprite _englishIcon;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Image _languageImage;

    private ILanguageService _languageService;
    

    public GameObject Pn_Exit;
    int reklamacount;
    
    private void Awake()
    {
        if (_russianIcon == null)
            throw new NullReferenceException($"{nameof(_russianIcon)} is not set");

        if (_englishIcon == null)
            throw new NullReferenceException($"{nameof(_englishIcon)} is not set");

        if (_titleText == null)
            throw new NullReferenceException($"{nameof(_titleText)} is not set");

        if (_languageImage == null)
            throw new NullReferenceException($"{nameof(_languageImage)} is not set");
    }

    void Start()
    {
        reklamacount = PlayerPrefs.GetInt("RekCount", 1);

        if (reklamacount > 1)
        {
            
            //AdHandler.instance.ShowInterstitialAd();
            //AdHandler.instance.ShowBanner(true);
            //GameAnalytics.gameAnalytics.InterstitialAd();
            //print("показываем рекламу reklamacount"+reklamacount);
            
        }

        reklamacount++;
        PlayerPrefs.SetInt("RekCount", reklamacount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Pn_Exit.activeSelf == true)
            {
                Pn_Exit.SetActive(false);
            }
            else
            {
                Pn_Exit.SetActive(true);
            }
        }
    }

    public void ChangeLanguage()
    {
        LanguageHandler.language = LanguageHandler.language == Language.English
            ? Language.Russian
            : Language.Russian;
        
        if (LanguageHandler.language == Language.English)
        {
            _titleText.text = "DINOSAUR PUZZLES";
            _languageImage.sprite = _englishIcon;
        }
        else
        {
            _languageImage.sprite = _russianIcon;
            _titleText.text = "ПАЗЛЫ ДИНОЗАВРЫ";
        }
    }

    public void Rate()
    {
        //PlayerPrefs.SetInt ("reklama", 1);
        //if (NewBanner!=null) NewBanner.Hide();
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Mamapapa.Dino");

        //	AppQuit();
    }

    public void onOpenWeb(string site)
    {
        Application.OpenURL(site);
    }

    public void Exit()
    {
        Application.Quit();
        //BigBanner.OnAdLoaded += OnBigBannerLoaded;
        //while (!BigBanner.IsLoaded()) {
        //yield return null;
        //}
    }
}