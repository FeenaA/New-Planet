using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{
    public GameObject PanelShopping;
    public GameObject textResource;

    public GameObject TransportButton;
    private static GameObject sTransportButton;
    public GameObject BuyButton;
    private static GameObject sBuyButton;
    public GameObject EtherButton;
    private static GameObject sEtherButton;
    public GameObject SellButton;
    private static GameObject sSellButton;

    private static int numRes;
    private static int numButton;

    void Start()
    {
        sTransportButton = TransportButton;
        sBuyButton = BuyButton;
        sEtherButton = EtherButton;
        sSellButton = SellButton;
    }

    // choose a resource to buy/sell/transport it
    public void ResourcePressed(int n)
    {
        if (ItemOnClick.PP.flagIsSelected == false)
        { return; }

        numButton = n;       

        // show name of the resource
        if (numButton > 0)
        {
            // extraordinary resource
            numRes = ItemOnClick.PP.ResAdd[numButton - 1];
            textResource.GetComponent<Text>().text = getItems.ResourceAdd[numRes];
        }
        else
        {
            // nesessary resource
            textResource.GetComponent<Text>().text = getItems.ResNess[numButton];
        }

        PanelShopping.SetActive(true);
        
        // now all buttons may be clicked
        sTransportButton.GetComponent<Button>().interactable = true;
        sBuyButton.GetComponent<Button>().interactable = true;
        sEtherButton.GetComponent<Button>().interactable = true;
        sSellButton.GetComponent<Button>().interactable = true;

        // settings: may be buttons clicked or not 
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

    // close the PanelShopping 
    public void CloseShopPressed()
    {
        PanelShopping.SetActive(false);
    }

    // transport resource to the selected planet
    public void TransportPressed()
    {
        bool resourceFound = false;
        int i = 0, N = getItems.sPlanetProperty.Count;

        if (numButton < 0)
        {
            // transport nesessary resource
            int NRes = GetNRes();
            if (NRes < 10)
            {
                if (settings.sNSpacecraft > 0)
                {
                    settings.sNSpacecraft--;
                    settingsResearches.sTextSC.GetComponent<Text>().text = System.Convert.ToString(settings.sNSpacecraft);

                    ItemOnClick.PP.ResNess_Amount[numButton + 3]++;
                    panelInform.ResetPlanet(ItemOnClick.PP);

                    settings.reqRes[numButton]++;
                    settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);

                    if (NRes + 1 == 10)
                    {
                        TransportButton.GetComponent<Button>().interactable = false;
                        sBuyButton.GetComponent<Button>().interactable = false;
                        sEtherButton.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
        else
        {
            // transport extraordinary resource

            // looking for the resource
            while ((resourceFound == false) || (i < N))
            {
                if (getItems.sPlanetProperty[i].flagIsResearched == true)
                {

                }

                i++;
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
}
