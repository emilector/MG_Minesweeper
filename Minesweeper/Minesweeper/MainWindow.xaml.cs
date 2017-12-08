using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            m_timer.Interval = new TimeSpan(0, 0, 1);
            m_timer.Tick += timer_tick;
        }

        List<TextBlock> m_recs = new List<TextBlock>();
        List<int> m_mines = new List<int>();
        Random m_rnd = new Random();
        int m_countMines;
        SolidColorBrush m_blue = new SolidColorBrush(Colors.Blue);
        SolidColorBrush m_black = new SolidColorBrush(Colors.Black);
        DispatcherTimer m_timer = new DispatcherTimer();
        int time = 0;
        bool firstClick = true;

        // Start
        private void start_Click(object sender, RoutedEventArgs e)
        {
            time = 0;
            m_countMines = 100;
            m_timer.Start();
            createPanels();
            setMines();
        }

        // Start Aufdecken
        void aufdecken(int index)
        {
            int i = 0;
            Random r = new Random(); 

            while (i < 10)
            {
                if (!m_mines.Contains(index))
                {
                    m_recs[index].Background = new SolidColorBrush(Colors.Green);
                    setNumberofMines(index, m_recs[index]);

                    index = index + r.Next(-1, 1);
                    
                    m_recs[index].Background = new SolidColorBrush(Colors.Green);
                    setNumberofMines(index, m_recs[index]);

                    index = index + r.Next(-27, 29);

                    i++;
                }
            }
        }

        // Timer tick
        private void timer_tick(object sender, EventArgs e)
        {
            time++;
            timeT.Text = "Time: " + time + " sec";
        }

        // Spielfläche generieren
        private void createPanels()
        {
            wrapPanelT.Children.Clear();
            m_recs.Clear();

            for (int i = 0; i < 448; i++)
            {
                TextBlock rec = new TextBlock();
                rec.Width = 15;
                rec.Height = 15;
                rec.Background = m_black;
                rec.MouseLeftButtonDown += new MouseButtonEventHandler(this.recMouseDown);
                rec.MouseRightButtonDown += new MouseButtonEventHandler(this.recRightMouseDown);

                m_recs.Add(rec);
                wrapPanelT.Children.Add(rec);

                Border b = new Border();
                b.Width = 1;
                b.Height = 1;
                b.BorderBrush = new SolidColorBrush(Colors.Black);
                wrapPanelT.Children.Add(b);
            }
        }

        // Rechts Klick
        private void recRightMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock rec = (TextBlock)sender;

            if (rec.Background == m_blue)
            {
                rec.Background = m_black;

                m_countMines++;
                minesT.Text = "Hidden mines: " + m_countMines;
            }
            else
            {
                rec.Background = m_blue;

                m_countMines--;
                minesT.Text = "Hidden mines: " + m_countMines;
            }
        }

        // Links Klick
        private void recMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock rec = (TextBlock)sender;
            int index = m_recs.IndexOf(rec);

            if (firstClick == true)
            {
                //aufdecken(index);
                firstClick = false;
            }

            if (m_mines.Contains(index))
            {
                for (int i = 0; i < m_mines.Count; i++)
                {
                    m_recs[m_mines[i]].Background = new SolidColorBrush(Colors.Red);
                }

                m_timer.Stop();
                MessageBox.Show("Game Over!");
            }
            else
            {
                rec.Background = new SolidColorBrush(Colors.Green);
                setNumberofMines(index, rec);
            }
        }

        // Minen in Umgebung berechnen
        private void setNumberofMines(int index, TextBlock rec)
        {
            int surroundingMines = 0;

            if(!leftSide(index))
            {
                if (m_mines.Contains(index - 1))
                    surroundingMines++;

                if (m_mines.Contains(index - 29))
                    surroundingMines++;

                if (m_mines.Contains(index + 27))
                    surroundingMines++;
            }

            if (!rightSide(index))
            {
                if (m_mines.Contains(index + 1))
                    surroundingMines++;

                if (m_mines.Contains(index + 29))
                    surroundingMines++;

                if (m_mines.Contains(index - 27))
                    surroundingMines++;
            }

            if (m_mines.Contains(index - 28))
                surroundingMines++;

            if (m_mines.Contains(index + 28))
                surroundingMines++;

            if (surroundingMines != 0)
            rec.Text = surroundingMines.ToString();
        }

        // Minen setzen
        private void setMines()
        {
            m_mines.Clear();

            for (int i = 0; i < 100; i++)
            {
                int mine = m_rnd.Next(0, 448);
                m_mines.Add(mine);
            }
        }

        // Check if Left Side
        bool leftSide(int index)
        {
            if (index % 29 == 0 || index == 0)
                return true;
            else
                return false;
        }

        // Check if Right Side
        bool rightSide(int index)
        {
            if (index % 28 == 0)
                return true;
            else
                return false;
        }
    }
}
