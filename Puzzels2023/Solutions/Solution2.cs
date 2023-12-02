using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Puzzels2023.Solutions;
public class Solution2() : SolutionBase(2, false)
{
    class Game
    {
        public int Id { get; set; }

        public List<Cube> Cubes { get; set; } = [];
    }

    class Cube
    {
        public Cube(string input)
        {
            input = input.Trim();
            string[] cubeParts = input.Split(" ");

            Amount = int.Parse(cubeParts[0]);

            // upper first char
            char[] color = cubeParts[1].ToCharArray();
            color[0] = char.ToUpper(color[0]);
            Color = (CubeColor)Enum.Parse(typeof(CubeColor), new string(color));
        }

        public int Amount { get; }

        public CubeColor Color { get; set; }
    }

    enum CubeColor
    {
        Red,
        Green,
        Blue,   
    }



    private List<Game> GetGames()
    {
        List<Game> games = new(_lines.Length);

        foreach (var line in _lines)
        {
            Game game = new();

            string[] gameSections = line.Split(':');
            game.Id = int.Parse(gameSections[0].Replace("Game ", ""));


            string[] samples = gameSections[1].Split(";");
            game.Cubes = new(samples.Length);

            foreach (var sample in samples)
            {
                string[] cubesFromSample = sample.Split(", ");

                foreach (var cube in cubesFromSample)
                {
                    game.Cubes.Add(new Cube(cube));
                }
            }

            games.Add(game);
        }

        return games;
    }

    public override string GetFirstSolution()
    {
        const int maxRedCubes = 12;
        const int maxGreenCubes = 13;
        const int maxBlueCubes = 14;

        List<Game> games = GetGames();

        int total = 0;
        foreach (var game in games)
        {
            bool cubeExeedsLimit = game.Cubes
                .Any(c =>
                {
                    return c.Color switch
                    {
                        CubeColor.Red => c.Amount > maxRedCubes,
                        CubeColor.Green => c.Amount > maxGreenCubes,
                        CubeColor.Blue => c.Amount > maxBlueCubes,
                        _ => false
                    };
                });
                
            if (cubeExeedsLimit)
            {
                continue;
            }

            total += game.Id;
        }

        return total.ToString();
    }

    public override string GetSecondSolution2()
    {
        List<Game> games = GetGames();

        int total = 0;

        foreach (var game in games)
        {
            int gamePower = 0;
            var groups = game.Cubes
                .GroupBy(c => c.Color);

            foreach (var group in groups)
            {
                int colorMaximum = group.MaxBy(c => c.Amount).Amount;

                if (gamePower == 0)
                {
                    gamePower = colorMaximum;
                    continue;
                }

                gamePower *= colorMaximum;
            }

            total += gamePower;
        }

        return total.ToString();
    }
}
