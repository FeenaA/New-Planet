using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preprocessing : MonoBehaviour 
{
    // public GameObjects 
    public GameObject CanvasMain;
    public GameObject PrefabLanguageBox;
    public GameObject PrefabCanvasHelp;
    public GameObject ButtonLanguage; 
    public GameObject ButtonNewGame;
    public GameObject ButtonContinue;
    public GameObject CanvasGameOver; // prefab
    public GameObject PrefabStatistics;

    public static GameObject sGObjectPreproc;
    public static GameObject sCanvasMain;

    private static bool FlagStartGame = true;

    public static string[] strHelp; 
    public static string[] strHelpTitle;

    private XMLOperations XMLOp;
    private PersonalSettings PS;
    private LoadGame LG;

    void Start()
    {
        sGObjectPreproc = gameObject;
        sCanvasMain = CanvasMain;

        XMLOp = gameObject.GetComponent<XMLOperations>();
        PS = gameObject.GetComponent<PersonalSettings>();
        LG = gameObject.GetComponent<LoadGame>();

        CorrectTextOnScene();

        // The applcation has been started
        if (FlagStartGame)
        {
            // create or read file with personal settings
            PS.GetPersonalSettings();
            CorrectTextOnScene();

            #region // Music
            if (PersonalSettings.flagMusic)
            {
                // swich on music 
                ButtonMusic.GetComponent<Image>().sprite = SpriteSwinchOn;
                AudioListener.pause = false;
            }
            else
            {
                // swich off music
                ButtonMusic.GetComponent<Image>().sprite = SpriteSwinchOff;
                AudioListener.pause = true;
            }
            #endregion

            // personal settings - if a user plays the first time 
            if (PersonalSettings.flagFirstGame)
            {
                // ask user about language
                LanguagePressed();
            }
            else 
            { 
                // read all files
                GetXML(); 
            } 
        }
        // current mission is finished or interrupted
        else
        {
            if (settings.flagIsFinished)
            {
                // show Statistics
                var instance = Instantiate(PrefabStatistics);
                instance.SendMessage("TheStart", true);

                // set all flags
                settings.flagIsFinished = false;
                PS.SetStatistics(false);
            }
        }

        // If the saved game doesn't exist, ButtonContinue is uninteractable
        Button buttonContinue = ButtonContinue.GetComponent<Button>();
        if (PersonalSettings.flagSavedGame == false)
        { buttonContinue.interactable = false; }
        else
        { buttonContinue.interactable = true; }
    }

    public void GetXML()
    {
        // save language to file
        PS.SetLanguage();
        CorrectTextOnScene();
        // read ALL files
        XMLOp.GetAll();
    }

    private void CorrectTextOnScene()
    {
        // correct text on the ButtonLanguage
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            ButtonLanguage.GetComponentInChildren<Text>().text = "EN";
            ButtonNewGame.GetComponentInChildren<Text>().text = "New game";
            ButtonContinue.GetComponentInChildren<Text>().text = "Continue";
        }
        else
        {
            if (PersonalSettings.language == LanguageSettings.Language.Russian)
            {
                ButtonLanguage.GetComponentInChildren<Text>().text = "RU";
                ButtonNewGame.GetComponentInChildren<Text>().text = "НОВАЯ ИГРА";
                ButtonContinue.GetComponentInChildren<Text>().text = "ПРОДОЛЖИТЬ";
            }
        }
    }

    public void LanguagePressed()
    {
        // ask user about language
        var instance = Instantiate(PrefabLanguageBox);
        instance.transform.SetParent(CanvasMain.transform, false);
    }

    public void HelpPressed()
    {
        CanvasMain.SetActive(false);
        Instantiate(PrefabCanvasHelp.gameObject);
    }

    public void StatisticsPressed()
    {
        // show Statistics
        var instance = Instantiate(PrefabStatistics);
        instance.SendMessage("TheStart", false);
    }

    public Sprite SpriteSwinchOn; 
    public Sprite SpriteSwinchOff;
    public GameObject ButtonMusic;
    public void MusicPressed()
    {
        PersonalSettings.flagMusic = !PersonalSettings.flagMusic;

        if (PersonalSettings.flagMusic)
        {
            // swich on music 
            ButtonMusic.GetComponent<Image>().sprite = SpriteSwinchOn;
            AudioListener.pause = false;
        }
        else
        {
            // swich off music
            ButtonMusic.GetComponent<Image>().sprite = SpriteSwinchOff;
            AudioListener.pause = true;

        }

        // save flagMusic to file
        PS.SetFlagMusic();
    }

    public void NewGamePressed()
    {
        // gameSettings and Continue.xml (for future) have their initial values 
        LG.StartNew();
        // flag - the application is working on the session
        FlagStartGame = false;
        // flag - the saved game exists
        PS.SetFlagSavedGame(true);
        // move to scene "Game"
        SceneManager.LoadScene("Game");
    }

    public void ContinuePressed()
    {
        // download the previous session
        if (FlagStartGame) { LG.Continue(); }
        // flag - the application is working on the session
        FlagStartGame = false;
        // flag - the saved game exists
        PS.SetFlagSavedGame(true);
        // move to scene "Game"
        SceneManager.LoadScene("Game");
    } 

    public void AchivementsPressed()
    {
        // Instantiate
    }
} 
