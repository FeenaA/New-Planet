using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelInform : MonoBehaviour
{
    public GameObject TextProbes;
    public GameObject TextDate;
    private readonly int presentedCoins = 1000;
    public GameObject MessageBox;
    public Canvas MainCanvas;
    public static string strResearch = "Research";
    public static string strSelect = "Select";


    void Start()
    {
        CorrectLanguage();
    }

    /// <summary>
    /// reset and show currently planet's properties on the PanelInformation
    /// </summary>
    /// <param name="PP"></param>
    public void ResetPlanet(getItems.PlanetProperty PP)
    {
        settingsResearches.sSphere.GetComponent<Renderer>().material = settings.sMaterials[PP.numMaterial];
        settingsResearches.sNamePlanet.GetComponent<Text>().text = PP.textName;
        settingsResearches.sTextIntro.GetComponent<Text>().text = getItems.sIntroduction[PP.numIntro];

        //show nesessary resources
        settingsResearches.rAir.GetComponentInChildren<Text>().text =
            getItems.ResNess[-3].name + " = " + PP.ResNess_Amount[0];
        settingsResearches.rWater.GetComponentInChildren<Text>().text =
            getItems.ResNess[-2].name + " = " + PP.ResNess_Amount[1];
        settingsResearches.rSoil.GetComponentInChildren<Text>().text =
            getItems.ResNess[-1].name + " = " + PP.ResNess_Amount[2];

        // extraordinary resources
        if (PP.flagIsResearched == false)
        {
            settingsResearches.sButtonResearchSelect.SetActive(true);
            settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = strResearch;

            if (settings.gameSettings.NProbe == 0)
            { settingsResearches.sButtonResearchSelect.GetComponent<Button>().interactable = false; }

            foreach (var resource in settingsResearches.r)
            {
                resource.GetComponentInChildren<Text>().text = "-";
            }
        }
        else
        {
            // dealing with Research/Select Button
            if (settings.gameSettings.flagSelectedPlanet == false)
            {
                settingsResearches.sButtonResearchSelect.SetActive(true);
                settingsResearches.sButtonResearchSelect.GetComponent<Button>().interactable = true;
                settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = strSelect;
            }
            else { settingsResearches.sButtonResearchSelect.SetActive(false); }

            for (int i = 0; i < 3; i++)
            {
                int amount = PP.ResAddAmount[i];
                if (amount > 0)
                {
                    settingsResearches.r[i].GetComponentInChildren<Text>().text =
                        getItems.ResourceAdd[PP.ResAdd[i]].name + " = " + PP.ResAddAmount[i];
                }
                else
                {
                    settingsResearches.r[i].GetComponentInChildren<Text>().text = "-";
                    // let free current resource slot 
                    PP.ResAdd[i] = 0;
                }
            }
        }
    }

    // learn more about this planet
    public void ResearchPressed()
    {
        // if researching is avaliable
        if (!ItemOnClick.PP.flagIsResearched)
        {
            if (settings.gameSettings.NProbe > 0)
            {
                settings.gameSettings.NProbe--;
                TextProbes.GetComponent<Text>().text = 
                    System.Convert.ToString(settings.gameSettings.NProbe);

                if (settings.gameSettings.flagSelectedPlanet == false)
                { settingsResearches.sButtonResearchSelect.GetComponentInChildren<Text>().text = strSelect; }
                else
                { settingsResearches.sButtonResearchSelect.SetActive(false); }

                ItemOnClick.PP.flagIsResearched = true;

                // show resources
                for (int i = 0; i < 3; i++)
                {
                    settingsResearches.r[i].GetComponentInChildren<Text>().text =
                        getItems.ResourceAdd[ItemOnClick.PP.ResAdd[i]].name + " = " + ItemOnClick.PP.ResAddAmount[i];
                }

                // deal with Ether
                if (ItemOnClick.PP.flagEther)
                {
                    if (settings.gameSettings.NEther + 1 <= DateChangeing.MaxN)
                    {
                        settings.gameSettings.NEther++;
                        // show changes
                        settingsResearches.sTextEth.GetComponent<Text>().text =
                            System.Convert.ToString(settings.gameSettings.NEther);

                        // show Congratulations
                        var instance = Instantiate(MessageBox);
                        instance.SendMessage("TheStart", false);
                        instance.transform.SetParent(MainCanvas.transform, false);

                        bool LoadStorage = false;
                        LoadGame.SetEther(LoadStorage);
                    }

                }

                // deal with Coins
                if (ItemOnClick.PP.flagCoins)
                {
                    DateChangeing DC = TextDate.GetComponent<DateChangeing>();
                    settings.gameSettings.NCoins = DC.AddCoins(presentedCoins);
                    // show changes
                    DateChangeing.sTextCoinsObject.GetComponent<Text>().text = System.Convert.ToString(settings.gameSettings.NCoins);

                    // show Congratulations
                    var instance = Instantiate(MessageBox);
                    instance.SendMessage("TheStart", true);
                    instance.transform.SetParent(MainCanvas.transform, false);
                }

                // update resources at storage
                AddToStorage();
                // set flagResearched=true, --probes, resources at storage
                LoadGame.SetResearched(ItemOnClick.PP.textName);
            }
            else
            {
                // ask to make order to a probe factory OR to get a probe free by BlueCoin

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

        settings.gameSettings.flagSelectedPlanet = true;
        settings.gameSettings.NameNew = ItemOnClick.PP.textName;
        //settings.SelectedPlanet = ItemOnClick.PP;
        settingsResearches.sButtonResearchSelect.SetActive(false);

        settingsResearches.ChosenPlanet.GetComponent<Outline>().effectColor = buttons.sColorPause;
        ItemOnClick.sButtonName.GetComponent<Outline>().effectColor = buttons.sColorPause;

        // change requested resources
        // nesessary
        for (int i = 0; i < 3; i++)
        { settings.gameSettings.RequestedResources[i - 3] = ItemOnClick.PP.ResNess_Amount[i]; }
        // extraordinary 
        for (int i = 0; i < 3; i++)
        {
            int n = ItemOnClick.PP.ResAdd[i];
            if (settings.gameSettings.RequestedResources.ContainsKey(n))
            {
                settings.gameSettings.RequestedResources[n] = ItemOnClick.PP.ResAddAmount[i];
            }

            // remove selected planet from storage
            RemoveFromStorage();
        }
        ShowProgress SP = GetComponent<ShowProgress>();
        settingsResearches.sTextRequestedResources.GetComponent<Text>().text = SP.Show(settings.gameSettings.RequestedResources);

        // save data about selected planet
        LoadGame.SetSelected();
    }

    /// <summary>
    /// add new resources to be transportable
    /// </summary>
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
            Input(key, PlanetAmount);
        }
    }

    // add new resource to storage by: num of resource, name of planet, amount of new resource
    public void Input(int key, settingsResearches.AcceptRes PlanetAmount)
    {
        if (settings.gameSettings.Storage.ContainsKey(key))
        {
            settings.gameSettings.Storage[key].Add(PlanetAmount);
        }
        else
        {
            List<settingsResearches.AcceptRes> value = new List<settingsResearches.AcceptRes> { PlanetAmount };
            settings.gameSettings.Storage.Add(key, value);
        }
    }

    // remove selected planet from storage
    private void RemoveFromStorage()
    {
        var keysForRemove = new List<int>();

        // remove information from Values
        foreach (var item in settings.gameSettings.Storage)
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
            settings.gameSettings.Storage.Remove(key);
        }
    }

    /// <summary>
    /// get amount of resourse "numRes" in the storage
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetAmountInStorage(int key)
    {
        int res = 0;
        if (settings.gameSettings.Storage.ContainsKey(key))
        {
            var content = settings.gameSettings.Storage[key];
            foreach (var item in content)
            {
                res += item.amount;
            }
        }
        return res;
    }

    // use 1 resource from the storage and remove empty resource
    public void TakeAwayResourceFromStorage(int numRes)
    {
        // change amount of resource at the storage
        --settings.gameSettings.Storage[numRes][0].amount;

        // change amount of resource at the planet
        int key = 1;
        int Size = settings.sNPlanets;
        bool flagPlanetFound = false;
        string NamePlanet = settings.gameSettings.Storage[numRes][0].NamePlanet;
        while ((!flagPlanetFound) && (key < Size))
        {
            //if (getItems.sPlanetProperty[key].textName == NamePlanet)
            if (settings.gameSettings.SetPlanets[key].textName == NamePlanet)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (settings.gameSettings.SetPlanets[key].ResAdd[j] == numRes)
                    { settings.gameSettings.SetPlanets[key].ResAddAmount[j]--; }
                }
                flagPlanetFound = true;
            }
            key++;
        }

        if (settings.gameSettings.Storage[numRes][0].amount == 0)
        {
            settings.gameSettings.Storage[numRes].Remove(settings.gameSettings.Storage[numRes][0]);
            // remove empty resource from the storage
            if (settings.gameSettings.Storage[numRes].Count == 0)
            { settings.gameSettings.Storage.Remove(numRes); }
        }
    }

    /// <summary>
    /// to correct all strings on the scene
    /// </summary>
    private void CorrectLanguage()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            strResearch = "Research";
            strSelect = "Select";
        }
        else
        {
            if ((PersonalSettings.language == LanguageSettings.Language.Russian))
            {
                strResearch = "ИЗУЧИТЬ";
                strSelect = "ВЫБРАТЬ";
            }
        }
    }
}