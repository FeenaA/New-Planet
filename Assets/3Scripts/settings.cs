using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    public GameObject PauseRectangle;
    public static GameObject sPrefabPauseRectangle;
    
    /*public static AllGameObjects GO;
    
    public GameObject PauseRectangle;*/





    public GameObject prefabTest;
    public static GameObject sPrefabTest;
    public GameObject Canvas;
    public static GameObject sCanvas;
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
    // colors of "textCoins" and "textDays"
    /*public static Color sColorProcess = new Color(255, 255, 255); //white
    public static Color sColorPause = new Color(221, 84, 0);//orange
    public static Color sColorCurrent;*/
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
    public static bool flagPauseBeforePrefab = false;

    public GameObject Earth;
    public GameObject Moon;

    // name of the native planet
    public static string sNameNativePlanet = null;
    public static int sValNativePlanet;
    private static int NMaterialEarth = 0;
    private static int NMaterialMoon = 0;

    // amount of planets
    public static int sNPlanets = 50;
    // information about planets
    public static TestItemModel[] sSetPlanets;
    //public static TestItemModel sItemPlanet;
    public static int nActivePlanet = 0;

    // materials for planets
    public Material[] materials;
    public static Material[] sMaterials;

    // !!! 
    public static int sNProbes = 20;
    public static int sNSpacecraft = 20;
    public static int sNEther = 3;
    public static int sNBlueCoin = 2;

    // !!! download
    public static int nLanguage = 0; // 0 - Russian, 1 - English

    public static bool flagSelectedPlanet = false;
    public static getItems.PlanetProperty SelectedPlanet;

    private static bool flagFirstTime = true;

    public static Dictionary<int, int> reqRes;

    void Start()
    {
        sPrefabPauseRectangle = PauseRectangle;

        // read all informationf from *.xml
        if (flagFirstTime == true)
        {
            flagFirstTime = false;

            readAll.GetAll();
            
            /*AllGameObjects GO = new AllGameObjects
            {
                pauseRectangle = PauseRectangle
            };*/

            // set of materials for planets
            sMaterials = new Material[materials.Length];
            int L = materials.Length;
            for (int i = 0; i < L; i++)
            { sMaterials[i] = materials[i]; }

            // materials for Earth and Moon
            System.Random rnd = new System.Random();
            NMaterialEarth = rnd.Next(0, L - 1);
            NMaterialMoon = rnd.Next(0, L - 1);

            // information about planets
            if (String.IsNullOrEmpty(sNameNativePlanet))
            {
                sNameNativePlanet = getItems.NameGenerate(out sValNativePlanet);
                sSetPlanets = getItems.GetItems();
            }

            // set requested resources
            reqRes = getItems.setReqs();
        }

        Earth.GetComponent<Renderer>().material = settings.sMaterials[NMaterialEarth];
        Moon.GetComponent<Renderer>().material = settings.sMaterials[NMaterialMoon];

        sPanelResources = PanelResources;
        Transform TextReqs = sPanelResources.transform.Find("TextRequestedResources");
        TextReqs.GetComponent<Text>().text = showProgress.Show(reqRes);

        sTextCoins = textCoins;
        sTextDays = textDays;
        sStringTextDays = sTextDays.GetComponent<Text>().text;
        //sColorCurrent = sColorProcess;
        sPauseImage = pauseImage;
        sContinueImage = continueImage;
        sButtonPause = buttonPause;
        sCanvasBuildings = canvasBuildings;
        sPanelPeople = PanelPeople;
        sPrefabTest = prefabTest;
        sCanvas = Canvas;


        if (flagPauseBeforePrefab) { buttons.sPausePressed(); }
    }

    public class AllGameObjects
    {
        public GameObject pauseRectangle;
    }

    // data for one planet 
    public class TestItemModel
    {
        public int numPlanet;
        public string textName;
        public int textTI; // terraindex
        public int numMaterial;

        public bool flagActive; // 0 - not active, 1 - active
        public bool flagResearched; // 0 - not researched, 1 - researched
    }
}