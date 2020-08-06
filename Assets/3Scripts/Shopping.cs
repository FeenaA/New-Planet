using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{
    // make PanelPlanets inactive
    public GameObject PanelShopping;
    private static GameObject sPanelShopping;
    public GameObject PanelPlanets;
    private static GameObject sPanelPlanets;
    public GameObject TextTitle;
    private static GameObject sTextTitle;

    // name of the resource
    public GameObject textResource;
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

    private static int numRes;
    private static int numButtonResource;
    private static int cost;

    private static panelInform PI;
    private static ShowProgress SP;

    private static int NRes = 0; // = GetNRes;
    private List<int> ListToFillEmpty = new List<int>();
    private static int numClick = 0;

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
    }

    // choose a resource to buy/sell/transport it
    public void ResourcePressed(int n)
    {
        SP = settingsResearches.sPanelInformation.GetComponent<ShowProgress>();
        PI = settingsResearches.sPanelInformation.GetComponent<panelInform>();

        // get amount of the resource on the selected planet
        numButtonResource = n;
        NRes = GetNRes();
        numClick = 0;

        // to operate with only SelectedPlanet
        if (ItemOnClick.PP.flagIsSelected == false)
        { return; }

        if (numButtonResource > 0)// extraordinary resource
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
        else // nesessary resource
        {
            // show name 
            numRes = numButtonResource;
            textResource.GetComponent<Text>().text = getItems.ResNess[numButtonResource].name;
            // show cost
            cost = getItems.ResNess[numButtonResource].cost;
            textCostResource.GetComponent<Text>().text = System.Convert.ToString(cost);
            // show amount of resource in the storage
            sTextStorage.GetComponent<Text>().text = "Unlimited resource";
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

    // transport resource to the selected planet using SpaseCraft
    public void TransportPressed()
    {
        // amount of Spacecrafts
        if (settings.sNSpacecraft == 0) { return; }
                
        // the slot was empty
        if( numButtonResource > 0 &&  NRes == 0 )
        {
            // set number of resource for this slot and amount = 0
            ItemOnClick.PP.ResAdd[numButtonResource - 1] = numRes;
            ItemOnClick.PP.ResNess_Amount[numButtonResource - 1] = 0;
        }

        bool flagTtravelWell = true;
        // necessary (unlimited) or extraordinary resource
        if (numButtonResource > 0)  
        { flagTtravelWell = settingsResearches.Storage.ContainsKey(numRes);  }

        if (flagTtravelWell == true)
        {
            // increase amount of the Resource at the selected planet
            AddResourceToPanet( ++NRes );

            // move 1 resource from storage
            if (numButtonResource > 0) { PI.TakeAwayResourceFromStorage(numRes); }

            // show amount of spacecrafts
            settings.sNSpacecraft--;
            settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);
            if (settings.sNSpacecraft == 0){sTextStorage.GetComponent<Text>().text = "No spacesraft";}

            // show information
            ShowShopping();
            // reset interactivity of buttons
            ResetButtons();
        }

        if (NRes == 10) { sTextStorage.GetComponent<Text>().text = "That's enough!"; return; }
    }

    // add resource to the storage using Ether
    public void EtherPressed()
    {
        // amount of the Resource at the selected planet
        if (GetNRes() < 10)
        {
            // amount of Ether may be changed during this function 
            if (settings.sNEther > 0)
            {
                int key = numRes;
                settingsResearches.AcceptRes PlanetAmount = new settingsResearches.AcceptRes
                {
                    amount = 1,
                    NamePlanet = "Ether"
                };
                // panelInform - add 1 resource to storage
                PI.Input(key, PlanetAmount);
                sTextStorage.GetComponent<Text>().text = PI.GetAmountInStorage(numRes) + " at storage";

                // amount of Ether
                settingsResearches.sTextEth.GetComponent<Text>().text = System.Convert.ToString(--settings.sNEther);
                if (settings.sNEther == 0)  { sEtherButton.SetActive(false); }

                // after Ether using resource became transportable
                sTransportButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    // sell resource from the selected planet
    public void SellPressed()
    {
        // amount of the Resource at the selected planet
        if (NRes == 0) { return; }

        // change an amount of coins
        DateChangeing DC = TextDate.GetComponent<DateChangeing>();
        DateChangeing.sCoins = DC.AddCoins(cost);
        // show an amount of coins
        TextCoins.GetComponent<Text>().text = System.Convert.ToString(DateChangeing.sCoins);

        // take away the sold resource
        NRes--;
        TakeAwayResource();
    }

    // transport resource to the selected planet using BlueCoin
    public void BuyPressed()
    {
        if (settings.sNBlueCoin > 0)
        {
            settings.sNBlueCoin--;
            settingsResearches.sTextBC.GetComponent<Text>().text = System.Convert.ToString(settings.sNBlueCoin);

            // increase amount of the Resource at the selected planet
            AddResourceToPanet(++NRes);
        }
        else
        {
            // suppose to watch a short advert to get 1 BlueCoin

        }

        // show information
        ShowShopping();
        // reset interactivity of buttons
        ResetButtons();

        if (NRes == 10) { sTextStorage.GetComponent<Text>().text = "That's enough!"; return; }
    }

    // dealing with an empty slot
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

    // show the name, the cost and amount of resource at the storage
    private void ShowShopping()
    {
        // show the name of resource
        textResource.GetComponent<Text>().text = getItems.ResourceAdd[numRes].name;
        // show cost 
        cost = getItems.ResourceAdd[numRes].cost;
        textCostResource.GetComponent<Text>().text = System.Convert.ToString(cost);
        // show (or not) EtherButton
        //if (settings.sNEther > 0) { sEtherButton.SetActive(true); }

        // show (or not) amount of resource in the storage
        int amountInStorage = PI.GetAmountInStorage(numRes);
        if (amountInStorage > 0)
        {
            sTextStorage.GetComponent<Text>().text = amountInStorage + " at storage";
        }
        else { sTextStorage.GetComponent<Text>().text = "No resources"; }
    }
    

    private void GenerateListForEmptySlot()
    {
        ListToFillEmpty.Clear();

        // 1 - requested resources that aren't on the planet
        int N = getItems.ResourceAdd.Count;
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (settings.reqRes.ContainsKey(numRes) && !ItemOnClick.PP.ResAdd.Contains(numRes))
            { ListToFillEmpty.Add(numRes); }
        }
        // 2 - resources that are avaliable to be transported
        for (int numRes = 1; numRes <= N; numRes++)
        {
            if (settingsResearches.Storage.ContainsKey(numRes) &&
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
        if (settings.reqRes.ContainsKey(numRes))
        {
            settings.reqRes[numRes]++;
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = SP.Show(settings.reqRes);
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
        if (numButtonResource > 0)
        {
            // take away extraordinary resource
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1]--;
        }
        else
        {
            // add nesessary resource
            ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]--;
        }
        // show changes on the panelInform
        PI.ResetPlanet(ItemOnClick.PP);

        // show changes on the requirement resources's panel  
        if (settings.reqRes.ContainsKey(numRes))
        {
            settings.reqRes[numRes]--; // it must be nonnegative
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = SP.Show(settings.reqRes);
        }

        ResetShoppingPanel();
    }

    // reset interactabilities of buttons
    private void ResetShoppingPanel()
    {
        if (NRes == 0) // dealing with an empty slot
        {
            // show name, introduction, cost
            textResource.GetComponent<Text>().text = "Resource";
            sTextStorage.GetComponent<Text>().text = "Press on the title to choose resource";
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
            ResetButtons();
        }
    }

    // settings: may be buttons clicked or not 
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
            if (settings.sNSpacecraft > 0)
            {
                if (((numButtonResource > 0) && (PI.GetAmountInStorage(numRes) > 0))
                    || (numButtonResource < 0))
                { sTransportButton.GetComponent<Button>().interactable = true; }
            }
            // ether
            sEtherButton.SetActive(settings.sNEther > 0);
            // blue coins
            sBuyButton.GetComponent<Button>().interactable = (settings.sNBlueCoin > 0);
            // sell 
            sSellButton.GetComponent<Button>().interactable = (NRes > 0);
        }
    }

    // amount of the Resource at the selected planet
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
