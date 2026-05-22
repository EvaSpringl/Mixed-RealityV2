using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class SwimmingController : MonoBehaviour
{
    [Header("Swimming Settings")]
    [SerializeField] private float swimmingForce = 10f;
    [SerializeField] private float resistanceForce = 1f;
    [SerializeField] private float deadZone = 0.1f;

    [Header("XR References")]
    [Tooltip("Hier den 'Camera Offset' des XR Origin reinziehen")]
    [SerializeField] private Transform trackingSpace;

    private Rigidbody rb;
    private Vector3 currentDirection;

    // XR Input Devices
    private InputDevice leftController;
    private InputDevice rightController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // Geräte suchen, sobald das Script aktiv wird
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitializeControllers();
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }

    private void OnDeviceConnected(InputDevice device)
    {
        TryInitializeControllers();
    }

    /// <summary>
    /// Versucht, linken und rechten Controller zu finden.
    /// Wird bei Start und bei jedem neu verbundenen Gerät aufgerufen.
    /// </summary>
    private void TryInitializeControllers()
    {
        if (!leftController.isValid)
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(
                InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
            if (devices.Count > 0)
                leftController = devices[0];
        }

        if (!rightController.isValid)
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(
                InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
            if (devices.Count > 0)
                rightController = devices[0];
        }
    }

    private void FixedUpdate()
    {
        // Sicherstellen, dass Controller initialisiert sind
        if (!leftController.isValid || !rightController.isValid)
            TryInitializeControllers();

        // Grip-Buttons abfragen (entspricht PrimaryHandTrigger im alten OVR-Code)
        bool leftGrip = false;
        bool rightGrip = false;
        leftController.TryGetFeatureValue(CommonUsages.gripButton, out leftGrip);
        rightController.TryGetFeatureValue(CommonUsages.gripButton, out rightGrip);

        if (leftGrip && rightGrip)
        {
            // Controller-Geschwindigkeiten auslesen
            Vector3 leftVelocity = Vector3.zero;
            Vector3 rightVelocity = Vector3.zero;
            leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out leftVelocity);
            rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out rightVelocity);

            // Kombinierte Bewegung, invertiert (Ruder-Logik: Hände nach hinten = vorwärts)
            Vector3 localVelocity = (leftVelocity + rightVelocity) * -1f;

            if (localVelocity.sqrMagnitude > deadZone * deadZone)
            {
                AddSwimmingForce(localVelocity);
            }
        }

        ApplyResistanceForce();
    }

    private void AddSwimmingForce(Vector3 localVelocity)
    {
        // Vom lokalen Tracking-Raum in Weltkoordinaten umrechnen
        Vector3 worldSpaceVelocity = trackingSpace != null
            ? trackingSpace.TransformDirection(localVelocity)
            : localVelocity;

        rb.AddForce(worldSpaceVelocity * swimmingForce, ForceMode.Acceleration);
        currentDirection = worldSpaceVelocity.normalized;
    }

    private void ApplyResistanceForce()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f && currentDirection != Vector3.zero)
        {
            rb.AddForce(-rb.linearVelocity * resistanceForce, ForceMode.Acceleration);
        }
        else
        {
            currentDirection = Vector3.zero;
        }
    }
}