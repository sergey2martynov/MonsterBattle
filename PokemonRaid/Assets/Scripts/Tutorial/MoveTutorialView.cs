using UnityEngine;

namespace Tutorial
{
    public class MoveTutorialView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private GameObject _parent;
        [SerializeField] int Speed = 10;
        [SerializeField] int XScale = 500;
        [SerializeField] int YScale = 500;

        private Vector3 Pivot;
        private Vector3 PivotOffset;
        private float m_Phase;
        private bool m_Invert;
        private float _HalfPi = Mathf.PI * 2;

        public GameObject Parent => _parent;

        private void Start()
        {
            Pivot = _rectTransform.anchoredPosition;
        }

        private void Update()
        {
            if (isActiveAndEnabled)
            {
                PivotOffset = Vector3.right * 2 * XScale;

                m_Phase += Speed * Time.deltaTime;

                if (m_Phase > _HalfPi)
                {
                    m_Invert = !m_Invert;
                    m_Phase -= _HalfPi;
                }

                if (m_Phase < 0) m_Phase += _HalfPi;

                _rectTransform.anchoredPosition = Pivot + (m_Invert ? PivotOffset : Vector3.zero);
                _rectTransform.anchoredPosition += new Vector2(Mathf.Cos(m_Phase) * (m_Invert ? -1 : 1) * YScale,
                    Mathf.Sin(m_Phase) * XScale);
            }
        }
    }
}