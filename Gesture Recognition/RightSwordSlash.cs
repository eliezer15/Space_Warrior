using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    public class RightSwordSlashGesture : GestureBase
    {

        public RightSwordSlashGesture()
            : base(GestureType.RightSwordSlash) { }

        private SkeletonPoint validatePosition;
        private SkeletonPoint startingPosition;

        /// <summary>
        /// Validates the gesture start condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            var headPosition = skeleton.Joints[JointType.Head].Position;

            if ((handRightPosition.Y > shoulderRightPosition.Y))
            {
                //Console.WriteLine("inside validate");
                validatePosition = skeleton.Joints[JointType.HandRight].Position;
                startingPosition = skeleton.Joints[JointType.HandRight].Position;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is gesture valid] [the specified skeleton data].
        /// </summary>
        /// <param name="skeleton">The skeleton data.</param>
        /// <returns>
        ///   <c>true</c> if [is gesture valid] [the specified skeleton data]; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsGestureValid(Skeleton skeleton)
        {
            var currentHandRightPoisition = skeleton.Joints[JointType.HandRight].Position;
            if (validatePosition.Y < currentHandRightPoisition.Y)
            {
                return false;
            }
            validatePosition = currentHandRightPoisition;
            return true;
        }

        /// <summary>
        /// Validates the gesture end condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            var currentRightHandPosition = skeleton.Joints[JointType.HandRight].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            if (currentRightHandPosition.Y < spinePosition.Y)
                return true;

            return false;
        }

        /// <summary>
        /// Valids the base condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected override bool ValidateBaseCondition(Skeleton skeleton)
        {
            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;

            if (startingPosition.Y > handRightPosition.Y)
            {
                return true;

            }
            return false;
        }
    }


}
