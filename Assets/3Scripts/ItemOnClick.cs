using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClick : MonoBehaviour
{
    // chosen planet
    public GameObject Planet;
    private static GameObject sPlanet;

    // gameobject to set its material
    public GameObject Sphere;
    private static GameObject sSphere;

    private static Outline myOutline;

    private static Color colorSelected = new Color(0, 0, 0); // Black
    private static Color colorDeselected = new Color(0, 0, 0); // needs changeing

    public void OnMouseDown()
    {
        // reference to Outline
        myOutline = GetComponent<Outline>();

        sPlanet = Planet;
        sSphere = Sphere;
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
        //settingsResearches.sPlanet;
        ItemSelect();
        //}
    }


    // reaction to the "OnMouseDown" event
    public static void ItemSelect()
    {
        GameObject instance = settingsResearches.ChosenPlanet;

        // illustrate the ItemImage changeing 
        if (colorDeselected== colorSelected)
        {   colorDeselected = instance.transform.GetComponent<Image>().color;   }

        instance.transform.GetComponent<Image>().color = colorSelected;
        myOutline = instance.GetComponent<Outline>();
        myOutline.enabled = true;


        // dealing with the PanelInformation
        //InformationPlanetView();


        // Sphere's texture
        //settingsResearches.TestItemView view = new settingsResearches.TestItemView(instance.transform);
        //view.material = settings.sMaterials[settingsResearches.sPlanet.numMaterial];
        //print(view.material);

        // connect with panelInformation
        //settingsResearches.ChosenPlanet.transform.numMaterial;

        //sSphere.GetComponent<Renderer>.material = settings.sMaterials[0];
        //Material M = GetComponent<Renderer>().material;
    }

    // reaction to the "OnMouseDown" event
    private static void ItemDeselect(GameObject previousPlanet)
    {
        GameObject instance = settingsResearches.ChosenPlanet;
        instance.transform.GetComponent<Image>().color = colorDeselected;
        myOutline = instance.GetComponent<Outline>();
        myOutline.enabled = false;
    }
}
