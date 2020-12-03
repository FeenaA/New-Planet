using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{
    // make PanelPlanets inactive
    public GameObject PanelShopping; 
    public static GameObject sPanelShopping;
    public GameObject PanelPlanets;
    public GameObject TextTitle;

    // name of the resource
    public GameObject textResource;
    private static GameObject sTextResource;
    // cost of resource to sell it
    public GameObject textCostResource;
    private static GameObject sTextCostResource;
    // text for requested resources
    public Text TextRequestedResources;
    // panel with name, intro, resources
    public GameObject PanelInformation;

    public Button TransportButton;
    public Button BuyButton;
    public Button SellButton;
    public GameObject EtherButton;

    public GameObject TextStorage;
    private static GameObject sTextStorage;

    // date
    public GameObject TextDate;
    // coins
    public GameObject TextCoins;
    // blueCoins 
    public GameObject BlueCoins;

    // amount of units
    public Text TextSC;
    public Text TextBC;
    public Text TextEth;

    private static int numRes;
    public static int numButtonResource;
    private static int cost;

    // objects of another classes
    private panelInform PI;
    private ShowProgress SP; 
    private BlueCoin BC;

    public Transform MainCanvas;
    public GameObject MessageBox;
    public GameObject ImageCrawlLine;

    public GameObject MessageBoxAllRes;

    /// <summary>
    /// amount of the current resource on the selected planet
    /// </summary>
    public static int NRes = 0; // = GetNRes;
    private List<int> ListToFillEmpty = new List<int>();
    private static int numClick = 0;

    private string strUnlimited;
    private string strNoSC;
    private string strMaximum;
    private string strAtStorage;
    private string strNoResources;
    private string strResource;
    private string strPress;
    private string CrawlLineTransport;
    private string CrawlLineResource;
    private string CrawlLineSell;
    private string CrawlLineProfit;
    private string strAllResOnNew;

    /// <summary>
    /// copy GOs to their static analoges and correct language
    /// </summary>
    void Start()
    {
        sPanelShopping = PanelShopping;
        sTextStorage = TextStorage;
        sTextCostResource = textCostResource;
        sTextResource = textResource;

        CorrectLanguages();
    }

    /// <summary>
    /// fill all strings on the scene in correct language
    /// </summary>
    private void CorrectLanguages()
    {
        if ((PersonalSettings.language == LanguageSettings.Language.Russian))
        {
            strUnlimited = "БЕСКОНЕЧНЫЙ РЕСУРС";
            strNoSC = "НЕТ КОСМИЧЕСКОГО КОРАБЛЯ";
            strMaximum = "МАКСИМУМ!";
            strAtStorage = " ДОСТУПНО";
            strNoResources = "НЕТ РЕСУРСОВ";
            strResource = "РЕСУРС";
            strPress = "НАЖМИ НА ЗАГОЛОВОК, ЧТОБЫ ИЗМЕНИТЬ РЕСУРС";
            strAllResOnNew = "ПРЕВОСХОДНО! ТЫ СОБРАЛ ВСЕ РЕСУРСЫ НА НОВОЙ ПЛАНЕТЕ! ОСТАЛОСЬ ПЕРЕВЕЗТИ ЛЮДЕЙ.";

            // buttons on the Shopping panel
            TransportButton.GetComponentInChildren<Text>().text = "ПЕРЕВЕЗТИ";
            BuyButton.GetComponentInChildren<Text>().text = "КУПИТЬ";
            SellButton.GetComponentInChildren<Text>().text = "ПРОДАТЬ";

            // crawl line
            CrawlLineResource = "РЕСУРС ";
            CrawlLineTransport = " УСПЕШНО ТРАНСПОРТИРОВАН С ПЛАНЕТЫ ";
            CrawlLineSell = "ПРОДАЖА РЕСУРСА ";
            CrawlLineProfit = " ПРИНОСИТ ПРИБЫЛЬ В БЮДЖЕТ";
        }
        else
        {
            strUnlimited = "Unlimited resource";
            strNoSC = "No spacesraft";
            strMaximum = "Maximum!";
            strAtStorage = " is available";
            strNoResources = "No resources";
            strResource = "Resource";
            strPress = "Press on the title to choose resource";
            strAllResOnNew = "Congratulations! You've collected all resources! Focus on the transportation of people.";

            // buttons on the Shopping panel
            TransportButton.GetComponentInChildren<Text>().text = "Transport";
            BuyButton.GetComponentInChildren<Text>().text = "Buy";
            SellButton.GetComponentInChildren<Text>().text = "Sell";

            // crawl line
            CrawlLineResource = "The resource of ";
            CrawlLineTransport = " was successfully transported from the planet ";
            CrawlLineSell = "Selling of the resource ";
            CrawlLineProfit = " brings profit to the budget";
        }
    }

    /// <summary>
    /// choose a resource to buy/sell/transport it
    /// </summary>
    /// <param name="n"></param>
    public void ResourcePressed(int n)
    {
        // to operate with only SelectedPlanet
        if (ItemOnClick.PP.flagIsSelected == false) { return; }

        // not to open Shopping panel for ResNecess if the first part of people is on new planet
        if ((n < 0) && (settings.gameSettings.flagPeopleVeBeenSent)) 
        {
            // CrawlLine: "You can do nothing with necessary resources after the first group of people was sent to the new planet."
            CloseShopPressed();
            return; 
        }

        SP = PanelInformation.GetComponent<ShowProgress>();
        PI = PanelInformation.GetComponent<panelInform>();
        BC = TextBC.GetComponent<BlueCoin>();

        // get amount of the resource on the selected planet
        numButtonResource = n;
        NRes = GetNRes();
        numClick = 0;

        // extraordinary resource
        if (numButtonResource > 0)
        {
            // show name 
            numRes = ItemOnClick.PP.ResAdd[numButtonResource - 1];

            // dealing with an empty slot
            if (NRes == 0) 
            {
                ResetShoppingPanel();
                // show PanelShopping and disable PanelPlanets
                sPanelShopping.SetActive(true);
                //PanelPlanets.SetActive(false);
                //TextTitle.SetActive(false);
                return;
            }
            // not empty slot
            else 
            {
                ShowShopping();
            }
        }
        else // necessary resource
        {
            // show name 
            numRes = numButtonResource;
            textResource.GetComponent<Text>().text = getItems.ResNess[numButtonResource].name;
            // show cost
            cost = getItems.ResNess[numButtonResource].cost;
            textCostResource.GetComponent<Text>().text = System.Convert.ToString(cost);
            if (NRes == 10)
            { sTextStorage.GetComponent<Text>().text = strMaximum; }
            else 
            { sTextStorage.GetComponent<Text>().text = strUnlimited;}
        }

        // show PanelShopping and disable PanelPlanets
        sPanelShopping.SetActive(true);
        //PanelPlanets.SetActive(false);
        //TextTitle.SetActive(false);

        // settings: may be buttons clicked or not 
        ResetButtons();
    }

    /// <summary>
    /// transport resource to the selected planet using SpaseCraft
    /// </summary>
    public void TransportPressed()
    {
        // amount of Spacecrafts
        if (settings.gameSettings.NSpasecraft == 0) { return; }
                
        // the slot was empty
        if( numButtonResource > 0 &&  NRes == 0 )
        {
            // set number of resource for this slot and amount = 0
            ItemOnClick.PP.ResAdd[numButtonResource - 1] = numRes;
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1] = 0;
        }

        // necessary (unlimited) or extraordinary resource
        bool flagTtravelWell = true;
        if (numButtonResource > 0)  
        { flagTtravelWell = settings.gameSettings.Storage.ContainsKey(numRes);  }

        if (flagTtravelWell == true)
        {
            // increase amount of the Resource at the selected planet
            AddResourceToPanet(++NRes);

            #region crawl line 
            crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
            if (numButtonResource > 0) // extraordinary resource
            {
                // move 1 resource from storage (from NamePlanet)
                string NamePlanet = PI.TakeAwayResourceFromStorage(numRes);

                CL.ShowWithoutPause(CrawlLineResource + 
                    getItems.ResourceAdd[numRes].name + CrawlLineTransport + NamePlanet);     
            }
            else // necessary resource
            {
                CL.ShowWithoutPause(CrawlLineResource + 
                    getItems.ResNess[numButtonResource].name + CrawlLineTransport + settings.gameSettings.NameNative);
            }
            #endregion

            // show amount of spacecrafts
            settings.gameSettings.NSpasecraft--;
            TextSC.text = System.Convert.ToString(settings.gameSettings.NSpasecraft);
            if (settings.gameSettings.NSpasecraft == 0) { sTextStorage.GetComponent<Text>().text = strNoSC; }

            if (numButtonResource > 0)
            {       
                // show information
                ShowShopping();
            }

            // reset interactivity of buttons
            ResetButtons();

            // count CurrentNResUnits
            settings.gameSettings.CurrentNResUnits = CountNReqRes();

            // checking: if people may be transported
            if (numButtonResource < 0) { Check30(); }

            // checking: if all resources are on the new planet
            CheckRequestRes();

            // save the new data
            LoadGame.SetTransport();
            if (NRes == 10) { sTextStorage.GetComponent<Text>().text = strMaximum; }
        }
    }

    /// <summary>
    /// add resource to the storage using Ether
    /// </summary>
    public void EtherPressed()
    {
        // amount of Ether may be changed during this function 
        if (settings.gameSettings.NEther > 0)
        {
            int key = numRes;
            settingsResearches.AcceptRes PlanetAmount = new settingsResearches.AcceptRes
            {
                amount = 1,
                NamePlanet = "Ether"
            };
            // panelInform - add 1 resource to storage
            PI.Input(key, PlanetAmount);
            sTextStorage.GetComponent<Text>().text = PI.GetAmountInStorage(numRes) + strAtStorage;

            // amount of Ether
            TextEth.text = System.Convert.ToString(--settings.gameSettings.NEther);
            if (settings.gameSettings.NEther == 0)  { EtherButton.SetActive(false); }

            // after Ether using resource became transportable
            if (settings.gameSettings.NSpasecraft > 0)
            { TransportButton.interactable = true; }

            LoadGame.SetEther(true);
        }
    }

    /// <summary>
    /// sell resource from the selected planet 
    /// </summary>
    public void SellPressed()
    {
        // amount of the Resource at the selected planet
        if (NRes == 0) { return; }

        // change an amount of coins
        DateChangeing DC = TextDate.GetComponent<DateChangeing>();
        settings.gameSettings.NCoins = DC.AddCoins(cost);
        // show an amount of coins
        TextCoins.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NCoins);

        #region crawl line 
        crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();

        // extraordinary resource
        if (numButtonResource > 0) 
        { CL.ShowWithoutPause(CrawlLineSell + getItems.ResourceAdd[numRes].name + CrawlLineProfit); }
        else 
        // necessary resource
        { CL.ShowWithoutPause(CrawlLineSell + getItems.ResNess[numButtonResource].name + CrawlLineProfit); }
        #endregion

        // take away the sold resource
        TakeAwayResource();
        // save an information about the selected planet's resources
        LoadGame.SetSell();
    }

    /// <summary>
    /// transport resource to the selected planet using BlueCoin
    /// </summary>
    public void BuyPressed()
    {
        if (BlueCoin.sNBlueCoin == 0) { return; }

        // the slot was empty
        if (numButtonResource > 0 && NRes == 0)
        {
            // set number of resource for this slot and amount = 0
            ItemOnClick.PP.ResAdd[numButtonResource - 1] = numRes;
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1] = 0;
        }

        // BlueCoin decrement
        BC.SubstractBlueCoins(1);
        TextBC.text = 
        System.Convert.ToString(BlueCoin.sNBlueCoin);

        // increase amount of the Resource at the selected planet
        AddResourceToPanet(++NRes);

        // checking: if people may be transported
        if (numButtonResource < 0) { Check30(); }

        // checking: if all resources are on the new planet
        CheckRequestRes();

        // show information
        ShowShopping();
        // reset interactivity of buttons
        ResetButtons();

        // save an information about current selected planet's resources
        LoadGame.SetBuy();

        if (NRes == 10) { sTextStorage.GetComponent<Text>().text = strMaximum; }
    }

    /// <summary>
    /// dealing with an empty slot
    /// </summary>
    public void ChangeResourcePressed()
    {
        if (numButtonResource<0){ return; }
        if (NRes>0)             { return; }
        
        // only showing
        if (numClick < ListToFillEmpty.Count)
        {
            numRes = ListToFillEmpty[numClick];
            // show information
            ShowShopping();
            // reset interactivity of buttons
            ResetButtons();
            // numClick increment
            numClick++;
        }
        else { numClick = 0;}
    }

    /// <summary>
    /// checking - if all requested resources are on the new planet
    /// </summary>
    /// <returns></returns>
    private void CheckRequestRes()
    {
        int sum = 0;
        foreach (var reqRes in settings.gameSettings.RequestedResources)
        { sum += reqRes.Value; }
        if (sum == settings.gameSettings.RequestedResources.Count * 10)
        {
            // show Message
            var messageBox = Instantiate(MessageBoxAllRes);
            messageBox.SendMessage("TheStart", strAllResOnNew);
            // SetParent to the MessageBox
            messageBox.transform.SetParent(MainCanvas, false);
        }
    }

    /// <summary>
    /// show changes after getting new spacecraft 
    /// </summary>
    public void AddSC()
    {
        if ((PanelShopping.activeSelf))
        {
            ResetButtons();
            ResetStorageText();
        }
    }

    /// <summary>
    /// if (sumNRes == 30) goto people transportation
    /// </summary>
    private void Check30()
    {
        // summ of the necessary resources
        int sumNess = 0;
        for (int i = 0; i < 3; i++)
        { sumNess += ItemOnClick.PP.ResNess_Amount[i]; }
        if (sumNess == 30)
        {
            // set and save flag to make transportation of people avaliable
            settings.gameSettings.flagPeopleTransport = true;
            //LoadGame.SetfPeopleNSC();
             
            // flag to show message on the scene "Game"
            settings.flagShowMessageTransport = true;

            CloseShopPressed();

            // show MessageBox: people may be transported
            var instance = Instantiate(MessageBox);
            instance.transform.SetParent(MainCanvas, false);
        }
    }

    /// <summary>
    /// show the name, the cost and amount of resource at the storage
    /// </summary>
    private void ShowShopping()
    {        
        if (numRes < 0) { ShowShoppingNecessary(); } // vital
        else { ShowShoppingExtra(); } // extraordinary
    }

    /// <summary>
    /// show the name, the cost and amount of Necessary resource at the storage
    /// </summary>
    private void ShowShoppingNecessary()
    {
        if (numRes < 0)
        {
            // show name 
            numRes = numButtonResource;
            textResource.GetComponent<Text>().text = getItems.ResNess[numButtonResource].name;
            // show cost
            cost = getItems.ResNess[numButtonResource].cost;
            textCostResource.GetComponent<Text>().text = System.Convert.ToString(cost);
            // show sTextStorage
            if (NRes == 10)
            { sTextStorage.GetComponent<Text>().text = strMaximum; }
            else
            { sTextStorage.GetComponent<Text>().text = strUnlimited; }
        }
        else return;
    }

    /// <summary>
    /// show the name, the cost and amount of Extra resource at the storage
    /// </summary>
    private void ShowShoppingExtra()
    {
        //if (numRes >= 0)
        {
            // show the name of resource
            textResource.GetComponent<Text>().text = getItems.ResourceAdd[numRes].name;
            // show cost 
            cost = getItems.ResourceAdd[numRes].cost;
            textCostResource.GetComponent<Text>().text = System.Convert.ToString(cost);

            // show (or not) amount of resource in the storage
            int amountInStorage = PI.GetAmountInStorage(numRes);
            if (amountInStorage > 0)
            {
                if (NRes == 10)
                { sTextStorage.GetComponent<Text>().text = strMaximum; }
                else
                { sTextStorage.GetComponent<Text>().text = amountInStorage + strAtStorage; }
            }
            else { sTextStorage.GetComponent<Text>().text = strNoResources; }
        }
    }

    private void GenerateListForEmptySlot()
    {
        // list consists of resources, wich are prioritized
        ListToFillEmpty.Clear();

        // 1 - requested resources that aren't on the planet
        int N = getItems.ResourceAdd.Count;
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (settings.gameSettings.RequestedResources.ContainsKey(numRes) && 
                !ItemOnClick.PP.ResAdd.Contains(numRes))
            { ListToFillEmpty.Add(numRes); }
        }
        // 2 - resources that are avaliable to be transported
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (settings.gameSettings.Storage.ContainsKey(numRes) &&
                !ItemOnClick.PP.ResAdd.Contains(numRes) &&
                !ListToFillEmpty.Contains(numRes))
            { ListToFillEmpty.Add(numRes); }
        }
        // 3 - the last resources that aren't on the planet
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (!ItemOnClick.PP.ResAdd.Contains(numRes) && 
                !ListToFillEmpty.Contains(numRes))
            { ListToFillEmpty.Add(numRes); }
        }
    }

    /// <summary>
    /// increment amount of resource
    /// </summary>
    /// <param name="NRes"></param>
    private void AddResourceToPanet(int NRes)
    {
        // add resource
        if (numButtonResource > 0)
        { ItemOnClick.PP.ResAddAmount[numButtonResource - 1]++; }
        else // necessary resource
        { ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]++; }

        // show changes on the panelInform
        PI.ResetPlanet(ItemOnClick.PP);

        // show changes on the requirement resources's panel  
        if (settings.gameSettings.RequestedResources.ContainsKey(numRes))
        {
            settings.gameSettings.RequestedResources[numRes]++;
            TextRequestedResources.text = SP.Show(settings.gameSettings.RequestedResources);
        }

        // reset buttons
        if (NRes == 10)
        {
            TransportButton.interactable = false;
            BuyButton.interactable = false;
            EtherButton.SetActive(false);
        }
        else
        {
            if (NRes > 0)
            {
                SellButton.interactable = true;
            }
        }
    }

    /// <summary>
    /// take away 1 resource and show changes
    /// </summary>
    private void TakeAwayResource()
    {
        // resource decrement
        NRes--;

        if (numButtonResource > 0)
        {
            // take away the extraordinary resource
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1]--;
        }
        else
        {
            // take away the nesessary resource
            ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]--;
        }
        // show changes on the panelInform
        PI.ResetPlanet(ItemOnClick.PP);

        // show changes on the requirement resources's panel  
        if (settings.gameSettings.RequestedResources.ContainsKey(numRes))
        {
            settings.gameSettings.RequestedResources[numRes]--; // it is nonnegative
            TextRequestedResources.text = SP.Show(settings.gameSettings.RequestedResources);
        }

        // make all buttons active or inactive
        ResetShoppingPanel();
    }

    /// <summary>
    /// reset interactabilities of buttons
    /// </summary>
    private void ResetShoppingPanel()
    {
        if (NRes!=0 || NRes==0 && numButtonResource<0)
        {
            // TextStorage
            ResetStorageText();
            // make all buttons interactable or uninteractable 
            ResetButtons();
        }
        else // empty extraordinary resource
        {
            // show name, introduction, cost
            sTextResource.GetComponent<Text>().text = strResource;
            sTextStorage.GetComponent<Text>().text = strPress;
            sTextCostResource.GetComponent<Text>().text = " 0";
            // make all buttons not interactable 
            TransportButton.interactable = false;
            BuyButton.interactable = false;
            SellButton.interactable = false;
            EtherButton.SetActive(false);

            GenerateListForEmptySlot();
        }
    }

    /// <summary>
    /// reset "5 in storage"
    /// </summary>
    private void ResetStorageText()
    {
        if (NRes == 10) { sTextStorage.GetComponent<Text>().text = strMaximum; }
        else
        {
            // extra resource
            if (numButtonResource > 0)
            {
                int amountInStorage = PI.GetAmountInStorage(numRes);

                if (amountInStorage == 0)
                {
                    if (settings.gameSettings.NSpasecraft == 0)
                    { sTextStorage.GetComponent<Text>().text = strNoSC; }
                    else { sTextStorage.GetComponent<Text>().text = strNoResources; }
                }
                else { sTextStorage.GetComponent<Text>().text = amountInStorage + strAtStorage; }
            }
            // necessary resource -> unlimited
            else { sTextStorage.GetComponent<Text>().text = strUnlimited; }
        }
    }

    /// <summary>
    /// settings: may be buttons clicked or not 
    /// </summary>
    private void ResetButtons()
    {
        // Maximum
        if (NRes == 10)
        {
            // transport
            TransportButton.interactable = false;
            // ether
            EtherButton.SetActive(false);
            // blue coins
            BuyButton.interactable = false;
            // sell 
            SellButton.interactable = true;
        }
        else
        {
            // transport
            TransportButton.interactable = false;
            if (settings.gameSettings.NSpasecraft > 0)
            {
                if (((numButtonResource > 0) && (PI.GetAmountInStorage(numRes) > 0))
                    || (numButtonResource < 0))
                { TransportButton.interactable = true; }
            }
            // ether
            if ( numButtonResource < 0 ) { EtherButton.SetActive(false); } // necessary
            else { EtherButton.SetActive(settings.gameSettings.NEther > 0); } // extra
            // blue coins
            BuyButton.interactable = (BlueCoin.sNBlueCoin > 0);
            // sell 
            SellButton.interactable = (NRes > 0);
        }
    }

    /// <summary>
    /// count CurrentNResUnits
    /// </summary>
    private int CountNReqRes()
    {
        int res = 0;
        foreach (var item in settings.gameSettings.RequestedResources.Values)
        { res += item; }
        return res;
    }

    /// <summary>
    /// amount of the Resource at the selected planet
    /// </summary>
    private int GetNRes()
    {
        // nesessary resource
        if (numButtonResource < 0)
            return ItemOnClick.PP.ResNess_Amount[numButtonResource + 3];
        
        // extraordinary resource
        return ItemOnClick.PP.ResAddAmount[numButtonResource - 1];
    }

    // close the PanelShopping 
    public void CloseShopPressed()
    {
        //PanelPlanets.SetActive(true);
        //TextTitle.SetActive(true);
        sPanelShopping.SetActive(false);
    }

}
