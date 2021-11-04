using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float acceleration;
    public float turnSpeed;

    public Transform carModel;
    private Vector3 startModelOffset;

    public float groundCheckRate;
    private float lastGroundCheckTime;

    private float currentYRotation;

    public bool canControl;

    private bool accelerateInput;
    private float turnInput;

    public TrackZone currentTrackZone;
    public int targetTrackZone = 0;
    public int zonesPassed;
    public int racePosition;
    public int currentLap;

    public Rigidbody rig;

    private void Start()
    {
        startModelOffset = carModel.transform.localPosition;
        GameManager.instance.cars.Add(this);
        transform.position = GameManager.instance.spawnPoints[GameManager.instance.cars.Count - 1].position;
    }

    private void Update()
    {
        if (!canControl)
            turnInput = 0;

        float turnRate = Vector3.Dot(rig.velocity.normalized, carModel.forward);
        turnRate = Mathf.Abs(turnRate);

        currentYRotation += turnInput * turnSpeed * turnRate * Time.deltaTime;

        carModel.position = transform.position + startModelOffset;

        CheckGround();
    }

    private void FixedUpdate()
    {
        if (!canControl)
            return;

        if (accelerateInput)
        {
            rig.AddForce(carModel.forward * acceleration, ForceMode.Acceleration);
        }
    }

    private void CheckGround()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, -0.75f, 0), Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.0f))
        {
            carModel.up = hit.normal;
        }
        else
        {
            carModel.up = Vector3.up;
        }

        carModel.Rotate(new Vector3(0, currentYRotation, 0), Space.Self);
    }

    public void OnAccelerateInput(InputAction.CallbackContext context)
    {
        accelerateInput = context.phase == InputActionPhase.Performed;
    }

    public void OnTurnInput(InputAction.CallbackContext context)
    {
        turnInput = context.ReadValue<float>();
    }
}
