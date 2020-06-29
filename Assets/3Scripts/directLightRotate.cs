using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directLightRotate : MonoBehaviour
{
    // gameObject to get the Sun position
    public GameObject gameObjectCentre;
    // gameObject to get the Earth position
    public GameObject gameObjectDst;

    // Update is called once per frame
    void Update()
    {
        // get position for the light
        transform.position = new Vector3(
            (gameObjectCentre.transform.position.x + gameObjectDst.transform.position.x)/2,
            (gameObjectCentre.transform.position.y + gameObjectDst.transform.position.y)/2,
            (gameObjectCentre.transform.position.z + gameObjectDst.transform.position.z)/2
            );

        transform.rotation = Quaternion.LookRotation(gameObjectDst.transform.position);
    }
}
