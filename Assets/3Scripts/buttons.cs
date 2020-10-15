using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 


public class buttons : MonoBehaviour
{
    /// <summary>
    /// Go to the scene "Begin" with main menu
    /// </summary>
    public void MenuPressed()
    {
        SceneManager.LoadScene("Begin");
    }

    /// <summary>
    /// Go to the scene "Game"
    /// </summary>
    public void BackPressed()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Pause is pressed 
    /// </summary>
    public static GameObject PrefabTest = null;
    public void PausePressed()
    {
        if (DateChangeing.pause)
                { flagPause = true; }
        else    { flagPause = false; }

        if (!flagPause) { PauseOn(); }
        else            { PauseOff(); }
    }

    public void OKPressed()
    {
        Destroy(PrefabTest);
    }


    /// <summary>
    /// change pause status,
    /// instantiate or destroy prefab PauseRectangle,
    /// stop or resume days counter increment,
    /// change texts' color,
    /// change ButtonPause's sprite
    /// </summary>
    public static Color sColorProcess = new Color(255, 255, 255); //white
    public static Color sColorPause = new Color(221, 84, 0);//orange
    public static Color sColorCurrent;
    private static bool flagPause = false;
    public void PauseOn()
    {
        flagPause = true;
        settings.sTextDays.GetComponent<Text>().color = sColorPause;
        settings.sTextCoins.GetComponent<Text>().color = sColorPause; 
        settings.sPauseRectangle.SetActive(true);
        DateChangeing.pause = true;
        sColorCurrent = sColorPause;
        settings.sButtonPause.GetComponent<Image>().sprite = settings.sContinueImage;
    }
    public void PauseOff()
    {
        flagPause = false;
        settings.sTextCoins.GetComponent<Text>().color = sColorProcess;//white
        settings.sTextDays.GetComponent<Text>().color = sColorProcess;
        settings.sPauseRectangle.SetActive(false);
        DateChangeing.pause = false;
        sColorCurrent = sColorProcess;
        settings.sButtonPause.GetComponent<Image>().sprite = settings.sPauseImage;

    }

    /// <summary>
    /// Go to the scene "Research"
    /// </summary>
    public void ResearchPressed()
    {
        SceneManager.LoadScene("Research");
    }

    public GameObject TextCoins;
    /// <summary>
    /// Coin Pressed -> Show NCoins a day
    /// </summary>
    /// <param name="prefab"></param>
    public void NCoinPressed()
    {
        if (!TextCoins.activeSelf)
        {
            TextCoins.SetActive(true);
            if (PersonalSettings.language == LanguageSettings.Language.English)
            { TextCoins.GetComponent<Text>().text = settings.gameSettings.stepCoins + " per day"; }
            else {
                if (PersonalSettings.language == LanguageSettings.Language.Russian)
                { TextCoins.GetComponent<Text>().text = settings.gameSettings.stepCoins + " В ДЕНЬ"; }}
            StartCoroutine(MakeSleepObject());
        }
    }

    IEnumerator MakeSleepObject()
    {
        yield return new WaitForSeconds(3.0f);
        if (TextCoins.activeSelf)
        {
            TextCoins.SetActive(false);
        }
    }


    public GameObject questionBox;
    public Button BlueCoin;
    public static Button sBlueCoin;
    /// <summary>
    /// prefab to ask about advertisement
    /// </summary>
    public void BlueCoinPressed() 
    {
        sBlueCoin = BlueCoin;

        // to prevent reclick on the button
        sBlueCoin.interactable = false;
        var instance = Instantiate(questionBox);
        instance.transform.SetParent(gameObject.transform, false);
    }

    // Нажата кнопка "Выход из игры"
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
