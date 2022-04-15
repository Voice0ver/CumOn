# CumOn
A plugin that gives control over cumshots and other fluids in H-scenes in Honey Select 2 main game. Also removes the annoying crossfades that distract from the money shots!

# Notes
This is still a work in progress
- Cumshot pattern can be changed in the settings under the plugin settings on F1. Can also tweak cum vomit amount.
- Currently uses one cumshot pattern for all scenes. Final version will need presets since different positions suit different patterns
- There is limited time in the orgasm animation, so for longer cum sequences the guy will continue into the post-coital talk... Will look for a way to extend the orgasm animation to resolve this
- Some parts of the body are not registered as colliders in some scenes and the cum will go straight through. Will look to fix that too.
- Long range cum shots (over the body or from handjobs) look better because the game's fluid physics engine emits quite large particles, making short range shots (facials) rather... chunky
- No support for Studio at this time (it's quite a different task)
- No customisation of creampies or urine yet

# Performance
The fluid physics used by the game is not especially fast. Collisions with face and breasts are particularly time intensive. To increase speed:
- Reduce particle lifespan
- Slow the cum speed a little (faster moving fluid uses more particles)
- Reduce gravity to help when you slow the cum speed
- Reduce cum volume

# Installation
Requires BepInEx<br>
Copy the dll to your install directory /BepInEx/plugins

# Compiling the source
Add this Nuget source for Illusion Mods 
  https://pkgs.dev.azure.com/IllusionMods/Nuget/_packaging/IllusionMods/nuget/v3/index.json

You will also need to add a reference to Obi.dll, which can be found in the game folder at:
  HoneySelect2_Data\Managed
