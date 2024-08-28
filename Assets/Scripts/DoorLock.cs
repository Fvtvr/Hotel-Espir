using UnityEngine;
using UnityEngine.Events;

public class DoorLock : MonoBehaviour
{
    public bool IsLocked => _isLocked;

    public void Lock()
    {
        _isLocked = true;
        _onLocked?.Invoke();
    }

    public void Unlock()
    {
        _isLocked = false;
        _onUnlocked?.Invoke();
    }

    [SerializeField]
    private bool _isLocked = true;
    [SerializeField]
    private UnityEvent _onLocked;
    [SerializeField]
    private UnityEvent _onUnlocked;
}
