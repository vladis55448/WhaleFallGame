using System;
using UnityEngine;

public class IndoorScenarioController : MonoBehaviour
{
    public event Action<OutdoorData> RequestLoadOutdoor;

    public void LoadOutdoor(int contextId)
    {
        RequestLoadOutdoor?.Invoke((OutdoorData)contextId);
    }
}
