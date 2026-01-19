# D&D Initiative Tracker
A C# command console app for tracking initiative order in Dungeons and Dragons. I built it to more efficiently progress my combat encounters during campaigns.

## Features
- Generate initiative lists from user input
- Add/remove characters
- Track current turn
- Swap character positions
- Input validity checked with fun personality-driven messages

## How to Run
**Requirements:** .NET SDK

1. Clone this repository
2. Navigate to project folder (InitiativeTracker)
3. Run `dotnet run`

## Commands
- `generate` - Create new initiative list
- `list` or `l` - Display current order
- `add <name> <roll>` - Add character
- `remove <name>` - Remove character
- `next` or `n` - Progress to next turn
- `swap <name1> <name2>` - Swap positions
- `help` - View commands
- `exit` - Exit program
