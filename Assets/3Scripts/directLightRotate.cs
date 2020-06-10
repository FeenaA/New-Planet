using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directLightRotate : MonoBehaviour
{
    // переменная для обращения к Sun position
    public GameObject gameObjectCentre;
    // переменная для обращения к Earth position
    public GameObject gameObjectDst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // получить вектор между Source и Dst
        //Vector3 SunEarthVector = gameObjectCentre.transform.position - gameObjectDst.transform.position;

        Vector3 lightPosition = new Vector3(
            (gameObjectCentre.transform.position.x + gameObjectDst.transform.position.x)/2,
            (gameObjectCentre.transform.position.y + gameObjectDst.transform.position.y)/2,
            (gameObjectCentre.transform.position.z + gameObjectDst.transform.position.z)/2
            );

        transform.position = lightPosition;

        //transform.rotation = Quaternion.Euler(gameObjectDst.transform.rotation.eulerAngles.x, gameObjectDst.transform.rotation.eulerAngles.y, gameObjectDst.transform.rotation.eulerAngles.z);
        //transform.rotation = Quaternion.LookRotation(lightPosition);
        transform.rotation = Quaternion.LookRotation(gameObjectDst.transform.position);

        
    }
}
