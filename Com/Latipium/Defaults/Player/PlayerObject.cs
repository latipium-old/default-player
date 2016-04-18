// PlayerImpl.cs
//
// Copyright (c) 2016 Zach Deibert.
// All Rights Reserved.
using System;
using System.Collections.Generic;
using System.Linq;
using Com.Latipium.Core;

namespace Com.Latipium.Defaults.Player {
	/// <summary>
	/// The object for a player.
	/// </summary>
	public class PlayerObject : AbstractLatipiumObject {
		private LatipiumObject _Object;
		private Func<Tuple<float, float, float>, Tuple<float, float, float>> Position;
		private Action<float, float, float, float> Rotate;
		private int LastFrameTime;
		private float XRot;
		internal InterfaceHandler Handler;
		internal float Speed = 4;
		internal float Sensitivity = (float) Math.PI / 8192f;

		internal LatipiumObject Object {
			get {
				return _Object;
			}

			set {
				_Object = value;
				if ( Handler == null ) {
					Handler = new InterfaceHandler();
				}
				Handler.Object = _Object;
				Position = _Object.GetFunction<Tuple<float, float, float>, Tuple<float, float, float>>("Position");
				if ( Position != null ) {
					Position(new Tuple<float, float, float>(0, -5, -5));
				}
				Rotate = _Object.GetProcedure<float, float, float, float>("Rotate");
				if ( Rotate != null ) {
					Rotate((float) -Math.PI / 4f, 1, 0, 0);
				}
			}
		}

		/// <summary>
		/// Updates the specified objects.
		/// </summary>
		/// <param name="objs">The objects to update.</param>
		[LatipiumMethod("PhysicsUpdate")]
		public Tuple<IEnumerable<LatipiumObject>, IEnumerable<LatipiumObject>> Update(IEnumerable<LatipiumObject> objs) {
			if ( Handler != null ) {
				LatipiumObject obj = objs.First(o => o == Object && Handler.Object == o);
				if ( obj != null && Position != null && Environment.TickCount > LastFrameTime ) {
					float delta = (((float) Environment.TickCount) - (float) LastFrameTime) / 1000f;
					LastFrameTime = Environment.TickCount;
					if ( Rotate != null ) {
						float x = Handler.Rotation.Object1;
						float y = Handler.Rotation.Object2;
						if ( x != 0 ) {
							float xs = x * Sensitivity;
							XRot -= xs;
							Rotate(xs, 0, -1, 0);
							Handler.Rotation.Object1 -= x;
						}
						if ( y != 0 ) {
							Rotate(-y * Sensitivity, (float) Math.Cos(XRot), 0, (float) Math.Sin(-XRot));
							Handler.Rotation.Object2 -= y;
						}
					}
					Tuple<float, float, float> pos = Position(null);
					delta *= Speed;
					float sin = (float) Math.Sin(XRot);
					float nsin = (float) Math.Sin(-XRot);
					float cos = (float) Math.Cos(XRot);
					pos.Object1 += Handler.Translation.Object3 * delta * sin + Handler.Translation.Object1 * delta * cos;
					pos.Object2 += Handler.Translation.Object2 * delta;
					pos.Object3 += Handler.Translation.Object3 * delta * cos + Handler.Translation.Object1 * delta * nsin;
					Position(pos);
				}
			}
			return null;
		}
	}
}

