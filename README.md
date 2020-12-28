# Xpm - Getting started:
## Prerequisites:
 - A PC with Windows installed.
 - Git installed
 - A working internet connection (duh)
 - Ensure you have windows 10 sdk installed, and the `Framework` directory in your path
## Installation:
 - Open a git bash terminal and clone the repository: `git clone https://github.com/xsucculentx/xpm`
 - Navigate to the folder using a GUI or the terminal `cd ./xpm/`
 - Type `build.bat` into the terminal
 - Copy `./xpm/bin/Debug/xpm.exe` to where ever you want to install the program. Please avoid administrator blocked directories.
 - Navigate to the directory `xpm.exe` is in with the terminal.
 - Type `xpm -Ce` to create the environment variable, and restart your shell (open another cmd)
## Usage:
Ensure you have Xpm installed properly, then continue.
 - Installing a package: `xpm -S packagename`
 - Update all mirrors: `xpm -Sy`
 - Removing a package: `xpm -R packagename`
 - Update all packages: `xpm -Syu` ~~still working on this one~~
 - List installed packages: `xpm -Q`
 - Search and list installable packages: `xpm -Ss` or `xpm -Ss packagename`
 - Create and remove path variables: `xpm -Ce` or `xpm -Re`

## Tips and maintenance: 
**WIP Page!!!**
