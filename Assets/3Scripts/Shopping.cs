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
    private static GameObject sPanelPlanets;
    public GameObject TextTitle;
    private static GameObject sTextTitle;

    // name of the resource
    public GameObject textResource;
    private static GameObject sTextResource;
    // cost of resource to sell it
    public GameObject textCostResource;
    private static GameObject sTextCostResource;

    public GameObject TransportButton;
    private static GameObject sTransportButton;
    public GameObject BuyButton; 
    private static GameObject sBuyButton;
    public GameObject EtherButton;
    private static GameObject sEtherButton;
    public GameObject SellButton;
    private static GameObject sSellButton;
    public GameObject TextStorage;
    private static GameObject sTextStorage;

    // date
    public GameObject TextDate;
    // coins
    public GameObject TextCoins;
    // blueCoins 
    public GameObject BlueCoins;

    private static int numRes;
    private static int numButtonResource;
    private static int cost;

    private static panelInform PI;
    private static ShowProgress SP; 
    private static BlueCoin BC;

    public Canvas MainCanvas;
    public GameObject MessageBox;

    /// <summary>
    /// amount of the current resource on the selected planet
    /// </summary>
    public static int NRes = 0; // = GetNRes;
    private List<int> ListToFillEmpty = new List<int>();
    private static int numClick = 0;

    private static string strUnlimited = "Unlimited resource";
    private static string strNoSC = "No spacesraft";
    private static string strMaximum = "Maximum!";
    private static string strAtStorage = " at storage";
    private static string strNoResources = "No resources";
    private static string strResource = "Resource";
    private static string strPress = "Press on the title to choose resource";

 
    

    /// <summary>
    /// copy GOs to their static analoges and correct language
    /// </summary>
    void Start()
    {
        sPanelShopping = PanelShopping;
        sPanelPlanets = PanelPlanets;
        sTextTitle = TextTitle;

        sTransportButton = TransportButton;
        sBuyButton = BuyButton;
        sEtherButton = EtherButton;
        sSellButton = SellButton;

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
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            strUnlimited = "Unlimited resource";
            strNoSC = "No spacesraft";
            strMaximum = "Maximum!";
            strAtStorage = " is available";
            strNoResources = "No resources";
            strResource = "Resource";
            strPress = "Press on the title to choose resource";
        }
        else
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
            }
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
        if (numButtonResource < 0)
        { }

        SP = settingsResearches.sPanelInformation.GetComponent<ShowProgress>();
        PI = settingsResearches.sPanelInformation.GetComponent<panelInform>();
        BC = settingsResearches.sTextBC.GetComponent<BlueCoin>();

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
                sPanelPlanets.SetActive(false);
                sTextTitle.SetActive(false);
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
            // using Ether is impossible
            sEtherButton.SetActive(false);
        }

        // show PanelShopping and disable PanelPlanets
        sPanelShopping.SetActive(true);
        sPanelPlanets.SetActive(false);
        sTextTitle.SetActive(false);

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
            ItemOnClick.PP.ResNess_Amount[numButtonResource - 1] = 0;
        }

        // necessary (unlimited) or extraordinary resource
        bool flagTtravelWell = true;
        if (numButtonResource > 0)  
        { flagTtravelWell = settings.gameSettings.Storage.ContainsKey(numRes);  }

        if (flagTtravelWell == true)
        {
            // increase amount of the Resource at the selected planet
            AddResourceToPanet(++NRes);

            // move 1 resource from storage
            if (numButtonResource > 0) { PI.TakeAwayResourceFromStorage(numRes); }

            // show amount of spacecrafts
            settings.gameSettings.NSpasecraft--;
            settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NSpasecraft);
            if (settings.gameSettings.NSpasecraft == 0) { sTextStorage.GetComponent<Text>().text = strNoSC; }

            if (numButtonResource > 0)
            {       
                // show information
                ShowShopping();
            }

            // reset interactivity of buttons
            ResetButtons();

            // checking: if people may be transported
            if (numButtonResource < 0) { Check30(); }

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
            settingsResearches.sTextEth.GetComponent<Text>().text = System.Convert.ToString(--settings.gameSettings.NEther);
            if (settings.gameSettings.NEther == 0)  { sEtherButton.SetActive(false); }

            // after Ether using resource became transportable
            if (settings.gameSettings.NSpasecraft > 0)
            { sTransportButton.GetComponent<Button>().interactable = true; }

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

        BC.SubstractBlueCoins(1);
        settingsResearches.sTextBC.GetComponent<Text>().text = 
        System.Convert.ToString(BlueCoin.sNBlueCoin);

        // increase amount of the Resource at the selected planet
        AddResourceToPanet(++NRes);

        // checking: if people may be transported
        if (numButtonResource < 0) { Check30(); }

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

            // show MessageBox: people may be transported
            var instance = Instantiate(MessageBox);
            instance.transform.SetParent(MainCanvas.transform, false);
        }
    }

    /// <summary>
    /// show the name, the cost and amount of resource at the storage
    /// </summary>
    private void ShowShopping()
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
    
    private void GenerateListForEmptySlot()
    {
        ListToFillEmpty.Clear();

        // 1 - requested resources that aren't on the planet
        int N = getItems.ResourceAdd.Count;
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (settings.gameSettings.RequestedResources.ContainsKey(numRes) && !ItemOnClick.PP.ResAdd.Contains(numRes))
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
        else
        { ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]++; }

        // show changes on the panelInform
        PI.ResetPlanet(ItemOnClick.PP);

        // show changes on the requirement resources's panel  
        if (settings.gameSettings.RequestedResources.ContainsKey(numRes))
        {
            settings.gameSettings.RequestedResources[numRes]++;
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = 
                SP.Show(settings.gameSettings.RequestedResources);
        }

        // reset buttons
        if (NRes == 10)
        {
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            sEtherButton.SetActive(false);
        }
        else
        {
            if (NRes > 0)
            {
                sSellButton.GetComponent<Button>().interactable = true;
            }
        }
    }

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
            settings.gameSettings.RequestedResources[numRes]--; // it must be nonnegative
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = SP.Show(settings.gameSettings.RequestedResources);
        }

        // make all buttons active or inactive
        ResetShoppingPanel();
    }

    /// <summary>
    /// reset interactabilities of buttons
    /// </summary>
    private void ResetShoppingPanel()
    {
        if (NRes == 0) // dealing with an empty slot
        {
            // show name, introduction, cost
            sTextResource.GetComponent<Text>().text = strResource;
            sTextStorage.GetComponent<Text>().text = strPress;
            sTextCostResource.GetComponent<Text>().text = " 0";
            // make all buttons not interactable 
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            sSellButton.GetComponent<Button>().interactable = false;
            sEtherButton.SetActive(false);

            GenerateListForEmptySlot();
        }
        else
        {
            // TextStorage
            ResetStorageText();
            // make all buttons interactable or uninteractable 
            ResetButtons();
        }
    }

    private void ResetStorageText()
    {
        if (NRes == 10) { sTextStorage.GetComponent<Text>().text = strMaximum; }
        else
        {
            // extra resource
            if (numButtonResource > 0)
            {
                int amountInStorage = PI.GetAmountInStorage(numRes);
                sTextStorage.GetComponent<Text>().text = amountInStorage + strAtStorage;
            }
            // necessary resource
            else { sTextStorage.GetComponent<Text>().text = strUnlimited; }
        }
    }

    /// <summary>
    /// settings: may be buttons clicked or not 
    /// </summary>
    private void ResetButtons()
    {
        if (NRes == 10)
        {
            // transport
            sTransportButton.GetComponent<Button>().interactable = false;
            // ether
            sEtherButton.SetActive(false);
            // blue coins
            sBuyButton.GetComponent<Button>().interactable = false;
            // sell 
            sSellButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            // transport
            sTransportButton.GetComponent<Button>().interactable = false;
            if (settings.gameSettings.NSpasecraft > 0)
            {
                if (((numButtonResource > 0) && (PI.GetAmountInStorage(numRes) > 0))
                    || (numButtonResource < 0))
                { sTransportButton.GetComponent<Button>().interactable = true; }
            }
            // ether
            sEtherButton.SetActive(settings.gameSettings.NEther > 0);
            // blue coins
            sBuyButton.GetComponent<Button>().interactable = (BlueCoin.sNBlueCoin > 0);
            // sell 
            sSellButton.GetComponent<Button>().interactable = (NRes > 0);
        }
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
        sPanelPlanets.SetActive(true);
        sTextTitle.SetActive(true);
        sPanelShopping.SetActive(false);
    }

}
