using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace The_Rubber_Ducky_Ninja_WinUI3
{ 
    internal class DuckyScriptProcessing
    {
        private bool defaultdelay = false;
        private int defaultdelayvalue = 0;
        private string lastCommand;
        private string lastKey;
        private bool isCapsEnabled = false;
        private Dictionary<string, string> constants = new Dictionary<string, string>();
        private Dictionary<string, object> variables = new Dictionary<string, object>();
        Validation validation = new Validation();
                
        public void SetDelay(int delay) //sets the global delay
        {
            if (delay > 1)
            {
                defaultdelay = true; 
            }
            defaultdelayvalue = delay;
        }

        public async Task ReadFileAsync(String FilePath) //Reads file and calls calculate for each line
        {
            string[] duckyFile = await File.ReadAllLinesAsync(FilePath);
            foreach (var currentLine in duckyFile)
            {
                Calculate(currentLine);
                await Task.Delay(1); // Allow UI updates
            }
        }

        public void Calculate(string currentLine) //splits the line into command & keys
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(currentLine))
                return;

            string[] words = currentLine.Split(' ');
            string command = words[0];
            string keys = "";
            int flag = 0;
            for (int i = 1; i < words.Length; i++)
            {
                if (flag == 0)
                {
                    keys += words[i];
                    flag++;
                }
                else
                {
                    keys += " " + words[i];
                }
            }
            KeyboardAction(command, keys);
        }

        private void CheckDefaultSleep() //checks if their is a delay set. If so, delays
        {
            if (defaultdelay == true)
            {
                Thread.Sleep(defaultdelayvalue);
            }
        }

        private void setLastCommand(string command, string keys) //sets the last command (for replay function)
        {
            lastCommand = command;
            lastKey = keys;
        }

        private string SubstituteConstants(string input) //substitutes constants in strings
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            // Match constants in the format #CONSTANT
            var matches = Regex.Matches(input, @"#([A-Za-z0-9_]+)");
            string result = input;
            
            foreach (Match match in matches)
            {
                string constName = match.Groups[1].Value;
                if (constants.ContainsKey(constName))
                {
                    // Replace the entire match (#CONSTANT) with the constant value
                    result = result.Replace(match.Value, constants[constName]);
                }
            }
            
            return result;
        }

        public bool validateCode(string FilePath) //validates the commands in a duckyscript
        {
            string[] duckyFile = File.ReadAllLines(FilePath);
            int currentLineNum = 1;
            foreach (var currentLine in duckyFile)
            {
                string[] words = currentLine.Split(' ');
                string command = words[0];
                string keys = "";
                int flag = 0;
                for (int i = 1; i < words.Length; i++)
                {
                    if (flag == 0)
                    {
                        keys += words[i];
                        flag++;
                    }
                    else
                    {
                        keys += " " + words[i];
                    }
                }
                bool result = validation.LineCheck(command,keys,currentLineNum);
                if (result == false)
                {
                    return false;
                }
                currentLineNum++;
            }
            return true;
        }

        // ... rest of KeyboardAction method stays the same ...
        private void KeyboardAction(string command, string keys)
        {
            // Your existing keyboard action implementation
            // This stays exactly the same as your current code
        }
    }
} 