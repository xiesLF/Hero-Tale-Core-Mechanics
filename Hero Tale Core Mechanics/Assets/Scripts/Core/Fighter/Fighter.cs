using System.Collections;
using UnityEngine;
using UI;

namespace Core
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private UIHealthStat _uiHealthStat;
        [SerializeField] private Animator _animator;
        [SerializeField] private WeaponSystem _weaponSystem;

        private FighterData _fighterData;

        public float Health { get; private set; }
        public float AttackPower { get; private set; }
        public float Armor { get; private set; }
        public string Name { get; private set; }
        public float PreparationDuration { get; private set; }
        public float AttackDuration { get; private set; }

        public Coroutine IsChangeWeaponDuringPreparation => _isChangeWeaponDuringPreparation;

        public bool IsPreparing => _isPreparing;

        private CombatUI _combatUI;

        private Coroutine _isChangeWeaponDuringPreparation;

        private bool _isPreparing = false;
        private bool _isAttacking = false;

        private void OnEnable()
        {
            if (_weaponSystem != null)
            {
                _weaponSystem.WeaponSwitched += WeaponSwitched;
            }
        }

        private void OnDisable()
        {
            if (_weaponSystem != null)
            {
                _weaponSystem.WeaponSwitched -= WeaponSwitched;
            }
        }

        public void Initialize(FighterData data, CombatUI combatUI)
        {
            _fighterData = data;
            Health = data.Health;
            AttackPower = data.AttackPower;
            AttackDuration = data.AttackDuration;
            Armor = data.Armor;
            Name = data.FighterName;
            PreparationDuration = data.PreparationDuration;
            _combatUI = combatUI;

            if (_uiHealthStat != null) _uiHealthStat.UpdateUI(Health);
            if (_weaponSystem != null) WeaponSwitched(_weaponSystem.GetCurrentWeapon());
        }

        public IEnumerator PrepareForAttack()
        {
            _isPreparing = true;

            yield return StartCoroutine(_combatUI.UpdateFillImage(PreparationDuration));

            _isPreparing = false;
        }

        public IEnumerator Attack(Fighter target)
        {
            _isAttacking = true;

            yield return StartCoroutine(_combatUI.UpdateFillImage(AttackDuration));

            target.TakeDamage(AttackPower);

            _isAttacking = false;
        }

        public void TakeDamage(float damage)
        {
            float effectiveDamage = Mathf.Max(0, damage - Armor);
            Health -= effectiveDamage;

            if (_uiHealthStat != null)
            {
                _uiHealthStat.UpdateUI(Health);
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Hit");
            }
        }

        public void Healing()
        {
            Health = _fighterData.Health;

            if (_uiHealthStat != null)
            {
                _uiHealthStat.UpdateUI(Health);
            }
        }

        public void Death()
        {
            DestroyUI();
            Destroy(gameObject);
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        private void DestroyUI()
        {
            if (_combatUI != null)
            {
                _combatUI.DestroyUI();
            }
        }

        private void WeaponSwitched(Weapon weapon)
        {
            if (_isPreparing)
            {
                _combatUI.Pause();

                if (_isChangeWeaponDuringPreparation != null) _isChangeWeaponDuringPreparation = null;

                _isChangeWeaponDuringPreparation = StartCoroutine(ChangeWeaponDuringPreparation(weapon));
            }
            else if (_isAttacking)
            {
                if (_isChangeWeaponDuringPreparation != null) _isChangeWeaponDuringPreparation = null;

                _isChangeWeaponDuringPreparation = StartCoroutine(ChangeWeaponAfterAttack(weapon));
            }
            else if (!_isPreparing && !_isAttacking)
            {
                ApplyWeapon(weapon);
            }
        }

        private IEnumerator ChangeWeaponDuringPreparation(Weapon weapon)
        {
            yield return new WaitForSeconds(2f);

            ApplyWeapon(weapon);
            _weaponSystem.UpdateWeaponSkin();

            _combatUI.Resume();

            _isChangeWeaponDuringPreparation = null;
        }

        private IEnumerator ChangeWeaponAfterAttack(Weapon weapon)
        {
            while (_isAttacking)
            {
                yield return null;
            }

            _combatUI.Pause();

            yield return new WaitForSeconds(2f);

            ApplyWeapon(weapon);
            _weaponSystem.UpdateWeaponSkin();

            _combatUI.Resume();

            _isChangeWeaponDuringPreparation = null;
        }

        private void ApplyWeapon(Weapon weapon)
        {
            AttackPower = weapon.AttackPower;
            AttackDuration = weapon.AttackDuration;
        }
    }
}
