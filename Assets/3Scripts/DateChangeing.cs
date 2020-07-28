using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    // account of coins
    public static int sCoins = 99999;
    // account of everyday coins
    public static int stepCoins = 2;
    // reference
    public GameObject textCoinsObject;
    public static GameObject sTextCoinsObject;

    private int MaxCoins = 99999;

    void Start()
    {
        pause = false;
        sTextCoinsObject = textCoinsObject;
        GetComponent<Text>().text = settings.sStringTextDays;
        sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

        InvokeRepeating("changeData", 0, nSecondsStep);
    }

    void changeData()
    {
        if (!pause)
        {
            // days counter increment 
            settings.sStringTextDays = "day " + nDay;
            GetComponent<Text>().text = settings.sStringTextDays;

            // coins increment
            if (sCoins + stepCoins > MaxCoins)
            {
                sCoins = MaxCoins;
            }
            else
            {
                sCoins += stepCoins;
            }
            
            sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

            // probes increment
            if (BuildingsOperations.ProbeFactory.N > 0 )
            {
                nDayPF++;
                if (nDayPF == BuildingsOperations.ProbeFactory.Time)
                {
                    settings.sNProbes++;
                    nDayPF = 0;
                }
            }

            // spacecrafts increment
            if (BuildingsOperations.SCfactory.N > 0)
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
}

