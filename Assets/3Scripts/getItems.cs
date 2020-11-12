using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getItems : MonoBehaviour
{
    public static List<string> sGreekAlph;
    public static List<string> sAdditionalResources;
    private static int NSymbols;
    private static int NPlanets;

    private static System.Random rnd;
    private static int value_Numb = 0;

    // materials for planets
    public static Material[] sMaterials;

    public class ResourceInformation
    {
        public string name;
        public int cost;
        public ResourceInformation(string Name, int Cost)
        {
            name = Name;
            cost = Cost;
        }
    }

    // nesessary resources
    public static Dictionary<int, ResourceInformation> ResNess;
    // extra resources
    public static Dictionary<int, ResourceInformation> ResourceAdd;
    // introduction
    public static Dictionary<int, string> sIntroduction;

    public class PlanetProperty
    {
        public string textName;
        public int textTI;
        public int numMaterial;
        public int numIntro;
        public int[] ResNess_Amount = new int[3];
        public int[] ResAdd = new int[3];// numbers of resources
        public int[] ResAddAmount = new int[3];// amount of resources
        public int amountProbes;
        public int amountCoins;
        public bool flagIsResearched = false;
        public bool flagIsSelected = false;
        public bool flagEther = false;
        public bool flagCoins = false;
    }

    /// <summary>
    /// requirements to the SelectedPlanet (number of resources and amount)
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> SetReqs()
    {
        Dictionary<int, int> res = new Dictionary<int, int>();

        foreach (var resourse in ResNess)
        {
            res[resourse.Key] = 0;
        }
        // extarordinary resources
        int[] arr3 = SetNumbersGenerate(3, 1, ResourceAdd.Count);
        for (int i = 0; i < 3; i++)
        {
            res[arr3[i]] = 0;
        }
        return res;
    }

    private int[] TI;
    /// <summary>
    /// getting set of planets with information about them
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, PlanetProperty>  GetItems()
    {
        NPlanets = settings.sNPlanets;
        NSymbols = sGreekAlph.Count;

        string[] Name = SetNameGenerate(NPlanets);
        Dictionary<int, int[]> resources = new Dictionary<int, int[]>();
        TI = new int[NPlanets];
        SetTIndexGenerate(NPlanets, ref TI, ref resources);
        int[] AP = new int[NPlanets]; // amount of probes
        int[] Coins = new int[NPlanets]; // treasure - amount of coins
        SetProbesGenerate(NPlanets, ref AP, ref Coins);
         
        rnd = new System.Random();
        int NMat = sMaterials.Length - 1;
        int NIntro = sIntroduction.Count;

        // key - number of planet, value - number of an additional resource
        Dictionary<int, int[]> resAdd = new Dictionary<int, int[]>();
        int NResAdd = ResourceAdd.Count;
        for (int i = 0; i < NPlanets; i++)
        {
            // generate a set of unique numbers
            int[] arr3 = SetNumbersGenerate(3, 1, NResAdd);
            resAdd.Add(i, arr3);
        }

        Dictionary<int, PlanetProperty> result = new Dictionary<int, PlanetProperty>();

        int minRes = 3, maxRes = 10;

        for (int i = 0; i < NPlanets; i++)
        {
            PlanetProperty PP = new PlanetProperty
            {
                textName = Name[i],
                textTI = TI[i],
                numMaterial = rnd.Next(0, NMat),
                numIntro = rnd.Next(1, NIntro),
                // nesessary resources
                ResNess_Amount = resources[i],
                // numbers of resources
                ResAdd = resAdd[i],
                // amounts of extraordinary resources
                ResAddAmount = new int[] { rnd.Next(minRes, maxRes),
                rnd.Next(minRes, maxRes), rnd.Next(minRes, maxRes) },
                amountProbes = AP[i],
                amountCoins = Coins[i],
                flagCoins = (Coins[i] > 0)
            };

            PP.flagCoins = Treasure(TI[i]);
            PP.flagEther = Treasure(TI[i]);
            // trigger to change coins or ether
            bool trigger = false;
            if (PP.flagCoins && PP.flagEther)
            {
                if (trigger)
                { 
                    PP.flagCoins = false;
                    PP.amountProbes = 0;
                    PP.amountCoins = 0; 
                }
                else { 
                    PP.flagEther = false; 
                    PP.amountCoins = 1000; 
                }
                trigger = !trigger;
            }

            result.Add((i + 1), PP);
        }
        return result;
    }

    /// <summary>
    /// does the planet have a treasure on its surface?
    /// </summary>
    /// <param name="terraIndex">terraIndex of the current planeet</param>
    /// <returns></returns>
    private bool Treasure(int terraIndex)
    {
        int N = 0;
             if (terraIndex <= 20) { N = 4; }
        else if ((terraIndex > 20) && (terraIndex <= 40)) { N = 6; }
        else if ((terraIndex > 40) && (terraIndex <= 60)) { N = 10; }
        else if ((terraIndex > 60) && (terraIndex <= 80)) { N = 30; }
        else if (terraIndex > 80) { N = 40; }

        if (rnd.Next(N-1) == 0) { return true; }
        return false;
    }

    /// <summary>
    /// generator: set of probable Names
    /// </summary>
    /// <param name="NPlanets">amount of Names</param>
    /// <returns></returns>
    private string[] SetNameGenerate(int NPlanets)
    {
        // generate set of unique numbers 
        int minVal = 12, maxVal = 999;
        rnd = new System.Random();
        int[] Numbers = SetNumbersGenerate(NPlanets, minVal, maxVal);
        string[] Name = new string[NPlanets];
        for (int i = 0; i < NPlanets; i++)
        {
            Name[i] = sGreekAlph[rnd.Next(0, NSymbols)] + '-' + Numbers[i];
        }
        return Name;
    }

    /// <summary>
    /// generator: set of terraindexes
    /// </summary>
    /// <param name="len">amount of elements in the set</param>
    /// <param name="setTIndex">the set of TerraIndex</param>
    /// <param name="resources">amount of necessary resources</param>
    private void SetTIndexGenerate(int len, ref int[] setTIndex, ref Dictionary<int, int[]> resources)
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
            int[] temp = { resNess[3 * N], resNess[3 * N + 1], resNess[3 * N + 2] };
            resources[i] = temp;
            num.Remove(n);
        }
    }

    /// <summary>
    /// generator: set of necessary probes and treasure
    /// </summary>
    /// <param name="len"></param>
    /// <param name="setTIndex"></param>
    /// <param name="ProbesAmount"></param>
    /// <param name="treasure"></param>
    private void SetProbesGenerate(int len, ref int[] ProbesAmount, ref int[] treasure)
    {
        for (int i = 0; i < len; i++)
        {
            int N = 0;
            int terraIndex = TI[i];
            if (terraIndex <= 20) { N = 4; }
            else if ((terraIndex > 20) && (terraIndex <= 40)) { N = 6; }
            else if ((terraIndex > 40) && (terraIndex <= 60)) { N = 10; }
            else if ((terraIndex > 60) && (terraIndex <= 80)) { N = 30; }
            else if (terraIndex > 80) { N = 40; }

            int Rnd = rnd.Next(N - 1);
            if (Rnd != 0) {  ProbesAmount[i] = 0; }
            else { ProbesAmount[i] = (terraIndex - N) / 10;}

            treasure[i] = (100 - TI[i]) * 50; // (100 - 10)*50 = 4500

            print(terraIndex + " " + ProbesAmount[i] + " " + treasure[i]);
        }


    }

    /// <summary>
    /// generator: set of unique numbers
    /// </summary>
    /// <param name="len">amount of elements in the returned array</param>
    /// <param name="minVal">low border</param>
    /// <param name="maxVal">high border</param>
    /// <returns></returns>
    private static int[] SetNumbersGenerate(int len, int minVal, int maxVal)
    {
        int[] array = new int[len];

        for (int i = 0; i < len; i++)
        {
            bool flag_isUnique;
            do
            {
                array[i] = rnd.Next(minVal, maxVal);
                //array[i] = rndNumbers.Next(minVal, maxVal);
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

    /// <summary>
    /// Name for Native Planet
    /// </summary>
    /// <returns></returns>
    public string NameGenerate()
    {
        // choise particular name
        System.Random rnd = new System.Random();
        int value_Symbol = rnd.Next(0, sGreekAlph.Count);
        value_Numb = rnd.Next(12, 999);
        return getItems.sGreekAlph[value_Symbol] + "-" + value_Numb;
    }
}


