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
        if ((!ItemOnClick.PP.flagIsResearched) && (settings.NProbes > 0))
        {
            settings.NProbes --;
            TextProbes.GetComponent<Text>().text = settings.NProbes + " probes";
            settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = "Select";
            ItemOnClick.PP.flagIsResearched = true;

            // show resources
            settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = "Select";
            settingsResearches.r1.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                (int)(ItemOnClick.PP.ResAdd.x)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.x);
            settingsResearches.r2.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                (int)(ItemOnClick.PP.ResAdd.y)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.y);
            settingsResearches.r3.GetComponentInChildren<Text>().text = getItems.ResourceAdd[
                (int)(ItemOnClick.PP.ResAdd.z)] + " = " + (int)(ItemOnClick.PP.ResAddAmount.z);

        }
    }

    // choose the planet to populate
    public void SelectPressed()
    {

    }


}
