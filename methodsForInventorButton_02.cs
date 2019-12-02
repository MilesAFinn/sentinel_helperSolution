using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
//InventorProcess.methodsForInventorButton;

namespace InventorProcess
{
    public static partial class methodsForInventorButton

    {
        public static void marchThroughCompleteFile(string completeFile, string[,] masterInventorList)
        {
            //int masterTableRows = 100000;
            //int masterTableCols = 16;
            int presentRowNumber = 0;

            //string[,] masterInventorList = new string[masterTableRows, masterTableCols];

            // create the strings we are looking for String startString, record;
            string startString, record;
            int location, numberOfRecords;

            List<int> indiciesOfHits = new List<int>(); // holds the locations of document records
            for (int i = 1; i < 10; i++)
            {
                startString = "US" + i.ToString(); // looking for US2019..., US5123123, etc.
                for (int index = 0; index < completeFile.Length - 1; index++)
                {
                    location = completeFile.IndexOf(startString, index); // did we find a patent number?
                    if (location != -1) // if YES
                    {
                        indiciesOfHits.Add(location); // remember the location.
                        index = location + 1;
                    }
                }
            }

            // sort the indicies (unsorted, they are now in order of "US1", "US2", . . .
            // want them to be in the order found in the file data.

            indiciesOfHits.Sort();
            numberOfRecords = indiciesOfHits.Count;  // give the number of records in file.

            // loop over the records,  computing starting and ending locations
            int start, stop;

            for (int i = 1; i < numberOfRecords + 1; i++)
            {
                // compute where the record starts and stops
                if (i != numberOfRecords)
                {
                    start = indiciesOfHits[i - 1];
                    stop = indiciesOfHits[i];
                }
                else
                {
                    start = indiciesOfHits[i - 1];
                    stop = completeFile.Length - 1;
                }

                try
                {
                    // now that the starting and stopping locations are determined, for the ith record, grab this record.
                    record = completeFile.Substring(start, stop - start - 1); // grab the ith record.

                    //masterInventorList
                    processRecord(record, ref presentRowNumber, masterInventorList);


                }
                catch
                {
                    // bad indicies
                    Console.WriteLine("failure in find of record");
                }
            }

            return;
        }

        public static void processRecord(string record1, ref int presentRowNumber, string[,] masterInventorList)
        // this is processRecord(string record1)
        {

            String patentNo, lastName, firstName, middleName, title, date1, date2, assignee, inventorString, CPCcodes;
            int numberInventors;
            string LFM, LF, LF1; // LFM = Last, First, Middle, LF = last first, LF1 = last and first letter of first.
            string LFM1, LF1M1; // LFM1 = last first middle initial,  LF1M1 = last and first initial and middle initial

            LFM = "";
            LF = ""; LF1 = ""; LFM1 = ""; LF1M1 = "";

            // there should be five/six "|" in here.
            // no -> error
            // yes -> split into fields

            (patentNo, title, date1, date2, assignee, inventorString, CPCcodes) = findFields(record1);

            // split "inventors" into as many full names as there are inventors
            string[] inventorsFullNames;
            inventorsFullNames = inventorString.Split(';');
            numberInventors = inventorsFullNames.Length;


            // create other versions of names

            for (int i = 1; i < numberInventors + 1; i++)
            {
                string fullName = inventorsFullNames[i - 1];
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

                // here we have all the info for one line of the masterInventorTable
                //--------all the name versions for one intentor
                //--------all the info. on one patent publicaiton.
                //
                try
                {
                    masterInventorList[presentRowNumber, 0] = LFM.Trim();
                }
                catch
                {
                    masterInventorList[presentRowNumber, 0] = LFM;
                }

                try
                {
                    masterInventorList[presentRowNumber, 1] = LF.Trim();
                }
                catch
                {
                    masterInventorList[presentRowNumber, 1] = LF; 
                }

                 try
                {
                    masterInventorList[presentRowNumber, 2] = LF1.Trim();
                }
                catch
                {
                    masterInventorList[presentRowNumber, 2] = LF1;
                }

                try
                {
                    masterInventorList[presentRowNumber, 3] = LFM1.Trim();
                }
                catch
                {
                    masterInventorList[presentRowNumber, 3] = LFM1;
                }
            
                try
                {
                    masterInventorList[presentRowNumber, 4] = LF1M1.Trim();
                }
                catch
                {
                    masterInventorList[presentRowNumber, 4] = LF1M1;
                }
;
                masterInventorList[presentRowNumber, 5] = patentNo;
                masterInventorList[presentRowNumber, 6] = title;
                masterInventorList[presentRowNumber, 7] = date1;
                masterInventorList[presentRowNumber, 8] = date2;
                masterInventorList[presentRowNumber, 9] = assignee;
                masterInventorList[presentRowNumber, 10] = inventorString;
                masterInventorList[presentRowNumber, 11] = CPCcodes;

                presentRowNumber += 1;

            }

            return;
        }

        public static (String patentNo, String title, String date1, String date2, String assignee, String inventorString, String CPCcodes) findFields(string record2)
        // this is findFields
        {

            // return (patentNo, title, date1, date2, assignee, inventorString, CPCcodes);
            String patentNo, title, date1, date2, assignee, inventorString, CPCcodes;

            //patentNo = "5123123"; inventorString = "Smith, Tom Q.";

            //how many "|" are there? s.b. 

            string cleanedrecord2 = Regex.Replace(record2, @"\r\n?|\n", "");

            int numberOfVerticalbars = record2.Count(f => f == '|');

            List<int> indiciesOfVerticalbars = new List<int>(); // holds the locations of '|' markers
            string markerString = "|"; // looking for '|'.
            int location;
            int startpoint = 0;
            for (int field = 1; field < 7; field++)
            {
                location = record2.IndexOf(markerString, startpoint); // did we find a '|'?
                if (location != -1) // if YES
                {
                    indiciesOfVerticalbars.Add(location); // remember the location.
                    startpoint = location + 1;
                }
            }


            // given the locaitons of the vertical bars, grab the 6 fields
            // 1 Publication No
            // 2 Title
            // 3 Date 1
            // 4 Date 2
            // 5 Assignee
            // 6 Inventors
            // 7 CPC codes

            patentNo = cleanedrecord2.Substring(2, indiciesOfVerticalbars[0] - 5);
            title = cleanedrecord2.Substring(indiciesOfVerticalbars[0] + 1, indiciesOfVerticalbars[1] - indiciesOfVerticalbars[0] - 1);
            date1 = cleanedrecord2.Substring(indiciesOfVerticalbars[1] + 1, indiciesOfVerticalbars[2] - indiciesOfVerticalbars[1] - 1);
            date2 = cleanedrecord2.Substring(indiciesOfVerticalbars[2] + 1, indiciesOfVerticalbars[3] - indiciesOfVerticalbars[2] - 1);
            assignee = cleanedrecord2.Substring(indiciesOfVerticalbars[3] + 1, indiciesOfVerticalbars[4] - indiciesOfVerticalbars[3] - 1);
            inventorString = cleanedrecord2.Substring(indiciesOfVerticalbars[4] + 2, indiciesOfVerticalbars[5] - indiciesOfVerticalbars[4] - 7);
            CPCcodes = cleanedrecord2.Substring(indiciesOfVerticalbars[5] + 1, cleanedrecord2.Length - indiciesOfVerticalbars[5] - 1);

            return (patentNo, title, date1, date2, assignee, inventorString, CPCcodes);
        }

        public static (String patentNumber, String patentDate11, String inventorNames, String assignee, String patentTitle, String APD, String CPCcodes) getOthersFields(String currentLine)
        {
            // (patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes);

            String patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes;

            patentNumber = "test";
            patentDate11 = "test";
            inventorNames = "test";
            assignee = "test";
            patentTitle = "test";
            APD = "test";
            CPCcodes = "test";

            string cleanedrecord2 = Regex.Replace(currentLine, @"\r\n?|\n", "");
            currentLine = currentLine.ToUpper(); // convert to upper

            int numberOfVerticalbars = currentLine.Count(f => f == '|'); //how many "|" are there? s.b. 26 ?

            List<int> indiciesOfVerticalbars = new List<int>(); // holds the locations of '|' markers
            string markerString = "|"; // looking for '|'.
            int location;
            int startpoint = 0;
            for (int field = 1; field < 27; field++)
            {
                location = currentLine.IndexOf(markerString, startpoint); // did we find a '|'?
                if (location != -1) // if YES
                {
                    indiciesOfVerticalbars.Add(location); // remember the location.
                    startpoint = location + 1;
                }
            }



            patentNumber = cleanedrecord2.Substring(indiciesOfVerticalbars[8] + 3, indiciesOfVerticalbars[9] - indiciesOfVerticalbars[8] -6); // ID
            patentDate11 = cleanedrecord2.Substring(indiciesOfVerticalbars[9] + 4, indiciesOfVerticalbars[10] - indiciesOfVerticalbars[9] - 8); // PRDF?
            inventorNames = cleanedrecord2.Substring(indiciesOfVerticalbars[10] + 1, indiciesOfVerticalbars[11] - indiciesOfVerticalbars[10] - 1); // IN
            assignee = cleanedrecord2.Substring(indiciesOfVerticalbars[15] + 1, indiciesOfVerticalbars[16] - indiciesOfVerticalbars[15] - 1); // PA
            patentTitle = cleanedrecord2.Substring(indiciesOfVerticalbars[19] + 1, indiciesOfVerticalbars[20] - indiciesOfVerticalbars[19] - 1); // TI
            APD = cleanedrecord2.Substring(indiciesOfVerticalbars[20] + 1, indiciesOfVerticalbars[21] - indiciesOfVerticalbars[20] - 1); // APD
            CPCcodes = cleanedrecord2.Substring(indiciesOfVerticalbars[22], indiciesOfVerticalbars[23] - indiciesOfVerticalbars[22] - 1); // CPC

            string lineToWrite;
            string vertBar = Convert.ToString('|');

           
            return (patentNumber, patentDate11, inventorNames, assignee, patentTitle, APD, CPCcodes);
        }



}
}
