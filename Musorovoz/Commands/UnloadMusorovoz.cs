using System;
using CommandSystem;
using Exiled.API.Features;

namespace Musorovoz.Commands
{

	[CommandHandler(typeof(ClientCommandHandler))]
	public class UnloadMusorovoz : ICommand
	{
		public string Command => "unload";

		public string[] Aliases { get; } = { };

		public string Description => "Выгрузить мусор";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (arguments.Count != 0)
			{
				response = "Использование: unload";
				return false;
			}

			var musorovoz = Player.Get(sender);

			if (musorovoz is null)
			{
				response = $"Игрок не найден";
				return false;
			}

			if (Musorovoz.UnloadMusorovoz(musorovoz))
			{
				response = $"Игрок {musorovoz.CustomName} ({musorovoz.Id}) выгрузил мусоровоз";
				return true;
			}

			response = "Условия выгрузки мусоровоза не выполнены";
			return false;
		}
	}
}
