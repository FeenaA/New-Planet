using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public Button[] ButtonsHelp;
    public Text TextHelp;
    public GameObject CanvasHelp;
    private Button ButtonHelp;
    private Color DeSelectedColor;

    void Start()
    {
        DeSelectedColor = ButtonsHelp[0].GetComponent<Graphic>().color;

        for (int n = 0; n < 4; n++)
        { ButtonsHelp[n].GetComponentInChildren<Text>().text = Preprocessing.strHelpTitle[n]; }

        // the first Button is selected by default
        ButtonHelp = ButtonsHelp[0];
        TextHelp.text = Preprocessing.strHelp[0];
        Select();
    }

    public void ButtonHelpPressed(int n)
    {
        for (int i = 0; i<4; i++)
        {
            ButtonHelp = ButtonsHelp[i];
            TextHelp.text = Preprocessing.strHelp[n];

            if (i == n)
            {
                Select();
            }
            else
            {
                DeSelect();
            }
        }
    }

    private void Select() 
    {
        ButtonHelp.GetComponent<Graphic>().color = Color.black;
        ButtonHelp.GetComponent<Outline>().enabled = true;
    } 

    private void DeSelect()
    {
        ButtonHelp.GetComponent<Graphic>().color = DeSelectedColor;
        ButtonHelp.GetComponent<Outline>().enabled = false;
    }

    public void BackPressed() 
    {
        Destroy(CanvasHelp);
        Preprocessing.sCanvasMain.SetActive(true);
    }
}
