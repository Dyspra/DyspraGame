using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        if (sceneName == "BDD UI Scene")
        {
            MenuTransition.startExercises = true;
            QuestionnaireGoogle.allowQuestionnaire = true;
        }
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
