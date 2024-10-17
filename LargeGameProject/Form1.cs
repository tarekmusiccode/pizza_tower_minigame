using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LargeGameProject
{
    public class ThePlayer
    {
        public Rectangle Destination, Source;
        public int CurrentPic = 0, CurrentState = 0, CurrentDirection = 1, X = 1000, Y = 570, HeightJump = 0, BigBulletDelay = 0, DelayHurt = 0;
        public bool Idle = true, Jumped = false, Falling = false, Landed = false, Climbing = false, StartRunning = false, Gunned = false, Shot = false, BigBulletShot = false, DelayBigShot = false, OnBlock = false, GotHurt = false, Dead = false;
        public List<Bitmap> RageIdle = new List<Bitmap>();
        public List<Bitmap> Walk = new List<Bitmap>();
        public List<Bitmap> Ded = new List<Bitmap>();
        public List<Bitmap> Shoot = new List<Bitmap>();
        public List<Bitmap> Fall = new List<Bitmap>();
        public List<Bitmap> Jump = new List<Bitmap>();
        public List<Bitmap> Land = new List<Bitmap>();
        public List<Bitmap> Run = new List<Bitmap>();
        public List<Bitmap> RunStart = new List<Bitmap>();
        public List<Bitmap> PistolGrab = new List<Bitmap>();
        public List<Bitmap> PistolIdle = new List<Bitmap>();
        public List<Bitmap> PistolJump = new List<Bitmap>();
        public List<Bitmap> PistolLand = new List<Bitmap>();
        public List<Bitmap> PistolFall = new List<Bitmap>();
        public List<Bitmap> PistolWalk = new List<Bitmap>();
        public List<Bitmap> LadderClimb = new List<Bitmap>();
        public List<Bitmap> LadderSlide = new List<Bitmap>();
        public Bitmap LadderHanging = new Bitmap("LadderHanging/1.png");
    }

    public class PizzaCutter
    {
        public int X, Y, CurrentPic = 0;
        public List<Bitmap> imgs = new List<Bitmap>();
    }

    public class TheBoss
    {
        public int X = 800, Y = 70, HP = 5;
        public bool ShotLaser = false, isIdle = true;
        public List<Bitmap> Idle = new List<Bitmap>();
        public int CurrentPic = 0, CurrentState = 0, Direction = 0;
    }

    public class Win
    {
        public Rectangle Destination, Source;
        public List<Bitmap> DoneImgs = new List<Bitmap>();
        public int CurrentPic = 0;
    }

    public class HPPlayer
    {
        public int X, Y, CurrentPic = 0;
        public List<Bitmap> HPimgs = new List<Bitmap>();
    }

    public class HPBoss
    {
        public int X, Y, CurrentPic = 0;
        public List<Bitmap> HPimgs = new List<Bitmap>();
    }

    public class FreeGun
    {
        public int X, Y;
        public Bitmap img;
    }

    public class Bullet
    {
        public Rectangle Destination, Source;
        public List<Bitmap> Bulletimgs = new List<Bitmap>();
        public int CurrentPic = -1, Direction = -1;
    }

    public class BigBullet
    {
        public Rectangle Destination, Source;
        public List<Bitmap> BigBulletimgs = new List<Bitmap>();
        public int CurrentPic = -1, Direction = -1;
    }

    public class Block
    {
        public int X, Y;
        public Bitmap img = new Bitmap("Block.bmp");
    }

    public class FlyingLadder
    {
        public int X = 100, Y = 530, CurrentPic = 0;
        public List<Bitmap> FlyingLadderimgs = new List<Bitmap>();
    }

    public class World
    {
        public Rectangle Destination, Source;
        public Bitmap WorldPic;
    }

    public partial class Form1 : Form
    {
        Bitmap off;
        Timer timer = new Timer();
        World world = new World();
        Win WinDone = new Win();
        ThePlayer Peppino = new ThePlayer();
        TheBoss PizzaFace = new TheBoss();
        FlyingLadder flyingladder = new FlyingLadder();
        FreeGun freegun = new FreeGun();
        Random random = new Random();
        List<PizzaCutter> pizzaCutters = new List<PizzaCutter>();
        List<HPPlayer> HPPeppino = new List<HPPlayer>();
        List<HPBoss> HPPizzaFace = new List<HPBoss>();
        List<Block> Blocks = new List<Block>();
        List<Bullet> Bullets = new List<Bullet>();
        List<BigBullet> BigBullets = new List<BigBullet>();
        bool Won = false;
        int CountTimer = 0;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Peppino VS Pizzas";
            Load += Form1_Load;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            timer.Tick += Timer_Tick;
            timer.Interval = 40;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!Won && !Peppino.Dead)
            {
                HPAnimations();
                PeppinoOnBlocks();
                MoveBulllets();
                MoveBigBulllets();
                IdlePeppino();
                PeppinoJump();
                PeppinoFalling();
                PeppinoLand();
                Ladder();
                PizzaCuttersAnimate();
                PizzaFaceMovement();
                PizzaFaceAnimation();
                CheckDamageOnPeppino();
                CheckDamageOnPizzaFace();
                if (Peppino.Gunned && Peppino.CurrentState == 8)
                {
                    Peppino.CurrentPic++;
                    if (Peppino.CurrentPic >= 21)
                    {
                        Peppino.CurrentPic = 0;
                        Peppino.CurrentState = 9;
                    }
                }
                if (Peppino.BigBulletShot)
                {
                    Peppino.BigBulletDelay = -1;
                    Peppino.DelayBigShot = true;
                }
                if (Peppino.DelayBigShot)
                {
                    Peppino.BigBulletDelay++;
                    if (Peppino.BigBulletDelay >= 100) Peppino.DelayBigShot = false;
                }
                if (Peppino.Shot || Peppino.BigBulletShot)
                {
                    Peppino.Shot = Peppino.BigBulletShot = false;
                    if (Peppino.CurrentState == 11) Peppino.CurrentPic = -1;
                    else Peppino.CurrentPic = 0;
                }
                if (Peppino.CurrentState == 11)
                {
                    Peppino.CurrentPic++;
                    if (Peppino.CurrentPic >= 12)
                    {
                        Peppino.CurrentPic = 0;
                        Peppino.CurrentState = 9;
                    }
                }
                if (Peppino.GotHurt)
                {
                    Peppino.DelayHurt++;
                    if (Peppino.DelayHurt >= 50) Peppino.GotHurt = false;
                }
            }
            else if (Won)
            {
                CountTimer++;
                if (CountTimer >= 30) if(WinDone.CurrentPic < 41) WinDone.CurrentPic++;
            }
            else if (Peppino.Dead)
            {
                Peppino.CurrentPic++;
                if (Peppino.CurrentPic >= 6) Peppino.CurrentPic = 0;
            }
            DrawDubb(CreateGraphics());
        }

        void CreateWin()
        {
            WinDone.Destination = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            for (int i = 1;i <= 42;i++) WinDone.DoneImgs.Add(new Bitmap("WinDone/" + i + ".png"));
        }

        void HPAnimations()
        {
            for (int i = 0; i < HPPeppino.Count; i++)
            {
                HPPeppino[i].CurrentPic++;
                if (HPPeppino[i].CurrentPic >= 19) HPPeppino[i].CurrentPic = 0;
            }
            for (int i = 0; i < HPPizzaFace.Count; i++)
            {
                HPPizzaFace[i].CurrentPic++;
                if (HPPizzaFace[i].CurrentPic >= 19) HPPizzaFace[i].CurrentPic = 0;
            }
        }

        void CheckDamageOnPeppino()
        {
            if (HPPeppino.Count >= 1 && !Peppino.GotHurt)
            {
                for (int i = 0; i < pizzaCutters.Count; i++)
                {
                    if (Peppino.X + 50 >= pizzaCutters[i].X && Peppino.X <= pizzaCutters[i].X + pizzaCutters[i].imgs[pizzaCutters[i].CurrentPic].Width && Peppino.Y + 50 >= pizzaCutters[i].Y && Peppino.Y <= pizzaCutters[i].Y + pizzaCutters[i].imgs[pizzaCutters[i].CurrentPic].Height)
                    {
                        HPPeppino.RemoveAt(HPPeppino.Count - 1);
                        Peppino.DelayHurt = 0;
                        Peppino.GotHurt = true;
                        return;
                    }
                }
                if (Peppino.X + 50 >= PizzaFace.X && Peppino.X <= PizzaFace.X + 200 && Peppino.Y + 50 >= PizzaFace.Y && Peppino.Y <= PizzaFace.Y + 200)
                {
                    HPPeppino.RemoveAt(HPPeppino.Count - 1);
                    Peppino.DelayHurt = 0;
                    Peppino.GotHurt = true;
                    return;
                }
                if (PizzaFace.ShotLaser)
                {
                    if (Peppino.Y + 50 >= PizzaFace.Y && Peppino.Y <= PizzaFace.Y + 200 && Peppino.X <= 600)
                    {
                        HPPeppino.RemoveAt(HPPeppino.Count - 1);
                        Peppino.DelayHurt = 0;
                        Peppino.GotHurt = true;
                        return;
                    }
                }
            }
            else if (HPPeppino.Count <= 0)
            {
                Peppino.CurrentPic = 0;
                Peppino.Dead = true;
            }
        }

        void CheckDamageOnPizzaFace()
        {
            if (PizzaFace.isIdle && HPPizzaFace.Count >= 1)
            {
                for (int i = 0; i < Bullets.Count; i++)
                {
                    if (PizzaFace.X <= Bullets[i].Destination.X && PizzaFace.X + PizzaFace.Idle[PizzaFace.CurrentPic].Width >= Bullets[i].Destination.X && PizzaFace.Y <= Bullets[i].Destination.Y && PizzaFace.Y + PizzaFace.Idle[PizzaFace.CurrentPic].Height >= Bullets[i].Destination.Y)
                    {
                        Bullets.RemoveAt(i);
                        if (PizzaFace.HP-- <= 0)
                        {
                            PizzaFace.HP = 5;
                            HPPizzaFace.RemoveAt(HPPizzaFace.Count - 1);
                        }
                        return;
                    }
                }
                for (int i = 0; i < BigBullets.Count; i++)
                {
                    if (PizzaFace.X <= BigBullets[i].Destination.X && PizzaFace.X + PizzaFace.Idle[PizzaFace.CurrentPic].Width >= BigBullets[i].Destination.X && PizzaFace.Y <= BigBullets[i].Destination.Y && PizzaFace.Y + PizzaFace.Idle[PizzaFace.CurrentPic].Height >= BigBullets[i].Destination.Y)
                    {
                        BigBullets.RemoveAt(i);
                        PizzaFace.HP -= 3;
                        if (PizzaFace.HP <= 0)
                        {
                            PizzaFace.HP = 5;
                            HPPizzaFace.RemoveAt(HPPizzaFace.Count - 1);
                        }
                        return;
                    }
                }
            }
            else if (HPPizzaFace.Count <= 0) Won = true;
        }

        void PizzaFaceMovement()
        {
            if (random.Next(0, 25) == 5)
            {
                PizzaFace.ShotLaser = true;
            }
            if (PizzaFace.Direction == 0)
            {
                if (PizzaFace.X <= 40) PizzaFace.Direction = 1;
                PizzaFace.X -= 13;
            }
            else if (PizzaFace.Direction == 1)
            {
                if (PizzaFace.X >= 1300) PizzaFace.Direction = 0;
                PizzaFace.X += 13;
            }
        }

        void PizzaFaceAnimation()
        {
            PizzaFace.CurrentPic++;
            if (PizzaFace.CurrentState == 0) if (PizzaFace.CurrentPic >= 16) PizzaFace.CurrentPic = 0;
            if (PizzaFace.CurrentState == 1) if (PizzaFace.CurrentPic >= 8) PizzaFace.CurrentPic = 0;
            if (PizzaFace.CurrentState == 2) if (PizzaFace.CurrentPic >= 3) PizzaFace.CurrentPic = 0;
            if (PizzaFace.CurrentState == 3) if (PizzaFace.CurrentPic >= 7) PizzaFace.CurrentPic = 0;
        }

        void PizzaCuttersAnimate()
        {
            for (int i = 0;i < pizzaCutters.Count;i++)
            {
                pizzaCutters[i].CurrentPic++;
                if (pizzaCutters[i].CurrentPic >= 3) pizzaCutters[i].CurrentPic = 0;
            }
        }

        void PeppinoOnBlocks()
        {
            int TotalSpace = 0;
            for (int i = 0;i < Blocks.Count; i++) TotalSpace += Blocks[i].img.Width;
            if (!Peppino.Jumped)
            {
                if (Peppino.X + 50 >= Blocks[0].X && Peppino.X <= Blocks[0].X + TotalSpace - 50 && Peppino.Y + 95 >= Blocks[0].Y && Peppino.Y <= Blocks[0].Y)
                {
                    Peppino.Falling = false;
                    Peppino.OnBlock = true;
                    Peppino.Y = 105;
                }
                else if (Peppino.OnBlock)
                {
                    Peppino.OnBlock = false;
                    Peppino.Falling = true;
                    if (Peppino.Gunned) Peppino.CurrentState = 16;
                    else if (!Peppino.Gunned) Peppino.CurrentState = 4;
                }
            }
        }

        void Ladder()
        {
            flyingladder.CurrentPic++;
            if (flyingladder.CurrentPic >= 4) flyingladder.CurrentPic = 0;
            if (Peppino.Climbing)
            {
                if (flyingladder.Y >= 20)
                {
                    Peppino.Y -= 3;
                    flyingladder.Y -= 3;
                }
            }
            else if (flyingladder.Y < 530)
            {
                flyingladder.Y += 10;
            }
        }

        void PeppinoLand()
        {
            if (!Peppino.Falling && Peppino.CurrentState == 4)
            {
                Peppino.CurrentState = 5;
                Peppino.CurrentPic = 0;
                Peppino.Landed = true;
            }
            else if (!Peppino.Falling && Peppino.CurrentState == 16)
            {
                Peppino.CurrentState = 17;
                Peppino.CurrentPic = 0;
                Peppino.Landed = true;
            }
            if (Peppino.Landed && !Peppino.Gunned)
            {
                if (Peppino.CurrentPic < 4) Peppino.CurrentPic++;
                else
                {
                    Peppino.Landed = false;
                    Peppino.Idle = true;
                    Peppino.CurrentPic = 0;
                    Peppino.CurrentState = 0;
                }
            }
            else if (Peppino.Landed && Peppino.Gunned)
            {
                if (Peppino.CurrentPic < 2) Peppino.CurrentPic++;
                else
                {
                    Peppino.Landed = false;
                    Peppino.Idle = true;
                    Peppino.CurrentPic = 0;
                    Peppino.CurrentState = 9;
                }
            }
        }

        void PeppinoFalling()
        {
            if (Peppino.Falling)
            {
                Peppino.Y += 30;
                if (Peppino.CurrentPic < 2) Peppino.CurrentPic++;
                else Peppino.CurrentPic = 0;
                if (Peppino.Y >= 570)
                {
                    Peppino.Y = 570;
                    Peppino.Falling = false;
                }
            }
        }

        void PeppinoJump()
        {
            if (Peppino.Jumped && !Peppino.Gunned)
            {
                Peppino.Y -= 40;
                if (Peppino.CurrentPic < 8 && Peppino.HeightJump < 8)
                {
                    if (Peppino.Shot) Peppino.CurrentPic = 5;
                    Peppino.CurrentPic++;
                    Peppino.HeightJump++;
                }
                else
                {
                    Peppino.Falling = true;
                    Peppino.Jumped = false;
                    Peppino.HeightJump = Peppino.CurrentPic = 0;
                    Peppino.CurrentState = 4;
                }
            }
            else if (Peppino.Jumped && Peppino.Gunned)
            {
                Peppino.Y -= 40;
                if (Peppino.CurrentPic < 12 && Peppino.HeightJump < 12)
                {
                    if (Peppino.Shot) Peppino.CurrentPic = 5;
                    Peppino.CurrentPic++;
                    Peppino.HeightJump++;
                }
                else
                {
                    Peppino.Falling = true;
                    Peppino.Jumped = false;
                    Peppino.HeightJump = Peppino.CurrentPic = 0;
                    Peppino.CurrentState = 16;
                }
            }
        }

        void IdlePeppino()
        {
            if (Peppino.Idle && !Peppino.Climbing)
            {
                Peppino.CurrentPic++;
                if (Peppino.CurrentState == 9 && Peppino.CurrentPic >= 22) Peppino.CurrentPic = 0;
                if (Peppino.CurrentState == 0 && Peppino.CurrentPic >= 27) Peppino.CurrentPic = 0;
            }
        }

        void MoveBulllets()
        {
            for (int i = 0; i < Bullets.Count; i++)//Regular Bullets
            {
                Bullets[i].CurrentPic++;
                if (Bullets[i].CurrentPic >= 5) Bullets[i].CurrentPic = 0;
                if (Bullets[i].Direction == 0)
                {
                    Bullets[i].Destination.X += 30;
                    Bullets[i].Destination = new Rectangle(Bullets[i].Destination.X, Bullets[i].Destination.Y, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Width, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Height);
                    Bullets[i].Source = new Rectangle(0, 0, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Width, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Height);
                }
                else if (Bullets[i].Direction == 1)
                {
                    Bullets[i].Destination.X -= 30;
                    Bullets[i].Destination = new Rectangle(Bullets[i].Destination.X, Bullets[i].Destination.Y, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Width * -1, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Height);
                    Bullets[i].Source = new Rectangle(0, 0, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Width, Bullets[i].Bulletimgs[Bullets[i].CurrentPic].Height);
                }
            }
        }

        void MoveBigBulllets()
        {
            for (int i = 0; i < BigBullets.Count; i++)//Big Bullets
            {
                BigBullets[i].CurrentPic++;
                if (BigBullets[i].CurrentPic >= 4) BigBullets[i].CurrentPic = 0;
                if (BigBullets[i].Direction == 0)
                {
                    BigBullets[i].Destination.X += 30;
                    BigBullets[i].Destination = new Rectangle(BigBullets[i].Destination.X, BigBullets[i].Destination.Y, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Width, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Height);
                    BigBullets[i].Source = new Rectangle(0, 0, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Width, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Height);
                }
                else if (BigBullets[i].Direction == 1)
                {
                    BigBullets[i].Destination.X -= 30;
                    BigBullets[i].Destination = new Rectangle(BigBullets[i].Destination.X, BigBullets[i].Destination.Y, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Width * -1, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Height);
                    BigBullets[i].Source = new Rectangle(0, 0, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Width, BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic].Height);
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && !Peppino.Idle && !Won && !Peppino.Dead)
            {
                Peppino.Idle = true;
                Peppino.StartRunning = false;
                Peppino.CurrentPic = 0;
                if (Peppino.Gunned && !Peppino.Climbing) Peppino.CurrentState = 9;
                else if (!Peppino.Climbing) Peppino.CurrentState = 0;
                else if (Peppino.Climbing) Peppino.CurrentState = 12;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Won && !Peppino.Dead)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        if (!Peppino.Climbing)
                        {
                            if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed)
                            {
                                if (Peppino.Idle)
                                {
                                    Peppino.Idle = false;
                                    Peppino.CurrentPic = 0;
                                }
                                if (Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentState = 10;
                                else if (Peppino.CurrentState != 11) Peppino.CurrentState = 1;
                                Peppino.CurrentPic++;
                                if (Peppino.CurrentPic >= 49 && !Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentPic = 0;
                                else if (Peppino.CurrentPic >= 18 && Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentPic = 0;
                            }
                            else if (!Peppino.Landed) Peppino.X += 30;
                            Peppino.CurrentDirection = 0;
                            if (Peppino.X - world.Source.X <= 700 || world.Source.X + Peppino.X > world.Source.Width) Peppino.X += 15;
                            else world.Source.X += 15;
                        }
                        break;
                    case Keys.A:
                        if (!Peppino.Climbing)
                        {
                            if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed)
                            {
                                if (Peppino.Idle)
                                {
                                    Peppino.Idle = false;
                                    Peppino.CurrentPic = 0;
                                }
                                if (Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentState = 10;
                                else if (Peppino.CurrentState != 11) Peppino.CurrentState = 1;
                                Peppino.CurrentPic++;
                                if (Peppino.CurrentPic >= 49 && !Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentPic = 0;
                                else if (Peppino.CurrentPic >= 18 && Peppino.Gunned && Peppino.CurrentState != 11) Peppino.CurrentPic = 0;
                            }
                            else if (!Peppino.Landed) Peppino.X -= 30;
                            Peppino.CurrentDirection = 1;
                            if (world.Source.X <= 0) Peppino.X -= 15;
                            else world.Source.X -= 15;
                        }
                        break;
                    case Keys.Space:
                        if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && !Peppino.Gunned)
                        {
                            Peppino.CurrentPic = -1;
                            Peppino.CurrentState = 3;
                            Peppino.Jumped = true;
                            Peppino.Idle = Peppino.Climbing = false;
                        }
                        else if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && Peppino.Gunned)
                        {
                            Peppino.CurrentPic = -1;
                            Peppino.CurrentState = 15;
                            Peppino.Jumped = true;
                            Peppino.Idle = Peppino.Climbing = false;
                        }
                        break;
                    case Keys.Right:
                        if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && !Peppino.Gunned && !Peppino.Climbing)
                        {
                            if (Peppino.Idle)
                            {
                                Peppino.Idle = false;
                                Peppino.CurrentPic = 0;
                            }
                            if (!Peppino.StartRunning && Peppino.CurrentState != 6)
                            {
                                Peppino.StartRunning = true;
                                Peppino.CurrentDirection = 0;
                                Peppino.CurrentPic = -1;
                                Peppino.CurrentState = 6;
                            }
                            if (Peppino.StartRunning && Peppino.CurrentPic < 15 && Peppino.CurrentState == 6)
                            {
                                Peppino.CurrentPic++;
                                if (Peppino.X - world.Source.X <= 700 || world.Source.X + Peppino.X > world.Source.Width) Peppino.X += 20;
                                else world.Source.X += 20;
                            }
                            if (Peppino.CurrentPic == 15)
                            {
                                Peppino.CurrentState = 7;
                                Peppino.CurrentPic = -1;
                            }
                            if (Peppino.StartRunning && Peppino.CurrentState == 7)
                            {
                                Peppino.CurrentPic++;
                                if (Peppino.CurrentPic >= 2) Peppino.CurrentPic = 0;
                                if (Peppino.X - world.Source.X <= 700 || world.Source.X + Peppino.X > world.Source.Width) Peppino.X += 30;
                                else world.Source.X += 30;
                            }
                        }
                        break;
                    case Keys.Left:
                        if (!Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && !Peppino.Gunned && !Peppino.Climbing)
                        {
                            if (Peppino.Idle)
                            {
                                Peppino.Idle = false;
                                Peppino.CurrentPic = 0;
                            }
                            if (!Peppino.StartRunning && Peppino.CurrentState != 6)
                            {
                                Peppino.StartRunning = true;
                                Peppino.CurrentDirection = 1;
                                Peppino.CurrentPic = -1;
                                Peppino.CurrentState = 6;
                            }
                            if (Peppino.StartRunning && Peppino.CurrentPic < 15 && Peppino.CurrentState == 6)
                            {
                                Peppino.CurrentPic++;
                                if (world.Source.X <= 0)
                                {
                                    Peppino.X -= 20;
                                    world.Source.X = 0;
                                }
                                else world.Source.X -= 20;
                            }
                            if (Peppino.CurrentPic == 15)
                            {
                                Peppino.CurrentState = 7;
                                Peppino.CurrentPic = -1;
                            }
                            if (Peppino.StartRunning && Peppino.CurrentState == 7)
                            {
                                Peppino.CurrentPic++;
                                if (Peppino.CurrentPic >= 2) Peppino.CurrentPic = 0;
                                if (world.Source.X <= 0)
                                {
                                    Peppino.X -= 30;
                                    world.Source.X = 0;
                                }
                                else world.Source.X -= 30;
                            }
                        }
                        break;
                    case Keys.F:
                        if (Peppino.X + 100 >= freegun.X && Peppino.X <= freegun.X + 50 && Peppino.Y + 100 >= freegun.Y && Peppino.Y <= freegun.Y + 50 && !Peppino.Jumped && !Peppino.Falling && !Peppino.Landed && !Peppino.Gunned && !Peppino.Climbing)
                        {
                            Peppino.CurrentPic = -1;
                            Peppino.CurrentState = 8;
                            Peppino.Gunned = true;
                        }
                        break;
                    case Keys.W:
                        if (Peppino.Climbing)
                        {
                            Peppino.Idle = false;
                            Peppino.Y -= 3;
                            if (Peppino.CurrentState != 13)
                            {
                                Peppino.CurrentState = 13;
                                Peppino.CurrentPic = -1;
                            }
                            Peppino.CurrentPic++;
                            if (Peppino.CurrentPic >= 8) Peppino.CurrentPic = 0;
                        }
                        break;
                    case Keys.S:
                        if (Peppino.Climbing)
                        {
                            Peppino.Idle = false;
                            Peppino.Y += 3;
                            if (Peppino.CurrentState != 14)
                            {
                                Peppino.CurrentState = 14;
                                Peppino.CurrentPic = -1;
                            }
                            Peppino.CurrentPic++;
                            if (Peppino.CurrentPic >= 2) Peppino.CurrentPic = 0;
                        }
                        break;
                    case Keys.Q:
                        if (Peppino.X + 60 >= flyingladder.X && Peppino.X <= flyingladder.X + flyingladder.FlyingLadderimgs[flyingladder.CurrentPic].Width - 20 && !Peppino.Jumped && !Peppino.Climbing && !Peppino.Falling)
                        {
                            Peppino.X = flyingladder.X - world.Source.X - 35;
                            Peppino.Y = flyingladder.Y + 30;
                            Peppino.CurrentPic = -1;
                            Peppino.CurrentState = 12;
                            Peppino.Climbing = true;
                        }
                        break;
                    case Keys.L:
                        if (Peppino.Gunned && !Peppino.Climbing)
                        {
                            Peppino.Shot = true;
                            Peppino.CurrentState = 11;
                            Bullet bullet = new Bullet();
                            bullet.Destination.Y = Peppino.Y;
                            bullet.Direction = Peppino.CurrentDirection;
                            if (bullet.Direction == 0) bullet.Destination.X = Peppino.X + 60 - world.Source.X;
                            if (bullet.Direction == 1) bullet.Destination.X = Peppino.X + 100 - world.Source.X;
                            for (int i = 1; i <= 5; i++)
                            {
                                Bitmap pnn = new Bitmap("Bullet/" + i + ".png");
                                bullet.Bulletimgs.Add(pnn);
                            }
                            Bullets.Add(bullet);
                        }
                        break;
                    case Keys.B:
                        if (Peppino.Gunned && !Peppino.DelayBigShot && !Peppino.Climbing)
                        {
                            Peppino.BigBulletShot = true;
                            Peppino.CurrentState = 11;
                            BigBullet bullet = new BigBullet();
                            bullet.Destination.Y = Peppino.Y;
                            bullet.Direction = Peppino.CurrentDirection;
                            if (bullet.Direction == 0) bullet.Destination.X = Peppino.X + 60 - world.Source.X;
                            if (bullet.Direction == 1) bullet.Destination.X = Peppino.X + 100 - world.Source.X;
                            for (int i = 1; i <= 4; i++)
                            {
                                Bitmap pnn = new Bitmap("BigBullet/" + i + ".png");
                                bullet.BigBulletimgs.Add(pnn);
                            }
                            BigBullets.Add(bullet);
                        }
                        break;
                    case Keys.O:
                        PeppinoOnBlocks();
                        break;
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e) { DrawDubb(e.Graphics); }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            CreateWorld();
            CreateWin();
            CreatePizzaFace();
            CreatePeppino();
            CreateBlocks();
            CreatePizzaCutter();
            CreateHP();
            CreateGun();
            CreateLadder();
            DrawDubb(CreateGraphics());
        }

        void CreatePizzaCutter()
        {
            PizzaCutter pizzaCutter = new PizzaCutter();
            pizzaCutter.X = 300; pizzaCutter.Y = 80;
            for (int i = 1; i <= 3; i++) pizzaCutter.imgs.Add(new Bitmap("PizzaCutter/" + i + ".png"));
            pizzaCutters.Add(pizzaCutter);
            pizzaCutter = new PizzaCutter();
            pizzaCutter.X = 700; pizzaCutter.Y = 530;
            for (int i = 1; i <= 3; i++) pizzaCutter.imgs.Add(new Bitmap("PizzaCutter/" + i + ".png"));
            pizzaCutters.Add(pizzaCutter);
        }

        void CreatePizzaFace()
        {
            for (int i = 1; i <= 16;i++) PizzaFace.Idle.Add(new Bitmap("PizzaFaceIdle/" + i + ".png"));
        }

        void CreateBlocks()
        {
            Block b = new Block();
            for (int i = 0, X = 300;i < 4;i++, X += b.img.Width)
            {
                b.X = X;
                b.Y = 200;
                Blocks.Add(b);
                b = new Block();
            }
        }

        void CreateHP()
        {
            for (int i = 0, X = 30, Y = 20;i < 6;i++, X += 64)
            {
                HPPlayer hPPlayer = new HPPlayer();
                if (i == 3)
                {
                    Y += 64;
                    X = 30;
                }
                hPPlayer.X = X;
                hPPlayer.Y = Y;
                for (int k = 1; k <= 19; k++) hPPlayer.HPimgs.Add(new Bitmap("HPPeppino/" + k + ".png"));
                HPPeppino.Add(hPPlayer);
            }
            for (int i = 0, X = 1200, Y = 20;i < 8;i++, X += 64)
            {
                HPBoss hPboss = new HPBoss();
                if (i == 4)
                {
                    Y += 64;
                    X = 1200;
                }
                hPboss.X = X;
                hPboss.Y = Y;
                for (int k = 1; k <= 19; k++) hPboss.HPimgs.Add(new Bitmap("HPPizzaFace/" + k + ".png"));
                HPPizzaFace.Add(hPboss);
            }
        }

        void CreateLadder()
        {
            for (int i = 1;i <= 4;i++)
            {
                Bitmap pnn = new Bitmap("FlyingLadder/" + i + ".png");
                flyingladder.FlyingLadderimgs.Add(pnn);
            }
        }

        void CreateWorld()
        {
            world.WorldPic = new Bitmap("Background.bmp");
            world.Destination = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            world.Source = new Rectangle(0, 0, world.WorldPic.Width - 150, world.WorldPic.Height);
        }

        void CreateGun()
        {
            freegun.X = 500;
            freegun.Y = 105;
            freegun.img = new Bitmap("Gun.png");
        }

        void CreatePeppino()
        {
            for (int i = 1; i <= 3; i++) Peppino.Run.Add(new Bitmap("Running/" + i + ".png"));
            for (int i = 1; i <= 16; i++) Peppino.RunStart.Add(new Bitmap("RunStart/" + i + ".png"));
            for (int i = 1; i <= 9; i++) Peppino.Jump.Add(new Bitmap("Jump/" + i + ".png"));
            for (int i = 1; i <= 21; i++) Peppino.PistolGrab.Add(new Bitmap("PistolStart/" + i + ".png"));
            for (int i = 1; i <= 22; i++) Peppino.PistolIdle.Add(new Bitmap("PistolIdle/" + i + ".png"));
            for (int i = 1; i <= 18; i++) Peppino.PistolWalk.Add(new Bitmap("WalkPistol/" + i + ".png"));
            for (int i = 1; i <= 5; i++) Peppino.Land.Add(new Bitmap("LandWalk/" + i + ".png"));
            for (int i = 1; i <= 3; i++) Peppino.Fall.Add(new Bitmap("Fall/" + i + ".png"));
            for (int i = 1; i <= 28; i++) Peppino.RageIdle.Add(new Bitmap("IdleRage/" + i + ".png"));
            for (int i = 1; i <= 12; i++) Peppino.Shoot.Add(new Bitmap("Shoot/" + i + ".png"));
            for (int i = 1; i <= 50; i++) Peppino.Walk.Add(new Bitmap("RageWalk/" + i + ".png"));
            for (int i = 1; i <= 8; i++) Peppino.LadderClimb.Add(new Bitmap("LadderClimb/" + i + ".png"));
            for (int i = 1; i <= 2; i++) Peppino.LadderSlide.Add(new Bitmap("LadderSlide/" + i + ".png"));
            for (int i = 1; i <= 13; i++) Peppino.PistolJump.Add(new Bitmap("JumpPistol/" + i + ".png"));
            for (int i = 1; i <= 3; i++) Peppino.PistolLand.Add(new Bitmap("LandPistol/" + i + ".png"));
            for (int i = 1; i <= 3; i++) Peppino.PistolFall.Add(new Bitmap("FallPistol/" + i + ".png"));
            for (int i = 1; i <= 6; i++) Peppino.Ded.Add(new Bitmap("Dead/" + i + ".png"));
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }

        void DrawPeppino(Graphics g)
        {
            if (Peppino.CurrentState == 0) //Idle
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.RageIdle[Peppino.CurrentPic].Width, Peppino.RageIdle[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.RageIdle[Peppino.CurrentPic].Width, Peppino.RageIdle[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.RageIdle[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.RageIdle[Peppino.CurrentPic].Width * -1, Peppino.RageIdle[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.RageIdle[Peppino.CurrentPic].Width, Peppino.RageIdle[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.RageIdle[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 1) //Walking
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Walk[Peppino.CurrentPic].Width, Peppino.Walk[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Walk[Peppino.CurrentPic].Width, Peppino.Walk[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Walk[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Walk[Peppino.CurrentPic].Width * -1, Peppino.Walk[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Walk[Peppino.CurrentPic].Width, Peppino.Walk[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Walk[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 2) //Running
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Run[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Run[Peppino.CurrentPic].Width * -1, Peppino.Run[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Run[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 3) //Jumping
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Jump[Peppino.CurrentPic].Width, Peppino.Jump[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Jump[Peppino.CurrentPic].Width, Peppino.Jump[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Jump[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Jump[Peppino.CurrentPic].Width * -1, Peppino.Jump[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Jump[Peppino.CurrentPic].Width, Peppino.Jump[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Jump[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 4) //Falling
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Fall[Peppino.CurrentPic].Width, Peppino.Fall[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Fall[Peppino.CurrentPic].Width, Peppino.Fall[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Fall[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Fall[Peppino.CurrentPic].Width * -1, Peppino.Fall[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Fall[Peppino.CurrentPic].Width, Peppino.Fall[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Fall[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 5) //Landing
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Land[Peppino.CurrentPic].Width, Peppino.Land[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Land[Peppino.CurrentPic].Width, Peppino.Land[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Land[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Land[Peppino.CurrentPic].Width * -1, Peppino.Land[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Land[Peppino.CurrentPic].Width, Peppino.Land[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Land[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 6) //RunStart
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.RunStart[Peppino.CurrentPic].Width, Peppino.RunStart[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.RunStart[Peppino.CurrentPic].Width, Peppino.RunStart[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.RunStart[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.RunStart[Peppino.CurrentPic].Width * -1, Peppino.RunStart[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.RunStart[Peppino.CurrentPic].Width, Peppino.RunStart[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.RunStart[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 7) //Run
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Run[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Run[Peppino.CurrentPic].Width * -1, Peppino.Run[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Run[Peppino.CurrentPic].Width, Peppino.Run[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Run[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 8) //IntroGunned
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolGrab[Peppino.CurrentPic].Width, Peppino.PistolGrab[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolGrab[Peppino.CurrentPic].Width, Peppino.PistolGrab[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolGrab[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolGrab[Peppino.CurrentPic].Width * -1, Peppino.PistolGrab[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolGrab[Peppino.CurrentPic].Width, Peppino.PistolGrab[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolGrab[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 9) //PistolIdle
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolIdle[Peppino.CurrentPic].Width, Peppino.PistolIdle[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolIdle[Peppino.CurrentPic].Width, Peppino.PistolIdle[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolIdle[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolIdle[Peppino.CurrentPic].Width * -1, Peppino.PistolIdle[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolIdle[Peppino.CurrentPic].Width, Peppino.PistolIdle[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolIdle[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 10) //PistolWalk
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolWalk[Peppino.CurrentPic].Width, Peppino.PistolWalk[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolWalk[Peppino.CurrentPic].Width, Peppino.PistolWalk[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolWalk[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolWalk[Peppino.CurrentPic].Width * -1, Peppino.PistolWalk[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolWalk[Peppino.CurrentPic].Width, Peppino.PistolWalk[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolWalk[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 11) //Shoot
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Shoot[Peppino.CurrentPic].Width, Peppino.Shoot[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Shoot[Peppino.CurrentPic].Width, Peppino.Shoot[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Shoot[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Shoot[Peppino.CurrentPic].Width * -1, Peppino.Shoot[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Shoot[Peppino.CurrentPic].Width, Peppino.Shoot[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Shoot[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 12) //LadderHanging
            {
                Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.LadderHanging.Width, Peppino.LadderHanging.Height);
                Peppino.Source = new Rectangle(0, 0, Peppino.LadderHanging.Width, Peppino.LadderHanging.Height);
                g.DrawImage(Peppino.LadderHanging, Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 13) //LadderClimb
            {
                Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.LadderClimb[Peppino.CurrentPic].Width, Peppino.LadderClimb[Peppino.CurrentPic].Height);
                Peppino.Source = new Rectangle(0, 0, Peppino.LadderClimb[Peppino.CurrentPic].Width, Peppino.LadderClimb[Peppino.CurrentPic].Height);
                g.DrawImage(Peppino.LadderClimb[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 14) //LadderSlide
            {
                Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.LadderSlide[Peppino.CurrentPic].Width, Peppino.LadderSlide[Peppino.CurrentPic].Height);
                Peppino.Source = new Rectangle(0, 0, Peppino.LadderSlide[Peppino.CurrentPic].Width, Peppino.LadderSlide[Peppino.CurrentPic].Height);
                g.DrawImage(Peppino.LadderSlide[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 15) //JumpPistol
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolJump[Peppino.CurrentPic].Width, Peppino.PistolJump[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolJump[Peppino.CurrentPic].Width, Peppino.PistolJump[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolJump[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolJump[Peppino.CurrentPic].Width * -1, Peppino.PistolJump[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolJump[Peppino.CurrentPic].Width, Peppino.PistolJump[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolJump[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 16) //PistolFall
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolFall[Peppino.CurrentPic].Width, Peppino.PistolFall[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolFall[Peppino.CurrentPic].Width, Peppino.PistolFall[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolFall[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolFall[Peppino.CurrentPic].Width * -1, Peppino.PistolFall[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolFall[Peppino.CurrentPic].Width, Peppino.PistolFall[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolFall[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
            if (Peppino.CurrentState == 17) //PistolLand
            {
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolLand[Peppino.CurrentPic].Width, Peppino.PistolLand[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolLand[Peppino.CurrentPic].Width, Peppino.PistolLand[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.PistolLand[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.PistolLand[Peppino.CurrentPic].Width * -1, Peppino.PistolLand[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.PistolLand[Peppino.CurrentPic].Width, Peppino.PistolLand[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.PistolLand[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
        }

        void DrawPizzaFace(Graphics g)
        {
            if (PizzaFace.CurrentState == 0) g.DrawImage(PizzaFace.Idle[PizzaFace.CurrentPic], PizzaFace.X, PizzaFace.Y);
            if (PizzaFace.ShotLaser)
            {
                PizzaFace.ShotLaser = false;
                g.DrawLine(new Pen(Color.Red, 10), 0, PizzaFace.Y + 100, PizzaFace.X + 100, PizzaFace.Y + 100);
            }
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            if (!Won && !Peppino.Dead)
            {
                g.DrawImage(world.WorldPic, world.Destination, world.Source, GraphicsUnit.Pixel);
                g.DrawImage(flyingladder.FlyingLadderimgs[flyingladder.CurrentPic], flyingladder.X - world.Source.X, flyingladder.Y - world.Source.Y);
                if (!Peppino.Gunned) g.DrawImage(freegun.img, freegun.X - world.Source.X, freegun.Y - world.Source.Y);
                for (int i = 0; i < pizzaCutters.Count; i++) g.DrawImage(pizzaCutters[i].imgs[pizzaCutters[i].CurrentPic], pizzaCutters[i].X - world.Source.X, pizzaCutters[i].Y - world.Source.Y);
                for (int i = 0; i < Bullets.Count; i++) g.DrawImage(Bullets[i].Bulletimgs[Bullets[i].CurrentPic], Bullets[i].Destination, Bullets[i].Source, GraphicsUnit.Pixel);
                for (int i = 0; i < BigBullets.Count; i++) g.DrawImage(BigBullets[i].BigBulletimgs[BigBullets[i].CurrentPic], BigBullets[i].Destination, BigBullets[i].Source, GraphicsUnit.Pixel);
                DrawPeppino(g);
                DrawPizzaFace(g);
                for (int i = 0; i < Blocks.Count; i++) g.DrawImage(Blocks[i].img, Blocks[i].X - world.Source.X, Blocks[i].Y - world.Source.Y);
                for (int i = 0; i < HPPeppino.Count; i++) g.DrawImage(HPPeppino[i].HPimgs[HPPeppino[i].CurrentPic], HPPeppino[i].X, HPPeppino[i].Y);
                for (int i = 0; i < HPPizzaFace.Count; i++) g.DrawImage(HPPizzaFace[i].HPimgs[HPPizzaFace[i].CurrentPic], HPPizzaFace[i].X, HPPizzaFace[i].Y);
            }
            else if (Won)
            {
                WinDone.Source = new Rectangle(0, 0, WinDone.DoneImgs[WinDone.CurrentPic].Width, WinDone.DoneImgs[WinDone.CurrentPic].Height);
                g.DrawImage(WinDone.DoneImgs[WinDone.CurrentPic], WinDone.Destination, WinDone.Source, GraphicsUnit.Pixel);
            }
            else if (Peppino.Dead)
            {
                g.Clear(Color.Black);
                if (Peppino.CurrentDirection == 0)
                {
                    Peppino.Destination = new Rectangle(Peppino.X - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Ded[Peppino.CurrentPic].Width, Peppino.Ded[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Ded[Peppino.CurrentPic].Width, Peppino.Ded[Peppino.CurrentPic].Height);
                }
                else if (Peppino.CurrentDirection == 1)
                {
                    Peppino.Destination = new Rectangle(Peppino.X + Peppino.Ded[Peppino.CurrentPic].Width - world.Source.X, Peppino.Y - world.Source.Y, Peppino.Ded[Peppino.CurrentPic].Width * -1, Peppino.Ded[Peppino.CurrentPic].Height);
                    Peppino.Source = new Rectangle(0, 0, Peppino.Ded[Peppino.CurrentPic].Width, Peppino.Ded[Peppino.CurrentPic].Height);
                }
                g.DrawImage(Peppino.Ded[Peppino.CurrentPic], Peppino.Destination, Peppino.Source, GraphicsUnit.Pixel);
            }
        }
    }
}