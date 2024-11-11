using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour {
    

    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    //[SerializeField] Transform frontRightTransform;
    //[SerializeField] Transform frontLeftTransform;
    //[SerializeField] Transform backRightTransform;
    //[SerializeField] Transform backLeftTransform;

    [SerializeField] private TMP_Text speedText;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CountDownTimer countDownTimer;

    // Use Input Manager in Unity to see which Input to type
    [SerializeField] private string inputNameHorizontal;
    [SerializeField] private string inputNameVertical;
    [SerializeField] private string inputBrake;

    // Reference to the FinishLineChecker object
    public FinishlineChecker finishLineChecker;

    public float acceleration = 500f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 15f;
    public float DownForceValue = 5000f;

    private float currentAcceleration = 0f;
    private float currentBrakeForce = 0f;
    private float currentTurnAngle = 0f;

    private void Update() {
        // This is a m/s to km/h conversion
        speedText.text = (rb.velocity.magnitude * 2.23693629f).ToString("0") + (" kmph");
    }

    private void FixedUpdate() {
        if (countDownTimer != null && countDownTimer.currentTime <= 0) {
            // Get forward/reverse acceleration from vertical axis (W and S)
            currentAcceleration = acceleration * Input.GetAxis(inputNameVertical);

            // Press Space to brake
            if (Input.GetButton(inputBrake))
                currentBrakeForce = brakingForce;
            else
                currentBrakeForce = 0;

            // Apply acceleration to front wheels
            ApplyAcceleration();

            // Apply brakes to all wheels
            ApplyBrakes();

            // Take care of turning
            currentTurnAngle = maxTurnAngle * Input.GetAxis(inputNameHorizontal);
            frontLeft.steerAngle = currentTurnAngle;
            frontRight.steerAngle = currentTurnAngle;

            // Update wheel meshes
            //UpdateWheel(frontLeft, frontLeftTransform);
            //UpdateWheel(frontRight, frontRightTransform);
            //UpdateWheel(backLeft, backLeftTransform);
            //UpdateWheel(backRight, backRightTransform);

            AddDownForce();
        }
    }

    private void ApplyAcceleration() {
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
    }

    private void ApplyBrakes() {
        frontRight.brakeTorque = currentBrakeForce;
        frontLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;
    }


    //private void UpdateWheel(WheelCollider col, Transform trans) {
    //    // Get wheel collider state
    //    Vector3 position;
    //    Quaternion rotation;
    //    col.GetWorldPose(out position, out rotation);

    //    // Set wheel transform
    //    trans.position = position;
    //    trans.rotation = rotation;
    //}

   private void AddDownForce()
    {
        rb.AddForce(-transform.up * DownForceValue * rb.velocity.magnitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            // Get the car's name or identifier
            string carName = gameObject.name; // Assuming the car's name is used as an identifier

            // Get the current race time or some time to represent the finish time
            float finishTime = Time.time; // Example: Use Time.time to represent the finish time

            // Call the FinishLineChecker method to register the car's finish
            finishLineChecker.ParticipantCrossedFinishLine(carName, finishTime);
        }
    }

}
