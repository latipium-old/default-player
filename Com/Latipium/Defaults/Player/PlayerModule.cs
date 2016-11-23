// PlayerModule.cs
//
// Copyright (c) 2016 Zach Deibert.
// All Rights Reserved.
using System;
using Com.Latipium.Core;

namespace Com.Latipium.Defaults.Player {
	/// <summary>
	/// The default implementation for the player module.
	/// </summary>
	public class PlayerModule : AbstractLatipiumModule, LatipiumLoader {
		internal static bool Loaded;

		/// <summary>
		/// Constructs a new player.
		/// </summary>
		/// <returns>The new player.</returns>
		[LatipiumMethod("GetPlayer")]
		public LatipiumObject GetPlayer() {
			return PlayerLoader.Instance;
		}

		/// <summary>
		/// Sets a player to handle the keyboard/mouse for.
		/// </summary>
		/// <param name="obj">The player.</param>
		[LatipiumMethod("HandleFor")]
		public void HandleFor(LatipiumObject obj) {
			PlayerLoader.Instance.Object = obj;
		}

        /// <summary>
        /// Loads this module.
        /// </summary>
        /// <param name="name">The module name.</param>
		public override void Load(string name) {
			Loaded = true;
		}

        /// <summary>
        /// Loads this module.
        /// </summary>
		public void Load() {
			ModuleFactory.FindModule("Com.Latipium.Modules.Player");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Com.Latipium.Defaults.Player.PlayerModule"/> class.
		/// </summary>
		public PlayerModule() : base(new string[] { "Com.Latipium.Modules.Player" }) {
			Loaded = false;
		}
	}
}

