using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    public class LeftSwordSlashGesture : GestureBase
    {

        public LeftSwordSlashGesture()
            : base(GestureType.LeftSwordSlash) { }

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
            var shoulderLeftPosition = skeleton.Joints[JointType.ShoulderLeft].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            var headPosition = skeleton.Joints[JointType.Head].Position;

           /* if ((handLeftPosition.Y > shoulderLeftPosition.Y) &&
              (handLeftPosition.Y > skeleton.Joints[JointType.ElbowLeft].Position.Y) &&
              (handRightPosition.Y < spinePosition.Y) &&
              (headPosition.X > handLeftPosition.X))*/
            if (handLeftPosition.Y > shoulderLeftPosition.Y)
            {
                //Console.WriteLine("inside validate");
                validatePosition = skeleton.Joints[JointType.HandLeft].Position;
                startingPosition = skeleton.Joints[JointType.HandLeft].Position;
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
            var currentHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            if (validatePosition.Y < currentHandLeftPosition.Y)
            {
                return false;
            }
            validatePosition = currentHandLeftPosition;
            return true;
        }

        /// <summary>
        /// Validates the gesture end condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            var currentLeftHandPosition = skeleton.Joints[JointType.HandLeft].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            if (currentLeftHandPosition.Y < spinePosition.Y)
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

            if (startingPosition.Y > handLeftPosition.Y)
            {
                return true;

            }
            return false;
        }
    }


}
