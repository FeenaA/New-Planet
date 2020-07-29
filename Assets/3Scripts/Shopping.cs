using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{
    public GameObject PanelShopping;
    private static GameObject sPanelShopping;
    public GameObject PanelPlanets;
    private static GameObject sPanelPlanets;
    public GameObject TextTitle;
    private static GameObject sTextTitle;

    public GameObject textResource;

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

    private static int numRes;
    private static int numButtonResource;

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
        if (ItemOnClick.PP.flagIsSelected == false)
        { return; }

        numButtonResource = n;

        bool flagTtravelWell = false;

        // show name of the resource
        if (numButtonResource > 0)
        {
            // extraordinary resource
            numRes = ItemOnClick.PP.ResAdd[numButtonResource - 1];
            textResource.GetComponent<Text>().text = getItems.ResourceAdd[numRes];
            int amountInStorage = panelInform.GetAmountInStorage(numRes);
            if (amountInStorage > 0)    
            {
                flagTtravelWell = true;
                sTextStorage.GetComponent<Text>().text = amountInStorage + " at storage";
            }
            else
            {
                sTextStorage.GetComponent<Text>().text = "resource is off";
            }
        }
        else
        {
            // nesessary resource
            numRes = numButtonResource;
            textResource.GetComponent<Text>().text = getItems.ResNess[numButtonResource];
            sTextStorage.GetComponent<Text>().text = "Unlimited resource";
            flagTtravelWell = true;
        }

        // show PanelShopping and disable PanelPlanets
        sPanelShopping.SetActive(true);
        sPanelPlanets.SetActive(false);
        sTextTitle.SetActive(false);

        // get amount of the resource
        int AmountOfRes = GetNRes();

        // settings: may be buttons clicked or not 
        sTransportButton.GetComponent<Button>().interactable = flagTtravelWell;
        sBuyButton.GetComponent<Button>().interactable = true;
        //sEtherButton.GetComponent<Button>().interactable = true;
        sSellButton.GetComponent<Button>().interactable = true;
        if (AmountOfRes == 10)
        {
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            //sEtherButton.GetComponent<Button>().interactable = false;
            sEtherButton.SetActive(false);
        }
        else
        {
            if (AmountOfRes == 0) {sSellButton.GetComponent<Button>().interactable = false;}

            if (settings.sNSpacecraft == 0) {sTransportButton.GetComponent<Button>().interactable = false;}
            if (settings.sNEther == 0)     
            {
                //sEtherButton.GetComponent<Button>().interactable = false;
                sEtherButton.SetActive(false);
            }
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
                    AddResource( ++NRes );

                    // show amount of spacecrafts
                    settings.sNSpacecraft--;
                    if (settings.sNSpacecraft == 0)
                    {TransportButton.GetComponent<Button>().interactable = false;}
                    settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

                    // checking: if no more this resource at the Storage (requirement for transportation)
                    if (numButtonResource > 0)  
                    {
                        
                        int amountInStorage = panelInform.GetAmountInStorage(numRes);
                        if (amountInStorage == 0)
                        {   
                            TransportButton.GetComponent<Button>().interactable = false;
                            sTextStorage.GetComponent<Text>().text = "resource is off"; 
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
        int NRes = GetNRes();
        // amount of resource may be changed during this function 
        if (NRes < 10)
        {
            // amount of Ether may be changed during this function 
            if (settings.sNEther > 0)
            {
                bool flagTtravelWell = false;
                // necessary (unlimited) or extraordinary resource
                if (numButtonResource > 0)
                { flagTtravelWell = settingsResearches.Storage.ContainsKey(numRes); }
                else
                { flagTtravelWell = true; }

                if (flagTtravelWell == true)
                {
                    AddResource(++NRes);

                    /*// show amount of spacecrafts
                    settings.sNSpacecraft--;
                    if (settings.sNSpacecraft == 0)
                    { TransportButton.GetComponent<Button>().interactable = false; }
                    settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

                    // checking: if no more this resource at the Storage (requirement for transportation)
                    if (numButtonResource > 0)
                    {
                        int amountInStorage = panelInform.GetAmountInStorage(numRes);
                        if (amountInStorage == 0)
                        {
                            TransportButton.GetComponent<Button>().interactable = false;
                            sTextStorage.GetComponent<Text>().text = "resource is off";
                        }
                        else
                        {
                            sTextStorage.GetComponent<Text>().text = amountInStorage + " at storage";
                        }
                    }*/
                }
            }
        }
    }

    private void AddResource(int NRes)
    {
        if (numButtonResource > 0)
        {
            // add extraordinary resource
            ItemOnClick.PP.ResAddAmount[numButtonResource - 1]++;
            panelInform.TransportResource(numRes);
        }
        else
        {
            // add nesessary resource
            ItemOnClick.PP.ResNess_Amount[numButtonResource + 3]++;
        }
        // show changes on the panelInform
        panelInform.ResetPlanet(ItemOnClick.PP);

        // show changes on the requirement resources's panel  
        if (settings.reqRes.ContainsKey(numRes))
        {
            settings.reqRes[numRes]++;
            settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
        }

        // reset buttons
        if (NRes == 10)
        {
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            //sEtherButton.GetComponent<Button>().interactable = false;
            sEtherButton.SetActive(false);
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
