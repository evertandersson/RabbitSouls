using Gamemode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeToProceed1 : MonoBehaviour
{

    [SerializeField]
    private float TimeToP;

    void Start()

    {
        StartCoroutine(TimeToGo());
        if(Input.GetButtonDown("Fire1"))
        {
            SceneController.LoadFirstLevel();
        }
            
    }


    void Update()
    {

    }

    private IEnumerator TimeToGo()
    {
    yield return new WaitForSeconds(TimeToP);
    SceneController.LoadFirstLevel();
    }
}
