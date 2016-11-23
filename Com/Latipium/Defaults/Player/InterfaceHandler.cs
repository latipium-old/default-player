// InterfaceHandler.cs
//
// Copyright (c) 2016 Zach Deibert.
// All Rights Reserved.
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Com.Latipium.Core;

namespace Com.Latipium.Defaults.Player {
	internal class InterfaceHandler {
		private LatipiumObject _Object;
        internal Com.Latipium.Core.Tuple<float, float, float> Translation;
        internal Com.Latipium.Core.Tuple<float, float> Rotation;

		internal LatipiumObject Object {
			get {
				return _Object;
			}

			set {
				_Object = value;
			}
		}

		private void HandleKeyboard(int key, int mod) {
			int d = 0;
			int type = ((mod >> 8) & 7);
			switch ( type ) {
				case 1:
					d = 1;
					break;
				case 2:
					d = 0;
					break;
			}
			switch ( key ) {
				case 105:
					Translation.Object3 = d;
					break;
				case 83:
					Translation.Object1 = d;
					break;
				case 101:
					Translation.Object3 = -d;
					break;
				case 86:
					Translation.Object1 = -d;
					break;
			}
			float mag = (float) Math.Sqrt(Translation.Object1 * Translation.Object1 + Translation.Object2 * Translation.Object2 + Translation.Object3 * Translation.Object3);
			if ( mag != 0 ) {
				Translation.Object1 /= mag;
				Translation.Object2 /= mag;
				Translation.Object3 /= mag;
			}
		}

		private void CenterScreen(out int x, out int y) {
			Rectangle screen = Screen.PrimaryScreen.Bounds;
			x = screen.Left + screen.Width / 2;
			y = screen.Top + screen.Height / 2;
		}

		private void HandleMouse(int x, int y, int dx, int dy) {
			int mx;
			int my;
			CenterScreen(out mx, out my);
			// dx and dy don't work when we reset the cursor position :(
			Point pt = Cursor.Position;
			Rotation.Object1 += pt.X - mx;
			Rotation.Object2 += pt.Y - my;
			Cursor.Position = new Point(mx, my);
		}

		internal InterfaceHandler() {
            Translation = new Com.Latipium.Core.Tuple<float, float, float>();
            Rotation = new Com.Latipium.Core.Tuple<float, float>();
			LatipiumModule graphics = ModuleFactory.FindModule("Com.Latipium.Modules.Graphics");
			if ( graphics != null ) {
				graphics.AddEvent("MouseMoved", (Action<int, int, int, int>) HandleMouse);
				Action<Action<int, int>, long> AddKeyboardBinding = graphics.GetProcedure<Action<int, int>, long>("AddKeyboardBinding");
				if ( AddKeyboardBinding != null ) {
					AddKeyboardBinding(HandleKeyboard, 0);
				}
				graphics.InvokeProcedure("HideCursor");
			}
			int x;
			int y;
			CenterScreen(out x, out y);
			Cursor.Position = new Point(x, y);
		}
	}
}

