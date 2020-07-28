using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMoving : MonoBehaviour
{
    // переменная для обращения к Sun position
    public GameObject gameObjectCentre;
    // скорость вращения
    public float rateRotate;
    // угол отклонения, задается в градусах
    public float anlgeRotate;


    // текущее значение угла наклона
    private float phi1, phi2;
    // шаг для вращения Earth
    private float step_phi;
    private float y_rotate;

    // Earth moving radius
    private float r_E;

    private static System.Random rnd = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        y_rotate = 0.25f;

        // Получить вектор, который указывает от Sun position к Earth position
        Vector3 SunEarthVector = gameObjectCentre.transform.position - transform.position;

        // get distance between the centre of moveing and the object   
        r_E = SunEarthVector.magnitude;

        // инициализация начального угла для Earth position
        phi1 = 0;
        // перевести из градусов в радианы
        phi2 = anlgeRotate * Mathf.PI / 180f;

        step_phi = Mathf.PI / 2000f;
        step_phi *= rateRotate;

        //-------------------------------------------
        //getMaterial();
    }

    private void getMaterial()
    {
        int L = settings.sMaterials.Length - 1;
        this.GetComponent<Renderer>().material = settings.sMaterials[rnd.Next(0, L)];
    }


    // Update is called once per frame
    void Update()
    {
        // вращение вокруг своей оси
        this.transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x, 
            transform.rotation.eulerAngles.y + y_rotate, 
            transform.rotation.eulerAngles.z);


        Vector3 CentrePos = gameObjectCentre.transform.position;
        // change Earth position
        transform.position = gameObjectCentre.transform.position + posEarth();
        //sunPos + posEarth(phi);
        phi1 += step_phi;

        // если Earth position совпала с startEarthPosition, начать подсчет шагов заново
        if (phi1 == 2*Mathf.PI) 
        {
            // начать подсчет шагов заново
            phi1 = 0f;
        }
    }

     Vector3 posEarth()
     {
         Vector3  v = new Vector3 (
                          r_E * Mathf.Sin(phi1) * Mathf.Cos(phi2),  
                          r_E * Mathf.Sin(phi1) * Mathf.Sin(phi2),
                          r_E * Mathf.Cos(phi1)
         ); 

         return v;
     }

}
