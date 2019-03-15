using ARKit;
using SceneKit;
using UIKit;

namespace AR.XFSample.iOS.Helpers
{
    public class ArViewControllerHelper : UIViewController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            StartAR();
        }

        public void StartAR()
        {
            var sceneView = new ARSCNView
            {
                Frame = View.Frame
            };

            View = sceneView;
            CreateARScene(sceneView);
            PositionScene(sceneView);
        }

        public void CreateARScene(ARSCNView sceneView)
        {
            var scene = SCNScene.FromFile("art.scnassets/ship");
            sceneView.Scene = scene;
            sceneView.DebugOptions = ARSCNDebugOptions.ShowWorldOrigin | ARSCNDebugOptions.ShowFeaturePoints;
        }

        public void PositionScene(ARSCNView sceneView)
        {
            var arConfiguration = new ARWorldTrackingConfiguration
            {
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true
            };

            sceneView.Session.Run(arConfiguration, ARSessionRunOptions.ResetTracking);
            var sceneNode = sceneView.Scene.RootNode.FindChildNode("ship", true);
            sceneNode.Position = new SCNVector3(0.0f, 0.0f, -30f);
            sceneView.Scene.RootNode.AddChildNode(sceneNode);

            sceneNode.RunAction(SCNAction.RepeatActionForever(SCNAction.RotateBy(0f, 6f, 0, 5)));
        }
    }
}