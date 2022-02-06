using System;
using Android.Content;
using Android.Hardware.Camera2;
using Android.Hardware.Display;
using Android.Views;
using Java.Interop;
using Java.Lang;

namespace AR.XFSample.Droid.Helpers
{
    public class DisplayRotationHelper : Java.Lang.Object, DisplayManager.IDisplayListener
    {
        bool mViewportChanged;
        int mViewportWidth;
        int mViewportHeight;
        
        //Context mContext;
        
        private Display mDisplay;
        private DisplayManager mDisplayManager;
        private CameraManager mCameraManager;

        /**
         * Constructs the DisplayRotationHelper but does not register the listener yet.
         *
         * @param context the Android {@link Context}.
         */
        public DisplayRotationHelper(Context context)
        {
            ///mContext = context;
            ///mDisplay = context.GetSystemService(Java.Lang.Class.FromType(typeof(IWindowManager)))
            ///                  .JavaCast<IWindowManager>().DefaultDisplay;

            mDisplayManager = (DisplayManager)context.GetSystemService(Context.DisplayService);
            mCameraManager = (CameraManager)context.GetSystemService(Context.CameraService);
            ///WindowManager windowManager = (IWindowManager)context.GetSystemService(Context.WindowService);
            mDisplay = context.GetSystemService(Java.Lang.Class.FromType(typeof(IWindowManager)))
                              .JavaCast<IWindowManager>().DefaultDisplay;
        }

        /** Registers the display listener. Should be called from {@link Activity#onResume()}. */
        public void OnResume()
        {
            mDisplayManager.RegisterDisplayListener(this, null);
            ///mContext.GetSystemService(Java.Lang.Class.FromType(typeof(DisplayManager)))
            ///        .JavaCast<DisplayManager>().RegisterDisplayListener(this, null);
        }

        /** Unregisters the display listener. Should be called from {@link Activity#onPause()}. */
        public void OnPause()
        {
            mDisplayManager.UnregisterDisplayListener(this);
            ///mContext.GetSystemService(Java.Lang.Class.FromType(typeof(DisplayManager)))
            ///    .JavaCast<DisplayManager>().UnregisterDisplayListener(this);
        }

        /**
         * Records a change in surface dimensions. This will be later used by
         * {@link #updateSessionIfNeeded(Session)}. Should be called from
         * {@link android.opengl.GLSurfaceView.Renderer
         *  #onSurfaceChanged(javax.microedition.khronos.opengles.GL10, int, int)}.
         *
         * @param width the updated width of the surface.
         * @param height the updated height of the surface.
         */
        public void OnSurfaceChanged(int width, int height)
        {
            mViewportWidth = width;
            mViewportHeight = height;
            mViewportChanged = true;
        }

        /**
         * Updates the session display geometry if a change was posted either by
         * {@link #onSurfaceChanged(int, int)} call or by {@link #onDisplayChanged(int)} system
         * callback. This function should be called explicitly before each call to
         * {@link Session#update()}. This function will also clear the 'pending update'
         * (viewportChanged) flag.
         *
         * @param session the {@link Session} object to update if display geometry changed.
         */
        public void UpdateSessionIfNeeded(Google.AR.Core.Session session)
        {
            if (mViewportChanged)
            {
                int displayRotation = (int)mDisplay.Rotation;
                session.SetDisplayGeometry(displayRotation, mViewportWidth, mViewportHeight);
                mViewportChanged = false;
            }
        }

        /**
         *  Returns the aspect ratio of the GL surface viewport while accounting for the display rotation
         *  relative to the device camera sensor orientation.
         */
        public float GetCameraSensorRelativeViewportAspectRatio(string cameraId)
        {
            float aspectRatio;
            int cameraSensorToDisplayRotation = GetCameraSensorToDisplayRotation(cameraId);
            switch (cameraSensorToDisplayRotation)
            {
                case 90:
                case 270:
                    aspectRatio = (float)mViewportHeight / (float)mViewportWidth;
                    break;
                case 0:
                case 180:
                    aspectRatio = (float)mViewportWidth / (float)mViewportHeight;
                    break;
                default:
                    throw new RuntimeException("Unhandled rotation: " + cameraSensorToDisplayRotation);
            }
            return aspectRatio;
        }


        /**
         * Returns the current rotation state of android display.
         * Same as {@link Display#getRotation()}.
         */
        ///public int GetRotation()
        ///{
        ///    return (int)mDisplay.Rotation;
        ///}

        /**
         * Returns the rotation of the back-facing camera with respect to the display. The value is one of
         * 0, 90, 180, 270.
         */
        public int GetCameraSensorToDisplayRotation(string cameraId)
        {
            CameraCharacteristics characteristics;
            try
            {
                characteristics = mCameraManager.GetCameraCharacteristics(cameraId);
            }
            catch (CameraAccessException e)
            {
                throw new RuntimeException("Unable to determine display orientation", e);
            }

            // Camera sensor orientation.
            int sensorOrientation = (int)characteristics.Get(CameraCharacteristics.SensorOrientation);

            // Current display orientation.
            int displayOrientation = ToDegrees(mDisplay.Rotation);

            // Make sure we return 0, 90, 180, or 270 degrees.
            return (sensorOrientation - displayOrientation + 360) % 360;
        }

        private int ToDegrees(SurfaceOrientation rotation)
        {
            return rotation switch
            {
                SurfaceOrientation.Rotation0 => 0,
                SurfaceOrientation.Rotation90 => 90,
                SurfaceOrientation.Rotation180 => 180,
                SurfaceOrientation.Rotation270 => 270,
                _ => throw new RuntimeException("Unknown rotation " + rotation),
            };
        }

        
        public void OnDisplayAdded(int displayId) { }

        public void OnDisplayRemoved(int displayId) { }

        public void OnDisplayChanged(int displayId)
        {
            mViewportChanged = true;
        }
    }
}
