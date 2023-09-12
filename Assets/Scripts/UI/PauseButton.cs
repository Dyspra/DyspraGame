using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.currentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;
            GameStateManager.Instance.SetState(newGameState);
            //Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
    }
}
