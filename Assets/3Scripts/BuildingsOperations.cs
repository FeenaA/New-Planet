using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsOperations : MonoBehaviour
{
    public GameObject textTitle;

    public int NMaxBuildings = 5;

    public GameObject textXPF;
    public GameObject textCostPF;
    public GameObject textProfitPF;
    public GameObject buttonPF;

    public GameObject buttonHospital;
    public GameObject buttonMine;
    public GameObject buttonSC;

    public static bool flagPF = false;

    private static GameObject textX;
    private static GameObject textCost;
    public static GameObject textProfit;
    private static GameObject buttonBuild;

    public class Building
    {
        public int Cost;
        public int N;
        
    }

    public class probeFactory: Building
    {
        public int Time; 
    }

    private static int initialTime = 80;
    private int followingTime = initialTime;

    public static probeFactory ProbeFactory = new probeFactory { Cost = 300, N = 0, Time = initialTime };
    public static Building Hospital = new Building { Cost = 100, N = 0};
    public static Building Mine = new Building { Cost = 200, N = 0};
    public static Building SCfactory = new Building { Cost = 1000, N = 0};

    // Start is called before the first frame update
    void Start()
    {
        textTitle.GetComponent<Text>().text = settings.sNameNativePlanet + "'s buildings";

        textXPF.GetComponent<Text>().text = "X" + ProbeFactory.N;
        textCostPF.GetComponent<Text>().text = "Cost:   " + ProbeFactory.Cost;
        textProfitPF.GetComponent<Text>().text = "  +1 per " + initialTime + " days\n  ";

    }

    public void BuildBuilding(int N)
    {
        switch (N)
        {
            case 0:
                textX = textXPF;
                textCost = textCostPF;
                buttonBuild = buttonPF;
                textProfit = textProfitPF;
                buildBuilding(ProbeFactory);
                break;
            case 1:

                //buildBuilding(ProbeFactory);
                //textX = textXPF;
                //textCost = textCostPF;
                break;
            case 2:
                //textCost = textCostPF;
                 //textX = textXPF;               
                 //buildBuilding(ProbeFactory);
                break;
            case 3:
                //textX = textXPF;
                //textCost = textCostPF;
                //buildBuilding(ProbeFactory);
                break;
            default:
                break;
        }
    }

    private void buildBuilding(Building building)
    {
        if (DateChangeing.sCoins > building.Cost)
        {
            // pay
            DateChangeing.sCoins -= building.Cost;
            // show
            DateChangeing.sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(DateChangeing.sCoins);

            // new cost and account
            building.Cost *= (++building.N + 1);
            // show new data
            textX.GetComponent<Text>().text = "X" + building.N;
            textCost.GetComponent<Text>().text = "Cost:   " + building.Cost;
            
            // profit
            if (building is probeFactory)
            {
                UpdatePF((probeFactory)building);
            }
            
            // to prevent building extra buildings
            if ( building.N == NMaxBuildings)
            {
               buttonBuild.SetActive(false);
            }
        }
        else
        {
            // error
        }
    }

    // generate and show profit
    private void UpdatePF(probeFactory building)
    {
        building.Time = followingTime;
        followingTime = building.Time / 2;
        settings.sNProbes++;
        textProfitPF.GetComponent<Text>().text = "  +1 per " + followingTime + " days\n  (now "+ building.Time + ")";
    }

    // "cross at canvas" is pressed
    public void ExitBuildings()
    {
        EarthOnClick.flagBuildings = false;
        settings.sCanvasBuildings.SetActive(false);
        if (!settings.flagPauseBeforePrefab)
        {
            buttons.sPausePressed();
        }
    }

}
