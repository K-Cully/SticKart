# SticKart

SticKart is a 2D side-scrolling platformer which I created as my major final year college project.
It utilizes the Kinect sensor for input and the target platform is Windows 7.
All player actions control the actions of a stick man running through a mine. 
The player must run jump and crouch their way across platforms and past obstacles. 
They can also travel in a mine cart to give their legs a rest.
The objective of the game is to set the high score on each level and the player can create their own levels once the game is complete.

A video of the game features can be viewed by clicking the image below:

<a href="http://www.youtube.com/watch?feature=player_embedded&v=5-o6HtFZbb4
" target="_blank"><img src="http://img.youtube.com/vi/5-o6HtFZbb4/0.jpg" 
alt="SticKart Game Video" width="240" height="180" border="10" /></a>

Installer download:

<a href="http://www.keithcully.com/files/SticKart.zip">Right click and select save as.</a>

The main features of the game are:

1. Motion control using a gesture managment system which automatically recalibrates to suit each individual player's body dimensions.
2. A bespoke player tracking system which filters the active player from the recognised skeletons.
3. Voice control.
4. A fully fledged NUI level editor.
5. A bespoke NUI friendly menu system.
6. A thread safe notification manager 
7. Local and online leaderboards (Online leaderboards are no longer active as my Azure subscription ran out).

The project is built using the following technologies:

1. XNA
2. Farseer Physics
3. Kinect for Windows SDK v1.6
4. Microsoft Speech platform
5. Windows Communication Foundation
6. Windows Azure
7. IIS
8. SQL 

Notes:

1. Information on how I implemented some of the system can be found in the Documents\Journal.docx file.
2. The installer will install the EN-US, EN-UK and EN-IE kinect language packs but if you install any other EN-?? language pack the one appropriate to your system's region will be chosen automatically.
3. I have not copied any of the web service components to this repository (just in case you were looking for them).
5. The game has not been tested on a machine without a graphics card so I don't know how it will perform on such.
 
