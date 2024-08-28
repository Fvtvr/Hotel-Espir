using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Flashlight : MonoBehaviour
{
    public enum FlashlightState
    {
        ON,
        OFF,
        CHARGING
    }

    [SerializeField]
    private Light _light;

    [SerializeField]
    private int _maxBatteries = 3;

    [SerializeField]
    private float _depleteTime = 15;
    [SerializeField]
    private float _chargeTime = 3;

    public FlashlightState State { get; private set; } = FlashlightState.OFF;

    public void AddBattery()
    {
        _numBatteries = Math.Min(_numBatteries + 1, _maxBatteries);
        _onBatteryAdded?.Invoke();
    }

    public void StartCharging()
    {
        PowerOff();
        State = FlashlightState.CHARGING;
    }

    public void StopCharging()
    {
        State = FlashlightState.OFF;
    }

    public void TogglePower()
    {
        if (State == FlashlightState.OFF)
        {
            PowerOn();
        } else
        if (State == FlashlightState.ON)
        {
            PowerOff();
        }
    }

    public void PowerOn()
    {
        if (State == FlashlightState.CHARGING)
        {
            return;
        }
        State = FlashlightState.ON;
        _light.gameObject.SetActive(true);
    }

    public void PowerOff()
    {
        if (State == FlashlightState.CHARGING)
        {
            return;
        }
        State = FlashlightState.OFF;
        _light.gameObject.SetActive(false);
    }

    private void OnValidate()
    {
        Awake();
    }

    private void Awake()
    {
        _numBatteries = _maxBatteries;
        _currentBatteryId = _numBatteries;
        _currentBatteryCharge = 1;
        _chargeSpeed = 1 / _chargeTime;
        _depleteSpeed = 1 / _depleteTime;
    }

    private void Start()
    {
        PowerOff();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TogglePower();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCharging();
        } else
        if (Input.GetKeyUp(KeyCode.R))
        {
            StopCharging();
        }

        UpdatePower();

        _toString = 
            $"State: {State}\n" +
            $"Battery: {_currentBatteryId}\n" +
            $"Charge: {_currentBatteryCharge}";
    }

    private void UpdatePower()
    {
        if (State == FlashlightState.OFF)
        {
            return;
        }

        var prevBatteryId = _currentBatteryId;

        var speed = State == FlashlightState.ON
            ? _depleteSpeed
            : _chargeSpeed;
        var delta = speed * Time.deltaTime;
        while (delta > 0)
        {
            if (State == FlashlightState.ON)
            {
                if (_currentBatteryCharge - delta <= 0)
                {
                    _currentBatteryCharge = 1;
                    --_currentBatteryId;
                    if (_currentBatteryId <= 1)
                    {
                        delta = 0;
                    }
                    else
                    {
                        delta -= 1 - _currentBatteryCharge;
                    }
                }
                else
                {
                    if (_currentBatteryId < 1)
                    {
                        break; // Cannot deplete non-existant battery.
                    }
                    _currentBatteryCharge -= delta;
                    delta = 0;
                }  
            }
            else
            {
                if (_currentBatteryCharge + delta >= 1)
                {
                    _currentBatteryCharge = 0;
                    ++_currentBatteryId;
                    if (_currentBatteryId >= _maxBatteries)
                    {
                        delta = 0;
                    }
                    else
                    {
                        delta -= 1 - _currentBatteryCharge;
                    }
                }
                else
                {
                    if (_currentBatteryId > _maxBatteries)
                    {
                        break;  // Cannot charge non-existant battery.
                    }
                    _currentBatteryCharge += delta;
                    delta = 0;
                }
            }
        }

        if (_currentBatteryId > prevBatteryId)
        {
            Debug.Log($"Battery {prevBatteryId} charged!");
            _onBatteryCharged?.Invoke();
        } else
        if (_currentBatteryId < prevBatteryId)
        {
            Debug.Log($"Battery {prevBatteryId} depleted!");
            _onBatteryEmptied?.Invoke();
        }

        if (_currentBatteryId < 1)
        {
            _currentBatteryId = 1;
            _currentBatteryCharge = 0;
            PowerOff();
        }

        if (_currentBatteryId > _maxBatteries)
        {
            _currentBatteryId = _maxBatteries;
            _currentBatteryCharge = 1;
        }
    }

    private int _numBatteries;
    private int _currentBatteryId;
    private float _currentBatteryCharge;
    private float _chargeSpeed;
    private float _depleteSpeed;
    
    [SerializeField]
    private UnityEvent _onBatteryAdded;
    [SerializeField]
    private UnityEvent _onBatteryEmptied;
    [SerializeField]
    private UnityEvent _onBatteryCharged;

    [SerializeField, TextArea(1, 4)]
    private string _toString;
}
