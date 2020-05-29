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
    // reference
    public GameObject textCoinsObject; 
    // Start is called before the first frame update
    void Start()
    {
        pause = false;
        InvokeRepeating("changeData", 0, nSecondsStep);
    }

    void changeData()
    {
        if (!pause)
        {
            // days counter increment 
            GetComponent<Text>().text = "day " + nDay;

            // coins increment
            coinsRegister.coins += coinsRegister.stepCoins;
            textCoinsObject.GetComponent<Text>().text = Convert.ToString(coinsRegister.coins);

            nDay++;
        }
    }
}

