using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class panelInform : MonoBehaviour
{
    // amount of units
    public Text TextProbes;
    public Text TextEth;

    public GameObject TextDate;
    // MB to show congratulations
    public GameObject MessageBox;
    // MB to note that a probe was crushed
    public GameObject GeneralMessageBox;
    
    public Transform MainCanvas;
    public Text TextCoins;
    public Transform[] ResAdd; 
    // button 
    public GameObject ButtonResearchSelect;
    private string strResearch;
    private string strSelect;
    // planet information
    public Renderer Sphere;
    public Text NamePlanet;
    public Text TextIntro;
    // text for requested resources
    public Text TextRequestedResources;

    // buttons for necessary resources
    public Transform[] resNecess;
     
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
        Sphere.material = getItems.sMaterials[PP.numMaterial];
        NamePlanet.text = PP.textName;
        TextIntro.text = getItems.sIntroduction[PP.numIntro];

        //show necessary resources
        for (int i = 0; i < 3; i++)
        {
            resNecess[i].GetComponentInChildren<Text>().text =
                getItems.ResNess[i-3].name + " = " + PP.ResNess_Amount[i];
        }

        // extraordinary resources
        if (PP.flagIsResearched == false)
        {
            // planet isn't researched
            ButtonResearchSelect.SetActive(true);
            ButtonResearchSelect.GetComponentInChildren<Text>().text = strResearch;

            if (settings.gameSettings.NProbe == 0)
            {
                print("Probe: " + settings.gameSettings.NProbe); 
                ButtonResearchSelect.GetComponent<Button>().interactable = false; 
            }
            else
            {
                ButtonResearchSelect.GetComponent<Button>().interactable = true;
            }

            //foreach (var resource in settingsResearches.r)
            foreach (var resource in ResAdd)
            {
                resource.GetComponentInChildren<Text>().text = "-";
            }
        }
        else // planet is researched
        {
            // dealing with Research/Select Button
            if (settings.gameSettings.flagSelectedPlanet == false)
            {
                ButtonResearchSelect.SetActive(true);
                ButtonResearchSelect.GetComponent<Button>().interactable = true;
                ButtonResearchSelect.GetComponentInChildren<Text>().text = strSelect;
            }
            else { ButtonResearchSelect.SetActive(false); }

            for (int i = 0; i < 3; i++)
            {
                int amount = PP.ResAddAmount[i];
                if (amount > 0)
                {
                    // not empty slot
                    ResAdd[i].GetComponentInChildren<Text>().text =
                        getItems.ResourceAdd[PP.ResAdd[i]].name + " = " + PP.ResAddAmount[i];
                }
                else
                {
                    // empty slot
                    ResAdd[i].GetComponentInChildren<Text>().text = "-";
                    // let free current resource slot 
                    PP.ResAdd[i] = 0;
                }
            }
        }
    }

    /// <summary>
    /// learn more about this planet
    /// </summary>
    public void ResearchPressed()
    {
        // if researching is avaliable
        if (!ItemOnClick.PP.flagIsResearched)
        {
            // there is a probe to be sent to the current new planet
            if (settings.gameSettings.NProbe > 0)
            {
                // Probe decrement
                settings.gameSettings.NProbe--;
                TextProbes.text = System.Convert.ToString(settings.gameSettings.NProbe);

                // successful researches
                if (ItemOnClick.PP.amountProbes == 0)
                {
                    if (settings.gameSettings.flagSelectedPlanet == false)
                    { ButtonResearchSelect.GetComponentInChildren<Text>().text = strSelect; }
                    else
                    { ButtonResearchSelect.SetActive(false); }

                    ItemOnClick.PP.flagIsResearched = true;

                    // show resources
                    for (int i = 0; i < 3; i++)
                    {
                        ResAdd[i].GetComponentInChildren<Text>().text =
                            getItems.ResourceAdd[ItemOnClick.PP.ResAdd[i]].name + " = " + ItemOnClick.PP.ResAddAmount[i];
                    }

                    // deal with Ether
                    if (ItemOnClick.PP.flagEther)
                    {
                        if (settings.gameSettings.NEther + 1 <= DateChangeing.MaxN)
                        {
                            settings.gameSettings.NEther++;
                            // show changes
                            TextEth.text = System.Convert.ToString(settings.gameSettings.NEther);

                            // show Congratulations
                            var instance = Instantiate(MessageBox);
                            instance.SendMessage("TheStart", false);
                            instance.transform.SetParent(MainCanvas, false);

                            bool LoadStorage = false;
                            LoadGame.SetEther(LoadStorage);
                        }
                    }

                    // deal with Coins
                    if (ItemOnClick.PP.flagCoins)
                    {
                        DateChangeing DC = TextDate.GetComponent<DateChangeing>();
                        settings.gameSettings.NCoins = DC.AddCoins(ItemOnClick.PP.amountCoins);
                        // show changes
                        TextCoins.text = System.Convert.ToString(settings.gameSettings.NCoins);

                        // show Congratulations
                        var instance = Instantiate(MessageBox);
                        instance.SendMessage("TheStart", true);
                        instance.transform.SetParent(MainCanvas, false);
                    }

                    // update resources at storage
                    AddToStorage();
                    // set flagResearched=true, --probes, resources at storage
                    LoadGame.SetResearched(ItemOnClick.PP.textName);
                }
                else // the probe has failed researches
                {
                    if ( ItemOnClick.PP.amountProbes > 0 ) { ItemOnClick.PP.amountProbes--; }
                    else { ItemOnClick.PP.amountProbes = 0; }

                    #region MessageBox: your probe crushed
                    var messageBox = Instantiate(GeneralMessageBox);
                    string strProbeCrush;
                    if (PersonalSettings.language == LanguageSettings.Language.Russian)
                    { strProbeCrush = "ПЛАНЕТУ ИЗУЧИТЬ НЕ УДАЛОСЬ. ЗОНД ПОТЕРПЕЛ КРУШЕНИЕ, НЕ ЗАВЕРШИВ ИССЛЕДОВАНИЕ."; }
                    else { strProbeCrush = "The planet research failed. The probe crashed without completing the study."; }
                    messageBox.SendMessage("TheStart", strProbeCrush);
                    // SetParent to the MessageBox
                    messageBox.transform.SetParent(MainCanvas, false);
                    #endregion

                    LoadGame.SetResearchIsFailed(ItemOnClick.PP.textName);
                }
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
        ButtonResearchSelect.SetActive(false);

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

        // count CurrentNResUnits
        settings.gameSettings.CurrentNResUnits = 0;
        foreach (var item in settings.gameSettings.RequestedResources.Values)
        { settings.gameSettings.CurrentNResUnits += item; }

        ShowProgress SP = GetComponent<ShowProgress>();
        TextRequestedResources.text = SP.Show(settings.gameSettings.RequestedResources);

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
    public string TakeAwayResourceFromStorage(int numRes)
    {
        // change amount of resource at the storage
        --settings.gameSettings.Storage[numRes][0].amount;

        // change amount of resource at the planet
        int key = 1;
        int Size = settings.sNPlanets;
        bool flagPlanetFound = false;
        // NamePlanet at storage with a resource. We're looking for this planet
        string NamePlanet = settings.gameSettings.Storage[numRes][0].NamePlanet;
        while ((!flagPlanetFound) && (key < Size))
        {
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

        return NamePlanet;
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