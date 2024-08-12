using UnityEngine;
using UnityEngine.UI;
using Main;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Button _startFight;
        [SerializeField] private Button _leaveFight;
        [SerializeField] private Button _healing;
        [SerializeField] private Button _switchWeapon;
         
        private void Start()
        {
            GameManager.Instance.GameplayStateChanged += GameplayStateChanged;

            GameplayStateChanged(GameplayStates.Idle);
        }

        private void OnDestroy()
        {
            GameManager.Instance.GameplayStateChanged -= GameplayStateChanged;
        }

        private void GameplayStateChanged(GameplayStates newState)
        {
            switch (newState)
            {
                case GameplayStates.Idle:
                    if (_leaveFight.gameObject.activeSelf) _leaveFight.gameObject.SetActive(false);
                    _startFight.gameObject.SetActive(true);
                    _healing.gameObject.SetActive(true);
                    _switchWeapon.gameObject.SetActive(false);
                    break;
                case GameplayStates.Fight:
                    if (_startFight.gameObject.activeSelf) _startFight.gameObject.SetActive(false);
                    _leaveFight.gameObject.SetActive(true);
                    _healing.gameObject.SetActive(false);
                    _switchWeapon.gameObject.SetActive(true);
                    break;
            }
        }
    }
}