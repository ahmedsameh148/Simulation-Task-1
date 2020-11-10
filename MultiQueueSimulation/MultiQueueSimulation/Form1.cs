﻿using System;
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
            system.readData("E:\\Collage\\Template_Students\\MultiQueueSimulation\\MultiQueueSimulation\\TestCases\\TestCase1.txt");
        }
    }
}
