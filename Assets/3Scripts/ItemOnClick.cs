using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnClick : MonoBehaviour
{
    public GameObject Planet;
    private static GameObject sPlanet;

    public GameObject Sphere;
    private static GameObject sSphere;

    private static Color colorSelected = new Color(0, 0, 0); // Black
    private static Color colorDeselected = new Color(104, 104, 104); // Grey
    private static GameObject rectSelect = null;

    public void OnMouseDown()
    {
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
        ItemSelect();
        //}
    }


    // reaction to the "OnMouseDown" event
    public static void ItemSelect()
    {
        GameObject instance = settingsResearches.ChosenPlanet;

        instance.transform.GetComponent<Image>().color = colorSelected;
        // create an object at parent's position
        rectSelect = Instantiate(settingsResearches.sRectSelect,
            instance.transform.position, instance.transform.rotation);
        rectSelect.transform.SetParent(instance.transform, true);

        // connect with panelInformation
        //settingsResearches.ChosenPlanet.transform.numMaterial;

        //sSphere.GetComponent<Renderer>.material = settings.sMaterials[0];
        //Material M = GetComponent<Renderer>().material;
    }

    // reaction to the "OnMouseDown" event
    private static void ItemDeselect(GameObject previousPlanet)
    {
        //previousPlanet.transform.GetComponent<Image>().color = colorDeselected;
        Destroy(rectSelect);
    }
}
