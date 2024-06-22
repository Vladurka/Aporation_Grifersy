using UnityEngine;

namespace FlyingSystem
{
    public class HelicopterFlyingSystem : MonoBehaviour
    {
        [Header("Object References")]
        public Rigidbody rootRigidbody;

        public Transform rootTransform;

        public Transform springArmTransform;
        public Transform springArmLocalReferenceTransform;

        public Transform rollRootTransform;
        public Transform meshRootTransform;

        [Header("Turning Attributes")]
        public float meshYawTurningSpeed = 60.0f;
        [Range(0.0f, 100.0f)]
        public float meshYawTurningSmoothingFactor = 2.0f;

        [Header("Horizontal Movement Attributes")]
        public float normalHorizontalFlyingSpeed = 60.0f;
        public float maximumHorizontalFlyingSpeed = 90.0f;
        public float horizontalBoostAcceleration = 25.0f;
        public float horizontalSlowDownAcceleration = 20.0f;

        public float maximumMeshPitchAngle = 25.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshPitchTurningRecoverySmoothingFactor = 0.25f;

        public float maximumMeshRollAngle = 25.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningSmoothingFactor = 2.0f;
        [Range(0.0f, 100.0f)]
        public float meshRollTurningRecoverySmoothingFactor = 0.25f;

        // The angle between the helicopter speed direction and tail rotor pushing direction
        public AnimationCurve speedTailRotorAngleRatioAnimationCurve;

        [Header("Vertical Movement Attributes")]
        public float normalVerticalFlyingSpeed = 20.0f;
        public float maximumVerticalFlyingSpeed = 35.0f;
        public float verticalBoostAcceleration = 25.0f;

        [Header("Custom Attributes")]
        public bool calculatePowerConsumption = true;
        public float currentPower = 100.0f;
        public float maximumPower = 800.0f;
        public float powerDecreaseSpeed = 0.1f;
        public float powerDecreaseSpeedWhenBoosting = 0.25f;
        public AnimationCurve speedRemainingPowerRatioAnimationCurve;

        public bool calculateCarryingWeight = true;
        public float currentCarryingWeight = 0.5f;
        public float maximumCarryingWeight = 600.0f;
        public AnimationCurve speedCarryingWeightRatioAnimationCurve;

        // Flying attributes
        [HideInInspector]
        public bool enabledFlyingLogic = true;

        [HideInInspector]
        public bool inAir = false;

        [HideInInspector]
        public Vector3 flyingDirection;

        [HideInInspector]
        public float horizontalFlyingSpeed;

        [HideInInspector]
        public float verticalFlyingSpeed;

        [HideInInspector]
        public bool flyingAtNormalSpeed = false;
        [HideInInspector]
        public bool boosting = false;

        private float currentHorizontalFlyingSpeed;

        // Turning variables
        private float targetMeshLocalRotationX, targetMeshLocalRotationY, targetMeshLocalRotationZ;

        // Vertical movement variables
        private bool verticalMoving = false;
        private bool ascending = false;
        private bool verticalSlowingDown = false;
        private bool horizontalMoving = false;

        private float currentVerticalFlyingSpeed;

        private float angleFactor = 1.0f;

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

        public void VerticalSlowDown()
        {
            verticalMoving = false;
            verticalSlowingDown = true;
        }

        public void AddYawInput(float value)
        {
            targetMeshLocalRotationY += value * meshYawTurningSpeed * Time.deltaTime;
        }

        public void StopYawInput()
        {
            horizontalMoving = false;
        }

        public void AddHorizontalInput(Vector2 direction)
        {
            horizontalMoving = true;

            direction = direction.normalized;

            if (DeltaAngle(springArmTransform.rotation.eulerAngles.y, meshRootTransform.rotation.eulerAngles.y) < 90.0f)
            {
                if (direction.x > 0.0f)
                    targetMeshLocalRotationZ = -maximumMeshRollAngle * direction.x;
                else
                    targetMeshLocalRotationZ = maximumMeshRollAngle * -direction.x;

                targetMeshLocalRotationX = maximumMeshPitchAngle * direction.y;
            }
            else
            {
                if (direction.x > 0.0f)
                    targetMeshLocalRotationZ = -maximumMeshRollAngle * -direction.x;
                else
                    targetMeshLocalRotationZ = maximumMeshRollAngle * direction.x;

                targetMeshLocalRotationX = maximumMeshPitchAngle * -direction.y;
            }

            // Use the reference point(subobject of spring arm) to calculate the flying direction
            springArmLocalReferenceTransform.localPosition = new Vector3(direction.x, 0.0f, direction.y) * 10000.0f;

            flyingDirection = (springArmLocalReferenceTransform.position - rootTransform.position);
            flyingDirection.y = 0.0f;
            flyingDirection = flyingDirection.normalized;
        }

        public void AddVerticalInput(float value)
        {
            verticalMoving = true;
            verticalSlowingDown = false;

            if (value > 0.01f)
                ascending = true;
            else
                ascending = false;
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
                rootTransform.rotation = Quaternion.Lerp(rootTransform.localRotation, Quaternion.Euler(0.0f, rootTransform.rotation.eulerAngles.y, 0.0f), 2.0f * Time.deltaTime);

                rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, targetMeshLocalRotationY, rollRootTransform.localRotation.eulerAngles.z), meshYawTurningSmoothingFactor * Time.deltaTime);

                angleFactor = speedTailRotorAngleRatioAnimationCurve.Evaluate(Vector3.Angle(new Vector3(flyingDirection.x, 0.0f, flyingDirection.z), new Vector3(meshRootTransform.forward.x, 0.0f, meshRootTransform.forward.z)) / 180.0f);

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

                if (horizontalMoving)
                {
                    rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, targetMeshLocalRotationZ), meshRollTurningSmoothingFactor * Time.deltaTime);
                    meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(targetMeshLocalRotationX, 0.0f, 0.0f), meshPitchTurningSmoothingFactor * Time.deltaTime);

                    if (boosting)
                        currentHorizontalFlyingSpeed = Mathf.Clamp(currentHorizontalFlyingSpeed + horizontalBoostAcceleration * Time.deltaTime, 0.0f, maximumHorizontalFlyingSpeed);
                    else
                    {
                        if (currentHorizontalFlyingSpeed > normalHorizontalFlyingSpeed)
                            currentHorizontalFlyingSpeed -= horizontalBoostAcceleration * Time.deltaTime;
                        else
                            currentHorizontalFlyingSpeed = Mathf.Clamp(currentHorizontalFlyingSpeed + horizontalBoostAcceleration * Time.deltaTime, 0.0f, normalHorizontalFlyingSpeed);
                    }

                    horizontalFlyingSpeed = currentHorizontalFlyingSpeed * angleFactor * powerFactor * weightFactor;

                    rootRigidbody.velocity = flyingDirection * horizontalFlyingSpeed;
                }
                else
                {
                    rollRootTransform.localRotation = Quaternion.Lerp(rollRootTransform.localRotation, Quaternion.Euler(0.0f, rollRootTransform.localRotation.eulerAngles.y, 0.0f), meshRollTurningRecoverySmoothingFactor * Time.deltaTime);
                    meshRootTransform.localRotation = Quaternion.Lerp(meshRootTransform.localRotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), meshPitchTurningRecoverySmoothingFactor * Time.deltaTime);

                    currentHorizontalFlyingSpeed = Mathf.Clamp(currentHorizontalFlyingSpeed - horizontalSlowDownAcceleration * Time.deltaTime, 0.0f, normalHorizontalFlyingSpeed);

                    horizontalFlyingSpeed = currentHorizontalFlyingSpeed * angleFactor * powerFactor * weightFactor;

                    rootRigidbody.velocity = flyingDirection * horizontalFlyingSpeed;
                }

                if (verticalMoving)
                {
                    if (ascending)
                    {
                        if (boosting)
                            currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed + verticalBoostAcceleration * Time.deltaTime, 0.0f, maximumVerticalFlyingSpeed);
                        else
                        {
                            if (currentVerticalFlyingSpeed > normalVerticalFlyingSpeed)
                                currentVerticalFlyingSpeed -= verticalBoostAcceleration * Time.deltaTime;
                            else
                                currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed + verticalBoostAcceleration * Time.deltaTime, 0.0f, normalVerticalFlyingSpeed);
                        }
                    }
                    else
                    {
                        if (boosting)
                            currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed - verticalBoostAcceleration * Time.deltaTime, -maximumVerticalFlyingSpeed, maximumVerticalFlyingSpeed);
                        else
                        {
                            if (currentVerticalFlyingSpeed < -normalVerticalFlyingSpeed)
                                currentVerticalFlyingSpeed += verticalBoostAcceleration * Time.deltaTime;
                            else
                                currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed - verticalBoostAcceleration * Time.deltaTime, -normalVerticalFlyingSpeed, normalVerticalFlyingSpeed);
                        }
                    }

                    verticalFlyingSpeed = currentVerticalFlyingSpeed * angleFactor * powerFactor * weightFactor;

                    rootRigidbody.velocity += new Vector3(0.0f, currentVerticalFlyingSpeed, 0.0f);
                }
                else if (verticalSlowingDown)
                {
                    if (currentVerticalFlyingSpeed < 0.0f)
                        currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed + verticalBoostAcceleration * Time.deltaTime, -maximumVerticalFlyingSpeed, 0.0f);
                    else
                        currentVerticalFlyingSpeed = Mathf.Clamp(currentVerticalFlyingSpeed - verticalBoostAcceleration * Time.deltaTime, 0.0f, maximumVerticalFlyingSpeed);

                    verticalFlyingSpeed = currentVerticalFlyingSpeed * angleFactor * powerFactor * weightFactor;

                    rootRigidbody.velocity += new Vector3(0.0f, currentVerticalFlyingSpeed, 0.0f);
                }
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