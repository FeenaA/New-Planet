using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsResearches: MonoBehaviour
{
    // text - amount of intervened days
    public GameObject textDays;
    // to make scrollBar be at the highest position
    public Scrollbar scrollBar;
    // prefab item
    public RectTransform prefabPlanet;
    // parent
    public RectTransform content;
    // chosen planet
    public static GameObject ChosenPlanet = null;

    // to have possibility to operate with the panel from ItemOnClick
    public GameObject PanelInformation; 
    public static GameObject sPanelInformation;

    // nesessary resources
    public Transform ResWater, ResAir, ResSoil;
    public static Transform rWater, rAir, rSoil;

    // extraordinary resources
    public Transform ResAdd1, ResAdd2, ResAdd3;
    public static Transform[] r;

    // amount of probes
    public GameObject textProbes;
    // amount of spacecrafts
    public GameObject textSC;
    public static GameObject sTextSC;
    // amount of Ether
    public GameObject textEth;
    public static GameObject sTextEth;
    // amount of Ether
    public GameObject textBC;
    public static GameObject sTextBC;

    // text for requested resources
    public Text TextRequestedResources;

    public Text TextPlanet;

    public class AcceptRes
    {
        // amount of resource
        public int amount;
        // number of source planet
        public string NamePlanet;
    }

    // crawl line
    public RectTransform ImageCrawlLine;
    public GameObject title; 
    public static GameObject sTitle;
    private static bool flagFirstInResearch = true;

    // all strings
    private string strWelcome = "";


    void Start()
    {
        CorrectLanguage();

        // craw line 
        if (flagFirstInResearch)
        {
            crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
            CL.RestartToShow();

            CL.ShowNext(strWelcome);
            flagFirstInResearch = false;
        }
        else { crawlLine.RestartTimer(); }
        
        // gameobjects to fill PanelInformation
        sPanelInformation = PanelInformation;

        rWater = ResWater;
        rAir = ResAir;
        rSoil = ResSoil;

        if ((PersonalSettings.language == LanguageSettings.Language.Russian))
        { TextPlanet.text = "ПЛАНЕТА"; }
        else { TextPlanet.text = "Planet"; }

        r = new Transform[3] { ResAdd1, ResAdd2, ResAdd3};

        // panel for different items
        sTextSC = textSC;
        sTextEth = textEth;
        sTextBC = textBC;
        textProbes.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NProbe);
        sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NSpasecraft);
        sTextEth.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NEther);
        sTextBC.GetComponent<Text>().text = System.Convert.ToString(BlueCoin.sNBlueCoin);

        // fill information about planets
        OnReceivedModels();

        ShowProgress SP = PanelInformation.GetComponent<ShowProgress>();
        TextRequestedResources.text = SP.Show(settings.gameSettings.RequestedResources);
    }

    private void CorrectLanguage()
    {
        if ((PersonalSettings.language == LanguageSettings.Language.Russian))
        {
            strWelcome = "УЧЕНЫЕ ПОДОБРАЛИ ПЛАНЕТЫ С ПОДХОДЯЩИМИ ТЕМПЕРАТУРОЙ, РАЗМЕРОМ И ГРАВИТАЦИЕЙ";
        }
        else
        {
            strWelcome = "Scientists've compiled a list of planets with suitable temperature, size and gravity";
        }
    }

    /// <summary>
    /// fill information about planets
    /// </summary>
    void OnReceivedModels()
    {
        // by default the Planet0 is chosen
        int nPlanet = 0;
        // add new items with data
        foreach (var planet in settings.gameSettings.SetPlanets)
        {
            var instance = Instantiate(prefabPlanet.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance.transform, planet);

            // name of an object in inspector
            if ( nPlanet == 0 )
            {
                ChosenPlanet = instance;
                ItemOnClick.sButtonName = instance.transform.Find("ButtonName").GetComponent<Button>();

                // item select
                ItemOnClick click = GameObject.Find("SettingsResearches").GetComponent<ItemOnClick>();
                click.ItemSelect(instance);
            }
            nPlanet++;
        }
        // make scrollBar be at the highest position
        scrollBar.value = 1;
    }

    /// <summary>
    /// connection between UI and script 
    /// </summary>
    /// <param name="viewGameObject">Поле таблицы</param>
    /// <param name="planet">Информация о планете</param>
    void InitializeItemView(Transform viewGameObject, KeyValuePair<int, getItems.PlanetProperty> planet)
    {
        TestItemView view = new TestItemView(viewGameObject);

        getItems.PlanetProperty PP = settings.gameSettings.SetPlanets[planet.Key];
        if (PP.flagIsResearched)
        {
            viewGameObject.GetComponent<Image>().color = Color.black;
            view.buttonName.transform.GetComponent<Image>().color = Color.black;
        }
        if (PP.flagIsSelected)
        {
            view.buttonName.GetComponent<Outline>().enabled = true;
            view.buttonName.GetComponent<Outline>().effectColor = buttons.sColorPause;
            viewGameObject.GetComponent<Outline>().enabled = true;
            viewGameObject.GetComponent<Outline>().effectColor = buttons.sColorPause;
        }
        view.textNumber.text = System.Convert.ToString(planet.Key);
        view.buttonName.GetComponentInChildren<Text>().text = PP.textName;
        view.textTI.text = System.Convert.ToString(planet.Value.textTI);
    }

    /// <summary>
    /// UI: data for one planet (Planets)
    /// </summary>
    public class TestItemView
    {
        public Text textNumber;
        public Button buttonName;
        public Text textTI;

        // constructor
        public TestItemView(Transform rootView)
        {
            textNumber = rootView.Find("TextNumber").GetComponent<Text>();
            buttonName = rootView.Find("ButtonName").GetComponent<Button>();
            textTI = rootView.Find("TextTerraIndex").GetComponent<Text>();
        }
    }

}