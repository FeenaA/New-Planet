using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClick : MonoBehaviour
{
    // chosen planet
    public GameObject Planet;
    private static GameObject sPlanet;
    // previous chosen planet
    private static GameObject sPreviousPlanet;
    // button on the table
    public Button buttonName;
    public static Button sButtonName;

    private static Color colorSelected = new Color(0, 0, 0); // Black
    private static Color colorDeselected = new Color(0, 0, 0); // needs changeing
    private static int nCurrentPlanet;
    // properties of chosen planet
    public static getItems.PlanetProperty PP;

    // Method to be called from the inspector
    public void OnMouseDown()
    {
        sPlanet = Planet;
        sButtonName = buttonName;

        sOnMouseDown();
    }

    // Method to be called from the inspector
    private void sOnMouseDown()
    {
        // if previous and current planets aren't the same
        if (sPreviousPlanet != sPlanet) 
        {
            // deselect the previous chosen planet
            ItemDeselect(sPreviousPlanet);

            // update chosen planet
            settingsResearches.ChosenPlanet = sPlanet;
            // select new planet
            ItemSelect(sPlanet);
        }
    }

    // reaction to the "OnMouseDown" event for table
    public void ItemSelect(GameObject instance)
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
        nCurrentPlanet = System.Convert.ToInt32(textNumber);

        // PP includes all information about particular planet
        //PP = getItems.sPlanetProperty[nCurrentPlanet];
        PP = settings.gameSettings.SetPlanets[nCurrentPlanet];

        panelInform PI = settingsResearches.sPanelInformation.GetComponent<panelInform>();
        PI.ResetPlanet(PP);

        sPreviousPlanet = instance;
    }

    // reaction to the "OnMouseDown" event for table
    private static void ItemDeselect(GameObject instance)
    {
        getItems.PlanetProperty PP = settings.gameSettings.SetPlanets[nCurrentPlanet];
        Button buttonName = instance.transform.Find("ButtonName").GetComponent<Button>();

        if (!PP.flagIsResearched)
        {
            instance.transform.GetComponent<Image>().color = colorDeselected;
            buttonName.transform.GetComponent<Image>().color = colorDeselected;
        }

        if (!PP.flagIsSelected)
        {
            buttonName.GetComponent<Outline>().enabled = false;
            instance.GetComponent<Outline>().enabled = false;
        }
    }
}
