using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiQueueModels;
using System.IO;
using System.Windows.Forms;

namespace MultiQueueModels
{
    public class TaskSimulation
    {
        public SimulationSystem system { set; get; }
        public Queue<Customer> queue { set; get; }
        List<Customer> customers { get; set; }
        public int SelectedMethod { set; get; }
        public int StoppingCriteria { set; get; }
        public TaskSimulation()
        {
            system = new SimulationSystem();
            queue = new Queue<Customer>();
            customers = new List<Customer>();
        }

        public void readData(string FilePath)
        {
            string[] lines = File.ReadAllLines(FilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Equals("NumberOfServers"))
                {
                    i++;
                    system.NumberOfServers = int.Parse(lines[i]);
                }
                else if (lines[i].Equals("StoppingNumber"))
                {
                    i++;
                    system.StoppingNumber = int.Parse(lines[i]);
                }
                else if (lines[i].Equals("StoppingCriteria"))
                {
                    i++;
                    StoppingCriteria = int.Parse(lines[i]);
                }
                else if (lines[i].Equals("SelectionMethod"))
                {
                    i++;
                    SelectedMethod = int.Parse(lines[i]);
                }
                else if (lines[i].Equals("InterarrivalDistribution"))
                {
                    i++;
                    int ac = 0;
                    while (lines[i].Length > 0)
                    {
                        string[] interval = lines[i].Split(',');
                        TimeDistribution distribution = new TimeDistribution();
                        int prob = (int)(double.Parse(interval[1]) * 100.0);
                        distribution.Time = int.Parse(interval[0]);
                        distribution.MinRange = ac + 1;
                        ac += prob;
                        distribution.MaxRange = ac; i++;
                        system.InterarrivalDistribution.Add(distribution);
                    }
                    for (int y = 0; y < system.NumberOfServers; y++)
                    {
                        i += 2;
                        int acc = 0;
                        Server server = new Server();
                        while (i < lines.Length && lines[i].Length > 0)
                        {
                            string[] interval = lines[i].Split(',');
                            TimeDistribution distribution = new TimeDistribution();
                            int prob = (int)(double.Parse(interval[1]) * 100.0);
                            distribution.Time = int.Parse(interval[0]);
                            distribution.MinRange = acc + 1;
                            acc += prob;
                            distribution.MaxRange = acc; i++;
                            server.TimeDistribution.Add(distribution);
                        }
                        server.ID = system.Servers.Count + 1;
                        server.TotalWorkingTime = 0;
                        server.FinishTime = 0;
                        system.Servers.Add(server);
                    }
                }
            }
            //MessageBox.Show("Number Of Servers = " + system.NumberOfServers.ToString());
            //MessageBox.Show("Stopping Number = " + system.StoppingNumber.ToString());
            //MessageBox.Show("Stopping Criteria = " + StoppingCriteria.ToString());
            //MessageBox.Show("Selected Method = " + SelectedMethod.ToString());
            //MessageBox.Show("InterArrival Time ");
            //foreach (TimeDistribution t in system.InterarrivalDistribution) {
            //    MessageBox.Show(t.Time.ToString() + "  " + t.MinRange.ToString() + "  " + t.MaxRange.ToString());
            //}
            //foreach (Server s in system.Servers) {
            //    MessageBox.Show("Sever " + s.ID);
            //    foreach (TimeDistribution t in s.TimeDistribution)
            //    {
            //        MessageBox.Show(t.Time.ToString() + "  " + t.MinRange.ToString() + "  " + t.MaxRange.ToString());
            //    }
            //}
            buildTable();
        }
        public KeyValuePair<int, int> generateInterArrival()
        {
            Random random = new Random();
            int rnd = random.Next() % 100;
            if (rnd == 0) rnd++;
            foreach (TimeDistribution t in system.InterarrivalDistribution)
            {
                if (t.MinRange <= rnd && t.MaxRange >= rnd)
                    return new KeyValuePair<int, int>(t.Time, rnd);
            }
            return new KeyValuePair<int, int>();
        }
        public KeyValuePair<int, int> generateServiceTime(int selectedServer)
        {
            Random random = new Random();
            int rnd = random.Next() % 100;
            if (rnd == 0) rnd++;
            foreach (TimeDistribution t in system.Servers[selectedServer].TimeDistribution)
            {
                if (t.MinRange <= rnd && t.MaxRange >= rnd)
                    return new KeyValuePair<int, int>(t.Time, rnd);
            }
            return new KeyValuePair<int, int>();
        }
        public void buildTable()
        {
            int ac = 0;
            while (true)
            {
                int t;
                if (StoppingCriteria == 1)
                    t = customers.Count;
                else t = ac;
                if (t >= system.StoppingNumber)
                    break;
                KeyValuePair<int, int> interarrival = generateInterArrival();
                if (StoppingCriteria == 2 && ac + interarrival.Key > ac)
                    break;
                Customer customer = new Customer();
                customer.ArrivalTime = ac + interarrival.Key;
                customer.InterArrivalTime = interarrival.Key;
                customer.RandomInterArrival = interarrival.Value;
                customer.CustomerNumber = customers.Count + 1;
                ac += interarrival.Key;
                customers.Add(customer);
            }
            int idx = 0;
            for (int i = 0; i <= ac || queue.Count > 0; i++)
            {
                checkQueue();
                if (customers[idx].ArrivalTime == i)
                {
                    assignToServer(customers[idx], i);
                }
            }
        }

        void checkQueue(int CurTime)
        {
            if (queue.Count == 0)
                return;
            bool st = false;
            foreach (Server s in system.Servers)
            {
                if (s.FinishTime <= CurTime) st = true;
            }
            if (!st)
                return;
            assignToServer(queue.Dequeue(), CurTime);
        }
        void assignToServer(Customer custoner, int CurTime)
        {

        }

    }

}
