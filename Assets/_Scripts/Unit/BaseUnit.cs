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

    private Vector2 _initialPos;
    private float _timer = 0;
    private SpriteRenderer _spriteRenderer;

    public Vector2 InitialPos
    {
        set => _initialPos = value;
    }

    private void Awake()
    {
        _initialPos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_isCastle) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private float DistanceToTarget =>
        _target != null ? Vector2.Distance(transform.position, _target.transform.position) : 100f;

    private bool TargetInRange => _doesAoe ? DistanceToTarget <= 0.7f : DistanceToTarget <= _range;

    private float DistanceTo(Vector2 pos) => Vector2.Distance(transform.position, pos); 

    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    private void Update()
    {
        if(_isCastle) return;
        
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


    private void Die()
    {
        if (_doesAoe) RealPosition.position += new Vector3(0, 2, 0);
        var dead = Instantiate(_dead, RealPosition);
        dead.transform.SetParent(transform.parent);
        UniTask.Delay(1800).ContinueWith(() => Destroy(dead.gameObject));
        Destroy(this.gameObject);
    }

    private void Explode()
    {
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
        if (_doesAoe)
        {
            var targetUnits =
                IsAlly
                    ? GameManager.Instance.UnitManager.AllEnemies
                    : GameManager.Instance.UnitManager.AllAllies;
            targetUnits
                .Where(t => DistanceTo(t.transform.position) < _range)
                .ToList()
                .ForEach(t =>
                {
                    t.TakeDamage(_damage);
                });
            Explode();
            return;
        }
        
        _target?.TakeDamage(_damage);
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
        GameManager.Instance.UnitManager.AllUnits.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnitManager.AllUnits.Remove(this);
    }

    private void CheckDirection(Vector2 target)
    {
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
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