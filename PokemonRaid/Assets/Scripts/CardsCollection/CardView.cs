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
        [SerializeField] private Image _pokemonImage;
        [SerializeField] private Image _lockImage;
        [SerializeField] private GameObject _statsPanel;

        public TextMeshProUGUI HealthText => _healthText;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI NameText => _nameText;
        public TextMeshProUGUI LevelText => _levelText;
        public Image SpriteCard => _spriteCard;
        public Image PokemonImage => _pokemonImage;
        public Image LockImage => _lockImage;
        public GameObject StatsPanel => _statsPanel;

    }
}
