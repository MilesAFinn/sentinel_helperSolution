// Process inventor information for Sentinel
// (C) Miles A. Finn, 2019.
// All rights reserved.

using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

using static InventorProcess.methodsForInventorButton;
using static InventorProcess.methodsForJSON;
//using DocumentFormat.OpenXml.Office.CustomUI;

namespace InventorProcess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        //int masterTableRows = 100000;
        //int masterTableCols = 16;
        int presentRowNumber = 0;

        string[,] masterInventorList = new string[100000, 16];


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string inventorData;

            inventorData = readInventorsData(); // read file containing documents information for our inventors.
            marchThroughCompleteFile(inventorData, masterInventorList); // process the inventor information (lots of attention to name parts)

            //string temp = masterInventorList[0, 0];  // this works! Can see mIL.

        }


        private void JSONbtn_Click(object sender, RoutedEventArgs e)
        {
            string othersData;

            othersData = readOthersData(masterInventorList); // read file containing documents information for others.
            //marchThroughCompleteFile(inventorData); // process the inventor information (lots of attention to name parts)
        }
    }
}
