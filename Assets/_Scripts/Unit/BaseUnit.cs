using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseUnit : MonoBehaviour
{
    public bool IsAlly;

    public Transform RealPosition;
    public AllyType AllyType;

    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private bool _doesAoe;
    [SerializeField, ShowIf("_doesAoe")] private Animator _explosion;
    [SerializeField] private AnimController _animController;
    [SerializeField] private Animator _dead;
    [SerializeField] private bool _isCastle;

    protected BaseUnit _target;

    private float _initialScale;
    private Vector2 _initialPos;
    private float _timer = 0;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    public Vector2 InitialPos
    {
        set => _initialPos = value;
    }

    private void Awake()
    {
        _initialScale = transform.localScale.x;
        _initialPos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_isCastle) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private float DistanceToTarget =>
        _target != null ? Vector2.Distance(transform.position, _target.transform.position) : 100f;

    private bool TargetInRange => _doesAoe ? DistanceToTarget <= 1f : DistanceToTarget <= _range;

    private float DistanceTo(Vector2 pos) => Vector2.Distance(transform.position, pos);

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
            {
                if (_isCastle) GameManager.Instance.Lose();
                Die();
            }
        }
    }

    private Vector2 _vector2 = Vector2.zero;

    private void Update()
    {
        if (_isCastle) return;

        if (_rb.velocity.magnitude > 0.01f) // Check if the velocity is significant
        {
            _rb.velocity = Vector2.SmoothDamp(_rb.velocity, Vector2.zero, ref _vector2, 1);
        }
        else
        {
            _rb.velocity = Vector2.zero; // Set velocity to zero to avoid floating-point issues
        }

        if (_target) CheckDirection(_target.transform.position);

        if (_spriteRenderer)
        {
            // Higher Y value means lower sorting order
            _spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100 + 1000);
        }

        FindClosestTarget();
        if (TargetInRange)
        {
            _timer += Time.deltaTime;
            if (_timer >= _attackCooldown)
            {
                Attack();
                _timer = 0;
            }

            return;
        }

        if (!_target)
        {
            if (transform.position.x < Mathf.Ceil(_initialPos.x) + 0.1f
                && transform.position.y < Mathf.Ceil(_initialPos.y) + 0.1f
                && transform.position.x >= Mathf.Floor(_initialPos.x)
                && transform.position.y >= Mathf.Floor(_initialPos.y))
            {
                return;
            }

            MoveToTarget(_initialPos);
            return;
        }

        MoveToTarget(_target.transform.position);
    }


    public void Die()
    {
        GameManager.Instance.AudioManager.PlayClip(ClipName.UnitDie);

        if (_doesAoe) RealPosition.position += new Vector3(0, 2, 0);
        var dead = Instantiate(_dead, RealPosition);
        dead.transform.SetParent(transform.parent);
        UniTask.Delay(1800).ContinueWith(() => Destroy(dead.gameObject));
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        GameManager.Instance.AudioManager.PlayClip(ClipName.Explosion);

        var ex = Instantiate(_explosion, RealPosition);
        ex.transform.SetParent(transform.parent);
        UniTask.Delay(900).ContinueWith(() => Destroy(ex.gameObject));
        Destroy(this.gameObject);
    }

    [Button]
    public virtual void Attack()
    {
        _animController.PlayAttack();
        UniTask.Delay(400).ContinueWith(DealDamage);
    }

    public void DealDamage()
    {
        if (!TargetInRange && !_doesAoe) return;

        GameManager.Instance.AudioManager.PlayClip(ClipName.UnitHit);

        if (_doesAoe)
        {
            var targetUnits = GameManager.Instance.UnitManager.AllUnits;
            targetUnits
                .Where(t => DistanceTo(t.transform.position) < _range)
                .ToList()
                .ForEach(t =>
                {
                    t.TakeDamage(_damage);
                    if (!t.CompareTag("Tower")) t.Push(4 *(t.transform.position - this.transform.position).normalized);
                });
            Explode();
            return;
        }

        _target?.TakeDamage(_damage);
    }

    public void Push(Vector2 dir)
    {
        Debug.Log($"Pushed: {dir}");
        _rb.AddForce(dir, ForceMode2D.Impulse);
        // DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, 1);
    }

    public void TakeDamage(int damage)
    {
        _spriteRenderer.DOColor(Color.red, 0.05f).OnComplete(
            () => { _spriteRenderer.DOColor(Color.white, 0.05f); }
        );
        Health -= damage;
    }

    private void FindClosestTarget()
    {
        var targetUnits =
            IsAlly
                ? GameManager.Instance.UnitManager.AllEnemies
                : GameManager.Instance.UnitManager.AllAllies;

        _target = targetUnits
            .OrderBy(u => DistanceTo(u.transform.position))
            .FirstOrDefault();
    }

    private void OnEnable()
    {
        if (!CompareTag("Player"))
            GameManager.Instance.UnitManager.AllUnits.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnitManager.AllUnits.Remove(this);
    }

    private void CheckDirection(Vector2 target)
    {
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(_initialScale, _initialScale, _initialScale);
        else
        {
            transform.localScale = new Vector3(-_initialScale, _initialScale, _initialScale);
        }
    }

    private void MoveToTarget(Vector2 target)
    {
        CheckDirection(target);
        _animController.PlayMove();
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            _speed * Time.deltaTime
        );
    }
}