using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirHockey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }
        // declare stopwatch and soundplayer for easier acess.
        Stopwatch sw = Stopwatch.StartNew();
        SoundPlayer sound = new SoundPlayer();
        
        Rectangle player1 = new Rectangle(223, 470, 45, 45);
        Rectangle player2 = new Rectangle(223, 170, 45, 45);

        Rectangle ball = new Rectangle(240, 326, 15, 15);
        Rectangle divideLine = new Rectangle(0, 329, 488, 3);

        // side borders

        Rectangle leftBorderBlue = new Rectangle(0, 0, 6, 1000);
        Rectangle rightBorderRed = new Rectangle(482, 1, 6, 1000);

        // borders on top

        Rectangle leftTopBorderBlue = new Rectangle(0, 0, 200, 6);
        Rectangle rightTopBorderRed = new Rectangle(244 + 50, 0, 200, 6);

        // borders on bottom

        Rectangle leftBottomBorderBlue = new Rectangle(0, 656, 200, 6);
        Rectangle rightBottomBorderRed = new Rectangle(244 + 50, 656, 200, 6);

        int player1Score = 0;
        int player2Score = 0;


        int playerSpeed = 5;


        int ballXSpeed = 0;
        int ballYSpeed = 0;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;
        bool aLeft = false;
        bool dRight = false;
        bool leftArrowDown = false;
        bool rightArrowDown = false;

        SolidBrush blueBrush = new SolidBrush(Color.DodgerBlue);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush whiteBrush = new SolidBrush(Color.Black);
        Pen whitePen = new Pen(Color.White);
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.A:
                    aLeft = true;
                    break;
                case Keys.D:
                    dRight = true;
                    break;
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.A:
                    aLeft = false;
                    break;
                case Keys.D:
                    dRight = false;
                    break;
                case Keys.Left:
                    leftArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // draw division line first to not overcover the other drawings
            e.Graphics.FillRectangle(whiteBrush, divideLine);

            //e.Graphics.DrawEllipse(whitePen, 0, , 244, 100);

            // draw the pucks
            e.Graphics.FillRectangle(blueBrush, player1);
            e.Graphics.FillRectangle(redBrush, player2);

            //e.Graphics.FillRectangle(blueBrush, player1Top);
            //e.Graphics.FillRectangle(redBrush, player2Top);

            //e.Graphics.FillRectangle(blueBrush, player1Left);
            //e.Graphics.FillRectangle(redBrush, player2Left);

            //e.Graphics.FillRectangle(blueBrush, player1Right);
            //e.Graphics.FillRectangle(redBrush, player2Right);

            //e.Graphics.FillRectangle(blueBrush, player1Bottom);
            //e.Graphics.FillRectangle(redBrush, player2Bottom);

            e.Graphics.FillRectangle(whiteBrush, ball);

            // draw the boundaries
            e.Graphics.FillRectangle(blueBrush, leftBorderBlue);
            e.Graphics.FillRectangle(redBrush, rightBorderRed);
            e.Graphics.FillRectangle(blueBrush, leftTopBorderBlue);
            e.Graphics.FillRectangle(redBrush, rightTopBorderRed);
            e.Graphics.FillRectangle(blueBrush, leftBottomBorderBlue);
            e.Graphics.FillRectangle(redBrush, rightBottomBorderRed);
        }
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // save player and ball values to stasis them when colliding
            int lastP1X = player1.X, lastP1Y = player1.Y, lastP2X = player2.X, lastP2Y = player2.Y;

            // move ball
            ball.X += ballXSpeed;
            ball.Y += ballYSpeed;

            // move player 1

            if (wDown == true && player1.Y > 0 + leftTopBorderBlue.Height)
            {
                player1.Y -= playerSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height - leftTopBorderBlue.Height)
            {
                player1.Y += playerSpeed;
            }

            if (aLeft == true && player1.X > 0 + leftBorderBlue.Width && player1.X > 0 + rightBorderRed.Width)
            {
                player1.X -= playerSpeed;
            }

            if (dRight == true && player1.X < this.Width - player1.Width +
                leftBorderBlue.Width && player1.X < this.Width - player1.Width - rightBorderRed.Width)
            {
                player1.X += playerSpeed;
            }

            // display player1 sides for correct collision

            Rectangle player1Top = new Rectangle(player1.X, player1.Y, 45, 5);

            Rectangle player1Left = new Rectangle(player1.X, player1.Y, 5, 45);

            Rectangle player1Right = new Rectangle(player1.X + player1.Width - 5, player1.Y, 5, 45);

            Rectangle player1Bottom = new Rectangle(player1.X, player1.Y + player1.Height - 5, 45, 5);

            //slow ball if it is going too fast
            //X
            if (ballXSpeed > 10)
            {
                ballXSpeed = 10;
            }
            if (ballXSpeed < -10)
            {
                ballXSpeed = -10;
            }

            // Y
            if (ballYSpeed > 10)
            {
                ballYSpeed = 10;
            }
            if (ballYSpeed < -10)
            {
                ballYSpeed = -10;
            }

            // check if either player is trying to pass the divider line

            if (divideLine.IntersectsWith(player1))
            {
                player1.Y = divideLine.Y + player1.Height;
            }

            if (player2.IntersectsWith(divideLine))
            {
                player2.Y = divideLine.Y - player2.Height;
            }

            // Check If Player 1 Intersects with a part of the ball and add movement accordingly
            if (player1Top.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player1.X = lastP1X;
                player1.Y = lastP1Y;

                // check ball speed and hit ball correctly

                if (ballYSpeed == 0)
                {
                    ballYSpeed += 4;
                }
                {
                    ballYSpeed *= -1;
                    ballYSpeed -= 4;
                    ball.Y = player1Top.Y - ball.Height;

                }
                player1.X += 1;
            }
            if (player1Bottom.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player1.X = lastP1X;
                player1.Y = lastP1Y;

                // check ball speed and hit ball correctly

                if (ballYSpeed == 0)
                {
                    ballYSpeed -= 4;
                }
                {
                    ballYSpeed *= -1;
                    ballYSpeed += 4;
                    ball.Y = player1Bottom.Y + ball.Height;



                }
                player1.Y += 1;

            }
            if (player1Left.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player1.X = lastP1X;
                player1.Y = lastP1Y;

                // check ball speed and hit ball correctly

                if (ballXSpeed == 0)
                {
                    ballXSpeed -= 7;
                }
                else
                {
                    // try to minimise error by replacing the ball infront of the player after collision
                    ball.X = player1Left.X - ball.Width;
                    ballXSpeed *= -1;
                    ballXSpeed -= 4;


                }
                player1.X += 1;

            }

            if (player1Right.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player1.X = lastP1X;
                player1.Y = lastP1Y;

                // check ball speed and hit ball correctly

                if (ballXSpeed == 0)
                {
                    ballXSpeed += 7;
                }
                else
                {
                    // try to minimise error by replacing the ball infront of the player after collision
                    ball.X = player1Right.X + ball.Width;
                    ballXSpeed *= -1;
                    ballXSpeed += 4;

                }
                player1.X -= 1;
            }

            //move player 2 
            if (upArrowDown == true && player2.Y > 0 + leftTopBorderBlue.Height)
            {
                player2.Y -= playerSpeed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height - leftTopBorderBlue.Height)
            {
                player2.Y += playerSpeed;
            }

            if (leftArrowDown == true && player2.X > 0 + leftBorderBlue.Width && player2.X > 0 + rightBorderRed.Width)
            {
                player2.X -= playerSpeed;
            }

            if (rightArrowDown == true && player2.X < this.Width - player2.Width +
                leftBorderBlue.Width && player2.X < this.Width - player2.Width - rightBorderRed.Width)
            {
                player2.X += playerSpeed;
            }

            // display player2 sides for correct collision

            Rectangle player2Top = new Rectangle(player2.X, player2.Y, 45, 5);

            Rectangle player2Left = new Rectangle(player2.X, player2.Y, 5, 45);

            Rectangle player2Right = new Rectangle(player2.X + player2.Width - 5, player2.Y, 5, 45);

            Rectangle player2Bottom = new Rectangle(player2.X, player2.Y + player2.Height - 5, 45, 5);

            // Check If Player 2 Intersects with a part of the ball and add movement accordingly

            if (player2Top.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player2.X = lastP2X;
                player2.Y = lastP2Y;

                // check ball speed and hit ball correctly
                if (ballYSpeed == 0)
                {
                    ballYSpeed += 7;
                }
                {
                    ball.Y = player2Top.Y - ball.Height;
                    ballYSpeed *= -1;
                    ballYSpeed -= 4;

                }
                player2.Y += 1;
            }
            if (player2Bottom.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player2.X = lastP2X;
                player2.Y = lastP2Y;

                // check ball speed and hit ball correctly
                if (ballYSpeed == 0)
                {
                    ballYSpeed -= 7;
                }
                {
                    // try to minimise error by replacing the ball infront of the player after collision
                    ball.Y = player2Bottom.Y + ball.Height;
                    ballYSpeed *= -1;
                    ballYSpeed += 4;


                }
                player2.Y -= 1;
            }
            if (player2Left.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player2.X = lastP2X;
                player2.Y = lastP2Y;

                // check ball speed and hit ball correctly

                if (ballXSpeed == 0)
                {
                    ballXSpeed -= 7;
                }
                else
                {
                    // try to minimise error by replacing the ball infront of the player after collision
                    ball.X = player2Left.X - ball.Width;
                    ballXSpeed *= -1;
                    ballXSpeed -= 4;


                }
                player2.X += 1;
            }
            if (player2Right.IntersectsWith(ball))
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.hit);
                sound.Play();

                //reset player position to before collision

                player2.X = lastP2X;
                player2.Y = lastP2Y;

                // check ball speed and hit ball correctly

                if (ballXSpeed == 0)
                {
                    ballXSpeed += 7;
                }
                else
                {
                    // try to minimise error by replacing the ball infront of the player after collision
                    ball.X = player2Right.X + ball.Width;
                    ballXSpeed *= -1;
                    ballXSpeed += 4;

                }
                player2.X -= 1;
            }

            // check if ball intersected with a boundary
            if (leftBorderBlue.IntersectsWith(ball))
            {
                ballXSpeed *= -1;
                ball.X = ball.Width + leftBorderBlue.Width;
            }
            if ((rightBorderRed.IntersectsWith(ball)))
            {
                ballXSpeed *= -1;
                ball.X = this.Width - ball.Width - rightBorderRed.Width;

            }
            // now check if it hit the top borders (change Y not X)

            if (leftTopBorderBlue.IntersectsWith(ball))
            {
                ballYSpeed *= -1;
                ball.Y = ball.Height + leftTopBorderBlue.Height;
            }
            if (rightTopBorderRed.IntersectsWith(ball))
            {
                ballYSpeed *= -1;
                ball.Y = ball.Height + rightTopBorderRed.Height;
            }
            //now check if ball hit bottom borders (change Y not X)

            if (leftBottomBorderBlue.IntersectsWith(ball))
            {
                ballYSpeed *= -1;
                ball.Y = this.Height - ball.Height - leftBottomBorderBlue.Height;
            }
            if (rightBottomBorderRed.IntersectsWith(ball))
            {
                ballYSpeed *= -1;
                ball.Y = this.Height - ball.Height - rightBottomBorderRed.Height;
            }

            //check  if ball scores

            if (ball.Y < -3)
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.score);
                sound.Play();

                // add score to player 2 and reset ball to middle position

                player1Score++;
                
                player1ScoreLabel.Text = $"{player1Score}";

                //reset ball speed

                ballXSpeed = 0;
                ballYSpeed = 0;

                //reset positions
                player1.X = 223;
                player1.Y = 470;
                player2.X = 223;
                player2.Y = 170;
                ball.Y = 326;
                ball.X = 234;

            }
            else if (ball.Y > this.Height - ball.Height + 5)
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.score);
                sound.Play();

                // add score to player 2 and reset ball to middle position

                player2Score++;
                player2ScoreLabel.Text = $"{player2Score}";

                //reset ball speed

                ballXSpeed = 0;
                ballYSpeed = 0;

                //reset positions
                player1.X = 223;
                player1.Y = 470;
                player2.X = 223;
                player2.Y = 170;
                ball.Y = 326;
                ball.X = 234;
            }

            // add friction

            if (sw.ElapsedMilliseconds % 50 == 0)
            {
                if (ballXSpeed == 0 && ballYSpeed == 0)
                {
                }
                else
                {
                    if (ballXSpeed > 0)
                    {
                        ballXSpeed += -1;
                    }
                    else if (ballXSpeed < 0)
                    {
                        ballXSpeed += 1;
                    }
                    if (ballYSpeed > 0)
                    {
                        ballYSpeed += -1;
                    }
                    else if (ballYSpeed < 0)
                    {
                        ballYSpeed += 1;
                    }
                }
            }


            // check score and stop game if either player is at 3 
            if (player1Score == 3)
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.win);
                sound.Play();

                // stop game and show appropriate win text

                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.ForeColor = Color.Blue;
                winLabel.Text = "Blue Wins!!";
            }
            else if (player2Score == 3)
            {
                //play sound 
                sound = new SoundPlayer(Properties.Resources.win);
                sound.Play();

                // stop game and show appropriate win text

                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.ForeColor = Color.Red;
                winLabel.Text = "Red Wins!!";
            }

            Refresh();

        }
    }

}
