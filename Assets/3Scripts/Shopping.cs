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
    private static int numButton;

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

        numButton = n;

        bool flagTtravelWell = false;

        // show name of the resource
        if (numButton > 0)
        {
            // extraordinary resource
            numRes = ItemOnClick.PP.ResAdd[numButton - 1];
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
            textResource.GetComponent<Text>().text = getItems.ResNess[numButton];
            sTextStorage.GetComponent<Text>().text = "Unlimited resource";
            flagTtravelWell = true;
        }

        // show PanelShopping
        sPanelShopping.SetActive(true);
        sPanelPlanets.SetActive(false);
        sTextTitle.SetActive(false);

        // settings: may be buttons clicked or not 
        sTransportButton.GetComponent<Button>().interactable = flagTtravelWell;
        sBuyButton.GetComponent<Button>().interactable = true;
        sEtherButton.GetComponent<Button>().interactable = true;
        sSellButton.GetComponent<Button>().interactable = true;
        if (GetNRes() == 10)
        {
            sTransportButton.GetComponent<Button>().interactable = false;
            sBuyButton.GetComponent<Button>().interactable = false;
            sEtherButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (GetNRes() == 0) {sSellButton.GetComponent<Button>().interactable = false;}

            if (settings.sNSpacecraft == 0) {sTransportButton.GetComponent<Button>().interactable = false;}
            if (settings.sNEither == 0)     {sEtherButton.GetComponent<Button>().interactable = false;}
            if (settings.sNBlueCoin == 0)   {sBuyButton.GetComponent<Button>().interactable = false;}
        }
    }

    // transport resource to the selected planet
    public void TransportPressed()
    {
        int NRes = GetNRes();
        // amount of resource may be changed during this function 
        if (NRes < 10)
        {
            // amount of Spacecrafts may be changed during this function 
            if (settings.sNSpacecraft > 0)
            {
                bool flagTtravelWell = false;
                // necessary (unlimited) or extraordinary resource
                if (numButton > 0)  
                { flagTtravelWell = settingsResearches.Storage.ContainsKey(numRes);  }
                else                
                { flagTtravelWell = true; }

                if (flagTtravelWell == true)
                {
                    if (numButton > 0)
                    {
                        // transport extraordinary resource
                        ItemOnClick.PP.ResAddAmount[numButton - 1]++;
                        panelInform.TransportResource(numRes);
                    }
                    else
                    {
                        // transport nesessary resource
                        ItemOnClick.PP.ResNess_Amount[numButton + 3]++;
                    }

                    // shoe amount of resourse at storage
                    // ---

                    // show changes on the panelInform
                    panelInform.ResetPlanet(ItemOnClick.PP);

                    // show amount of spacecrafts
                    settings.sNSpacecraft--;
                    if (settings.sNSpacecraft == 0)
                    {TransportButton.GetComponent<Button>().interactable = false;}
                    settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

                    // show changes on the requirement resources's panel
                    if (settings.reqRes.ContainsKey(numRes))
                    {
                        settings.reqRes[numRes]++;
                        settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
                    }

                    // reset buttons
                    NRes++;
                    if (NRes > 0)
                    {
                        sSellButton.GetComponent<Button>().interactable = true;
                    }
                    if (NRes == 10)
                    {
                        sTransportButton.GetComponent<Button>().interactable = false;
                        sBuyButton.GetComponent<Button>().interactable = false;
                        sEtherButton.GetComponent<Button>().interactable = false;
                    }
                    if (numButton > 0)  
                    {
                        // if no more this resource at the Storage
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

    private int GetNRes()
    {
        // nesessary resource
        if (numButton < 0)
            return ItemOnClick.PP.ResNess_Amount[numButton + 3];
        
        // extraordinary resource
        return ItemOnClick.PP.ResAddAmount[numButton - 1];
    }

    // close the PanelShopping 
    public void CloseShopPressed()
    {
        sPanelPlanets.SetActive(true);
        sTextTitle.SetActive(true);
        sPanelShopping.SetActive(false);
    }
}
