using System;
using System.Collections.Generic;
using System.IO;
using Il2CppSystem.Text;
using UnityEngine.InputSystem;

namespace MelonSRML.Console;

public class KeyBindManager
{
    const string KEYBIND_FILENAME = "keybind";
    public static bool loaded = false;

    public static void Push()
    {
        string path = Path.Combine(SystemContext.Instance.GetStorageProvider().Cast<FileStorageProvider>().savePath, KEYBIND_FILENAME);
        if (!File.Exists(path))
        {
            loaded = true;
            return;
        }

        ModBindingsV01 bindings = new ModBindingsV01();
        Il2CppSystem.IO.FileStream stream = Il2CppSystem.IO.File.Open(path, Il2CppSystem.IO.FileMode.Open);
        try
        {
            Il2CppSystem.IO.BinaryReader reader = new Il2CppSystem.IO.BinaryReader(stream, Encoding.UTF8);
            if (reader.ReadString() == "srmlconsolebindings" & reader.ReadInt32() == 1)
                bindings.LoadData(reader);
            else
                throw new NullReferenceException();
        }
        finally { stream.Close(); }

        loaded = true;
    }

    public static void Pull()
    {
        if (!loaded) return;

        string path = Path.Combine(SystemContext.Instance.GetStorageProvider().Cast<FileStorageProvider>().savePath, KEYBIND_FILENAME);
        ModBindingsV01 bindings = new ModBindingsV01();

        Il2CppSystem.IO.FileStream stream = Il2CppSystem.IO.File.Open(path, Il2CppSystem.IO.FileMode.OpenOrCreate);
        try
        {
            long position = stream.Position;
            Encoding utF8 = Encoding.UTF8;
            Il2CppSystem.IO.BinaryWriter writer = new Il2CppSystem.IO.BinaryWriter(stream, utF8);
            writer.Write("srmlconsolebindings");
            writer.Write(1U);
            bindings.WriteData(writer);
        }
        finally { stream.Close(); }
    }
    internal class ModBindingsV01
    {
        public static List<ConsoleBind> consoleBinds = new List<ConsoleBind>();

        public void WriteData(Il2CppSystem.IO.BinaryWriter writer)
        {
            writer.Write(consoleBinds.Count);
            foreach (ConsoleBind bind in consoleBinds)
            {
                writer.Write((int)bind.Key);
                writer.Write(bind.Command.Length);
                foreach (byte b in Encoding.UTF8.GetBytes(bind.Command))
                    writer.Write(b);
            }
        }

        public void LoadData(Il2CppSystem.IO.BinaryReader reader)
        {
            consoleBinds.Clear();

            int dictLength = reader.ReadInt32();
            Dictionary<Key, byte[]> dict = new Dictionary<Key, byte[]>();
            for (int j = 0; j < dictLength; j++)
            {
                int key = reader.ReadInt32();
                byte[] ints = reader.ReadBytes(reader.ReadInt32());
                dict.Add((Key)key, ints);
            }

            foreach (var kvp in dict)
            {
                ConsoleBind bind = new ConsoleBind(kvp.Key, Encoding.UTF8.GetString(kvp.Value));
                consoleBinds.Add(bind);
            }
        }
    }
    internal class ConsoleBind
    {
        public readonly Key Key;
        public string Command;
        public InputAction action;

        public ConsoleBind(Key key, string command)
        {
            action = new InputAction();
            action.AddBinding(Keyboard.current[key]);
            action.Enable();

            Key = key;
            Command = command;
        }
    }
}