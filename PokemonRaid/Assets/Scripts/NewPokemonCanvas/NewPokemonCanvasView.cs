using Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewPokemonCanvas
{
    public class NewPokemonCanvasView : MenuViewBase
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _spriteCard;
        [SerializeField] private Image _pokemonImage;
        [SerializeField] private Button _getButton;
        
        public TextMeshProUGUI HealthText => _healthText;
        public TextMeshProUGUI DamageText => _damageText;
        public TextMeshProUGUI NameText => _nameText;
        public TextMeshProUGUI LevelText => _levelText;
        public Image SpriteCard => _spriteCard;
        public Image PokemonImage => _pokemonImage;

        public void Initialize()
        {
            _getButton.onClick.AddListener(Disable);
        }

        public void SetStats(string health, string damage, Sprite spriteCard, Sprite pokemonSprite)
        {
            _healthText.text = health;
            _damageText.text = damage;
            _spriteCard.sprite = spriteCard;
            _pokemonImage.sprite = pokemonSprite;
        }
    }
}
