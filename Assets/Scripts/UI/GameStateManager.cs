using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager
{
	private static GameStateManager instance;

	public static GameStateManager Instance
	{
		get {
			if (instance == null)
				instance = new GameStateManager();
			return instance;
		}
	}

	public GameState currentGameState { get; private set; }
	public delegate void GameStateChangeHandler(GameState newGameState);
	public event GameStateChangeHandler onGameStateChange;

	public void SetState(GameState newGameState)
	{
		if (newGameState == currentGameState)
		{
			return;
		}
		currentGameState = newGameState;
		onGameStateChange?.Invoke(currentGameState);
	}
}
