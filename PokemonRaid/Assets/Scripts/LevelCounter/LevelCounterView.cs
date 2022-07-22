using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelCounter
{
    public class LevelCounterView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private List<Image> _images;

        public TextMeshProUGUI LevelText => _levelText;
        public List<Image> Images => _images;
    }
}
