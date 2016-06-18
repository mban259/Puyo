using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

class Program {
    static bool BlockFlag = false;
    static int r;
    static int[] BlockIndexX = { 0, 2, 4, 6 };
    static int[] BlockIndexY = { 0, 2, 4, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30 };
    static int[,] CollisionField = new int[Height, Width];
    static int[] Color = { 0, 1, 3, 5, 7, 11 };
    const int Height = 12 + 2, Width = 6 + 2, BlockHeight = 2, BlockWidth = BlockHeight;
    static int Fall, Side;
    static int TurnPoint = 0;
    static int[,] Stage = new int[Height, Width];
    static int[,] Field = new int[Height, Width];
    static int[,] Block = new int[BlockHeight, BlockWidth];
    static int[,] Blocks = {
        {1,1,1,0,1,1,1,0},
        {0,0,1,0,0,0,1,0},

        {2,2,2,0,2,2,2,0},
        {0,0,2,0,0,0,2,0},

        {3,3,3,0,3,3,3,0},
        {0,0,3,0,0,0,3,0},

        {4,4,4,0,4,4,4,0},
        {0,0,4,0,0,0,4,0},

        {5,5,5,0,5,5,5,0},
        {0,0,5,0,0,0,5,0},

        {1,2,1,0,2,1,2,0},
        {0,0,2,0,0,0,1,0},

        {1,3,1,0,3,1,3,0},
        {0,0,3,0,0,0,1,0},

        {1,4,1,0,4,1,4,0},
        {0,0,4,0,0,0,1,0},

        {1,5,1,0,5,1,5,0},
        {0,0,5,0,0,0,1,0},

        {2,3,2,0,3,2,3,0},
        {0,0,3,0,0,0,2,0},

        {2,4,2,0,4,2,4,0},
        {0,0,4,0,0,0,2,0},

        {2,5,2,0,5,2,5,0},
        {0,0,5,0,0,0,2,0},

        {3,4,3,0,4,3,4,0},
        {0,0,4,0,0,0,3,0},

        {3,5,3,0,5,3,5,0},
        {0,0,5,0,0,0,3,0},

        {4,5,4,0,5,4,5,0},
        {0,0,5,0,0,0,4,0}
    };
    volatile static bool KeyReaded = false;
    static ConsoleKeyInfo Key;
    static Dictionary<int, ConsoleColor> Dic = new Dictionary<int, ConsoleColor>();
    static bool GameOverFlag = false;

    static void Main() {
        Start();
        InitVar();
        while (true) {
            if (!GameOverFlag) {
                MakeBlock();
                GameOver();
                GetKey();
                MakeField();
                InitField();
                // FallBlock();
                FreezeBlock();
                //FallBlock();
                DrawField();
                ClearField();
                Thread.Sleep(500);
                //KeyReaded = false;
            }
            else {
                Console.WriteLine("GameOver");
                Console.ReadKey(true);
                break;
            }
            Fall++;
        }

    }
    static void Start() {
        Dic.Add(1, ConsoleColor.DarkGreen);
        Dic.Add(3, ConsoleColor.Red);
        Dic.Add(5, ConsoleColor.Yellow);
        Dic.Add(7, ConsoleColor.Blue);
        Dic.Add(11, ConsoleColor.Green);
        Dic.Add(2, ConsoleColor.White);
        Dic.Add(4, Dic[2]);
        Dic.Add(6, Dic[2]);
        Dic.Add(8, Dic[2]);
        Dic.Add(10, Dic[2]);
        Dic.Add(12, Dic[2]);
        Dic.Add(14, Dic[2]);
        Dic.Add(16, Dic[2]);
        Dic.Add(18, Dic[2]);
        Dic.Add(22, Dic[2]);
    }
    static void MakeCollisionField() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                CollisionField[i, j] = Stage[i, j];
                CollisionField[i, 0] = 9;
                CollisionField[Height - 1, j] = 9;
                CollisionField[i, Width - 1] = 9;
            }
        }
    }
    static void FreezeBlock() {
        bool freezeFlag = false;
        MakeCollisionField();
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                if (Block[y, x] != 0) {
                    if (CollisionField[Fall + y + 1, Side + x] != 0) {
                        freezeFlag = true;
                    }
                }
            }
        }
        if (freezeFlag) {
            FallBlock();
            SaveField();
            //FallBlock();
            // FallBlock();
            InitVar2();
            // FallBlock();
        }
    }
    static void FallBlock() {
        bool a = false;
        DrawField();
        for (int i = 0; i < Height - 1; i++) {
            for (int j = 0; j < Width; j++) {
                if (Field[i, j] != 0 && Field[i + 1, j] == 0) {
                    a = true;
                }
            }
        }
        if (a) {
            int[,] V = new int[Height, Width];
            for (int i = 0; i < Width; i++) {
                int c = Height - 1;
                for (int j = Height - 1; j >= 0; j--) {
                    if (Field[j, i] != 0) {
                        V[c, i] = Field[j, i];
                        c--;
                    }
                }
            }
            Thread.Sleep(1000);
            Field = V;
            DrawField();
            DeleteBlock();
        }
        else {
            DeleteBlock();
        }

    }
    static void GameOver() {
        MakeCollisionField();
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                if (Block[y, x] != 0) {
                    if (CollisionField[Fall + y, Side + x] != 0) {
                        GameOverFlag = true;
                    }
                }
            }
        }
    }
    static void SaveField() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Stage[i, j] = Field[i, j];
            }
        }
        // FallBlock();
    }
    static void InitVar2() {
        Fall = 0;
        Side = 3;
        TurnPoint = 0;
    }
    static void GetKey() {
        int sideFlag = 0;
        MakeCollisionField();
        Timer timer = new Timer(Handler, null, 100, 400);
        if (KeyReaded) {
            switch (Key.Key) {
                case ConsoleKey.LeftArrow:
                    for (int y = 0; y < BlockHeight; y++) {
                        for (int x = 0; x < BlockWidth; x++) {
                            if (Block[y, x] != 0) {
                                if (CollisionField[Fall + y, Side + (x - 1)] != 0) {
                                    sideFlag++;
                                }
                            }
                        }
                    }
                    if (sideFlag == 0) {
                        Side--;
                    }
                    // KeyReaded = false;
                    break;
                case ConsoleKey.RightArrow:
                    for (int y = 0; y < BlockHeight; y++) {
                        for (int x = 0; x < BlockWidth; x++) {
                            if (Block[y, x] != 0) {
                                if (CollisionField[Fall + y, Side + (x + 1)] != 0) {
                                    sideFlag++;
                                }
                            }
                        }
                    }
                    if (sideFlag == 0) {
                        Side++;
                    }
                    // KeyReaded = false;
                    break;
                case ConsoleKey.X:
                    TurnRight();
                    break;
                case ConsoleKey.Z:
                    TurnLeft();
                    break;
                default:
                    break;
            }
        }
        KeyReaded = false;
    }
    static void ClearField() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Field[i, j] = 0;
            }
        }
    }
    static void Handler(object userState) {
        Key = Console.ReadKey(true);
        KeyReaded = true;
    }
    static void DeleteBlock() {
        bool[,] dontDelete = new bool[Height, Width];
        bool d = false;
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                if (Field[i, j] == 0 || Field[i, j] == 9) {
                    continue;
                }
                else {
                    int[] c = new int[2];
                    List<int[]> Con = new List<int[]>();
                    c[0] = i;
                    c[1] = j;
                    Con.Add(c);
                    int color = Field[i, j];
                    if (i - 1 >= 0) {
                        if (Field[i - 1, j] == color) {
                            c = new int[2];
                            c[0] = i - 1;
                            c[1] = j;
                            Con.Add(c);
                            if (Field[i - 1, j + 1] == color) {
                                c = new int[2];
                                c[0] = i - 1;
                                c[1] = j + 1;
                                Con.Add(c);
                            }
                            if (Field[i - 1, j - 1] == color) {
                                c = new int[2];
                                c[0] = i - 1;
                                c[1] = j - 1;
                                Con.Add(c);
                            }
                            if (i - 2 >= 0) {
                                if (Field[i - 2, j] == color) {
                                    c = new int[2];
                                    c[0] = i - 2;
                                    c[1] = j;
                                    Con.Add(c);
                                }
                            }
                        }
                    }
                    if (Field[i, j + 1] == color) {
                        c = new int[2];
                        c[0] = i;
                        c[1] = j + 1;
                        Con.Add(c);
                        if (Field[i + 1, j + 1] == color) {
                            c = new int[2];
                            c[0] = i + 1;
                            c[1] = j + 1;
                            if (!Con.Contains(c)) {
                                Con.Add(c);
                            }
                        }
                        if (i - 1 >= 0) {
                            if (Field[i - 1, j + 1] == color) {
                                c = new int[2];
                                c[0] = i - 1;
                                c[1] = j + 1;
                                if (!Con.Contains(c)) {
                                    Con.Add(c);
                                }
                            }
                        }
                        if (j + 2 < Width) {
                            if (Field[i, j + 2] == color) {
                                c = new int[2];
                                c[0] = i;
                                c[1] = j + 2;
                                Con.Add(c);
                            }
                        }
                    }
                    if (Field[i, j - 1] == color) {
                        c = new int[2];
                        c[0] = i;
                        c[1] = j - 1;
                        Con.Add(c);
                        if (Field[i + 1, j - 1] == color) {
                            c = new int[2];
                            c[0] = i + 1;
                            c[1] = j - 1;
                            if (!Con.Contains(c)) {
                                Con.Add(c);
                            }
                        }
                        if (i - 1 >= 0) {
                            if (Field[i - 1, j - 1] == color) {
                                c = new int[2];
                                c[0] = i - 1;
                                c[1] = j - 1;
                                if (!Con.Contains(c)) {
                                    Con.Add(c);
                                }
                            }
                        }
                        if (j - 2 >= 0) {
                            if (Field[i, j - 2] == color) {
                                c = new int[2];
                                c[0] = i;
                                c[1] = j - 2;
                                Con.Add(c);
                            }
                        }
                    }

                    if (Field[i + 1, j] == color) {
                        c = new int[2];
                        c[0] = i + 1;
                        c[1] = j;
                        Con.Add(c);
                        if (Field[i + 1, j + 1] == color) {
                            c[0] = i + 1;
                            c[1] = j + 1;
                            if (!Con.Contains(c)) {
                                Con.Add(c);
                            }
                        }
                        if (Field[i + 1, j - 1] == color) {
                            c = new int[2];
                            c[0] = i + 1;
                            c[1] = j - 1;
                            if (!Con.Contains(c)) {
                                Con.Add(c);
                            }
                        }
                        if (i + 2 < Width) {
                            if (Field[i + 2, j] == color) {
                                c = new int[2];
                                c[0] = i + 2;
                                c[1] = j;
                                Con.Add(c);
                            }
                        }
                    }
                    if (Con.Count >= 4) {
                        for (int q = 0; q < Con.Count; q++) {
                            dontDelete[Con[q][0], Con[q][1]] = true;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                if (dontDelete[i, j]) {
                    d = true;
                }
            }
        }
        DrawField();
        if (d) {
            Thread.Sleep(1000);
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    if (dontDelete[i, j]) {
                        Field[i, j] = 0;
                    }
                }
            }
            FallBlock();
        }
        else {
            BlockFlag = false;
        }
    }
    static void TurnRight() {
        TurnPoint++;
        bool turnFlag = true;
        int[,] turnBlock = new int[BlockHeight, BlockWidth];
        MakeCollisionField();
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                turnBlock[y, x] = Color[Blocks[BlockIndexY[r] + y, BlockIndexX[TurnPoint % 4] + x]];
            }
        }
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                if (turnBlock[y, x] != 0) {
                    if (CollisionField[Fall + y, Side + x] != 0) {
                        turnFlag = false;
                    }
                }
            }
        }
        if (turnFlag) {
            for (int y = 0; y < BlockHeight; y++) {
                for (int x = 0; x < BlockWidth; x++) {
                    Block[y, x] = turnBlock[y, x];
                }
            }
        }
        else {
            TurnPoint--;
        }

    }
    static void TurnLeft() {
        TurnPoint += 3;
        bool turnFlag = true;
        int[,] turnBlock = new int[BlockHeight, BlockWidth];
        MakeCollisionField();
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                turnBlock[y, x] = Color[Blocks[BlockIndexY[r] + y, BlockIndexX[TurnPoint % 4] + x]];
            }
        }
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                if (turnBlock[y, x] != 0) {
                    if (CollisionField[Fall + y, Side + x] != 0) {
                        turnFlag = false;
                    }
                }
            }
        }
        if (turnFlag) {
            for (int y = 0; y < BlockHeight; y++) {
                for (int x = 0; x < BlockWidth; x++) {
                    Block[y, x] = turnBlock[y, x];
                }
            }
        }
        else {
            TurnPoint++;
        }
    }
    static void MakeBlock() {
        if (!BlockFlag) {
            Random rnd = new Random();
            r = rnd.Next(14);
            //  r = 3;
            for (int y = 0; y < BlockHeight; y++) {
                for (int x = 0; x < BlockWidth; x++) {
                    Block[y, x] = Color[Blocks[BlockIndexY[r] + y, BlockIndexX[0] + x]];
                }
            }
            BlockFlag = true;
        }
    }
    static void MakeField() {
        for (int y = 0; y < BlockHeight; y++) {
            for (int x = 0; x < BlockWidth; x++) {
                Field[y + Fall, x + Side] = Block[y, x];
            }
        }
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Field[i, j] += Stage[i, j];
            }
        }
    }
    static void InitField() {
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                Field[i, 0] = 9;
                Field[Height - 1, j] = 9;
                Field[i, Width - 1] = 9;
            }
        }
    }
    static void InitVar() {
        Fall = 0;
        Side = 3;
        BlockFlag = false;
        GameOverFlag = false;
    }
    static void DrawField() {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        for (int i = 0; i < Height; i++) {
            for (int j = 0; j < Width; j++) {
                switch (Field[i, j]) {
                    case 9:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("■");
                        break;
                    case 0:
                        Console.Write("　");
                        break;
                    default:
                        Console.ForegroundColor = Dic[Field[i, j]];
                        Console.Write("●");
                        break;
                }
            }
            Console.WriteLine();
        }
    }
}