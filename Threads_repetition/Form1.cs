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
        public CancellationTokenSource cts = new CancellationTokenSource();


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
            int randomNum = rnd.Next((int)numMin.Value, (int)numMax.Value);
            //метод Invoke принимает делегат и выполняет его в потоке
            Invoke((MethodInvoker)delegate
            {
                chart1.Series[0].Points.AddXY(x, randomNum);
            }
            );
            x++;
            onExit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(thread.ThreadState == ThreadState.Stopped) {
                button1.Text = "Pause";
                thread.Resume();
            }
            else
            {
                button1.Text = "Resume";
                thread.Suspend();
            }
        }

      

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (thread.ThreadState == ThreadState.Suspended) thread.Resume();
            thread.Abort();
            cts.Cancel();
            
        }

        private void numMax_ValueChanged(object sender, EventArgs e)
        {
            //thread.Suspend();
            if(numMax.Value < numMin.Value)
            {
                if((sender as NumericUpDown).Name == "numMax") {
                    numMax.Value = numMin.Value;
                }
                else
                {
                    numMin.Value = numMax.Value;
                }
            }
            //if(!(thread.ThreadState == ThreadState.Running))  thread.Resume();

        }
    }
}
