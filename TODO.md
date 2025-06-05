# DuckyScript Commands TODO List

This document lists all DuckyScript commands from the [Hak5 DuckyScript Quick Reference](https://docs.hak5.org/hak5-usb-rubber-ducky/duckyscript-tm-quick-reference) that are not currently implemented in The Rubber Ducky Ninja. Each command includes implementation notes and considerations.

## Currently Implemented Commands

The following commands are already implemented in the codebase:

- STRING
- ENTER
- DELAY
- DEFAULT_DELAY/DEFAULTDELAY
- GUI/WINDOWS
- ALT
- CONTROL/CTRL
- SHIFT
- TAB
- UP/UPARROW
- DOWN/DOWNARROW
- LEFT/LEFTARROW
- RIGHT/RIGHTARROW
- DELETE
- SPACE
- PRINTSCREEN
- CAPS
- REPLAY
- REM

## Commands To Implement

### 1. STRINGLN

**Description:** Types a string and presses ENTER
**Current Status:** Not implemented
**Implementation Notes:**

- Currently implemented as separate STRING + ENTER commands
- Should be implemented as a single command for better compatibility
- Implementation should be in DuckyScriptProcessing.cs in the KeyboardAction method
- Should handle the same string processing as STRING command but automatically append ENTER

### 2. DEFINE

**Description:** Defines a variable for use in the script
**Current Status:** Not implemented
**Implementation Notes:**

- Requires adding a variable storage system
- Need to implement variable substitution in STRING commands
- Should be processed before script execution
- Consider adding a Dictionary<string, string> to store variables
- Implementation should handle both DEFINE and variable usage (e.g., #VARIABLE)

### 3. REPEAT

**Description:** Repeats the last command a specified number of times
**Current Status:** Not implemented
**Implementation Notes:**

- Similar to REPLAY but with different syntax
- Should store last command and number of repeats
- Implementation should be in DuckyScriptProcessing.cs
- Consider merging with existing REPLAY functionality

### 4. ALTCHAR

**Description:** Types a character using ALT + numpad
**Current Status:** Not implemented
**Implementation Notes:**

- Requires implementing ALT + numpad key combinations
- Need to handle character to numpad code conversion
- Should be implemented in DuckyScriptProcessing.cs
- Consider adding a character to numpad code mapping

### 5. ALTSTRING

**Description:** Types a string using ALT + numpad
**Current Status:** Not implemented
**Implementation Notes:**

- Similar to ALTCHAR but for entire strings
- Should use ALTCHAR implementation
- Consider performance optimization for long strings
- Implementation should be in DuckyScriptProcessing.cs

### 6. ESCAPE

**Description:** Presses the ESC key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.ESCAPE
- Similar to other single key commands

### 7. HOME

**Description:** Presses the HOME key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.HOME
- Similar to other single key commands

### 8. END

**Description:** Presses the END key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.END
- Similar to other single key commands

### 9. INSERT

**Description:** Presses the INSERT key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.INSERT
- Similar to other single key commands

### 10. NUMLOCK

**Description:** Toggles the NUMLOCK key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.NUMLOCK
- Similar to CAPS implementation

### 11. SCROLLLOCK

**Description:** Toggles the SCROLLLOCK key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.SCROLL
- Similar to CAPS implementation

### 12. BREAK

**Description:** Presses the BREAK key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.CANCEL
- Similar to other single key commands

### 13. PAUSE

**Description:** Presses the PAUSE key
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.PAUSE
- Similar to other single key commands

### 14. F1-F12

**Description:** Presses function keys F1 through F12
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.F1 through VirtualKeyCode.F12
- Similar to other single key commands

### 15. APP

**Description:** Presses the APP key (context menu)
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.APPS
- Similar to other single key commands

### 16. MENU

**Description:** Presses the MENU key (context menu)
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.APPS
- Similar to APP command

### 17. WINDOWS

**Description:** Presses the WINDOWS key
**Current Status:** Not implemented (GUI is implemented instead)
**Implementation Notes:**

- Should be implemented as an alias for GUI
- Consider updating documentation to reflect GUI as primary command

### 18. COMMAND

**Description:** Presses the COMMAND key (Mac)
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.LWIN (same as GUI)
- Consider platform-specific handling

### 19. OPTION

**Description:** Presses the OPTION key (Mac)
**Current Status:** Not implemented
**Implementation Notes:**

- Simple implementation in KeyboardAction method
- Should use VirtualKeyCode.LMENU (same as ALT)
- Consider platform-specific handling

### 20. COMMANDSTRING

**Description:** Types a string using COMMAND + numpad
**Current Status:** Not implemented
**Implementation Notes:**

- Similar to ALTSTRING but for COMMAND key
- Should use COMMAND implementation
- Consider platform-specific handling

## Implementation Priority

1. STRINGLN (High) - Most commonly used command
2. DEFINE (High) - Essential for variable support
3. Function Keys (F1-F12) (Medium) - Common in scripts
4. Navigation Keys (HOME, END, etc.) (Medium) - Common in scripts
5. Special Keys (NUMLOCK, SCROLLLOCK, etc.) (Low) - Less commonly used
6. Platform-specific Commands (COMMAND, OPTION) (Low) - Platform dependent

## Notes

- All implementations should be added to DuckyScriptProcessing.cs
- Each new command should be added to the Validation.cs for proper validation
- Consider adding unit tests for new commands
- Documentation should be updated when new commands are implemented
- Consider adding examples in the README.md for new commands
