using UnityEngine;
using UnityEngine.Events;

public class OnScrolled : MonoBehaviour, IInteractable
{
    public float Delta { get; private set; }

    public void Interact()
    {
        Debug.Log($"Scrolled on {gameObject.name} Delta {Delta}");
        _onScrolled?.Invoke(Delta);
    }

    private void OnMouseOver()
    {
        var delta = Input.GetAxis("Mouse ScrollWheel");
        if (delta != 0)
        {
            Delta = _shouldInvert ? -delta : +delta;
            Interact();
        }
    }

    [SerializeField]
    private bool _shouldInvert;

    [SerializeField]
    private UnityEvent<float> _onScrolled;
}
