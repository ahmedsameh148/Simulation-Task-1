using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{
    public partial class Form1 : Form
    {
        public TaskSimulation system;
        public Form1()
        {
            InitializeComponent();
            system = new TaskSimulation();
        }

        private void runTestCase_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            system = new TaskSimulation();
            system.readData("E:\\Collage\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase2.txt");
            Print();
        }
        public void Print()
        {
            foreach (SimulationCase s in system.system.SimulationTable)
            {
                dataGridView1.Rows.Add(s.CustomerNumber, s.RandomInterArrival, s.InterArrival, s.ArrivalTime, s.RandomService, s.ServiceTime, s.AssignedServer.ID, s.StartTime, s.EndTime, s.TimeInQueue);
            }
            system.calcPreformance();
            drawChart(1);
            string res = TestingManager.Test(system.system, Constants.FileNames.TestCase2);
            MessageBox.Show(res);
        }

        private void readFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;
                MessageBox.Show(Constants.FileNames.TestCase1);
                
            }
        }
        public void drawChart(int serverID)
        {
            chart1.Series["Time"].Points.Clear();
            for (int i = 0; i <= system.totalSimulation; i++)
            {
                chart1.Series["Time"].Points.AddXY(i.ToString(), system.trakeServers[serverID - 1][i].ToString());
            }
        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int serverID = int.Parse(textBox1.Text);
            if (serverID > system.system.NumberOfServers)
                MessageBox.Show("Their Is No Server With That ID");
            else drawChart(serverID);
        }
    }
}
