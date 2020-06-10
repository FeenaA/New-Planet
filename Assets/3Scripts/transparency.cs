using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// скрипт для установки прозрачности
public class menuGraph : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderer = gameObject.GetComponent<Renderer>();
        renderer.material.shader = Shader.Find("Transparent/Diffuse");
        renderer.material.color = Color.white * 0.25f; // белый с 25% не прозрачности
    }
}
