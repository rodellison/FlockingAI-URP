using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("Exit key pressed.");


            //If we are running in a standalone build of the game
#if UNITY_STANDALONE || UNITY_64 || UNITY_STANDALONE_WIN
            //Quit the application
            Application.Quit();
#endif

            //If we are running in the editor
#if UNITY_EDITOR
            //Stop playing the scene
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}