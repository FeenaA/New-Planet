using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    public Button ButtonResearch;
    public GameObject PeopleTextTitle;
    public GameObject ReqResTitle; 

    public GameObject PauseRectangle;
    public static GameObject sPauseRectangle;

    // the main canvas
    public GameObject Canvas;
    // MessageBox about the transportation of people
    public GameObject MessageBoxTrPeople;

    // general message box
    public GameObject MessageBox;

    // text - amount of intervened days
    public GameObject textDays;
    public static GameObject sTextDays;
    public static string sStringTextDays;

    // requested resources
    public Transform TextReqs;

    // Sprites, Button - changeing images of ButtonPause
    public Sprite pauseImage;
    public Sprite continueImage;
    public static Sprite sPauseImage;
    public static Sprite sContinueImage;
    public Button buttonPause;
    public static Button sButtonPause;

    // GOs for random materials
    public GameObject Earth;
    public GameObject Moon;

    public GameObject PrefabSound;

    // amount of planets
    public static int sNPlanets = 50;

    // materials for planets
    public Material[] materials;
    public static Material[] sMaterials;

    // !!! download
    public static int nLanguage = 0; // 0 - Russian, 1 - English

    // Are people sent cycled or not? 
    public static bool flagCycledSent = false;

    // have we just started the application?
    private static bool flagBeginNewSession = true;

    // crawl line
    public RectTransform ImageCrawlLine;

    /// <summary>
    /// finished(true), in promotionary(false)
    /// </summary>
    public static bool flagIsFinished = false;
    /// <summary>
    /// winning(true), failure(false)
    /// </summary>
    public static bool flagIsWin = false;
    /// <summary>
    /// welcomeBack was(true) / wasn't(false) shown
    /// </summary>
    private static bool flagWelcomeBack = false;
    /// <summary>
    /// suggestion to build buildings
    /// </summary>
    public static bool flagBuild = false;

    // flag to show MessageBox with Congratulations
    public static bool flagShowMessageTransport = false;

    public class GameSettings 
    {
        public string NameNative;//+ 
        public int NEarthMaterial; //+
        public int NMoonMaterial;//+
        public Dictionary<int, int> RequestedResources;//+
        public Dictionary<int, getItems.PlanetProperty> SetPlanets;//+
        public int NDays;//+
        public int NCoins;//+
        public int NPeopleOnNative;//+
        public int NPeopleDied;//+
        public int stepCoins;//+
        public bool flagSelectedPlanet;//+
        public string NameNew;//+
        public int NPeopleOnNew;//+
        public int AllPeople;
        public BuildingsOperations.BuildingTime ProbeFactory;//+
        public BuildingsOperations.BuildingHospital Hospital;
        public BuildingsOperations.BuildingMine Mine;//+
        public BuildingsOperations.BuildingTime SCfactory;//+
        public float koefToday;//+
        public int NProbe;//+
        public Dictionary<int, List<settingsResearches.AcceptRes>> Storage;
        public int NSpasecraft;
        public bool flagPeopleTransport = false; // flag - people may be transported
        public bool flagPeopleVeBeenSent = false; // flag - at leat one group of people was sent
        public int NEther;
        public int CurrentPerSent; 
        public int CurrentNResUnits;
    } 
    public static GameSettings gameSettings;
    private getItems GI;

    private string StrWelcome;
    private string StrAuto;
    private string StrWelcomeBack;
    private string StrBuildings;

    void Start()
    {
        // general objects which are valid whether it's the new session or not 
        sTextDays = textDays;
        sStringTextDays = sTextDays.GetComponent<Text>().text;
        sPauseImage = pauseImage;
        sContinueImage = continueImage;
        sButtonPause = buttonPause;
        sPauseRectangle = PauseRectangle;

        buttons BUT = Canvas.GetComponent<buttons>();

        GI = gameObject.GetComponent<getItems>();
        int L = materials.Length;
        
        #region Switch on/off music
        if (PersonalSettings.flagMusic)
        { GameObject.FindGameObjectWithTag("Music").GetComponent<SoundClass>().PlayMusic(); }
        else
        { GameObject.FindGameObjectWithTag("Music").GetComponent<SoundClass>().StopMusic(); }
        #endregion

        #region materials for the set of planets
        if (flagBeginNewSession == true)
        {
            flagBeginNewSession = false;

            // set of materials for planets
            sMaterials = new Material[materials.Length];
            for (int i = 0; i < L; i++)
            { sMaterials[i] = materials[i]; }
        }
        #endregion

        // rewrite all strings
        CorrectTextOnScene();
        // to operate with CrawlLine
        crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();

        // start of a new session
        if (String.IsNullOrEmpty(gameSettings.NameNative))
        {
            DateChangeing.pause = false;
            BUT.PauseOff(); 

            gameSettings.NameNative = GI.NameGenerate();

            // set of all planets with their properties
            gameSettings.SetPlanets = GI.GetItems();

            // primary amount of people
            gameSettings.NPeopleOnNative = gameSettings.AllPeople;

            #region set materials for Earth and Moon 
            System.Random rnd = new System.Random();
            gameSettings.NEarthMaterial = rnd.Next(0, L - 1);
            gameSettings.NMoonMaterial = rnd.Next(0, L - 1);
            #endregion

            gameSettings.RequestedResources = GI.SetReqs();

            // rewrite all strings
            CorrectTextOnScene();

            #region MessageBox: "Build buildings!"
            if (!flagBuild)
            {
                flagBuild = true;
                var messageBox = Instantiate(MessageBox);
                messageBox.SendMessage("TheStart", StrBuildings);
                // SetParent to the MessageBox
                messageBox.transform.SetParent(Canvas.transform, false);
            }
            #endregion

            // show welcome message (crawl line)
            CL.ShowWithoutPause(StrWelcome);

            // to prevent the message "Welcome back!" during the session
            flagWelcomeBack = true;

            // save all new params
            LoadGame.SetAll();
        }
        else
        {
            #region MessageBox: "Welcome back!"
            if (!flagWelcomeBack)
            {
                flagWelcomeBack = true;
                var messageBox = Instantiate(MessageBox);
                messageBox.SendMessage("TheStart", StrWelcomeBack);
                // SetParent to the MessageBox
                messageBox.transform.SetParent(Canvas.transform, false);
            }
            #endregion

            crawlLine.RestartTimer();
            if (DateChangeing.pause) { BUT.PauseOn(); }
        }

        EarthOnClick.flagBuildings = false;
        Earth.GetComponent<Renderer>().material = settings.sMaterials[gameSettings.NEarthMaterial];
        Moon.GetComponent<Renderer>().material = settings.sMaterials[gameSettings.NMoonMaterial];

        ShowProgress SP = TextReqs.GetComponent<ShowProgress>();
        TextReqs.GetComponent<Text>().text = SP.Show(gameSettings.RequestedResources);

        #region show MessageBox: people may be transported
        if ( flagShowMessageTransport )
        {
            var instance = Instantiate(MessageBoxTrPeople);
            instance.transform.SetParent(Canvas.transform, false);
            // craw line 
            CL.ShowNext(StrAuto);

            flagShowMessageTransport = false;
        }
        #endregion
    }

    private void CorrectTextOnScene()
    {
        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            ButtonResearch.GetComponentInChildren<Text>().text = "ПОИСК";
            PeopleTextTitle.transform.GetComponent<Text>().text = "ЛЮДИ";
            ReqResTitle.transform.GetComponent<Text>().text = "ТРЕБУЕМЫЕ РЕСУРСЫ";
            StrWelcome = "НАРОД ПЛАНЕТЫ " + gameSettings.NameNative + " ПРИВЕТСТВУЕТ ТЕБЯ!";
            StrAuto = "ТЫ МОЖЕШЬ ПОСЫЛАТЬ КОСМОЛЕТЫ С ЖИТЕЛЯМИ АВТОМАТИЧЕСКИ.";
            StrWelcomeBack = "С ВОЗВРАЩЕНИЕМ!";
            StrBuildings = "ПЛАНЕТА " + gameSettings.NameNative + " ЗАРАЖЕНА ВИРУСОМ! СТРОЙ ЗДАНИЯ, ЧТОБЫ ИСКАТЬ НОВУЮ ПЛАНЕТУ. ДЛЯ ЭТОГО НАЖМИ НА " + gameSettings.NameNative;
        }
        else
        {
            ButtonResearch.GetComponentInChildren<Text>().text = "Research";
            PeopleTextTitle.transform.GetComponent<Text>().text = "People";
            ReqResTitle.transform.GetComponent<Text>().text = "Requested resources";
            StrWelcome = "Welcome to the planet " + gameSettings.NameNative + "!";
            StrAuto = "You can send spacecraft with people automatically.";
            StrWelcomeBack = "Welcome back!";
            StrBuildings = "The planet " + gameSettings.NameNative + " is infected with a virus. Build buildings to find a new planet. Tab to " + gameSettings.NameNative;
        }
    }
}