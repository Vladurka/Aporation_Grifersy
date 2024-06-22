using UnityEngine;

namespace FlyingSystem
{
    public class HumanoidAircraftFlyingSystem : MonoBehaviour
    {
        [Header("Object References")]
        public Rigidbody humanoidAircraftRigidbody;

        public Transform rootTransform;

        public Transform springArmTransform;
        public Transform springArmLocalReferenceTransform;

        public Transform rollRootTransform;
        public Transform meshRootTransform;

        [Header("Flying Mode Attributes")]
        public bool freezeMeshRotationX = false;

        [Header("General Attributes")]
        public float normalFlyingSpeed = 30.0f;
        public float maximumFlyingSpeed = 45.0f;
        public float boostAcceleration = 12.0f;
        public float slowDownAcceleration = 20.0f;
        public bool hoverMode = false;

        [Header("Turning Attributes")]
        [Range(0.0f, 100.0f)]
        public float meshYawTurningSmoothingFactor = 4.0f;

        [Header("Horizontal Movement Attributes")]
        public float maximumMeshPitchAngle = 25.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningRecoverySmoothingFactor = 0.5f;

        public float maximumMeshRollAngle = 25.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningRecoverySmoothingFactor = 0.5f;

        [Header("Custom Attributes")]
        public bool calculatePowerConsumption = true;
        public float currentPower = 100.0f;
        public float maximumPower = 800.0f;
        public float powerDecreaseSpeed = 0.1f;
        public float powerDecreaseSpeedWhenBoosting = 0.25f;
        public AnimationCurve speedRemainingPowerRatioAnimationCurve;

        public bool calculateCarryingWeight = true;
        public float currentCarryingWeight = 0.5f;
        public float maximumCarryingWeight = 50.0f;
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
        private Vector3 backupFlyingDirection;

        // Turning variables
        private float targetMeshLocalRotationX, targetMeshLocalRotationY, targetMeshLocalRotationZ;

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

        public void TakeOff()
        {
            inAir = true;
        }

        public void Land()
        {
            inAir = false;
        }

        public void AddYawInput(float value)
        {
            flyingDirection.x += value;
        }

        public void AddPitchInput(float value)
        {
            flyingDirection.y += value;
        }

        public void AddWeight(float increaseValue)
        {
            currentCarryingWeight += Mathf.Clamp(currentCarryingWeight + increaseValue, 0.0f, maximumCarryingWeight);
            weightPercentage = currentCarryingWeight / maximumCarryingWeight;
        }

        void Fly()
        {
            rootTransform.rotation = Quaternion.Lerp(rootTransform.localRotation, Quaternion.Euler(0.0f, rootTransform.rotation.eulerAngles.y, 0.0f), 2.0f * Time.deltaTime);

            if (calculatePowerConsumption)
            {
                // Power reduces faster when it is boosting
                if (inAir)
                {
                    if (boosting)
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeed * Time.deltaTime, 0.0f, maximumPower);
                    else
                        currentPower = Mathf.Clamp(currentPower - powerDecreaseSpeedWhenBoosting * Time.deltaTime, 0.0f, maximumPower);

                    powerPercentage = currentPower / maximumPower;
                    powerFactor = speedRemainingPowerRatioAnimationCurve.Evaluate(1.0f - powerPercentage);
                }
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

            if (Mathf.Abs(flyingDirection.x) > 0.01f || Mathf.Abs(flyingDirection.y) > 0.01f)
            {
                // Rotation
                flyingDirection = flyingDirection.normalized;

                if (flyingDirection.x > 0.0f)
                    targetMeshLocalRotationZ = -maximumMeshRollAngle * flyingDirection.x;
                else
                    targetMeshLocalRotationZ = maximumMeshRollAngle * -flyingDirection.x;

                if (!freezeMeshRotationX)
                {
                    if (DeltaAngle(springArmTransform.rotation.eulerAngles.y, meshRootTransform.rotation.eulerAngles.y) < 90.0f)
                        targetMeshLocalRotationX = maximumMeshPitchAngle * flyingDirection.y;
                    else
                        targetMeshLocalRotationX = maximumMeshPitchAngle * -flyingDirection.y;
                }
                else
                    targetMeshLocalRotationX = 0.0f;

                targetMeshLocalRotationY = springArmTransform.rotation.eulerAngles.y;

                // Lerp the pitch, yaw, roll to their target values
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, targetMeshLocalRotationY, rollRootTransform.localRotation.eulerAngles.z), meshYawTurningSmoothingFactor * Time.deltaTime);
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, targetMeshLocalRotationZ), meshRollTurningSmoothingFactor * Time.deltaTime);
                meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(targetMeshLocalRotationX, 0.0f, 0.0f), meshPitchTurningSmoothingFactor * Time.deltaTime);

                // Position
                springArmLocalReferenceTransform.localPosition = new Vector3(flyingDirection.x, 0.0f, flyingDirection.y) * 10000.0f;

                if (boosting)
                    currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, 0.0f, maximumFlyingSpeed);
                else
                {
                    if (currentFlyingSpeed > normalFlyingSpeed)
                        currentFlyingSpeed -= boostAcceleration * Time.deltaTime;
                    else
                        currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed + boostAcceleration * Time.deltaTime, 0.0f, normalFlyingSpeed);
                }

                flyingDirection = (springArmLocalReferenceTransform.position - rootTransform.position);

                if (!hoverMode)
                    flyingDirection.y = 0.0f;

                flyingDirection = flyingDirection.normalized;

                flyingSpeed = currentFlyingSpeed * powerFactor * weightFactor;
                flyingVelocity = flyingDirection * currentFlyingSpeed;

                if (!hoverMode)
                    humanoidAircraftRigidbody.velocity = new Vector3(flyingVelocity.x, humanoidAircraftRigidbody.velocity.y, flyingVelocity.z);
                else
                    humanoidAircraftRigidbody.velocity = flyingVelocity;

                backupFlyingDirection = flyingDirection;

                flyingDirection = Vector3.zero;
            }
            else
            {
                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, 0.0f), meshRollTurningRecoverySmoothingFactor * Time.deltaTime);
                meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), meshPitchTurningRecoverySmoothingFactor * Time.deltaTime);

                currentFlyingSpeed = Mathf.Clamp(currentFlyingSpeed - slowDownAcceleration * Time.deltaTime, 0.0f, maximumFlyingSpeed);

                flyingDirection = backupFlyingDirection;

                flyingSpeed = currentFlyingSpeed * powerFactor * weightFactor;
                flyingVelocity = flyingDirection * currentFlyingSpeed;

                if (!hoverMode)
                    humanoidAircraftRigidbody.velocity = new Vector3(flyingVelocity.x, humanoidAircraftRigidbody.velocity.y, flyingVelocity.z);
                else
                    humanoidAircraftRigidbody.velocity = flyingVelocity;

                flyingDirection = Vector3.zero;
            }
        }

        float DeltaAngle(float angle1, float angle2)
        {
            // Return the positive delta angle
            if (angle1 > 270.0f && angle2 < 90.0f)
                return angle2 + 360.0f - angle1;
            else if (angle1 < 90.0f && angle2 > 270.0f)
                return angle1 + 360.0f - angle2;
            else
                return Mathf.Abs(angle2 - angle1);
        }
    }
}