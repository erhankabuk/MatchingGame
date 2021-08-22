using MatchingGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MatchingGame
{
    public partial class Form1 : Form
    {

        #region Variables
        int column = 4, line = 4;//Table size can be change.
        int randomNum, widthOfObject, heightOfObject, quantityOfObjects,
            quantityOfCouple, widthBetweenObjects = 10, score = 0;
        int[] allObjectsNames;
        List<int> randomNumbersList = new List<int>();
        List<PictureBox> clickedObjectList;
        Button btnRetry;
        Random random = new Random();
        #endregion

        public Form1()
        {
            InitializeComponent();
            StartGame();
        }
        void StartGame()
        {
            #region Variables when starting

            pnlCards.Controls.Clear();
            clickedObjectList = new List<PictureBox>();
            quantityOfObjects = column * line;
            widthOfObject = (pnlCards.Width - widthBetweenObjects * (column - 1)) / column;
            heightOfObject = (pnlCards.Height - widthBetweenObjects * (line - 1)) / line;
            quantityOfCouple = 0;
            score = 0;
            #endregion

            CreateRetryButton();

            CreateRandomList();
            
            CreatePictureBox();  
        }
        #region Create object in Picture Box
        void CreatePictureBox()
        {
            PictureBox pb;
            for (int i = 0; i < allObjectsNames.Length; i++)//How to locate all pb objects on pnlCards object
            {
                pb = new PictureBox();
                pb.Left = (i % column) * (widthOfObject + widthBetweenObjects);//Location of pb object from left side of pnlCards object.
                pb.Top = (i / column) * (heightOfObject + widthBetweenObjects);//Location of pb object from top side of pnlCards object.                
                pb.Width = widthOfObject;
                pb.Height = heightOfObject;
                pb.SizeMode = PictureBoxSizeMode.Zoom;//Object calls for pb fits in pb area; 
                pb.BackColor = Color.DarkKhaki;
                pb.Tag = allObjectsNames[i];
                pb.Image = (Image)Resources.ResourceManager.GetObject("help");//How to get image from database.
                pb.Click += WhenClicked;
                pnlCards.Controls.Add(pb);//Object calls with pb will be shown at pnlCards object.
            }

        }
        #endregion

        #region Retry Button Event        
        private void CreateRetryButton()
        {
            btnRetry = new Button();
            btnRetry.Font = new Font("Arial", 24);
            btnRetry.Location = new Point(50, 50);
            btnRetry.Size = new Size(400, 400);
            btnRetry.BackColor = Color.DarkKhaki;
            btnRetry.Hide();
            pnlCards.Controls.Add(btnRetry);
            btnRetry.Click += BtnRetry_Click;
        }
        private void BtnRetry_Click(object sender, EventArgs e)
        {
            btnRetry.Hide();
            StartGame();
        }
        #endregion

        #region Object click event       
        private void WhenClicked(object sender, EventArgs e)
        {
            PictureBox clickedObject = (PictureBox)sender;
            clickedObject.BackColor = Color.LightGray;
            if (clickedObjectList.Contains(clickedObject))
                return;
            if (clickedObjectList.Count == 2)
            {
                score++;
                while (clickedObjectList.Count > 0)
                    HideObject(clickedObjectList[0]);
            }

            ShowObject(clickedObject);

            if (clickedObjectList.Count == 2 && (int)clickedObjectList[0].Tag == (int)clickedObjectList[1].Tag)
            {
                Refresh();
                Thread.Sleep(350);
                while (clickedObjectList.Count > 0)
                {
                    clickedObjectList[0].Hide();
                    HideObject(clickedObjectList[0]);
                    score++;
                }
                quantityOfCouple++;
                if (quantityOfCouple == quantityOfObjects / 2)
                {
                    randomNumbersList.Clear();
                    btnRetry.Text = $"\tGame Over!!!\n\n\tScore : {1000 / score} \n\nRetry?";
                    btnRetry.Show();
                }
            }
        }
        private void ShowObject(PictureBox clickedObject)
        {
            int clickedObjectName = (int)clickedObject.Tag;
            clickedObject.Image = (Image)Resources.ResourceManager.GetObject("_" + clickedObjectName);//How to get image from database.
            clickedObjectList.Add(clickedObject);
        }
        private void HideObject(PictureBox clickedObject)
        {
            clickedObject.BackColor = Color.DarkKhaki;
            int clickedObjectName = (int)clickedObject.Tag;
            clickedObject.Image= (Image)Resources.ResourceManager.GetObject("help");
            //clickedObject.Image = null;
            clickedObjectList.Remove(clickedObject);
        }
        #endregion

        #region Create random list. Convert array and shuffle.
        void CreateRandomList()//Create Random List
        {
            while (randomNumbersList.Count < 263)
            {
                randomNum = random.Next(0, 263);
                if (!randomNumbersList.Contains(randomNum))
                    randomNumbersList.Add(randomNum);
            }
            ShuffleRandomListToArray(randomNumbersList);
        }
        void ShuffleRandomListToArray(List<int> randomNumbersList) //take random list half dublicate List to Array  and shuffle
        {
            //int half = column * line / 2;
            randomNumbersList = randomNumbersList.GetRange(0, quantityOfObjects / 2);
            randomNumbersList.AddRange(randomNumbersList);
            allObjectsNames = randomNumbersList.ToArray();
            ShuffleNumbers(allObjectsNames);
        }
        void ShuffleNumbers(int[] intArray)
        {
            int temp, selectedIndex;
            for (int i = 0; i < intArray.Length - 1; i++)
            {
                selectedIndex = random.Next(i, intArray.Length);
                temp = intArray[i];
                intArray[i] = intArray[selectedIndex];
                intArray[selectedIndex] = temp;
            }
        }
        #endregion
    }
}
