using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _pointValue = 1;
    public float Velocity = 4f;

    public Collider coll;

    private float timeBeforeKillingPlayer = 1f;
    public UnityEvent WindupToKillPlayer;
    public UnityEvent KillPlayer;

    public bool isKillingPlayer = false;
    public bool isElectrocuted = false;
    private bool _dying = false;

    public Animator animator;
    public GameObject deathVFX;

    public float PointValue
    {
        get => _pointValue;
        set => _pointValue = value;
    }

    void Awake()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Start()
    {
        isKillingPlayer = false;

        DOTween.Sequence().AppendCallback(() => 
        {
            transform.DOScale(1, 1);
        }).Play();

        if (AIController.Instance != null)
        {
            AIController.Instance.RegisterEnemy(this);
        }
    }
    
    public void Die()
    {
        if (_dying)
        {
            return;
        }
        Destroy(coll);
        _dying = true;
        Destroy(gameObject, 5f);

        DOTween.Sequence().AppendCallback(() => 
        {
            transform.DOShakeScale(2, 0.2f, 10, 90, true);
            transform.DOShakePosition(2, 0.1f, 10, 90);
        }).AppendInterval(2f).AppendCallback(() => 
        {
            transform.DOScale(0.1f, 3f);
        })
        .Play();

        animator.SetTrigger("Death");
        
        if (AIController.Instance != null)
        {
            AIController.Instance.DeregisterEnemy(this);
        }
        
        if (deathVFX != null)
        {
            Destroy(Instantiate(deathVFX, transform.position, Quaternion.identity), 5f);
        }
    }

    public void StartKillingPlayer()
    {
        isKillingPlayer = true;
        StartCoroutine(StartKillPlayerSequence());
    }

    public IEnumerator StartKillPlayerSequence()
    {
        WindupToKillPlayer.Invoke();

        var sequence = DOTween.Sequence()
            .AppendCallback(() =>
            {
                transform.DOScale(4f, timeBeforeKillingPlayer);
            }).AppendInterval(timeBeforeKillingPlayer - 0.5f)
            .AppendCallback(() => {
                animator.SetTrigger("Attack");
            })
            .AppendInterval(0.5f)
            .Play();

        while (sequence.IsPlaying())
        {
            if (_dying)
            {
                yield break;
            }
            yield return null;
        }

        KillPlayer.Invoke();
        GameMain.Instance.PlayerWasKilled.Invoke();
    }

    void OnDisable()
    {   
        StopAllCoroutines();
    }
}
