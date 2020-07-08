using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getItems : MonoBehaviour
{

    public static List<string> sGreekAlph;
    public static List<string> sIntroduction;
    public static List<string> sAdditionalResources;
    private static int NSymbols;
    private static int NPlanets;

    private static System.Random rnd;

    // download
    public static Dictionary<int, string> ResourceAdd;

    public class planetProperty
    {
        public int numMaterial;
        public string textName;
        public int numIntro;
        public Vector3 ResNess_Amount;
        public Vector3 ResAdd;// numbers of resources
        public Vector3 ResAddAmount;// amounts of resources
        public bool flagIsResearched = false;
        public bool flagIsSelected = false;
    }



    // the Dictionary containing all information about all planets
    public static Dictionary<int, planetProperty> sPlanetProperty;

    // requirements to the SelectedPlanet (number of resources and amount)
    public static Dictionary<int, int> setReqs()
    {
        Dictionary<int, int> res = new Dictionary<int, int>();
        int[] arr3 = SetNumbersGenerate(3, 1, ResourceAdd.Count);
        for (int i = 0; i < 3; i++)
        {
            res[arr3[i]] = 0;
        }
        return res;
    }

    // getting information about other planets
    public static settings.TestItemModel[] GetItems()
    {
        NPlanets = settings.sNPlanets;
        NSymbols = sGreekAlph.Count;

        // set of planets
        settings.TestItemModel[] results = new settings.TestItemModel[NPlanets];

        string[] Name = SetNameGenerate(NPlanets);
        Dictionary<int, Vector3> resources = new Dictionary<int, Vector3>();
        int[] TI = new int[NPlanets];
        SetTIndexGenerate(NPlanets, ref TI, ref resources);

        rnd = new System.Random();
        //System.Random rnd = new System.Random();
        int NMat = settings.sMaterials.Length - 1;
        int NIntro = sIntroduction.Count - 1;

        // key - number of planet, value - number of an additional resource
        Dictionary<int, Vector3> resAdd = new Dictionary<int, Vector3>();
        int NResAdd = ResourceAdd.Count;
        //rnd = new System.Random();
        for (int i = 0; i < NPlanets; i++)
        {
            // generate a set of unique numbers
            int[] arr3 = SetNumbersGenerate(3, 1, NResAdd);
            //print(arr3[0] + " " + arr3[1] + " " + arr3[2]);
            resAdd.Add(i, new Vector3(arr3[0], arr3[1], arr3[2]));
        }
        

        // key - number of planet, value - amount of an additional resource
        //Dictionary<int, Vector3> resAddAmount = new Dictionary<int, Vector3>();

        sPlanetProperty = new Dictionary<int, planetProperty>();
        int minRes = 3, maxRes = 10;

        for (int i = 0; i < NPlanets; i++)
        {
            results[i] = new settings.TestItemModel
            {
                numPlanet = i + 1,
                textName = Name[i],
                textTI = TI[i],

                flagActive = false,
                flagResearched = false
            };

            planetProperty PP = new planetProperty
            {
                textName = Name[i],
                numMaterial = rnd.Next(0, NMat),
                numIntro = rnd.Next(0, NIntro),
                // nesessary resources
                ResNess_Amount = resources[i],
                // myValue = myDictionary[myKey];
                ResAdd = resAdd[i],// numbers of resources
                ResAddAmount = new Vector3(rnd.Next(minRes, maxRes), 
                rnd.Next(minRes, maxRes), rnd.Next(minRes, maxRes)) // amounts of resources
            };

            sPlanetProperty.Add((i + 1), PP);
        }

        results[settings.nActivePlanet].flagActive = true;

        return results;
    }

    // generator: set of probable Names
    private static string[] SetNameGenerate(int NPlanets)
    {
        // generate set of unique numbers 
        int minVal = 12, maxVal = 999;
        //System.Random rnd = new System.Random();
        rnd = new System.Random();
        int[] Numbers = SetNumbersGenerate(NPlanets, minVal, maxVal);
        string[] Name = new string[NPlanets];
        for (int i = 0; i < NPlanets; i++)
        {
            Name[i] = sGreekAlph[rnd.Next(0, NSymbols)] + '-' + Numbers[i];
        }
        return Name;
    }

    // generator: set of terraindexes
    private static void SetTIndexGenerate(int len, ref int[] setTIndex, ref Dictionary<int, Vector3> resources)
    {
        System.Random rnd = new System.Random();
        int[] tempSetTIndex = new int[len];

        int len2 = 2 * len, len3 = 3 * len;
        int[] resNess = new int[len * 3];
        // good
        for (int i = 0; i < len; i++) { resNess[i] = rnd.Next(7, 10); }
        // bad
        for (int i = len; i < len2; i++) { resNess[i] = rnd.Next(0, 4); }
        // normal
        for (int i = len2; i < len3; i++) { resNess[i] = rnd.Next(0, 10); }
        // TI
        int k = 0;
        for (int i = 0; i < len3; i += 3)
        {
            tempSetTIndex[k++] = (int)((resNess[i] + resNess[i + 1] + resNess[i + 2]) / 0.3);
        }

        // set 4 unique indexes
        List<int> num = new List<int>();
        for (int i = 0; i < len; i++) { num.Add(i); }

        // set 4 mix of terraindexes
        int minVal = 0, maxVal = len - 1;
        for (int i = 0; i < len; i++)
        {
            int n = rnd.Next(minVal, maxVal);
            maxVal--;
            int N = num[n];
            setTIndex[i] = tempSetTIndex[N];
            //int[] temp = { resNess[3 * N], resNess[3 * N + 1], resNess[3 * N + 2] };
            //resources.Add(i, temp);
            Vector3 temp = new Vector3( resNess[3 * N], resNess[(3 * N) + 1], resNess[3 * N + 2] );
            //resources[i] = temp;
            resources.Add(i, temp);
            num.Remove(n);
        }
    }

    // generator: set of unique numbers
    private static int[] SetNumbersGenerate(int len, int minVal, int maxVal)
    {
        //System.Random rnd = new System.Random();
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

    // Name for Native Planet
    public static string NameGenerate(out int value_Numb)
    {
        // choise particular name
        System.Random rnd = new System.Random();
        int value_Symbol = rnd.Next(0, sGreekAlph.Count);
        value_Numb = rnd.Next(12, 999);
        return getItems.sGreekAlph[value_Symbol] + "-" + value_Numb;
    }
}


