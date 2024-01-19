using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace CRUDConsoleApp
{
    class Program
    {
       public static string enviroment = System.Environment.CurrentDirectory;
       public static string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;

        static void Main(string[] args)
        {

            while (true)
            {
                PrintMenu();
                HandleUserInput();
            }
        }


        public static void PrintMenu()
        {
            //Menu options
            Console.WriteLine("1. Create Person");
            Console.WriteLine("2. Read All Persons");
            Console.WriteLine("3. Update Person");
            Console.WriteLine("4. Delete Person");
            Console.WriteLine("5. Generate 10 Random Names");
            Console.WriteLine("6. Return User's Name Alphabetized");
            Console.WriteLine("7. Exit");
            Console.WriteLine("Enter your choice:");
        }

        public static void HandleUserInput()
        {
            string input = Console.ReadLine();

            //Handle user input from console
            switch (input)
            {
                case "1":
                    CreatePerson();
                    break;
                case "2":
                    ReadAllPersons();
                    break;
                case "3":
                    UpdatePerson();
                    break;
                case "4":
                    DeletePerson();
                    break;
                case "5":
                    GenerateNames();
                    break;
                case "6":
                    ReturnAlphabetizedUser();
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }

        public static void CreatePerson()
        {
            Console.WriteLine();
            Console.WriteLine("Enter the person's first name:");
            string firstname = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's last name:");
            string lastname = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's email address:");
            string email = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's phone number:");
            string phonenumber = Console.ReadLine();
            Console.WriteLine();


            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();
                    string sql = "Insert into Person Values(NULL, @firstname, @lastname, @email, @phonenumber)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber);

                        var result = cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

            Console.WriteLine("SUCCESS: Person created");
            Console.WriteLine();
        }

        public static void ReadAllPersons()
        {
            String commandText = "";

            Console.WriteLine();
            Console.WriteLine("Do you want it alphabetized by first name? Y/N");
            string input = Console.ReadLine();

            //Ask user if they want to alphabetize list by first name ASC
            if (input.ToLower() == "y")
            {
                commandText = "SELECT * FROM Person Order by firstName COLLATE NOCASE ASC";
            }
            else
            {
                commandText = "SELECT * FROM Person";
            }

            Console.WriteLine();
            Console.WriteLine("List of Persons:");

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand(commandText, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                Console.WriteLine($"ID: {reader[0]}, Name: {reader[1]} {reader[2]}, Email: {reader[3]}, Phone Number: {reader[4]}");
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

        }

        public static void UpdatePerson()
        {
            bool validID = false;
            string personID = "";

            while (!validID)
            {
                Console.WriteLine();
                Console.WriteLine("Enter ID of person you want to change:");
                personID = Console.ReadLine();
                Console.WriteLine();
                validID = IsValidUser(personID);

                if (!validID)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            Console.WriteLine("Enter the person's first name:");
            string firstname = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's last name:");
            string lastname = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's email address:");
            string email = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Enter the person's phone number:");
            string phonenumber = Console.ReadLine();
            Console.WriteLine();


            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();
                    string sql = "Update Person Set FirstName = @firstname, LastName = @lastname, Email = @email, PhoneNumber = @phonenumber Where PersonID = @personid";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@personid", personID);
                        cmd.Parameters.AddWithValue("@firstname", firstname);
                        cmd.Parameters.AddWithValue("@lastname", lastname);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phonenumber", phonenumber);

                        var result = cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

            Console.WriteLine("SUCCESS: Person updated");
            Console.WriteLine();
        }

        public static void DeletePerson()
        {
            bool validID = false;
            string personID = "";

            while (!validID)
            {
                Console.WriteLine();
                Console.WriteLine("Enter ID of person you want to delete:");
                personID = Console.ReadLine();
                Console.WriteLine();
                validID = IsValidUser(personID);

                if (!validID)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();
                    string sql = "Delete From Person Where PersonID = @personid";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@personid", personID);
                        var result = cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

            Console.WriteLine("SUCCESS: Person deleted");
            Console.WriteLine();
        }

        public static void GenerateNames()
        {

            try
            {
                int count = 1;
                var random = new Random();

                string[] firstNames = { "Michael", "Quentin", "Margaret", "Michelle" };
                string[] lastNames = { "Smih", "Williams", "Scott", "Elliot" };

                string firstName = "";
                string lastName = "";

                Console.WriteLine();
                while (count <= 10)
                {
                    firstName = firstNames[random.Next(0, firstNames.Length)];
                     lastName = firstNames[random.Next(0, firstNames.Length)];
                    Console.WriteLine($"{firstName} {lastName}");
                    count++;
                }
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

            Console.WriteLine("SUCCESS: 10 random names generated");
            Console.WriteLine();
        }

        public static void ReturnAlphabetizedUser()
        {
            bool validID = false;
            string personID = "";

            while (!validID)
            {
                Console.WriteLine();
                Console.WriteLine("Enter ID of person's name you want alphabetized:");
                personID = Console.ReadLine();
                Console.WriteLine();
                validID = IsValidUser(personID);

                if (!validID)
                {
                    Console.WriteLine("Invalid ID");
                }
            }

            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Person Where PersonID = @personid", conn))
                    {

                        cmd.Parameters.AddWithValue("@personid", personID);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                IEnumerable<char> firstAlpha =
                                from first in reader[1].ToString().ToLower()
                                select  first;

                                IEnumerable<char> lastAlpha =
                                from last in reader[2].ToString().ToLower()
                                select last;

                                string firstName = String.Concat(firstAlpha.OrderBy(c => c));
                                string lastName = String.Concat(lastAlpha.OrderBy(x => x));

                                Console.WriteLine();
                                Console.WriteLine($"Alphabetized Name: {firstName} {lastName}");
                                Console.WriteLine();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

        }

        public static bool IsValidUser(string personID)
        {


            try
            {
                using (var conn = new SQLiteConnection($"Data Source={projectDirectory}//Person.db"))
                {
                    conn.Open();

                    using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Person Where PersonID = @personid", conn))
                    {

                        cmd.Parameters.AddWithValue("@personid", personID);

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                return true;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR" + e.StackTrace);
            }

            return false;

        }




        }


    }
