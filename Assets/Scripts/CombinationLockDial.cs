using System.Linq;
using UnityEngine;

public class CombinationLockDial : MonoBehaviour
{
    public bool IsTurning { get; private set; }

    public void TurnDial(float delta)
    {
        _rotationDirection = Mathf.Sign(delta);

        // Linearly remap delta and then clamp it.
        _speedModifier = Mathf.Clamp(
            Mathf.LerpUnclamped(0.5F, 1.0F, Mathf.Abs(delta)), 
            0.5F, 
            2.0F
          );
        
        if (!IsTurning)
        {
            _prevRotation = _rotation;
            _nextRotation = _rotation + _rotationDirection * _degreesPerInterval;
            IsTurning = true;
            return;
        }

        // Check if the dial has changed direction.
        var prevRotationDirection = Mathf.Sign(_nextRotation - _prevRotation);
        if (_rotationDirection * prevRotationDirection < 0)
        {
            var t = _nextRotation;
            _nextRotation = _prevRotation;
            _prevRotation = t;
        }
    }

    private void OnValidate()
    {
        Awake();
        if (_symbols?.Count() != _numSymbols)
        {
            Debug.LogError("CombinationLockDial: Number of symbols does not match expected number!");
        }
    }

    private void Awake()
    {
        _degreesPerInterval = 360.0F / _numSymbols;
    }

    private void Update()
    {
        if (!IsTurning)
        {
            return;
        }
        
        var delta = Time.deltaTime * _speed * _speedModifier * _rotationDirection;
        _rotation += delta;

        // Clamp the rotation.
        if (
            _rotationDirection > 0 && _rotation >= _nextRotation ||
            _rotationDirection < 0 && _rotation <= _nextRotation
          )
        {
            _rotation = _nextRotation;
            _prevRotation = _nextRotation;
            IsTurning = false;
        }

        transform.localRotation = Quaternion.Euler(0, _rotation, 0);

        _toString = 
            $"Code: {GetCode()}\n" +
            $"Is turning: {IsTurning}\n" +
            $"Prev rotation: {_prevRotation}\n" +
            $"Curr rotation: {_rotation}\n" +
            $"Next rotation: {_nextRotation}";
    }

    public string GetCode()
    {
        var wrappedRotation = _rotation - Mathf.Floor(_rotation / 360.0F) * 360;

        int code = Mathf.RoundToInt(wrappedRotation / _degreesPerInterval);
        return _symbols[code];
    }

    [SerializeField]
    private float _speed = 360;

    private float _speedModifier;

    private float _rotationDirection;
    private float _rotation;
    private float _prevRotation;
    private float _nextRotation;

    [SerializeField]
    private int _numSymbols = 10;
    [SerializeField]
    private string[] _symbols;
    private float _degreesPerInterval;

    [SerializeField, TextArea(1, 5)]
    private string _toString;
}
