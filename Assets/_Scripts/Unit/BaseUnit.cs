using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public bool IsAlly;

    [SerializeField] private int _health;
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private float _range;

    private BaseUnit _target;

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
        if (_targetInRange)
        {
            Attack();
            return;
        }

        FindClosestTarget();
        MoveToTarget();
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }

    public virtual void Attack()
    {
        _target.TakeDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
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

    private void MoveToTarget()
    {
        if (_target != null)
            transform.position = Vector2.MoveTowards(
                    transform.position, 
                    _target.transform.position, 
                    _speed * Time.deltaTime
                    );
    }
}