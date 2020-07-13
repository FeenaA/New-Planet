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
    private static int nCurrentPlanet;
    // properties of chosen planet
    public static getItems.planetProperty PP;

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

    // reaction to the "OnMouseDown" event for table
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
        nCurrentPlanet = System.Convert.ToInt32(textNumber);
        
        // PP includes all information about particular planet
        PP = getItems.sPlanetProperty[nCurrentPlanet];
        settingsResearches.sSphere.GetComponent<Renderer>().material = settings.sMaterials[PP.numMaterial];
        settingsResearches.sNamePlanet.GetComponent<Text>().text = PP.textName;
        settingsResearches.sTextIntro.GetComponent<Text>().text = getItems.sIntroduction[PP.numIntro];

        //show nesessary resources
        settingsResearches.rAir.GetComponentInChildren<Text>().text = "Air = " + PP.ResNess_Amount[0];
        settingsResearches.rWater.GetComponentInChildren<Text>().text = "Water = " + PP.ResNess_Amount[1];
        settingsResearches.rSoil.GetComponentInChildren<Text>().text = "Soil = " + PP.ResNess_Amount[2];

        if (PP.flagIsResearched == false)
        {
            settingsResearches.sButtonResearchSelect.SetActive(true);
            settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = "Research";
            settingsResearches.r1.GetComponentInChildren<Text>().text = "-";
            settingsResearches.r2.GetComponentInChildren<Text>().text = "-";
            settingsResearches.r3.GetComponentInChildren<Text>().text = "-";
        }
        else
        {
            if (settings.flagSelectedPlanet == false)
            {
                settingsResearches.sButtonResearchSelect.SetActive(true);
                settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = "Select";
            }
            else
            {
                settingsResearches.sButtonResearchSelect.SetActive(false);
            }

            //show additional resources
            settingsResearches.r1.GetComponentInChildren<Text>().text = getItems.ResourceAdd[(int)(PP.ResAdd.x)] + 
            " = " + (int)(PP.ResAddAmount.x);
            settingsResearches.r2.GetComponentInChildren<Text>().text = getItems.ResourceAdd[(int)(PP.ResAdd.y)] + 
            " = " + (int)(PP.ResAddAmount.y);
            settingsResearches.r3.GetComponentInChildren<Text>().text = getItems.ResourceAdd[(int)(PP.ResAdd.z)] + 
            " = " + (int)(PP.ResAddAmount.z);
        }

        sPreviousPlanet = instance;
    }

    // reaction to the "OnMouseDown" event for table
    private static void ItemDeselect(GameObject instance)
    {
        getItems.planetProperty PP = getItems.sPlanetProperty[nCurrentPlanet];
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
