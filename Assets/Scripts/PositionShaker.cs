using DG.Tweening;
using UnityEngine;

public class PositionShaker : MonoBehaviour
{
    [SerializeField]
    private float _distance;
    [SerializeField]
    private float _delay;

    private Vector3 _startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        _startPosition = transform.position;
        Move();
    }

    private void Move()
    {
        var targetPosition = _startPosition + Vector3.right * Random.Range(-_distance, _distance) + Vector3.up * Random.Range(-_distance, _distance) + Vector3.forward * Random.Range(-_distance, _distance);
        transform.DOMove(targetPosition, 0.1f).SetDelay(_delay).OnComplete(() =>
        {
            Move();
        });
    }
}
