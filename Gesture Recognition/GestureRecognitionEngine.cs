/* Gesture Recognition Engine */

namespace SpaceAdventure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Kinect;

    /// <summary>
    /// Gesture Recognition Engine
    /// </summary>
    public class GestureRecognitionEngine
    {
        /// <summary>
        /// Skip Frames After Gesture IsDetected
        /// </summary>
        int SkipFramesAfterGestureIsDetected = 0;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is gesture detected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is gesture detected; otherwise, <c>false</c>.
        /// </value>
        public bool IsGestureDetected { get; set; }

        /// <summary>
        /// Collection of Gesture
        /// </summary>
        private List<GestureBase> gestureCollection = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureRecognitionEngine" /> class.
        /// </summary>
        public GestureRecognitionEngine()
        {
            this.InitializeGesture();
        }

        /// <summary>
        /// Initilizes the gesture.
        /// </summary>
        private void InitializeGesture()
        {
            this.gestureCollection = new List<GestureBase>();
            this.gestureCollection.Add(new LeftSwordSlashGesture());
            this.gestureCollection.Add(new RightSwordSlashGesture());
            this.gestureCollection.Add(new PushGesture());
            this.gestureCollection.Add(new RightGuardGesture());
            this.gestureCollection.Add(new LeftGuardGesture());
        }

        /// <summary>
        /// Occurs when [gesture recognized].
        /// </summary>
        public event EventHandler<GestureEventArgs> GestureRecognized;

        /// <summary>
        /// Gets or sets the type of the gesture.
        /// </summary>
        /// <value>
        /// The type of the gesture.
        /// </value>
        public GestureType GestureType { get; set; }

        /// <summary>
        /// Gets or sets the skeleton.
        /// </summary>
        /// <value>
        /// The skeleton.
        /// </value>
        public Skeleton Skeleton { get; set; }

        /// <summary>
        /// Starts the recognize.
        /// </summary>
        public void StartRecognize()
        {
            if (this.IsGestureDetected)
            {
                while (this.SkipFramesAfterGestureIsDetected <= 25)
                {
                    this.SkipFramesAfterGestureIsDetected++;
                }
                this.RestGesture();
                return;
            }

            foreach (var item in this.gestureCollection)
            {
                if (item.CheckForGesture(this.Skeleton))
                {
                    if (this.GestureRecognized != null)
                    {
                        this.GestureRecognized(this, new GestureEventArgs(RecognitionResult.Success, item.GestureType));
                        this.IsGestureDetected = true;
                    }

                }
            }
        }

        /// <summary>
        /// Rests the gesture.
        /// </summary>
        private void RestGesture()
        {
            this.gestureCollection = null;
            this.InitializeGesture();
            this.SkipFramesAfterGestureIsDetected = 0;
            this.IsGestureDetected = false;
        }
    }
}
