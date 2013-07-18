
namespace SpaceAdventure
{
    using System;
    public class GestureEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public RecognitionResult Result { get; internal set; }

        /// <summary>
        /// Gets the type of the gesture.
        /// </summary>
        /// <value>
        /// The type of the gesture.
        /// </value>
        public GestureType GestureType { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureEventArgs" /> class.
        /// </summary>
        /// <param name="result">The result.</param>
        public GestureEventArgs(RecognitionResult result, GestureType type)
        {
            this.Result = result;
            this.GestureType = type;
        }
    }
}
