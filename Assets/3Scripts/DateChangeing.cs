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
    public GameObject PopUpLine;

    private static int nDayPF = 0;
    private static int nDaySC = 0;
    // Coins
    public Text TextCoins; 
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

    private readonly int DaysWithoutDeth = 20;
    public static int DayDeth = 0; 
    public readonly static float koefPeopleStart = 0.1f;
    private static int DiedToday = 1;

    private readonly int DaysWithoutBuildings = 5;
    private static bool flagMBoxBuildings = false;

    public static readonly int MaxCoins = 99999;
    public static readonly int MaxN = 99;

    private string SceneName;

    public GameObject CanvasBuildings;
    public GameObject CanvasGameOver;

    // CrawlLine script's parent
    public GameObject ImageCrawlLine;

    // prefab to show message via a string
    public GameObject MessageBox;
    private static bool flagFirstVictim = false;
    private GameObject messageBox;
    // MessageBox's parent
    public Transform MainCanvas;

    private static string strOn = "";
    private static string strDied = "";
    private static string strNew = "";
    private static string strDay = "";
    private static string strMoved = "";
    private static string strFirstVictim = "";
    private static string strNoBuildings = "";
    private static string strPlanet = "";
    private static string strFirstOnNew = "";

    void Start()
    {
        if (pause)
        {
            gameObject.GetComponent<Text>().color = buttons.sColorPause;
            TextCoins.color = buttons.sColorPause;
            PauseRectangle.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Text>().color = buttons.sColorProcess;
            TextCoins.color = buttons.sColorProcess;
            PauseRectangle.SetActive(false);
        }

        GetComponent<Text>().text = settings.sStringTextDays;
        TextCoins.text = Convert.ToString(settings.gameSettings.NCoins);

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
        InvokeRepeating(nameof(ChangeData), 0, nSecondsStep); 
    }

    private void CorrectLanguage()
    {
        if ((PersonalSettings.language == LanguageSettings.Language.Russian))
        {
            strDay = "ДЕНЬ ";
            strOn = "НА ";
            strDied = "ПОГИБЛО: ";
            strNew = " (НОВАЯ): ";
            strMoved = " ПЕРЕЕХАЛИ";
            strFirstVictim = "ПЕРВАЯ ЖЕРТВА! ВИРУС НАЧАЛ УБИВАТЬ ТВОЙ НАРОД.";
            strPlanet = "ПЛАНЕТА ";
            strNoBuildings = " ЗАРАЖЕНА ВИРУСОМ! СТРОЙ ЗДАНИЯ, ЧТОБЫ ИСКАТЬ НОВУЮ ПЛАНЕТУ. ДЛЯ ЭТОГО НАЖМИ НА ";
            strFirstOnNew = "ПЕРВАЯ ГРУППА ЛЮДЕЙ УСПЕШНО ВЫСАДИЛАСЬ НА ПЛАНЕТУ ";
        }
        else
        {
            strDay = "Day ";
            strOn = "On ";
            strDied = "Died: ";
            strNew = " (new): ";
            strMoved = " shifted";
            strFirstVictim = "The first victim has appeared!\nThe virus has begun to kill your people.";
            strPlanet = "The planet ";
            strNoBuildings = " is infected with a virus. Build buildings to find a new planet. Tab to ";
            strFirstOnNew = "The first group of people successfully landed on the planet ";
        }
    }

    /// <summary>
    /// update daily information
    /// </summary>
    private void ChangeData()
    {
        if (!pause)
        {
            #region  days increment 
            settings.sStringTextDays = strDay + settings.gameSettings.NDays;
            GetComponent<Text>().text = settings.sStringTextDays;
            #endregion

            #region coins increment
            settings.gameSettings.NCoins = AddCoins(settings.gameSettings.stepCoins);
            TextCoins.text = Convert.ToString(settings.gameSettings.NCoins);
            #endregion

            // if people've started to die
            if (settings.gameSettings.NDays > DaysWithoutDeth) 
            {
                GetPeopleAmount();

                #region MessageBox: "People've started to die"
                if (!flagFirstVictim && DiedToday>0)
                {
                    // show Message
                    messageBox = Instantiate(MessageBox);
                    messageBox.SendMessage("TheStart", strFirstVictim);
                    // SetParent to the MessageBox
                    messageBox.transform.SetParent(MainCanvas, false);

                    flagFirstVictim = true;
                }
                #endregion
            }

            if (SceneName == "Game")
            {
                #region show people

                // died people
                if (settings.gameSettings.NDays > DaysWithoutDeth)
                {
                    if (peopleDied.activeSelf == false)
                    {
                        crawlLine.RestartTimer();
                    }

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
                #endregion

                #region MessageBox: "Build buildings!"
                bool NoBuildings = ((settings.gameSettings.ProbeFactory.N == 0) &&
                    (settings.gameSettings.Mine.N == 0) &&
                    (settings.gameSettings.Hospital.N == 0) &&
                    (settings.gameSettings.SCfactory.N == 0));
                if (!flagMBoxBuildings && 
                    (settings.gameSettings.NDays > DaysWithoutBuildings) &&
                    NoBuildings)
                {
                    // show Message
                    messageBox = Instantiate(MessageBox);
                    string strMessage = strPlanet + settings.gameSettings.NameNative + strNoBuildings + 
                        settings.gameSettings.NameNative + ".";
                    messageBox.SendMessage("TheStart", strMessage);
                    // SetParent to the MessageBox
                    messageBox.transform.SetParent(MainCanvas, false);

                    flagMBoxBuildings = true;
                }
                #endregion

                #region show buildings 
                if (EarthOnClick.flagBuildings == true)
                {
                    BuildingsOperations BO = CanvasBuildings.GetComponent<BuildingsOperations>();
                    BO.ReloadButtons();
                }
                #endregion
            }

            #region probes increment
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
            #endregion

            #region spacecrafts increment

            // spacecrafts increment
            if ((settings.gameSettings.SCfactory.N > 0) && (settings.gameSettings.NSpasecraft < MaxN))
            {
                nDaySC++;
                if (nDaySC == settings.gameSettings.SCfactory.Time)
                {
                    settings.gameSettings.NSpasecraft++;

                    // if SCs are sent automatically
                    if ( settings.flagCycledSent )
                    {
                        // Send the SC with people to the new planet
                        SendPeople(settings.gameSettings.NSpasecraft);
                        peopleOnNative.GetComponent<Text>().text = strOn + 
                            settings.gameSettings.NameNative + ": " + 
                            System.Convert.ToString(settings.gameSettings.NPeopleOnNative);
                        peopleOnNew.GetComponent<Text>().text = strOn +
                            settings.gameSettings.NameNew + strNew +
                            System.Convert.ToString(settings.gameSettings.NPeopleOnNew);
                    }

                    if (SceneName == "Research")
                    { 
                        StartCoroutine(AddShow(NSpaceCraft, Convert.ToString(settings.gameSettings.NSpasecraft)));
                        // operate with Shopping
                        int CurrentAmount = Shopping.NRes;
                        if ((Shopping.sPanelShopping.activeSelf) && (CurrentAmount < 10) && (CurrentAmount > 0))
                        {
                            // make buttonTransport active
                            Transform PanelButtons = Shopping.sPanelShopping.transform.Find("Buttons");
                            Transform ButtonTransport = PanelButtons.Find("ButtonTransport");
                            ButtonTransport.GetComponent<Button>().interactable = true;
                        }
                    }
                    else if (SceneName == "Game")
                    {
                        if (!settings.flagCycledSent && settings.gameSettings.flagPeopleTransport)
                        {
                            ButtonSendPeople.GetComponent<Button>().interactable = true;
                        }
                    }
                    nDaySC = 0;
                }
            }
            // if the selected planet exists and there is al list one SpaceCraft
            if (SceneName == "Game" &&
                settings.gameSettings.flagSelectedPlanet == true && 
                settings.gameSettings.NSpasecraft > 0)
            {
                ButtonSendPeople.GetComponent<Button>().interactable = true;
            }
            #endregion

            // save new daily params 
            LoadGame.SetDailyData();

            // day increment
            settings.gameSettings.NDays++;
        }
    }

    /// <summary>
    /// Send NSpacecraft SC
    /// </summary>
    /// <param name="NSpacecraft"></param>
    public void SendPeople(int NSpacecraft)
    {
        if (settings.gameSettings.NPeopleOnNative == 0) return;

        People PEP = MainCanvas.GetComponent<People>();
        int NPeopleInSC = PEP.NPeopleInSC;

        // NSpacecraft - amount of avaliable SCs
        if (settings.gameSettings.NSpasecraft < NSpacecraft)
        { NSpacecraft = settings.gameSettings.NSpasecraft; }
        if (NSpacecraft == 0) return;

        // the first group of people was sent
        if (!settings.gameSettings.flagPeopleVeBeenSent)
        {
            settings.gameSettings.flagPeopleVeBeenSent = true;
            LoadGame.SetPeopleVeBeenSent();

            // to operate with CrawlLine
            crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
            CL.ShowNext(strFirstOnNew + settings.gameSettings.NameNew);
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

        // pop-up line
        ShowPopUpLine(NTransportedPeople);
    }

    /// <summary>
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

        // to prevent negative amount of people
        if (DiedToday > settings.gameSettings.NPeopleOnNative)
        { DiedToday = settings.gameSettings.NPeopleOnNative; }

        settings.gameSettings.NPeopleDied += DiedToday;
        settings.gameSettings.NPeopleOnNative -= DiedToday;

        #region realization of "Game over"
        if ( settings.gameSettings.NPeopleOnNative <= 0 )
        {
            // stop date increment
            pause = true;
            // flag - session is finished
            settings.flagIsFinished = true;
            // failure
            if (settings.gameSettings.NPeopleOnNew <= 0)
            { settings.flagIsWin = false; }
            else // winning
            { settings.flagIsWin = true; }
            Instantiate(CanvasGameOver);
        }
        #endregion
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