using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RagdollJoystick : MonoBehaviour
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
    public float joystickRotationSpeed = 100f;

    [Header("Axis Control")]
    public bool isZAxis;
    public bool isNegative;

    private Transform selectedBodyPart;
    private Quaternion initialRotation;

    void Start()
    {
        AssignButtonListeners();
    }

    void Update()
    {
        HandleJoystickInput();
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
        selectedBodyPart = GetBodyPartByName(partName);
        if (selectedBodyPart != null)
            initialRotation = selectedBodyPart.localRotation;
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

    private void HandleJoystickInput()
    {
        if (selectedBodyPart == null) return;

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
        selectedBodyPart.localRotation = Quaternion.Slerp(selectedBodyPart.localRotation,
                                                          selectedBodyPart.localRotation * deltaRotation,
                                                          Time.deltaTime * joystickRotationSpeed);
    }
}
