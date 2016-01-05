# mono-keyboarder

# The Idea
  
  Save time working with keyboard + mouse by eliminating the need to move your hand from the mouse to the keyboard.
  I know it sounds like not much but there are a couple of people out there who might benefit from it.
  
# Requirements

  A keyboard and a mouse (doh :)) with extended buttons preferrably (those two by your thumb).
  
# Limitations

  * Windows only at the moment.
  * Due to the nature of the application working as a hook, other hooks installed might interfere with
    the application if ran after initiating "mono-keyboarder"
  * Due to no graphic or other external input currently the mapping is hardcoded in the code
  * Exit is done only by killing the process
  
# Installation

  Not released yet so the best install is just clone and compile with VS13 and up on windows.

# Usage

  Follow the example provided in Main.cs. In any case it goes like this:
  * Create new instance of Hook
  * Register hotkeys with RegisterHotKey(HotKeyCode.LCONTROL)
  * Register mapping with MapKey(KeyCode.KEY_Q, KeyCode.KEY_Y)
  
  Thats it. Don't forget to kill the process once you are done to restore normal functionality.
  
# License

  The license is included as LICENSE in this directory
