using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //Components
    public Transform target;

    private void Start()
    {
        //Initializing Components
        target = GameObject.Find("Slime").GetComponent<Transform>();
    }
    private void Update()
    {
        
    }
}
