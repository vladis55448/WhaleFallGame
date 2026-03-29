using System;
using UnityEngine;

public class WalkZone : MonoBehaviour
{
    [SerializeField]
    private WalkZoneParams _params;

    public WalkZoneParams Params => _params;
}

[Serializable]
public class WalkZoneParams
{
    public float Speed;
    public float StepDelay;
    public float StepFrequency;
    public float StepHeight;
}