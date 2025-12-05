using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiLegBodyController : MonoBehaviour
{
    [SerializeField]
    private List<ProcedualLeg> _legs;
    [SerializeField]
    private List<LegGroup> _groups;
    [SerializeField]
    private float _followSpeed;
    [SerializeField]
    private float _rotationSpeed;
    [SerializeField]
    private Vector3 _centerOffset;

    private Vector3 _directionalOffset => transform.forward * _centerOffset.z + transform.forward * _centerOffset.x + transform.up * _centerOffset.y;

    private void Start()
    {
        foreach (var group in _groups)
        {
            group.Init();
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _directionalOffset + LegsCenter(), Time.deltaTime * _followSpeed);
        var targetForward = Vector3.RotateTowards(transform.forward, LegsForward(), _rotationSpeed * Time.deltaTime, 0);
        transform.forward = targetForward;
    }

    private Vector3 LegsCenter()
    {
        var center = new Vector3();
        foreach (var leg in _legs)
        {
            if (leg != null && leg.BodyTarget != null)
                center += leg.BodyTarget.position;
        }
        return center /= _legs.Count;
    }

    private Vector3 LegsForward()
    {
        var forward = new Vector3();
        foreach (var leg in _legs)
        {
            if (leg != null && leg.RaycastTarget != null)
                forward += leg.RaycastTarget.forward;
        }
        return forward /= _legs.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_directionalOffset + LegsCenter(), Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(LegsCenter(), Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, LegsForward());
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward);
    }


    [Serializable]
    private class LegGroup
    {
        public List<ProcedualLeg> PairedLegs;

        public void Init()
        {
            foreach (var leg in PairedLegs)
            {
                leg.StartedMove += OnLegStartedMove;
                leg.CompletedMove += OnLegCompletedMove;
            }
        }

        private void OnLegStartedMove(ProcedualLeg movedLeg)
        {
            foreach (var leg in PairedLegs)
            {
                if (leg != movedLeg)
                    leg.CanMove = false;
            }
        }

        private void OnLegCompletedMove(ProcedualLeg movedLeg)
        {
            foreach (var leg in PairedLegs)
            {
                if (leg != movedLeg)
                    leg.CanMove = true;
            }
        }
    }
}
