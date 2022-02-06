using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Java.Util.Concurrent;
using System.Collections.Concurrent;

namespace AR.XFSample.Droid.Helpers
{
    public class TapHelper : Java.Lang.Object, View.IOnTouchListener
    {
        private GestureDetector gestureDetector;
        private ConcurrentQueue<MotionEvent> queuedSingleTaps = new ConcurrentQueue<MotionEvent>();

        public TapHelper(Context context)
        {
            gestureDetector =
                new GestureDetector(
                    context,
                    new MySimpleOnGestureListener(queuedSingleTaps));
        }

        public bool OnTouch(View view, MotionEvent motionEvent)
        {
            return gestureDetector.OnTouchEvent(motionEvent);
        }
        public MotionEvent Poll()
        {
            MotionEvent tap = null;
            queuedSingleTaps.TryDequeue(out tap);
            return tap;
        }

    }

    public class MySimpleOnGestureListener : GestureDetector.SimpleOnGestureListener
    {
        ConcurrentQueue<MotionEvent> queuedSingleTaps;
        public MySimpleOnGestureListener(ConcurrentQueue<MotionEvent> concurrentQueue)
        {
            queuedSingleTaps = concurrentQueue;
        }
        public override bool OnSingleTapUp(MotionEvent e)
        {
            // Queue tap if there is space. Tap is lost if queue is full.
            queuedSingleTaps.Enqueue(e);
            return true;
        }

        public override bool OnDown(MotionEvent e)
        {
            return true;
        }
    }
}