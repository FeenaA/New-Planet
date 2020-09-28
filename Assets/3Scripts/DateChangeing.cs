using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Колесо Сансары дало оборот
/// </summary>
public class DateChangeing : MonoBehaviour
{
    // the main pause flag
    public static bool pause = false;
    // number of seconds between days counter increment
    public int nSecondsStep = 2;
    public string result;
    public GameObject PauseRectangle;

    private static int nDayPF = 0;
    private static int nDaySC = 0;
    // Coins
    public GameObject textCoinsObject;
    public static GameObject sTextCoinsObject;
    // People
    public GameObject peopleOnNative;
    public GameObject peopleOnNew;
    public GameObject peopleDied;
    // button to transport people
    public GameObject ButtonSendPeople;
    // probes
    public GameObject NProbes; 
    // SpaceCraft
    public GameObject NSpaceCraft; 

    private readonly int DaysWithoutDeth = 10;
    public static int DayDeth = 0;
    public readonly static float koefPeopleStart = 0.1f;
    private static int DiedToday = 1;

    public static readonly int MaxCoins = 99999;
    public static readonly int MaxN = 99;

    private string SceneName;

    private static string strOn = "";
    private static string strDied = "";
    private static string strNew = "";
    private static string strDay = "";

    void Start()
    {
        if (pause)
        {
            gameObject.GetComponent<Text>().color = buttons.sColorPause;
            textCoinsObject.GetComponent<Text>().color = buttons.sColorPause;
            PauseRectangle.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Text>().color = buttons.sColorProcess;
            textCoinsObject.GetComponent<Text>().color = buttons.sColorProcess;
            PauseRectangle.SetActive(false);
        }

        sTextCoinsObject = textCoinsObject;
        GetComponent<Text>().text = settings.sStringTextDays;
        sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(settings.gameSettings.NCoins);

        if (SceneName == "Research")
        {
            NProbes.GetComponent<Text>().text = Convert.ToString(settings.gameSettings.NProbe);
            NSpaceCraft.GetComponent<Text>().text = Convert.ToString(settings.gameSettings.NSpasecraft);
        }

        // the name of the current scene
        SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Game")
        {
            // people on the native planet
            if (!String.IsNullOrEmpty(settings.gameSettings.NameNative))
            {
                peopleOnNative.GetComponent<Text>().text = strOn + settings.gameSettings.NameNative + ": " +
                    Convert.ToString(settings.gameSettings.NPeopleOnNative);
            }
            // Does the selected planet exist?
            if (settings.gameSettings.flagSelectedPlanet == true) { peopleOnNew.SetActive(true); }
        }

        CorrectLanguage();
        //if (!pause) { 
            InvokeRepeating(nameof(ChangeData), 0, nSecondsStep); 
        //}
    }

    private void CorrectLanguage()
    {
        if (PersonalSettings.language == LanguageSettings.Language.English)
        {
            strDay = "Day ";
            strOn = "On ";
            strDied = "Died: ";
            strNew = " (new): ";
        }
        else
        {
            if ((PersonalSettings.language == LanguageSettings.Language.Russian))
            {
                strDay = "ДЕНЬ ";
                strOn = "НА ";
                strDied = "ПОГИБЛО: ";
                strNew = " (НОВАЯ): ";
            }
        }
    }

    /// <summary>
    /// update daily information
    /// </summary>
    private void ChangeData()
    {
        if (!pause)
        {
            // days counter increment 
            settings.sStringTextDays = strDay + settings.gameSettings.NDays;
            GetComponent<Text>().text = settings.sStringTextDays;

            // increase and show amount of coins
            settings.gameSettings.NCoins = AddCoins(settings.gameSettings.stepCoins);
            sTextCoinsObject.GetComponent<Text>().text = Convert.ToString(settings.gameSettings.NCoins);

            // if people've started to die
            if (settings.gameSettings.NDays > DaysWithoutDeth) { GetPeopleAmount(); }

            if (SceneName == "Game")
            {
                // died people
                if (settings.gameSettings.NDays > DaysWithoutDeth)
                {
                    peopleDied.SetActive(true);
                    peopleDied.GetComponent<Text>().text = strDied + Convert.ToString(settings.gameSettings.NPeopleDied);
                }
                // people on the native planet
                if (!String.IsNullOrEmpty(settings.gameSettings.NameNative))
                {
                    peopleOnNative.GetComponent<Text>().text = strOn + settings.gameSettings.NameNative + ": " +
                        Convert.ToString(settings.gameSettings.NPeopleOnNative);
                }
                // people on new planet
                if (settings.gameSettings.flagSelectedPlanet == true)
                {
                    peopleOnNew.GetComponent<Text>().text = strOn + settings.gameSettings.NameNew + strNew +
                        Convert.ToString(settings.gameSettings.NPeopleOnNew);
                }
                // buildings
                if (EarthOnClick.flagBuildings == true)
                {
                    BuildingsOperations BO = settings.sCanvasBuildings.GetComponent<BuildingsOperations>();
                    BO.ReloadButtons();
                }
            }

            // probes increment
            if ((settings.gameSettings.ProbeFactory.N > 0) && (settings.gameSettings.NProbe < MaxN))
            {
                nDayPF++;
                if (nDayPF == settings.gameSettings.ProbeFactory.Time)
                {
                    settings.gameSettings.NProbe++;
                    if (SceneName == "Research")
                    { 
                        StartCoroutine(AddShow(NProbes, Convert.ToString(settings.gameSettings.NProbe)));
                        // if sButtonResearchSelect exists and is not interactable, make it interactable
                        if (settingsResearches.sButtonResearchSelect.activeSelf &&
                            !settingsResearches.sButtonResearchSelect.GetComponent<Button>().interactable)
                        { settingsResearches.sButtonResearchSelect.GetComponent<Button>().interactable = true; } 
                    }
                    nDayPF = 0;
                }
            }

            // spacecrafts increment
            if ((settings.gameSettings.SCfactory.N > 0) && (settings.gameSettings.NSpasecraft < MaxN))
            {
                nDaySC++;
                if (nDaySC == settings.gameSettings.SCfactory.Time)
                {
                    settings.gameSettings.NSpasecraft++;
                    if (SceneName == "Research")
                    { 
                        StartCoroutine(AddShow(NSpaceCraft, Convert.ToString(settings.gameSettings.NSpasecraft)));
                        // operate with Shopping
                        if ((Shopping.sPanelShopping.activeSelf) && (Shopping.NRes < 10) )
                        {
                            // make buttonTransport active
                            Transform PanelButtons = Shopping.sPanelShopping.transform.Find("Buttons");
                            Transform ButtonTransport = PanelButtons.Find("ButtonTransport");
                            ButtonTransport.GetComponent<Button>().interactable = true;
                        }
                    }
                    else if (SceneName == "Game")
                    {
                        if (settings.gameSettings.flagPeopleTransport)
                        {
                            ButtonSendPeople.GetComponent<Button>().interactable = true;
                        }
                    }
                    nDaySC = 0;
                }
            }

            // save new daily params 
            LoadGame.SetDailyData();

            // day increment
            settings.gameSettings.NDays++;
        }
    }

    public void AddSC(GameObject NSpaceCraft)
    {
        if (SceneName == "Research") 
        { StartCoroutine(AddShow(NSpaceCraft, Convert.ToString(settings.gameSettings.NSpasecraft)));}
    }

    /// <summary>
    /// show updated amount of probes or spacsecrafts
    /// </summary>
    /// <param name="GO">GameObject with a text to be changed</param>
    /// <param name="strN">string to renew GO's text</param>
    /// <returns></returns>
    private IEnumerator AddShow(GameObject GO, string strN)
    {
        // periodical spiking of the title
        float timePulse = 0.5f;
        GO.GetComponent<Text>().fontStyle = FontStyle.Bold;
        yield return new WaitForSeconds(timePulse);
        GO.GetComponent<Text>().text = strN;
        yield return new WaitForSeconds(timePulse);
        GO.GetComponent<Text>().fontStyle = FontStyle.Normal;
    }

    /// <summary>
    /// get NPeopleDied, NPeopleOnNative and make actual GOs active
    /// </summary>
    private void GetPeopleAmount()
    {
        DayDeth++;
        settings.gameSettings.koefToday += 0.112f;
        DiedToday = DayDeth * System.Convert.ToInt32(settings.gameSettings.koefToday);
        settings.gameSettings.NPeopleDied += DiedToday;
        settings.gameSettings.NPeopleOnNative -= DiedToday;
    }

    /// <summary>
    /// clever coins increment
    /// </summary>
    /// <param name="N">amount of additional coins</param>
    /// <returns></returns>
    public int AddCoins(int N)
    {
        int res = settings.gameSettings.NCoins;
        if (res + N > MaxCoins) { res = MaxCoins;  }
        else                    { res += N;}
        return res;
    }
}

