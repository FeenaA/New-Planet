using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // это важно 


public class buttons : MonoBehaviour
{
    // нажата кнопка МЕНЮ
    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    // cross is pressed
    public void BackPressed()
    {
        SceneManager.LoadScene("Game");
    }

    // Pause is pressed
    public void PausePressed()
    {
        // call static function
        sPausePressed();
    }

    // нажата кнопка Pause
    private static bool flagPause = false;
    public static GameObject pauseRectangle = null;
    public static void sPausePressed()
    {
        // the object may be called several times
        if (pauseRectangle != null) 
                {   flagPause = true;   }
        else    {   flagPause = false;  }

        if (!flagPause) {   PauseOn();  }
        else            {   PauseOff(); }
    }

    // change pause status
    // instantiate or destroy prefab PauseRectangle
    // stop or resume days counter increment
    // change texts' color
    // change ButtonPause's sprite
    private static void PauseOn()
    {
        flagPause = true;
        pauseRectangle = Instantiate(settings.sPrefabPauseRectangle);
        DateChangeing.pause = true;
        settings.sTextDays.GetComponent<Text>().color = settings.sColorPause;
        settings.sTextCoins.GetComponent<Text>().color = settings.sColorPause;      
        settings.sColorCurrent = settings.sColorPause;
        settings.sButtonPause.GetComponent<Image>().sprite = settings.sContinueImage;
    }
    private static void PauseOff()
    {
        flagPause = false;
        Destroy(pauseRectangle);
        DateChangeing.pause = false;
        settings.sTextCoins.GetComponent<Text>().color = settings.sColorProcess;//white
        settings.sTextDays.GetComponent<Text>().color = settings.sColorProcess;
        settings.sColorCurrent = settings.sColorProcess;
        settings.sButtonPause.GetComponent<Image>().sprite = settings.sPauseImage;
    }

    // нажата кнопка Research
    public void ResearchPressed()
    {
        bool pause = DateChangeing.pause;
        settings.flagPauseBeforePrefab = pause;
        // if the game isn't paused, pause
        if (!pause)
        {
            //DateChangeing.pause = true;
            sPausePressed();
        }
        SceneManager.LoadScene("Research");
    }

    // нажата кнопка Buildings
    public void BuildingsPressed()
    {
        SceneManager.LoadScene("Buildings");
    }

    // нажата кнопка Монета
    private bool flagCoin = false;
    private GameObject instance = null;
    public void NCoinPressed(GameObject prefab)
    {
        // объект мог быть уничтожен вне данного скрипта
        if (instance != null)   { flagCoin = true;    }
        else                    { flagCoin = false;   }

        if (!flagCoin)  { flagCoin = true;    instance = Instantiate(prefab); }
        else            { flagCoin = false;   Destroy(instance);              }
    }

    // "cross at prefab" is pressed
    public void ExitPrefabPressed1(GameObject prefab)
    {
        EarthOnClick.flagBuildings = false;
        GameObject instancePrefab = prefab;
        Destroy(instancePrefab);
        if (!settings.flagPauseBeforePrefab)
        {
            sPausePressed();
        }
    }

    // Нажата кнопка "Выход из игры"
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

}
