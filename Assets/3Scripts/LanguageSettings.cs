using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSettings : MonoBehaviour
{
    public GameObject prefabLanguages;
    public Button buttonEnglish;
    public Button buttonRussian;

    public Sprite ImageEmpty; 
    public Sprite ImageChosen;

    public enum Language { English, Russian };
     
    void Start()
    {
        if (PersonalSettings.language == Language.English) { settings.nLanguage = 1; ShowButtonsEn(); }
        else { if (PersonalSettings.language == Language.Russian) { settings.nLanguage = 0; ShowButtonsRU(); } }
    }

    // 0 - Russian, 1 - English
    public void EnglishPressed() 
    {
        if (PersonalSettings.language == Language.English) { return; }

        PersonalSettings.language = Language.English;
        ShowButtonsEn();
        settings.nLanguage = 1;
    }
    public void RussianPressed() 
    {
        if (PersonalSettings.language == Language.Russian) { return; }

        PersonalSettings.language = Language.Russian;
        ShowButtonsRU();
        settings.nLanguage = 0; 
    }

    // swap Images of buttons
    private void ShowButtonsEn()
    {
        buttonEnglish.GetComponent<Image>().sprite = ImageChosen;
        buttonRussian.GetComponent<Image>().sprite = ImageEmpty;
    }
    private void ShowButtonsRU()
    {
        buttonRussian.GetComponent<Image>().sprite = ImageChosen;
        buttonEnglish.GetComponent<Image>().sprite = ImageEmpty;
    }

    public void OKPressed()
    {
        // read all files accordingly to the selected language
        Preprocessing.sGObjectPreproc.GetComponent<Preprocessing>().GetXML();
        // destroy prefab
        Destroy(prefabLanguages);
    }
}
