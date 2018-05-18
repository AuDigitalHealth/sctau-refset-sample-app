using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTDemo;

namespace CTDemo
{
    /// <summary>
    ///  The main class of the application to be executed.
    ///  Provides a simple command line interface to perform SCT-AU query operations.
    ///  This application is for demonstration purposes only and is not intended for clinical use.
    /// </summary>

    class Program {

    private static void PrintMenu()
    {
        Console.WriteLine("\nThis application is not for clinical use.");

        Console.WriteLine("\n\nOptions:");
        Console.WriteLine("\t1. Find concept by SCT ID");
        Console.WriteLine("\t2. Find concept by term");
        Console.WriteLine("\t3. List all members of a refset");
        Console.WriteLine("\tQ. Quit");
        Console.WriteLine("\n\nEnter selection:");
    }

    private static void FindConceptById()
    {
        Console.WriteLine("\nFinding concept by SCT ID...");
        Console.WriteLine("\nEnter SCT ID of concept:");

        try
        {
            long sctId = long.Parse(ReadLineFromConsole());

            DataSource.PrintDatabaseDetails();
            Concept concept = ConceptFinder.FindById(sctId);
            if (concept != null)
            {
                Console.WriteLine(concept);
            }
            else
            {
                Console.WriteLine("No concept found with SCT ID " + sctId.ToString());
            }
        }
        catch 
        {
            Console.WriteLine("Invalid SCT ID!");
        }
    }

    private static void FindConceptByTerm()
    {
        Console.WriteLine("\nFinding concept by term...");
        Console.WriteLine("\nEnter partial term:");

        string term = ReadLineFromConsole();
        DataSource.PrintDatabaseDetails();
        List<Concept> concepts = ConceptFinder.FindByTerm(term);
        PrintConcepts(concepts);
    }

    private static void ListAllRefsetMembers()
    {
        Console.WriteLine("\nListing all members of a refset...");
        Console.WriteLine("\nEnter SCT ID of refset:");
        try
        {
            long sctId = long.Parse(ReadLineFromConsole());
            DataSource.PrintDatabaseDetails();
            List<Concept> concepts = ConceptFinder.FindRefsetMembers(sctId);
            PrintConcepts(concepts);

        }
        catch 
        {
            Console.WriteLine("Invalid SCT ID!");
        }
    }

    private static char ReadCharFromConsole()
    {
        int c = Console.Read();
        return Convert.ToChar(c);
    }

    private static string ReadLineFromConsole()
    {
        string word;
        do
        {
          word = Console.ReadLine();
        } while (word == string.Empty);

        return word;
    }

    private static void PrintConcepts(List<Concept> concepts) {
        if (concepts == null || concepts.Count == 0) {
            Console.WriteLine("No suitable concepts found!");
        } else {
            String resultCount = concepts.Count.ToString();
            if (concepts.Count == DataSource.GetMaxRows()) {
                resultCount += " (limited)";
            }
            int conceptNumber = 1;
            foreach (Concept concept in concepts)
            {
                Console.WriteLine("Concept " + (conceptNumber++) + " of " + resultCount);
                Console.WriteLine(concept);
            }
        }
    }

    /// <summary>
    ///  Continually prompt the user with the menu to select an option and run the selected operation.
    /// </summary>
    private static void Execute()
    {
        bool loop = true;
        while (loop)
        {
            PrintMenu();
            switch (ReadCharFromConsole()) {
            
                case '1' : 
                    FindConceptById();
                    break;
                case '2' : 
                    FindConceptByTerm();
                    break;
                case '3' : 
                    ListAllRefsetMembers();
                    break;
                case 'q': case 'Q':
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please select 1, 2, or 3.");
                    break;      
            }
        }
    }

    static void Main(string[] args)
    {
        Execute();
    }

    }
}