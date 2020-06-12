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
    // prefab - RectangleSelected aroung the chosen planet
    //public GameObject prefabRectangleSelected = null;
    public static GameObject sRectSelect = null;
    // chosen planet
    public static GameObject ChosenPlanet = null;
    public static settings.TestItemModel sPlanet = null;

    // gameobjects to fill PanelInformation
    public GameObject Sphere;
    public static GameObject sSphere;
    public GameObject NamePlanet;
    public static GameObject sNamePlanet;

    void Start()
    {
        // gameobject to fill PanelInformation
        sSphere = Sphere;
        sNamePlanet = NamePlanet;

        // upload settings
        textDays.GetComponent<Text>().text = settings.sStringTextDays;
        textDays.GetComponent<Text>().color = settings.sColorCurrent;

        // fill information about planets
        OnReceivedModels(settings.sSetPlanets);
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
                ItemOnClick.ItemSelect();
            }
            nPlanet++;
        }
    }

    // connection between UI and script 
    void InitializeItemView(Transform viewGameObject, settings.TestItemModel planet)
    {
        TestItemView view = new TestItemView(viewGameObject);
        view.textNumber.text = System.Convert.ToString(planet.numPlanet);
        view.buttonName.GetComponentInChildren<Text>().text = planet.textName;
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

    // UI: data for one planet (Inform)
    public class TestInformView
    {
        public Material material;
        public Text textIntroduction;
        public Text textResources;

        // constructor
        public TestInformView(Transform rootView)
        {
            material = rootView.Find("Sphere").GetComponent<Material>(); 
            textIntroduction = rootView.Find("TextIntroduction").GetComponent<Text>();
            textResources = rootView.Find("TextResources").GetComponent<Text>();
        }
    }

}


