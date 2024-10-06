using Exiled.API.Interfaces;

namespace Musorovoz
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;
		public bool Debug { get; set; }

		/// <summary>
		/// Время между выгрузкой предметов из мусоровоза
		/// </summary>
		public float MusorovozDropItemDelay { get; set; } = 0.5f;
	}
}
