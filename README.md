# D&D Initiative Tracker
A C# command line application for tracking initiative order in Dungeons and Dragons. Features include turn tracking and robust input validation. Built to streamline combat encounters during my campaign.

## Features
- Generate initiative lists from user input
- Add/remove characters 
- Track current turn with automatic progression
- Swap character positions
- Input validity checked alongside personality-driven feedback messages

## How to Run (With VS Code)
**Requirements:** .NET SDK

1. Clone this repository
2. Navigate to project folder (InitiativeTracker)
3. Run 'dotnet run'

## Commands
- `generate` - Create new initiative list
- `list` or `l` - Display current order
- `add <name> <roll>` - Add character
- `remove <name>` - Remove character
- `next` or `n` - Progress to next turn
- `swap <name1> <name2>` - Swap positions
- `clear` - Clear console
- `help` - View commands
- `exit` - Exit program
