
using Microsoft.Kinect;
using System;
namespace SpaceAdventure
{
    public abstract class GestureBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGesture" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public GestureBase(GestureType type)
        {
            this.CurrentFrameCount = 0;
            this.GestureType = type;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recognized.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is recognized; otherwise, <c>false</c>.
        /// </value>
        public bool IsRecognitionStarted { get; set; }

        /// <summary>
        /// Gets or sets the current frame count.
        /// </summary>
        /// <value>
        /// The current frame count.
        /// </value>
        private int CurrentFrameCount { get; set; }

        /// <summary>
        /// Gets or sets the type of the gesture.
        /// </summary>
        /// <value>
        /// The type of the gesture.
        /// </value>
        public GestureType GestureType { get; set; }

        /// <summary>
        /// Gets the maximum number of frame to process.
        /// </summary>
        /// <value>
        /// The maximum number of frame to process.
        /// </value>
        protected virtual int MaximumNumberOfFrameToProcess { get { return 15; } }

        /// <summary>
        /// Validates the gesture start condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected abstract bool ValidateGestureStartCondition(Skeleton skeleton);


        /// <summary>
        /// Validates the gesture end condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected abstract bool ValidateGestureEndCondition(Skeleton skeleton);


        /// <summary>
        /// Validates the base condition.
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns></returns>
        protected abstract bool ValidateBaseCondition(Skeleton skeleton);


        /// <summary>
        /// Determines whether [is gesture valid] [the specified skeleton].
        /// </summary>
        /// <param name="skeleton">The skeleton.</param>
        /// <returns>
        ///   <c>true</c> if [is gesture valid] [the specified skeleton]; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool IsGestureValid(Skeleton skeleton);

        public virtual bool CheckForGesture(Skeleton skeleton)
        {
            if (this.IsRecognitionStarted == false)
            {
                if (this.ValidateGestureStartCondition(skeleton))
                {
                    this.IsRecognitionStarted = true;
                    this.CurrentFrameCount = 0;
                }
            }
            else
            {
                if (this.CurrentFrameCount == this.MaximumNumberOfFrameToProcess)
                {
                    this.IsRecognitionStarted = false;
                    if (ValidateBaseCondition(skeleton) /**&& ValidateGestureEndCondition(skeleton)**/)
                    {
                        return true;
                    }
                }

                this.CurrentFrameCount++;
                if (!IsGestureValid(skeleton) && !ValidateBaseCondition(skeleton))
                {
                    this.IsRecognitionStarted = false;
                }
            }
            return false;
        }

    }
}
