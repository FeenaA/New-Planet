using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsOperations : MonoBehaviour
{
    public GameObject textTitle;
    public int NMaxBuildings = 5;

    public GameObject imageCompleted;

    // GOs for probeFactory
    public GameObject buttonPF;
    public GameObject textXPF;
    public GameObject textCostPF;
    public GameObject textProfitPF;
    public GameObject panelPF;

    // GOs for Hospital
    public GameObject buttonHospital;
    public GameObject textXHSP;
    public GameObject textCostHSP;
    public GameObject panelHSP; 

    public GameObject buttonMine;
    //---

    // GOs for spaceport
    public GameObject buttonSC;
    public GameObject textXSC;
    public GameObject textCostSC;
    public GameObject textProfitSC;
    public GameObject panelSC;

    private static GameObject textX;
    private static GameObject textCost;
    public static GameObject textProfit;
    private static GameObject buttonBuild;
    private static GameObject sImageCompleted;
    private static GameObject panel;
    private static string Profit;

    public class Building
    {
        public int Cost;
        public int N;
    }

    public class BuildingTime: Building
    {
        public int Time; 
    }

    public class BuildingHospital : Building {}

    public class BuildingMine: Building { }

    public static BuildingTime      ProbeFactory    = new BuildingTime { Cost = 300, N = 0, Time = 80 };
    public static BuildingHospital  Hospital        = new BuildingHospital { Cost = 100, N = 0};
    public static BuildingMine      Mine            = new BuildingMine { Cost = 200, N = 0};
    public static BuildingTime      SCfactory       = new BuildingTime { Cost = 500, N = 0, Time = 100 };

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
        // Hospital
        textXHSP.GetComponent<Text>().text = "X" + Hospital.N;
        textCostHSP.GetComponent<Text>().text = "Cost:          " + Hospital.Cost;
        // Mine
        // SCfactory
        textXSC.GetComponent<Text>().text = "X" + SCfactory.N;
        textCostSC.GetComponent<Text>().text = "Cost:          " + SCfactory.Cost;
        textProfitSC.GetComponent<Text>().text = ProfitSC;

        // to prevent building extra buildings or too expensove buildings
        ReloadButtons();
    }

    // button "Build" is pressed 
    public void BuildBuilding(int N)
    {
        sImageCompleted = imageCompleted;

        //set all static gameobjects
        switch (N)
        {
            case 0:
                // Probe Factory
                textX = textXPF;
                textCost = textCostPF;
                buttonBuild = buttonPF;
                textProfit = textProfitPF;
                panel = panelPF;
                BuildBuilding(ProbeFactory);
                ProfitPF = textProfit.GetComponent<Text>().text;
                settings.sNProbes++;
                break;
                
            case 1:
                // Hospital
                textX = textXHSP;
                textCost = textCostHSP;
                buttonBuild = buttonHospital;
                panel = panelHSP;
                BuildBuilding(Hospital);
                break;
            case 2:
                // Mine
                //textCost = textCostPF;
                 //textX = textXPF;               
                 //buildBuilding(ProbeFactory);
                break;
            case 3:
                // SpaceCraft
                textX = textXSC;
                textCost = textCostSC;
                buttonBuild = buttonSC;
                textProfit = textProfitSC;
                panel = panelSC;
                BuildBuilding(SCfactory);
                ProfitSC = textProfit.GetComponent<Text>().text;
                settings.sNSpacecraft++;
                break;
            default:
                break;
        }
        ReloadButtons();
    }

    // fulfill building
    private void BuildBuilding(Building building)
    {
        if (DateChangeing.sCoins > building.Cost)
        {
            // pay and show
            DateChangeing.sCoins -= building.Cost;
            DateChangeing.sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(DateChangeing.sCoins);

            // new cost and account
            building.Cost *= (++building.N + 1);
            // show 
            textX.GetComponent<Text>().text = "X" + building.N;
            textCost.GetComponent<Text>().text = "Cost:          " + building.Cost;

            // profit
                    if (building is BuildingTime)     { UpdateProfit((BuildingTime)building);       }
            else    if (building is BuildingHospital) { UpdateHospital((BuildingHospital)building); }
            else    if (building is BuildingMine)     { /*DateChangeing.stepCoins *= building.N;*/ }
            
            // to prevent building extra buildings
            if ( building.N == NMaxBuildings)
            {
                Destroy(buttonBuild);
                //GameObject instance = Instantiate(imageCompletedPF);
                GameObject instance = Instantiate(imageCompleted);
                instance.transform.SetParent(panel.transform, false);
            }
        }
    }

    public void ReloadButtons()
    {
        int CurrentCoins = DateChangeing.sCoins;

        // ProbeFactory
        if (ProbeFactory.N == NMaxBuildings)
        {
            Destroy(textCostPF);
            Destroy(buttonPF);
            GameObject instance = Instantiate(imageCompleted);
            instance.transform.SetParent(panelPF.transform, false);
        }
        else
        {
            if (ProbeFactory.Cost > CurrentCoins)
            { buttonPF.GetComponent<Button>().interactable = false; }
        }
        // Hospital
        if (Hospital.N == NMaxBuildings)
        {
            Destroy(textCostHSP);
            Destroy(buttonHospital);
            GameObject instance = Instantiate(imageCompleted);
            instance.transform.SetParent(panelHSP.transform, false);
        }
        else
        {
            if (Hospital.Cost > CurrentCoins)
            { buttonHospital.GetComponent<Button>().interactable = false; }
        }
        // Mine
        if (Mine.N == NMaxBuildings)
        {
            //Destroy(textCostMine);
            Destroy(buttonMine);
            GameObject instance = Instantiate(imageCompleted);
            //instance.transform.SetParent(panelMine.transform, false);
        }
        else
        {
            if (Hospital.Cost > CurrentCoins)
            { buttonHospital.GetComponent<Button>().interactable = false; }
        }
        // SpacePort
        if (SCfactory.N == NMaxBuildings)
        {
            Destroy(textCostSC);
            Destroy(buttonSC);
            GameObject instance = Instantiate(imageCompleted);
            instance.transform.SetParent(panelSC.transform, false);
        }
        else
        {
            if (SCfactory.Cost > CurrentCoins)
            { buttonSC.GetComponent<Button>().interactable = false; }
        }
    }

    // update and show profit
    private void UpdateProfit(BuildingTime building)
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

    // update data about people
    private void UpdateHospital(BuildingHospital building)
    {
        DateChangeing.koefToday = DateChangeing.koefPeopleStart;
        DateChangeing.DayDeth = 0;
        if (building.N == NMaxBuildings)
        {
            Destroy(textCost);
        }
    }

    // "cross at canvas" is pressed
    public void ExitBuildings()
    {
        EarthOnClick.flagBuildings = false;
        settings.sCanvasBuildings.SetActive(false);
        settings.sPanelPeople.SetActive(true);
        settings.sPanelResources.SetActive(true);
    }

}
