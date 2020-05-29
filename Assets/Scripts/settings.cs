using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class settings : MonoBehaviour
{
    // prefab - PauseRectangle aroung the scene
    public GameObject prefabPauseRectangle;
    public static GameObject sPrefabPauseRectangle;
    // text - amount of coins
    public GameObject textCoins;
    public static GameObject sTextCoins;
    // text - amount of intervened days
    public GameObject textDays;
    public static GameObject sTextDays;

    // Sprites, Button - changeing images of ButtonPause
    public Sprite pauseImage;
    public Sprite continueImage; 
    public static Sprite sPauseImage;
    public static Sprite sContinueImage;
    // 
    public Button buttonPause;
    public static Button sButtonPause;
    

    void Start()
    {
        sPrefabPauseRectangle = prefabPauseRectangle;
        sTextCoins = textCoins;
        sTextDays = textDays;
        sPauseImage = pauseImage;
        sContinueImage = continueImage;
        sButtonPause = buttonPause;
    }
}
