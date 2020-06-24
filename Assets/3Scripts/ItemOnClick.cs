using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClick : MonoBehaviour
{
    // chosen planet
    public GameObject Planet;
    private static GameObject sPlanet;
    private static GameObject sPreviousPlanet;
    public Button buttonName;
    public static Button sButtonName;

    private static Color colorSelected = new Color(0, 0, 0); // Black
    private static Color colorDeselected = new Color(0, 0, 0); // needs changeing

    // Method to be called from the inspector
    public void OnMouseDown()
    {
        sPlanet = Planet;
        sButtonName = buttonName;

        sOnMouseDown();
    }

    // Method to be called from the inspector
    private static void sOnMouseDown()
    {
        // if (!=) // if previous and current planets aren't the same
        //{
        // deselect the previous chosen planet
        ItemDeselect(sPreviousPlanet);

        // update chosen planet
        settingsResearches.ChosenPlanet = sPlanet;
        // select new planet
        ItemSelect(sPlanet);
        //}
    }

    // reaction to the "OnMouseDown" event
    public static void ItemSelect(GameObject instance)
    {
        // illustrate the ItemImage changeing 
        if (colorDeselected == colorSelected)
        {   colorDeselected = instance.transform.GetComponent<Image>().color;   }

        instance.transform.GetComponent<Image>().color = colorSelected;
        instance.GetComponent<Outline>().enabled = true;

        sButtonName.GetComponent<Graphic>().color = colorSelected;
        sButtonName.GetComponent<Outline>().enabled = true;

        // dealing with the PanelInformation
        string textNumber = instance.transform.Find("TextNumber").GetComponent<Text>().text;
        int nPlanet = System.Convert.ToInt32(textNumber);
        
        settings.planetProperty PP = settings.sPlanetProperty[nPlanet];
        settingsResearches.sSphere.GetComponent<Renderer>().material = settings.sMaterials[PP.numMaterial];
        settingsResearches.sNamePlanet.GetComponent<Text>().text = PP.textName;
        settingsResearches.sTextIntro.GetComponent<Text>().text = settings.sIntroduction[PP.numIntro];
        settingsResearches.sTextResource.GetComponent<Text>().text = PP.textName;

        sPreviousPlanet = instance;
    }

    // reaction to the "OnMouseDown" event
    private static void ItemDeselect(GameObject instance)
    {
        instance.transform.GetComponent<Image>().color = colorDeselected;

        Button buttonName = instance.transform.Find("ButtonName").GetComponent<Button>();
        buttonName.transform.GetComponent<Image>().color = colorDeselected;
        buttonName.GetComponent<Outline>().enabled = false;

        instance.GetComponent<Outline>().enabled = false;
    }
}
