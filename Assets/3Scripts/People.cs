using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class People : MonoBehaviour
{
    // buttons to transport people
    public GameObject ButtonSendPeople;
    public GameObject ButtonCycle;
    // pop-up line 
    public GameObject PopUpLine;
    // People
    public GameObject peopleOnNative;
    public GameObject peopleOnNew;
    // Images for button
    public Sprite CycleImage;  
    public Sprite NotCycleImage;
    
    private static string strOn = "";
    private static string strNew = "";

    public GameObject TextDate;
     
    public readonly int NPeopleInSC = 1000;

    /// <summary>
    /// settings of visible or unvisible buttons
    /// </summary>
    void Start()
    {
        CorrectLanguage();

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
    }

    private void CorrectLanguage()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            strOn = "On ";
            strNew = " (new): ";
        }
        else
        {
            if ((PersonalSettings.language == LanguageSettings.Language.Russian))
            {
                strOn = "НА ";
                strNew = " (НОВАЯ): ";
            }
        }
    }

    /// <summary>
    /// Send 1 SC with 1ooo people in it to the new planet
    /// </summary>
    public void PressSendPeople()
    {
        if (settings.gameSettings.NSpasecraft == 0) return;

        // Send 1 Spacecraft with people
        DateChangeing DC = TextDate.GetComponent<DateChangeing>();
        DC.SendPeople(1);
        // show amount of people
        ShowPeople();
        // if there is no more SCs, make ButtonSendPeople uninteractable
        if (settings.gameSettings.NSpasecraft == 0)
        { ButtonSendPeople.GetComponent<Button>().interactable = false; }
    }

    /*
    /// <summary>
    /// Send NSpacecraft SC
    /// </summary>
    /// <param name="NSpacecraft"></param>
    public void SendPeople(int NSpacecraft)
    {
        // NSpacecraft - amount of avaliable SCs
        if (settings.gameSettings.NSpasecraft < NSpacecraft)
        { NSpacecraft = settings.gameSettings.NSpasecraft; }
        if (NSpacecraft == 0) return;

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

        // Save NSpasecraft, amount of people on new and native planets
        LoadGame.SetPeopleTransport();

        // pop-up line
        ShowPopUpLine(NTransportedPeople);
    }*/

    /// <summary>
    /// show amount of people
    /// </summary>
    private void ShowPeople()
    {
        peopleOnNative.GetComponent<Text>().text = strOn + settings.gameSettings.NameNative + ": " +
            System.Convert.ToString(settings.gameSettings.NPeopleOnNative);
        peopleOnNew.GetComponent<Text>().text = strOn + settings.gameSettings.NameNew + strNew +
            System.Convert.ToString(settings.gameSettings.NPeopleOnNew);
    }

   /* /// <summary>
    /// Show pop-up line
    /// </summary>
    /// <param name="NTransportedPeople">Amount ot transported people</param>
    private void ShowPopUpLine(int NTransportedPeople)
    {
        if (!PopUpLine.activeSelf)
        {
            PopUpLine.GetComponent<Text>().text = System.Convert.ToString(NTransportedPeople) + strMoved;
            PopUpLine.SetActive(true);
            StartCoroutine(MakeSleepObject());
        }
    }

    IEnumerator MakeSleepObject()
    {
        yield return new WaitForSeconds(2.0f);
        if (PopUpLine.activeSelf)
        {
            PopUpLine.SetActive(false);
        }
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
            DateChangeing DC = TextDate.GetComponent<DateChangeing>();
            DC.SendPeople(settings.gameSettings.NSpasecraft);
            // show amount of people
            ShowPeople();
        }
        else { ButtonCycle.GetComponent<Image>().sprite = CycleImage; }
    }
}
