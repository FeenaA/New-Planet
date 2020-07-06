using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelInform : MonoBehaviour
{
    public GameObject TextProbes;

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
                    (int)(ItemOnClick.PP.ResAdd.x)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.x);
                settingsResearches.r2.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                    (int)(ItemOnClick.PP.ResAdd.y)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.y);
                settingsResearches.r3.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                    (int)(ItemOnClick.PP.ResAdd.z)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.z);
            }
            else
            {
                // ask to make order to a probe factory OR to get a probe free by Advert 

            }
        }
        else
        {
            // Select is pressed
            ItemOnClick.PP.flagIsSelected = true;

            settings.flagSelectedPlanet = true;
            settings.SelectedPlanet = ItemOnClick.PP;
            settingsResearches.sButtonResearchSelect.SetActive(false);

            settingsResearches.ChosenPlanet.GetComponent<Outline>().effectColor = settings.sColorPause;
            ItemOnClick.sButtonName.GetComponent<Outline>().effectColor = settings.sColorPause;

        }
    }

    // choose the planet to populate
    public void SelectPressed()
    {

    }


}
