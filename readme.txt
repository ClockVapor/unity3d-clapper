Unity3D Clapper
Copyright (C) 2013 Nick Lowery (nick.a.lowery@gmail.com)
See license.txt for full license.

Unity3D Clapper is a simple program that listens in on your default microphone for claps, and plays music based on how many claps were heard in a group.

To pick which songs to play, you must have a file called "songs.txt" adjacent to the program's executable. This file should be formatted like so:

(number of claps) @ (sound file #1 to play);
(number of claps) @ (sound file #2 to play);

For example, if we wanted to play "Let's Get It On" by Marvin Gaye (letsgetiton.ogg adjacent to the executable) after two claps, our songs.txt file would look like this:

2 @ file://letsgetiton.ogg;

Note that the file path MUST have the protocol alongside it (file://, http://, etc.).