using UnityEngine;
using TMPro;

namespace UI
{
    public class UIHealthStat : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void UpdateUI(float health)
        {
            _text.text = $"xp: {health}";
        }
    }
}