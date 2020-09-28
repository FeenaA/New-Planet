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
    public GameObject textTitlePF;
    public GameObject textXPF;
    public GameObject textCostPF;
    public GameObject textProfitTitlePF;
    public GameObject textProfitPF;
    public GameObject panelPF;
     
    // GOs for Hospital
    public GameObject buttonHospital;
    public GameObject textTitleHospital;
    public GameObject textXHSP; 
    public GameObject textProfitTitleHSP; 
    public GameObject textCostHSP;
    public GameObject panelHSP;
    public GameObject textMortality;

    // GOs for Mine
    public GameObject buttonMine;
    public GameObject textTitleMine;
    public GameObject textXMN;
    public GameObject textCostMN;
    public GameObject textProfitTitleMN;
    public GameObject textProfitMN;
    public GameObject panelMN;

    // GOs for spaceport
    public GameObject buttonSC;
    public GameObject textTitleSC;
    public GameObject textXSC; 
    public GameObject textCostSC;
    public GameObject textProfitTitleSC;
    public GameObject textProfitSC;
    public GameObject panelSC;

    private static GameObject textX;
    private static GameObject textCost;
    public static GameObject textProfit;
    private static GameObject buttonBuild;
    private static GameObject panel;

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

    public class BuildingMine: Building 
    {
        public int stepCoins; 
    }

    private static BuildingTime ProbeFactory = new BuildingTime();
    private static BuildingHospital Hospital = new BuildingHospital();
    private static BuildingMine Mine = new BuildingMine();
    private static BuildingTime SCfactory = new BuildingTime();

    private static string Cost = ""; 
    private static string Per = "";
    private static string Days = "";
    private static string Day = "";
    private static string Now = "";
    private static string Maximum = "";
    private static string Treatment = ""; 
     
    void Start()
    {
        CorrectTextOnScene();

        ProbeFactory = settings.gameSettings.ProbeFactory;
        Hospital = settings.gameSettings.Hospital;
        Mine = settings.gameSettings.Mine;
        SCfactory = settings.gameSettings.SCfactory;

        string ProfitPF = UpdateProfitTime(ProbeFactory);
        string ProfitSC = UpdateProfitTime(SCfactory);
        string ProfitMN = UpdateProfitCoin(Mine);

        // ProbeFactory
        textXPF.GetComponent<Text>().text = "X" + ProbeFactory.N;
        textCostPF.GetComponent<Text>().text = Cost + ProbeFactory.Cost;
        textProfitPF.GetComponent<Text>().text = ProfitPF;
        // Hospital
        textXHSP.GetComponent<Text>().text = "X" + Hospital.N;
        textCostHSP.GetComponent<Text>().text = Cost + Hospital.Cost;
        // Mine
        textXMN.GetComponent<Text>().text = "X" + Mine.N;
        textCostMN.GetComponent<Text>().text = Cost + Mine.Cost;
        textProfitMN.GetComponent<Text>().text = ProfitMN;
        // SCfactory
        textXSC.GetComponent<Text>().text = "X" + SCfactory.N;
        textCostSC.GetComponent<Text>().text = Cost + SCfactory.Cost;
        textProfitSC.GetComponent<Text>().text = ProfitSC;

        // to prevent building too expensove buildings
        ReloadButtons();
        // to prevent building extra buildings
        CheckMax();
    }

    // button "Build" is pressed 
    public void BuildBuilding(int N)
    {
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
                settings.gameSettings.NProbe++;// settings.sNProbes++;
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
                textX = textXMN;
                textCost = textCostMN;
                buttonBuild = buttonMine;
                textProfit = textProfitMN;
                panel = panelMN;
                BuildBuilding(Mine);
                settings.gameSettings.stepCoins = Mine.stepCoins;
                break;
            case 3:
                // SpaceCraft
                textX = textXSC;
                textCost = textCostSC;
                buttonBuild = buttonSC;
                textProfit = textProfitSC;
                panel = panelSC;
                BuildBuilding(SCfactory);
                settings.gameSettings.NSpasecraft++;
                break;
            default:
                break;
        }
        ReloadButtons();
        LoadGame.SetBuildings();
    }

    // fulfill building
    private void BuildBuilding(Building building)
    {
        if (settings.gameSettings.NCoins > building.Cost)
        {
            // pay and show
            settings.gameSettings.NCoins -= building.Cost;
            DateChangeing.sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(settings.gameSettings.NCoins);

            // new cost and account
            building.Cost *= (++building.N + 1);
            // show 
            textX.GetComponent<Text>().text = "X" + building.N;
            textCost.GetComponent<Text>().text = Cost + building.Cost;

            // update and show profit
            if (building is BuildingTime) 
            { textProfit.GetComponent<Text>().text = CorrectProfitTime((BuildingTime)building); }
            else    if (building is BuildingHospital) { UpdateHospital((BuildingHospital)building); }
            else    if (building is BuildingMine)     
            { textProfit.GetComponent<Text>().text = CorrectProfitCoin((BuildingMine)building); }
            
            // to prevent building extra buildings
            if ( building.N == NMaxBuildings)
            {
                Destroy(buttonBuild);
                GameObject instance = Instantiate(imageCompleted);
                instance.GetComponentInChildren<Text>().text = Maximum;
                instance.transform.SetParent(panel.transform, false);
            }
        }
    }

    public void CheckMax()
    {
        // ProbeFactory
        if (ProbeFactory.N == NMaxBuildings)
        {
            Destroy(textCostPF);
            Destroy(buttonPF);
            GameObject instance = Instantiate(imageCompleted);
            instance.GetComponentInChildren<Text>().text = Maximum;
            instance.transform.SetParent(panelPF.transform, false);
        }
        // Hospital
        if (Hospital.N == NMaxBuildings)
        {
            Destroy(textCostHSP);
            Destroy(buttonHospital);
            GameObject instance = Instantiate(imageCompleted);
            textMortality.transform.GetComponent<Text>().text = Treatment;
            instance.GetComponentInChildren<Text>().text = Maximum;
            instance.transform.SetParent(panelHSP.transform, false);
        }
        // Mine
        if (Mine.N == NMaxBuildings)
        {
            Destroy(textCostMN);
            Destroy(buttonMine);
            GameObject instance = Instantiate(imageCompleted);
            instance.GetComponentInChildren<Text>().text = Maximum;
            instance.transform.SetParent(panelMN.transform, false);
        }
        // SpacePort
        if (SCfactory.N == NMaxBuildings)
        {
            Destroy(textCostSC);
            Destroy(buttonSC);
            GameObject instance = Instantiate(imageCompleted);
            instance.GetComponentInChildren<Text>().text = Maximum;
            instance.transform.SetParent(panelSC.transform, false);
        }
    }

    public void ReloadButtons()
    {
        int CurrentCoins = settings.gameSettings.NCoins;

        // ProbeFactory
        if (ProbeFactory.N == NMaxBuildings)
        {
            Destroy(textCostPF);
            Destroy(buttonPF);
        }
        else
        {
            if (ProbeFactory.Cost > CurrentCoins)
            { buttonPF.GetComponent<Button>().interactable = false; }
            else
            { buttonPF.GetComponent<Button>().interactable = true; }
        }
        // Hospital
        if (Hospital.N == NMaxBuildings)
        {
            Destroy(textCostHSP);
            Destroy(buttonHospital);
            textMortality.transform.GetComponent<Text>().text = Treatment;
        }
        else
        {
            if (Hospital.Cost > CurrentCoins)
            { buttonHospital.GetComponent<Button>().interactable = false; }
            else
            { buttonHospital.GetComponent<Button>().interactable = true; }
        }
        // Mine
        if (Mine.N == NMaxBuildings)
        {
            Destroy(textCostMN);
            Destroy(buttonMine);
        }
        else
        {
            if (Mine.Cost > CurrentCoins)
            { buttonMine.GetComponent<Button>().interactable = false; }
            else
            { buttonMine.GetComponent<Button>().interactable = true; }
        }
        // SpacePort
        if (SCfactory.N == NMaxBuildings)
        {
            Destroy(textCostSC);
            Destroy(buttonSC);
        }
        else
        {
            if (SCfactory.Cost > CurrentCoins)
            { buttonSC.GetComponent<Button>().interactable = false; }
            else
            { buttonSC.GetComponent<Button>().interactable = true; }
        }
    }

    // change profit of ProbeFactory and SpacePort
    private string CorrectProfitCoin(BuildingMine building)
    {
        // updated stepCoins
        building.stepCoins += 10 * (building.N);
        return UpdateProfitCoin(building);
    }

    private string UpdateProfitCoin(BuildingMine building)
    {
        string profit;
        if (building.N == 0)
        {
            profit = "     +" + (building.stepCoins + 10 * (building.N+1)) + Per + Day + Now + "+" + building.stepCoins + ")";
        }
        else
        {
            if (building.N == NMaxBuildings)
            {profit = "     +" + building.stepCoins + Per + Day; }
            else
            {profit = "     +" + (building.stepCoins + 10 * (building.N+1)) + Per + Day + Now + building.stepCoins + ")";}
        }
        return profit;
    }

    // change profit of ProbeFactory and SpacePort
    private string CorrectProfitTime(BuildingTime building)
    {        
        // updated time
        building.Time /= 2;
        return UpdateProfitTime(building);
    }
    
    // get profit of ProbeFactory and SpacePort
    private string UpdateProfitTime(BuildingTime building)
    {
        string profit;
        // update profit
        if (building.N == 0)
        {
            profit = "     +1" + Per + building.Time / 2 + Days + Now + "+0)";
        }
        else
        {
            if (building.N == NMaxBuildings)
            { profit = "     +1" + Per + building.Time / 2 + Days; }
            else { profit = "     +1" + Per + building.Time / 2 + Days + Now + building.Time + ")"; }
        }
        return profit;
    }

    // update data about people
    private void UpdateHospital(BuildingHospital building)
    {
        settings.gameSettings.koefToday = DateChangeing.koefPeopleStart;
        DateChangeing.DayDeth = 0;
        if (building.N == NMaxBuildings)
        {
            Destroy(textCost);
        }
    }

    private void CorrectTextOnScene()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            textTitle.GetComponent<Text>().text = settings.gameSettings.NameNative + "'s buildings";
            Cost = "Cost:             ";
            Per = " per ";
            Days = " days\n";
            Day = "day\n";
            Now = "  (now ";
            Maximum = "Max!";
            Treatment = "Under treatment";

            string strBuild = "Build";
            buttonPF.GetComponentInChildren<Text>().text = strBuild;
            buttonSC.GetComponentInChildren<Text>().text = strBuild;
            buttonHospital.GetComponentInChildren<Text>().text = strBuild;
            buttonMine.GetComponentInChildren<Text>().text = strBuild;

            string strProfit = "Profit:";
            textProfitTitlePF.transform.GetComponent<Text>().text = strProfit;
            textProfitTitleHSP.transform.GetComponent<Text>().text = strProfit;
            textProfitTitleMN.transform.GetComponent<Text>().text = strProfit;
            textProfitTitleSC.transform.GetComponent<Text>().text = strProfit;

            textTitlePF.transform.GetComponent<Text>().text = "Probe Factory";
            textTitleMine.transform.GetComponent<Text>().text = "Mine";
            textTitleHospital.transform.GetComponent<Text>().text = "Hospital";
            textMortality.transform.GetComponent<Text>().text = "Reduction of\nmortality";
            textTitleSC.transform.GetComponent<Text>().text = "Spaceport";
        }
        else
        {
            if (PersonalSettings.language == LanguageSettings.Language.Russian)
            {
                textTitle.GetComponent<Text>().text = "ЗДАНИЯ НА " + settings.gameSettings.NameNative;
                Cost = "ЦЕНА:            ";
                Per = " ЗА ";
                Days = " ДНЕЙ\n";
                Day = "ДЕНЬ\n";
                Now = " (СЕЙЧАС ";
                Maximum = "МАКСИМУМ!";
                Treatment = "ЛЕЧЕНИЕ ИДЕТ";

                string strBuild = "СТРОИТЬ";
                buttonPF.GetComponentInChildren<Text>().text = strBuild;
                buttonSC.GetComponentInChildren<Text>().text = strBuild;
                buttonHospital.GetComponentInChildren<Text>().text = strBuild;
                buttonMine.GetComponentInChildren<Text>().text = strBuild;

                string strProfit = "ВЫГОДА:";
                textProfitTitlePF.transform.GetComponent<Text>().text = strProfit;
                textProfitTitleHSP.transform.GetComponent<Text>().text = strProfit;
                textProfitTitleMN.transform.GetComponent<Text>().text = strProfit;
                textProfitTitleSC.transform.GetComponent<Text>().text = strProfit;

                textTitlePF.transform.GetComponent<Text>().text = "ФАБРИКА ЗОНДОВ";
                textTitleMine.transform.GetComponent<Text>().text = "РУДНИК";
                textTitleHospital.transform.GetComponent<Text>().text = "БОЛЬНИЦА";
                textMortality.transform.GetComponent<Text>().text = "СНИЖЕНИЕ\nСМЕРТНОСТИ";
                textTitleSC.transform.GetComponent<Text>().text = "КОСМОПОРТ";
            }
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
