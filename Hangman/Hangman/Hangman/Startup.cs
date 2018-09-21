using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Hangman
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            string[] input = File.ReadAllText("../../input.txt")
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, List<string>> wordBank = new Dictionary<string, List<string>>();
            List<string> phrases = new List<string>();
            Random rnd = new Random();

            //adds all words with underscore as keys and the words without underscores as their values
            for (int i = 0; i < input.Length; i++)
            {

                if (!input[i].StartsWith("_"))
                {
                    phrases.Add(input[i]);

                }

                if (input[i].StartsWith("_"))
                {
                    phrases = new List<string>();
                    wordBank.Add(input[i].Remove(0, 1), phrases);

                }

            }
            int score = 1;
            bool endGame = false;

            while (!endGame)
            {
                Console.WriteLine($"Please choose a category:");
                Console.WriteLine($"NOTE: the input for picking category is case sensitive!");
                foreach (KeyValuePair<string, List<string>> word in wordBank)
                {
                    Console.WriteLine(word.Key);

                }

                string command = Console.ReadLine();
                string randomWord = String.Empty;
                int randomWordIndex;

                while (!wordBank.Keys.Contains(command))
                {
                    Console.WriteLine($"{command} is not a valid category!");
                    Console.WriteLine($"Please choose a category:");
                    command = Console.ReadLine();
                }

                randomWordIndex = rnd.Next(0, wordBank[command].Count);
                randomWord = wordBank[command][randomWordIndex];
                string randomWordLower = randomWord.ToLower();
                List<char> guessedChars = new List<char>();
                char[] rndWord = ReplaceLetters(randomWord).ToCharArray();
                int attempts = 10;

                Console.WriteLine($"Attempts: {attempts}");
                Console.Write("Current word/phrase: ");
                Console.WriteLine(rndWord);
                Console.WriteLine($"Enter a letter: ");

                while (true)
                {
                    char guessLetter = char.Parse(Console.ReadLine().ToLower());

                    if (randomWordLower.Contains(guessLetter) && !guessedChars.Contains(guessLetter))
                    {
                        for (int i = 0; i < randomWord.Length; i++)
                        {
                            if (guessLetter == randomWordLower[i])
                            {
                                rndWord[i] = guessLetter;
                                guessedChars.Add(guessLetter);
                            }
                        }
                    }
                    else if (attempts == 1)
                    {
                        Console.WriteLine($"Score: {score -1}, Attempts: {attempts - 1}");
                        endGame = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"The word/phrase doesn't have this letter or the player already guessed that letter!");
                        attempts--;
                    }

                    if (!rndWord.Contains('_'))
                    {
                        Console.WriteLine($"Congratulations your score is {score}! You've guessed the word/phrase {randomWord}! {Environment.NewLine}");
                        score++;
                        break;
                    }

                    Console.WriteLine($"Attempts: {attempts}");
                    Console.Write("Current word/phrase: ");
                    Console.WriteLine(rndWord);
                    Console.WriteLine($"Enter a letter: ");
                }
            }
        }

        #region replace letters
        //method that replaces all characters with underscores
        public static string ReplaceLetters(string word)
        {
            var replacedWord = new String(word.Select(r => r == ' ' ? ' ' : '_').ToArray());

            return replacedWord;
        }
        #endregion
    }
}
