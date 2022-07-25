using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CardsCollection
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _spriteCard;

        public TextMeshProUGUI HealthText => _healthText;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI NameText => _nameText;
        public TextMeshProUGUI LevelText => _levelText;
        public Image SpriteCard => _spriteCard;

        private void SetHealthText(int health)
        {
            _healthText.text = health.ToString();
        }
        
        private void SetDamageText(int health)
        {
            _damageText.text = health.ToString();
        }
        
        private void SetNameText(int health)
        {
            _nameText.text = health.ToString();
        }
        
        private void SetLevelText(int health)
        {
            _levelText.text = health.ToString();
        }
        
        private void SetSpriteCard(Sprite sprite)
        {
            _spriteCard.sprite = sprite;
        }
    }
}
