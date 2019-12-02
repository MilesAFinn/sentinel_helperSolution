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
        private static StreamWriter fileOut;

        enum suspicionLevel
        {
            Low,
            Medium,
            High
        }

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
        public static string readOthersData(string[,] masterInventorList)
        {
            String lineOfFile; // this is the file of others' document data.
            {
                //string temp = masterInventorList[0, 0];  // this works! Can see mIL.


                //var fileName = string.Format("{0}\\goodotherdata100-short.csv", Directory.GetCurrentDirectory());
                var fileName = string.Format("{0}\\goodotherdata100-fake.csv", Directory.GetCurrentDirectory());

                string patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes;

                using (StreamReader sr = new StreamReader(fileName))
                {
                    string currentLine;
                    // currentLine will be null when the StreamReader reaches the end of file

                    string[,] masterRiskList = new string[10000, 24]; // store others' docouments of concern
                    int concernedEntryCounter = 0;
                    
                    string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    //fileOut = new System.IO.StreamWriter("out1.txt");

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
                        bool somethingToWrite = false;

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
                            try
                            {
                                firstName = inventorNameFragments[1];
                            }
                            catch
                            {
                                firstName = "";
                            }
                            try
                            {
                                middleName = inventorNameFragments[2];
                            }
                            catch
                            {
                                middleName = "";
                            }

                            // DETERMINE NAME OPTIONS FOR THIS "OTHER" DOCUMENT.

                            string space = Convert.ToString(' ');
                            string LFM, LF, LF1; // LFM = Last, First, Middle, LF = last first, LF1 = last and first letter of first.
                            string LFM1, LF1M1; // LFM1 = last first middle initial,  LF1M1 = last and first initial and middle initial

                            LFM = String.Concat(lastName, space, firstName, space, middleName);
                            LF = String.Concat(lastName, space, firstName);

                            try { 
                                LF1 = String.Concat(lastName, space, firstName.Substring(0, 1));
                            }
                            catch 
                                {
                                LF1 = lastName;
                                }

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
                            // Putting it another way:
                            //     1. earlier, we read a group of "our" patent documents
                            //     2. from "our" list, we created masterInventorList[i, j]
                            //     3. mIL contains various spellings of our inventor names, and bibliographic data from their patents
                            //     4. the comments below show where this info is located in mIL (LF1 - e.g., means Last name + Initial of first name.)

                            //  masterInventorList[presentRowNumber, 0] = LFM;
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
                            //
                            //      5. At this point (in the instant method -- readOthersData -- we have one row of a record of others'
                            //          patent publications in a relevant time period and CPC group.
                            //      6. What we want to determine now -- DO ANY OF "OUR" INVENTORS NAMES "MATCH" THE NAME OF THE PRESENT (OTHER) PUBLICATION?
                            //          IF YES, THEN FLAG THIS OTHER RECORD AS A POSSIBLE THEFT FROM US.
                            //

                            // Iterate through masterInventorList, looking for matches.

 

                            suspicionLevel thisSuspicion;
                            thisSuspicion = suspicionLevel.Low;

                            

                            for (int j = 0; j < 15; j++) // to do -- need to set "1000" correctly
                            {
                                string temp1 = masterInventorList[1, 0];
                                string ourName, concerningName;
                                ourName = LFM.Trim();
                                concerningName = masterInventorList[j, 0];
                                try
                                {
                                    concerningName = concerningName.Trim();
                                }
                                catch
                                {
                                    concerningName = "nooneMatchesThis";
                                }

                                if (ourName == concerningName)
                                {
                                    // match of complete name -- classify as highly suspicous
                                    thisSuspicion = suspicionLevel.High;
                                    masterRiskList[concernedEntryCounter, 0] = thisSuspicion.ToString();
                                    masterRiskList[concernedEntryCounter, 1] = patentNumber;
                                    masterRiskList[concernedEntryCounter, 2] = patentDate11;
                                    masterRiskList[concernedEntryCounter, 3] = inventorNames;
                                    masterRiskList[concernedEntryCounter, 4] = assignee;
                                    masterRiskList[concernedEntryCounter, 5] = patentTitle;
                                    masterRiskList[concernedEntryCounter, 6] = APD;
                                    masterRiskList[concernedEntryCounter, 7] = CPCcodes;

                                    //to do - there are many more fields to store

                                }
                                else if (thisSuspicion != suspicionLevel.High && ((LF.Trim() == masterInventorList[j, 1])) || (LF1.Trim() == masterInventorList[j, 2]))
                                {
                                    // match of last and first names -- classify as moderately suspicous
                                    thisSuspicion = suspicionLevel.Medium;
                                    masterRiskList[concernedEntryCounter, 1] = thisSuspicion.ToString();
                                    //to do - there are many more fields to store
                                }
                            }

                            if (thisSuspicion.ToString() != suspicionLevel.Low.ToString()) // have we logged this "other" publication?
                            {
                                // we have stored this publication as suspicious, so
                                                            // increment the pointer for that table
                                                            // if there is something to write, write it.
                                string vertBar = Convert.ToString('|');
                                
                                
                                string lineToWrite;

                                string a, b, d, c, e, f, g, h;
                                a = masterRiskList[concernedEntryCounter, 0];
                                b = masterRiskList[concernedEntryCounter, 1];
                                c = masterRiskList[concernedEntryCounter, 2];
                                d = masterRiskList[concernedEntryCounter, 3];
                                e = masterRiskList[concernedEntryCounter, 4];
                                f = masterRiskList[concernedEntryCounter, 5];
                                g = masterRiskList[concernedEntryCounter, 6];
                                h = masterRiskList[concernedEntryCounter, 7];
                                lineToWrite = String.Concat(a, vertBar, b, vertBar, c, vertBar, d, vertBar, e, vertBar, f, vertBar, g, vertBar, h);
                                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "out2.txt"), true))
                                {
                                    outputFile.WriteLine(lineToWrite);
                                }
                                concernedEntryCounter += 1;

                            }

                        }


                        

                    }  // end of while loop.
                }

            }
            lineOfFile = "test";
           return (lineOfFile); // return the data.
        }
    }
}
