# Helping Hands
![Image of simulation](/gitMedia/helpingHands.png)

Collaboration with [Steffen543](https://github.com/Steffen543).
Project for a game programming lecture.

## Play
[Release binary](https://github.com/Wasserwecken/HelpingHands/releases)

#### Player 1:
| Action | Keyboard | Controller |
|-|-|-|
| Movement | WSAD | Left tick |
| Jump | Space | Button A |
| Shoot | Shift left | Button B |
| Helping | Ctrl left | Trigger Right |


#### Player 2
| Action | Keyboard | Controller |
|-|-|-|
| Movement | Arrow keys | Left tick |
| Jump | Ctrl right | Button A |
| Shoot | Shift right | Button B |
| Helping | Keypad 0 | Trigger Right |

## About
Co-op game which heavily inspired by the game `BattleBlocks Theatre`. The two players have to work together to manage to get through the level. Obstacles are broken robots which have to be aktivate with crystals first. To get to the required crystals, the players have to avoid or encounter various enemies and deal with the heights of the environment.

## Implementations
- Keyboard & Controller support
- Only OpenTK is used as dependency
- Level editor
- Rigidbody physics
- Quadtree and Grid implementation for collision

## Open the project
The correct solution for starting and diving into the game is located at: `\Athene\Spielwiese\`