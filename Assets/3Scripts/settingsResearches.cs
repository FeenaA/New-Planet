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
    public static settings.TestItemModel sPlanet = null;

    // gameobjects to fill PanelInformation
    public GameObject Sphere;
    public static GameObject sSphere;
    public GameObject NamePlanet;
    public static GameObject sNamePlanet;
    public GameObject textIntro;
    public static GameObject sTextIntro;
    public GameObject buttonResearchSelect;
    public static GameObject sButtonResearchSelect;

    // nesessary resources
    public Transform ResWater, ResAir, ResSoil;
    public static Transform rWater, rAir, rSoil;

    // extraordinary resources
    public Transform ResAdd1, ResAdd2, ResAdd3;
    public static Transform r1, r2, r3;

    // amount of probes
    public GameObject textProbes;
    public static GameObject sTextProbes;

    // amount of spacecrafts
    public GameObject textSC;
    public static GameObject sTextSC;

    // text for requested resources
    public GameObject TextRequestedResources;
    public static GameObject sTextRequestedResources;

    public class AcceptableResource
    {
        int amount;
        // number of source planet
        int numPlanet;
    }
    // key - number of resource
    public static Dictionary<int, List<AcceptableResource>> ARes = new Dictionary<int, List<AcceptableResource>>();

    void Start()
    {
        // gameobjects to fill PanelInformation
        sSphere = Sphere;
        sNamePlanet = NamePlanet;
        sTextIntro = textIntro;
        sTextRequestedResources = TextRequestedResources;

        rWater = ResWater;
        rAir = ResAir;
        rSoil = ResSoil;

        r1 = ResAdd1;
        r2 = ResAdd2;
        r3 = ResAdd3;

        // button to Research or Select planet
        sButtonResearchSelect = buttonResearchSelect;

        // upload settings
        textDays.GetComponent<Text>().text = settings.sStringTextDays;
        textDays.GetComponent<Text>().color = settings.sColorCurrent;

        sTextProbes = textProbes;
        sTextProbes.GetComponent<Text>().text = System.Convert.ToString(settings.sNProbes);

        sTextSC = textSC;
        sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

        // fill information about planets
        OnReceivedModels(settings.sSetPlanets);

        sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
    }

    // fill information about planets
    void OnReceivedModels(settings.TestItemModel[] setPlanets)
    {
        // by default the Planet0 is chosen
        int nPlanet = 0;
        // add new items with data
        foreach (var planet in setPlanets)
        {
            var instance = Instantiate(prefabPlanet.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            InitializeItemView(instance.transform, planet);

            // name of an object in inspector
            instance.transform.name = "Planet" + nPlanet;
            if (nChoosenPlanet == nPlanet)
            {
                ChosenPlanet = instance;
                sPlanet = planet;
                ItemOnClick.sButtonName = instance.transform.Find("ButtonName").GetComponent<Button>();
                ItemOnClick.ItemSelect(instance);
            }
            nPlanet++;
        }
    }

    // connection between UI and script 
    void InitializeItemView(Transform viewGameObject, settings.TestItemModel planet)
    {
        TestItemView view = new TestItemView(viewGameObject);

        getItems.PlanetProperty PP = getItems.sPlanetProperty[planet.numPlanet];
        if (PP.flagIsResearched)
        {
            viewGameObject.GetComponent<Image>().color = Color.black;
            view.buttonName.transform.GetComponent<Image>().color = Color.black;
        }
        if (PP.flagIsSelected)
        {
            view.buttonName.GetComponent<Outline>().enabled = true;
            view.buttonName.GetComponent<Outline>().effectColor = settings.sColorPause;
            viewGameObject.GetComponent<Outline>().enabled = true;
            viewGameObject.GetComponent<Outline>().effectColor = settings.sColorPause;
        }
        view.textNumber.text = System.Convert.ToString(planet.numPlanet);
        view.buttonName.GetComponentInChildren<Text>().text = PP.textName;
        view.textTI.text = System.Convert.ToString(planet.textTI);
    }

    // UI: data for one planet (Planets)
    public class TestItemView
    {
        public Text textNumber;
        //public Text textName;
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


