## Battleship State Tracker

[![Build Status](https://amrreda.visualstudio.com/BattleshipStateTracker/_apis/build/status/AmrReda.BattleshipStateTracker?branchName=master)](https://amrreda.visualstudio.com/BattleshipStateTracker/_build/latest?definitionId=1&branchName=master)

### Background
Emulate the classic game "Battleship".
- Two players
- Each have a 10x10 board
- During setup, players can place an arbitrary number of “battleships” on their
board. The ships are 1-by-n sized, must fit entirely on the board, and must be
aligned either vertically or horizontally.
- During play, players take turn “attacking” a single position on the opponent’s
board, and the opponent must respond by either reporting a “hit” on one of
their battleships (if one occupies that position) or a “miss”
- A battleship is sunk if it has been hit on all the squares it occupies
- A player wins if all of their opponent’s battleships have been sunk. 

