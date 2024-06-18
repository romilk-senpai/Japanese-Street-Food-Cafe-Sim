using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    public event System.Action<AICharacter> OnOrderGrabbed;

    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float walkingMagnitude = 0.01f;

    [SerializeField] private string[] randomBlendShapesAnimations;

    [SerializeField] private Vector2 blendShapeAnimDelayRange = new Vector2(3f, 7f);

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] ohayoSounds;
    [SerializeField] private AudioClip[] arigatoSounds;

    private bool _isBusy = false;
    private bool _shouldFireFinish = false;
    private OrderStand _agentTargetStand;

    private System.Action<AICharacter> _agentReachedCallback;

    public bool IsBusy => _isBusy;

    private void Start()
    {
        if (randomBlendShapesAnimations.Length > 0)
            StartCoroutine(PlayRandomBlendShape());
    }

    private IEnumerator PlayRandomBlendShape()
    {
        yield return new WaitForSeconds(Random.Range(blendShapeAnimDelayRange.x, blendShapeAnimDelayRange.y));

        animator.Play(randomBlendShapesAnimations[Random.Range(0, randomBlendShapesAnimations.Length)]);

        StartCoroutine(PlayRandomBlendShape());
    }

    public void OrderAt(OrderStand stand)
    {
        _agentTargetStand = stand;

        _agentTargetStand.IsBusy = true;
        _isBusy = true;

        void OnStandReached(AICharacter _)
        {
            _agentTargetStand.GenerateRandomOrder();
            _agentTargetStand.OnOrderFulfilled += OnOrderFulfilled;
        }

        SetAgentDestination(_agentTargetStand.OrderAiSpot.position, OnStandReached);
    }

    public void SetAgentDestination(Vector3 position, System.Action<AICharacter> callback = null)
    {
        agent.SetDestination(position);

        _shouldFireFinish = true;

        _agentReachedCallback += callback;
    }

    private void Update()
    {
        if (agent.velocity.magnitude > walkingMagnitude)
        {
            animator.SetFloat("WalkingSpeed", 1f);
        }
        else
        {
            animator.SetFloat("WalkingSpeed", 0f);
        }

        if (_shouldFireFinish && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            transform.DORotateQuaternion(_agentTargetStand.OrderAiSpot.rotation, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                animator.CrossFadeInFixedTime("Wave", .15f);
            });

            _shouldFireFinish = false;

            OnPointReached();
        }
    }

    private void OnPointReached()
    {
        audioSource.PlayOneShot(ohayoSounds[Random.Range(0, ohayoSounds.Length)]);

        _agentReachedCallback?.Invoke(this);

        _agentReachedCallback = null;
    }

    private void OnOrderFulfilled()
    {
        _agentTargetStand.OnOrderFulfilled -= OnOrderFulfilled;

        _agentTargetStand.IsBusy = false;
        _isBusy = false;

        audioSource.PlayOneShot(arigatoSounds[Random.Range(0, arigatoSounds.Length)]);

        OnOrderGrabbed?.Invoke(this);
    }
}
