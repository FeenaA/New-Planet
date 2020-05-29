using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // это важно 
// NEW

public class buttons : MonoBehaviour
{
    // нажата кнопка МЕНЮ
    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    // нажат красный крестик
    public void BackPressed()
    {
        SceneManager.LoadScene("Game");
    }

    // нажата кнопка Pause
    public void PausePressed()
    {
        // call static function
        sPausePressed();
    }

    // нажата кнопка Pause
    private static bool flagPause = false;
    public static GameObject pauseRectangle = null;
    private Image myImageComponent;
    public static void sPausePressed()
    {
        // change Source Image
        //myImageComponent = GetComponent<Image>(); 

        

        // the object may be called several times
        if (pauseRectangle != null) { flagPause = true; }
        else         {     flagPause = false;        }

        // change pause status
        // instantiate or destroy prefab PauseRectangle
        // stop or resume days counter increment
        // change texts' color
        // change ButtonPause's sprite
        if (!flagPause)
        {
            flagPause = true;
            pauseRectangle = Instantiate(settings.sPrefabPauseRectangle);
            DateChangeing.pause = true;
            settings.sTextCoins.GetComponent<Text>().color = new Color(221, 84, 0);
            settings.sTextDays.GetComponent<Text>().color = new Color(221, 84, 0);
            //settings.sButtonPause.GetComponent<Image>().sprite = settings.sContinueImage;
            settings.sButtonPause.GetComponent<Image>().sprite = settings.sContinueImage;
            //settings.sSecondImage;
        }
        else
        {
            flagPause = false;
            Destroy(pauseRectangle);
            DateChangeing.pause = false;
            settings.sTextCoins.GetComponent<Text>().color = new Color(255, 255, 255);
            settings.sTextDays.GetComponent<Text>().color = new Color(255, 255, 255);
            settings.sButtonPause.GetComponent<Image>().sprite = settings.sPauseImage;
        }
    }




    // нажата кнопка ЗДАНИЯ
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

    // Нажата кнопка "Красный крестик в префабе"
    public void ExitPrefabPressed1(GameObject prefab)
    {
        EarthOnClick.flagBuildings = false;
        GameObject instancePrefab = prefab;
        Destroy(instancePrefab);
        //if (!flagPause)
        if (!EarthOnClick.flagPauseBeforePrefab)
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
