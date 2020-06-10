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
    // account of coins
    private static int sCoins = 500;
    // account of everyday coins
    public static int stepCoins = 2;
    // reference
    public GameObject textCoinsObject; 
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        GetComponent<Text>().text = settings.sStringTextDays;
        textCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

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
            sCoins += stepCoins;
            textCoinsObject.GetComponent<Text>().text = Convert.ToString(sCoins);

            nDay++;
        }
    }
}

