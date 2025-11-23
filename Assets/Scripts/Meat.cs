using DG.Tweening;
using UnityEngine;

public class Meat : MonoBehaviour, IHarpoonTarget
{
    [SerializeField]
    private ParticleSystem _hitParticles;
    [SerializeField]
    private int _pullTimes;

    private int _pullCounter = 0;

    public void Hit(Vector3 position)
    {
        transform.DOShakePosition(0.5f, 0.8f);
        _hitParticles.transform.position = position;
        _hitParticles.Play();
    }

    public void PoolCompleted()
    {
        Debug.Log("Got Meat");
        Destroy(gameObject);
    }

    public void PoolOut(Transform parent, float pullTime)
    {
        transform.DOKill();
        transform.DOScale(Vector3.zero, pullTime);
        _hitParticles.Play();
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
    }

    public bool Pull()
    {
        transform.DOShakePosition(0.5f, 0.3f);
        _hitParticles.Play();
        _pullCounter++;
        return _pullCounter >= _pullTimes;
    }
}
