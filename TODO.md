# DuckyScript Commands TODO List

This document lists all DuckyScript commands from the [Hak5 DuckyScript Quick Reference](https://docs.hak5.org/hak5-usb-rubber-ducky/duckyscript-tm-quick-reference) that are not currently implemented in The Rubber Ducky Ninja. Each command includes implementation notes and considerations.

## Currently Implemented Commands

The following commands are already implemented in the codebase:

### Basic Commands

- STRING (with constant substitution support)
- STRINGLN (with constant substitution support)
- ENTER
- DELAY (with constant substitution support)
- DEFAULT_DELAY/DEFAULTDELAY
- REM (comments and blank lines)

### Modifier Keys

- GUI/WINDOWS (with modifier support)
- ALT (with function keys, letters, and arrow keys)
- CONTROL/CTRL (with function keys, letters, and arrow keys)
- SHIFT (with various key combinations)

### Navigation and Special Keys

- TAB
- UP/UPARROW
- DOWN/DOWNARROW
- LEFT/LEFTARROW
- RIGHT/RIGHTARROW
- DELETE
- SPACE
- PRINTSCREEN
- APP/MENU (context menu key)

### Advanced Features

- CAPS (toggle)
- REPLAY
- DEFINE (constants with # prefix)
- VAR (variables with $ prefix - integers 0-65535 and booleans TRUE/FALSE)

## Commands To Implement

### 1. Function Keys (F1-F12)

**Description:** Presses function keys F1 through F12
**Current Status:** Not implemented as standalone commands
**Implementation Notes:**

- Function keys work with modifier keys (ALT, CTRL, SHIFT) but not as standalone commands
- Should be implemented in DuckyScriptProcessing.cs in the KeyboardAction method
- Should use VirtualKeyCode.F1 through VirtualKeyCode.F12
- Similar to other single key commands
- High priority due to common usage

### 2. ESCAPE (ESC)

**Description:** Presses the ESC key
**Current Status:** Partially implemented (works with CTRL modifier only)
**Implementation Notes:**

- Currently only works as CTRL ESC or CTRL ESCAPE
- Need to implement as standalone ESC command
- Should use VirtualKeyCode.ESCAPE
- Similar to other single key commands

### 3. Navigation Keys

**Description:** HOME, END, INSERT, PAGEUP, PAGEDOWN
**Current Status:** Not implemented as standalone commands
**Implementation Notes:**

- These work with SHIFT modifier but not as standalone commands
- Should be implemented in KeyboardAction method
- Should use appropriate VirtualKeyCode values
- Medium priority

### 4. Lock Keys

**Description:** NUMLOCK, SCROLLLOCK
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.NUMLOCK and VirtualKeyCode.SCROLL
- Similar to CAPS implementation (toggle functionality)
- Low priority

### 5. BREAK and PAUSE as Standalone Commands

**Description:** Presses the BREAK/PAUSE keys independently
**Current Status:** Partially implemented (works with CTRL modifier only)
**Implementation Notes:**

- Currently only works as CTRL BREAK or CTRL PAUSE
- Need standalone implementations
- Should use VirtualKeyCode.CANCEL and VirtualKeyCode.PAUSE
- Low priority

### 6. ALTCHAR and ALTSTRING

**Description:** Types characters/strings using ALT + numpad
**Current Status:** Not implemented
**Implementation Notes:**

- Requires implementing ALT + numpad key combinations
- Need to handle character to numpad code conversion
- ALTCHAR for single characters, ALTSTRING for entire strings
- Consider performance optimization for long strings
- Medium priority for special character support

### 7. Platform-Specific Commands

**Description:** COMMAND, OPTION (Mac), COMMANDSTRING
**Current Status:** Not implemented
**Implementation Notes:**

- COMMAND should map to same as GUI (VirtualKeyCode.LWIN)
- OPTION should map to same as ALT (VirtualKeyCode.LMENU)
- Consider platform-specific handling
- Low priority (Windows-focused toolkit)

### 8. REPEAT Command

**Description:** Similar to REPLAY but with different syntax
**Current Status:** Not implemented (REPLAY exists instead)
**Implementation Notes:**

- Consider if REPEAT is needed alongside REPLAY
- Could be implemented as alias to REPLAY
- Very low priority

### 9. Variable Support in Commands

**Description:** Support for $ variables in STRING, STRINGLN, DELAY commands
**Current Status:** Variables can be defined but not yet used in commands
**Implementation Notes:**

- VAR command is implemented for defining variables
- Need to add variable substitution similar to constant substitution
- Should work in STRING, STRINGLN, DELAY commands
- Need to implement GetVariableValue usage in SubstituteConstants method
- High priority for completing variable system

### 10. Conditional Statements (IF/ENDIF/ELSE)

**Description:** Conditional execution based on variable values
**Current Status:** Not implemented
**Implementation Notes:**

- Would require significant changes to parsing logic
- Need to implement conditional parsing and execution
- Should support boolean variable evaluation
- Complex implementation, requires control flow handling
- Medium priority for advanced scripting

### 11. WHILE/FOR Loops

**Description:** Loop constructs for repeated execution
**Current Status:** Not implemented
**Implementation Notes:**

- Would require major parsing changes
- Need loop state management
- Complex implementation
- Low priority

## Implementation Priority

1. **Variable substitution in commands (High)** - Complete the existing variable system
2. **Function Keys F1-F12 (High)** - Most commonly used missing commands
3. **Standalone ESCAPE (High)** - Common single key command
4. **Navigation Keys (Medium)** - HOME, END, INSERT, PAGEUP, PAGEDOWN standalone
5. **Conditional statements IF/ENDIF (Medium)** - Advance scripting capability
6. **ALTCHAR/ALTSTRING (Medium)** - Special character support
7. **Lock Keys (Low)** - NUMLOCK, SCROLLLOCK
8. **Standalone BREAK/PAUSE (Low)** - Less commonly used
9. **Platform-specific commands (Low)** - Mac compatibility
10. **Advanced control flow (Low)** - WHILE/FOR loops

## Implementation Details

### Adding Variable Substitution

To complete the variable system, modify the `SubstituteConstants` method in DuckyScriptProcessing.cs to also handle variables:

```csharp
private string SubstituteVariables(string input)
{
    if (string.IsNullOrEmpty(input)) return input;
    
    // Match variables in the format $VARIABLE
    var matches = Regex.Matches(input, @"\$([A-Za-z0-9_]+)");
    string result = input;
    
    foreach (Match match in matches)
    {
        string varName = match.Groups[1].Value;
        if (variables.ContainsKey(varName))
        {
            result = result.Replace(match.Value, variables[varName].ToString());
        }
    }
    
    return result;
}
```

Then call this method in addition to `SubstituteConstants` in STRING, STRINGLN, and DELAY commands.

### Adding Function Keys

Add cases for F1-F12 in the KeyboardAction method:

```csharp
case "F1":
    CheckDefaultSleep();
    InputSimulator.SimulateKeyPress(VirtualKeyCode.F1);
    break;
// ... continue for F2-F12
```

## Notes

- All implementations should be added to DuckyScriptProcessing.cs
- Each new command should be added to the Validation.cs for proper validation
- Consider adding unit tests for new commands
- Documentation should be updated when new commands are implemented
- The validation system is comprehensive and should be extended for new commands
- Current constant and variable systems provide a good foundation for advanced features
