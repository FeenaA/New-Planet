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
        settings.reqRes[-3] = (int)ItemOnClick.PP.ResNess_Amount[0];
        settings.reqRes[-2] = (int)ItemOnClick.PP.ResNess_Amount[1];
        settings.reqRes[-1] = (int)ItemOnClick.PP.ResNess_Amount[2];

        int n = (int)(ItemOnClick.PP.ResAdd.x);
        if (settings.reqRes.ContainsKey(n))
        {
            settings.reqRes[n] = (int)ItemOnClick.PP.ResAddAmount[0];
        }
        n = (int)(ItemOnClick.PP.ResAdd.y);
        if (settings.reqRes.ContainsKey(n))
        {
            settings.reqRes[n] = (int)ItemOnClick.PP.ResAddAmount[1];
        }
        n = (int)(ItemOnClick.PP.ResAdd.z);
        if (settings.reqRes.ContainsKey(n))
        {
            settings.reqRes[n] = (int)ItemOnClick.PP.ResAddAmount[2];
        }
        settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
    }
}
