using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    public class RightGuardGesture : GestureBase
    {

        public RightGuardGesture()
            : base(GestureType.RightGuard) { }

        private SkeletonPoint startingHandRightPosition;

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var leftHandPosition = skeleton.Joints[JointType.HandLeft].Position;
            var elbowRightPosition = skeleton.Joints[JointType.ElbowRight].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var headPosition = skeleton.Joints[JointType.Head].Position;
            var spinePosition = skeleton.Joints[JointType.Spine].Position;
            double Epsilon = 0.1;
            if ((Math.Abs(shoulderRightPosition.Y - elbowRightPosition.Y) <= Epsilon) &&
                (headPosition.X > handRightPosition.X) &&
                ((leftHandPosition.Y < elbowRightPosition.Y)))
            {
                startingHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
                return true;
            }
            return false;
        }

        protected override bool IsGestureValid(Skeleton skeleton)
        {
            var currentHandRightPoisition = skeleton.Joints[JointType.HandRight].Position;
            if (startingHandRightPosition.Y > currentHandRightPoisition.Y)
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
