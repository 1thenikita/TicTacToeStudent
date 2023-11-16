namespace TicTacToeStudent.Shared.Model;

public class GamePiece
{
    public PieceStyle Style { get; set; } = PieceStyle.Blank;
}

public class GameBoard
{
    //Двумерный массив игровых фигур, 
    // ожидается пустым в начале игры.
    public GamePiece[,] Board { get; set; }

    public PieceStyle CurrentTurn = PieceStyle.X;
    public readonly int Count = 3;

    public GameBoard(int count = 3)
    {
        Count = count;
        Reset();
    }

    public GamePiece? GetBoard(int x, int y)
    {
        try
        {
            return Board[x, y];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine($"{x} {y}");
            return null;
        }
    }
    
    public void Reset()
    {
        Board = new GamePiece[Count, Count];

        //Populate the Board with blank pieces
        for (int i = 0; i <= (Count-1); i++)
        {
            for (int j = 0; j <= (Count-1); j++)
            {
                Board[i, j] = new();
            }
        }
    }

    private void SwitchTurns()
    {
        //This is equivalent to: if currently X's turn, 
        // make it O's turn, and vice-versa
        CurrentTurn =
            CurrentTurn == PieceStyle.X ? PieceStyle.O : PieceStyle.X;
    }

    public bool GameComplete => GetWinner() != null || IsADraw();

    public void PieceClicked(int x, int y)
    {
        //If the game is complete, do nothing
        if (GameComplete)
        {
            return;
        }

        //If the space is not already claimed...
        var clickedSpace = GetBoard(x, y);
        if (clickedSpace == null)
        {
            return;
        }
        if (clickedSpace.Style == PieceStyle.Blank)
        {
            //Set the marker to the current turn marker (X or O)
            clickedSpace.Style = CurrentTurn;

            //Make it the other player's turn
            SwitchTurns();
        }
    }

    public bool IsADraw()
    {
        int pieceBlankCount = 0;

        //Count all the blank spaces. 
        //If the count is 0, this is a draw.
        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < Count; j++)
            {
                pieceBlankCount = this.Board[i, j].Style == PieceStyle.Blank
                    ? pieceBlankCount + 1
                    : pieceBlankCount;
            }
        }

        return pieceBlankCount == 0;
    }

    private WinningPlay EvaluatePieceForWinner(int i, int j,
        EvaluationDirection dir)
    {
        GamePiece currentPiece = Board[i, j];
        if (currentPiece.Style == PieceStyle.Blank)
        {
            return null;
        }

        int inARow = 1;
        int iNext = i;
        int jNext = j;

        var winningMoves = new List<string>();

        while (inARow < Count)
        {
            //For each direction, increment the pointers 
            //to the next space to be evaluated
            switch (dir)
            {
                case EvaluationDirection.Up:
                    jNext -= 1;
                    break;
                case EvaluationDirection.UpRight:
                    iNext += 1;
                    jNext -= 1;
                    break;
                case EvaluationDirection.Right:
                    iNext += 1;
                    break;
                case EvaluationDirection.DownRight:
                    iNext += 1;
                    jNext += 1;
                    break;
            }

            //If the next "space" is off the board, 
            //don't check it.
            if (iNext < 0 || iNext >= Count
                          || jNext < 0 || jNext >= Count)
            {
                break;
            }

            //If the next space has a matching letter...
            if (Board[iNext, jNext].Style == currentPiece.Style)
            {
                //Add this space to the collection 
                //of winning spaces.
                winningMoves.Add($"{iNext},{jNext}");
                inARow++;
            }
            else //Otherwise, no tic-tac-toe is found 
                //for this space/direction
            {
                return null;
            }
        }

        //If we found three in a row
        if (inARow >= Count)
        {
            //Return this set of spaces as the winning set
            winningMoves.Add($"{i},{j}");

            return new WinningPlay()
            {
                WinningMoves = winningMoves,
                WinningStyle = currentPiece.Style,
                WinningDirection = dir,
            };
        }

        //If we got here, we didn't find any tic-tac-toes 
        //for the given space.
        return null;
    }

    public WinningPlay GetWinner()
    {
        WinningPlay winningPlay = null;

        for (int i = 0; i < Count; i++)
        {
            for (int j = 0; j < Count; j++)
            {
                foreach (EvaluationDirection evalDirection in (EvaluationDirection[])Enum.GetValues(
                             typeof(EvaluationDirection)))
                {
                    winningPlay = EvaluatePieceForWinner(i, j, evalDirection);
                    if (winningPlay != null)
                    {
                        return winningPlay;
                    }
                }
            }
        }

        return winningPlay;
    }

    public string GetGameCompleteMessage()
    {
        var winningPlay = GetWinner();
        return winningPlay != null
            ? $"{winningPlay.WinningStyle} Wins!"
            : "Draw!";
    }

    public bool IsGamePieceAWinningPiece(int i, int j)
    {
        var winningPlay = GetWinner();
        return winningPlay?.WinningMoves?
            .Contains($"{i},{j}") ?? false;
    }
}

public enum EvaluationDirection
{
    Up,
    UpRight,
    Right,
    DownRight
}

public class WinningPlay
{
    public List<string> WinningMoves { get; set; }
    public EvaluationDirection WinningDirection { get; set; }
    public PieceStyle WinningStyle { get; set; }
}