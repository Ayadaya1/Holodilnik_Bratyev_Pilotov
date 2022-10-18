using System;
using System.Security.Cryptography.Xml;

namespace TestTask
{
    public partial class Form1 : Form
    {
        private int count = 0;
        private int[,] array;
        private int n;
        private Button[,] buttons;
        private List<int[]> solvingList = new List<int[]>();
        private bool solving = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            n = Convert.ToInt32(numericUpDown1.Value);
            if (n > 1 && n<31)
            {
                array = new int[n, n];
                Pilots.Fill(array, n);

                if (buttons != null)
                {
                    foreach (Button b in buttons)
                    {
                        this.Controls.Remove(b);
                    }
                }
                buttons = new Button[n, n];
                int w = (this.Height - 225) / n;
                int h = (this.Height - 225) / n;
                int startingX = this.Width / 2 - w * n / 2;
                int x = startingX;
                int y = 50;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Button b = new Button();
                        b.SetBounds(x, y, w, h);
                        x += w;
                        //b.Click += new EventHandler(PlayButtonClicked(sender,e,array,i,j,n));
                        b.Click += PlayButtonClicked;
                        buttons[i, j] = b;
                    }
                    x = startingX;
                    y += h;
                }
                foreach (Button b in buttons)
                {
                    this.Controls.Add(b);
                }
                ChangeText();
                count = 0;
            }
            else if(n<2)
            {
                MessageBox.Show("Введите число, большее, чем 1!");
            }
            else
            {
                MessageBox.Show("С размером поля больше 30, ячейки будут слишком маленькими, пожалуйста, введите меньшее число!");
            }
        }
        private void PlayButtonClicked(object sender, EventArgs e)
        {
            if (Pilots.Check(array)&&!solving)
            {
                Tuple<int, int> coordinates = Extention.CoordinatesOf<Button>(buttons, sender);
                Pilots.Switch(array, coordinates.Item1 + 1, coordinates.Item2 + 1, n);
                ChangeText();
                count++;

                if (!Pilots.Check(array))
                {  
                    MessageBox.Show(EndGameText(count));
                }
            }
            else if(!solving)
            {
                MessageBox.Show("Вы уже победили, сгенерируйте поле заново!");
            }
        }
        public void ChangeText()
        {
            for(int i = 0; i<n; i++)
            {
                for(int j = 0; j<n; j++)
                {
                    if (array[i, j] == 1)
                    {
                        buttons[i,j].Text = "|";
                        buttons[i, j].BackColor = Color.Cyan;
                    }
                    else
                    {
                        buttons[i, j].Text = "-";
                        buttons[i, j].BackColor = Color.RebeccaPurple;
                    }
                }
            }
        }

        async private void button2_Click(object sender, EventArgs e)
        {
            if (array != null)
            {
                if (!solving && Pilots.Check(array))
                {
                    solving = true;
                    button2.Text = "Стоп";
                    while (solving)
                    {
                        Pilots.Solve(array, n, solvingList);
                        await Task.Delay(180);
                        ChangeText();
                        count++;

                        if (!Pilots.Check(array))
                        {
                            MessageBox.Show(EndGameText(count) + " (или не совсем вы..)");
                            button2.Text = "Решить";
                            solving = false;
                        }
                    }
                }
                else if (solving)
                {
                    solving = false;
                    button2.Text = "Решить";
                    solvingList.Clear();
                }
            }
        }
        private string EndGameText(int count)
        {
            string str;
            str = "Вы победили за " + count.ToString();
            if (count % 10 == 1 && count != 11)
                str += " ход!";
            else if ((count % 10 == 2 || count % 10 == 3 || count % 10 == 4) && count / 10 != 1)
                str += " хода!";
            else
                str += " ходов!";
            return str;
        }

    }



    public static class Extention
    {
        public static Tuple<int, int> CoordinatesOf<T>(this T[,] matrix, object value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }
    }
}