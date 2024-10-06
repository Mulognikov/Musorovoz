using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;

namespace Musorovoz.Commands
{

	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class SetMusorovozCommand : ICommand
	{
		public string Command => "musorovoz";

		public string[] Aliases { get; } = { };

		public string Description => "Сделать игрока мусоровозом";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count != 1)
			{
				response = "Использование: musorovoz <id игрока>";
				return false;
			}

			int.TryParse(arguments.First(), out var playerId);
			var musorovoz = Player.List.FirstOrDefault(player => player.Id == playerId);

			if (musorovoz is null)
			{
				response = $"Игрок с id {playerId} не найден";
				return false;
			}

			if (Musorovoz.IsPlayerMusorovoz(musorovoz))
			{
				response = $"Игрок {musorovoz.CustomName} ({musorovoz.Id}) уже мусоровоз";
				return false;
			}

			if (Musorovoz.MakePlayerMusorovoz(musorovoz))
			{
				response = $"Игрок {musorovoz.CustomName} ({musorovoz.Id}) теперь мусоровоз";
				musorovoz.Broadcast(5, "Ты теперь мусоровоз!!!", shouldClearPrevious: true);
				return true;
			}

			response = $"Игрока {musorovoz.CustomName} ({musorovoz.Id}) не удалось сделать мусоровозом";
			return false;
		}
	}
}
