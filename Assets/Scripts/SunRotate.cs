using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    float y_rotate;

    // Start is called before the first frame update
    void Start()
    {
        y_rotate = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        // вращение вокруг своей оси
        this.transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y + y_rotate, 
            transform.rotation.eulerAngles.z
            );
    }
}
