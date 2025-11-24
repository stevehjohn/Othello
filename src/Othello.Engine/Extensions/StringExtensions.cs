using System.Text;
using Othello.Engine.Infrastructure;

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

                if ((moves & (1ul << cell)) != 0)
                {
                    builder.Append('X');
                }
                else
                {
                    builder.Append(board[i]);
                }

                cell++;
            }

            return builder.ToString();
        }
    }
}