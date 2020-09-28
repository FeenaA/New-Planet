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

    public GameObject prefabTest;
    public static GameObject sPrefabTest;
    // the main canvas
    public GameObject Canvas;
    public static GameObject sCanvas;
    // MessageBox about the transportation of people
    public GameObject MessageBox;

    // text - amount of coins
    public GameObject textCoins;
    public static GameObject sTextCoins; 

    // text - amount of intervened days
    public GameObject textDays;
    public static GameObject sTextDays;
    public static string sStringTextDays;

    // panels - People and resources
    public GameObject PanelPeople;
    public GameObject PanelResources;
    public static GameObject sPanelPeople;
    public static GameObject sPanelResources;

    // canvas for buildings
    public GameObject canvasBuildings;
    public static GameObject sCanvasBuildings;

    // Sprites, Button - changeing images of ButtonPause
    public Sprite pauseImage;
    public Sprite continueImage;
    public static Sprite sPauseImage;
    public static Sprite sContinueImage;
    public Button buttonPause;
    public static Button sButtonPause;
    // flag - was the game paused before the instance of prefab instanciated?
    //public static bool flagPauseBeforePrefab = false;

    // GOs for random materials
    public GameObject Earth;
    public GameObject Moon;

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
    public GameObject title;
    public static GameObject sTitleCrawlLine;

    //public static bool flagPeopleTransport = false;
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
        public BuildingsOperations.BuildingTime ProbeFactory;//+
        public BuildingsOperations.BuildingHospital Hospital;
        public BuildingsOperations.BuildingMine Mine;//+
        public BuildingsOperations.BuildingTime SCfactory;//+
        public float koefToday;//+
        public int NProbe;//+
        public Dictionary<int, List<settingsResearches.AcceptRes>> Storage;
        public int NSpasecraft;
        public bool flagPeopleTransport = false;//+
        public bool flagPeopleVeBeenSent = false;
        public int NEther; 
    }
    public static GameSettings gameSettings;
    private getItems GI;


    void Start()
    {
        sTextCoins = textCoins;
        sTextDays = textDays;
        sStringTextDays = sTextDays.GetComponent<Text>().text;
        sPauseImage = pauseImage;
        sContinueImage = continueImage;
        sButtonPause = buttonPause;
        sCanvasBuildings = canvasBuildings;
        sPanelPeople = PanelPeople;
        sPrefabTest = prefabTest;
        sCanvas = Canvas;
        sPauseRectangle = PauseRectangle;
        buttons BUT = sCanvas.GetComponent<buttons>();

        if (DateChangeing.pause) { BUT.PauseOn(); }
        CorrectTextOnScene(); 

        GI = gameObject.GetComponent<getItems>();
        int L = materials.Length;

        // general objects which are valid whether it's the new session or not 
        if (flagBeginNewSession == true)
        {
            flagBeginNewSession = false;
            

            // set of materials for planets
            sMaterials = new Material[materials.Length];
            for (int i = 0; i < L; i++)
            { sMaterials[i] = materials[i]; }
        }

        // start of a new session - use all from GameSettings
        if (String.IsNullOrEmpty(gameSettings.NameNative))
        {
            gameSettings.NameNative = GI.NameGenerate();

            // set of all planets with their properties
            //gameSettings.SetPlanets = getItems.GetItems();
            gameSettings.SetPlanets = GI.GetItems();

            // materials for Earth and Moon
            System.Random rnd = new System.Random();
            gameSettings.NEarthMaterial = rnd.Next(0, L - 1);
            gameSettings.NMoonMaterial = rnd.Next(0, L - 1);

            gameSettings.RequestedResources = GI.SetReqs();

            // craw line 
            crawlLine cl = ImageCrawlLine.GetComponent<crawlLine>();
            sTitleCrawlLine = title;
            cl.Show(CrawLineText());

            // save all new params
            LoadGame.SetAll();
        }

        Earth.GetComponent<Renderer>().material = settings.sMaterials[gameSettings.NEarthMaterial];
        Moon.GetComponent<Renderer>().material = settings.sMaterials[gameSettings.NMoonMaterial];

        sPanelResources = PanelResources;
        Transform TextReqs = sPanelResources.transform.Find("TextRequestedResources");

        ShowProgress SP = TextReqs.GetComponent<ShowProgress>();
        TextReqs.GetComponent<Text>().text = SP.Show(gameSettings.RequestedResources);

        
        if ( flagShowMessageTransport )
        {
            // show MessageBox: people may be transported
            var instance = Instantiate(MessageBox);
            instance.transform.SetParent(sCanvas.transform, false);

            flagShowMessageTransport = false;
        }
    }

    private string CrawLineText()
    {
        string StrCrawLine = "";

        // correct text on the ButtonLanguage
        if (PersonalSettings.language == LanguageSettings.Language.English)
        { StrCrawLine = "Welcome to the planet " + gameSettings.NameNative + "!"; }
        else
        {
            if (PersonalSettings.language == LanguageSettings.Language.Russian)
            { StrCrawLine = "НАРОД ПЛАНЕТЫ " + gameSettings.NameNative + " ПРИВЕТСТВУЕТ ТЕБЯ!"; }
        }
        return StrCrawLine;
    }

    private void CorrectTextOnScene()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            ButtonResearch.GetComponentInChildren<Text>().text = "Research";
            PeopleTextTitle.transform.GetComponent<Text>().text = "People";
            ReqResTitle.transform.GetComponent<Text>().text = "Requested resources";
        }
        else
        {
            if (PersonalSettings.language == LanguageSettings.Language.Russian)
            {
                ButtonResearch.GetComponentInChildren<Text>().text = "ПОИСК";
                PeopleTextTitle.transform.GetComponent<Text>().text = "ЛЮДИ";
                ReqResTitle.transform.GetComponent<Text>().text = "ТРЕБУЕМЫЕ РЕСУРСЫ";
            }
        }
    }
}