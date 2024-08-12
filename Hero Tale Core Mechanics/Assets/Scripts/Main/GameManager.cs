using System;
using UnityEngine;

namespace Main
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private GameplayStates _currentGameplayState = GameplayStates.Idle;

        public GameplayStates CurrentGameplayState => _currentGameplayState;

        public event Action<GameplayStates> GameplayStateChanged;

        private void Awake()
        {
            Instance = this;
        }

        public void SetGameplayState(GameplayStates state)
        {
            if (state != _currentGameplayState)
            {
                _currentGameplayState = state;
                GameplayStateChanged?.Invoke(_currentGameplayState);
            }

        }
    }
}
