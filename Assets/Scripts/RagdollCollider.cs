using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RagdollCollider : MonoBehaviour
{
   public Transform shoulderRight;
    public Transform elbowRight;
    public Transform shoulderLeft;
    public Transform elbowLeft;
    public Transform kneeRight;
    public Transform footRight;
    public Transform kneeLeft;
    public Transform footLeft;
    public Transform hip;

    private Transform selectedBodyPart = null;

    private Quaternion initialRotation;

    private Vector2 previousPointerPosition;

    public float rotationSpeed = 5f;

    public bool isZAxis;
    public bool isNegative;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    void Start()
    {
       // Set up input system
       playerInput = GetComponent<PlayerInput>();
       if (playerInput == null)
       {
           playerInput = gameObject.AddComponent<PlayerInput>();
       }

       touchPositionAction = playerInput.actions["TouchPosition"];
       touchPressAction = playerInput.actions["TouchPress"];
    }

    void Update()
    {
       HandlePointerInput();
    }

    private void HandlePointerInput()
    {
       // Check for touch/click start
       if (touchPressAction.WasPressedThisFrame())
       {
           Vector2 pointerPosition = touchPositionAction.ReadValue<Vector2>();
           Ray ray = Camera.main.ScreenPointToRay(pointerPosition);

           if (Physics.Raycast(ray, out RaycastHit hit))
           {
               SelectBodyPart(hit);
           }
           previousPointerPosition = pointerPosition;
       }

       // Check for touch/click hold with movement
       if (touchPressAction.IsPressed() && selectedBodyPart != null)
       {
           Vector2 currentPointerPosition = touchPositionAction.ReadValue<Vector2>();

           if (Vector2.Distance(currentPointerPosition, previousPointerPosition) > 5f)
           {
               RotateSelectedBodyPart(currentPointerPosition);
               previousPointerPosition = currentPointerPosition;
           }
       }

       // Check for touch/click release
       if (touchPressAction.WasReleasedThisFrame())
       {
           selectedBodyPart = null;
       }
    }

    private void SelectBodyPart(RaycastHit hit)
    {
       if (hit.transform == shoulderRight || hit.transform == elbowRight || 
            hit.transform == shoulderLeft || hit.transform == elbowLeft || 
            hit.transform == hip || hit.transform == kneeRight || 
            hit.transform == footRight || hit.transform == kneeLeft || 
            hit.transform == footLeft)
       {
           selectedBodyPart = hit.transform;
           initialRotation = selectedBodyPart.localRotation;
       }
    }

    private void RotateSelectedBodyPart(Vector2 inputPosition)
    {
       Vector3 screenPos = new Vector3(inputPosition.x, inputPosition.y, Camera.main.WorldToScreenPoint(selectedBodyPart.position).z);
       Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(screenPos);
       Vector3 direction = (targetWorldPos - selectedBodyPart.position).normalized;

       float inputAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

       if (!isZAxis)
       {
           float initialY = initialRotation.eulerAngles.y;
           float deltaY = Mathf.DeltaAngle(initialY, inputAngle);
           //float clampedDeltaY = Mathf.Clamp(deltaY, -90f, 90f);
           //float finalY = initialY + clampedDeltaY;
           deltaY = isNegative? -deltaY: deltaY;
           Quaternion targetRotation = Quaternion.Euler(0, deltaY, 0);

           ApplyRotation(targetRotation, -180, 180);
       }
       else
       {
           float initialZ = initialRotation.eulerAngles.z;
           float inputZ = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; // use X-Y for Z axis control
           float deltaZ = Mathf.DeltaAngle(initialZ, inputZ);
           //float clampedDeltaZ = Mathf.Clamp(deltaZ, -90f, 90f);
           //float finalZ = initialZ + clampedDeltaZ;
           deltaZ = isNegative ? -deltaZ : deltaZ;
           Quaternion targetRotation = Quaternion.Euler(0, 0, deltaZ);

           ApplyRotation(targetRotation, -180, 180, true);
       }
    }

    private void ApplyRotation(Quaternion baseRotation, float offsetNegative, float offsetPositive, bool isZ = false)
    {
       if (selectedBodyPart == hip)
       {
           selectedBodyPart.localRotation = Quaternion.Slerp(selectedBodyPart.localRotation, initialRotation * baseRotation, Time.deltaTime * rotationSpeed);
       }
       else if (selectedBodyPart == shoulderRight || selectedBodyPart == elbowRight)
       {
           Quaternion adjustedRotation = baseRotation * Quaternion.Euler(0, isZ ? 0 : offsetPositive, isZ ? offsetPositive : 0);
           selectedBodyPart.localRotation = Quaternion.RotateTowards(selectedBodyPart.localRotation, initialRotation * baseRotation, Time.deltaTime * rotationSpeed * 100);
       }
       else if (selectedBodyPart == shoulderLeft || selectedBodyPart == elbowLeft)
       {
           Quaternion adjustedRotation = baseRotation * Quaternion.Euler(0, isZ ? 0 : offsetNegative, isZ ? offsetNegative : 0);
           selectedBodyPart.localRotation = Quaternion.RotateTowards(selectedBodyPart.localRotation, initialRotation * baseRotation, Time.deltaTime * rotationSpeed * 100);
       }
       else if (selectedBodyPart == kneeRight || selectedBodyPart == footRight)
       {
           Quaternion adjustedRotation = baseRotation * Quaternion.Euler(0, isZ ? 0 : offsetPositive, isZ ? offsetPositive : 0);
           selectedBodyPart.localRotation = Quaternion.Lerp(selectedBodyPart.localRotation, initialRotation * baseRotation, Time.deltaTime * rotationSpeed);
       }
       else if (selectedBodyPart == kneeLeft || selectedBodyPart == footLeft)
       {
           Quaternion adjustedRotation = baseRotation * Quaternion.Euler(0, isZ ? 0 : offsetNegative, isZ ? offsetNegative : 0);
           selectedBodyPart.localRotation = Quaternion.Lerp(selectedBodyPart.localRotation, initialRotation * baseRotation, Time.deltaTime * rotationSpeed);
       }
    }
}
