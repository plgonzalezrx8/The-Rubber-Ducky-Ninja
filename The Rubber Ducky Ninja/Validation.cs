﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace The_Rubber_Ducky_Ninja
{

    class Validation
    {
        //This class is going to be used to validate the DuckyScript (instead of just checking the 1st word like before)

        private string[] validFKeys = new string[12] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };
        private string[] validShiftKeys = new string[12] { "DELETE", "HOME", "INSERT", "PAGEUP", "PAGEDOWN", "WINDOWS", "GUI", "UPARROW", "DOWNARROW", "LEFTARROW", "RIGHTARROW", "TAB" };
        private string[] validCTRLkeys = new string[4] { "BREAK", "PAUSE", "ESCAPE", "ESC" };
        private string[] validAltKeys = new string[6] { "ALT", "END", "ESC", "ESCAPE", "SPACE", "TAB" };
        private string[] validBooleanValues = new string[2] { "TRUE", "FALSE" };

        public bool LineCheck(string command, string keys, int currentLine)
        {
            switch (command)
            {
                case "VAR":
                    if (string.IsNullOrEmpty(keys))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", VAR command requires a variable name and value.");
                        return false;
                    }
                    // Check for proper VAR syntax: VAR $NAME = VALUE
                    if (!keys.Contains("="))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", VAR command requires an equals sign (e.g., VAR $NAME = VALUE).");
                        return false;
                    }
                    string[] varParts = keys.Split(new[] { '=' }, 2);
                    if (varParts.Length != 2)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", VAR command requires both a variable name and value.");
                        return false;
                    }
                    string varNameWithPrefix = varParts[0].Trim();
                    string varValue = varParts[1].Trim();
                    
                    // Check variable name
                    if (!varNameWithPrefix.StartsWith("$"))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", variable name must start with $ (e.g., $VARIABLE).");
                        return false;
                    }
                    string varNameWithoutPrefix = varNameWithPrefix.Substring(1); // Remove $ for validation
                    if (string.IsNullOrEmpty(varNameWithoutPrefix) || !Regex.IsMatch(varNameWithoutPrefix, @"^[A-Za-z0-9_]+$"))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", variable name must contain only letters, numbers, and underscores (after the $ prefix).");
                        return false;
                    }
                    
                    // Check variable value
                    if (validBooleanValues.Contains(varValue.ToUpper()))
                    {
                        // Boolean value is valid
                        break;
                    }
                    
                    // Try to parse as integer
                    if (!int.TryParse(varValue, out int intValue))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", variable value must be an integer (0-65535) or TRUE/FALSE.");
                        return false;
                    }
                    
                    // Check integer range
                    if (intValue < 0 || intValue > 65535)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", integer variable value must be between 0 and 65535.");
                        return false;
                    }
                    break;
                case "DEFINE":
                    if (string.IsNullOrEmpty(keys))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", DEFINE command requires a variable name and value.");
                        return false;
                    }
                    string[] defineParts = keys.Split(new[] { ' ' }, 2);
                    if (defineParts.Length != 2)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", DEFINE command requires both a variable name and value (e.g., DEFINE #MYVAR myvalue).");
                        return false;
                    }
                    string varName = defineParts[0].Trim();
                    if (varName.StartsWith("#"))
                    {
                        varName = varName.Substring(1);
                    }
                    if (string.IsNullOrEmpty(varName) || !Regex.IsMatch(varName, @"^[A-Za-z0-9_]+$"))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", variable name must contain only letters, numbers, and underscores (after the # prefix if used).");
                        return false;
                    }
                    break;
                case "REM":
                    break;
                case "STRING":
                    if (keys.Length <= 0)
                    {
                        MessageBox.Show("On line " + currentLine + " your STRING command is empty. This means it will type nothing." +
                            " This isn't a compile error. Just thought I'd let you know");
                    }
                    break;
                case "STRINGLN":
                    if (keys.Length <= 0)
                    {
                        MessageBox.Show("On line " + currentLine + " your STRINGLN command is empty. This means it will type nothing and just press ENTER." +
                            " This isn't a compile error. Just thought I'd let you know");
                    }
                    break;
                case "DEFAULT_DELAY":
                case "DEFAULTDELAY":
                    try
                    {
                        Convert.ToInt32(keys);
                    }
                    catch
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", the command following DEFAULT_DELAY/DEFAULTDELAY is not an integer (ex 500)");
                        return false;
                    }
                    break;

                case "DELAY":
                    try
                    {
                        Convert.ToInt32(keys);
                    }
                    catch
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", the command following delay is not a integer (ex 500)");
                        return false;
                    }
                    break;

                case "WINDOWS":
                case "GUI":
                    // GUI/WINDOWS can be used alone or with a single key
                    // Valid examples: GUI, GUI r, WINDOWS r
                    if (keys.Length > 0 && keys.Split(' ').Length > 1)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", GUI/WINDOWS can only be used with a single key or alone.");
                        return false;
                    }
                    break;

                case "ENTER":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", their is a command following ENTER. ENTER function doesn't support keyboard combos");
                        return false;
                    }
                    break;

                case "APP":
                case "MENU":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", their is a command following MENU/APP. MENU function doesn't support keyboard combos");
                        return false;
                    }
                    break;

                case "SHIFT":
                    if (keys.Length > 0 && !validShiftKeys.Contains(keys))
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", the command following SHIFT is invalid. Please look at DuckyScript documentation for more info");
                        return false;
                    }
                    break;

                case "ALT":
                    // ALT can be used alone or with function keys, letters, or other valid keys
                    if (keys.Length > 0)
                    {
                        // Check if it's a valid F-key, single letter, or in validAltKeys
                        if (!validFKeys.Contains(keys) && !validAltKeys.Contains(keys) && 
                            !(keys.Length == 1 && char.IsLetterOrDigit(keys[0])))
                        {
                            MessageBox.Show("Error. On line " + currentLine + ", the command following ALT is not valid. See official DuckyScript documentation for compatible functions");
                            return false;
                        }
                    }
                    break;

                case "CONTROL":
                case "CTRL":
                    // CTRL can be used alone or with function keys, letters, or other valid keys
                    if (keys.Length > 0)
                    {
                        // Check if it's a valid F-key, single letter, or in validCTRLkeys
                        if (!validFKeys.Contains(keys) && !validCTRLkeys.Contains(keys) && 
                            !(keys.Length == 1 && char.IsLetterOrDigit(keys[0])))
                        {
                            MessageBox.Show("Error. On line " + currentLine + ", the command following CTRL is not valid. See official DuckyScript documentation for compatible functions");
                            return false;
                        }
                    }
                    break;

                case "TAB":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following TAB.");
                        return false;
                    }
                    break;

                case "DOWNARROW":
                case "DOWN":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following DOWN.");
                        return false;
                    }
                    break;

                case "LEFTARROW":
                case "LEFT":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following LEFT.");
                        return false;
                    }
                    break;

                case "RIGHTARROW":
                case "RIGHT":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following RIGHT.");
                        return false;
                    }
                    break;

                case "UPARROW":
                case "UP":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following UP.");
                        return false;
                    }
                    break;

                case "REPLAY":
                    try
                    {
                        Convert.ToInt32(keys);
                    }
                    catch
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", the command following REPLAY is not a integer (ex 500)");
                        return false;
                    }
                    break; 
                case "DELETE":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following DELETE.");
                        return false;
                    }
                    break;
                case "CAPS":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following CAPS.");
                        return false;
                    }
                    break;
                case "SPACE":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following SPACE.");
                        return false;
                    }
                    break;
                case "PRINTSCREEN":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following PRINTSCREEN.");
                        return false;
                    }
                    break;

                case "BREAK":
                case "PAUSE":
                    if (keys.Length > 0)
                    {
                        MessageBox.Show("Error. On line " + currentLine + ", there is a command following BREAK/PAUSE.");
                        return false;
                    }
                    break;

                default:
                    MessageBox.Show("Error. On line " + currentLine + ", the command you are trying to run was not recognized.");
                    return false;
            }
            return true;
        }

    }
}
