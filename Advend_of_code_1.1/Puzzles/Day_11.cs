using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advend_of_code_1._1.Puzzles
{
    internal class Day_11 : PuzzleSolution
    {
        public Day_11(StreamReader file) : base(file) { }

        private List<Monkey> _monkeys = new List<Monkey>();
        private List<int> _monkeyinspectedCount = new List<int>();

        public override string Puzzle1()
        {
            return GetMonkeyBusiness(true, 20);
        }

        public override string Puzzle2()
        {
            return GetMonkeyBusiness(false, 10000);
        }

        private string GetMonkeyBusiness(bool divideWorryLevel, int rounds)
        {
            InitMonkeys(divideWorryLevel);

            for (int round = 0; round < rounds; round++)
            {
                int monkeyCount = _monkeys.Count;
                for (int i = 0; i < monkeyCount; i++)
                {
                    Monkey monkey = _monkeys[i];

                    List<KeyValuePair<int, ulong>> itemsThrown = monkey.ProcessItems();

                    // throw items to new monkey
                    foreach (KeyValuePair<int, ulong> itemThrown in itemsThrown)
                        _monkeys[itemThrown.Key].AddItem(itemThrown.Value);

                    _monkeyinspectedCount[i] += itemsThrown.Count;
                }
            }

            _monkeyinspectedCount.Sort();
            _monkeyinspectedCount.Reverse();

            return (_monkeyinspectedCount[0] * _monkeyinspectedCount[1]).ToString();
        }

        private void InitMonkeys(bool divideWorryLevel)
        {
            List<string> lines = new();
            string line;

            while ((line = InputFile.ReadLine()) != null)
                lines.Add(line.Trim());

            lines.Add("");
            int linesCount = lines.Count / 7;
            for (int i = 0; i < linesCount; i++)
            {
                List<ulong> startingItems = new();
                string[] startingItemsRaw = lines[i * 7 + 1][15..].Split(", ");

                foreach (string startingItem in startingItemsRaw)
                    startingItems.Add(ulong.Parse(startingItem));

                string[] operationItems = lines[i * 7 + 2][17..].Split(' ');
                ulong testNumber = ulong.Parse(lines[i * 7 + 3][18..]);
                int trueCase = int.Parse(lines[i * 7 + 4][24..]);
                int falseCase = int.Parse(lines[i * 7 + 5][25..]);

                _monkeys.Add(new Monkey(
                    startingItems,
                    testNumber,
                    operationItems[1][0],
                    operationItems[0],
                    operationItems[2],
                    trueCase,
                    falseCase,
                    divideWorryLevel
                ));
                _monkeyinspectedCount.Add(0);
            }
        }
    }

    class Monkey
    {
        private List<ulong> _items;
        private ulong _divisibleTestNumber;
        private char _operationOperator;
        private string _operationValue1;
        private string _operationValue2;
        private int _monkeyOnTrue;
        private int _monkeyOnFalse;
        private bool _divideWorryLevel;

        public Monkey(
            List<ulong> startingItems
            , ulong divisibleTestNumber
            , char operationOperator
            , string operationValue1
            , string operationValue2
            , int monkeyOnTrue
            , int monkeyOnFalse
            , bool divideWorryLevel
        )
        {
            _items = startingItems;
            _divisibleTestNumber = divisibleTestNumber;
            _operationOperator = operationOperator;
            _operationValue1 = operationValue1;
            _operationValue2 = operationValue2;
            _monkeyOnTrue = monkeyOnTrue;
            _monkeyOnFalse = monkeyOnFalse;
            _divideWorryLevel = divideWorryLevel;
        }

        public List<KeyValuePair<int, ulong>> ProcessItems()
        {
            List<KeyValuePair<int, ulong>> result = new();
            int itemsCount = _items.Count;
            for (int i = 0; i < itemsCount; i++)
            {
                // inspeciton operation
                ulong value1 = _operationValue1 == "old" ? _items[i] : ulong.Parse(_operationValue1),
                    value2 = _operationValue2 == "old" ? _items[i] : ulong.Parse(_operationValue2);

                _items[i] = ExecuteOperation(value1, value2);

                if (_divideWorryLevel)
                    _items[i] = (ulong)Math.Floor((double)(_items[i] / 3));

                result.Add(new KeyValuePair<int, ulong>(ExecuteTest(_items[i]), _items[i]));
            }

            _items.Clear();

            return result;
        }
 
        public void AddItem(ulong item)
        {
            _items.Add(item);
        }

        private ulong ExecuteOperation(ulong value1, ulong value2)
        {
            return _operationOperator == '+' ? value1 + value2 : value1 * value2;
        }

        private int ExecuteTest(ulong worryLevel)
        {
            return worryLevel % _divisibleTestNumber == 0 ? _monkeyOnTrue : _monkeyOnFalse;
        }
    }
}
