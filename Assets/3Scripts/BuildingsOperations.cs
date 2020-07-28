using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsOperations : MonoBehaviour
{
    public GameObject textTitle;
    public int NMaxBuildings = 5;

    // GOs for probeFactory
    public GameObject buttonPF;
    public GameObject textXPF;
    public GameObject textCostPF;
    public GameObject textProfitPF;
    public GameObject imageCompletedPF;
    public GameObject panelPF;

    public GameObject buttonHospital;
    //---
    public GameObject buttonMine;
    //---

    // GOs for spaceport
    public GameObject buttonSC;
    public GameObject textXSC;
    public GameObject textCostSC;
    public GameObject textProfitSC;
    public GameObject imageCompletedSC;
    public GameObject panelSC;

    private static GameObject textX;
    private static GameObject textCost;
    public static GameObject textProfit;
    private static GameObject buttonBuild;
    private static GameObject imageCompleted;
    private static GameObject panel;
    private static string Profit;

    public class Building
    {
        public int Cost;
        public int N;
    }

    public class buildingTime: Building
    {
        public int Time; 
    }

    public static buildingTime ProbeFactory = new buildingTime { Cost = 300, N = 0, Time = 80 };
    public static Building Hospital = new Building { Cost = 100, N = 0};
    public static Building Mine = new Building { Cost = 200, N = 0};
    public static buildingTime SCfactory = new buildingTime { Cost = 500, N = 0, Time = 100 };

    private static string ProfitPF = "     +1 per " + ProbeFactory.Time + " days\n  (now +0)";
    private static string ProfitSC = "     +1 per " + SCfactory.Time + " days\n  (now +0)";

    void Start()
    {
        // title
        textTitle.GetComponent<Text>().text = settings.sNameNativePlanet + "'s buildings";
        // ProbeFactory
        textXPF.GetComponent<Text>().text = "X" + ProbeFactory.N;
        textCostPF.GetComponent<Text>().text = "Cost:          " + ProbeFactory.Cost;
        textProfitPF.GetComponent<Text>().text = ProfitPF;
        // SCfactory
        textXSC.GetComponent<Text>().text = "X" + SCfactory.N;
        textCostSC.GetComponent<Text>().text = "Cost:          " + SCfactory.Cost;
        textProfitSC.GetComponent<Text>().text = ProfitSC;

        // to prevent building extra buildings
        if (ProbeFactory.N == NMaxBuildings)
        {
            Destroy(textCostPF);
            Destroy(buttonPF);
            GameObject instance = Instantiate(imageCompletedPF);
            instance.transform.SetParent(panelPF.transform, false);
        }
        if (SCfactory.N == NMaxBuildings)
        {
            Destroy(textCostSC);
            Destroy(buttonSC);
            GameObject instance = Instantiate(imageCompletedPF);
            instance.transform.SetParent(panelSC.transform, false);
        }
    }

    // button "Build" is pressed 
    public void BuildBuilding(int N)
    {
        //set all static gameobjects
        switch (N)
        {
            case 0:
                textX = textXPF;
                textCost = textCostPF;
                buttonBuild = buttonPF;
                textProfit = textProfitPF;
                imageCompleted = imageCompletedPF;
                panel = panelPF;
                buildBuilding(ProbeFactory);
                ProfitPF = textProfit.GetComponent<Text>().text;
                settings.sNProbes++;
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
                textX = textXSC;
                textCost = textCostSC;
                buttonBuild = buttonSC;
                textProfit = textProfitSC;
                imageCompleted = imageCompletedSC;
                panel = panelSC;
                buildBuilding(SCfactory);
                ProfitSC = textProfit.GetComponent<Text>().text;
                settings.sNSpacecraft++;
                break;
            default:
                break;
        }
    }

    // fulfill building
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
            textCost.GetComponent<Text>().text = "Cost:          " + building.Cost;
            
            // profit
            if (building is buildingTime)
            {
                UpdateProfit((buildingTime)building);
            }
            
            // to prevent building extra buildings
            if ( building.N == NMaxBuildings)
            {
                Destroy(buttonBuild);
                GameObject instance = Instantiate(imageCompletedPF);
                instance.transform.SetParent(panel.transform, false);
            }
        }
        else
        {
            // error
        }
    }

    // update and show profit
    private void UpdateProfit(buildingTime building)
    {
        // to prevent building extra buildings
        if (building.N < NMaxBuildings)
        {
            Profit = "     +1 per " + building.Time/2 + " days\n  (now " + building.Time + ")";
        }
        else
        {
            Profit = "     +1 per " + building.Time + " days\n";
            Destroy(textCost);
        }
        // update time
        building.Time /= 2;
        // show profit
        textProfit.GetComponent<Text>().text = Profit;
    }

    // "cross at canvas" is pressed
    public void ExitBuildings()
    {
        EarthOnClick.flagBuildings = false;
        settings.sCanvasBuildings.SetActive(false);
        settings.sPanelPeople.SetActive(true);
        settings.sPanelResources.SetActive(true);

        if (!settings.flagPauseBeforePrefab)
        {
            buttons.sPausePressed();
        }
    }

}
