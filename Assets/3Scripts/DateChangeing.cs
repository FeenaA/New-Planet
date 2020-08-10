using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // это важно

// Колесо Сансары дало оборот
public class DateChangeing : MonoBehaviour
{
    // флаг для отслеживания паузы
    public static bool pause;
    // number of seconds between days counter increment
    public int nSecondsStep;
    public string result;

    private static int nDay = 1;
    private static int nDayPF = 0;
    private static int nDaySC = 0;
    // amount of coins
    public static int sCoins = 50000;
    // account of everyday coins
    public static int stepCoins = 2;
    // Coins
    public GameObject textCoinsObject;
    public static GameObject sTextCoinsObject;
    // People
    public GameObject peopleOnNative;
    public GameObject peopleOnNew;
    public GameObject peopleDied;
    
    /*public static GameObject sPeopleOnNative;
    public static GameObject sPeopleOnNew;
    public static GameObject sPeopleDied;*/

    //private readonly int PeopleStart = 1000000;
    private readonly int DaysWithoutDeth = 10;
    public static int DayDeth = 0;
    public readonly static float koefPeopleStart = 0.1f;
    private static int DiedToday = 1;
    public static float koefToday = 0.1f;
    // initial amount of people
    private static int NPeopleOnNative = 1000000;
    private static int NPeopleOnNew = 0;
    private static int NPeopleDied = 0;

    private readonly int MaxCoins = 99999;
    private readonly int MaxNProbes = 99;
    private readonly int MaxNSpacecraft = 99;

    private string SceneName;
    
    void Start()
    {
        pause = false;
        sTextCoinsObject = textCoinsObject;
        GetComponent<Text>().text = settings.sStringTextDays;
        sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

        // the name of the current scene
        SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Game")
        {
            // Does the selected planet exist?
            if (panelInform.flagSelectedPlanet == true) { peopleOnNew.SetActive(true); }
        }

        InvokeRepeating("changeData", 0, nSecondsStep);
    }

    void changeData()
    {
        if (!pause)
        {
            // days counter increment 
            settings.sStringTextDays = "day " + nDay;
            GetComponent<Text>().text = settings.sStringTextDays;

            // increase and show amount of coins
            sCoins = AddCoins(stepCoins);
            sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

            // people
            //GetPeopleAmount();
            if (SceneName == "Game" )
            {
                // first several days amout of people is constant
                if (nDay > DaysWithoutDeth)
                {
                    // people've started to die
                    peopleDied.SetActive(true);
                    GetPeopleAmount();
                }
                if (!String.IsNullOrEmpty(settings.sNameNativePlanet))
                {
                    peopleOnNative.GetComponent<Text>().text = "On " + settings.sNameNativePlanet + " (native): " + 
                        Convert.ToString(NPeopleOnNative);
                }
                if( NPeopleDied > 0 )
                { peopleDied.GetComponent<Text>().text = "Died: " + Convert.ToString(NPeopleDied); }
                if( panelInform.flagSelectedPlanet == true )
                { 
                    peopleOnNew.GetComponent<Text>().text = "On " + settings.SelectedPlanet.textName + " (new): " + 
                        Convert.ToString(NPeopleOnNew); 
                }
            }



            // probes increment
            if ((BuildingsOperations.ProbeFactory.N > 0 ) && (settings.sNProbes < MaxNProbes))
            {
                nDayPF++;
                if (nDayPF == BuildingsOperations.ProbeFactory.Time)
                {
                    settings.sNProbes++;
                    nDayPF = 0;
                }
            }

            // spacecrafts increment
            if ((BuildingsOperations.SCfactory.N > 0) && (settings.sNProbes < MaxNSpacecraft))
            {
                nDaySC++;
                if (nDaySC == BuildingsOperations.ProbeFactory.Time) 
                {
                    settings.sNSpacecraft++;
                    nDaySC = 0;
                }
            }

            nDay++;
        }
    }

    // get NPeopleDied, NPeopleOnNative and make actual GOs active
    private void GetPeopleAmount()
    {
        //print(koefToday + " " + DiedToday);

        //int day = nDay - DaysWithoutDeth;
        DayDeth++;
        koefToday += 0.112f;
        //DiedToday = day * System.Convert.ToInt32(koefToday);
        DiedToday = DayDeth * System.Convert.ToInt32(koefToday);
        NPeopleDied += DiedToday;
        NPeopleOnNative -= DiedToday;
    }

    // clever coins increment
    public int AddCoins(int N)
    {
        int currentCoins = sCoins;
        if (currentCoins + N > MaxCoins)    { currentCoins = MaxCoins;  }
        else                                { currentCoins += N;}
        return currentCoins;
    }
}

