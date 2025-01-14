using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Memento.Caretaker;
using Memento.Model;

namespace Memento
{
    public class DocumentMementoTests
    {
        private static void PrintSeparator(string title)
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 50));
        }

        public static void RunAllTests()
        {
            TestBasicFunctionality();
            TestMaxHistorySize();
            TestMultipleUndos();
            TestComplexFormatting();
            TestMetadataOperations();
            TestEmptyDocument();
            TestLargeDocumentChanges();
            TestHistoryNavigation();
            TestStressTest();
        }

        private static void TestBasicFunctionality()
        {
            PrintSeparator("Testing Basic Functionality");

            var document = new Document("TEST-001");
            var history = new DocumentHistory(document);

            Console.WriteLine("Initial empty document:");
            Console.WriteLine(document);

            // Test 1: Basic content update
            document.UpdateContent("First version");
            history.SaveState("Added initial content");
            Console.WriteLine("\nAfter first update:");
            Console.WriteLine(document);

            // Test 2: Add formatting
            document.ApplyFormatting("header", "bold");
            history.SaveState("Added header formatting");
            Console.WriteLine("\nAfter formatting:");
            Console.WriteLine(document);

            // Test 3: Basic undo
            history.Undo();
            Console.WriteLine("\nAfter undo:");
            Console.WriteLine(document);
        }

        private static void TestMaxHistorySize()
        {
            PrintSeparator("Testing Maximum History Size");

            var document = new Document("TEST-002");
            var history = new DocumentHistory(document, maxHistorySize: 3);

            // Add more states than the maximum
            for (int i = 1; i <= 5; i++)
            {
                document.UpdateContent($"Version {i}");
                history.SaveState($"Update {i}");
                Thread.Sleep(100); // Ensure unique timestamps
            }

            Console.WriteLine("Current history (should only show 3 most recent):");
            foreach (var memento in history.GetHistory())
            {
                Console.WriteLine($"- {memento.SavedAt}: {memento.Description}");
            }
        }

        private static void TestMultipleUndos()
        {
            PrintSeparator("Testing Multiple Undos");

            var document = new Document("TEST-003");
            var history = new DocumentHistory(document);

            // Create multiple states
            string[] contents = { "First", "Second", "Third", "Fourth", "Fifth" };
            foreach (var content in contents)
            {
                document.UpdateContent(content);
                history.SaveState($"Added {content}");
            }

            Console.WriteLine("Initial state:");
            Console.WriteLine(document);

            // Perform multiple undos
            for (int i = 0; i < 3; i++)
            {
                history.Undo();
                Console.WriteLine($"\nAfter undo {i + 1}:");
                Console.WriteLine(document);
            }
        }

        private static void TestComplexFormatting()
        {
            PrintSeparator("Testing Complex Formatting");

            var document = new Document("TEST-004");
            var history = new DocumentHistory(document);

            // Add complex formatting
            document.UpdateContent("Title\nParagraph 1\nParagraph 2");
            document.ApplyFormatting("title", "font-size: 24px; font-weight: bold");
            document.ApplyFormatting("p1", "color: blue; margin: 10px");
            history.SaveState("Initial formatting");

            Console.WriteLine("After complex formatting:");
            Console.WriteLine(document);

            // Modify formatting
            document.ApplyFormatting("title", "font-size: 28px; font-weight: bold; color: red");
            document.ApplyFormatting("p2", "font-style: italic");
            history.SaveState("Updated formatting");

            Console.WriteLine("\nAfter formatting update:");
            Console.WriteLine(document);

            // Undo formatting changes
            history.Undo();
            Console.WriteLine("\nAfter undo:");
            Console.WriteLine(document);
        }

        private static void TestMetadataOperations()
        {
            PrintSeparator("Testing Metadata Operations");

            var document = new Document("TEST-005");
            var history = new DocumentHistory(document);

            // Add metadata
            document.UpdateMetadata("author", "John Doe");
            document.UpdateMetadata("created", DateTime.Now.ToString());
            history.SaveState("Initial metadata");

            // Update metadata
            document.UpdateMetadata("author", "Jane Doe");
            document.UpdateMetadata("modified", DateTime.Now.ToString());
            history.SaveState("Updated metadata");

            Console.WriteLine("Current document state:");
            Console.WriteLine(document);

            // Undo metadata changes
            history.Undo();
            Console.WriteLine("\nAfter undo:");
            Console.WriteLine(document);
        }

        private static void TestEmptyDocument()
        {
            PrintSeparator("Testing Empty Document Operations");

            var document = new Document("TEST-006");
            var history = new DocumentHistory(document);

            // Test undo on empty document
            Console.WriteLine("Attempting undo on empty document:");
            bool undoResult = history.Undo();
            Console.WriteLine($"Undo successful: {undoResult}");

            // Save empty state
            history.SaveState("Empty document");
            Console.WriteLine("\nEmpty document state:");
            Console.WriteLine(document);
        }

        private static void TestLargeDocumentChanges()
        {
            PrintSeparator("Testing Large Document Changes");

            var document = new Document("TEST-007");
            var history = new DocumentHistory(document);

            // Create a large content string
            string largeContent = string.Join("\n", Enumerable.Range(1, 100)
                .Select(i => $"This is line {i} of the test document."));

            // Add large content
            document.UpdateContent(largeContent);
            history.SaveState("Added large content");

            Console.WriteLine("Large document summary:");
            Console.WriteLine($"Content length: {largeContent.Length} characters");
            Console.WriteLine("First 100 characters:");
            Console.WriteLine(largeContent.Substring(0, 100) + "...");

            // Modify part of the content
            string modifiedContent = largeContent.Replace("line", "row");
            document.UpdateContent(modifiedContent);
            history.SaveState("Modified large content");

            // Undo
            history.Undo();
            Console.WriteLine("\nAfter undo - verifying content restored correctly...");
            Console.WriteLine($"Content matches original: {document.ToString().Contains("line 1")}");
        }

        private static void TestHistoryNavigation()
        {
            PrintSeparator("Testing History Navigation");

            var document = new Document("TEST-008");
            var history = new DocumentHistory(document);

            // Create several states with different types of changes
            document.UpdateContent("Version 1");
            document.ApplyFormatting("style1", "bold");
            history.SaveState("State 1");

            document.UpdateContent("Version 2");
            document.UpdateMetadata("status", "draft");
            history.SaveState("State 2");

            document.UpdateContent("Version 3");
            document.ApplyFormatting("style2", "italic");
            document.UpdateMetadata("status", "review");
            history.SaveState("State 3");

            Console.WriteLine("Full history:");
            foreach (var memento in history.GetHistory())
            {
                Console.WriteLine($"- {memento.SavedAt}: {memento.Description}");
            }

            // Navigate through history
            Console.WriteLine("\nNavigating through history:");
            while (history.Undo())
            {
                Console.WriteLine("\nAfter undo:");
                Console.WriteLine(document);
            }
        }

        private static void TestStressTest()
        {
            PrintSeparator("Stress Testing");

            var document = new Document("TEST-009");
            var history = new DocumentHistory(document, maxHistorySize: 100);

            Console.WriteLine("Performing multiple rapid changes...");

            // Perform many rapid changes
            for (int i = 0; i < 50; i++)
            {
                document.UpdateContent($"Content version {i}");
                document.ApplyFormatting($"style{i}", $"property{i}");
                document.UpdateMetadata($"meta{i}", $"value{i}");
                history.SaveState($"Change {i}");
            }

            Console.WriteLine($"Total states in history: {history.GetHistory().Count}");

            // Perform rapid undos
            Console.WriteLine("\nPerforming rapid undos...");
            int undoCount = 0;
            while (history.Undo())
            {
                undoCount++;
            }
            Console.WriteLine($"Successfully performed {undoCount} undos");
        }

        public static void Main()
        {
            Console.WriteLine("Starting Document Memento Pattern Tests\n");
            RunAllTests();
            Console.WriteLine("\nAll tests completed!");
        }
    }
}