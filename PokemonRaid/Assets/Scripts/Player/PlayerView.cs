using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    public Transform Transform => transform;
    public Animator Animator => _animator;

    public event Action ViewDestroyed;

    public void SetViewActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        ViewDestroyed?.Invoke();
    }
}
