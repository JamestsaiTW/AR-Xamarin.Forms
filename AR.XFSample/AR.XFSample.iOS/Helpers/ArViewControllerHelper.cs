using ARKit;
using SceneKit;
using System;
using UIKit;

namespace AR.XFSample.iOS.Helpers
{
    public class ArViewControllerHelper : UIViewController
    {
        private readonly string arLaunchType;

        public ArViewControllerHelper(string arLaunchType)
        {
            this.arLaunchType = arLaunchType;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            StartAR(arLaunchType);
        }

        public void StartAR(string arLaunchType)
        {
            var sceneView = new ARSCNView
            {
                Frame = View.Frame
            };

            View = sceneView;
            CreateARScene(sceneView);
            PositionScene(sceneView, arLaunchType);
        }

        public void CreateARScene(ARSCNView sceneView)
        {
            var scene = SCNScene.FromFile("art.scnassets/ship");
            sceneView.Scene = scene;
            sceneView.DebugOptions = ARSCNDebugOptions.ShowWorldOrigin | ARSCNDebugOptions.ShowFeaturePoints;
        }

        public void PositionScene(ARSCNView sceneView, string arLaunchType)
        {
            var arConfiguration = new ARWorldTrackingConfiguration
            {
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true
            };

            sceneView.Session.Run(arConfiguration, ARSessionRunOptions.ResetTracking);
           
            var sceneShipNode = sceneView.Scene.RootNode.FindChildNode("ship", true);
            sceneShipNode.Position = new SCNVector3(2f, -2f, -9f);

            var animationCycle = SCNAction.RepeatActionForever(SCNAction.RotateBy(0f, 6f, 0, 5));
            var animationCrash = SCNAction.RepeatActionForever(SCNAction.RotateBy(0, (float)Math.PI, (float)Math.PI, (float)1));
            var animationNormal = SCNAction.RepeatActionForever(SCNAction.RotateBy(0, 0, 0, 1));
            var animationRotate = SCNAction.RepeatActionForever(SCNAction.RotateBy(0, 0, 2, 1));


            var scenePivotNode = new SCNNode { Position = new SCNVector3(0.0f, 2.0f, 0.0f) };
            scenePivotNode.RunAction(SCNAction.RepeatActionForever(SCNAction.RotateBy(0, -2, 0, 10)));

            sceneShipNode.RemoveFromParentNode();
            scenePivotNode.AddChildNode(sceneShipNode);
            
            sceneView.Scene.RootNode.AddChildNode(scenePivotNode);
            
            sceneShipNode.Scale = new SCNVector3(0.1f, 0.1f, 0.1f);
            sceneShipNode.Position = new SCNVector3(2f, -2f, -3f);


            switch (arLaunchType)
            {
                
                case "Rotate Fly":
                    sceneShipNode.RunAction(animationRotate);
                    break;
                case "Crash Fly":
                    sceneShipNode.RunAction(animationCrash);
                    break;
                case "Cycle Fly":
                    sceneShipNode.RunAction(animationCycle);
                    break;
                case "Normal Fly":
                    sceneShipNode.RunAction(animationNormal);
                    break;
                default:
                    scenePivotNode.RemoveAllActions();
                    scenePivotNode.RunAction(SCNAction.Unhide());
                    break;
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            // Pause the view's session
            (View as ARSCNView).Session.Pause();
        }
    }
}