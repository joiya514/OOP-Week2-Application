using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week2Application.BL;
using System.IO;
using System.Threading;

namespace Week2Application
{
    class Program
    {
        static void Main(string[] args)
        {
            char option;
            while (true)
            {
                List<signUp> newUser = new List<signUp>();
                List<adminData> billsData = new List<adminData>();
                readBillData(ref billsData);
                readData(ref newUser);
                Console.Clear();
                header();
                option = signMenu();

                if (option == '1')
                {
                    int index = signIn(newUser);
                    if (newUser[index].role == "admin")
                    {
                        while (true)
                        {
                            char choice = adminMenu();
                            if (choice == '1')
                            {
                                CityBillsFunction(billsData);
                            }
                            if (choice == '2')
                            {
                                char option1 = checkAnyBillFunction();
                                if (option1 == '1')
                                {
                                    string nameBill = billByNameFunction();
                                    showNameBill(billsData, nameBill);
                                }
                                if (option1 == '2')
                                {
                                    string idBill = billByIDFunction();
                                    showIDBill(billsData, idBill);
                                }
                                else
                                {
                                    Console.WriteLine(" Invalid Input!");
                                }
                            }
                            if (choice == '3')
                            {
                                string name = "";
                                string id = "";
                                string bill = "";
                                int y = 0;
                                while (true)
                                {
                                    adminData billData = new adminData();
                                    Console.Clear();
                                    header();
                                    if (y != 0)
                                    {
                                        Console.WriteLine("\n  (Not Valid Name!)");
                                    }
                                    Console.Write("  Enter Name of Person: ");
                                    name = Console.ReadLine();
                                    bool flag = isValidBillName(billsData, billData);
                                    if (flag == true)
                                    {
                                        break;
                                    }
                                    y++;
                                }

                                int x = 0;

                                while (true)
                                {
                                    adminData billData = new adminData();
                                    Console.Clear();
                                    header();
                                    Console.WriteLine("  Name of Person: " + name);
                                    if (x != 0)
                                    {
                                        Console.WriteLine("\n  (Not Valid User ID!)");
                                    }
                                    Console.Write("  Enter Bill ID: ");
                                    id = Console.ReadLine();
                                    bool flag = isValidID(billsData, billData, id);
                                    if (flag == true)
                                    {
                                        x = 0;
                                        break;
                                    }
                                    x++;
                                    Console.WriteLine("      (Checking ID... )");
                                    Thread.Sleep(500);
                                }

                                while (true)
                                {
                                    adminData billData = new adminData();
                                    Console.Clear();
                                    header();
                                    Console.WriteLine("  Name of Person: " + name);
                                    Console.WriteLine("  Bill ID of Person: " + id);
                                    if (x != 0)
                                    {
                                        Console.WriteLine("\n  (Enter Correct Bill Amount!)");
                                    }
                                    Console.Write("  Enter Amount of Bill: ");
                                    bill = Console.ReadLine();
                                    bool flag = isValidBill(billsData, billData, bill);
                                    if (flag == true)
                                    {
                                        break;
                                    }
                                    x++;
                                }

                                addBillDataFunction(name, id, bill);

                            }
                            if (choice == '4')
                            {
                                updateDataFunction(billsData);
                            }
                            if (choice == '5')
                            {
                                deleteDataFunction(billsData);
                            }
                            if (choice == '6')
                            {
                                break;
                            }
                            if (choice == '7')
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("  Invalid User Role!");
                    }
                }
                    if (option == '2')
                    {
                        newUser.Add(addNewUser(newUser));
                    }

                    if (option == '3')
                    {
                        return;
                    }
                
            }
        }

        static void header()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*****************************************************************************************************");
            Console.WriteLine("*****     $                     -(ELECTRIC BILLS MANAGEMENT SYSTEM)-                      $     *****");
            Console.WriteLine("*****************************************************************************************************");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        static char signMenu()
        {
            header();
            Console.WriteLine(" 1: Sign In");
            Console.WriteLine(" 2: Sign Up");
            Console.WriteLine(" 3: Exit");
            Console.Write("  Enter Your Choice: ");
            char choice = char.Parse(Console.ReadLine());
            return choice;
        }

        static int signIn(List<signUp> newUser)
        {
            header();
            string username;
            string passward;
            int index = -1;

            Console.Write(" Enter Username: ");
            username = Console.ReadLine();
            Console.Write(" Enter Passward: ");
            passward = Console.ReadLine();
            for (int x = 0; x < newUser.Count; x++)
            {
                if (newUser[x].userName == username && newUser[x].passward == passward)
                {
                    index = x;
                    break;
                }
            }
            return index;
        }

        static signUp addNewUser(List<signUp> newUser)
        {
            header();
            string username;
            signUp newUsers = new signUp();

            int y = 0;
            while (true)
            {
                header();
                if (y != 0)
                {
                    Console.WriteLine("\n  (Not Valid UserName!)");
                }
                Console.Write(" Enter Username: ");
                username = Console.ReadLine();
                bool flag = isValidName(username, newUser);
                if (flag == true)
                {
                    break;
                }
                y++;
            }
            Console.Write("Enter Passward: ");
            newUsers.passward = Console.ReadLine();
            Console.Write("Enter Role: ");
            newUsers.role = Console.ReadLine();
            addDataFunction(username, newUsers.passward, newUsers.role);
            return newUsers;
        }

        static bool isValidName(string name, List<signUp> newUser)
        {
            for (int x = 0; x < name.Length; x++)
            {
                if ((name[x] < 65) || (name[x] > 90 && name[x] < 97) || (name[x] > 122))
                {
                    return false;
                }
            }
            for (int x = 0; x < newUser.Count; x++)
            {
                if (name == newUser[x].userName)
                {
                    return false;
                }
            }
            return true;
        }

        static void addDataFunction(string userName, string passward, string role)
        {
            string path = "E:\\OOP\\Week 2\\Week2Application\\Week2Application\\signData.txt";
            StreamWriter f1 = new StreamWriter(path, true);
            f1.WriteLine(userName + "," + passward + "," + role);
            f1.Flush();
            f1.Close();
        }

        static void readData(ref List<signUp> newUser)
        {
            string path = "E:\\OOP\\Week 2\\Week2Application\\Week2Application\\signData.txt";
            string record = "";
            if (File.Exists(path))
            {
                StreamReader data = new StreamReader(path);
                while ((record = data.ReadLine()) != null)
                {
                    string fileContents = File.ReadAllText(path);
                    signUp newUsers = new signUp();
                    newUsers.userName = parseData(record, 1);
                    newUsers.passward = parseData(record, 2);
                    newUsers.role = parseData(record, 3);
                    newUser.Add(newUsers);
                }
                data.Close();
            }
            else
            {
                Console.WriteLine("File Not Exists!");
            }
        }

        static string parseData(string record, int field)
        {
            int comma = 1;
            string item = "";
            for (int x = 0; x < record.Length; x++)
            {
                if (record[x] == ',')
                {
                    comma++;
                }
                else if (comma == field)
                {
                    item = item + record[x];
                }
            }
            return item;
        }

        static char adminMenu()
        {
            header();
            Console.WriteLine("  1: Check City Bills");
            Console.WriteLine("  2: Check any person's Bill");
            Console.WriteLine("  3: Add any Person's Data");
            Console.WriteLine("  4: Update someone's Data");
            Console.WriteLine("  5: Delete someone's Data");
            Console.WriteLine("  6: Log Out");
            Console.WriteLine("  7: Quit");
            Console.Write(" Enter Your Choice: ");
            char option = char.Parse(Console.ReadLine());
            return option;
        }

        static void readBillData(ref List<adminData> billsData)
        {
            string path = "E:\\OOP\\Week 2\\Week2Application\\Week2Application\\billsData.txt";
            string record = "";
            if (File.Exists(path))
            {
                StreamReader data = new StreamReader(path);
                while ((record = data.ReadLine()) != null)
                {
                    string fileContents = File.ReadAllText(path);
                    adminData billData = new adminData();
                    billData.names = parseData(record, 1);
                    billData.ids = parseData(record, 2);
                    billData.bills = parseData(record, 3);
                    billsData.Add(billData);
                }
                data.Close();
            }
            else
            {
                Console.WriteLine("File Not Exists!");
            }
        }

        static void CityBillsFunction(List<adminData> billData)
        {
            header();
            Console.WriteLine("\tName\t\t|\t\tBill ID\t\t|\t\tBill Amount\n");
            if (billData.Count > 0)
            {
                for (int x = 0; x < billData.Count; x++)
                {
                    Console.WriteLine("\t" + billData[x].names + "\t\t|\t\t" + billData[x].ids + "\t\t|\t\t" + billData[x].bills);
                }
            }
            else
            {
                Console.WriteLine("  No Results Found!");
                Console.WriteLine(billData.Count);
            }
            Console.ReadKey();
        }
        static bool isValidBillName(List<adminData> billsData, adminData billData)
        {
            bool flag = true;
            for (int x = 0; x < billData.names.Length; x++)
            {

                if ((billData.names[x] < 65) || (billData.names[x] > 90 && billData.names[x] < 97) || (billData.names[x] > 122))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        static bool isValidBill(List<adminData> billsData, adminData billData, string bill)
        {
            bool flag = true;
            bool flag1 = true;
            for (int x = 0; x < billData.bills.Length; x++)
            {
                if (billData.bills[x] < 48 || billData.bills[x] > 57)
                {
                    flag = false;
                    break;
                }
            }
            int bil = int.Parse(bill);

            if (bil < 0)
            {
                flag1 = false;
            }
            if (flag1 == true && flag == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool isValidID(List<adminData> billsData, adminData billData, string id)
        {
            bool flag = true;
            bool flag1 = true;
            for (int x = 0; x < billData.ids.Length; x++)
            {
                if ((billData.ids[x] > 57 && billData.ids[x] < 64) || (billData.ids[x] > 90 && (billData.ids[x] < 97) || (billData.ids[x] > 122) || (billData.ids[x] < 46) || (billData.ids[x] == 47)))
                {
                    flag1 = false;
                    break;
                }
            }
            for (int x = 0; x < billsData.Count; x++)
            {
                if (id == billsData[x].ids)
                {
                    flag = false;
                    break;
                }
            }
            if (flag1 == true && flag == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void addBillDataFunction(string name, string id, string bill)
        {
            string path = "E:\\OOP\\Week 2\\Week2Application\\Week2Application\\BillsData.txt";
            StreamWriter f1 = new StreamWriter(path, true);
            f1.WriteLine(name + "," + id + "," + bill);
            f1.Flush();
            f1.Close();
        }

        static char checkAnyBillFunction()
        {
            header();
            Console.WriteLine("  1: Check Bill by Name");
            Console.WriteLine("  2: Check Bill by ID");
            Console.Write(" Enter Your Choice: ");
            char option = char.Parse(Console.ReadLine());
            return option;
        }

        static string billByNameFunction()
        {
            header();
            Console.Write("  Enter Name Of Person: ");
            string nameBill = Console.ReadLine();
            return nameBill;
        }

        static void showNameBill(List<adminData> billData, string nameBill)
        {
            header();
            int resultCount = 0;
            Console.WriteLine("\tName\t\t|\t\tBill ID\t\t|\t\tBill Amount");
            for (int x = 0; x < billData.Count; x++)
            {
                if (billData[x].names == nameBill)
                {
                    Console.WriteLine("\t" + billData[x].names + "\t\t|\t\t" + billData[x].ids + "\t\t|\t\t" + billData[x].bills);
                }
                else
                {
                    resultCount++;
                }
                if (resultCount == billData.Count)
                {
                    Console.WriteLine(" \n      No Results Found!");
                }
            }
            Console.ReadKey();
        }

        static string billByIDFunction()
        {
            header();
            Console.Write("Enter Bill ID Of Person: ");
            string idBill = Console.ReadLine();
            return idBill;
        }

        static void showIDBill(List<adminData> billData, string idBill)
        {
            header();
            int resultCount = 0;
            Console.WriteLine("Name\t\tBill ID\t\tBill Amount");
            for (int x = 0; x < billData.Count; x++)
            {
                if (billData[x].ids == idBill)
                {
                    Console.WriteLine(billData[x].names + "\t\t" + billData[x].ids + "\t\t" + billData[x].bills);
                }
                else
                {
                    resultCount++;
                }
                if (resultCount == billData.Count)
                {
                    Console.WriteLine("       No Results Found!");
                }
            }
            Console.ReadKey();
        }

        static void deleteDataFunction(List<adminData> billData)
        {
            int count = 0;
            header();
            Console.Write(" Enter Name of Person: ");
            string name = Console.ReadLine();
            Console.Write(" Enter Bill ID of Person: ");
            string id = Console.ReadLine();
            int index = -1;
            for (int x = 0; x < billData.Count; x++)
            {
                if (id == billData[x].ids)
                {
                    index = x;
                    break;
                }
                else
                {
                    count++;
                }
            }

            if (index == -1)
            {
                Console.WriteLine("  No Results Found!");
            }
            if (index != -1)
            {
                if (name == billData[index].names)
                {
                    billData.RemoveAt(index);
                    restoreBillDataFunction(billData);
                }
                else
                {
                    Console.WriteLine("   Invalid Input!");
                }
            }

            Console.ReadKey();
        }

        static void updateDataFunction(List<adminData> billData)
        {
            header();
            char option = updateMenu();
            if (option == '1')
            {
                updateNameFunction(billData);
            }
            if (option == '2')
            {
                updateBillFunction(billData);
            }
            else
            {

            }

        }

        static void restoreBillDataFunction(List<adminData> billData)
        {
            string path = "E:\\OOP\\Week 2\\Week2Application\\Week2Application\\BillsData.txt";
            StreamWriter f1 = new StreamWriter(path, false);
            for (int x = 0; x < billData.Count; x++)
            {
                f1.WriteLine(billData[x].names + "," + billData[x].ids + "," + billData[x].bills);
            }
            f1.Flush();
            f1.Close();
        }
        static char updateMenu()
        {
            header();
            Console.WriteLine(" 1: Update Name of a Person");
            Console.WriteLine(" 2: Update Bill Amount of a Person");
            Console.Write("  Enter Your Choice: ");
            char choice = char.Parse(Console.ReadLine());
            return choice;
        }

        static void updateNameFunction(List<adminData> billData)
        {
            header();
            int resultCount = 0;
            Console.Write("  Enter Name of Person: ");
            string name = Console.ReadLine();
            Console.Write("  Enter Bill ID of Person: ");
            string id = Console.ReadLine();
            for (int x = 0; x < billData.Count; x++)
            {
                if (billData[x].ids == id && billData[x].names == name)
                {
                    string newName = changeNameFunction(billData[x].names);
                    billData[x].names = newName;
                    break;
                }
                else
                {
                    resultCount++;
                }
            }
            restoreBillDataFunction(billData);
        }
        static string changeNameFunction(string name)
        {
            header();
            Console.Write("  Enter New Name: ");
            string newName = Console.ReadLine();
            return newName;
        }

        static void updateBillFunction(List<adminData> billData)
        {
            header();
            int resultCount = 0;
            Console.Write("  Enter Name of Person: ");
            string name = Console.ReadLine();
            Console.Write("  Enter Bill ID of Person: ");
            string id = Console.ReadLine();
            for (int x = 0; x < billData.Count; x++)
            {
                if (billData[x].ids == id && billData[x].names == name)
                {
                    string newBill = changeBillFunction(billData[x].bills);
                    billData[x].bills = newBill;
                    break;
                }
                else
                {
                    resultCount++;
                }
            }
            restoreBillDataFunction(billData);
        }
        static string changeBillFunction(string name)
        {
            header();
            Console.Write("  Enter Updated Amount of Bill: ");
            string newBill = Console.ReadLine();
            return newBill;
        }

    }
}