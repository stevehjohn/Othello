using System.Text;

namespace Othello.Engine.Extensions;

public static class StringExtensions
{
    extension(string board)
    {
        public string SuperimposeLegalMoves(ulong moves)
        {
            var builder = new StringBuilder();

            var cell = 0;
            
            for (var i = 0; i < board.Length; i++)
            {
                if (board[i] < ' ')
                {
                    builder.Append(board[i]);
                    
                    continue;
                }

                builder.Append((moves & (1ul << cell)) != 0 ? '+' : board[i]);

                cell++;
            }

            return builder.ToString();
        }
    }
}