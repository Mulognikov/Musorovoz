using Exiled.Events.EventArgs.Player;

namespace Musorovoz.Events
{
	internal sealed class PlayerHandler
	{
		/// <inheritdoc cref="Exiled.Events.Handlers.Player.OnPickingUpItem(PickingUpItemEventArgs)"/>
		public void OnPickingUpItem(PickingUpItemEventArgs ev)
		{
			if (!Musorovoz.IsPlayerMusorovoz(ev.Player))
			{
				return;
			}

			ev.IsAllowed = false;

			if (Musorovoz.PutItemInMusorovoz(ev.Player, ev.Pickup))
			{
				ev.Player.Broadcast(5, $"Предмет {ev.Pickup.Type} был загружен в мусоровоз", shouldClearPrevious: true);
			}
		}
	}
}
