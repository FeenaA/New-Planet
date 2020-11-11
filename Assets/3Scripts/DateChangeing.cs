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

    // to operate with Shopping
    public GameObject CanvasPlanets;

    private static int nDayPF = 0;
    private static int nDaySC = 0;
    // Coins
    public Text TextCoins;
    // People
    public Text peopleOnNative;
    public GameObject peopleOnNew;
    public GameObject peopleDied;
    // button to be done active or not
    public GameObject ButtonResearchSelect;

    // button to transport people
    public GameObject ButtonSendPeople;
    // probes
    public GameObject NProbes;
    // SpaceCraft
    public GameObject NSpaceCraft;

    private readonly int DaysWithoutDeth = 20;
    public static int DayDeth = 0;
    public readonly static float koefPeopleStart = 0.1f;
    // additive to koefPeopleStart 
    public readonly static float[] DeathRate = { 0.116f, 0.113f, 0.108f, 0.104f, 0.102f, 0.100f };

    private static int DiedToday = 1;

    public static readonly int MaxCoins = 99999;
    public static readonly int MaxN = 99;

    // flag - x% of people are died
    private static bool flag50 = false;
    private static bool flag25 = false;

    private string SceneName;

    public GameObject CanvasBuildings;
    public GameObject CanvasGameOver;

    // CrawlLine script's parent
    public GameObject ImageCrawlLine;

    // prefab to show message via a string
    public GameObject MessageBox;
    private GameObject messageBox;
    // MessageBox's parent
    public Transform MainCanvas;

    // Text: +1 in 10d
    public Text TimerProbe;
    public Text TimerSC;

    private string strOn;
    private string strDied;
    private string strNew;
    private string strDay;
    private string strMoved;
    private string strFirstVictim;
    private string str50;
    private string str25;
    private string strPlusOne = "+1 / ";
    private string strDayShort;
    private string strFirstOnNew;

    void Start()
    {
        flag50 = (settings.gameSettings.NPeopleDied * 2 > settings.gameSettings.AllPeople);
        flag25 = (settings.gameSettings.NPeopleDied * 3 > settings.gameSettings.AllPeople * 2);

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
                peopleOnNative.text = strOn + settings.gameSettings.NameNative + ": " +
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
            str50 = "ПОГИБЛА УЖЕ ПОЛОВИНА ТВОЕГО НАРОДА. КОНЕЦ ПРИБЛИЖАЕТСЯ.";
            str25 = "ТВОЙ НАРОД ТАЕТ НА ГЛАЗАХ: 75% ЖИТЕЛЕЙ ПОГИБЛИ.";
            strDayShort = "Д";
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
            str50 = "A half of your people have already died. The end is coming.";
            str25 = "Your people is draining away: 75% are died.";
            strDayShort = " d";
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
                int previousMortality = settings.gameSettings.NPeopleDied;

                GetPeopleAmount();

                #region MessageBox: "People've started to die"
                if (DiedToday > 0 && previousMortality == 0)
                {
                    // show Message
                    messageBox = Instantiate(MessageBox);
                    messageBox.SendMessage("TheStart", strFirstVictim);
                    // SetParent to the MessageBox
                    messageBox.transform.SetParent(MainCanvas, false);
                }
                #endregion

                #region MessageBox: "% of people are died"
                if (!flag50)
                {
                    if (settings.gameSettings.NPeopleDied * 2 >= settings.gameSettings.AllPeople)
                    {
                        flag50 = true;
                        // show Message
                        messageBox = Instantiate(MessageBox);
                        messageBox.SendMessage("TheStart", str50);
                        // SetParent to the MessageBox
                        messageBox.transform.SetParent(MainCanvas, false);
                    }
                }
                else
                {
                    // more then 50% of people are dead 
                    if (!flag25 &&
                        settings.gameSettings.NPeopleDied * 3 >= settings.gameSettings.AllPeople * 2)
                    {
                        flag25 = true;
                        // show Message
                        messageBox = Instantiate(MessageBox);
                        messageBox.SendMessage("TheStart", str25);
                        // SetParent to the MessageBox
                        messageBox.transform.SetParent(MainCanvas, false);
                    }
                }
                #endregion
            }

            if (SceneName == "Game")
            {
                #region show people

                // died people
                if (settings.gameSettings.NDays > DaysWithoutDeth)
                {
                    // restart timer for crawl line
                    if (peopleDied.activeSelf == false) { crawlLine.TimerCrawlLine = 0; }

                    peopleDied.SetActive(true);
                    peopleDied.GetComponent<Text>().text = strDied + Convert.ToString(settings.gameSettings.NPeopleDied);
                }
                // people on the native planet
                if (!String.IsNullOrEmpty(settings.gameSettings.NameNative))
                {
                    peopleOnNative.text = strOn + settings.gameSettings.NameNative + ": " +
                        Convert.ToString(settings.gameSettings.NPeopleOnNative);
                }
                // people on new planet
                if (settings.gameSettings.flagSelectedPlanet == true)
                {
                    peopleOnNew.GetComponent<Text>().text = strOn + settings.gameSettings.NameNew + strNew +
                        Convert.ToString(settings.gameSettings.NPeopleOnNew);
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

                // Text timer: +1 in 10d
                if (SceneName == "Research")
                {
                    int timeProbes = settings.gameSettings.ProbeFactory.Time - nDayPF;
                    TimerProbe.text = strPlusOne + timeProbes + strDayShort;
                }

                if (nDayPF == settings.gameSettings.ProbeFactory.Time)
                {
                    settings.gameSettings.NProbe++;
                    if (SceneName == "Research")
                    {
                        StartCoroutine(AddShow(NProbes, Convert.ToString(settings.gameSettings.NProbe)));

                        // if sButtonResearchSelect exists, make it interactable
                        if (ButtonResearchSelect.activeSelf)
                        { ButtonResearchSelect.GetComponent<Button>().interactable = true; }
                    }
                    nDayPF = 0;
                }
                // to cope with changed ProbeFactory.Time
                else if (nDayPF > settings.gameSettings.ProbeFactory.Time) { nDayPF = 0; }
            }
            #endregion

            #region spacecrafts increment

            // spacecrafts increment
            if ((settings.gameSettings.SCfactory.N > 0) && (settings.gameSettings.NSpasecraft < MaxN))
            {
                nDaySC++;

                // Text timer: +1 in 10d
                if (SceneName == "Research")
                {
                    int timeSC = settings.gameSettings.SCfactory.Time - nDaySC;
                    TimerSC.text = strPlusOne + timeSC + strDayShort;
                }

                if (nDaySC == settings.gameSettings.SCfactory.Time)
                {
                    settings.gameSettings.NSpasecraft++;

                    // if SCs are sent automatically
                    if (settings.flagCycledSent)
                    {
                        bool flag = settings.gameSettings.flagPeopleVeBeenSent;

                        // Send the SC with people to the new planet
                        People PEP = MainCanvas.GetComponent<People>();
                        int NTransportedPeople = PEP.SendPeople(settings.gameSettings.NSpasecraft);

                        print(NTransportedPeople + " " + settings.gameSettings.NSpasecraft);

                        // the first group of people was sent
                        if (!flag && NTransportedPeople > 0)
                        {
                            // to operate with CrawlLine
                            crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
                            CL.ShowNext(strFirstOnNew + settings.gameSettings.NameNew);
                        }

                        if (SceneName == "Game")
                        {
                            // pop-up line
                            ShowPopUpLine(NTransportedPeople);
                            // show new amount of people
                            ShowPeople();
                        }
                    }

                    if (SceneName == "Research")
                    {
                        StartCoroutine(AddShow(NSpaceCraft, Convert.ToString(settings.gameSettings.NSpasecraft)));

                        // reset ability or disability of buttons
                        Shopping SH = CanvasPlanets.GetComponent<Shopping>();
                        SH.AddSC();
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
                // to note changed ProbeFactory.Time
                else if (nDaySC > settings.gameSettings.SCfactory.Time) { nDaySC = 0; }
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

    // show new amount of people
    public void ShowPeople()
    {
        peopleOnNative.text = strOn + settings.gameSettings.NameNative + ": " +
    System.Convert.ToString(settings.gameSettings.NPeopleOnNative);
        peopleOnNew.GetComponent<Text>().text = strOn +
            settings.gameSettings.NameNew + strNew +
        System.Convert.ToString(settings.gameSettings.NPeopleOnNew);
    }

    /*

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

            // to operate with CrawlLine
            crawlLine CL = ImageCrawlLine.GetComponent<crawlLine>();
            CL.ShowNext(strFirstOnNew + settings.gameSettings.NameNew);
        }

        // NTransportedPeople - amount of avaliable people
        int NTransportedPeople = People.NPeopleInSC * NSpacecraft;
        if (settings.gameSettings.NPeopleOnNative < NTransportedPeople)
        { NTransportedPeople = settings.gameSettings.NPeopleOnNative; }
        NSpacecraft = NTransportedPeople / People.NPeopleInSC;
        if (NSpacecraft * People.NPeopleInSC < NTransportedPeople) NSpacecraft++;

        // change amount of people on new and native planets
        settings.gameSettings.NSpasecraft -= NSpacecraft;
        settings.gameSettings.NPeopleOnNative -= NTransportedPeople;
        settings.gameSettings.NPeopleOnNew += NTransportedPeople;
        settings.gameSettings.CurrentPerSent = settings.gameSettings.NPeopleOnNew * 100 / settings.gameSettings.AllPeople;

        // Save NSpasecraft, amount of people on new and native planets
        LoadGame.SetPeopleTransport();

        return NTransportedPeople;
    }*/

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
        settings.gameSettings.koefToday += DeathRate[settings.gameSettings.Hospital.N];
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