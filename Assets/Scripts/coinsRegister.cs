using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // это важно

public class coinsRegister : MonoBehaviour
{
    // account of coins in the beginning
    public int startCoins;
    // account of everyday coins 
    public int startStepCoins;

    public static int coins;
    public static int stepCoins;

    void Start()
    {
        // initial conditions for coins
        coins = startCoins;
        stepCoins = startStepCoins;
        GetComponent<Text>().text =  Convert.ToString(coins);
    }

    /*
    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "555";
    }*/
    /* public class DateGUI_script : MonoBehaviour
     {
         public GameObject guiTextLink; //Ссылка на объект, если скрипт не на нем
         void Start()
         {
             guiTextLink.GetComponent<Text>().text = "42";
         }
     }*/
}
