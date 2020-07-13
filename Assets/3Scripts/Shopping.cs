using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shopping : MonoBehaviour
{
    public GameObject PanelShopping;
    public GameObject textResource;

    // choose a resource to buy/sell/transport it
    public void ResourcePressed(int n)
    {
        if (ItemOnClick.PP.flagIsSelected == false)
        { return; }


        if (n<0)
        {
            // nesessary resource
            textResource.GetComponent<Text>().text = getItems.ResNess[n];
        }
        else
        {
            // extraordinary resource

        }

        PanelShopping.SetActive(true);

    }

    // close the PanelShopping 
    public void CloseShopPressed()
    {
        PanelShopping.SetActive(false);
    }
}
