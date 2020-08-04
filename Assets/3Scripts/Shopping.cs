using System.Collections;
using System.Collections.Generic;
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
    }

    // choose a resource to buy/sell/transport it
    public void ResourcePressed(int n)
    {
        SP = settingsResearches.sPanelInformation.GetComponent<ShowProgress>();
        PI = settingsResearches.sPanelInformation.GetComponent<panelInform>();

        // to operate with only SelectedPlanet
        if (ItemOnClick.PP.flagIsSelected == false)
        { return; }

        numButtonResource = n;

        // flag - if the resource exists in the storage
        bool flagTtravelWell = false;

        if (numButtonResource > 0)// extraordinary resource
        {
            // show name 
            numRes = ItemOnClick.PP.ResAdd[numButtonResource - 1];
            textResource.GetComponent<Text>().text = getItems.ResourceAdd[numRes].name;
            // show cost 
            cost = getItems.ResourceAdd[numRes].cost;
            textCostResource.GetComponent<Text>().text = System.Convert.ToString( cost );
            // show (or not) EtherButton
            if (settings.sNEther>0) {sEtherButton.SetActive(true);}

            // show (or not) amount of resource in the storage
            int amountInStorage = PI.GetAmountInStorage(numRes);
            if (amountInStorage > 0)    
            {
                flagTtravelWell = true;
                sTextStorage.GetComponent<Text>().text = amountInStorage + " at storage";
            }
            else { sTextStorage.GetComponent<Text>().text = "No resources"; }
        }
        else// nesessary resource
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
            flagTtravelWell = true;
        }

        // show PanelShopping and disable PanelPlanets
        sPanelShopping.SetActive(true);
        sPanelPlanets.SetActive(false);
        sTextTitle.SetActive(false);

        // get amount of the resource on the selected planet
        int AmountOfRes = GetNRes();

        // settings: may be buttons clicked or not 
        sTransportButton.GetComponent<Button>().interactable = flagTtravelWell;
        sBuyButton.GetComponent<Button>().interactable = true;
        sSellButton.GetComponent<Button>().interactable = true;
        if (AmountOfRes == 10)
        {
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            sEtherButton.SetActive(false);
        }
        else
        {
            if (AmountOfRes == 0) {sSellButton.GetComponent<Button>().interactable = false;}

            if (settings.sNSpacecraft == 0) 
            {
                sTransportButton.GetComponent<Button>().interactable = false;
                sTextStorage.GetComponent<Text>().text = "No spacesraft";
            }
            if (settings.sNEther == 0)      {sEtherButton.SetActive(false);}
            if (settings.sNBlueCoin == 0)   {sBuyButton.GetComponent<Button>().interactable = false;}
        }
    }

    // transport resource to the selected planet using SpaseCraft
    public void TransportPressed()
    {
        // amount of the Resource at the selected planet
        int NRes = GetNRes();
        // amount of resource may be changed during this function 
        if (NRes < 10)
        {
            // amount of Spacecrafts may be changed during this function 
            if (settings.sNSpacecraft > 0)
            {
                bool flagTtravelWell = false;
                // necessary (unlimited) or extraordinary resource
                if (numButtonResource > 0)  
                { flagTtravelWell = settingsResearches.Storage.ContainsKey(numRes);  }
                else                
                { flagTtravelWell = true; }

                if (flagTtravelWell == true)
                {
                    AddResourceToPanet( ++NRes );

                    // show amount of spacecrafts
                    settings.sNSpacecraft--;
                    if (settings.sNSpacecraft == 0)
                    {
                        TransportButton.GetComponent<Button>().interactable = false;
                        sTextStorage.GetComponent<Text>().text = "No spacesraft";
                    }
                    settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

                    // checking: if no more this resource at the Storage (requirement for transportation)
                    if (numButtonResource > 0)  
                    {
                        int amountInStorage = PI.GetAmountInStorage(numRes);
                        if (amountInStorage == 0)
                        {   
                            TransportButton.GetComponent<Button>().interactable = false;
                            sTextStorage.GetComponent<Text>().text = "No resources";//"resource is off"; 
                        }
                        else
                        {
                            sTextStorage.GetComponent<Text>().text = amountInStorage + " at storage";
                        }
                    }
                }
            }
        }
    }

    // transport resource to the selected planet using Ether
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
        int NRes = GetNRes();
        if (NRes == 0) { return; }

        // change amount of your coins
        DateChangeing DC = TextDate.GetComponent<DateChangeing>();
        DateChangeing.sCoins = DC.AddCoins(cost);
        // show amount of your coins
        TextCoins.GetComponent<Text>().text = System.Convert.ToString(DateChangeing.sCoins);

        // take away the sold resource
        TakeAwayResource(--NRes);
    }

    private void AddResourceToPanet(int NRes)
    {
        if (numButtonResource > 0)
        {
            // add extraordinary resource
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1]++;
            PI.TakeAwayResourceFromStorage(numRes);
        }
        else
        {
            // add nesessary resource
            ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]++;
        }
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

    private void TakeAwayResource(int NRes)
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
            settings.reqRes[numRes]--;
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = SP.Show(settings.reqRes);
        }

        if (NRes == 0)
        {
            // dealing with an empty slot
            sSellButton.GetComponent<Button>().interactable = false;
        }
        if (settings.sNEther > 0)       { sEtherButton.SetActive(true); }
        if (settings.sNSpacecraft > 0)  { sTransportButton.GetComponent<Button>().interactable = true;  }
        if (settings.sNBlueCoin > 0)    { sBuyButton.GetComponent<Button>().interactable = true; }
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
