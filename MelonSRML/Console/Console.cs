using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using MelonLoader.Utils;
using MelonSRML.Console.Commands;
using MelonSRML.UnstrippedClasses;
using MelonSRML.Utils;
using Color = System.Drawing.Color;

namespace MelonSRML.Console
{
    /// <summary>
    /// Controls the in-game console
    /// </summary>
    public class Console
    {
        // CONFIGURE SOME NUMBERS
        public const int MAX_ENTRIES = 100; // MAX ENTRIES TO SHOW ON CONSOLE (CAN'T GO ABOVE 100, TEXT MESH GENERATOR WILL BUG IF SO)
        public const int HISTORY = 10; // NUMBER OF COMMANDS TO KEEP ON HISTORY

        // LOG STUFF
        internal static string unityLogFile = Path.Combine(Application.persistentDataPath, "Player.log");
        internal static string melonLoaderLogFile = Path.Combine(MelonEnvironment.MelonLoaderDirectory, "Latest.log");

        internal static readonly Console console = new Console();

        // COMMAND STUFF
        internal static Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();
        internal static Dictionary<string, ConsoleButton> cmdButtons = new Dictionary<string, ConsoleButton>();
        internal static List<string> hideLogs = new List<string>();

        // LINES
        internal static List<string> lines = new List<string>();

        // COMMAND HISTORY
        internal static List<string> history = new List<string>(HISTORY);

        // RELOAD EVENT (THIS IS CALLED WHEN THE COMMAND RELOAD IS CALLED, USED TO RUN A RELOAD METHOD FOR A MOD, IF THE AUTHOR WISHES TO CREATE ONE)
        public delegate void ReloadAction(); // Creates the delegate here to prevent 'TypeNotFound' exceptions

      
        // DUMP ACTIONS
        // KEY = Dump Command Argument; VALUE = The method to run
        public delegate void DumpAction(StreamWriter writer);
        internal static Dictionary<string, DumpAction> dumpActions = new Dictionary<string, DumpAction>();

        // COMMAND CATCHER
        public delegate bool CommandCatcher(string cmd, string[] args);
        internal static List<CommandCatcher> catchers = new List<CommandCatcher>();


        private static Action<Color, Color, string, string, bool> MsgToMelonLoader = AccessTools.MethodDelegate<Action<Color, Color, string, String, bool>>(AccessTools.Method(typeof(MelonLogger), "NativeMsg"));


        /// <summary>
        /// Initializes the console
        /// </summary>
        internal static void Init()
        {
            ClassInjector.RegisterTypeInIl2Cpp<ScrollViewStateModified>();
            ClassInjector.RegisterTypeInIl2Cpp<SliderStateModified>();
            ClassInjector.RegisterTypeInIl2Cpp<ConsoleWindow>();
            ConsoleBinder.ReadBinds();
            
            
            Application.add_logMessageReceived(new Action<string, string, LogType>(AppLogUnity));
            MelonLogger.MsgDrawingCallbackHandler += (color, color1, arg3, arg4) => LogEntry("INFO", arg4, arg3, color, color1);
            MelonLogger.ErrorCallbackHandler += (s, s1) => AppLog(s, s1, LogType.Error, false);
            MelonLogger.WarningCallbackHandler += (s, s1) => AppLog(s, s1, LogType.Warning, false);
            

            MelonLogger.Msg("CONSOLE INITIALIZED!");
            MelonLogger.Msg("Patching SceneManager to attach window");
            ConsoleWindow.consoleFont = Font.CreateDynamicFontFromOSFont(new string[] { "Lucida Console", "Monaco" }, 13);
            ConsoleWindow.consoleFont.hideFlags |= HideFlags.HideAndDontSave;
            ConsoleWindow.AttachConsole();

            
            RegisterCommand(new ModsCommand());
            RegisterCommand(new AddButtonCommand());
            RegisterCommand(new RemoveButtonCommand());
            RegisterCommand(new HelpCommand());
            RegisterCommand(new FastForwardCommand());
            RegisterCommand(new SpawnCommand());
            RegisterCommand(new KillCommand());
            RegisterCommand(new KillAllCommand());
            RegisterCommand(new ClearCommand());
            RegisterCommand(new GiveCommand());
            RegisterCommand(new GiveGadgetCommand());
            RegisterCommand(new AddBindingCommand());
            RegisterCommand(new RemoveBindingCommand());

            RegisterButton("clear", new ConsoleButton("Clear Console", "clear"));
            RegisterButton("help", new ConsoleButton("Show Help", "help"));
            RegisterButton("mods", new ConsoleButton("Show Mods", "mods"));
            RegisterButton("reload", new ConsoleButton("Run Reload", "reload"));
            RegisterButton("dump.all", new ConsoleButton("Dump All Files", "dump all"));
            
        }
        


        /// <summary>
        /// Registers a new command into the console
        /// </summary>
        /// <param name="cmd">Command to register</param>
        /// <returns>True if registered succesfully, false otherwise</returns>
        public static bool RegisterCommand(ConsoleCommand cmd)
        {
            if (commands.ContainsKey(cmd.ID.ToLowerInvariant()))
            {
                MelonLogger.Warning($"Trying to register command with id '<color=white>{cmd.ID.ToLowerInvariant()}</color>' but the ID is already registered!");
                return false;
            }

            // TODO: Update to new system
            cmd.belongingMod = MelonUtils.GetMelonFromStackTrace();
            commands.Add(cmd.ID.ToLowerInvariant(), cmd);
            ConsoleWindow.cmdsText += $"{(ConsoleWindow.cmdsText.Equals(string.Empty) ? "" : "\n")}<color=#77DDFF>{ColorUsage(cmd.Usage)}</color> - {cmd.Description}";
            return true;
        }

        /// <summary>
        /// Registers a new console button
        /// </summary>
        /// <param name="id">The id of the button</param>
        /// <param name="button">Button to register</param>
        /// <returns>True if registered succesfully, false otherwise</returns>
        public static bool RegisterButton(string id, ConsoleButton button)
        {
            if (id.Equals("all"))
            {
                MelonLogger.Warning($"Trying to register command button with id '<color=white>all</color>' but '<color=white>all</color>' is not a valid id!");
                return false;
            }

            if (cmdButtons.ContainsKey(id))
            {
                MelonLogger.Warning($"Trying to register command button with id '<color=white>{id}</color>' but the ID is already registered!");
                return false;
            }

            cmdButtons.Add(id, button);
            return true;
        }

        /// <summary>
        /// Registers a new dump action for the dump command
        /// </summary>
        /// <param name="id">The id to use for the dump command argument</param>
        /// <param name="action">The dump action to run</param>
        /// <returns>True if registered succesfully, false otherwise</returns>
        public static bool RegisterDumpAction(string id, DumpAction action)
        {
            if (dumpActions.ContainsKey(id.Replace(" ", string.Empty)))
            {
                MelonLogger.Warning($"Trying to register dump action with id '<color=white>{id.Replace(" ", string.Empty)}</color>' but the ID is already registered!");
                return false;
            }

            dumpActions.Add(id.Replace(" ", string.Empty), action);
            return true;
        }

        /// <summary>
        /// Registers a command catcher which allows commands to be processed and their execution controlled by outside methods
        /// </summary>
        /// <param name="catcher">The method to catch the commands</param>
        public static void RegisterCommandCatcher(CommandCatcher catcher)
        {
            catchers.Add(catcher);
        }
        


        // PROCESSES THE TEXT FROM THE CONSOLE INPUT
        internal static void ProcessInput(string command, bool forced = false)
        {
            if (command.Equals(string.Empty))
                return;

            if (!forced)
            {
                if (history.Count == HISTORY)
                    history.RemoveAt(0);

                history.Add(command);
            }

            try
            {
                MelonLogger.Msg("<color=cyan>Command: </color>" + command);

                bool spaces = command.Contains(" ");
                string cmd = spaces ? command.Substring(0, command.IndexOf(' ')) : command;

                if (commands.ContainsKey(cmd))
                {
                    bool executed = false;
                    bool keepExecution = true;
                    string[] args = spaces ? StripArgs(command) : null;

                    foreach (CommandCatcher catcher in catchers)
                    {
                        keepExecution = catcher.Invoke(cmd, args);

                        if (!keepExecution)
                            break;
                    }

                    if (keepExecution)
                    {
                        //SRMod.ForceModContext(commands[cmd].belongingMod);
                        try
                        {
                            executed = commands[cmd].Execute(args);
                        }
                        finally
                        {
                            //SRMod.ClearModContext();
                        }
                    }

                    if (!executed && keepExecution)
                        MelonLogger.Msg($"<color=cyan>Usage:</color> <color=#77DDFF>{ColorUsage(commands[cmd].Usage)}</color>");
                }
                else
                {
                    MelonLogger.Error("Unknown command. Please use '<color=white>help</color>' for available commands or check the menu on the right");
                }
            }
            catch (Exception e)
            {
                MelonLogger.Error(e);
            }
        }
        internal static string[] StripArgs(string command, bool autoComplete = false)
        {
            MatchCollection result = Regex.Matches(command.Substring(command.IndexOf(' ') + 1), "[^'\"\\s\\n]+|'[^']+'?|\"[^\"]+\"?");
            List<string> args = new List<string>(result.Count);

            foreach (Match match in result)
                args.Add(autoComplete ? match.Value : Regex.Replace(match.Value, "'|\"", ""));

            if (autoComplete && command.EndsWith(" "))
                args.Add(string.Empty);

            return args.ToArray();
        }

        // CONVERTS LOG TYPE TO A SMALLER MORE READABLE TYPE
        private string TypeToText(LogType logType)
        {
            if (logType == LogType.Error || logType == LogType.Exception)
                return "ERRO";

            return logType == LogType.Warning ? "WARN" : "INFO";
        }

        

        // LOGS A NEW ENTRY
        private static void LogEntry(string logType, string message, string name, Color nameCol, Color colorText)
        {
            if (lines.Count >= MAX_ENTRIES)
                lines.RemoveRange(0, 10);

            
            lines.Add($"<color=cyan>[{DateTime.Now:HH:mm:ss}]</color> <color=#{(nameCol.ToHexString())}>[{name}]</color> <color=#{colorText.ToHexString()}>[{logType.ToUpper()}] {Regex.Replace(message, @"<material[^>]*>|<\/material>|<size[^>]*>|<\/size>|<quad[^>]*>|<b>|<\/b>", "")}</color>");
            ConsoleWindow.updateDisplay = true;
        }


        internal static string ColorUsage(string usage)
        {
            string result = string.Empty;
            MatchCollection matches = Regex.Matches(usage, @"[\w\d]+|\<[\w]+\>|\[[\w]+\]");

            foreach (Match match in matches)
            {
                if (match.Value.StartsWith("<") && match.Value.EndsWith(">"))
                {
                    result += $" <<color=white>{match.Value.Substring(1, match.Value.Length - 2)}</color>>";
                    continue;
                }

                if (match.Value.StartsWith("[") && match.Value.EndsWith("]"))
                {
                    result += $" <i>[<color=white>{match.Value.Substring(1, match.Value.Length - 2)}</color>]</i>";
                    continue;
                }

                result += " " + match.Value;
            }

            return result.TrimStart(' ');
        }




        
        
        private static void AppLog(string namesection, string txt, LogType type, bool melonLoader = true)
        {
            Color namesectionColor = Color.Cyan;
            Color txtColor = Color.Wheat;
            if (namesection.Equals("Unity"))
                namesectionColor = Color.Yellow;

            switch (type)
            {
                case LogType.Error:
                    txtColor = Color.Red;
                    break;
                case LogType.Exception:
                    txtColor = Color.Red;
                    break;
                case LogType.Log:
                    txtColor = Color.LightGray;
                    break;
                case LogType.Assert:
                    txtColor = Color.Yellow;
                    break;
                case LogType.Warning:
                    txtColor = Color.Yellow;
                    break;
            }

            LogEntry(type.ToString(), txt, namesection, namesectionColor, txtColor);
            if (melonLoader)
                MsgToMelonLoader(namesectionColor, txtColor, namesection, txt, false);
        }

        private static void AppLogUnity(string message, string trace, LogType type)
        {
            if (message.Equals(string.Empty))
                return;

            string toDisplay = message;
            if (!trace.Equals(string.Empty))
                toDisplay += "\n" + trace;
            toDisplay = Regex.Replace(toDisplay, @"\[INFO]\s|\[ERROR]\s|\[WARNING]\s", "");

            AppLog("Unity", toDisplay, type);
        }

    }
}