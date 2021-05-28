# Jump King Save Editor

Web save editor for Jump King written in C# and compiled to WebAssembly with mono-wasm.

You can find live version here:  
(Size of the app is 15 mb so it can be loading for several seconds):  
https://mike-7.github.io/index.html

# How to use?

  - Select your save file (gamefolder/Content/Saves/combined.sav)
  - Modify data
  - Save it and replace the original save file

# Building

  - Get Mono (https://www.mono-project.com/download/stable/)
  - Get mono-wasm (https://jenkins.mono-project.com/job/test-mono-mainline-wasm/label=ubuntu-1804-amd64/lastSuccessfulBuild/Azure/)
  - Set MONOWASM variable in Makefile to your mono-wasm location
  - Make
