using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float _healthPoints = 100f;

    private bool _isDead = false;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if( _animator == null )
        {
            Debug.LogError("Animator is Null!");
        }
    }
    
    private void Update()
    {
        
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void TakeDamage(float damage)
    {
        // This stops _healthPoints from dropping below 0
        _healthPoints = Mathf.Max(_healthPoints - damage, 0);

        if(_healthPoints == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(_isDead)
        {
            return;
        }

        _isDead = true;
        _animator.SetTrigger("die");
    }
}
