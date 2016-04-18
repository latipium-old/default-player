// PlayerLoader.cs
//
// Copyright (c) 2016 Zach Deibert.
// All Rights Reserved.
using System;
using Com.Latipium.Core;

namespace Com.Latipium.Defaults.Player {
	/// <summary>
	/// Loads the player objects into the world.
	/// </summary>
	public class PlayerLoader : AbstractLatipiumLoader {
		internal static PlayerObject Instance;

		public override void Load() {
			if ( PlayerModule.Loaded ) {
				LatipiumModule worldObjects = ModuleFactory.FindModule("Com.Latipium.Modules.World.Objects");
				Action<LatipiumObject> load = worldObjects.GetProcedure<LatipiumObject>("LoadObject");
				load(Instance = new PlayerObject());
			}
		}
	}
}

