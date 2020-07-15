using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelInform : MonoBehaviour
{
    public GameObject TextProbes;

    // reset currently planet's properties
    public static void ResetPlanet( getItems.PlanetProperty PP)
    {
        settingsResearches.sSphere.GetComponent<Renderer>().material = settings.sMaterials[PP.numMaterial];
        settingsResearches.sNamePlanet.GetComponent<Text>().text = PP.textName;
        settingsResearches.sTextIntro.GetComponent<Text>().text = getItems.sIntroduction[PP.numIntro];

        //show nesessary resources
        settingsResearches.rAir.GetComponentInChildren<Text>().text = getItems.ResNess[-3] + " = " + PP.ResNess_Amount[0];
        settingsResearches.rWater.GetComponentInChildren<Text>().text = getItems.ResNess[-2] + " = " + PP.ResNess_Amount[1];
        settingsResearches.rSoil.GetComponentInChildren<Text>().text = getItems.ResNess[-1] + " = " + PP.ResNess_Amount[2];

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
            settingsResearches.r1.GetComponentInChildren<Text>().text = getItems.ResourceAdd[PP.ResAdd[0]] +
            " = " + PP.ResAddAmount[0];
            settingsResearches.r2.GetComponentInChildren<Text>().text = getItems.ResourceAdd[PP.ResAdd[1]] +
            " = " + PP.ResAddAmount[1];
            settingsResearches.r3.GetComponentInChildren<Text>().text = getItems.ResourceAdd[PP.ResAdd[2]] +
            " = " + PP.ResAddAmount[2];
        }
    }
    
    // learn more about this planet
    public void ResearchPressed()
    {
        // if researching is avaliable
        if (!ItemOnClick.PP.flagIsResearched)
        {
            if (settings.sNProbes > 0)
            {
                settings.sNProbes --;
                TextProbes.GetComponent<Text>().text = settings.sNProbes + " probes";

                // update resources at storage


                if (settings.flagSelectedPlanet==false)
                {
                    settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = "Select";
                }
                else
                {
                    settingsResearches.sButtonResearchSelect.SetActive(false);
                }
                
                ItemOnClick.PP.flagIsResearched = true;

                // show resources
                settingsResearches.r1.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                    ItemOnClick.PP.ResAdd[0]] + " = " + ItemOnClick.PP.ResAddAmount[0];
                settingsResearches.r2.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                    ItemOnClick.PP.ResAdd[1]] + " = " + ItemOnClick.PP.ResAddAmount[1];
                settingsResearches.r3.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                    ItemOnClick.PP.ResAdd[2]] + " = " + ItemOnClick.PP.ResAddAmount[2];
            }
            else
            {
                // ask to make order to a probe factory OR to get a probe free by Advert 

            }
        }
        else
        {
            SelectPressed();
        }
    }

    // choose the planet to populate it
    private void SelectPressed()
    {
        ItemOnClick.PP.flagIsSelected = true;

        settings.flagSelectedPlanet = true;
        settings.SelectedPlanet = ItemOnClick.PP;
        settingsResearches.sButtonResearchSelect.SetActive(false);

        settingsResearches.ChosenPlanet.GetComponent<Outline>().effectColor = settings.sColorPause;
        ItemOnClick.sButtonName.GetComponent<Outline>().effectColor = settings.sColorPause;

        // change requested resources
        // nesessary
        for (int i = 0; i < 3; i++)
        {settings.reqRes[i - 3] = ItemOnClick.PP.ResNess_Amount[i];}
        // extraordinary 
        for (int i = 0; i < 3; i++)
        {
            int n = ItemOnClick.PP.ResAdd[i];
            if (settings.reqRes.ContainsKey(n))
            {
                settings.reqRes[n] = ItemOnClick.PP.ResAddAmount[i];
            }
        }
        settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
    }
}
