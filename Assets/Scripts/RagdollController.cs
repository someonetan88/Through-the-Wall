using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    [Header("Body Parts")]
    public Transform shoulderRight;
    public Transform elbowRight;
    public Transform shoulderLeft;
    public Transform elbowLeft;
    public Transform kneeRight;
    public Transform footRight;
    public Transform kneeLeft;
    public Transform footLeft;
    public Transform hip;

    [Header("UI")]
    public GameObject bodyPartButtons;
    public FixedJoystick joystick; // Use Joystick from Joystick Pack
    public float joystickRotationSpeed = 400f;

    [Header("Axis Control")]
    public bool isZAxis;
    public bool isNegative;

    private Transform selectedBodyPart;
    private Transform selectedBodyPartUI;
    private Quaternion initialRotation;

    private Vector2 previousPointerPosition;

    public float rotationSpeed = 400f;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction rotateBodyAction;
    private InputAction rotateBodyAction2;

    private InputAction selectHipAction;
    private InputAction selectShoulderRightAction;
    private InputAction selectElbowRightAction;
    private InputAction selectShoulderLeftAction;
    private InputAction selectElbowLeftAction;
    private InputAction selectKneeRightAction;
    private InputAction selectFootRightAction;
    private InputAction selectKneeLeftAction;
    private InputAction selectFootLeftAction;

    private InputAction selectHipAction1;
    private InputAction selectShoulderRightAction1;
    private InputAction selectElbowRightAction1;
    private InputAction selectShoulderLeftAction1;
    private InputAction selectElbowLeftAction1;
    private InputAction selectKneeRightAction1;
    private InputAction selectFootRightAction1;
    private InputAction selectKneeLeftAction1;
    private InputAction selectFootLeftAction1;

    [HideInInspector]
    public bool playerTwo;


    public void Init()
    {
        AssignButtonListeners();
        AdjustInput();  
    }

    void Update()
    {
        HandleJoystickInput();
        HandlePointerInput();  
        if(!playerTwo)
        {
        HandleKeyboardInput();
        HandleKeyboardSelection();
        }else
        {
        HandleKeyboardInput2();
        HandleKeyboardSelection2();
        }
    }

    private void AssignButtonListeners()
    {
        foreach (Button btn in bodyPartButtons.GetComponentsInChildren<Button>())
        {
            string partName = btn.name.Replace("Button_", "");
            btn.onClick.AddListener(() => SelectBodyPartByName(partName));
        }
    }

    private void SelectBodyPartByName(string partName)
    {
        selectedBodyPartUI = GetBodyPartByName(partName);
        if (selectedBodyPartUI != null)
            initialRotation = selectedBodyPartUI.localRotation;
    }

    private Transform GetBodyPartByName(string name)
    {
        return name switch
        {
            "Hip" => hip,
            "ShoulderRight" => shoulderRight,
            "ElbowRight" => elbowRight,
            "ShoulderLeft" => shoulderLeft,
            "ElbowLeft" => elbowLeft,
            "KneeRight" => kneeRight,
            "FootRight" => footRight,
            "KneeLeft" => kneeLeft,
            "FootLeft" => footLeft,
            _ => null,
        };
    }

    private void HandleKeyboardInput()
    {
        if (selectedBodyPartUI == null) return;

        Vector2 input = rotateBodyAction.ReadValue<Vector2>();
        if (input.magnitude > 0.1f)
        {
            float deltaAngle = isZAxis
                ? input.x * rotationSpeed * Time.deltaTime
                : input.x * rotationSpeed * Time.deltaTime;

            deltaAngle = isNegative ? -deltaAngle : deltaAngle;

            Quaternion rotation = isZAxis
                ? Quaternion.Euler(0, 0, deltaAngle)
                : Quaternion.Euler(0, deltaAngle, 0);

            ApplyRotation(rotation);
        }
    }

    private void HandleKeyboardInput2()
    {
        if (selectedBodyPartUI == null) return;

        Vector2 input = rotateBodyAction2.ReadValue<Vector2>();
        if (input.magnitude > 0.1f)
        {
            float deltaAngle = isZAxis
                ? input.x * rotationSpeed * Time.deltaTime
                : input.x * rotationSpeed * Time.deltaTime;

            deltaAngle = isNegative ? -deltaAngle : deltaAngle;

            Quaternion rotation = isZAxis
                ? Quaternion.Euler(0, 0, deltaAngle)
                : Quaternion.Euler(0, deltaAngle, 0);

            ApplyRotation(rotation);
        }
    }

    private void HandleJoystickInput()
    {
        if (selectedBodyPartUI == null) return;

        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        
        if (input.magnitude > 0.1f)
        {
            float deltaAngle = isZAxis
                ? input.x * joystickRotationSpeed * Time.deltaTime
                : input.x * joystickRotationSpeed * Time.deltaTime;

            deltaAngle = isNegative ? -deltaAngle : deltaAngle;
            Quaternion rotation = isZAxis
                ? Quaternion.Euler(0, 0, deltaAngle)
                : Quaternion.Euler(0, deltaAngle, 0);

            ApplyRotation(rotation);
        }
    }

    private void ApplyRotation(Quaternion deltaRotation)
    {
        selectedBodyPartUI.localRotation = Quaternion.Slerp(selectedBodyPartUI.localRotation,
                                                          selectedBodyPartUI.localRotation * deltaRotation,
                                                          Time.deltaTime * joystickRotationSpeed);
    }


    private void AdjustInput()
    {
         foreach(Canvas cvn in gameObject.GetComponentsInChildren<Canvas>()){
            cvn.gameObject.SetActive(true);
        }

        playerInput = GetComponent<PlayerInput>();
       if (playerInput == null)
       {
           playerInput = gameObject.AddComponent<PlayerInput>();
       }

       touchPositionAction = playerInput.actions["TouchPosition"];
       touchPressAction = playerInput.actions["TouchPress"];       
       rotateBodyAction = playerInput.actions["RotateBody"];
       rotateBodyAction2 = playerInput.actions["RotateBody2"];

       selectHipAction = playerInput.actions["Select_Hip"];
    selectShoulderRightAction = playerInput.actions["Select_ShoulderRight"];
    selectElbowRightAction = playerInput.actions["Select_ElbowRight"];
    selectShoulderLeftAction = playerInput.actions["Select_ShoulderLeft"];
    selectElbowLeftAction = playerInput.actions["Select_ElbowLeft"];
    selectKneeRightAction = playerInput.actions["Select_KneeRight"];
    selectFootRightAction = playerInput.actions["Select_FootRight"];
    selectKneeLeftAction = playerInput.actions["Select_KneeLeft"];
    selectFootLeftAction = playerInput.actions["Select_FootLeft"];

    selectHipAction1 = playerInput.actions["Select_Hip1"];
    selectShoulderRightAction1 = playerInput.actions["Select_ShoulderRight1"];
    selectElbowRightAction1 = playerInput.actions["Select_ElbowRight1"];
    selectShoulderLeftAction1 = playerInput.actions["Select_ShoulderLeft1"];
    selectElbowLeftAction1 = playerInput.actions["Select_ElbowLeft1"];
    selectKneeRightAction1 = playerInput.actions["Select_KneeRight1"];
    selectFootRightAction1 = playerInput.actions["Select_FootRight1"];
    selectKneeLeftAction1 = playerInput.actions["Select_KneeLeft1"];
    selectFootLeftAction1 = playerInput.actions["Select_FootLeft1"];
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

    private void HandleKeyboardSelection()
    {
        if (selectHipAction.WasPressedThisFrame()) SelectBodyPartByName("Hip");
        if (selectShoulderRightAction.WasPressedThisFrame()) SelectBodyPartByName("ShoulderRight");
        if (selectElbowRightAction.WasPressedThisFrame()) SelectBodyPartByName("ElbowRight");
        if (selectShoulderLeftAction.WasPressedThisFrame()) SelectBodyPartByName("ShoulderLeft");
        if (selectElbowLeftAction.WasPressedThisFrame()) SelectBodyPartByName("ElbowLeft");
        if (selectKneeRightAction.WasPressedThisFrame()) SelectBodyPartByName("KneeRight");
        if (selectFootRightAction.WasPressedThisFrame()) SelectBodyPartByName("FootRight");
        if (selectKneeLeftAction.WasPressedThisFrame()) SelectBodyPartByName("KneeLeft");
        if (selectFootLeftAction.WasPressedThisFrame()) SelectBodyPartByName("FootLeft");
    }

    private void HandleKeyboardSelection2()
    {
        if (selectHipAction1.WasPressedThisFrame()) SelectBodyPartByName("Hip");
        if (selectShoulderRightAction1.WasPressedThisFrame()) SelectBodyPartByName("ShoulderRight");
        if (selectElbowRightAction1.WasPressedThisFrame()) SelectBodyPartByName("ElbowRight");
        if (selectShoulderLeftAction1.WasPressedThisFrame()) SelectBodyPartByName("ShoulderLeft");
        if (selectElbowLeftAction1.WasPressedThisFrame()) SelectBodyPartByName("ElbowLeft");
        if (selectKneeRightAction1.WasPressedThisFrame()) SelectBodyPartByName("KneeRight");
        if (selectFootRightAction1.WasPressedThisFrame()) SelectBodyPartByName("FootRight");
        if (selectKneeLeftAction1.WasPressedThisFrame()) SelectBodyPartByName("KneeLeft");
        if (selectFootLeftAction1.WasPressedThisFrame()) SelectBodyPartByName("FootLeft");
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