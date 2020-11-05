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
    
    // crawl line
    public GameObject ImageCrawlLine;
    // MessageBox - to suggest researching new planets
    public GameObject MessageBox; 
    private static bool flagStartResearch = false;
    public Transform MainCanvas;

    // coins - to show changes of the budjet
    public Text TextCoins;


    public GameObject panelHelp;
    public Text textHelp;

    // panels to become active after destroy gameobject
    public GameObject PanelPeople;
    public GameObject PanelResources;

    // GOs for probeFactory
    public GameObject buttonPF;
    public Text textTitlePF;
    public Text textXPF;
    public Text textCostPF;
    public GameObject textProfitTitlePF;
    public Text textProfitPF;
    public GameObject panelPF;
     
    // GOs for Hospital
    public GameObject buttonHospital;
    public Text textTitleHospital;
    public Text textXHSP; 
    public GameObject textProfitTitleHSP; 
    public Text textCostHSP;
    public GameObject panelHSP;
    public Text textMortality;

    // GOs for Mine
    public GameObject buttonMine;
    public Text textTitleMine;
    public Text textXMN;
    public Text textCostMN;
    public GameObject textProfitTitleMN;
    public Text textProfitMN;
    public GameObject panelMN;

    // GOs for spaceport
    public GameObject buttonSC;
    public Text textTitleSC;
    public Text textXSC; 
    public Text textCostSC;
    public GameObject textProfitTitleSC;
    public Text textProfitSC;
    public GameObject panelSC;

    // general gameobjects
    private Text textX;
    private Text textCost;
    public Text textProfit;
    private GameObject buttonBuild;
    private GameObject panel;

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

    private BuildingTime ProbeFactory = new BuildingTime();
    private BuildingHospital Hospital = new BuildingHospital();
    private BuildingMine Mine = new BuildingMine();
    private BuildingTime SCfactory = new BuildingTime();

    private string Cost = ""; 
    private string Per = "";
    private string Days = "";
    private string Day = "";
    private string Now = "";
    private string Maximum = "";
    private string Treatment = "";

    private string FirstPF = ""; 
    private string FirstHospital = "";
    private string FirstMine = "";
    private string FirstSC = "";

    private string NewPF = "";
    private string NewHospital = "";
    private string NewMine = "";
    private string NewSC = "";

    private string MaxPF = "";
    private string MaxHospital = "";
    private string MaxMine = "";
    private string MaxSC = "";

    private string strStartResearch = "";

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
        // to operate with CrawlLine
        crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
        bool flagBuild = false;

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
                flagBuild = BuildBuilding(ProbeFactory);
                settings.gameSettings.NProbe++;

                // deal with crawl line
                if (flagBuild)
                {
                    if (ProbeFactory.N == 1) CL.ShowWithoutPause(FirstPF);
                    else if (ProbeFactory.N == NMaxBuildings) CL.ShowWithoutPause(MaxPF);
                    else CL.ShowWithoutPause(NewPF);
                }
                break;
                
            case 1:
                // Hospital 
                textX = textXHSP;
                textCost = textCostHSP;
                buttonBuild = buttonHospital;
                panel = panelHSP;
                flagBuild = BuildBuilding(Hospital);

                // deal with crawl line
                if (flagBuild)
                {
                    if (Hospital.N == 1) { CL.ShowWithoutPause(FirstHospital); }
                    else if (Hospital.N == NMaxBuildings) CL.ShowWithoutPause(MaxHospital);
                    else { CL.ShowWithoutPause(NewHospital); }
                }
                break;
            case 2:
                // Mine
                textX = textXMN;
                textCost = textCostMN;
                buttonBuild = buttonMine;
                textProfit = textProfitMN;
                panel = panelMN;
                flagBuild = BuildBuilding(Mine);

                // deal with crawl line
                if (flagBuild)
                {
                    if (Mine.N == 1) { CL.ShowWithoutPause(FirstMine); }
                    else if (Mine.N == NMaxBuildings) CL.ShowWithoutPause(MaxMine);
                    else { CL.ShowWithoutPause(NewMine); }
                }
                settings.gameSettings.stepCoins = Mine.stepCoins;
                break;
            case 3:
                // SpaceCraft
                textX = textXSC;
                textCost = textCostSC;
                buttonBuild = buttonSC;
                textProfit = textProfitSC;
                panel = panelSC;
                flagBuild = BuildBuilding(SCfactory);

                // deal with crawl line
                if (flagBuild)
                {
                    if (SCfactory.N == 1) { CL.ShowWithoutPause(FirstSC); }
                    else if (SCfactory.N == NMaxBuildings) CL.ShowWithoutPause(MaxSC);
                    else { CL.ShowWithoutPause(NewSC); }
                }
                settings.gameSettings.NSpasecraft++;
                break;
            default:
                break;
        }

        ReloadButtons();
        LoadGame.SetBuildings();
    }

    /// <summary>
    /// fulfill building
    /// </summary>
    /// <param name="building">updated buildings</param>
    private bool BuildBuilding(Building building)
    {
        // there isn't enough money to build the building
        if (settings.gameSettings.NCoins < building.Cost)
        { return false; }

        // pay and show
        settings.gameSettings.NCoins -= building.Cost;
        TextCoins.text = Convert.ToString(settings.gameSettings.NCoins);

        // new cost and account
        building.Cost *= (++building.N + 1);
        // show 
        textX.text = "X" + building.N;
        textCost.text = Cost + building.Cost;

        // update and show profit
        if (building is BuildingTime) 
        { textProfit.text = CorrectProfitTime((BuildingTime)building); }
        else    if (building is BuildingHospital) 
        { UpdateHospital((BuildingHospital)building); }
        else    if (building is BuildingMine)     
        { textProfit.text = CorrectProfitCoin((BuildingMine)building); }
            
        // to prevent building extra buildings
        if ( building.N == NMaxBuildings)
        {
            Destroy(buttonBuild);
            GameObject instance = Instantiate(imageCompleted);
            instance.GetComponentInChildren<Text>().text = Maximum;
            instance.transform.SetParent(panel.transform, false);
        }

        // the biulding is built
        return true;
    }

    private void CheckMax()
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
            textMortality.text = Treatment;
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
            textMortality.text = Treatment;
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

            FirstPF = "ИССЛЕДОВАТЬ ПЛАНЕТЫ ЛЕГКО! ПЕРВАЯ ФАБРИКА ЗОНДОВ ГОТОВА К РАБОТЕ";
            FirstHospital = "ПЕРВЫЙ ПРОФИЛЬНЫЙ МЕДИЦИНСКИЙ ЦЕНТР ПРИСТУПИЛ К БОРЬБЕ С ВИРУСОМ";
            FirstMine = "ЗАПУЩЕН РУДНИК, ПРИНОСЯЩИЙ ЕЖЕДНЕВНУЮ ПРИБЫЛЬ";
            FirstSC = "ПЕРЕВОЗИТЬ РЕСУРСЫ И ЛЮДЕЙ ТЕПЕРЬ ВОЗМОЖНО! ПОСТРОЕН ПЕРВЫЙ КОСМОПОРТ";

            NewPF = "НОВАЯ ФАБРИКА ЗОНДОВ ПОЗВОЛЯЕТ ЧАЩЕ ИССЛЕДОВАТЬ НОВЫЕ ПЛАНЕТЫ";
            NewHospital = "НОВАЯ БОЛЬНИЦА БОРЕТСЯ С РАСПРОСТРАНЕНИЕМ ВИРУСА";
            NewMine = "НОВЫЙ РУДНИК УВЕЛИЧИЛ ЕЖЕДНЕВНУЮ ПРИБЫЛЬ";
            NewSC = "НОВЫЙ КОСМОПОРТ ПОЗВОЛЯЕТ ЧАЩЕ ПЕРЕВОЗИТЬ РЕСУРСЫ И ЛЮДЕЙ НА НОВУЮ ПЛАНЕТУ";

            MaxPF = "СТРОИТЕЛЬСТВО ФАБРИК ЗОНДОВ ВЫШЛО НА МАКСИМАЛЬНЫЙ УРОВЕНЬ";
            MaxHospital = "МЕДИЦИНА ДОСТИГЛА ПРЕДЕЛА СВОИХ ВОЗМОЖНОСТЕЙ";
            MaxMine = "СУЩЕСТВУЮЩИЕ РУДНИКИ ЗАНЯЛИ ВСЕ ИЗВЕСТНЫЕ МЕСТОРОЖДЕНИЯ"; 
            MaxSC = "ПРОИЗВОДСТВО КОСМИЧЕСКИХ КОРАБЛЕЙ ВЫШЛО НА МАКСИМАЛЬНЫЙ УРОВЕНЬ";

            strStartResearch = "ПОРА НАЧИНАТЬ ПОИСК НОВОЙ ПЛАНЕТЫ. ДЛЯ ЭТОГО НАЖМИ КНОПКУ \"ПОИСК\"";

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

            textTitlePF.text = "ФАБРИКА ЗОНДОВ";
            textTitleMine.text = "РУДНИК";
            textTitleHospital.text = "БОЛЬНИЦА";
            textMortality.text = "СНИЖЕНИЕ\nСМЕРТНОСТИ";
            textTitleSC.text = "КОСМОПОРТ";
        }
        else
        {
            textTitle.GetComponent<Text>().text = settings.gameSettings.NameNative + "'s buildings";
            Cost = "Cost:             ";
            Per = " per ";
            Days = " days\n";
            Day = "day\n";
            Now = "  (now ";
            Maximum = "Max!";
            Treatment = "Under treatment";

            FirstPF = "Planets researching is easy! The first probe factory ready to operate";
            FirstHospital = "The first specialized medical center began to fight the virus";
            FirstMine = "The mine is lanched to receive profit daily";
            FirstSC = "It is possible to transport resources and people! The spaceport is built";

            NewPF = "New probe factory allows to research more new planets";
            NewHospital = "New hospital fighting the spread of the virus";
            NewMine = "New mine has increased daily profit";
            NewSC = "New spaceport allows to transport resources and people more often";

            MaxPF = "Probe factory construction reaches maximum level";
            MaxHospital = "МЕДИЦИНА ДОСТИГЛА ПРЕДЕЛА СВОИХ ВОЗМОЖНОСТЕЙ";
            MaxMine = "Existing mines took all known deposits";
            MaxSC = "Spacecraft production reaches maximum level";

            strStartResearch = "It's time to start searching for a new planet. For this press the button \"RESEARCH\"";

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

            textTitlePF.text = "Probe Factory"; 
            textTitleMine.text = "Mine";
            textTitleHospital.text = "Hospital";
            textMortality.text = "Reduction of\nmortality";
            textTitleSC.text = "Spaceport";
        }
    }

    // "help" is pressed
    public void ShowHelp()
    {
        //to block showing any new messages while the messagebox exists
        crawlLine.BlockCrawlLine = true;
        CorrectHelpText();
        panelHelp.SetActive(true);
    }

    private void CorrectHelpText()
    {
        if (PersonalSettings.language == LanguageSettings.Language.Russian)
        {
            textHelp.text = "ЗОНД НЕОБХОДИМ, ЧТОБЫ ИЗУЧИТЬ НОВУЮ ПЛАНЕТУ\n" +
                "БОЛЬНИЦЫ СНИЖАЮТ УРОВЕНЬ СМЕРТНОСТИ ВИРУСА\n" +
                "РУДНИКИ ДАЮТ ПРИБЫЛЬ\n" +
                "КОСМОЛЁТЫ ПЕРЕВОЗЯТ РЕСУРСЫ И ЛЮДЕЙ НА НОВУЮ ПЛАНЕТУ ";
        }
        else
        {
            textHelp.text = "Probe is necessary to research a new planet\n" +
                "Hospital reduces mortality\n" +
                "Mine produses coins\n" +
                "Spacecraft transports resources and people";
        }
    }
     
    public void ExitHelp()
    {
        panelHelp.SetActive(false);
    }

    // "cross at canvas" is pressed
    public void ExitBuildings()
    {
        EarthOnClick.flagBuildings = false;
        crawlLine.BlockCrawlLine = false;
        gameObject.SetActive(false);
        PanelPeople.SetActive(true);
        PanelResources.SetActive(true);

        #region MessageBox: "start researching planets"
        if (!flagStartResearch && !settings.gameSettings.flagSelectedPlanet)
        {
            // show Message
            GameObject messageBox = Instantiate(MessageBox);
            messageBox.SendMessage("TheStart", strStartResearch);
            // SetParent to the MessageBox
            messageBox.transform.SetParent(MainCanvas, false);

            flagStartResearch = true;
        }
        #endregion
    }
}
