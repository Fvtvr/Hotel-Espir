using TMPro;
using UnityEngine;

public class KeypadButton : MonoBehaviour
{
    public void Press()
    {
        _keypad.PressKey(_key);
    }

    private void Start()
    {
        _keypad = GetComponentInParent<Keypad>();

        var textMeshPro = GetComponentInChildren<TextMeshPro>();
        _key = textMeshPro.text.Trim();
    }

    private Keypad _keypad;
    private string _key;
}
