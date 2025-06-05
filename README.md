# The Rubber Ducky Ninja

A toolkit for the USB Rubber ducky, designed to simplify testing, validating, and encoding DuckyScript payloads.

![The Rubber Ducky Ninja](https://i.imgur.com/5HF0how.png)

## Video

You can find videos demonstrating this project on my YouTube channel [here.](https://www.youtube.com/@PetesGonzalezCybernetics)

## About

The Rubber Ducky Ninja is a powerful toolkit that makes working with DuckyScripts easier and more efficient. It allows you to test your code by emulating what the ducky would do, validate your scripts before running them, and easily encode your scripts into binary format with just one click.

## In-Depth Overview

### Project Structure

- **The Rubber Ducky Ninja.csproj** – The project file (Visual Studio) for the toolkit.
- **Program.cs** – The entry point (main) of the application.
- **formMain.cs (and .Designer.cs)** – The main form (GUI) of the toolkit. It lets you load a DuckyScript (txt) file, debug (validate) your script, and execute (emulate) the payload.
- **formEncoding.cs (and .Designer.cs)** – A form that (if duckencode.jar is present) lets you encode your DuckyScript (txt) into a binary (bin) file (using duckencode.jar) so that you can upload it to your USB Rubber Ducky.
- **DuckyScriptProcessing.cs** – The "engine" that reads your DuckyScript (txt) file, parses each line (using a "Calculate" method), and then "emulates" (simulates) the ducky's keystrokes (using InputSimulator) on your PC. It supports a subset of DuckyScript commands (see "Supported Commands" below).
- **Validation.cs** – A helper class that validates (checks) your DuckyScript (txt) for "compile" errors (for example, unsupported commands or syntax errors) so that you can debug your payload before running it.
- **AboutBox.cs (and .Designer.cs)** – A simple "About" dialog (with a logo) for the toolkit.
- **App.config** – The application's configuration (for example, binding redirects).
- **app.manifest** – The manifest (for example, requesting admin privileges) for the application.
- **Properties/** – Contains (among other things) the assembly info (version, copyright, etc.) and Resources (for example, the logo).
- **bin/** – The output folder (for example, bin/x64/Debug) where the compiled executable (The Rubber Ducky Ninja.exe) and (if you run the encoder) your encoded (bin) payloads are stored.

### Supported Commands

The toolkit (via DuckyScriptProcessing.cs) supports a subset of DuckyScript commands. Below is a list of supported commands (and a brief description):

- **REM** – A comment (remark) that is ignored by the compiler. Can be used to add comments or vertical spacing in your script. Blank lines are also ignored.
- **STRING** – Types the given text (without an ENTER). Constants can be used with #CONSTANT syntax.
- **STRINGLN** – Types the given text and presses ENTER. Constants can be used with #CONSTANT syntax.
- **ENTER** – Simulates pressing the ENTER key.
- **DELAY** – Pauses (sleeps) for the given number of milliseconds.
- **DEFAULT_DELAY (or DEFAULTDELAY)** – Sets a "global" delay (in milliseconds) between each command (if enabled).
- **GUI (or WINDOWS)** – Simulates pressing the Windows key (or a modified key stroke, for example, WIN + R). WINDOWS is an alias for GUI.
- **ALT** – Simulates pressing the ALT key (or a modified key stroke, for example, ALT + F4).
- **CONTROL (or CTRL)** – Simulates pressing the CONTROL key (or a modified key stroke, for example, CTRL + C).
- **SHIFT** – Simulates pressing the SHIFT key (or a modified key stroke, for example, SHIFT + DELETE).
- **TAB** – Simulates pressing the TAB key.
- **UP (or UPARROW), DOWN (or DOWNARROW), LEFT (or LEFTARROW), RIGHT (or RIGHTARROW)** – Simulates pressing the arrow keys.
- **DELETE** – Simulates pressing the DELETE key.
- **SPACE** – Simulates pressing the SPACE key.
- **PRINTSCREEN** – Simulates pressing the PRINT SCREEN key.
- **CAPS** – Toggles "caps lock" (so that subsequent STRING commands are typed in uppercase).
- **REPLAY** – Replays (repeats) the last command (except "REM" or "REPLAY") a given number of times.
- **DEFINE** – Defines a variable for use in the script. Variables can be used in STRING and STRINGLN commands using #VARIABLE syntax.

### Constants and Variables

The toolkit supports both constants (DEFINE) and variables (VAR) commands. They serve different purposes and have different syntax:

#### Constants (DEFINE)

Constants are defined using the DEFINE command and are used for find-and-replace at compile time. They are prefixed with # and can contain any string value.

1. Define a constant:

```
DEFINE #WAIT 2000
DEFINE #TEXT Hello World
DEFINE #MYURL example.com
```

2. Use the constant in commands:

```
DELAY #WAIT
STRINGLN #TEXT
STRING https://#MYURL
```

Constants must:

- Be defined with a # prefix (e.g., #VARIABLE)
- Contain only letters, numbers, and underscores (after the # prefix)
- Be defined before they are used
- Can be used in STRING, STRINGLN, and DELAY commands

#### Variables (VAR)

Variables are defined using the VAR command and can hold unsigned integers (0-65535) or boolean values. They are prefixed with $ and can be modified during script execution.

1. Define a variable:

```
VAR $BLINK = TRUE
VAR $BLINK_TIME = 1000
```

2. Use the variable in commands:

```
IF $BLINK
    DELAY $BLINK_TIME
    STRINGLN Blinking...
ENDIF
```

Variables must:

- Be defined with a $ prefix (e.g., $VARIABLE)
- Contain only letters, numbers, and underscores (after the $ prefix)
- Hold unsigned integers (0-65535) or boolean values (TRUE/FALSE)
- Be defined before they are used
- Can be used in IF statements and other commands that support variables

Example script using both constants and variables:

```
DEFINE #WAIT 2000
DEFINE #TEXT Hello World
DEFINE #MYURL example.com

VAR $BLINK = TRUE
VAR $BLINK_TIME = 1000

DELAY #WAIT
STRINGLN #TEXT

IF $BLINK
    DELAY $BLINK_TIME
    STRING https://#MYURL
ENDIF
```

This will:

1. Define three constants: #WAIT (2000), #TEXT ("Hello World"), and #MYURL ("example.com")
2. Define two variables: $BLINK (TRUE) and $BLINK_TIME (1000)
3. Wait for 2000 milliseconds (using the #WAIT constant)
4. Type "Hello World" and press Enter (using the #TEXT constant)
5. If $BLINK is TRUE, wait for 1000ms and type "<https://example.com>"

### Payload Processing

- **Loading a Payload:** In the main form (formMain), you click "Open" (or "Find File") to select a DuckyScript (txt) file. (The toolkit copies that file ("script.txt") into the exe folder so that the encoder (if used) can read it.)
- **Debugging (Validating) a Payload:** Click "Debug" (or "Validate") to run Validation.cs (which checks for "compile" errors – for example, unsupported commands – in your payload). (If no errors are found, the "Execute" button is enabled.)
- **Executing (Emulating) a Payload:** Click "Execute" (or "Run") to "emulate" your payload. (The toolkit (via DuckyScriptProcessing.ReadFile) reads "script.txt" (or your chosen file) line by line, "Calculate"s (parses) each line, and then "KeyboardAction" (simulates) the ducky's keystrokes on your PC.) (Note that if UAC (User Account Control) is on, the toolkit cannot "type" (or "press" keys) while the UAC prompt is open – you may need to disable UAC (or "click" "Yes" yourself) for testing.)
- **Encoding a Payload (Optional):** If you have "duckencode.jar" (and Java) installed, you can click "En (or "Encode Form") to open the encoder form (formEncoding). There, you choose an output folder (and a name ending in ".bin") and then click "En (or "Encode to Bin") to "encode" (convert) your DuckyScript (txt) (i.e. "script.txt") into a binary (bin) file (using "java –jar duckencode.jar –i script.txt –o ...").

### Usage Instructions

1. **Open (or "Find File")** – Click "Open" (or "Find File") (or "btnPath_Click") to select a DuckyScript (txt) file (for example, "WIFI-EXFIL2.txt" or "WIFI-EXFIL.txt"). (The toolkit copies that file ("script.txt") into the exe folder.)
2. **Debug (or "Validate")** – Click "Debug" (or "Validate") (or "btnDebug_Click") to "validate" (check) your payload (using Validation.cs) for "compile" errors. (If no errors are found, the "Execute" button is enabled.)
3. **Execute (or "Run")** – Click "Execute" (or "Run") (or "btnExecuteButton_Click") to "emulate" (simulate) your payload (using DuckyScriptProcessing.ReadFile and "KeyboardAction") on your PC. (Note that if UAC is on, the toolkit cannot "type" (or "press" keys) while the UAC prompt is open – you may need to disable UAC (or "click" "Yes" yourself) for testing.)
4. **(Optional) Encode (or "Encode Form")** – If you have "duckencode.jar" (and Java) installed, click "En (or "Encode Form") (or "btnEncodeForm_Click") to open the encoder form (formEncoding). There, choose an output folder (and a name ending in ".bin") and then click "En (or "Encode to Bin") (or "btnEncode_Click") to "encode" (convert) your DuckyScript (txt) (i.e. "script.txt") into a binary (bin) file (using "java –jar duckencode.jar –i script.txt –o ...").

### Comments and Spacing

The toolkit supports two ways to add comments and spacing to your scripts:

1. Using the REM command:

```
REM This is a comment explaining what the next section does
REM You can use multiple REM lines for longer comments
REM ============================================
REM This is a section separator
```

2. Using blank lines:

```
REM This is a comment
STRING Hello World

REM This is another comment after a blank line
STRINGLN Goodbye World
```

Both REM commands and blank lines are ignored by the compiler and can be used to improve script readability.

## Contributing

If you're interested in contributing and have any questions, feel free to reach out to me at my GitHub profile: [@plgonzalezrx8](https://github.com/plgonzalezrx8)

[![forthebadge](http://forthebadge.com/images/badges/designed-in-ms-paint.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/powered-by-electricity.svg)](http://forthebadge.com)
