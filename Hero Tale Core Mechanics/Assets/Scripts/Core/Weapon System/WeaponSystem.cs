using System;
using UnityEngine;

namespace Core
{
    public class WeaponSystem : MonoBehaviour
    {
        [SerializeField] private Weapon[] _weapons;
        [SerializeField] private Fighter _fighter;
        private int _currentWeaponIndex = 0;

        public event Action<Weapon> WeaponSwitched;

        private void Start()
        {
            UpdateWeaponSkin();
        }

        public Weapon GetCurrentWeapon()
        {
            return _weapons[_currentWeaponIndex];
        }

        public void SwitchWeapon()
        {
            if (_fighter.IsChangeWeaponDuringPreparation != null) return;

            _currentWeaponIndex++;

            if (_currentWeaponIndex >= _weapons.Length)
            {
                _currentWeaponIndex = 0;
            }

            WeaponSwitched?.Invoke(_weapons[_currentWeaponIndex]);
        }

        public void UpdateWeaponSkin()
        {
            foreach (var weapon in _weapons)
            {
                weapon.gameObject.SetActive(false);

                if (_currentWeaponIndex == weapon.ID)
                {
                    weapon.gameObject.SetActive(true);
                }
            }
        }
    }
}