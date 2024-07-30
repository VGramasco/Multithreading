using System;
using System.Threading;
using System.Windows.Forms;

namespace sincronizacao_thread
{
    public partial class Form1 : Form
    {
        private const int TOTAL_UTILIZADORES = 20;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(RunThreads);
            thread.Start();
        }

        private void RunThreads()
        {
            Impressora trocaTintas = new Impressora(this);
            Utilizador[] pessoa = new Utilizador[TOTAL_UTILIZADORES];
            Thread[] pessoaThr = new Thread[TOTAL_UTILIZADORES];
            for (int i = 0; i < pessoaThr.Length; i++)
            {
                pessoa[i] = new Utilizador(i, trocaTintas);
                pessoaThr[i] = new Thread(new ThreadStart(pessoa[i].Run));
            }
            foreach (Thread utilizador in pessoaThr)
                utilizador.Start();
        }

        public void AppendText(string text)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<string>(AppendText), text);
            }
            else
            {
                textBox1.AppendText(text + Environment.NewLine);
                // Scroll to the bottom
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }
        }
    }

    public class Impressora
    {
        private Form1 form;
        private readonly object lockObject = new object();

        public Impressora(Form1 form)
        {
            this.form = form;
        }

        public void Imprime(int idUtilizador, string msg)
        {
            lock (lockObject)
            {
                form.AppendText($"------ Trabalho de {idUtilizador} -----");
                form.AppendText(msg);
                form.AppendText($"-- Fim do trabalho de {idUtilizador} --");
                form.AppendText(string.Empty);
            }
        }
    }

    public class Utilizador
    {
        public const int TOTAL_POEMAS = 20;
        private int Id;
        private Impressora Impressora;

        public Utilizador(int id, Impressora impressora)
        {
            this.Id = id;
            this.Impressora = impressora;
        }

        public void Run()
        {
            for (int i = 0; i < TOTAL_POEMAS; i++)
            {
                Thread.Sleep(10);
                string poema = $"Poema {i} do utilizador id= {Id}";
                Impressora.Imprime(Id, poema);
            }
        }
    }
}
