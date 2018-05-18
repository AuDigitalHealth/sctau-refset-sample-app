package au.gov.digitalhealth;

import java.util.Collection;
import java.util.InputMismatchException;
import java.util.Scanner;

import au.gov.digitalhealth.db.ConceptFinder;
import au.gov.digitalhealth.model.Concept;
import au.gov.digitalhealth.Main;

/**
 * The main class of the application to be executed.
 *
 * Provides a simple command line interface to perform SCT-AU query operations.
 */
public class Main {

    private Scanner input = new Scanner(System.in);

    public Main() {
        input.useDelimiter(System.getProperty("line.separator"));
    }

    private void printMenu() {

    	System.out.println("This application is not for clinical use.");

        System.out.println("\n\nOptions:");
        System.out.println("\t1. Find concept by SCT ID");
        System.out.println("\t2. Find concept by term");
        System.out.println("\t3. List all members of a refset");
        System.out.println("\tQ. Quit");
        System.out.println("\n\nEnter selection:");
    }


    private void findConceptById() {
        System.out.println("\nFinding concept by SCT ID...");
        System.out.println("\nEnter SCT ID of concept:");

        try {

            long sctId = input.nextLong();
            Concept concept = ConceptFinder.findById(sctId);
            if (concept != null) {
                System.out.println(concept);
            } else {
                System.out.println("No concept found with SCT ID " + sctId);
            }

        } catch (InputMismatchException e) {
            System.out.println("Invalid SCT ID!");
            input.next();
        }
    }

    private void findConceptByTerm() {
        System.out.println("\nFinding concept by term...");
        System.out.println("\nEnter partial term:");
        String term = input.next();

        Collection<Concept> concepts = ConceptFinder.findByTerm(term);
        printConcepts(concepts);
    }

    private void listAllRefsetMembers() {
        System.out.println("\nListing all members of a refset...");
        System.out.println("\nEnter SCT ID of refset:");
        try {

            long sctId = input.nextLong();
            Collection<Concept> concepts = ConceptFinder.findRefsetMembers(sctId);
            printConcepts(concepts);

        } catch (InputMismatchException e) {
            System.out.println("Invalid SCT ID!");
            input.next();
        }
    }

    private void printConcepts(Collection<Concept> concepts) {
        if (concepts == null || concepts.size() == 0) {
            System.out.println("No suitable concepts found!");
        } else {
            String resultCount = String.valueOf(concepts.size());
            if (concepts.size() == ConceptFinder.getResultLimit()) {
                resultCount += " (limited)";
            }
            int conceptNumber = 1;
            for (Concept concept : concepts) {
                System.out.println("Concept " + (conceptNumber++) + " of " + resultCount);
                System.out.println(concept);
            }
        }
    }

    private char getUserSelection() {
        while (true) {
            String selection = input.next();
            if (selection.length() > 0) {
                return selection.charAt(0);
            }
        }
    }

    /**
     * Continually prompt the user with the menu to select an option and run the selected operation.
     */
    public void execute() {

        while (true) {
            printMenu();
            switch (getUserSelection()) {

                case '1' : {
                    findConceptById();
                    break;
                }

                case '2' : {
                    findConceptByTerm();
                    break;
                }

                case '3' : {
                    listAllRefsetMembers();
                    break;
                }

                case 'Q' : case 'q' : {
                    System.exit(0);
                    break;
                }

                default : {
                    System.out.println("Invalid option!");
                }
            }
        }
    }


    public static void main(String ... args) {
    	try {
    		new Main().execute();
    	} catch (OutOfMemoryError e) {
            	System.err.println(
            			"You do not have enough memory available to complete this query. " +
            			"You can increase the maximum memory setting in the application properties file.");
            	System.exit(1);
        }
    }

}
