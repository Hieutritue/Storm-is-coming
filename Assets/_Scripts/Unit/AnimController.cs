using NaughtyAttributes;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _idle;
    [SerializeField] private string _move;
    [SerializeField] private string _attack;

    [SerializeField, ReadOnly] private string _currentAnim = "";

    public void ChangeAnim(string newAnim)
    {
        if (_currentAnim.Equals(_attack)) return;
        if (_currentAnim.Equals(newAnim)) return;
        _animator.Play(newAnim);
        _currentAnim = newAnim;
    }

    public void PlayIdle() => ChangeAnim(_idle);
    public void PlayMove() => ChangeAnim(_move);
    public void PlayAttack() => ChangeAnim(_attack);

    public void ForceIdle()
    {
        _currentAnim = _idle;
        PlayIdle();
    }
}