using UnityEngine;
using UnityEngine.Events;

public class OnClicked : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log($"Clicked on {gameObject.name}");
        _onClicked?.Invoke();
    }

    private void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Interact();
        }
    }

    [SerializeField]
    private UnityEvent _onClicked;
}
