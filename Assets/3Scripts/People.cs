using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class People : MonoBehaviour
{
    // buttons to transport people
    public GameObject ButtonSendPeople;
    public GameObject ButtonCycle;
    // People
    public GameObject peopleOnNative;
    public GameObject peopleOnNew;
    // Images for button
    public Sprite CycleImage;  
    public Sprite NotCycleImage;
    
    private static string strOn = "";
    private static string strNew = "";

    private static readonly int NPeopleInSC = 1000;

    /// <summary>
    /// settings of visible or unvisible buttons
    /// </summary>
    void Start()
    {
        // to show buttons to transport people
        if (settings.gameSettings.flagPeopleTransport)
        {
            if (settings.gameSettings.NSpasecraft > 0 )
            { ButtonSendPeople.GetComponent<Button>().interactable = true; }
            else { ButtonSendPeople.GetComponent<Button>().interactable = false; }
            ButtonSendPeople.SetActive(true);

            if (settings.flagCycledSent)
            { ButtonCycle.GetComponent<Image>().sprite = NotCycleImage; }
            else { ButtonCycle.GetComponent<Image>().sprite = CycleImage; }
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

    public void PressSendPeople()
    {
        if (settings.gameSettings.NSpasecraft == 0) return;

        // NSpasecraft decrement
        settings.gameSettings.NSpasecraft--;

        // the last SC was used
        if (settings.gameSettings.NSpasecraft == 0)
        { ButtonSendPeople.GetComponent<Button>().interactable = false; }

        // change amount of people on new and native planets
        int NTransportedPeople = NPeopleInSC;
        if (settings.gameSettings.NPeopleOnNative < NTransportedPeople)
        { NTransportedPeople = settings.gameSettings.NPeopleOnNative; }
        settings.gameSettings.NPeopleOnNative -= NTransportedPeople;
        settings.gameSettings.NPeopleOnNew += NTransportedPeople;

        // show these changes
        peopleOnNative.GetComponent<Text>().text = strOn + settings.gameSettings.NameNative + ": " +
                System.Convert.ToString(settings.gameSettings.NPeopleOnNative);
        peopleOnNew.GetComponent<Text>().text = strOn + settings.gameSettings.NameNew + strNew +
                System.Convert.ToString(settings.gameSettings.NPeopleOnNew);

        // Save NSpasecraft, amount of people on new and native planets
        LoadGame.SetPeopleTransport();
    }

    public void PressAutomatically()
    {
        settings.flagCycledSent = !settings.flagCycledSent;
        if (settings.flagCycledSent)
        { ButtonCycle.GetComponent<Image>().sprite = NotCycleImage; }
        else { ButtonCycle.GetComponent<Image>().sprite = CycleImage; }
        ButtonCycle.SetActive(true);
    }
}
