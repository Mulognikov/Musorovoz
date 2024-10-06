using Exiled.API.Features;
using Musorovoz.Events;

namespace Musorovoz
{
	internal class Plugin : Plugin<Config>
	{
		private static readonly Plugin Singleton = new();
		public static Plugin Instance => Singleton;

		private PlayerHandler _playerHandler;

		public override void OnEnabled()
		{
			base.OnEnabled();
			RegisterEvents();
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			UnregisterEvents();
		}

		private void RegisterEvents()
		{
			_playerHandler = new PlayerHandler();

			Exiled.Events.Handlers.Player.PickingUpItem += _playerHandler.OnPickingUpItem;
		}

		private void UnregisterEvents()
		{
			_playerHandler = null;

			Exiled.Events.Handlers.Player.PickingUpItem -= _playerHandler.OnPickingUpItem;
		}
	}
}
