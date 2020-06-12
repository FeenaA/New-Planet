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
    // 
    public Button buttonPause;
    public static Button sButtonPause;
    // flag - was the game paused before the instance of prefab instanciated?
    public static bool flagPauseBeforePrefab = false;

    //
    public static string sNameNativePlanet = null;
    public static int sValNativePlanet;
    public static string[] sGreekAlph = {
        "Alpha", "Beta", "Gamma", "Delta", "Epsilon", "Zeta", "Eta", "Theta", "Iota","Kappa", "Lambda", "Mu",
        "Nu", "Xi", "Omicron", "Pi", "Rho", "Sigma", "Tau", "Upsilon", "Phi", "Chi", "Psi", "Omega" };
    public static int sNSymbols;
    // amount of planets
    public static int sNPlanets = 50;
    // information about planets
    public static TestItemModel[] sSetPlanets;
    public static TestItemModel sItemPlanet;
    public static int nActivePlanet = 0;

    // materials for planets
    public Material[] materials;
    public static Material[] sMaterials;

    public class planetProperty
    {
        public int numMaterial;
        public string textName;
        public string textIntroduction;
        public string textResources;
    }

    public static Dictionary<int, planetProperty> sPlanetProperty;

    void Start()
    {
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
        sNSymbols = sGreekAlph.Length - 1;
        if (String.IsNullOrEmpty(sNameNativePlanet))
        {
            sNameNativePlanet = NameGenerate(out sValNativePlanet);
            sSetPlanets = GetItems();
        }
    }

    // Name for Native Planet
    private static string NameGenerate(out int value_Numb)
    {
        // choise particular name
        System.Random rnd = new System.Random();
        int value_Symbol = rnd.Next(0, sNSymbols);
        value_Numb = rnd.Next(12, 999);
        return sGreekAlph[value_Symbol] + value_Numb;
    }

    // getting information about other planets
    private TestItemModel[] GetItems()
    {
        // accountant of planets
        int NPlanets = settings.sNPlanets;
        // set of planets
        TestItemModel[] results = new TestItemModel[NPlanets];

        string[] Name = SetNameGenerate(NPlanets);
        int[] TI = SetTIndexGenerate(NPlanets);
        int[] NMat = SetNumMaterial(NPlanets);        

        sPlanetProperty = new Dictionary<int, planetProperty>();

        for (int i = 0; i < NPlanets; i++)
        {
            results[i] = new TestItemModel
            {
                textNumber = System.Convert.ToString(i + 1),
                textName = Name[i],
                textTI = TI[i],

                flagActive = false,
                flagResearched = false
            };

            planetProperty PP = new planetProperty
            {
                numMaterial = NMat[i],
                textName = Name[i],
                textIntroduction = "intro " + System.Convert.ToString(i + 201),
                textResources = "resourses " + System.Convert.ToString(i + 101)
            };

            sPlanetProperty.Add((i + 1), PP);
        }

        results[nActivePlanet].flagActive = true;

        return results;
    }

    // generator: set of probable Names
    private static string[] SetNameGenerate(int NPlanets)
    {
        // generate set of unique numbers 
        int minVal = 12, maxVal = 999;
        int[] Numbers = SetNumbersGenerate(NPlanets, minVal, maxVal);
        System.Random rnd = new System.Random();
        string[] Name = new string[NPlanets];
        for (int i = 0; i < NPlanets; i++)
        {
            Name[i] = sGreekAlph[rnd.Next(0, sNSymbols)] + '-' + Numbers[i];
        }

        return Name;
    }

    // generator: set of unique numbers
    private static int[] SetNumbersGenerate(int len, int minVal, int maxVal)
    {
        // minVal = 12, maxVal = 999
        System.Random rnd = new System.Random();
        int[] array = new int[len];
        // number of native planet
        int value_Numb = settings.sValNativePlanet;

        for (int i = 0; i < len; i++)
        {
            bool flag_isUnique;
            do
            {
                array[i] = rnd.Next(minVal, maxVal);
                flag_isUnique = true;

                for (int j = 0; j < i; j++)
                {
                    int A = array[i];
                    if ((A == array[j]) || (A == value_Numb))
                    {
                        flag_isUnique = false;
                        break;
                    }
                }
            } while (!flag_isUnique);
        }
        return array;
    }

    // generator: set of terraindexes
    private static int[] SetTIndexGenerate(int len)
    {
        // 15% - good (TI: 90-95)
        // 20% - bad (TI: 10-25)
        // 65% - normal (TI: 26-89)

        int[] tempSetTIndex = new int[len];
        System.Random rnd = new System.Random();
        // good
        int n_good = len * 15 / 100;
        for (int i = 0; i < n_good; i++) { tempSetTIndex[i] = rnd.Next(90, 95); }
        // bad
        int n_bad = len * 20 / 100;
        int n_gb = n_good + n_bad;
        for (int i = n_good; i < n_gb; i++) { tempSetTIndex[i] = rnd.Next(10, 25); }
        // normal
        for (int i = n_gb; i < len; i++) { tempSetTIndex[i] = rnd.Next(26, 89); }

        // set 4 unique indexes
        List<int> num = new List<int>();
        for (int i = 0; i < len; i++) { num.Add(i); }

        // set 4 mix of terraindexes
        int[] setTIndex = new int[len];
        int minVal = 0, maxVal = len - 1;
        for (int i = 0; i < len; i++)
        {
            int n = rnd.Next(minVal, maxVal);
            maxVal--;
            setTIndex[i] = tempSetTIndex[num[n]];
            num.Remove(n);
        }

        return setTIndex;
    }

    // generator: set of materials numbers
    private static int[] SetNumMaterial(int len)
    {
        int[] tempSetMaterial = new int[len];
        System.Random rnd = new System.Random();

        int L = sMaterials.Length;
        for (int i = 0; i < len; i++)
        {
            tempSetMaterial[i] = rnd.Next(0, L - 1);
        }

        return tempSetMaterial;
    }

    // data for one planet 
    public class TestItemModel
    {
        public string textNumber;
        public string textName;
        public int textTI; // terraindex
        public int numMaterial;

        public bool flagActive; // 0 - not active, 1 - active
        public bool flagResearched; // 0 - not researched, 1 - researched

        public string textIntroduction;
        public string textResources;
        // public int[3] nessesaryResourses; // delivered once
        // public int[5] extraResourses; // needs regular delivery
    }
}


