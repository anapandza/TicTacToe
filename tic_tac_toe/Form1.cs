using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac_toe {
    public partial class Form1 : Form
    {
        int movesCounter = 0; // if it comes to 9 its a tie
        string player;
        string computer;
        int computerWins = 0;
        int playerWins = 0;
        int drawCounter = 0;
        string stats;

        public Form1()
        {

            InitializeComponent();
            SetFirstPlayer();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {  // about the game
            MessageBox.Show("Point of the game is to mark three spaces in horizontal, verical or diagonal row.\n" +
                             "Player plays against the computer.\nFirst player is marked with X and second one with O.", "About the game");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        { //exits the game
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        { // starts a new game
            movesCounter = 0;
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;   // all buttons in the form are enabled
                    b.Enabled = true;
                    b.Text = "";
                }
            }
            catch { }
            SetFirstPlayer();
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender; // sender is object that is clicked
            if (b.Text != "") return;  // disables already used button

            if (player == "X")
            {
                b.Text = "X";
                b.ForeColor = Color.Red;
            }
            else
            {
                b.Text = "O";
                b.ForeColor = Color.Blue;
            }
            movesCounter++;

            if (movesCounter == 9)
            {
                DisableButtons();
                drawCounter++;
                var Answer = MessageBox.Show("Nobody won!\nDo you want to play again?", "Tie!", MessageBoxButtons.YesNo);
                if (Answer == DialogResult.Yes)
                {
                    NewGame();
                }
            }
            else
            {
                if (WinnerSearch())
                { // checks if player has won
                    DisableButtons();
                    playerWins++;
                    var Answer = MessageBox.Show("Congradulations, you won!\nDo you want to play again?", "Win!", MessageBoxButtons.YesNo);
                    if (Answer == DialogResult.Yes)
                    {
                        NewGame();
                    }
                }
                else
                {
                    ComputerMove();
                    if (WinnerSearch())
                    {
                        DisableButtons();
                        computerWins++;
                        var Answer = MessageBox.Show("Unforunatly, you lost!\nDo you want to play again?", "Lose!", MessageBoxButtons.YesNo);
                        if (Answer == DialogResult.Yes)
                        {
                            NewGame();
                        }
                    }
                }
            }

        }

        private void DisableButtons()
        {   // all buttons are disabled after the game ends
            try
            {  // chatches exeptions like menu strip
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;   // all the buttons in form are disabled
                    b.Enabled = false;
                }
            }
            catch { }
        }

        private bool WinnerSearch()
        {   // checkes if there is a winner 
            // horizontal checks
            if ((A1.Text != "") && (A1.Text == A2.Text) && (A2.Text == A3.Text))
                return true;
            if ((B1.Text != "") && (B1.Text == B2.Text) && (B2.Text == B3.Text))
                return true;
            if ((C1.Text != "") && (C1.Text == C2.Text) && (C2.Text == C3.Text))
                return true;

            // verical checks
            if ((A1.Text != "") && (A1.Text == B1.Text) && (B1.Text == C1.Text))
                return true;
            if ((A2.Text != "") && (A2.Text == B2.Text) && (B2.Text == C2.Text))
                return true;
            if ((A3.Text != "") && (A3.Text == B3.Text) && (B3.Text == C3.Text))
                return true;

            // diagonal checks
            if ((A1.Text != "") && (A1.Text == B2.Text) && (B2.Text == C3.Text))
                return true;
            if ((C1.Text != "") && (A3.Text == B2.Text) && (B2.Text == C1.Text))
                return true;

            // if there is no winner
            return false;
        }

        private void SetFirstPlayer()
        {  // sets who plays first
            var Answer = MessageBox.Show("Do you want to start first?", "New game?", MessageBoxButtons.YesNo);
            if (Answer == DialogResult.Yes)
            {
                // player is first
                player = "X";
                computer = "O";
            }

            else
            {
                // computer plays first
                player = "O";
                computer = "X";
                ComputerMove();
            }
        }

        private void ComputerMove()
        {
            //1. find winning move
            //2. check if player has winning move
            //3. check if center space is free and take it
            //4. check if corner space is free and take it 
            //5. check other free space

            Button b = null;

            b = WinOrBlockPosition(computer);  // 1. 
            if (b == null)
            {
                b = WinOrBlockPosition(player); // 2. 
                if (b == null)
                {
                    b = FreeCenter(); //3. 
                    if (b == null)
                    {
                        b = FreeCorner(); // 4.
                        if (b == null)
                        {
                            b = OtherFreeSpace(); // 5.
                        }
                    }
                }
            }
            b.Text = computer;
            if (computer == "X")
                b.ForeColor = Color.Red;
            else
                b.ForeColor = Color.Blue;

            movesCounter++;
            if (movesCounter == 9)
            {
                DisableButtons();
                drawCounter++;
                var Answer = MessageBox.Show("Nobody won!\nDo you want to play again?", "Tie!", MessageBoxButtons.YesNo);
                if (Answer == DialogResult.Yes)
                {
                    NewGame();
                }
            }
        }

        private Button WinOrBlockPosition(string mark)
        {  // checks for all combo that could give a winning position
            //horizontal checks
            if ((A1.Text == mark) && (A2.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A2.Text == mark) && (A3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (A3.Text == mark) && (A2.Text == ""))
                return A2;

            if ((B1.Text == mark) && (B2.Text == mark) && (B3.Text == ""))
                return B3;
            if ((B2.Text == mark) && (B3.Text == mark) && (B1.Text == ""))
                return B1;
            if ((B1.Text == mark) && (B3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((C1.Text == mark) && (C2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((C2.Text == mark) && (C3.Text == mark) && (C1.Text == ""))
                return C1;
            if ((C1.Text == mark) && (C3.Text == mark) && (C2.Text == ""))
                return C2;

            //vertical checks
            if ((A1.Text == mark) && (B1.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B1.Text == mark) && (C1.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C1.Text == mark) && (B1.Text == ""))
                return B1;

            if ((A2.Text == mark) && (B2.Text == mark) && (C2.Text == ""))
                return C2;
            if ((B2.Text == mark) && (C2.Text == mark) && (A2.Text == ""))
                return A2;
            if ((A2.Text == mark) && (C2.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B3.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B3.Text == mark) && (C3.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C3.Text == mark) && (B3.Text == ""))
                return B3;

            //diagonal checks
            if ((A1.Text == mark) && (B2.Text == mark) && (C3.Text == ""))
                return C3;
            if ((B2.Text == mark) && (C3.Text == mark) && (A1.Text == ""))
                return A1;
            if ((A1.Text == mark) && (C3.Text == mark) && (B2.Text == ""))
                return B2;

            if ((A3.Text == mark) && (B2.Text == mark) && (C1.Text == ""))
                return C1;
            if ((B2.Text == mark) && (C1.Text == mark) && (A3.Text == ""))
                return A3;
            if ((A3.Text == mark) && (C1.Text == mark) && (B2.Text == ""))
                return B2;

            return null;
        }

        private Button FreeCenter()
        { // checks if center space if available
            if (B2.Text == "") return B2;
            else return null;
        }

        private Button FreeCorner()
        { // checks if corner space if avaiable
            if (A1.Text == "") return A1;
            if (A3.Text == "") return A3;
            if (C1.Text == "") return C1;
            if (C3.Text == "") return C3;
            return null;
        }

        private Button OtherFreeSpace()
        { // checks for any space that is free
            if (A2.Text == "") return A2;
            if (B1.Text == "") return B1;
            if (B3.Text == "") return B3;
            if (C2.Text == "") return C2;
            return null;
        }

        private void NewGame()
        { // same thing as a menu strip only it starts imidiatly after other game
            movesCounter = 0;
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;   // all buttons in the form are enabled
                    b.Enabled = true;
                    b.Text = "";
                }
            }
            catch { }
            SetFirstPlayer();
        }

        private void statsToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("Stats\r\nWins: " + playerWins + "\r\nLoses: " + computerWins
                                     + "\r\nTies: " + drawCounter, "Stats");
        }

        private void resetStatsToolStripMenuItem_Click(object sender, EventArgs e) {
            computerWins = 0;
            playerWins = 0;
            drawCounter = 0;
        }
    }
}
