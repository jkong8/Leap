/*AUTHOR: JEFFERY KONG
 * NOTE: MANAGES ALL OF THE DATA USED IN THE GAME SUCH AS TIME AND WHEN THE GAME IS FINISHED*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
    //Boolean to mark completion
    private bool completed = false;
    //Float to track time
    private  float time = 0.0f;
    private float elapsedTime = 0.0f;
    private float devTime = 143.16f;
    //Int to keep track of levels
    private static int currentLevel = 1; //<----CHANGE TO 1 WHEN STARTING AT SCENE 'LEVEL 1' 
    //String for current level
    private string currentLevelName;

    private void Start()
    {
        currentLevelName = "Level_" + currentLevel;
        Debug.Log(currentLevelName);
    }

    private void FixedUpdate()
    {
        //startOver();
    }

    //GUI
    private void OnGUI()
    {
        //Font Size
        GUI.skin.box.fontSize = 30;

        //While the game is not completed, keep track of the time.
        if (!completed)
        {   //Convert the Time.time float to seconds
            elapsedTime = (float)Mathf.Round(Time.time * 100)/100 - time;
            //Write the time!
            GUI.Box(new Rect(350, 0, Screen.width-350, 50), "Your Time: " + elapsedTime.ToString());
            //Write the developer's (me) time.
            GUI.Box(new Rect(0, 0, 350, 50), "Developer's Time: " + devTime );

            //
            GUI.Box(new Rect(0, Screen.height-50, Screen.width, 50), "Move - WASD    Jump - Space (Wall Jump With Wall Contact)    Dash - J (While Mid-Air)");
        }
        else
        {
            //End of Game GUI Boxes
            if(elapsedTime < devTime)
            {
                GUI.Box(new Rect(0, 0, Screen.width, 50), "Wow! You beat the dev with "+ elapsedTime + " seconds!  Restart game by refreshing browser");
            }else
            {

                GUI.Box(new Rect(0, 0, Screen.width, 50), "Too Slow! Your time was " + elapsedTime + " seconds! Restart game by refreshing browser");
            }
        }

    }

    //Marks the completion bool true
    public void markCompletion()
    {
        this.completed = true;
    }

    //Moves to the next level
    public void nextLevel()
    {
        currentLevel += 1;
        string nextLevelName = "Level_" + currentLevel;
        //Implement incrementation of levels
        if(currentLevel <= 8)
        {
            SceneManager.LoadScene(nextLevelName);
        }else
        {
            Debug.Log("Game is over!");
            markCompletion();
        }
    }

    public void startOver()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            time = elapsedTime;
            currentLevel = 1;
            SceneManager.LoadScene("Level_" + currentLevel);
        }
    }
}
