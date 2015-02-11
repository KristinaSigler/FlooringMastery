﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.BLL;
using FlooringMastery.Models;

namespace FlooringMastery.UI
{
    public static class Input
    {
        /// <summary>
        /// Given a prompt, continually prompt the user until they enter a non-empty string
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static string GetString(string prompt)
        {
            string input = null;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();
                
            } while (string.IsNullOrEmpty(input));
            
            return input;
        }

        /// <summary>
        /// take a string parse it into a date time, format it, and return it as a new string
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static string GetDate(string prompt)
        {
            DateTime dateValue;
            do
            {
                Console.Write(prompt);
                Console.Write(DateTime.Now.ToString("MM/dd/yyyy"));
                Console.SetCursorPosition(prompt.Length,Console.CursorTop
                    );
                
                string dateInput = Console.ReadLine();
                if (String.IsNullOrEmpty(dateInput))
                    dateInput = DateTime.Now.ToString("MM/dd/yyyy");

                if (DateTime.TryParse(dateInput, out dateValue))
                {
                    string newString = dateValue.ToString("MMddyyyy");
                    return newString;
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected date, but got {0}", dateInput);
                }
            } while (true);

        }

        /// <summary>
        /// Given a prompt, continually prompt the user until the enter a non negative decimal then return it
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string prompt)
        {
            string input = null;
            decimal myDecimal = 0;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (decimal.TryParse(input, out myDecimal))
                {
                    //If decimal is positive loop will break
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected decimal, but got {0}", input);
                }

            } while (myDecimal < 0);

            return myDecimal;
        }

        /// <summary>
        /// given a prompt, continually prompt the user until the enter a non negative integer then return it
        /// </summary>
        /// <param name="prompt"></param>
        /// <returns></returns>
        public static int GetInt(string prompt)
        {
            string input = null;
            int myInt = 0;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (int.TryParse(input, out myInt))
                {
                    //If integer is positive loop will break
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected integer, but got {0}", input);
                }

            } while (myInt < 0);

            return myInt;
        }

        /// <summary>
        /// Query user for a new order, return that order.
        /// </summary>
        /// <returns></returns>
        public static Order QueryUserForOrder()
        {
            Order myOrder = new Order();

            StateDictionaryClass aDictionary = new StateDictionaryClass();

            myOrder.CustomerName = (GetString("Please enter the customer name")).ToUpper();

            myOrder.OrderState = GetState();

            myOrder.OrderProduct = GetProduct();

            myOrder.Area = GetDecimal("Please enter the area of the floor");
            
            myOrder = ChangeOrder.CalculateRemainingProperties(myOrder);

            return myOrder;
        }
        

        /// <summary>
        /// Query user for a state that exists on the state list, return that state
        /// </summary>
        /// <returns></returns>
        private static State GetState()
        {
            bool gotState = false;
            //Enums.StateAbbreviations stateAbbrevs;
            string tempState;

            do
            {
                tempState = GetString("Please enter the state abbreviation");

                if (WorkingMemory.StateList.Any(s => s.StateAbbreviation.ToString().Equals(tempState, StringComparison.OrdinalIgnoreCase)))
                {
                    gotState = true;
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected State name, but got {0}", tempState);
                }

            } while (!gotState);
            
            var temp = from s in WorkingMemory.StateList
                       where s.StateAbbreviation.ToString().Equals(tempState, StringComparison.OrdinalIgnoreCase) 
                       select s;

            State myState = new State();
            
            foreach (var s in temp)
            {
                myState = s;
            }
            return myState;
        }

        /// <summary>
        /// Query user for a product that exists on the product list, return that product
        /// </summary>
        /// <returns></returns>
        private static Product GetProduct()
        {
            bool gotProduct = false;
            List<string> myProducts = new List<string>();

            foreach (var s in WorkingMemory.ProductList)
            {
                myProducts.Add(s.ProductType);
            }
            string tempProduct;        

            do
            {
                tempProduct = GetString("Please enter the product name");
                
                if (myProducts.Any(s => s.Equals(tempProduct, StringComparison.OrdinalIgnoreCase)))
                {
                    gotProduct = true;
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected product name, but got {0}", tempProduct);
                }

            } while (!gotProduct);

            Product newProduct = new Product();

            var temp = from p in WorkingMemory.ProductList
                       where p.ProductType.Equals(tempProduct, StringComparison.OrdinalIgnoreCase) 
                select p;

            foreach (var p in temp)
            {
                newProduct = p;
            }
            return newProduct;
        }

        /// <summary>
        /// Ask the user if they want to commit changes, return a bool indicating their choice.
        /// </summary>
        /// <returns></returns>
        public static bool QueryForCommit()
        {
            bool badAnswer = true;

            do
            {
                Console.WriteLine("Would you like to commit changes to file? Y/N");
                string commitAnswer = Console.ReadLine();
                if (commitAnswer.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    commitAnswer.Equals("yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else if (commitAnswer.Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
                    commitAnswer.Equals("no", StringComparison.CurrentCultureIgnoreCase))
                {
                    return false;
                }

                using (System.IO.StreamWriter sw = new StreamWriter("log.txt", true))
                {
                    sw.WriteLine("Expected yes or no, but got {0}", commitAnswer);
                }

            } while (badAnswer);
            return false;
        }
    }
}
