# Minesweeper CLI
## By Obed

Small CLI project made with C# and .NET 8

If you want to build it yourself open folder with .csproj file and enter the command:

```bash
dotnet publish -c Release -r (platform (win-x64 for example) --self-contained (true/false) -p:PublishSingleFile=(true/false)
```

Example command:

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

**YOU MUST HAVE .NET 8 TO BUILD THE PROJECT!**

You can download built version in **releases**

**How to play:**
- In main menu by arrows choose button "Play"
- Next by arrows choose difficult
- Use arrows to move, enter to open a cell and backspace to mark the mine

**Difficulties:**
- Easy: map 6x6 + 6 mines
- Normal: map 8x8 + 12 mines
- Hard: map 10x10 + 18 mines

**Debug mode:**
Run the game with "DebugMode" argument to enter the debug mode
- You can see real map under the visual map
