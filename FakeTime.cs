using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeTime : MonoBehaviour
{
    // Start is called before the first frame update

    public float faketime;
    public float speed = 1;
    public MeshRenderer[] render;


    // Update is called once per frame
    void Update()
    {
        faketime += Time.deltaTime * speed;
        foreach (MeshRenderer renderer in render)
        {
            renderer.material.SetFloat("_FakeTime", faketime);
        }
    }
}
