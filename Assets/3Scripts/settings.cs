using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settings : MonoBehaviour
{
    // prefab - PauseRectangle aroung the scene
    public GameObject prefabPauseRectangle;
    public static GameObject sPrefabPauseRectangle;
    // text - amount of coins
    public GameObject textCoins;
    public static GameObject sTextCoins;
    // text - amount of intervened days
    public GameObject textDays;
    public static GameObject sTextDays;
    public static string sStringTextDays;
    // colors of "textCoins" and "textDays"
    public static Color sColorProcess = new Color(255, 255, 255); //white
    public static Color sColorPause = new Color(221, 84, 0);//orange
    public static Color sColorCurrent;

    // Sprites, Button - changeing images of ButtonPause
    public Sprite pauseImage;
    public Sprite continueImage;
    public static Sprite sPauseImage;
    public static Sprite sContinueImage;
    
    public Button buttonPause;
    public static Button sButtonPause;
    // flag - was the game paused before the instance of prefab instanciated?
    public static bool flagPauseBeforePrefab = false;

    // name of the native planet
    public static string sNameNativePlanet = null;
    public static int sValNativePlanet;

    //private static int sNSymbols;

    public static Dictionary<int, string> ResourceNess = new Dictionary<int, string>
    {
        {1, "Air   = " },
        {2, "Water = " },
        {3, "Soil  = " }
    };

    // amount of planets
    public static int sNPlanets = 50;
    // information about planets
    public static TestItemModel[] sSetPlanets;
    //public static TestItemModel sItemPlanet;
    public static int nActivePlanet = 0;

    // materials for planets
    public Material[] materials;
    public static Material[] sMaterials;

    // !!! 0
    public static int NProbes = 3;
    // !!! download
    public static int nLanguage = 0; // 0 - Russian, 1 - English

    void Start()
    {
        // read all informationf from *.xml
        readAll.GetAll();

        sPrefabPauseRectangle = prefabPauseRectangle;
        sTextCoins = textCoins;
        sTextDays = textDays;
        sStringTextDays = sTextDays.GetComponent<Text>().text;
        sColorCurrent = sColorProcess;
        sPauseImage = pauseImage;
        sContinueImage = continueImage;
        sButtonPause = buttonPause;

        if (flagPauseBeforePrefab)  {buttons.sPausePressed();}

        // set of materials for planets
        if (sMaterials == null)
        {
            sMaterials = new Material[materials.Length];
            int L = materials.Length;
            for (int i = 0; i < L; i++)
            {sMaterials[i] = materials[i];}
        }

        // information about planets
        if (String.IsNullOrEmpty(sNameNativePlanet))
        {
            sNameNativePlanet = getItems.NameGenerate(out sValNativePlanet);
            sSetPlanets = getItems.GetItems();
        }
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