﻿using System.Collections.Generic;

namespace MelonSRML.Console.Commands
{
	/// <summary>
	/// A command to add a user defined button to the command menu
	/// </summary>
	public class AddButtonCommand : ConsoleCommand
	{
		public override string ID { get; } = "addbutton";
		public override string Usage { get; } = "addbutton <id> <text> <command>";
		public override string Description { get; } = "Adds a user defined button to the command menu";

		public override string ExtendedDescription => 
			"<color=#77DDFF><id></color> - The id of the button. '<color=#77DDFF>all</color>' is not a valid id\n" +
			"<color=#77DDFF><text></color> - The text to display on the button\n" +
			"<color=#77DDFF><command></color> - The command the button will execute";

		public override bool Execute(string[] args)
		{
			if (args == null)
			{
				MelonLogger.Error($"The '<color=white>{ID}</color>' command takes 3 arguments");
				return false;
			}

			if (ArgsOutOfBounds(args.Length, 3, 3))
				return false;

			if (args[0].Contains(' '))
			{
				MelonLogger.Error($"The '<color=white><id></color>' argument cannot contain any spaces");
				return false;
			}

			if (args[0].Equals("all"))
			{
				MelonLogger.Error($"Trying to register user defined button with id '<color=white>all</color>' but '<color=white>all</color>' is not a valid id!");
				return false;
			}

			if (Console.cmdButtons.ContainsKey("user." + args[0]))
			{
				MelonLogger.Warning($"Trying to register user defined button with id '<color=white>{args[0]}</color>' but the ID is already registered!");
				return false;
			}

			ConsoleBinder.RegisterBind(args[0], args[1], args[2]);
			MelonLogger.Msg($"Added new user defined button '<color=white>{args[0]}</color>' with command '<color=white>{args[2]}</color>'");

			return true;
		}

		public override List<string> GetAutoComplete(int argIndex, string argText)
		{
			return argIndex == 2 ? new List<string>(Console.commands.Keys) : base.GetAutoComplete(argIndex, argText);
		}
	}
}