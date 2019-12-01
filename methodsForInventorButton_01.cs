using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace InventorProcess
{
    public partial class methodsForInventorButton
    {


        public static string readInventorsData()
        {
            String completeFile; // this is the file of inventor document data.
            {
                // Process list of inventors
                // the inventor list is an Excel file that contains one row per client document
                // each row has one or more inventors.
                // We desire to extract a list of unique inventor names, and (later) match up different spellings of same.
                //
                int masterTableRows = 100000;
                int masterTableCols = 16;
                int presentRowNumber = 0;

                string[,] masterInventorList = new string[masterTableRows, masterTableCols];

                var fileName = string.Format("{0}\\clientdocuments-short.csv", Directory.GetCurrentDirectory());

                using (var reader = new StreamReader(fileName))
                {

                    completeFile = File.ReadAllText(fileName);
                    Console.WriteLine(completeFile);

                }
                
            }
            return (completeFile); // return the data.
        }
        public static string readOthersData()
        {
            String lineOfFile; // this is the file of others' document data.
            {

                //

                var fileName = string.Format("{0}\\goodotherdata100-short.csv", Directory.GetCurrentDirectory());

                string patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes;

                using (StreamReader sr = new StreamReader(fileName))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = sr.ReadLine()) != null)
                    {
                        (patentNumber,  patentDate11,  inventorNames,  assignee, patentTitle, APD,  CPCcodes) = getOthersFields(currentLine);
                        //return (patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes);
                        // iterate over inventor names
                        // create other versions of names

                        // split "inventors" into as many full names as there are inventors
                        string[] inventorsFullNames;
                        int numberInventors;

                        inventorsFullNames = inventorNames.Split(';');
                        numberInventors = inventorsFullNames.Length;


                        for (int i = 1; i < numberInventors + 1; i++)
                        {
                            string fullName = inventorsFullNames[i - 1];
                            string lastName, firstName, middleName;
                            //
                            // delete addresses (in parentheses)
                            // delete quote and commas
                            //

                            string regex = "(\\[.*\\])|(\".*\")|('.*')|(\\(.*\\))";
                            string fullName_cleaned = Regex.Replace(fullName, regex, "");

                            fullName_cleaned = Regex.Replace(fullName_cleaned, @",", "");  // remove commas 
                            fullName_cleaned = fullName_cleaned.Trim('"'); // delete quotes
                            fullName_cleaned = fullName_cleaned.Replace(".", string.Empty);  // delete periods
                            fullName_cleaned = fullName_cleaned.Replace("-", string.Empty); // delete hyphens
                            fullName_cleaned = fullName_cleaned.ToUpper(); // convert to upper

                            string[] inventorNameFragments;
                            inventorNameFragments = fullName_cleaned.Split(' ');
                            int numberOfNameFragments = inventorNameFragments.Length;
                            lastName = inventorNameFragments[0];
                            firstName = inventorNameFragments[1];
                            middleName = inventorNameFragments[2];

                            // save flattened list of documents and names


                            string space = Convert.ToString(' ');
                            string LFM, LF, LF1; // LFM = Last, First, Middle, LF = last first, LF1 = last and first letter of first.
                            string LFM1, LF1M1; // LFM1 = last first middle initial,  LF1M1 = last and first initial and middle initial

                            LFM = "";
                            LF = ""; LF1 = ""; LFM1 = ""; LF1M1 = "";
                            LFM = String.Concat(lastName, space, firstName, space, middleName);
                            LF = String.Concat(lastName, space, firstName);
                            LF1 = String.Concat(lastName, space, firstName.Substring(0, 1));
                            try
                            {
                                LFM1 = String.Concat(lastName, space, firstName, space, middleName.Substring(0, 1));
                            }
                            catch
                            {
                                LFM1 = "";
                            }
                            try
                            {
                                LF1M1 = String.Concat(lastName, space, firstName.Substring(0, 1), space, middleName.Substring(0, 1));
                            }
                            catch
                            {
                                LF1M1 = "";
                            }

                            // Is this inventor (an "other" inventor) named in masterInventorList in any way?
                            //

                            //  char temp = masterInventorList[0, 0];
                            //  masterInventorList[presentRowNumber, 1] = LF;
                            //  masterInventorList[presentRowNumber, 2] = LF1;
                            //  masterInventorList[presentRowNumber, 3] = LFM1;
                            //  masterInventorList[presentRowNumber, 4] = LF1M1;
                            //  masterInventorList[presentRowNumber, 5] = patentNo;
                            //  masterInventorList[presentRowNumber, 6] = title;
                            //  masterInventorList[presentRowNumber, 7] = date1;
                            //  masterInventorList[presentRowNumber, 8] = date2;
                            //  masterInventorList[presentRowNumber, 9] = assignee;
                            //  masterInventorList[presentRowNumber, 10] = inventorString;
                            //  masterInventorList[presentRowNumber, 11] = CPCcodes;

                            //  presentRowNumber += 1;

                        }



                    }  // end of while loop.
                }

            }
            lineOfFile = "test";
           return (lineOfFile); // return the data.
        }
    }
}
