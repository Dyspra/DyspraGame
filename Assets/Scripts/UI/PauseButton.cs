using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void PauseMenu()
    {
        GameState currentGameState = GameStateManager.Instance.currentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ? GameState.Paused : GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);

        if (GameStateManager.Instance.currentGameState == GameState.Paused)
        {
            pauseMenu.SetActive(true);
            if (cursor)
            {
                cursor.SetActive(true);
                cursor.GetComponent<CursorManager>().SearchLastCanvas();
            }
        }
        else
        {
            pauseMenu.SetActive(false);
            if (cursor) cursor.SetActive(false);
        }
    }
}
