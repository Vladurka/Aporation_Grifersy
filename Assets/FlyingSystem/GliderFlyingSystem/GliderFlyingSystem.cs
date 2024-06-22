using UnityEngine;

namespace FlyingSystem
{
    public class GliderFlyingSystem : MonoBehaviour
    {
        [Header("Object References")]
        public Rigidbody gliderRigidbody;

        public Transform rootTransform;
        public Transform rollRootTransform;
        public Transform meshRootTransform;

        [Header("General Attributes")]
        [Range(0.0f, 10.0f)]
        public float airDrag = 9.75f;

        [Header("Turning Attributes")]
        public float turningSpeed = 45.0f;
        public float meshYawTurningSpeed = 45.0f;
        [Range(0.0f, 100.0f)]
        public float meshYawTurningSmoothingFactor = 2.0f;
        public float maximumMeshYawAngle = 15.0f;
        public float meshPitchTurningSpeed = 60.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningSmoothingFactor = 4.0f;
        public float maximumMeshRollAngle = 30.0f;
        public float meshRollTurningSpeed = 30.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningSmoothingFactor = 2.0f;
        public bool lookUpWhenEnteringAirflow = true;
        public float lookUpAngle = 15.0f;

        [Header("Diving Attributes")]
        public bool canDive = true;
        public float maximumDivingSpeed = 500.0f;
        [Range(0.0f, 90.0f)]
        public float divingStartAngle = 30.0f;
        public float decelerationAfterDiving = 2.0f;

        [Header("Custom Attributes")]
        public float g = 9.8f;
        public bool calculateCarryingWeight = true;
        public float currentCarryingWeight = 0.5f;
        public float maximumCarryingWeight = 100.0f;
        public AnimationCurve speedCarryingWeightRatioAnimationCurve;

        // Flying attributes
        [HideInInspector]
        public bool enabledFlyingLogic = true;

        [HideInInspector]
        public bool inAir = false;

        [HideInInspector]
        public bool inAirflow = false;

        [HideInInspector]
        public Vector3 flyingDirection;

        [HideInInspector]
        public float originalFlyingSpeed;

        [HideInInspector]
        public float flyingSpeed;

        [HideInInspector]
        public Vector3 flyingVelocity;

        private float currentFlyingSpeed;

        // Turning variables
        private float targetMeshLocalRotationX, targetMeshLocalRotationY, targetMeshLocalRotationZ;
        private float totalTurningDegree;

        // Diving variables
        [HideInInspector]
        public bool diving = false;
        private float verticalAcceleration;
        private float verticalSpeed;

        // Airflow variables
        private float airflowSpeed;
        private float airflowIntensity;
        private float airflowAcceleration;
        private float airflowFadeOutAcceleration;

        [HideInInspector]
        public float weightPercentage = 0.0f;
        private float weightFactor = 1.0f;

        void Update()
        {
            if (enabledFlyingLogic)
                Fly();
        }

        public void TakeOff(float takeOffSpeed)
        {
            originalFlyingSpeed = takeOffSpeed;
            currentFlyingSpeed = takeOffSpeed;

            inAir = true;
        }

        public void Land()
        {
            inAir = false;
        }

        public void AddYawInput(float value)
        {
            targetMeshLocalRotationY = Mathf.Clamp(targetMeshLocalRotationY + value * meshYawTurningSpeed * Time.deltaTime, -maximumMeshYawAngle, maximumMeshYawAngle);
        }

        public void StopYawInput()
        {
            targetMeshLocalRotationY = 0.0f;
        }

        public void AddPitchInput(float value)
        {
            targetMeshLocalRotationX = Mathf.Clamp(targetMeshLocalRotationX + value * meshPitchTurningSpeed * Time.deltaTime, -89.995f, 89.995f);
        }

        public void AddRollInput(float value)
        {
            targetMeshLocalRotationZ = Mathf.Clamp(targetMeshLocalRotationZ - value * meshRollTurningSpeed * Time.deltaTime, -maximumMeshRollAngle, maximumMeshRollAngle);
        }

        public void AddAirflowForce(float intensity, float acceleration, float fadeOutAcceleration)
        {
            airflowIntensity = intensity;
            airflowAcceleration = acceleration;
            airflowFadeOutAcceleration = fadeOutAcceleration;

            targetMeshLocalRotationX = Mathf.Clamp(targetMeshLocalRotationX - lookUpAngle, -89.995f, 89.995f);

            inAirflow = true;
        }

        public void EndAirflowForce()
        {
            inAirflow = false;
        }

        public void AddWeight(float increaseValue)
        {
            currentCarryingWeight += Mathf.Clamp(currentCarryingWeight + increaseValue, 0.0f, maximumCarryingWeight);
            weightPercentage = currentCarryingWeight / maximumCarryingWeight;
        }

        void Fly()
        {
            if (inAir)
            {
                // Lerp the pitch, yaw, roll to their target values
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, targetMeshLocalRotationY, rollRootTransform.localRotation.eulerAngles.z), meshRollTurningSmoothingFactor * Time.deltaTime);
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, targetMeshLocalRotationZ), meshRollTurningSmoothingFactor * Time.deltaTime);
                meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(targetMeshLocalRotationX, 0.0f, 0.0f), meshPitchTurningSmoothingFactor * Time.deltaTime);

                if (!inAirflow && canDive)
                {
                    // Diving angle should be greater than minimum diving angle and smaller than 90 degrees(straight down)
                    if (meshRootTransform.localRotation.eulerAngles.x < 89.995f && meshRootTransform.localRotation.eulerAngles.x > divingStartAngle)
                    {
                        if (!diving)
                        {
                            diving = true;
                            originalFlyingSpeed = currentFlyingSpeed;
                        }

                        // The closer to gravity direction(straight down), the faster accleration
                        currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + g * Mathf.Sin(meshRootTransform.localRotation.eulerAngles.x * Mathf.Deg2Rad) * Time.deltaTime, 0.0f, maximumDivingSpeed);
                    }
                    else
                    {
                        diving = false;

                        // The closer to horizontal direction, the greater air drag for vertical direction
                        if (verticalSpeed < -0.1f)
                            verticalSpeed += airDrag * Mathf.Cos((meshRootTransform.localRotation.eulerAngles.x) * Mathf.Deg2Rad) * Time.deltaTime;
                        else
                            verticalSpeed = 0.0f;

                        // Facing angle greater than horizon will decrease flying speed
                        if (meshRootTransform.localRotation.eulerAngles.x > 270.0f)
                        {
                            if (currentFlyingSpeed > originalFlyingSpeed)
                                currentFlyingSpeed -= (decelerationAfterDiving + decelerationAfterDiving * Mathf.Sin((360.0f - meshRootTransform.localRotation.eulerAngles.x) * Mathf.Deg2Rad)) * Time.deltaTime;
                        }
                        else
                        {
                            if (currentFlyingSpeed > originalFlyingSpeed)
                                currentFlyingSpeed -= decelerationAfterDiving * Time.deltaTime;
                        }
                    }
                }

                if (calculateCarryingWeight)
                {
                    // Carrying too much weight will slow down the speed
                    weightPercentage = currentCarryingWeight / maximumCarryingWeight;
                    weightFactor = speedCarryingWeightRatioAnimationCurve.Evaluate(weightPercentage);
                }
                else
                    weightFactor = 1.0f;

                flyingDirection = meshRootTransform.forward;

                flyingSpeed = currentFlyingSpeed * weightFactor;
                flyingVelocity = flyingDirection * flyingSpeed;
                gliderRigidbody.velocity = flyingVelocity;

                if (inAirflow)
                {
                    // Airflow can lift up the flyer
                    airflowSpeed = Mathf.Clamp(airflowSpeed + airflowAcceleration * Time.deltaTime, 0.0f, airflowIntensity);
                    gliderRigidbody.velocity = flyingVelocity + Vector3.up * airflowSpeed;
                }
                else
                {
                    // Due to the nature of glider, pointing up won't fly upward
                    if (flyingDirection.y > 0.001f && currentFlyingSpeed < originalFlyingSpeed + 0.001f)
                        flyingDirection = new Vector3(flyingDirection.x, 0.0f, flyingDirection.z);

                    // Air drag(reaches maximum when completely horizontal) can cancel out part of the gravity
                    verticalAcceleration = -g + airDrag * Mathf.Cos((meshRootTransform.localRotation.eulerAngles.x) * Mathf.Deg2Rad);
                    verticalSpeed += verticalAcceleration * Time.deltaTime;

                    if (airflowSpeed > 0.01f)
                    {
                        // Airflow boost will fade out after leaving airflow
                        airflowSpeed -= airflowFadeOutAcceleration * Time.deltaTime;
                        gliderRigidbody.velocity = flyingVelocity + new Vector3(0.0f, Mathf.Clamp(verticalSpeed - verticalAcceleration * Time.deltaTime, -99999999.0f, 0.0f), 0.0f) + Vector3.up * airflowSpeed;
                    }
                    else
                        gliderRigidbody.velocity = flyingVelocity + new Vector3(0.0f, Mathf.Clamp(verticalSpeed - verticalAcceleration * Time.deltaTime, -99999999.0f, 0.0f), 0.0f);
                }

                totalTurningDegree = targetMeshLocalRotationY - targetMeshLocalRotationZ;

                if (Mathf.Abs(totalTurningDegree) > 180.0f)
                    totalTurningDegree = -totalTurningDegree % 180.0f;

                rootTransform.Rotate(Vector3.up * turningSpeed * Mathf.Clamp(totalTurningDegree / 180.0f, -1.0f, 1.0f) * Time.deltaTime);
            }
        }
    }
}