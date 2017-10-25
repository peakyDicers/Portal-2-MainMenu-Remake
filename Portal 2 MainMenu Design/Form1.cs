/*Ian Chui
  June 21, 2015
  Portal 2 Main Menu Design */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AudioPlayer;
using System.Media;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
        //Portal 2 Main Menu
    {
        //============================================================================    //
        Point initialOptionsLocation = new Point(200, 400);                               //
        Size screenSize = new Size(1280, 720);                                            //        Here are all the constants that you can play around with. 
        Size optionsRectSize = new Size(300, 40);                                         //        
        const int numOfOptions = 5;                                                       //
        const int optionsFontSize = 16;                                                   //
        //============================================================================
        AudioFilePlayer[] sndPlayer;
        AudioFilePlayer themePlayer = new AudioFilePlayer();
        Rectangle mousePositionRect;
        Rectangle[] optionsRect;
        Timer selectionTimer;
        Point[] optionsLocation;

        string[] options;
        bool[] sndPlayed;
        bool[] optionSelected;

        PictureBox backgroundGif = new PictureBox();
        Image gifImage = Image.FromFile(Application.StartupPath + @"\GameFiles\portalGif.gif");
        //============================================================================

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Controls.Add(backgroundGif);
            backgroundGif.Location = new Point(0, 0);
            backgroundGif.Size = screenSize;
            backgroundGif.Image = gifImage;
            backgroundGif.SizeMode = PictureBoxSizeMode.StretchImage;
            backgroundGif.Visible = true;
            backgroundGif.Paint += new PaintEventHandler(backgroundGif_Paint);
           
            //set screen settings.
            this.Size = screenSize;
            this.BackColor = Color.Black;
            //============================================================================

            //setting all variables
            options = new string[numOfOptions];
            optionSelected = new bool[numOfOptions];
            optionsLocation = new Point[numOfOptions];
            optionsRect = new Rectangle[numOfOptions];
            sndPlayer = new AudioFilePlayer[numOfOptions];
            sndPlayed = new bool[numOfOptions];
            //============================================================================

            //create seletionTimer.
            selectionTimer = new Timer();
            selectionTimer.Tick += new EventHandler(selectionTimer_Tick);
            selectionTimer.Interval = 1;
            selectionTimer.Start();
            //============================================================================

            //Etc
            createOptions();                                                       
            optionSelected[0] = true;
            mousePositionRect.Size = new Size(1, 1);
            this.DoubleBuffered = true;

            themePlayer.setAudioFile(Application.StartupPath + @"\GameFiles\menuTheme.mp3");
            for (int i = 0; i < numOfOptions; i++)
            {
                sndPlayer[i] = new AudioFilePlayer();
                sndPlayer[i].setAudioFile(Application.StartupPath + @"\GameFiles\mouseOverSnd.mp3");
                sndPlayer[i].stop();
                sndPlayed[i] = true;
            }    
        }
        void createOptions()
        {
            for (int i = 0; i < numOfOptions; i++) //goes through all options.
            {
                options[0] = "PLAY SINGLE PLAYER";
                options[1] = "PLAY COOPERATIVE GAME";
                options[2] = "OPTIONS";
                options[3] = "EXTRAS";
                options[4] = "QUIT";

                optionsLocation[i] = initialOptionsLocation;           //sets on screen location of options.
                optionsLocation[i].Y += i * optionsRectSize.Height;    //start next option under the previous option.

                optionsRect[i].Location = optionsLocation[i];          //sets highlighted option-rectangle locations and size.
                optionsRect[i].Y -= (Font.Height)/2;                   //adjusts gray rectangle so that text is in the middle.
                optionsRect[i].X -= 5;
                optionsRect[i].Size = new Size(optionsRectSize.Width + 5, optionsRectSize.Height - 6);
            }
        }
        void selectionTimer_Tick(object sender, EventArgs e)
        {
            backgroundGif.Invalidate();

            //============================================================================

            //selecting options and playing option sounds
            mousePositionRect.Location = this.PointToClient(MousePosition); //gets location of mouse.
            for (int i = 0; i < numOfOptions; i++)
            {
                if (mousePositionRect.IntersectsWith(optionsRect[i]))   //if the mouse is over an option,
                {                                                       //de-select all options and select hovered option.
                    for (int j = 0; j < numOfOptions; j++)  
                    {
                        optionSelected[j] = false;
                    }
                    optionSelected[i] = true;
                }
                if (optionSelected[i] && !sndPlayed[i]) //if option is selected, and sound hasn't played yet. play the sound.
                {
                    sndPlayer[i].play();
                    sndPlayed[i] = true;
                }

                if (!optionSelected[i]) //allows sound to play if re-hover over option.
                {
                    sndPlayed[i] = false;
                }
            }
            //============================================================================
        } 
        void backgroundGif_Paint(object sender, PaintEventArgs e)
        {
            //get all the painting tools.
            Font drawFont = new Font("arial", optionsFontSize);
            SolidBrush drawBrush = new SolidBrush(Color.Gray);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            //paints the list of options.
            for (int i = 0; i < numOfOptions; i++) //goes through all options.
            {
                e.Graphics.DrawString(options[i], drawFont, drawBrush, optionsLocation[i]); //paints all options on screen.

                if (optionSelected[i]) //if mouse is over option; change font colour to white, and highlight it gray.
                {
                    e.Graphics.FillRectangle(drawBrush, optionsRect[i]);
                    e.Graphics.DrawString(options[i], drawFont, whiteBrush, optionsLocation[i]);
                }
            }
        }
    }
}
