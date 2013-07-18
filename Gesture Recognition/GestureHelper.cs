using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace SpaceAdventure
{
    /// <summary>
    /// Gesture Helper
    /// </summary>
    public class GestureHelper
    {
        GestureHelper()
        {
        }

        /// <summary>
        /// Gets the joint distance.
        /// </summary>
        /// <param name="firstJoint">The first joint.</param>
        /// <param name="secondJoint">The second joint.</param>
        /// <returns>retunr the distance</returns>
        public static float GetJointDistance(Joint firstJoint, Joint secondJoint)
        {
            float distanceX = firstJoint.Position.X - secondJoint.Position.X;
            float distanceY = firstJoint.Position.Y - secondJoint.Position.Y;
            float distanceZ = firstJoint.Position.Z - secondJoint.Position.Z;
            return (float)Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2) + Math.Pow(distanceZ, 2));
        }
    }
}
