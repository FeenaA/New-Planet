using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelInform : MonoBehaviour
{
    public GameObject TextProbes;

    // reset and show currently planet's properties
    public static void ResetPlanet(getItems.PlanetProperty PP)
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
            //if (settings.flagSelectedPlanet == false)
            if (PP.flagIsSelected == false)
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
                settings.sNProbes--;
                TextProbes.GetComponent<Text>().text = settings.sNProbes + " probes";

                if (settings.flagSelectedPlanet == false)
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

                // update resources at storage
                AddToStorage();
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

        settingsResearches.ChosenPlanet.GetComponent<Outline>().effectColor = buttons.sColorPause;
        ItemOnClick.sButtonName.GetComponent<Outline>().effectColor = buttons.sColorPause;

        // change requested resources
        // nesessary
        for (int i = 0; i < 3; i++)
        { settings.reqRes[i - 3] = ItemOnClick.PP.ResNess_Amount[i]; }
        // extraordinary 
        for (int i = 0; i < 3; i++)
        {
            int n = ItemOnClick.PP.ResAdd[i];
            if (settings.reqRes.ContainsKey(n))
            {
                settings.reqRes[n] = ItemOnClick.PP.ResAddAmount[i];
            }

            // remove selected planet from storage
            RemoveFromStorage();
        }
        settingsResearches.sTextRequestedResources.GetComponent<Text>().text = showProgress.Show(settings.reqRes);
    }

    // add new resources to be transportable
    private void AddToStorage()
    {
        for (int i = 0; i < 3; i++)
        {
            int key = ItemOnClick.PP.ResAdd[i];
            settingsResearches.AcceptRes PlanetAmount = new settingsResearches.AcceptRes
            {
                amount = ItemOnClick.PP.ResAddAmount[i],
                NamePlanet = ItemOnClick.PP.textName
            };
            if (settingsResearches.Storage.ContainsKey(key))
            {
                settingsResearches.Storage[ItemOnClick.PP.ResAdd[i]].Add(PlanetAmount);
            }
            else
            {
                List<settingsResearches.AcceptRes> value = new List<settingsResearches.AcceptRes> { PlanetAmount };
                settingsResearches.Storage.Add(key, value);
            }
        }

        /*foreach (var item in settingsResearches.Storage)
        {
            foreach (var value in item.Value)
            {
                print(getItems.ResourceAdd[item.Key] + ": " + value.NamePlanet + ", " + value.amount);
            }
        }*/
    }

    // remove selected planet from storage
    private void RemoveFromStorage()
    {
        var keysForRemove = new List<int>();

        // remove information from Values
        foreach (var item in settingsResearches.Storage)
        {
            int j = 0, Size = item.Value.Count;
            bool flagFound = false;
            while ((flagFound == false) && (j < Size))
            {
                if (item.Value[j].NamePlanet == ItemOnClick.PP.textName)
                {
                    flagFound = true;
                    item.Value.Remove(item.Value[j]);

                    // Make a list of keys with no resources
                    if (Size - 1 == 0)
                    {
                        keysForRemove.Add(item.Key);
                    }
                }
                j++;
            }
        }

        // Remove empty resource from the storage
        foreach (var key in keysForRemove)
        {
            settingsResearches.Storage.Remove(key);
        }
    }

    // get amount of resourse "numRes" in the storage
    public static int GetAmountInStorage(int key)
    {
        int res = 0;
        if (settingsResearches.Storage.ContainsKey(key))
        {
            var content = settingsResearches.Storage[key];
            foreach (var item in content)
            {
                res += item.amount;
            }
        }
        return res;
    }

    // use 1 resource from the storage and remove empty resource
    public static void TransportResource(int numRes)
    {
        // change amount of resource at the storage
        --settingsResearches.Storage[numRes][0].amount;

        // change amount of resource at the planet
        int key = 1;
        int Size = settings.sNPlanets;
        bool flagPlanetFound = false;
        string NamePlanet = settingsResearches.Storage[numRes][0].NamePlanet;
        while ((!flagPlanetFound) && (key < Size))
        {
            if (getItems.sPlanetProperty[key].textName == NamePlanet)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (getItems.sPlanetProperty[key].ResAdd[j] == numRes)
                    { getItems.sPlanetProperty[key].ResAddAmount[j]--; }
                }
                flagPlanetFound = true;
            }
            key++;
        }

        if (settingsResearches.Storage[numRes][0].amount == 0)
        {
            settingsResearches.Storage[numRes].Remove(settingsResearches.Storage[numRes][0]);
            // remove empty resource from the storage
            if (settingsResearches.Storage[numRes].Count == 0)
            { settingsResearches.Storage.Remove(numRes); }
        }
    }
}