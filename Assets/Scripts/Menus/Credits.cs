using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float timeRemaining = 28;
    private void Update() {
        if(timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else if(timeRemaining <= 0)
            SceneManager.LoadScene("main_menu");

        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("main_menu");
    }
}
