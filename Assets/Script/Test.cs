using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject Item;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Test1();
        }
    }
    void Test1()
    {
        Vector3 td = transform.TransformDirection(Vector3.forward);
        Debug.Log("TransformDirection  : " + td);
        Item.transform.position = td;
    }

}
