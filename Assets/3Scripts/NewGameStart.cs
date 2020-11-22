using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameStart : MonoBehaviour
{
    // the field containing NameNative
    public InputField inputField;
    // 
    public GameObject PreprocGO;
    private PersonalSettings PS;
    private LoadGame LG;
    private getItems GI;

    public GameObject ButtonLevel1;
    public GameObject ButtonLevel2;
    public GameObject ButtonLevel3;

    public Sprite ImageEmpty;
    public Sprite ImageChosen;

    public Text TextPerSentPeople;

    public static readonly int[] PerSentAlivePeople = { 15, 30, 50 };
    private string ToWin; 
    private string PerSent;

    public static int sLevel = 0;

    void OnEnable()
    {
        CorrectLanguage();
        ChooseLevelPressed(sLevel);
    }

    public void ChooseLevelPressed(int Level)
    {
        sLevel = Level;
        // change pictures
        switch (sLevel)
        {
            case 1:
                ButtonLevel1.GetComponent<Image>().sprite = ImageEmpty;
                ButtonLevel2.GetComponent<Image>().sprite = ImageChosen;
                ButtonLevel3.GetComponent<Image>().sprite = ImageEmpty;
                break;

            case 2:
                ButtonLevel1.GetComponent<Image>().sprite = ImageEmpty;
                ButtonLevel2.GetComponent<Image>().sprite = ImageEmpty;
                ButtonLevel3.GetComponent<Image>().sprite = ImageChosen;
                break;

            default:
                ButtonLevel1.GetComponent<Image>().sprite = ImageChosen;
                ButtonLevel2.GetComponent<Image>().sprite = ImageEmpty;
                ButtonLevel3.GetComponent<Image>().sprite = ImageEmpty;
                break;
        }

        // change text 
        TextPerSentPeople.text = ToWin + System.Convert.ToString(PerSentAlivePeople[sLevel]) + PerSent;
    }

    /// <summary>
    /// Close the MessageBox and start new sessoin
    /// </summary>
    public void StartNewPressed()
    {
        PS = PreprocGO.GetComponent<PersonalSettings>();
        LG = PreprocGO.GetComponent<LoadGame>();
        GI = PreprocGO.GetComponent<getItems>();

        // gameSettings and Continue.xml (for future) have their initial values 
        LG.StartNew();
        // flag - the application is working on the session
        Preprocessing.FlagStartGame = false;
        // flag - to generate new materials in settings
        Preprocessing.FlagStartSession = true;

        // flag - the saved game exists
        PS.SetFlagSavedGame(true);

        // set of all planets with their properties
        settings.gameSettings.SetPlanets = GI.GetItems();
        // set of requested resources
        settings.gameSettings.RequestedResources = GI.SetReqs();

        gameObject.SetActive(false);

        // Reset NameNative
        string str = inputField.text;
        str = RuDetectAndUpper(str); // correct register, case RU
        settings.gameSettings.NameNative = str;

        // go to the scene "Game"
        SceneManager.LoadScene("Game");
    }

    public void CancelPressed()
    { 
        gameObject.SetActive(false);
    }

    /// <summary>
    /// if line includes russian symbols they'll be changed toUpper
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private string RuDetectAndUpper(string line)
    {
        string[] str = new[] { line }; 

        for (int i = 0; i < str.Length; i++)
        {
            char c = line[i];
            if ((c >= 'а') && (c <= 'я'))
            { str[i] = str[i].ToUpper(new System.Globalization.CultureInfo("ru-RU", false)); }
        }
        return System.String.Join(" ", str); ;
    }

    private void CorrectLanguage()
    {
        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            ToWin = "ДЛЯ ПОБЕДЫ НЕОБХОДИМО СПАСТИ МИНИМУМ ";
            PerSent = "% НАСЕЛЕНИЯ";
        }
        else
        {
            ToWin = "You have to rescue at list ";
            PerSent = "% of people to win";
        }
    }
}
