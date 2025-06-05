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
                    case "DEFAULT_DELAY":
                    case "DEFAULTDELAY":
                        defaultdelay = true;
                        defaultdelayvalue += Convert.ToInt32(keys);
                        break;

                    case "DELAY":
                        CheckDefaultSleep();
                        Thread.Sleep(Convert.ToInt32(keys));
                        break;

                    case "STRING":
                        CheckDefaultSleep();
                        string textToType = SubstituteConstants(keys);
                        if (isCapsEnabled == true)
                        {
                            InputSimulator.SimulateTextEntry((textToType.ToUpper()));
                        } else
                        {
                            InputSimulator.SimulateTextEntry(textToType);
                        }
                        break;

                    case "STRINGLN":
                        CheckDefaultSleep();
                        string textToTypeLn = SubstituteConstants(keys);
                        if (isCapsEnabled == true)
                        {
                            InputSimulator.SimulateTextEntry((textToTypeLn.ToUpper()));
                        } else
                        {
                            InputSimulator.SimulateTextEntry(textToTypeLn);
                        }
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                        break;

                    case "WINDOWS":
                    case "GUI":
                        CheckDefaultSleep();
                        if (keyboardkey.Length > 0)
                        {
                            if (Enum.TryParse<VirtualKeyCode>("VK_" + keyboardkey, out code))
                            {
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, code);
                            }
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LWIN);
                        }
                        
                        break;

                    case "ENTER":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                        break;

                    case "APP":
                    case "MENU":
                        CheckDefaultSleep();
                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.F10);
                        break;

                    case "SHIFT":
                        CheckDefaultSleep();
                        switch (keys)
                        {
                            case "WINDOWS":
                            case "GUI":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.LWIN);
                                break;

                            case "UPARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.UP);
                                break;

                            case "DOWNARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.DOWN);
                                break;

                            case "LEFTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.LEFT);
                                break;

                            case "RIGHTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.RIGHT);
                                break;

                            default:
                                if (Enum.TryParse<VirtualKeyCode>("" + keyboardkey, out code))
                                {
                                    InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, code);
                                }
                                break;
                        }

                        break;

                    case "ALT":
                        CheckDefaultSleep();
                        switch (keys)
                        {
                            case "WINDOWS":
                            case "GUI":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.LWIN);
                                break;

                            case "UPARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.UP);
                                break;

                            case "DOWNARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.DOWN);
                                break;

                            case "LEFTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.LEFT);
                                break;

                            case "RIGHTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, VirtualKeyCode.RIGHT);
                                break;

                            default:
                                if (keyboardkey.Length > 1)
                                {
                                    //For support for keys such as "end"
                                    if (Enum.TryParse<VirtualKeyCode>("" + keyboardkey, out code))
                                    {
                                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, code);
                                    }
                                }
                                else
                                {
                                    if (Enum.TryParse<VirtualKeyCode>("VK_" + keyboardkey, out code))
                                    {
                                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, code);
                                    }
                                }
                                break;
                        }
                        break;

                    case "CONTROL":
                    case "CTRL":
                        CheckDefaultSleep();
                        switch (keys)
                        {
                            case "ESC":
                            case "ESCAPE":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.ESCAPE);
                                break;
                            case "PAUSE":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.PAUSE);
                                break;

                            case "UPARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.UP);
                                break;

                            case "DOWNARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.DOWN);
                                break;

                            case "LEFTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.LEFT);
                                break;

                            case "RIGHTARROW":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.RIGHT);
                                break;
                            case "SHIFT":
                                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT);
                                break;

                            default:
                                if (keyboardkey.Length > 1)
                                {
                                    //For support for keys such as "end"
                                    if (Enum.TryParse<VirtualKeyCode>("" + keyboardkey, out code))
                                    {
                                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, code);
                                    }
                                }
                                else
                                {
                                    if (Enum.TryParse<VirtualKeyCode>("VK_" + keyboardkey, out code))
                                    {
                                        InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.CONTROL, code);
                                    }
                                }
                                break;
                        }
                        break;

                    case "TAB":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                        break;

                    case "DOWNARROW":
                    case "DOWN":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
                        break;

                    case "LEFTARROW":
                    case "LEFT":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
                        break;

                    case "RIGHTARROW":
                    case "RIGHT":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
                        break;

                    case "UPARROW":
                    case "UP":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
                        break;
                    
                    case "REPLAY":
                        CheckDefaultSleep();
                        for (int i = 0; i < Convert.ToInt32(keys); i++)
                        {
                            KeyboardAction(lastCommand, lastKey);
                        }
                        break;

                    case "DELETE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DELETE);
                        break;

                    case "CAPS":
                        CheckDefaultSleep();
                        isCapsEnabled = !isCapsEnabled;
                        break;
                    case "SPACE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        break;
                    case "PRINTSCREEN":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.PRINT);
                        break;

                    case "DEFINE":
                        // DEFINE command for constants
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
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error while trying to simulate keys. Probably UAC getting in the way. " +
                    "This toolkit cannot type when UAC is open for security reasons, try disabling UAC and running the script again ");
            }
            if (command != "REPLAY" && command != "REM" && command != "DEFINE" && command != "VAR") //If the last function wasn't replay/rem/define/var. Make the current command the last one
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