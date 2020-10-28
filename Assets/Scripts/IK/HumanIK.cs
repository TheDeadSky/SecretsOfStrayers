using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    public class HumanIK : MonoBehaviour
    {
        private Animator anim;
        private Vector3 leftFootPos, leftFootIKPos, rightFootPos, rightFootIKPos;
        private Quaternion leftFootIKRot, rightFootIKRot;
        private float leftFootWeight, rightFootWeight;
        private float lastPelvisPosY, lastRFootPosY, lastLFootPosY;
        private Vector3 lastLookupPosition;

        [Header("Head Look IK")]

        [SerializeField] private float lookIKWeight;
        [SerializeField] private float eyesWeight;
        [SerializeField] private float headWeight;
        [SerializeField] private float bodyWeight;
        [SerializeField] private float clampWeight;

        //[SerializeField] private Transform targetTransform;

        [Header("Feet Grounder")]

        [SerializeField] private bool enableFeetIK = true;
        [Range(0, 2)] [SerializeField] private float heightFromGroundRaycast = 1.14f;
        [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f;
        [SerializeField] private LayerMask environmentLayer;
        [SerializeField] private float pelvisOffset = 0f;
        [Range(0, 1)] [SerializeField] private float pelvisUpDownSpeed = 0.25f;
        [Range(0, 1)] [SerializeField] private float feetToIKPosSpeed = 0.5f;

        //[SerializeField] private string lFootAnimVarName = "LeftFoot";
        //[SerializeField] private string rFootAnimVarName = "RightFoot";

        [SerializeField] private bool useProIKFeature = false;
        [SerializeField] private bool showSolverDebug = false;

        [SerializeField] private PlayerStates playerStates;

        [Range(-100, 100)] [SerializeField] private float bpos = 0f;

        private void Start()
        {
            anim = GetComponent<Animator>();
            playerStates = GetComponent<PlayerStates>();
        }

        /// <summary>
        /// Updating the AdjustFeetTarget method and also find the position
        /// of each foot inside our Solver Position
        /// </summary>
        private void FixedUpdate()
        {
            if (enableFeetIK == false) return;
            if (anim == null) return;
            if (GameMenuActions.GamePaused) return;

            AdjustFeetTarget(ref rightFootPos, HumanBodyBones.RightFoot);
            AdjustFeetTarget(ref leftFootPos, HumanBodyBones.LeftFoot);

            //find and raycast to the ground to find positions
            FeetPosSolver(rightFootPos, ref rightFootIKPos, ref rightFootIKRot); // handle the solver for right foot
            FeetPosSolver(leftFootPos, ref leftFootIKPos, ref leftFootIKRot); // handle the solver for left foot
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (enableFeetIK == false) return;
            if (anim == null) return;
            //if (GameMenuActions.GamePaused) return;

            raycastDownDistance = anim.GetFloat("raycastDownDistance");

            anim.SetLookAtWeight(lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);

            Camera cam = null;
            cam = playerStates.FPCam.GetComponent<Camera>();

            if (!GameMenuActions.GamePaused)
                lastLookupPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3);
            
            anim.SetLookAtPosition(cam.ScreenToWorldPoint(lastLookupPosition));


            MovePelvisHeight();

            //right foot IK position and rotation -- utilise the pro features in here
            rightFootWeight = anim.GetFloat("RightFoot");

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

            if (useProIKFeature)
            {
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
            }

            MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPos, rightFootIKRot, ref lastRFootPosY);

            //left foot IK position and rotation -- utilise the pro features in here
            leftFootWeight = anim.GetFloat("LeftFoot");

            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

            if (useProIKFeature)
            {
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
            }

            MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPos, leftFootIKRot, ref lastLFootPosY);
        }

        #region Feet Grounder Methods

        private void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
        {
            Vector3 targetIKPosition = anim.GetIKPosition(foot);

            if (positionIKHolder != Vector3.zero)
            {
                targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
                positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

                float yVar = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPosSpeed);
                targetIKPosition.y += yVar;

                lastFootPositionY = yVar;

                targetIKPosition = transform.TransformPoint(targetIKPosition);

                anim.SetIKRotation(foot, rotationIKHolder);
            }

            anim.SetIKPosition(foot, targetIKPosition);
        }

        private void MovePelvisHeight()
        {
            if (rightFootIKPos == Vector3.zero || leftFootIKPos == Vector3.zero || lastPelvisPosY == 0)
            {
                lastPelvisPosY = anim.bodyPosition.y;
                return;
            }

            float lOffsetPos = leftFootIKPos.y - transform.position.y;
            float rOffsetPos = rightFootIKPos.y - transform.position.y;

            float totalOffset = (lOffsetPos < rOffsetPos) ? lOffsetPos : rOffsetPos;

            Vector3 newPelvisPos = anim.bodyPosition + Vector3.up * totalOffset;

            newPelvisPos.y = Mathf.Lerp(lastPelvisPosY, newPelvisPos.y, pelvisUpDownSpeed);

            anim.bodyPosition = newPelvisPos;

            lastPelvisPosY = anim.bodyPosition.y;
        }

        private void FeetPosSolver(Vector3 fromSkyPos, ref Vector3 feetIKPositions, ref Quaternion feetIKRotations)
        {
            // raycast handling section
            RaycastHit feetOutHit;
            if (showSolverDebug)
            {
                Debug.DrawLine(fromSkyPos, fromSkyPos + Vector3.down * (raycastDownDistance + heightFromGroundRaycast), Color.yellow);
            }
            if (Physics.Raycast(fromSkyPos, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRaycast, environmentLayer))
            {
                //finding our feet IK positions from the sky position
                feetIKPositions = fromSkyPos;
                feetIKPositions.y = feetOutHit.point.y + pelvisOffset;
                feetIKRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

                return;
            }

            feetIKPositions = Vector3.zero; // it didn't work :(
        }

        private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot)
        {
            feetPositions = anim.GetBoneTransform(foot).position;
            feetPositions.y = transform.position.y + heightFromGroundRaycast;
        }

        #endregion
    }
}
