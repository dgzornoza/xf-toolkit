using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Urho2D;

namespace XamarinForms.Toolkit.Urho3D.Helpers
{
    /// <summary>
    /// Helpers to extend physicsWorld2D urho object
    /// </summary>
    public static class PhysicsWorld2DHelpers
    {
        /// <summary>
        /// Physics Begin contact Helper funcion for get other obect by discarding from this object.
        /// </summary>
        /// <typeparam name="T">Object type for compare and discarding (valid types in 'PhysicsBeginContact2DEventArgs' => RigidBody2D, CollisionShape2D or Node)</typeparam>
        /// <param name="args">physics begin contact event arguments</param>
        /// <param name="thisObj">this object reference used for discarding</param>
        /// <returns>other object if this object exists, otherwise null</returns>
        public static T GetOther<T>(this PhysicsBeginContact2DEventArgs args, T thisObj) where T : class
        {
            T result = default;

            switch (thisObj)
            {
                case Node node:
                    result = (node == args.NodeA ? args.NodeB : node == args.NodeB ? args.NodeA : null) as T;
                    break;
                case RigidBody2D rigidBody2D:
                    result = (rigidBody2D == args.BodyA ? args.BodyB : rigidBody2D == args.BodyB ? args.BodyA : null) as T;
                    break;
                case CollisionShape2D collisionShape2D:
                    result = (collisionShape2D == args.ShapeA ? args.ShapeB : collisionShape2D == args.ShapeB ? args.ShapeA : null) as T;
                    break;
                default:
                    throw new InvalidOperationException($"PhysicsWorld2DHelpers:GetOther(). thisObj is invalid type: {typeof(T)}, valid types RigidBody2D, CollisionShape2D or Node");                    
            }

            return result;
        }
    }
}
