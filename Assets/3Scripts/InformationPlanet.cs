using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationPlanet : MonoBehaviour
{
    public Material material;
    public Text textIntroduction;
    public Text textResources;

    public static Material sMaterial;
    public static Text sTextIntroduction;
    public static Text sTextResources;

    // Start is called before the first frame update
    void Start()
    {
        sMaterial = material;
        sTextIntroduction = textIntroduction;
        sTextResources = textResources;
    }
}
