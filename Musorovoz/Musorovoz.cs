using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Enums;
using MEC;

namespace Musorovoz
{
	internal class Musorovoz
	{
		private static readonly Dictionary<Player, List<ItemType>> musorovozInventory = new();

		/// <summary>
		/// Сделать игрока мусоровозом
		/// </summary>
		/// <param name="player">Игрок</param>
		/// <returns>Успешно ли</returns>
		public static bool MakePlayerMusorovoz(Player player)
		{
			if (musorovozInventory.ContainsKey(player))
			{
				return false;
			}

			player.DropItems();
			musorovozInventory.Add(player, new List<ItemType>());
			return true;
		}

		/// <summary>
		/// Является ли игрок мусоровозом
		/// </summary>
		/// <param name="player">Игрок</param>
		/// <returns>Успешно ли</returns>
		public static bool IsPlayerMusorovoz(Player player)
		{
			return musorovozInventory.ContainsKey(player);
		}

		/// <summary>
		/// Положить предмет в мусоровоз игрока
		/// </summary>
		/// <param name="player">Игрок мусоровоз</param>
		/// <param name="item">Предмето</param>
		/// <returns>Успешно ли</returns>
		public static bool PutItemInMusorovoz(Player player, Pickup item)
		{
			if (!musorovozInventory.ContainsKey(player))
			{
				return false;
			}

			musorovozInventory[player].Add(item.Type);
			item.Destroy();
			return true;
		}

		/// <summary>
		/// Выгрузить мусоровоз игрока
		/// </summary>
		/// <param name="player">Игрок мусоровоз</param>
		/// <returns>Успешно ли</returns>
		public static bool UnloadMusorovoz(Player player)
		{
			if (!musorovozInventory.ContainsKey(player))
			{
				return false;
			}

			if (player.CurrentRoom.Zone is not ZoneType.Surface)
			{
				player.Broadcast(5, $"Мусоровоз должен быть на улице, чтобы выгрузиться", shouldClearPrevious: true);
				return false;
			}

			if (musorovozInventory[player].Count == 0)
			{
				player.Broadcast(5, $"Мусоровоз пустой", shouldClearPrevious: true);
				return false;
			}

			Timing.RunCoroutine(DropItemsFromMusorovoz(player));

			return true;
		}

		/// <summary>
		/// Выбрасывание предметов из мусоровоза с задержкой
		/// </summary>
		/// <param name="musorovoz">Игрок мусоровоз</param>
		/// <returns>Успешно ли</returns>
		private static IEnumerator<float> DropItemsFromMusorovoz(Player musorovoz)
		{
			var delay = Plugin.Instance.Config.MusorovozDropItemDelay;
			musorovoz.EnableEffect(EffectType.Ensnared, delay * musorovozInventory[musorovoz].Count);

			for (int i = 0; i < musorovozInventory[musorovoz].Count; i++)
			{
				var item = musorovozInventory[musorovoz][i];
				var position = musorovoz.Position - musorovoz.Transform.forward;
				Item.Create(item).CreatePickup(position);

				musorovoz.Broadcast(5, $"Выгружено {i}/{musorovozInventory[musorovoz].Count} из мусоровоза", shouldClearPrevious: true);

				yield return Timing.WaitForSeconds(delay);
			}

			musorovozInventory[musorovoz].Clear();
			musorovoz.Broadcast(5, $"Мусоровоз выгружен", shouldClearPrevious: true);

		}
	}
}
