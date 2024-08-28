using UnityEngine;
using UnityEngine.Events;

public class Keypad : MonoBehaviour
{
    public void PressKey(string key)
    {
        switch (key)
        {
            case "#":
            {
                EnterGuess();
                ClearGuess();
                break;
            }
            case "*":
            {
                ClearGuess();
                break;
            }
            default:
            {
                AppendGuess(key);
                break;
            }
        }
    }

    private void ClearGuess()
    {
        _guess = "";
    }

    private void EnterGuess()
    {
        if (_guess == _keycode)
        {
            Debug.Log("Correct code!");
            _onSuccess?.Invoke();
        }
        else
        {
            Debug.Log("Incorrect code!");
            _onFailure?.Invoke();
        }
    }

    private void AppendGuess(string newDigit)
    {
        if (_guess.Length >= _keycode.Length)
        {
            return;
        }
        _guess += newDigit;
    }

    private void Update()
    {
        _toString = $"Guess: {_guess}";
    }

    [SerializeField]
    private string _keycode;
    private string _guess = "";

    [SerializeField]
    private UnityEvent _onSuccess;
    [SerializeField]
    private UnityEvent _onFailure;

    [SerializeField, TextArea(1, 3)]
    private string _toString;
}
