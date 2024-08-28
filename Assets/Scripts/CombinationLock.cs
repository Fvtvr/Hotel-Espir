using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    private void OnValidate()
    {
        Awake();
        if (_dials.Count() != _correctCombination.Length)
        {
            Debug.LogError("CombinationLock: The number of dials must equal the size of the correct combination!");
        }
    }

    private void Awake()
    {
        _dials = GetComponentsInChildren<CombinationLockDial>();
    }

    private void Update()
    {
        _prevState = _currState;
        _currState = CheckCode();

        if (_prevState != _currState)
        {
            if (_currState)
            {
                Debug.Log("Code was successful!");
                _onSuccess?.Invoke();
            }
            else
            {
                Debug.Log("Code was unsuccessful!");
                _onFailure?.Invoke();
            }
        }

        var combination = String.Join(' ', _dials.Select(x => x.GetCode()));
        var correctCombination = String.Join(' ', _correctCombination);
        _toString = 
            $"Combination: {combination}\n" +
            $"Correct combination: {correctCombination}";
    }

    private bool CheckCode()
    {
        int i = 0;
        foreach (var dial in _dials)
        {
            if (dial.IsTurning)
            {
                return false;
            }

            if (dial.GetCode() != _correctCombination[i])
            {
                return false;
            }

            ++i;
        }

        return true;
    }
    
    private bool _prevState = false;
    private bool _currState = false;

    [SerializeField]
    private UnityEvent _onSuccess;
    [SerializeField]
    private UnityEvent _onFailure;

    private CombinationLockDial[] _dials;

    [SerializeField]
    private string[] _correctCombination;

    [SerializeField, TextArea(1, 4)]
    private string _toString;
}
