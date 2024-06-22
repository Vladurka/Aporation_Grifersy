using UnityEngine;

namespace FlyingSystem
{
    public class AircraftFlyingSystem : MonoBehaviour
    {
        [Header("Object References")]
        public Rigidbody aircraftRigidbody;

        public Transform rootTransform;
        public Transform rollRootTransform;
        public Transform meshRootTransform;

        [Header("General Attributes")]
        public float minimumTakeOffSpeed = 79.5f;
        public float normalFlyingSpeed = 80.0f;
        public float maximumFlyingSpeed = 110.0f;
        public float boostAcceleration = 15.0f;

        [Header("Turning Attributes")]
        public float turningSpeed = 25.0f;
        public float meshYawTurningSpeed = 10.0f;
        [Range(0.0f, 100.0f)]
        public float meshYawTurningSmoothingFactor = 0.5f;
        public float maximumMeshYawAngle = 5.0f;
        public float meshPitchTurningSpeed = 20.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningSmoothingFactor = 2.0f;
        public float meshRollTurningSpeed = 75.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningSmoothingFactor = 1.5f;

        [Header("Custom Attributes")]
        public bool calculatePowerConsumption = true;
        public float currentPower = 1000.0f;
        public float maximumPower = 1000.0f;
        public float powerDecreaseSpeed = 0.1f;
        public float powerDecreaseSpeedWhenBoosting = 0.25f;
        public AnimationCurve speedRemainingPowerRatioAnimationCurve;

        public bool calculateCarryingWeight = true;
        public float currentCarryingWeight = 0.5f;
        public float maximumCarryingWeight = 2000.0f;
        public AnimationCurve speedCarryingWeightRatioAnimationCurve;

        // Flying attributes
        [HideInInspector]
        public bool enabledFlyingLogic = true;

        [HideInInspector]
        public bool inAir = false;

        [HideInInspector]
        public Vector3 flyingDirection;

        [HideInInspector]
        public float flyingSpeed;

        [HideInInspector]
        public Vector3 flyingVelocity;

        [HideInInspector]
        public bool flyingAtNormalSpeed = false;

        [HideInInspector]
        public bool boosting = false;

        private float currentFlyingSpeed;

        // Turning variables
        private float targetMeshLocalRotationX, targetMeshLocalRotationY, targetMeshLocalRotationZ;
        private float totalTurningDegree;

        [HideInInspector]
        public float powerPercentage = 1.0f;
        private float powerFactor = 1.0f;

        [HideInInspector]
        public float weightPercentage = 0.0f;
        private float weightFactor = 1.0f;

        void Update()
        {
            if (enabledFlyingLogic)
                Fly();
        }

        public bool TakeOff(float groundMovementSpeed)
        {
            if (groundMovementSpeed > minimumTakeOffSpeed)
            {
                currentFlyingSpeed = normalFlyingSpeed;

                inAir = true;
            }

            return inAir;
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
            targetMeshLocalRotationX -= value * meshPitchTurningSpeed * Time.deltaTime;

        }

        public void AddRollInput(float value)
        {
            targetMeshLocalRotationZ -= value * meshRollTurningSpeed * Time.deltaTime;
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
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, 0.0f, targetMeshLocalRotationZ), meshRollTurningSmoothingFactor * Time.deltaTime);
                meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(targetMeshLocalRotationX, meshRootTransform.localRotation.eulerAngles.y, meshRootTransform.localRotation.eulerAngles.z), meshPitchTurningSmoothingFactor * Time.deltaTime);
                meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(meshRootTransform.localRotation.eulerAngles.x, targetMeshLocalRotationY, meshRootTransform.localRotation.eulerAngles.z), meshYawTurningSmoothingFactor * Time.deltaTime);

                if (calculatePowerConsumption)
                {
                    // Power reduces faster when it is boosting
                    if (boosting)
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeed * Time.deltaTime, 0.0f, maximumPower);
                    else
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeedWhenBoosting * Time.deltaTime, 0.0f, maximumPower);

                    powerPercentage = currentPower / maximumPower;
                    powerFactor = speedRemainingPowerRatioAnimationCurve.Evaluate(1.0f - powerPercentage);
                }
                else
                    powerFactor = 1.0f;

                if (calculateCarryingWeight)
                {
                    // Carrying too much weight will slow down the speed
                    weightPercentage = currentCarryingWeight / maximumCarryingWeight;
                    weightFactor = speedCarryingWeightRatioAnimationCurve.Evaluate(weightPercentage);
                }
                else
                    weightFactor = 1.0f;

                if (boosting)
                    currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, 0.0f, maximumFlyingSpeed);
                else
                {
                    if (currentFlyingSpeed > normalFlyingSpeed)
                        currentFlyingSpeed -= boostAcceleration * Time.deltaTime;
                    else
                        currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, 0.0f, normalFlyingSpeed);
                }

                flyingSpeed = currentFlyingSpeed * powerFactor * weightFactor;

                flyingVelocity = meshRootTransform.forward * flyingSpeed;

                aircraftRigidbody.velocity = flyingVelocity;

                // Total turning degree is the sum of yaw and roll
                totalTurningDegree = targetMeshLocalRotationY - targetMeshLocalRotationZ;

                // Current it cannot turn more than 180 degrees
                if (Mathf.Abs(totalTurningDegree) > 180.0f)
                    totalTurningDegree = -totalTurningDegree % 180.0f;

                rootTransform.Rotate(Vector3.up * turningSpeed * Mathf.Clamp(totalTurningDegree / 180.0f, -1.0f, 1.0f) * Time.deltaTime);
            }
        }
    }
}