//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.CoordinateMappingBasics
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    

    /// <summary>
    /// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
    /// and updates the associated GestureResultView object with the latest results for the 'Seated' gesture
    /// </summary>
    public class GestureDetector : IDisposable
    {
        /// <summary> Path to the gesture database that was trained with VGB </summary>
        private readonly string gestureDatabase_practice1 = @"D:\Visual Studio 2019 projects\CoordinateMappingBasics-WPF_final\Database\practice1.gbd";

        /// <summary> Name of the discrete gesture in the database that we want to track </summary>
        private readonly string paperGestureName = "paper";
        private readonly string sayhiGestureName = "sayhi";

        /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
        private VisualGestureBuilderFrameSource vgbFrameSource = null;

        /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
        private VisualGestureBuilderFrameReader vgbFrameReader = null;


        #region new statement

        private MainWindow mainWindow;
        private bool[] backgroundBuffer = new bool[30]; // buffer for not change background too frequently
        private int backgroundBufferIndex = 0;
        private int frame_num = 0; // count the number of frames

        #endregion new statement


        /// <summary>
        /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
        /// </summary>
        /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
        /// <param name="gestureResultView">GestureResultView object to store gesture results of a single body to</param>


        /*public GestureDetector(KinectSensor kinectSensor, GestureResultView gestureResultView)
        {
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            if (gestureResultView == null)
            {
                throw new ArgumentNullException("gestureResultView");
            }
            
            this.GestureResultView = gestureResultView;
            this.ClosedHandState = false;

            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            //this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
               // this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // load the 'Seated' gesture from the gesture database
            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(this.gestureDatabase_hi))
            {
                // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
                // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
                foreach (Gesture gesture in database.AvailableGestures)
                {
                    if (gesture.Name.Equals(this.hiGestureName))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                }
            }
        }

        */

        public GestureDetector(KinectSensor kinectSensor, MainWindow mainWindow)
        {
            if(kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSeneor");
            }

            this.mainWindow = mainWindow;

            // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);

            // open the reader for the vgb frames
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();

            if(this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }

            // setting initial backgroundBuffer = false
            for(int i = 0; i < backgroundBuffer.Length; i++)
            {
                backgroundBuffer[i] = false;
            }

            using (VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(this.gestureDatabase_practice1))
            {
                foreach(Gesture gesture in database.AvailableGestures)
                {
                    // first gesture
                    if (gesture.Name.Equals(this.paperGestureName))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                    else if (gesture.Name.Equals(this.sayhiGestureName))
                    {
                        this.vgbFrameSource.AddGesture(gesture);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the body tracking ID associated with the current detector
        /// The tracking ID can change whenever a body comes in/out of scope
        /// </summary>
        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                {
                    this.vgbFrameSource.TrackingId = value;
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether or not the detector is currently paused
        /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                {
                    this.vgbFrameReader.IsPaused = value;
                }
            }
        }

        /// <summary>
        /// Disposes all unmanaged resources for the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        /// </summary>
        /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.vgbFrameReader != null)
                {
                    this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                    this.vgbFrameReader.Dispose();
                    this.vgbFrameReader = null;
                }

                if (this.vgbFrameSource != null)
                {
                    this.vgbFrameSource.Dispose();
                    this.vgbFrameSource = null;
                }
            }
        }

        /// <summary>
        /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    // get the discrete gesture results which arrived with the latest frame
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    // get the continue gesture results
                    IReadOnlyDictionary<Gesture, ContinuousGestureResult> continuousResults = frame.ContinuousGestureResults;

                    if (discreteResults != null)
                    {
                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            if (gesture.Name.Equals(this.paperGestureName) && gesture.GestureType == GestureType.Discrete)
                            {
                                frame_num++;
                                if (frame_num % 2 == 0) // analysis the input data every two frames
                                {
                                    if (frame_num == 30)
                                    {
                                        frame_num = 0;
                                    }

                                    break;
                                }

                                DiscreteGestureResult dResult = null;
                                discreteResults.TryGetValue(gesture, out dResult);

                                if (dResult != null)
                                {
                                    // update the GestureResultView object with new gesture result values
                                    //this.GestureResultView.UpdateGestureResult(true, dResult.Detected, dResult.Confidence);

                                    // if high confidence then record 'true'
                                    if (dResult.Confidence > 0.8)
                                    {
                                        backgroundBuffer[backgroundBufferIndex] = true;
                                    }
                                    else
                                    {
                                        backgroundBuffer[backgroundBufferIndex] = false;
                                    }

                                    backgroundBufferIndex++;
                                    backgroundBufferIndex = backgroundBufferIndex % backgroundBuffer.Length;

                                    int doEvent = 0;
                                    foreach(bool b in backgroundBuffer)
                                    {
                                        if (b)
                                        {
                                            doEvent++;
                                        }
                                    }

                                    if (doEvent == backgroundBuffer.Length / 2)
                                    {
                                        // change background
                                        int index = (mainWindow.bgSelectionComboBox.SelectedIndex + 1) % mainWindow.backgroundUrl.Length;
                                        mainWindow.BackgroundSelectedIndex = index;

                                        // clear buffer to avoid flashing
                                        for(int i = 0; i < backgroundBuffer.Length; i++)
                                        {
                                            backgroundBuffer[i] = false;
                                        }
                                    }
                                }
                            }
                            else if(gesture.Name.Equals(this.sayhiGestureName) && gesture.GestureType == GestureType.Continuous)
                            {
                                ContinuousGestureResult cResult;
                                continuousResults.TryGetValue(gesture, out cResult);
                                if (cResult != null)
                                {
                                    if (cResult.Progress > 0.8)
                                    {
                                        backgroundBuffer[backgroundBufferIndex] = true;
                                    }
                                    else
                                    {
                                        backgroundBuffer[backgroundBufferIndex] = false;
                                    }
                                    backgroundBufferIndex++;
                                    backgroundBufferIndex = backgroundBufferIndex % backgroundBuffer.Length;

                                    int doEvent = 0;
                                    foreach (bool b in backgroundBuffer)
                                    {
                                        if (b)
                                        {
                                            doEvent++;
                                        }
                                    }
                                    if (doEvent == backgroundBuffer.Length / 2)
                                    {
                                        int index = (mainWindow.bgSelectionComboBox.SelectedIndex + 1) % mainWindow.backgroundUrl.Length;

                                        mainWindow.BackgroundSelectedIndex = index;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
