using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Data;
using System.Runtime.CompilerServices;
using System.IO.Pipes;
using System.Security.Cryptography;
using System.Reflection.Metadata;

public class IntiativeTracker
{
    public static void Main()
    {
        Dictionary<string, int> final_intitiative = null;
        string command = "";
        message("Type 'Help' to view command list.");
        while (command.ToUpper() != "EXIT")
        {
            command = Console.ReadLine();
            Console.Clear();
            List<string> command_words = command.Split(' ').ToList();
            bool num = false;
            if (command_words.Count > 2)
            {

                num = int.TryParse(command_words[2], out int result);
            }

            // runs clear command
            if (command.ToUpper() == "CLEAR")
            {
                Console.Clear();
            }

            // runs help command
            if (command.ToUpper() == "HELP")
            {
                Console.WriteLine("-------------------------------------------------" + Environment.NewLine);
                foreach (var item in command_list)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine(Environment.NewLine + "-------------------------------------------------");
            }
            // runs initiative command
            if (command.ToUpper() == "GENERATE")
            {
                final_intitiative = createlist();
                PrintList(final_intitiative);
            }

            // runs print list command

            if (command.ToUpper() == "LIST" || command.ToUpper() == "L")
            {
                if (final_intitiative != null)
                {
                    message("Fetching initiative...");
                    PrintList(final_intitiative);
                }
                else
                {
                    message("Error, initiative list has not been generated");
                }

            }
            // Remove command
            if (command_words[0].ToUpper() == "REMOVE")
            {
                if (final_intitiative != null && final_intitiative.Count > 1 && command_words.Count == 2)
                {
                    RemoveChar(final_intitiative, command_words[1]);
                    PrintList(final_intitiative);
                }
                else
                {
                    get_negative_message();
                }
            }
            // Add command
            if (command_words[0].ToUpper() == "ADD")
            {
                if (final_intitiative != null && num is true && command_words.Count == 3)
                {
                    final_intitiative = AddChar(final_intitiative, command_words[1], command_words[2]);
                    PrintList(final_intitiative);
                }
                else
                {
                    get_negative_message();
                }

            }
            // Next command
            if (command.ToUpper() == "NEXT" || command.ToUpper() == "N")
            {
                if (final_intitiative != null)
                {
                    Next(final_intitiative);
                }
                else
                {
                    get_negative_message();
                }
            }
            // swap command
            if (command_words[0].ToUpper() == "SWAP")
            {
                if (final_intitiative != null && command_words.Count == 3)
                {
                    final_intitiative = SwapChar(final_intitiative, command_words[1], command_words[2]);
                    PrintList(final_intitiative);
                }
                else
                {
                    get_negative_message();
                }

            }
            

        }


    }

    // stores command list
    public static string[] command_list = {
        "Generate: Creates initial list based on user input",
        "Exit: Exit the program",
        "List (l): List the current initiative order, and display the current turn",
        "Add <Person> <Int>: Add a new character to the initiative list",
        "Remove <Person>: Remove a character from the initiative list",
        "Next (n): Start's the next character's turn",
        "Swap <Person1> <Person2>: Swap the position of two characters",
        "Clear: Clear the command history"
        };

    // a counter to keep track of current initiative
    public static int counter = 0;

    // a list holding positive feedback!
    public static string[] positive_feedback = { "I see you.", "Alright!", "Roger!", "Understood.", "Noted.", "Affirmative!", "As you wish!", "I suppose.", "Per your command!"};

    // a list with a bit less positive feedback...

    public static string[] negative_feedback = { "I don't know...", "Umm...", "Maybe not.", "That isn't gonna work.", "I don't wanna.", "Invalid.", "Think again, casual.", "Heh, you thought.", "Boo hoo."};

    // method to print message in good format

    public static void message(string text)
    {
        Console.WriteLine("-------------------------------------------------" + Environment.NewLine + Environment.NewLine + $"{text}" + Environment.NewLine + Environment.NewLine + "-------------------------------------------------");
    }    
    // method to get positive message
    public static void get_positive_message()
    {
        Random rand = new Random();
        int index = rand.Next(0, positive_feedback.Length);
        message(positive_feedback[index]);
    }

    public static void get_negative_message()
    {
        Random rand = new Random();
        int index = rand.Next(0, negative_feedback.Length);
        message(negative_feedback[index]);
    }
    // checks if input conditions for initiative are valid



    public static bool Valid(string attempt)
    {
        // create a list to check if it meets the criteria of two indexs
        List<string> attempt_list = attempt.Split(' ').ToList();
        if (attempt_list.Count == 2)
        {
            // try to parse [1] into int ONLY after checking it has two slots
            bool success = int.TryParse(attempt_list[1], out int result);
            
            return success;
        }
        return false;
    }


    // creates a list for initiatve. Input required is 'name' 'intiative'
    public static Dictionary<string, int> createlist()
    {

        counter = 0;
        string input = "";
        message("Please type each character, followed by their roll (i.e, goblin 10). Type 'End' to stop:");
        // dict which contains initiatives
        
        Dictionary<string, int> init_list = new Dictionary<string, int>();
        while (input.ToUpper() != "END")
        {
            bool different_person = true;
            input = Console.ReadLine();


            if (Valid(input) == true && different_person is true)
            {
                List<string> character_and_roll = input.Split(' ').ToList();

                // test to make sure person is not a repeat
                foreach (string person in init_list.Keys)
                {
                    if (person == character_and_roll[0])
                    {
                        different_person = false;
                    }


                }
                if (different_person is true)
                {
                    init_list.Add(character_and_roll[0].ToLower(), int.Parse(character_and_roll[1]));
                    get_positive_message();
                }
                else
                {
                    get_negative_message();
                }

                

            }
            else if (input.ToUpper() == "END")
            {
                message("All done!");
            }
            else
            {
                get_negative_message();
            }





        }
        var sortedDict = from entry in init_list orderby entry.Value descending select entry;
        return sortedDict.ToDictionary<string, int>();

    }
    // swaps two characters
    public static Dictionary<string, int> SwapChar(Dictionary<string, int> initiative, string character1, string character2)
    {

        // get list of all characters in dictionary

        List<string> people = initiative.Keys.ToList();

        // dictionary to be returned

        Dictionary<string, int> to_return = new Dictionary<string, int>();

        // ints to store index of characters in list
        int char1_index = -1;
        int char2_index = -1;

        // Goes through list to store index of characters
        for (int i = 0; i < people.Count; i++)
        {
            if (people[i] == character1)
            {
                char1_index = i;
            }
            ;
            if (people[i] == character2)
            {
                char2_index = i;
            }
        }
        // if one or both characters aren't in list ends function
        if (char1_index == -1 || char2_index == -1)
        {
            get_negative_message();
            return initiative;
        }

        // otherwise swaps position of people in list, and makes a new dictionary
        people[char1_index] = character2;
        people[char2_index] = character1;

        for (int i = 0; i < people.Count; i++)
        {
            to_return[people[i]] = initiative.Values.ToList()[i];
        }
        return to_return;

    }
    // adds a character to initiative
    public static Dictionary<string, int> AddChar(Dictionary<string, int> initiative, string character, string roll)
    {

        // bool that returns false if the person you are trying to add is alreday in the dictionary

        character = character.ToLower();
        
        bool valid = true;

        foreach (string person in initiative.Keys)
        {
            if (character == person)
            {
                valid = false;
            }
        }
        
        // adds character if they are not in dictionary
        if (valid is true)
        {
            initiative.Add(character, int.Parse(roll));
            var sortedDict = from entry in initiative orderby entry.Value descending select entry;
            return sortedDict.ToDictionary<string, int>();
        }
        else
        {
            get_negative_message();
            return initiative;
        }





    }
    public static void RemoveChar(Dictionary<string, int> initiative, string name)
    {
        name = name.ToLower();
        string before = initiative.Keys.ElementAt(counter);
        bool success = initiative.Remove(name);


        if (success is true)
        {


            // check if counter goes out of range
            if (counter >= initiative.Count)
            {
                counter = counter - 1;
            }
            // compares counter before and after removal in case it needs to be moved
            string after = initiative.Keys.ElementAt(counter);
            if (after != before && before != name)
            {
                counter = counter - 1;
            }
        }
        else
        {
            get_negative_message();
        }
    }
    // prints the current initiative list
    public static void PrintList(Dictionary<string, int> initiative)
    {
        Console.WriteLine("-------------------------------------------------");
        foreach (var item in initiative)
        {
            // shows who's turn it currently is
            if (initiative.Keys.ElementAt(counter) == item.Key)
            {
                Console.WriteLine(item + " <----- (CURRENT TURN)");
            }
            else
            {
                Console.WriteLine(item);
            }


        }
        Console.WriteLine("-------------------------------------------------");
    }


    // makes it the next person's turn
    public static void Next(Dictionary<string, int> initiative)
    {
        if (initiative.Count > 0)
        {
            if (counter < (initiative.Count - 1))
            {
                counter++;
            }
            else
            {
                counter = 0;
            }
            PrintList(initiative);
            Console.WriteLine("-------------------------------------------------" + Environment.NewLine + $"Next up...{initiative.Keys.ElementAt(counter)}!!" + Environment.NewLine + "-------------------------------------------------");
        }
    }
};

