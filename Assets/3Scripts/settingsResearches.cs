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

    public class AcceptRes
    {
        // amount of resource
        public int amount;
        // number of source planet
        public string NamePlanet;
    }
    // key - number of resource
    public static Dictionary<int, List<AcceptRes>> Storage = new Dictionary<int, List<AcceptRes>>();


    //public GameObject prefabCrawnLine;

    // image with script
    public RectTransform ImageCrawlLine;
    public GameObject anchor;
    public static GameObject sPrefabCrawnLine;
    //public static RectTransform sImageCrawlLine;
    public static GameObject sAnchor;
    public GameObject title; 
    public static GameObject sTitle;


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
        textDays.GetComponent<Text>().color = buttons.sColorCurrent;

        sTextProbes = textProbes;
        sTextProbes.GetComponent<Text>().text = System.Convert.ToString(settings.sNProbes);

        // craw line 
        //sImageCrawlLine = ImageCrawlLine;
        crawlLine cl = ImageCrawlLine.GetComponent<crawlLine>();
        sTitle = title;
        cl.Show("Water was successfully transported from your native planet!");
        //sPrefabCrawnLine = prefabCrawnLine;
        //sPanelCrawlLine = panelCrawlLine; 
        //sAnchor = anchor;
        //crawlLine.Show("Water was successfully transported from your native planet!");

        sTextSC = textSC;
        sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

        // fill information about planets
        OnReceivedModels(settings.sSetPlanets);

        sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
    }

   /* private void MoveCrawlLine()
    {
        //Vector2 sizeContent = imageCrawnLine.transform.GetComponent<SpriteRenderer>().sprite.rect.size;
        //Vector2 sz = imageCrawnLine.transform.GetComponent<SpriteRenderer>().sprite.rect.size;
        float width = imageCrawnLine.rect.width;
        float dist = 0;
        while (dist < width)
        //for (int i = 0; i < 5; i++)
        {
            StartCoroutine(StepCrawlLine());
            //yield return new WaitForSeconds(movementSpeed);
            imageCrawnLine.transform.position = textCrawnLine.transform.position + new Vector3(-stepSize, 0, 0);
            dist += stepSize;
            print(dist);
        }
    }

    IEnumerator StepCrawlLine()
    {
        yield return new WaitForSeconds(movementSpeed);
    }*/

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
            view.buttonName.GetComponent<Outline>().effectColor = buttons.sColorPause;
            viewGameObject.GetComponent<Outline>().enabled = true;
            viewGameObject.GetComponent<Outline>().effectColor = buttons.sColorPause;
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


