using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UI;
using Main;

namespace Core
{
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private FighterData _playerData;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private Image _playerFillImage; 
        [SerializeField] private GameObject _enemyFillPrefab; 

        [SerializeField] private Fighter _playerFighter;
        private Fighter _currentEnemyFighter;
        private Coroutine _combatLoop;

        private void Start()
        {
            _playerFighter.Initialize(_playerData, new CombatUI(_playerFillImage));
        }

        private void OnEnable()
        {
            _enemySpawner.EnemySpawned += StartFight;
        }

        private void OnDisable()
        {
            _enemySpawner.EnemySpawned -= StartFight;
        }

        public void StopFight(bool playerWon)
        {
            if (_combatLoop != null)
            {
                StopCoroutine(_combatLoop);
                _combatLoop = null;
            }

            if (playerWon)
            {
                if (_currentEnemyFighter != null)
                {
                    _currentEnemyFighter.Death();
                    _currentEnemyFighter = null;
                }
            }
            else
            {
                if (_playerFighter != null)
                {
                    _playerFighter.Death();
                    _playerFighter = null;

                    StopAllCoroutines();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    return;
                }
            }

            GameManager.Instance.SetGameplayState(GameplayStates.Idle);
        }

        private void StartFight(Fighter fighter, FighterData fighterData)
        {
            GameObject enemyFillObj = Instantiate(_enemyFillPrefab);
            Image enemyFillImage = enemyFillObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();

            _currentEnemyFighter = fighter;
            _currentEnemyFighter.Initialize(fighterData, new CombatUI(enemyFillImage));

            enemyFillObj.transform.SetParent(_currentEnemyFighter.transform);
            enemyFillObj.transform.localPosition = new Vector3(0, 1, 0);
            enemyFillObj.transform.localScale = new Vector3(enemyFillObj.transform.localScale.x, 0.5f, enemyFillObj.transform.localScale.z);

            _combatLoop = StartCoroutine(CombatLoop());
        }

        private IEnumerator CombatLoop()
        {
            Coroutine playerCoroutine = StartCoroutine(PlayerCombatLoop());
            Coroutine enemyCoroutine = StartCoroutine(EnemyCombatLoop());

            yield return playerCoroutine;
            yield return enemyCoroutine;

            StopFight(_playerFighter.IsAlive());
        }

        private IEnumerator PlayerCombatLoop()
        {
            while (_playerFighter.IsAlive() && _currentEnemyFighter.IsAlive())
            {
                if (_playerFighter != null) yield return StartCoroutine(_playerFighter.PrepareForAttack());
                if (_playerFighter != null) yield return StartCoroutine(_playerFighter.Attack(_currentEnemyFighter));

                if (!_currentEnemyFighter.IsAlive())
                {
                    StopFight(true);
                    yield break;
                }
            }
        }

        private IEnumerator EnemyCombatLoop()
        {
            while (_playerFighter.IsAlive() && _currentEnemyFighter.IsAlive())
            {
                if (_currentEnemyFighter != null) yield return StartCoroutine(_currentEnemyFighter.PrepareForAttack());
                if (_currentEnemyFighter != null) yield return StartCoroutine(_currentEnemyFighter.Attack(_playerFighter));

                if (!_playerFighter.IsAlive())
                {
                    StopFight(false);
                    yield break;
                }
            }
        }
    }
}