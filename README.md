Space Warrior
=============
Space Warrior is a Kinect based action game geared towards visually impaired children,
but that can be played and enjoyed by typical children as well.

The game was created using Microsoft's XNA framework and it uses the Microsoft Kinect to
capture motion. The game can also be played with a keyboard.

The object of the game is to survive an ambush of alien robots. The enemies will appear 
once at a time, from either the left or right, and will make a sound (robot voice message)
that will be heard from either the left or right speaker, indicating the player the position
of the enemy. To destory the enemy, lift your hand over your head and bring it down, mimicking a 
sword slash. If you wait too long, the enemy will attack you and then leave the screen. An enemy attack
substract health points from the player, and the game is zero when the player reaches zero health points.

A special kind of enemy is the dragon. The dragon appears on the middle of the screen and a distinct
scream announces its arrival. The player cannot attack the dragon; the player can only put her arm in front
of her face, as if covering herself. If the gesture is done correctly, the dragon will dissapear.

** Important **

The code in the Gesture Recognition folder was copied from the book 'Kinect for Windows SDK Programming Guide.'
The classes RightSwordSlash, LeftSwordSlash, RightGuard, LeftGuard, and Push were written by using the gesture
recognition API provided by the author of aforementioned book.
