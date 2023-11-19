
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    private VirtualMouseInput virtualMouseInput;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }
    private void Update()
    {
        transform.localScale = Vector3.one * (1f / canvasRectTransform.localScale.x);
        transform.SetAsLastSibling();
    }
    private void LateUpdate()
    {
       Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.x, 0f, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }
}
