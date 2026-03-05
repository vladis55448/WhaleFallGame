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
    [SerializeField]
    private Vector2 _cursorHotspot;
    [SerializeField]
    private Texture2D _cursorTexture;
    [SerializeField]
    private Texture2D _interactionCursor;

    public bool Locked;

    private void Start()
    {
        Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
    }

    public void Lock()
    {
        Locked = true;
    }

    public void Unlock()
    {
        Locked = false;
    }

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
            if (clickComponent != null)
            {
                Cursor.SetCursor(_interactionCursor, _cursorHotspot, CursorMode.Auto);
                if (Input.GetMouseButtonDown(0))
                {
                    clickComponent.Click();
                }
            }
            else
            {
                Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
            }
        }
        else
        {
            Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
        }
    }
}
