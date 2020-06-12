using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClick : MonoBehaviour
{
    // chosen planet
    public GameObject Planet;
    private static GameObject sPlanet;
    public Button buttonName;
    public static Button sButtonName;

    private static Outline myOutline;

    private static Color colorSelected = new Color(0, 0, 0); // Black
    private static Color colorDeselected = new Color(0, 0, 0); // needs changeing

    public void OnMouseDown()
    {
        // reference to Outline
        myOutline = GetComponent<Outline>();

        sPlanet = Planet;
        sButtonName = buttonName;

        sOnMouseDown();
    }

    private static void sOnMouseDown()
    {
        

        // if (!=) // if previous and current planets aren't the same
        //{
        // deselect previous chosen planet
        ItemDeselect(settingsResearches.ChosenPlanet);
        // update chosen planet
        settingsResearches.ChosenPlanet = sPlanet;
        ItemSelect();
        //}
    }


    // reaction to the "OnMouseDown" event
    public static void ItemSelect()
    {
        GameObject instance = settingsResearches.ChosenPlanet;

        // illustrate the ItemImage changeing 
        if (colorDeselected == colorSelected)
        {   colorDeselected = instance.transform.GetComponent<Image>().color;   }

        instance.transform.GetComponent<Image>().color = colorSelected;
        myOutline = instance.GetComponent<Outline>();
        myOutline.enabled = true;

        sButtonName.GetComponent<Graphic>().color = colorSelected;

        // dealing with the PanelInformation
        string textNumber = instance.transform.Find("TextNumber").GetComponent<Text>().text;
        int nPlanet = System.Convert.ToInt32(textNumber);
        
        settings.planetProperty PP = settings.sPlanetProperty[nPlanet];
        settingsResearches.sSphere.GetComponent<Renderer>().material = settings.sMaterials[PP.numMaterial];
        settingsResearches.sNamePlanet.GetComponent<Text>().text = PP.textName;

    }

    // reaction to the "OnMouseDown" event
    private static void ItemDeselect(GameObject previousPlanet)
    {
        GameObject instance = settingsResearches.ChosenPlanet;
        instance.transform.GetComponent<Image>().color = colorDeselected;

        Button buttonName = previousPlanet.transform.Find("ButtonName").GetComponent<Button>();
        buttonName.transform.GetComponent<Image>().color = colorDeselected;

        //sButtonName.GetComponent<Graphic>().color = colorDeselected;
        myOutline = instance.GetComponent<Outline>();
        myOutline.enabled = false;
    }
}
