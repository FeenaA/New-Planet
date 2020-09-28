using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsResearches: MonoBehaviour
{
    // text - amount of intervened days
    public GameObject textDays;
    // prefab item
    public RectTransform prefabPlanet;
    // parent
    public RectTransform content;
    // left planet
    public GameObject panelInform;
    // number of choosen palnet
    public static int nChoosenPlanet = 0;
    // chosen planet
    public static GameObject ChosenPlanet = null;

    // gameobjects to fill PanelInformation
    public GameObject Sphere;
    public static GameObject sSphere;
    public GameObject NamePlanet;
    public static GameObject sNamePlanet;
    public GameObject textIntro;
    public static GameObject sTextIntro;
    public GameObject buttonResearchSelect;
    public static GameObject sButtonResearchSelect;
    public GameObject panelInformation;
    public static GameObject sPanelInformation;

    // nesessary resources
    public Transform ResWater, ResAir, ResSoil;
    public static Transform rWater, rAir, rSoil;

    // extraordinary resources
    public Transform ResAdd1, ResAdd2, ResAdd3;
    public static Transform[] r;

    // amount of probes
    public GameObject textProbes;
    public static GameObject sTextProbes;
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
    public GameObject TextRequestedResources;
    public static GameObject sTextRequestedResources;

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

    void Start()
    {
        // gameobjects to fill PanelInformation
        sSphere = Sphere;
        sNamePlanet = NamePlanet;
        sTextIntro = textIntro;
        sTextRequestedResources = TextRequestedResources;
        sPanelInformation = panelInformation;

        rWater = ResWater;
        rAir = ResAir;
        rSoil = ResSoil;

        if (PersonalSettings.language == LanguageSettings.Language.English)
        { TextPlanet.GetComponent<Text>().text = "Planet"; }
        else { if ((PersonalSettings.language == LanguageSettings.Language.Russian))
            { TextPlanet.GetComponent<Text>().text = "ПЛАНЕТА"; }}

        r = new Transform[3] { ResAdd1, ResAdd2, ResAdd3};

        // button to Research or Select planet
        sButtonResearchSelect = buttonResearchSelect;

        // panel for different items
        sTextProbes = textProbes;
        sTextSC = textSC;
        sTextEth = textEth;
        sTextBC = textBC;
        sTextProbes.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NProbe);
        sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NSpasecraft);
        sTextEth.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NEther);
        sTextBC.GetComponent<Text>().text = System.Convert.ToString(BlueCoin.sNBlueCoin);

        // craw line 
        crawlLine cl = ImageCrawlLine.GetComponent<crawlLine>();
        settings.sTitleCrawlLine = title;
        cl.Show("Water was successfully transported from your native planet!");

        // fill information about planets
        OnReceivedModels();

        ShowProgress SP = panelInformation.GetComponent<ShowProgress>();
        sTextRequestedResources.GetComponent<Text>().text = 
            SP.Show(settings.gameSettings.RequestedResources);
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
            instance.transform.name = "Planet" + nPlanet;
            if (nChoosenPlanet == nPlanet)
            {
                ChosenPlanet = instance;
                //sPlanet = planet;
                ItemOnClick.sButtonName = instance.transform.Find("ButtonName").GetComponent<Button>();

                // item select
                ItemOnClick click = GameObject.Find("SettingsResearches").GetComponent<ItemOnClick>();
                click.ItemSelect(instance);
            }
            nPlanet++;
        }
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


