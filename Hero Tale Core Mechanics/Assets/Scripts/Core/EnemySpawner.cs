using System;
using UnityEngine;
using Main;

namespace Core
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private FighterData[] _enemyConfigs;
        [SerializeField] private Transform _spawnPoint;

        public event Action<Fighter, FighterData> EnemySpawned;

        public void SpawnEnemy()
        {
            float totalProbability = 0f;

            foreach (var config in _enemyConfigs)
            {
                totalProbability += config.SpawnProbability;
            }

            float randomPoint = UnityEngine.Random.Range(0, totalProbability);
            float currentProbability = 0f;

            foreach (var config in _enemyConfigs)
            {
                currentProbability += config.SpawnProbability;
                if (randomPoint <= currentProbability)
                {
                    var enemy = Instantiate(config.FighterPrefab, _spawnPoint.position, _spawnPoint.rotation);
                    EnemySpawned?.Invoke(enemy.GetComponent<Fighter>(), config);
                    break;
                }
            }

            GameManager.Instance.SetGameplayState(GameplayStates.Fight);
        }
    }
}