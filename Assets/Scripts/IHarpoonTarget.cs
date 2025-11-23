using UnityEngine;

public interface IHarpoonTarget
{
    public void Hit(Vector3 position);
    public bool Pull();
    public void PoolOut(Transform parent, float pullTime);
    public void PoolCompleted();
}
