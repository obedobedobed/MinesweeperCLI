namespace MinesweeperCLI;

class Program
{
    static int[,] ?mapReal = null;
    static char[,] ?mapVisual = null;
    static Difficult ?difficult = null;
    static int currentButtonID = 1;

    static string binPath = AppContext.BaseDirectory;

    static int flagsCount = 0;
    static int minesCount = 0;

    static bool debug = false;

    static (int left, int top) cursorMapPos;

    static ConsoleColor accentColor = ConsoleColor.Green;
    static ConsoleColor defaultColor = Console.ForegroundColor;

    static void Main(string[] args)
    {
        // Checking for debug mode enabled
        if (args.Contains("DebugMode")) debug = true;

        // Setting terminal for game
        Console.Title = "Minesweeper CLI";
        Console.CursorVisible = false;
        Console.Clear();

        UIType currentUI = UIType.MainMenu;

        bool firstStep = true;
        bool gameEnded = false;
        bool playerWon = false;
        bool enterIsToMenu = false;

        ConsoleKeyInfo userInput;

        (int left, int top) nextCurPos = (1, 4);

        // Main game cycle
        while (true)
        {
            // Checking for UI to draw
            switch (currentUI)
            {
                case UIType.MainMenu:
                    DrawLogo();
                    Console.WriteLine();
                    DrawUI(UIType.MainMenu);

                    switch (currentButtonID)
                    {
                        case 1:
                            Console.SetCursorPosition(25, 9);
                            break;
                        case 2:
                            Console.SetCursorPosition(35, 9);
                            break;
                    }

                    userInput = Console.ReadKey(true);

                    switch (userInput.Key)
                    {
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                            switch (currentButtonID)
                            {
                                case 1:
                                    currentButtonID = 2;
                                    Console.SetCursorPosition(35, 9);
                                    break;
                                case 2:
                                    currentButtonID = 1;
                                    Console.SetCursorPosition(25, 9);
                                    break;
                            }
                            break;
                        case ConsoleKey.Enter:
                            switch (currentButtonID)
                            {
                                case 1:
                                    Console.Clear();
                                    currentUI = UIType.ModeChoosing;
                                    currentButtonID = 1;
                                    break;
                                case 2:
                                    // Quiting
                                    Console.Clear();
                                    Console.CursorVisible = true;
                                    Console.WriteLine("Thanks for playing!\nMinesweeper CLI by Obed");
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }

                    Console.Clear();
                    break;
                case UIType.ModeChoosing:
                    DrawLogo();
                    Console.WriteLine();
                    DrawUI(UIType.ModeChoosing);

                    Console.WriteLine("\nPress ESC to go back");

                    switch (currentButtonID)
                    {
                        case 1:
                            Console.SetCursorPosition(19, 9);
                            break;
                        case 2:
                            Console.SetCursorPosition(30, 9);
                            break;
                        case 3:
                            Console.SetCursorPosition(41, 9);
                            break;
                    }


                    userInput = Console.ReadKey(true);

                    switch (userInput.Key)
                    {
                        case ConsoleKey.RightArrow:
                            switch (currentButtonID)
                            {
                                case 1:
                                    currentButtonID = 2;
                                    Console.SetCursorPosition(30, 9);
                                    break;
                                case 2:
                                    currentButtonID = 3;
                                    Console.SetCursorPosition(41, 9);
                                    break;
                                case 3:
                                    currentButtonID = 1;
                                    Console.SetCursorPosition(19, 9);
                                    break;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            switch (currentButtonID)
                            {
                                case 1:
                                    currentButtonID = 3;
                                    Console.SetCursorPosition(41, 9);
                                    break;
                                case 2:
                                    currentButtonID = 1;
                                    Console.SetCursorPosition(19, 9);
                                    break;
                                case 3:
                                    currentButtonID = 2;
                                    Console.SetCursorPosition(30, 9);
                                    break;
                            }
                            break;
                        case ConsoleKey.Enter:
                            switch (currentButtonID)
                            {
                                case 1:
                                    difficult = Difficult.Easy;
                                    break;
                                case 2:
                                    difficult = Difficult.Normal;
                                    break;
                                case 3:
                                    difficult = Difficult.Hard;
                                    break;
                            }
                            currentUI = UIType.InGame;
                            break;
                        case ConsoleKey.Escape:
                            // Quiting to main menu
                            Console.Clear();
                            currentUI = UIType.MainMenu;
                            currentButtonID = 1;
                            break;
                    }

                    Console.Clear();
                    break;
                case UIType.InGame:
                    // Setting game
                    if (firstStep)
                    {
                        switch (difficult)
                        {
                            case Difficult.Easy:
                                mapReal = new int[6, 6];
                                minesCount = 6;
                                break;
                            case Difficult.Normal:
                                mapReal = new int[8, 8];
                                minesCount = 12;
                                break;
                            case Difficult.Hard:
                                mapReal = new int[10, 10];
                                minesCount = 18;
                                break;
                        }

                        RandomizeMines();
                        firstStep = false;

                        cursorMapPos = (0, 0);
                    }

                    DrawUI(UIType.InGame);
                    DrawMap();

                    // Drawing tips
                    if (!gameEnded)
                    {
                        Console.WriteLine("\nPress Enter to open a cell");
                        Console.WriteLine("Press Backspace to mark mine");
                        Console.WriteLine("Press ESC to go back");
                    }
                    else if (gameEnded && !playerWon)
                    {
                        Console.WriteLine("\nYou lose\nPress Enter to continue");
                    }
                    else if (gameEnded && playerWon)
                    {
                        Console.WriteLine("\nYou won\nPress Enter to continue");
                    }

                    Console.SetCursorPosition(nextCurPos.left, nextCurPos.top);

                    userInput = Console.ReadKey(true);

                    if (!gameEnded)
                    {
                        switch (userInput.Key)
                        {
                            case ConsoleKey.RightArrow:
                                if (cursorMapPos.left != mapReal.GetLength(1) - 1)
                                {
                                    nextCurPos = (Console.CursorLeft + 3, Console.CursorTop);
                                    cursorMapPos = (cursorMapPos.left + 1, cursorMapPos.top);
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (cursorMapPos.left != 0)
                                {
                                    nextCurPos = (Console.CursorLeft - 3, Console.CursorTop);
                                    cursorMapPos = (cursorMapPos.left - 1, cursorMapPos.top);
                                }
                                break;
                            case ConsoleKey.UpArrow:
                                if (cursorMapPos.top != 0)
                                {
                                    nextCurPos = (Console.CursorLeft, Console.CursorTop - 1);
                                    cursorMapPos = (cursorMapPos.left, cursorMapPos.top - 1);
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (cursorMapPos.top != mapReal.GetLength(0) - 1)
                                {
                                    nextCurPos = (Console.CursorLeft, Console.CursorTop + 1);
                                    cursorMapPos = (cursorMapPos.left, cursorMapPos.top + 1);
                                }
                                break;
                            case ConsoleKey.Backspace:
                                if (mapVisual[cursorMapPos.top, cursorMapPos.left] == '░')
                                {
                                    mapVisual[cursorMapPos.top, cursorMapPos.left] = 'F';
                                    flagsCount++;
                                }

                                if (CheckForWin())
                                {
                                    gameEnded = true;
                                    playerWon = true;
                                }
                                break;
                            case ConsoleKey.Enter:
                                if (mapReal[cursorMapPos.top, cursorMapPos.left] == 0)
                                {
                                    if (mapVisual[cursorMapPos.top, cursorMapPos.left] == 'F')
                                    {
                                        mapVisual[cursorMapPos.top, cursorMapPos.left] = '░';
                                        flagsCount--;
                                    }
                                    else
                                    {
                                        int y = cursorMapPos.left;
                                        int x = cursorMapPos.top;

                                        if (mapReal[x, y] == 0)
                                        {
                                            int minesAround = AnalyzeMinesAround(cursorMapPos);

                                            if (minesAround == 0) mapVisual[x, y] = ' ';
                                            else if (minesAround != 0) mapVisual[x, y] = Convert.ToString(minesAround)[0];
                                        }

                                        if (CheckForWin())
                                        {
                                            gameEnded = true;
                                            playerWon = true;
                                        }
                                    }
                                }
                                else if (mapReal[cursorMapPos.top, cursorMapPos.left] == 1)
                                {
                                    if (mapVisual[cursorMapPos.top, cursorMapPos.left] == 'F')
                                    {
                                        mapVisual[cursorMapPos.top, cursorMapPos.left] = '░';
                                        flagsCount--;
                                    }
                                    else
                                    {
                                        accentColor = ConsoleColor.Red;
                                        mapVisual[cursorMapPos.top, cursorMapPos.left] = 'B';

                                        gameEnded = true;

                                        Console.Clear();
                                        continue;
                                    }
                                }
                                break;
                            case ConsoleKey.Escape:
                                Console.Clear();
                                currentUI = UIType.MainMenu;
                                currentButtonID = 1;
                                mapReal = null;
                                mapVisual = null;
                                difficult = null;
                                firstStep = true;
                                nextCurPos = (1, 4);
                                break;
                        }
                    }
                    else if (gameEnded)
                    {
                        switch (userInput.Key)
                        {
                            case ConsoleKey.Enter:
                                if (!enterIsToMenu)
                                {
                                    for (int y = 0; y < mapVisual.GetLength(0); y++)
                                    {
                                        for (int x = 0; x < mapVisual.GetLength(1); x++)
                                        {
                                            if (mapReal[y, x] == 0) mapVisual[y, x] = ' ';
                                            else if (mapReal[y, x] == 1) mapVisual[y, x] = 'B';
                                        }
                                    }
                                    enterIsToMenu = true;
                                }
                                else if (enterIsToMenu)
                                {
                                    // Quiting
                                    accentColor = ConsoleColor.Green;
                                    enterIsToMenu = false;
                                    gameEnded = false;
                                    Console.Clear();
                                    currentUI = UIType.MainMenu;
                                    currentButtonID = 1;
                                    mapReal = null;
                                    mapVisual = null;
                                    difficult = null;
                                    firstStep = true;
                                    nextCurPos = (1, 4);
                                }
                                break;
                        }
                    }

                    Console.Clear();
                    break;
            }
        }
    }

    static void DrawLogo()
    {
        // Checking for file existing
        if (!File.Exists($"{binPath}/Logo"))
        {
            Console.Clear();
            Console.WriteLine("Minesweeper CLI error:\nFile \"Logo\" doesnt exists...");
            Console.CursorVisible = true;
            Environment.Exit(-1);
        }

        string[] logo = File.ReadAllLines($"{binPath}/Logo");

        // Drawing all lines from file
        foreach (string str in logo)
        {
            Console.WriteLine(str);
        }
    }

    static void DrawUI(UIType uiType)
    {
        // Checking for file existing
        if (!File.Exists($"{binPath}/UI"))
        {
            Console.Clear();
            Console.WriteLine("Minesweeper CLI error:\nFile \"UI\" doesnt exists...");
            Console.CursorVisible = true;
            Environment.Exit(-1);
        }

        string[] ui = File.ReadAllLines($"{binPath}/UI");
        int itemIndex = 0;

        // Drawing lines from file
        foreach (string str in ui)
        {
            bool quitCycle = false;

            switch (uiType)
            {
                case UIType.MainMenu:
                    if (itemIndex <= 1)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (i > 22 && i < 29 && currentButtonID == 1)
                            {
                                Console.ForegroundColor = accentColor;
                            }
                            else if (i > 32 && i < 38 && currentButtonID == 2)
                            {
                                Console.ForegroundColor = accentColor;
                            }

                            Console.Write(str[i]);

                            Console.ForegroundColor = defaultColor;
                        }

                        Console.WriteLine();
                    }
                    if (itemIndex == 1)
                    {
                        quitCycle = true;
                    }
                    break;
                case UIType.ModeChoosing:
                    if (itemIndex >= 2 && itemIndex <= 4)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            if (i > 17 && i < 22 && currentButtonID == 1)
                            {
                                Console.ForegroundColor = accentColor;
                            }
                            else if (i > 27 && i < 34 && currentButtonID == 2)
                            {
                                Console.ForegroundColor = accentColor;
                            }
                            else if (i > 39 && i < 44 && currentButtonID == 3)
                            {
                                Console.ForegroundColor = accentColor;
                            }

                            Console.Write(str[i]);

                            Console.ForegroundColor = defaultColor;
                        }

                        Console.WriteLine();
                    }
                    if (itemIndex == 4)
                    {
                        quitCycle = true;
                    }
                    break;
            }

            if (quitCycle)
            {
                break;
            }

            itemIndex++;
        }
        
        // Top panel for in-game UI
        if (uiType == UIType.InGame)
        {
            Console.WriteLine("Minesweeper CLI");
            Console.WriteLine($"Difficult: {difficult}");

            int minesCount = 0;

            switch (difficult)
            {
                case Difficult.Easy:
                    minesCount = 6;
                    break;
                case Difficult.Normal:
                    minesCount = 12;
                    break;
                case Difficult.Hard:
                    minesCount = 18;
                    break;
            }

            Console.WriteLine($"Mines: {minesCount}\n");
        }
    }

    static void RandomizeMines()
    {
        bool isMine2PointsAgo = false;
        int poinsToResetBool = 0;

        while (minesCount > 0)
        {
            for (int y = 0; y < mapReal.GetLength(0); y++)
            {
                for (int x = 0; x < mapReal.GetLength(1); x++)
                {
                    // Chance to generate mine: 8%
                    if (Random.Shared.Next(0, 101) < 8)
                    {
                        if (minesCount > 0)
                        {
                            if (!isMine2PointsAgo)
                            {
                                // Writing mine to map
                                mapReal[y, x] = 1;
                                isMine2PointsAgo = true;
                                poinsToResetBool = 2;
                                minesCount--;
                            }
                            else
                            {
                                poinsToResetBool--;

                                if (poinsToResetBool == 0)
                                {
                                    isMine2PointsAgo = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    static void DrawMap()
    {
        // Creating visual map array if doesnt exists
        if (mapVisual == null)
        {
            mapVisual = new char[mapReal.GetLength(0), mapReal.GetLength(1)];

            for (int y = 0; y < mapVisual.GetLength(0); y++)
            {
                for (int x = 0; x < mapVisual.GetLength(1); x++)
                {
                    mapVisual[y, x] = '░';
                }
            }
        }

        for (int y = 0; y < mapVisual.GetLength(0); y++)
        {
            for (int x = 0; x < mapVisual.GetLength(1); x++)
            {
                // Changing color to accent if point choosed
                if (y == cursorMapPos.top && x == cursorMapPos.left)
                {
                    Console.ForegroundColor = accentColor;
                }

                Console.Write($"[{mapVisual[y, x]}]");

                // Changing color to default
                Console.ForegroundColor = defaultColor;
            }
            Console.WriteLine();
        }

        // If debug mode draw real map
        if (debug)
        {
            Console.WriteLine();

            for (int y = 0; y < mapVisual.GetLength(0); y++)
            {
                for (int x = 0; x < mapVisual.GetLength(1); x++)
                {
                    if (y == cursorMapPos.top && x == cursorMapPos.left)
                    {
                        Console.ForegroundColor = accentColor;
                    }

                    Console.Write($"[{mapReal[y, x]}]");

                    Console.ForegroundColor = defaultColor;
                }
                Console.WriteLine();
            }
        }
    }

    static int AnalyzeMinesAround((int left, int top) pos)
    {
        int minesFounded = 0;

        // Array of directions
        (int left, int top)[] dirs =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1,  0),          (1,  0),
            (-1,  1), (0,  1), (1,  1)
        };

        foreach ((int left, int top) dir in dirs)
        {
            // Calculating point to check
            (int left, int top) checkingPoint = (pos.left + dir.left, pos.top + dir.top);

            // Checking mine on point
            if (checkingPoint.left >= 0 && checkingPoint.top >= 0 &&
            checkingPoint.left < mapReal.GetLength(1) && checkingPoint.top < mapReal.GetLength(0))
            {
                if (mapReal[checkingPoint.top, checkingPoint.left] == 1)
                {
                    minesFounded++;
                }
            }

        }

        return minesFounded;
    }

    static bool CheckForWin()
    {
        for (int y = 0; y < mapVisual.GetLength(0); y++)
        {
            for (int x = 0; x < mapVisual.GetLength(1); x++)
            {
                // Checking to closed points and is it mines
                if (mapVisual[y, x] == '░' && mapReal[y, x] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    enum UIType
    {
        MainMenu, ModeChoosing, InGame
    }

    enum Difficult
    {
        Easy, Normal, Hard
    }
}