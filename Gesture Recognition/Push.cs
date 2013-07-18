using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    public class PushGesture : GestureBase
    {

        public PushGesture()
            : base(GestureType.Push) { }

        private SkeletonPoint validateHandRightPosition;
        private SkeletonPoint validateHandLeftPosition;
        private SkeletonPoint startingHandRightPosition;
        private SkeletonPoint startingHandLeftPosition;
        private SkeletonPoint currentHandRightPosition;
        private SkeletonPoint currentHandLeftPosition;

        protected override bool ValidateGestureStartCondition(Skeleton skeleton)
        {
            var handRightPosition = skeleton.Joints[JointType.HandRight].Position;
            var handLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            var shoulderRightPosition = skeleton.Joints[JointType.ShoulderRight].Position;
            var shoulderLeftPosition = skeleton.Joints[JointType.ShoulderLeft].Position;
            var headPosition = skeleton.Joints[JointType.Head].Position;
            double Epsilon = 0.15;

            if (((shoulderRightPosition.Y - handRightPosition.Y) <= Epsilon) &&
                ((shoulderLeftPosition.Y - handLeftPosition.Y) <= Epsilon))
            {
                //Console.WriteLine("inside validate");
                validateHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
                startingHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
                validateHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
                startingHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
                return true;
            }
            return false;
        }

        protected override bool IsGestureValid(Skeleton skeleton)
        {
             currentHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
             currentHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;

            if (((validateHandRightPosition.Z < currentHandRightPosition.Z) ) ||
                ((validateHandLeftPosition.Z < currentHandLeftPosition.Z)))
            {
                return false;
            }
            validateHandRightPosition = currentHandRightPosition;
            validateHandLeftPosition = currentHandLeftPosition;
            return true;
        }

        protected override bool ValidateGestureEndCondition(Skeleton skeleton)
        {
            currentHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
            currentHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;
            double zDifference = 0.25;
            if (((startingHandRightPosition.Z - currentHandRightPosition.Z) >= zDifference) &&
                ((startingHandLeftPosition.Z - currentHandLeftPosition.Z ) >= zDifference))
                return true;

            return false;
        }

        protected override bool ValidateBaseCondition(Skeleton skeleton)
        {
             currentHandRightPosition = skeleton.Joints[JointType.HandRight].Position;
             currentHandLeftPosition = skeleton.Joints[JointType.HandLeft].Position;

            if ((startingHandRightPosition.Z > currentHandRightPosition.Z) ||
                ((startingHandLeftPosition.Z > currentHandLeftPosition.Z)))
            {
                return true;
            }
             
            return false;
        }
    }
}
