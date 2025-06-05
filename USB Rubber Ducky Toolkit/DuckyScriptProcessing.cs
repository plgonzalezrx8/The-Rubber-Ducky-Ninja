using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace The_Rubber_Ducky_Ninja
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

        public void ReadFile(String FilePath) //Reads file and calls calculate for each line
        {
            string[] duckyFile = File.ReadAllLines(FilePath);
            foreach (var currentLine in duckyFile)
            {
                Calculate(currentLine);
            }
        }

        public void Calculate(string currentLine) //splits the line into command & keys
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

        private string SubstituteConstants(string input)
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

        private object GetVariableValue(string varName)
        {
            if (string.IsNullOrEmpty(varName)) return null;
            
            // Remove $ prefix if present
            if (varName.StartsWith("$"))
            {
                varName = varName.Substring(1);
            }
            
            if (variables.ContainsKey(varName))
            {
                return variables[varName];
            }
            
            return null;
        }

        private void KeyboardAction(string command, string keys) //executes the code line by line.
        {
            string keyboardkey = keys.ToUpper();
            VirtualKeyCode code;
            try
            {
                switch (command)
                {
                    case "STRING":
                        // Substitute any constants in the string
                        string textToType = SubstituteConstants(keys);
                        InputSimulator.SimulateTextEntry(textToType);
                        CheckDefaultSleep();
                        break;

                    case "ENTER":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                        CheckDefaultSleep();
                        break;

                    case "DELAY":
                        int delay;
                        if (int.TryParse(keys, out delay))
                        {
                            Thread.Sleep(delay);
                        }
                        break;

                    case "GUI":
                    case "WINDOWS":
                        if (keyboardkey.Length > 0)
                        {
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, GetKeyCode(keyboardkey));
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LWIN);
                        }
                        CheckDefaultSleep();
                        break;

                    case "ALT":
                        if (keyboardkey.Length > 0)
                        {
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, GetKeyCode(keyboardkey));
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LMENU);
                        }
                        CheckDefaultSleep();
                        break;

                    case "SHIFT":
                        if (keyboardkey.Length > 0)
                        {
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LSHIFT, GetKeyCode(keyboardkey));
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LSHIFT);
                        }
                        CheckDefaultSleep();
                        break;

                    case "CTRL":
                    case "CONTROL":
                        if (keyboardkey.Length > 0)
                        {
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LCONTROL, GetKeyCode(keyboardkey));
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LCONTROL);
                        }
                        CheckDefaultSleep();
                        break;

                    case "TAB":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                        CheckDefaultSleep();
                        break;

                    case "DELETE":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DELETE);
                        CheckDefaultSleep();
                        break;

                    case "BACKSPACE":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.BACK);
                        CheckDefaultSleep();
                        break;

                    case "UPARROW":
                    case "UP":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
                        CheckDefaultSleep();
                        break;

                    case "DOWNARROW":
                    case "DOWN":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
                        CheckDefaultSleep();
                        break;

                    case "LEFTARROW":
                    case "LEFT":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
                        CheckDefaultSleep();
                        break;

                    case "RIGHTARROW":
                    case "RIGHT":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
                        CheckDefaultSleep();
                        break;

                    case "SPACE":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        CheckDefaultSleep();
                        break;

                    case "CAPSLOCK":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.CAPITAL);
                        isCapsEnabled = !isCapsEnabled;
                        CheckDefaultSleep();
                        break;

                    case "NUMLOCK":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NUMLOCK);
                        CheckDefaultSleep();
                        break;

                    case "SCROLLLOCK":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SCROLL);
                        CheckDefaultSleep();
                        break;

                    case "ESCAPE":
                    case "ESC":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.ESCAPE);
                        CheckDefaultSleep();
                        break;

                    case "INSERT":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.INSERT);
                        CheckDefaultSleep();
                        break;

                    case "HOME":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.HOME);
                        CheckDefaultSleep();
                        break;

                    case "PAGEUP":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.PRIOR);
                        CheckDefaultSleep();
                        break;

                    case "PAGEDOWN":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
                        CheckDefaultSleep();
                        break;

                    case "END":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.END);
                        CheckDefaultSleep();
                        break;

                    case "BREAK":
                    case "PAUSE":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.PAUSE);
                        CheckDefaultSleep();
                        break;

                    case "F1":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F1);
                        CheckDefaultSleep();
                        break;

                    case "F2":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F2);
                        CheckDefaultSleep();
                        break;

                    case "F3":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F3);
                        CheckDefaultSleep();
                        break;

                    case "F4":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F4);
                        CheckDefaultSleep();
                        break;

                    case "F5":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F5);
                        CheckDefaultSleep();
                        break;

                    case "F6":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F6);
                        CheckDefaultSleep();
                        break;

                    case "F7":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F7);
                        CheckDefaultSleep();
                        break;

                    case "F8":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F8);
                        CheckDefaultSleep();
                        break;

                    case "F9":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F9);
                        CheckDefaultSleep();
                        break;

                    case "F10":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F10);
                        CheckDefaultSleep();
                        break;

                    case "F11":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F11);
                        CheckDefaultSleep();
                        break;

                    case "F12":
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.F12);
                        CheckDefaultSleep();
                        break;

                    case "DEFINE":
                        // DEFINE command should be processed before execution
                        // Format: DEFINE #CONSTANT value
                        string[] defineParts = keys.Split(new[] { ' ' }, 2);
                        if (defineParts.Length == 2)
                        {
                            string constName = defineParts[0].Trim();
                            string constValue = defineParts[1].Trim();
                            
                            // Remove # prefix if present for storage
                            if (constName.StartsWith("#"))
                            {
                                constName = constName.Substring(1);
                            }
                            
                            if (!string.IsNullOrEmpty(constName))
                            {
                                constants[constName] = constValue;
                            }
                        }
                        break;

                    case "VAR":
                        // VAR command for variables
                        // Format: VAR $VARIABLE = VALUE
                        string[] varParts = keys.Split(new[] { '=' }, 2);
                        if (varParts.Length == 2)
                        {
                            string varName = varParts[0].Trim();
                            string varValue = varParts[1].Trim();
                            
                            // Remove $ prefix if present for storage
                            if (varName.StartsWith("$"))
                            {
                                varName = varName.Substring(1);
                            }
                            
                            if (!string.IsNullOrEmpty(varName))
                            {
                                // Try to parse as boolean first
                                if (varValue.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                                {
                                    variables[varName] = true;
                                }
                                else if (varValue.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                                {
                                    variables[varName] = false;
                                }
                                else
                                {
                                    // Try to parse as number
                                    int intValue;
                                    if (int.TryParse(varValue, out intValue))
                                    {
                                        variables[varName] = intValue;
                                    }
                                    else
                                    {
                                        // Store as string
                                        variables[varName] = varValue;
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        // Try to parse as a single key press
                        if (Enum.TryParse<VirtualKeyCode>(keyboardkey, out code))
                        {
                            InputSimulator.SimulateKeyPress(code);
                            CheckDefaultSleep();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing command: " + command + " with keys: " + keys + "\nError: " + ex.Message);
            }
            if (command != "REPLAY" && command != "REM") //If the last function wasn't replay/rem. Make the current command the last one
            {
                setLastCommand(command, keys);  //Used for the repeat function
            }
        }

        private VirtualKeyCode GetKeyCode(string key)
        {
            VirtualKeyCode code;
            if (Enum.TryParse<VirtualKeyCode>(key, out code))
            {
                return code;
            }
            return VirtualKeyCode.VK_A; // Default to 'A' if key not found
        }
    }
}