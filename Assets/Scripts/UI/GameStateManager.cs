using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
	private static GameStateManager instance;

	public static GameStateManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameStateManager();
			}
			return instance;
		}
	}

	public GameState currentGameState { get; private set; }
	public delegate void GameStateChangeHandler(GameState newGameState);
	public event GameStateChangeHandler onGameStateChanged;

	public void SetState(GameState newGameState)
	{
		if (currentGameState == newGameState)
		{
			return;
		}
		currentGameState = newGameState;
		onGameStateChanged?.Invoke(currentGameState);
	}
}
