﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using FlooringMastery.Models;

namespace FlooringMaster.Data
{
    public class TestOrders : IContainOrders
    {


        /// <summary>
        /// Read in multiple orders from a text file
        /// </summary>
        private void LoadOrders()
        {
            using (StreamReader sr = new StreamReader(WorkingMemory.CurrentOrderFile))
                while (!sr.EndOfStream)
                {
                    string WholeOrder = sr.ReadLine();
                    if (!string.IsNullOrEmpty(WholeOrder))
                    {

                        string[] WholeOrderArray = WholeOrder.Split(',');

                        if (WholeOrderArray[0] == "OrderNumber")
                        {
                            WholeOrder = sr.ReadLine();
                            WholeOrderArray = WholeOrder.Split(',');
                        }
                       
                        Order newOrder = new Order();


                        int orderNumber;
                        if (int.TryParse(WholeOrderArray[0], out orderNumber))
                        {
                            newOrder.OrderNumber = orderNumber;
                        }


                        newOrder.CustomerName = WholeOrderArray[1];


                        Enums.StateAbbreviations stateAbbrevs;
                        if (Enums.StateAbbreviations.TryParse(WholeOrderArray[2], out stateAbbrevs))
                        {
                            newOrder.OrderState.StateAbbreviation = stateAbbrevs;
                        }


                        decimal taxRate;
                        if (decimal.TryParse(WholeOrderArray[3], out taxRate))
                        {
                            newOrder.OrderState.TaxRate = taxRate;
                        }


                        newOrder.OrderProduct.ProductType = WholeOrderArray[4];


                        decimal orderArea;
                        if (decimal.TryParse(WholeOrderArray[5], out orderArea))
                        {
                            newOrder.Area = orderArea;
                        }


                        decimal costPerSquareFoot;
                        if (decimal.TryParse(WholeOrderArray[6], out costPerSquareFoot))
                        {
                            newOrder.OrderProduct.CostPerSquareFoot = costPerSquareFoot;
                        }


                        decimal laborCostPerSquareFoot;
                        if (decimal.TryParse(WholeOrderArray[7], out laborCostPerSquareFoot))
                        {
                            newOrder.OrderProduct.LaborCostPerSquareFoot = laborCostPerSquareFoot;
                        }


                        decimal materialCost;
                        if (decimal.TryParse(WholeOrderArray[8], out materialCost))
                        {
                            newOrder.TotalMaterialCost = materialCost;
                        }


                        decimal laborCost;
                        if (decimal.TryParse(WholeOrderArray[9], out laborCost))
                        {
                            newOrder.TotalLaborCost = laborCost;
                        }


                        decimal totalTax;
                        if (decimal.TryParse(WholeOrderArray[10], out totalTax))
                        {
                            newOrder.TotalTax = totalTax;
                        }


                        decimal totalCost;
                        if (decimal.TryParse(WholeOrderArray[11], out totalCost))
                        {
                            newOrder.TotalCost = totalCost;
                        }

                        WorkingMemory.OrderList.Add(newOrder);
                    }

                }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public string LoadOrdersFromFile(string date)
        {
            string properFileName = FileNameBuilder(date);
            if (System.IO.File.Exists(properFileName))
            {
                WorkingMemory.CurrentOrderFile = properFileName;
                LoadOrders();
                return "File was loaded successfully.";
            }
            return "Sorry, there is no file for that date.";
        }


        /// <summary>
        /// given a string, build a valid path and prefix the filename with Orders_ and append .txt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FileNameBuilder(string input)
        {
            string filename = input;
            if (!filename.EndsWith(".txt", true, CultureInfo.CurrentCulture))
            {
                filename = input + ".txt";
            }
            if (!filename.StartsWith(@".\Orders\Orders_"))
            {
                filename = @".\Orders\Orders_" + filename;
            }
            else
            {
                filename = input;
            }
            return filename;
        }



        public void SaveOrdersToFile()
        {
            string lastOrder;

            int orderNumber = 1;
            Console.WriteLine("OrderNumber,CustomerName,State,TaxRate,ProductType,Area,CostPerSquareFoot,LaborCostPerSquareFoot,MaterialCost,LaborCost,Tax,Total");

            foreach (var myOrder in WorkingMemory.OrderList)
            {
                using (StreamWriter sw = new StreamWriter(WorkingMemory.CurrentOrderFile, true))
                {
                    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                        orderNumber,
                        myOrder.CustomerName,
                        myOrder.OrderState.StateAbbreviation,
                        myOrder.OrderState.TaxRate,
                        myOrder.OrderProduct.ProductType,
                        myOrder.Area,
                        myOrder.OrderProduct.CostPerSquareFoot,
                        myOrder.OrderProduct.LaborCostPerSquareFoot,
                        myOrder.TotalMaterialCost,
                        myOrder.TotalLaborCost,
                        myOrder.TotalTax,
                        myOrder.TotalCost);
                }
                orderNumber++;
            }
        }
    }
}
