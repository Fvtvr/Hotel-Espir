using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (IsClosed())
        {
            if (IsLocked())
            {
                Rattle();
            }
            else
            {
                Open();
            }
        } 
        else
        {
            Shut();
        }
    }

    public bool IsClosed()
    {
        return _isClosed;
    }

    public bool IsLocked()
    {
        return _locks != null && _locks.Count() != 0 && _locks.Any(x => x.IsLocked);
    }

    private void Open()
    {
        Debug.Log("Opening!");
        _animator.SetTrigger("Open");
        _isClosed = false;
        _onOpened?.Invoke();
    }

    private void Shut()
    {
        Debug.Log("Shutting!");
        _animator.SetTrigger("Shut");
        _isClosed = true;
        _onClosed?.Invoke();
    }

    private void Rattle()
    {
        Debug.Log("Rattling!");
        _animator.SetTrigger("Rattle");
        _onRattled?.Invoke();
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_locks == null || _locks.Count() == 0)
        {
            Debug.Log($"Door {name} was not supplied any locks, checking if it might have any in its children!");
            _locks = GetComponentsInChildren<DoorLock>();
        }
    }

    private void Update()
    {
        _toString =
            $"Is closed: {_isClosed}\n" +
            $"Is locked: {IsLocked()}";
    }

    [SerializeField]
    private DoorLock[] _locks = null;

    [SerializeField]
    private UnityEvent _onOpened;
    [SerializeField]
    private UnityEvent _onClosed;
    [SerializeField]
    private UnityEvent _onRattled;

    private bool _isClosed = true;
    
    private Animator _animator;

    [SerializeField, TextArea(1, 4)]
    private string _toString;
}
