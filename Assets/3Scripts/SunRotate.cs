using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    public float x_rotate = 0;
    public float y_rotate = 0.15f;
    public float z_rotate = 0;

    // Start is called before the first frame update
    /*void Start()
    {
        y_rotate = 0.15f;
    }*/

    // Update is called once per frame
    void Update()
    {
        // вращение вокруг своей оси
        this.transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x + x_rotate,
            transform.rotation.eulerAngles.y + y_rotate, 
            transform.rotation.eulerAngles.z + z_rotate
            );
    }
}
