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

    [Header("Turning")]
    [Tooltip("Wie schnell sich der Blick der Bewegungsrichtung anpasst (Grad/Sekunde)")]
    [SerializeField] private float turnSpeed = 90f;
    [Tooltip("Ab welcher seitlicher Geschwindigkeit (m/s) die Drehung einsetzt")]
    [SerializeField] private float turnVelocityThreshold = 0.3f;

    [Header("Wall Interaction")]
    [SerializeField] private float pushOffForce = 5f;
    [SerializeField] private float wallPullForce = 4f;
    [SerializeField] private float handReachDistance = 0.35f;
    [SerializeField] private LayerMask wallLayerMask = ~0;

    [Header("XR References")]
    [Tooltip("'Camera Offset' des XR Origin")]
    [SerializeField] private Transform trackingSpace;
    [Tooltip("Transform der linken Hand / Left Controller")]
    [SerializeField] private Transform leftHandTransform;
    [Tooltip("Transform der rechten Hand / Right Controller")]
    [SerializeField] private Transform rightHandTransform;

    private Rigidbody rb;
    private Vector3 currentDirection;

    private InputDevice leftController;
    private InputDevice rightController;

    // Einhändiges Schwimmen: merken, ob nur eine Hand aktiv war
    private bool singleHandSwimming;

    // Wandkontakt
    private bool leftHandOnWall;
    private bool rightHandOnWall;
    private Vector3 leftWallNormal;
    private Vector3 rightWallNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        InputDevices.deviceConnected += OnDeviceConnected;
        TryInitializeControllers();
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= OnDeviceConnected;
    }

    private void OnDeviceConnected(InputDevice device) => TryInitializeControllers();

    private void TryInitializeControllers()
    {
        if (!leftController.isValid)
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(
                InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller, devices);
            if (devices.Count > 0) leftController = devices[0];
        }
        if (!rightController.isValid)
        {
            var devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(
                InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, devices);
            if (devices.Count > 0) rightController = devices[0];
        }
    }

    private void FixedUpdate()
    {
        if (!leftController.isValid || !rightController.isValid)
            TryInitializeControllers();

        leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
        rightController.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);

        DetectWallContact(leftGrip, rightGrip);

        bool handledByWall = HandleWallInteraction(leftGrip, rightGrip);

        if (!handledByWall)
            HandleFreeSwimming(leftGrip, rightGrip);

        ApplyResistanceForce();
        ApplyTurning();
    }

    // -----------------------------------------------------------------------
    // DREHEN
    // -----------------------------------------------------------------------

    /// <summary>
    /// Dreht das XR Origin (= Spieler-Blick) sanft in die horizontale
    /// Bewegungsrichtung. Greift nur beim einhändigen Schwimmen.
    /// </summary>
    private void ApplyTurning()
    {
        if (!singleHandSwimming) return;

        // Nur horizontale Komponente der aktuellen Geschwindigkeit
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVelocity.magnitude < turnVelocityThreshold) return;

        // Zielrotation: in die Bewegungsrichtung schauen (invertiert, da Schwimmkraft = -velocity)
        Quaternion targetRotation = Quaternion.LookRotation(-flatVelocity, Vector3.up);

        // Nur die Y-Achse des XR Origin drehen; Pitch/Roll bleiben unberührt
        float currentY  = transform.eulerAngles.y;
        float targetY   = targetRotation.eulerAngles.y;
        float newY      = Mathf.MoveTowardsAngle(currentY, targetY,
                              turnSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Euler(0f, newY, 0f);
    }

    // -----------------------------------------------------------------------
    // FREIES SCHWIMMEN
    // -----------------------------------------------------------------------

    private void HandleFreeSwimming(bool leftGrip, bool rightGrip)
    {
        singleHandSwimming = false;
        if (!leftGrip && !rightGrip) return;

        leftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVel);
        rightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVel);

        Vector3 localVelocity;

        if (leftGrip && rightGrip)
        {
            // Beide Hände → volle Kraft, keine Zwangsdrehung
            localVelocity = (leftVel + rightVel) * -1f;
        }
        else if (leftGrip)
        {
            // Nur linke Hand → halbe Kraft, Drehung einschalten
            localVelocity = leftVel * -0.5f;
            singleHandSwimming = true;
        }
        else
        {
            // Nur rechte Hand → halbe Kraft, Drehung einschalten
            localVelocity = rightVel * -0.5f;
            singleHandSwimming = true;
        }

        if (localVelocity.sqrMagnitude > deadZone * deadZone)
            AddSwimmingForce(localVelocity);
    }

    private void AddSwimmingForce(Vector3 localVelocity)
    {
        Vector3 worldVelocity = trackingSpace != null
            ? trackingSpace.TransformDirection(localVelocity)
            : localVelocity;

        rb.AddForce(worldVelocity * swimmingForce, ForceMode.Acceleration);
        currentDirection = worldVelocity.normalized;
    }

    // -----------------------------------------------------------------------
    // WAND-INTERAKTION
    // -----------------------------------------------------------------------

    private void DetectWallContact(bool leftGrip, bool rightGrip)
    {
        leftHandOnWall  = false;
        rightHandOnWall = false;

        if (leftGrip && leftHandTransform != null &&
            Physics.SphereCast(leftHandTransform.position, 0.08f,
                leftHandTransform.forward, out RaycastHit lHit,
                handReachDistance, wallLayerMask))
        {
            leftHandOnWall  = true;
            leftWallNormal  = lHit.normal;
        }

        if (rightGrip && rightHandTransform != null &&
            Physics.SphereCast(rightHandTransform.position, 0.08f,
                rightHandTransform.forward, out RaycastHit rHit,
                handReachDistance, wallLayerMask))
        {
            rightHandOnWall = true;
            rightWallNormal = rHit.normal;
        }
    }

    private bool HandleWallInteraction(bool leftGrip, bool rightGrip)
    {
        bool any = false;

        if (leftGrip  && leftHandOnWall)  { ApplyWallForce(leftController,  leftWallNormal);  any = true; }
        if (rightGrip && rightHandOnWall) { ApplyWallForce(rightController, rightWallNormal); any = true; }

        return any;
    }

    private void ApplyWallForce(InputDevice controller, Vector3 wallNormal)
    {
        controller.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 localVel);
        Vector3 worldVel = trackingSpace != null
            ? trackingSpace.TransformDirection(localVel)
            : localVel;

        if (worldVel.sqrMagnitude < 0.001f) return;

        float     pushComponent = Vector3.Dot(worldVel, wallNormal);
        Vector3   parallelVel   = worldVel - wallNormal * pushComponent;

        if (pushComponent > deadZone)
        {
            rb.AddForce(wallNormal * pushComponent * pushOffForce, ForceMode.Acceleration);
            currentDirection = wallNormal;
        }
        else if (parallelVel.sqrMagnitude > deadZone * deadZone)
        {
            rb.AddForce(-parallelVel * wallPullForce, ForceMode.Acceleration);
            currentDirection = (-parallelVel).normalized;
        }
    }

    // -----------------------------------------------------------------------
    // WIDERSTAND
    // -----------------------------------------------------------------------

    private void ApplyResistanceForce()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f && currentDirection != Vector3.zero)
            rb.AddForce(-rb.linearVelocity * resistanceForce, ForceMode.Acceleration);
        else
            currentDirection = Vector3.zero;
    }

    // -----------------------------------------------------------------------
    // DEBUG GIZMOS
    // -----------------------------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        DrawHandGizmo(leftHandTransform,  leftHandOnWall,  Color.cyan,  Color.red);
        DrawHandGizmo(rightHandTransform, rightHandOnWall, Color.green, Color.red);
    }

    private void DrawHandGizmo(Transform hand, bool onWall, Color normalColor, Color wallColor)
    {
        if (hand == null) return;
        Gizmos.color = onWall ? wallColor : normalColor;
        Gizmos.DrawWireSphere(hand.position, 0.08f);
        Gizmos.DrawRay(hand.position, hand.forward * handReachDistance);
    }
}
