using System;
using System.Collections.Generic;
using System.Linq;

namespace CountingAtoms
{
    class Program
    {
        public static void HasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/'?»«@£§€{}.-;<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item))
                {
                    throw new Exception("You can not use special characters");
                }

            }
        }
        public static void CheckForEmptyString(string input)
        {
            if (input == "")
            {
                throw new Exception("You can not pass empty string");
            }
        }
        public static string ReadText()
        {
            string input = Console.ReadLine();

            //Checks if you try to pass empty string and special characters
            CheckForEmptyString(input);
            HasSpecialChar(input);

            //Removes useless text
            input = input.Replace("molecule = ", "");
            input = input.Replace("\"", "");

            return input;
        }
        public static void CheckIfDictionaryContainsKeyAndAdds(Dictionary<string, int> output, string atom, int count)
        {
            // Check if there is already a key in the dictionary and adds a new one if there is not
            if (output.ContainsKey(atom) == false)
            {
                if (atom != "")
                {
                    output.Add(atom, count);
                    atom = "";
                    count = 0;
                }
            }
            else
            {
                output[atom] += count;
                atom = "";
                count = 0;
            }
        }
        public static Dictionary<string, int> CountAtoms(string formula)
        {
            Dictionary<string, int> output = new Dictionary<string, int>();
            int count = 0;
            string atom = "";
            int bracketStart = int.MaxValue;
            int bracketsEnd = int.MaxValue;

            for (int i = 0; i < formula.Length; i++)
            {
                //Checks for new element by searching for upper case letter
                if (Char.IsUpper(formula[i]) && formula[i] != '(' && formula[i] != ')')
                {
                    //First element
                    if (i == 0)
                    {
                        atom += formula[i];
                        count++;
                    }
                    //Rest
                    else
                    {
                        CheckIfDictionaryContainsKeyAndAdds(output, atom, count);
                        //Restarts count
                        atom = "";
                        count = 0;

                        atom += formula[i];
                        count++;
                    }
                }
                //Checks for a element with 2 letter name
                if (Char.IsLower(formula[i]) && formula[i] != '(' && formula[i] != ')')
                {
                    atom += formula[i];
                }
                //Checks for opening bracket
                else if (formula[i] == '(')
                {
                    atom = "";
                    bracketStart = i;
                    for (int j = i + 1; j < formula.Length; j++)
                    {
                        //Finds closing bracket`s index
                        if (formula[j] == ')')
                        {
                            bracketsEnd = i + (j - i);
                            break;
                        }
                    }
                }
                //Multiplies atoms in brackets
                else if (i > bracketStart && i < bracketsEnd && Char.IsDigit(formula[i]) != true)
                {
                    //Checks if after atom there is digit
                    if (Char.IsDigit(formula[i + 1]))
                    {
                        string n = formula[i + 1].ToString();
                        int number = Int32.Parse(n.ToString());
                        if (count == 1)
                        {
                            count += number - 1;
                        }
                        else
                        {
                            count += number;
                        }
                    }
                    //Multiplication
                    string num = formula[bracketsEnd + 1].ToString();
                    int numb = Int32.Parse(num.ToString());
                    count *= numb;
                    num = "";
                    numb = 0;

                }
                //Checks for digit after atom and adds it
                else if (Char.IsDigit(formula[i]) == true && i < bracketStart || i > bracketsEnd)
                {
                    if (Char.IsDigit(formula[i]))
                    {
                        string n = formula[i].ToString();
                        int number = Int32.Parse(n.ToString());
                        if (count == 1)
                        {
                            count += number - 1;
                        }
                        else
                        {
                            count += number;
                        }
                    }
                }
                //Adds new atoms to the dictionary
                if (i + 1 < formula.Length)
                {
                    if (!Char.IsLower(formula[i + 1]) && !Char.IsDigit(formula[i + 1]))
                    {
                        CheckIfDictionaryContainsKeyAndAdds(output, atom, count);
                        atom = "";
                        count = 0;
                    }
                }
                //Adds the last atom to the dictionary
                else
                {
                    CheckIfDictionaryContainsKeyAndAdds(output, atom, count);
                }
            }
            return output;
        }
        static void Main(string[] args)
        {
            string formula = ReadText();
            Dictionary<string, int> atoms = new Dictionary<string, int>();
            atoms = CountAtoms(formula);

            //Sorts dictionary alphabetically
            var list = atoms.Keys.ToList();
            list.Sort();

            //Prints the result
            Console.Write("countAtoms(molecule) = \"");
            foreach (var item in list)
            {
                Console.Write("{0}{1}", item, atoms[item]);
            }
            Console.Write("\"");

            //Moves "Press any key to continue..." to new line
            Console.WriteLine();
        }
    }
}
