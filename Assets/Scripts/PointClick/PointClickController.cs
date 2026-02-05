using UnityEngine;

public class PointClickController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform _bottomPoint;
    [SerializeField]
    private Transform _topPoint;
    [SerializeField]
    private LayerMask _mask;

    public bool Locked;

    public void Lock()
    {
        Locked = true;
    }

    public void Unlock()
    {
        Locked = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Locked)
            return;
        RaycastHit hit;
        var screenMousePos = Input.mousePosition;
        var mouseRectPosX = Mathf.InverseLerp(_bottomPoint.position.x, _topPoint.position.x, screenMousePos.x);
        var mouseRectPosY = Mathf.InverseLerp(_bottomPoint.position.y, _topPoint.position.y, screenMousePos.y);
        var mouseRectPos = new Vector3(mouseRectPosX, mouseRectPosY, 0);
        var mouseViewRay = _camera.ViewportPointToRay(mouseRectPos);
        if (Physics.Raycast(mouseViewRay, out hit, 100, _mask))
        {
            var clickComponent = hit.collider.GetComponent<IClickComponent>();
            if (Input.GetMouseButtonDown(0) && clickComponent != null)
            {
                clickComponent.Click();
            }
        }
    }
}
