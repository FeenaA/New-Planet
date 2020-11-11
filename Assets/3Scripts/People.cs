using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class People : MonoBehaviour
{
    // buttons to transport people
    public GameObject ButtonSendPeople;
    public GameObject ButtonCycle;
    // date
    public GameObject Date;
    // pop-up line 
    public GameObject PopUpLine;
    // People
    public GameObject peopleOnNative;
    public GameObject peopleOnNew;
    // Images for button
    public Sprite CycleImage;  
    public Sprite NotCycleImage;
    /*
    private string strOn;
    private string strNew;*/


    private readonly int NPeopleInSC = 1000;

    /// <summary>
    /// settings of visible or unvisible buttons
    /// </summary>
 /*   void Start()
    {
        //CorrectLanguage();

        // to show buttons to transport people
        if (settings.gameSettings.flagPeopleTransport)
        {
            ButtonSendPeople.GetComponent<Button>().interactable = false;
            if ( !settings.flagCycledSent )
            {
                ButtonCycle.GetComponent<Image>().sprite = CycleImage;
                if (settings.gameSettings.NSpasecraft > 0 )
                { ButtonSendPeople.GetComponent<Button>().interactable = true; }
            }
            else { ButtonCycle.GetComponent<Image>().sprite = NotCycleImage; }

            ButtonSendPeople.SetActive(true);
            ButtonCycle.SetActive(true);
        }
    }*/

    /*private void CorrectLanguage()
    {
        if ((PersonalSettings.language == LanguageSettings.Language.Russian))
        {
            strOn = "НА ";
            strNew = " (НОВАЯ): ";
        }
        else
        {
            strOn = "On ";
            strNew = " (new): ";
        }
    }*/

    /// <summary>
    /// Send 1 SC with 1ooo people in it to the new planet
    /// </summary>
    public void PressSendPeople()
    {
        if (settings.gameSettings.NSpasecraft == 0) return;

        // Send 1 Spacecraft with people
        SendPeople(1);
        // show amount of people
        DateChangeing DC = Date.GetComponent<DateChangeing>();
        DC.ShowPeople();
        // if there is no more SCs, make ButtonSendPeople uninteractable
        if (settings.gameSettings.NSpasecraft == 0)
        { ButtonSendPeople.GetComponent<Button>().interactable = false; }
    }
    
    /// <summary>
    /// show amount of people
    /// </summary>
    /*private void ShowPeople()
    {
        peopleOnNative.GetComponent<Text>().text = strOn + settings.gameSettings.NameNative + ": " +
            System.Convert.ToString(settings.gameSettings.NPeopleOnNative);
        peopleOnNew.GetComponent<Text>().text = strOn + settings.gameSettings.NameNew + strNew +
            System.Convert.ToString(settings.gameSettings.NPeopleOnNew);
    }*/

    /// <summary>
    /// keeping track of producing new SC to send them with people to the new planet
    /// </summary>
    public void PressAutomatically()
    {
        // change Auto-status
        settings.flagCycledSent = !settings.flagCycledSent;

        // Cycled is on
        if (settings.flagCycledSent)
        {
            // change an image on the ButtonCycle
            ButtonCycle.GetComponent<Image>().sprite = NotCycleImage;
            ButtonSendPeople.GetComponent<Button>().interactable = false;
            // Send all possible SCs with people to the planet
            SendPeople(settings.gameSettings.NSpasecraft);
            // show amount of people
            DateChangeing DC = Date.GetComponent<DateChangeing>();
            DC.ShowPeople();
        }
        else { ButtonCycle.GetComponent<Image>().sprite = CycleImage; }
    }

    /// <summary>
    /// Send NSpacecraft SC
    /// </summary>
    /// <param name="NSpacecraft"></param>
    /// <returns>amount of sent people</returns>
    public int SendPeople(int NSpacecraft)
    {
        // if (SceneName == "Game")
        if (settings.gameSettings.NPeopleOnNative == 0) return 0;

        // NSpacecraft - amount of avaliable SCs
        if (settings.gameSettings.NSpasecraft < NSpacecraft)
        { NSpacecraft = settings.gameSettings.NSpasecraft; }
        if (NSpacecraft == 0) return 0;

        // the first group of people was sent
        if (!settings.gameSettings.flagPeopleVeBeenSent)
        {
            settings.gameSettings.flagPeopleVeBeenSent = true;
            LoadGame.SetPeopleVeBeenSent();
        }

        // NTransportedPeople - amount of avaliable people
        int NTransportedPeople = NPeopleInSC * NSpacecraft;
        if (settings.gameSettings.NPeopleOnNative < NTransportedPeople)
        { NTransportedPeople = settings.gameSettings.NPeopleOnNative; }
        NSpacecraft = NTransportedPeople / NPeopleInSC;
        if (NSpacecraft * NPeopleInSC < NTransportedPeople) NSpacecraft++;

        // change amount of people on new and native planets
        settings.gameSettings.NSpasecraft -= NSpacecraft;
        settings.gameSettings.NPeopleOnNative -= NTransportedPeople;
        settings.gameSettings.NPeopleOnNew += NTransportedPeople;
        settings.gameSettings.CurrentPerSent = settings.gameSettings.NPeopleOnNew * 100 / settings.gameSettings.AllPeople;

        // Save NSpasecraft, amount of people on new and native planets
        LoadGame.SetPeopleTransport();

        return NTransportedPeople;
    }
}
