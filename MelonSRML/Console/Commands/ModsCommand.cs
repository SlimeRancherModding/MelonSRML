namespace MelonSRML.Console.Commands
{
	/// <summary>
	/// A command to display all mods
	/// </summary>
	public class ModsCommand : ConsoleCommand
	{
		public override string ID => "mods";
		public override string Usage { get; } = "mods";
		public override string Description { get; } = "Displays all mods loaded";

		public override bool Execute(string[] args)
		{
			if (args != null)
			{
				MelonLogger.Error($"The '<color=white>{ID}</color>' command takes no arguments");
				return false;
			}

			MelonLogger.Msg("<color=cyan>List of Mods Loaded:</color>");

			foreach (string line in ConsoleWindow.modsText.Split('\n'))
				MelonLogger.Msg(line);

			return true;
		}
	}
}
