using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // это важно 


public class buttons : MonoBehaviour
{
    // нажата кнопка МЕНЮ
    public void MenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    // home is pressed
    public void BackPressed()
    {
        SceneManager.LoadScene("Game");
    }

    // Pause is pressed
    public static GameObject PrefabTest = null;
    public void PausePressed()
    {
        sPausePressed();
    }

    public void OKPressed()
    {
        Destroy(PrefabTest);
    }

    // нажата кнопка Pause
    private static bool flagPause = false;
    public static void sPausePressed()
    {
        if (settings.sPauseRectangle.activeSelf)
        { flagPause = true; }
        else { flagPause = false; }

        if (!flagPause) { PauseOn(); }
        else { PauseOff(); }
    }

    // change pause status
    // instantiate or destroy prefab PauseRectangle
    // stop or resume days counter increment
    // change texts' color
    // change ButtonPause's sprite
    public static Color sColorProcess = new Color(255, 255, 255); //white
    public static Color sColorPause = new Color(221, 84, 0);//orange
    public static Color sColorCurrent;
    private static void PauseOn()
    {
        flagPause = true;
        settings.sTextDays.GetComponent<Text>().color = Color.green;// sColorPause;
        settings.sTextCoins.GetComponent<Text>().color = sColorPause; 
        settings.sPauseRectangle.SetActive(true);
        DateChangeing.pause = true;
        sColorCurrent = sColorPause;
        settings.sButtonPause.GetComponent<Image>().sprite = settings.sContinueImage;
    }
    private static void PauseOff()
    {
        flagPause = false;
        settings.sTextCoins.GetComponent<Text>().color = sColorProcess;//white
        settings.sTextDays.GetComponent<Text>().color = sColorProcess;
        settings.sPauseRectangle.SetActive(false);
        DateChangeing.pause = false;
        sColorCurrent = sColorProcess;
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

    // нажата кнопка Монета
    public GameObject TextCoins;
    public void NCoinPressed(GameObject prefab)
    {
        if (!TextCoins.activeSelf)
        {
            TextCoins.SetActive(true);
            TextCoins.GetComponent<Text>().text = DateChangeing.stepCoins + " per day";
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

    // prefab to ask about advertisement
    public GameObject questionBox;
    public Button BlueCoin;
    public static Button sBlueCoin;
    public void BlueCoinPressed() 
    {
        sBlueCoin = BlueCoin;

        // to prevent reclick on the button
        sBlueCoin.interactable = false;
        var instance = Instantiate(questionBox);

        // suppose to watch a short advert to get 1 BlueCoin
        //var instance = Instantiate(questionBox.gameObject) as GameObject;

        instance.transform.SetParent(gameObject.transform, false);
    }


    // Нажата кнопка "Выход из игры"
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    /*private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }*/
}
