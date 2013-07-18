using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    public class LeftGuardGesture : GestureBase
    {

        public LeftGuardGesture()
            : base(GestureType.LeftGuard) { }

        private SkeletonPoint startingHandLeftPosition;

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var rightHandPosition = skeleton.Joints[JointType.HandRight].Position;
            var elbowLeftPosition = skeleton.Joints[JointType.ElbowLeft].Position;
            var shoulderLeftPosition = skeleton.Joints[JointType.ShoulderLeft].Position;
            var headPosition = skeleton.Joints[JointType.Head].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            double Epsilon = 0.1;
            if ((Math.Abs(shoulderLeftPosition.Y - elbowLeftPosition.Y) <= Epsilon) &&
                (headPosition.X < handLeftPosition.X) &&
                ((rightHandPosition.Y < elbowLeftPosition.Y)))
            {
                startingHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
                return true;
            }
            return false;
        }

        protected override bool IsGestureValid(Skeleton skeleton)
        {
            var currentHandLeftPoisition = skeleton.Joints[JointType.HandLeft].Position;
            if (startingHandLeftPosition.Y > currentHandLeftPoisition.Y)
                return true;

            return false; 
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            return true;
        }

        protected override bool ValidateBaseCondition(Skeleton skeleton)
        {
            return true;
        }
    }
}
