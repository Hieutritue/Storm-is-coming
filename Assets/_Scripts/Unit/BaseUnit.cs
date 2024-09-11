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

    [FormerlySerializedAs("_realPosition")]
    public Transform RealPosition;

    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private AnimController _animController;

    protected BaseUnit _target;

    private Vector2 _initialPos;
    private float _timer = 10f;
    private SpriteRenderer _spriteRenderer;

    public Vector2 InitialPos
    {
        set => _initialPos = value;
    }

    private void Awake()
    {
        _initialPos = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private float _distanceToTarget =>
        _target != null ? Vector2.Distance(transform.position, _target.transform.position) : 100f;

    private bool _targetInRange => _distanceToTarget <= _range;

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
        if (_spriteRenderer != null)
        {
            // Higher Y value means lower sorting order
            _spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100 + 1000);
        }
        
        FindClosestTarget();
        if (_targetInRange)
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
            if(transform.position.Equals(_initialPos))
                return;
            
            Debug.Log(_initialPos);
            MoveToTarget(_initialPos);
            return;
        }

        MoveToTarget(_target.transform.position);
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }

    [Button]
    public virtual void Attack()
    {
        _animController.PlayAttack();
    }

    public void DealDamage()
    {
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
            .OrderBy(u => _distanceToTarget)
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

    private void MoveToTarget(Vector2 target)
    {
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        _animController.PlayMove();
        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            _speed * Time.deltaTime
        );
    }
}