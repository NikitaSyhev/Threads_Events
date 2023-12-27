using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

// суть программы
// создается поток, в поток пробрасывается метод JOB ( который генерирует случайное число и добавлеяет его на график )

namespace Threads_repetition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // подписка на событие onExit
            onExit += CreateThread;
            CreateThread();
        }


        //создали делегат
        public delegate void MethoContainer();
        //создали событие под этот делегат
        public event MethoContainer onExit;
        //создали поток
        public Thread thread;
        //начальная координата
        public int x = 0;


        // метод для создания потока и запускается при открытии формы
        public void CreateThread()
        {
            thread = new Thread(Job);
            thread.Start();
        }

        // метод для создания случайного числа и добавление на график в потоке
        public void Job()
        {
            Thread.Sleep(1000);
            Random rnd = new Random();
            int randomNum = rnd.Next(0, 100);
            //метод Invoke принимает делегат и выполняет его в потоке
            Invoke((MethodInvoker)delegate
            {
                chart1.Series[0].Points.AddXY(x, randomNum);
            }
            );
            x++;
            onExit();
        }
    }
}
