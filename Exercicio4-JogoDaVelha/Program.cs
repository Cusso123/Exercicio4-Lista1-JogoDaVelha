using System;

class Program
{
    static char[,] Quadro = {
        {'1', '2', '3'},
        {'4', '5', '6'},
        {'7', '8', '9'}
    };

    static char PlayerAtual = 'X';
    static int Dificuldade = 3; // 1 = Fácil, 2 = Médio, 3 = Difícil

    static void Main(string[] args)
    {
        Console.WriteLine("Escolha a dificuldade: 1 - Fácil, 2 - Médio, 3 - Difícil");
        Dificuldade = int.Parse(Console.ReadLine());
        int turno = 0;
        bool gameOver = false;

        while (!gameOver)
        {
            ExibirTabuleiro();
            if (PlayerAtual == 'X')
            {
                Console.WriteLine("Jogador X, escolha uma posição: ");
                int escolha = int.Parse(Console.ReadLine());
                Marcador(escolha);
            }
            else
            {
                Console.WriteLine("O Computador está jogando...");
                int bestMove = ObterMelhorMov();
                Marcador(bestMove);
            }

            gameOver = VerificarVitoria() || turno == 8;

            PlayerAtual = (PlayerAtual == 'X') ? 'O' : 'X';
            turno++;
        }

        ExibirTabuleiro();
        if (VerificarVitoria())
        {
            Console.WriteLine("O vencedor é o jogador " + (PlayerAtual == 'X' ? 'O' : 'X') + "!");
        }
        else
        {
            Console.WriteLine("O jogo terminou empatado!");
        }
    }

    static void ExibirTabuleiro()
    {
        Console.Clear();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Quadro[i, j] == 'X')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else if (Quadro[i, j] == 'O')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }
                Console.Write(" " + Quadro[i, j]);
                Console.ResetColor();
                if (j < 2) Console.Write(" |");
            }
            if (i < 2) Console.WriteLine("\n---|---|---");
        }
        Console.WriteLine();
    }

    static void Marcador(int escolha)
    {
        char marcacao = (PlayerAtual == 'X') ? 'X' : 'O';
        switch (escolha)
        {
            case 1: Quadro[0, 0] = marcacao; break;
            case 2: Quadro[0, 1] = marcacao; break;
            case 3: Quadro[0, 2] = marcacao; break;
            case 4: Quadro[1, 0] = marcacao; break;
            case 5: Quadro[1, 1] = marcacao; break;
            case 6: Quadro[1, 2] = marcacao; break;
            case 7: Quadro[2, 0] = marcacao; break;
            case 8: Quadro[2, 1] = marcacao; break;
            case 9: Quadro[2, 2] = marcacao; break;
        }
    }

    static bool VerificarVitoria()
    {
        char[] LinhaVitoria = {
            Quadro[0, 0], Quadro[0, 1], Quadro[0, 2],
            Quadro[1, 0], Quadro[1, 1], Quadro[1, 2],
            Quadro[2, 0], Quadro[2, 1], Quadro[2, 2],
            Quadro[0, 0], Quadro[1, 0], Quadro[2, 0],
            Quadro[0, 1], Quadro[1, 1], Quadro[2, 1],
            Quadro[0, 2], Quadro[1, 2], Quadro[2, 2],
            Quadro[0, 0], Quadro[1, 1], Quadro[2, 2],
            Quadro[0, 2], Quadro[1, 1], Quadro[2, 0]
        };

        for (int i = 0; i < LinhaVitoria.Length; i += 3)
        {
            if (LinhaVitoria[i] == LinhaVitoria[i + 1] && LinhaVitoria[i + 1] == LinhaVitoria[i + 2])
            {
                return true;
            }
        }
        return false;
    }

    static bool MovRestante()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (Quadro[i, j] != 'X' && Quadro[i, j] != 'O')
                    return true;
        return false;
    }

    static int Avaliar()
    {
        for (int row = 0; row < 3; row++)
        {
            if (Quadro[row, 0] == Quadro[row, 1] && Quadro[row, 1] == Quadro[row, 2])
            {
                if (Quadro[row, 0] == 'O')
                    return +10;
                else if (Quadro[row, 0] == 'X')
                    return -10;
            }
        }

        for (int col = 0; col < 3; col++)
        {
            if (Quadro[0, col] == Quadro[1, col] && Quadro[1, col] == Quadro[2, col])
            {
                if (Quadro[0, col] == 'O')
                    return +10;
                else if (Quadro[0, col] == 'X')
                    return -10;
            }
        }

        if (Quadro[0, 0] == Quadro[1, 1] && Quadro[1, 1] == Quadro[2, 2])
        {
            if (Quadro[0, 0] == 'O')
                return +10;
            else if (Quadro[0, 0] == 'X')
                return -10;
        }

        if (Quadro[0, 2] == Quadro[1, 1] && Quadro[1, 1] == Quadro[2, 0])
        {
            if (Quadro[0, 2] == 'O')
                return +10;
            else if (Quadro[0, 2] == 'X')
                return -10;
        }

        return 0;
    }

    static int CalcularPontuacaoPosicao(int row, int col)
    {
        int pontuacao = 0;

        if (row == 1 && col == 1)
        {
            pontuacao += 2;
        }

        if ((row == 0 || row == 2) && (col == 0 || col == 2))
        {
            pontuacao += 1;
        }

        char adversario = (PlayerAtual == 'X') ? 'O' : 'X';
        if (VerificarLinhaColunaDiagonal(row, col, adversario))
        {
            pontuacao -= 2;
        }

        if (ImpedeVitoriaAdversario(row, col))
        {
            pontuacao += 4;
        }

        if (LevaAVitoria(row, col))
        {
            pontuacao += 4;
        }

        return pontuacao;
    }

    static bool VerificarLinhaColunaDiagonal(int row, int col, char adversario)
    {
        return (Quadro[row, 0] == adversario || Quadro[row, 1] == adversario || Quadro[row, 2] == adversario) ||
               (Quadro[0, col] == adversario || Quadro[1, col] == adversario || Quadro[2, col] == adversario) ||
               (row == col && (Quadro[0, 0] == adversario || Quadro[1, 1] == adversario || Quadro[2, 2] == adversario)) ||
               (row + col == 2 && (Quadro[0, 2] == adversario || Quadro[1, 1] == adversario || Quadro[2, 0] == adversario));
    }

    static bool ImpedeVitoriaAdversario(int row, int col)
    {
        char adversario = (PlayerAtual == 'X') ? 'O' : 'X';
        Quadro[row, col] = adversario;
        bool impede = Avaliar() == 10;
        Quadro[row, col] = (char)('1' + 3 * row + col);
        return impede;
    }

    static bool LevaAVitoria(int row, int col)
    {
        Quadro[row, col] = PlayerAtual;
        bool vitoria = Avaliar() == 10;
        Quadro[row, col] = (char)('1' + 3 * row + col);
        return vitoria;
    }

    static int ObterMelhorMov()
    {
        if (Dificuldade == 1)
        {
            return ObterMovRandom();
        }
        else if (Dificuldade == 2)
        {
            return ObterMovRandomBest();
        }
        else
        {
            int melhorVal = -1000;
            int melhorMov = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Quadro[i, j] != 'X' && Quadro[i, j] != 'O')
                    {
                        int pontuacao = CalcularPontuacaoPosicao(i, j);
                        if (pontuacao > melhorVal)
                        {
                            melhorMov = 3 * i + j + 1;
                            melhorVal = pontuacao;
                        }
                    }
                }
            }
            return melhorMov;
        }
    }

    static int ObterMovRandom()
    {
        Random aleatorio = new Random();
        int movimento;
        do
        {
            movimento = aleatorio.Next(1, 10);
        } while (Quadro[(movimento - 1) / 3, (movimento - 1) % 3] == 'X' || Quadro[(movimento - 1) / 3, (movimento - 1) % 3] == 'O');
        return movimento;
    }

    static int ObterMovRandomBest()
    {
        Random aleatorio = new Random();
        if (aleatorio.Next(0, 2) == 0)
        {
            return ObterMelhorMov();
        }
        else
        {
            return ObterMovRandom();
        }
    }
}
