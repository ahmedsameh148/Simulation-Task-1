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
        public int totalSimulation { set; get; }
        public int[][] trakeServers;
        public TaskSimulation()
        {
            system = new SimulationSystem();
            queue = new Queue<Customer>();
            customers = new List<Customer>();
            trakeServers = new int[200][];
            for (int i = 0; i < 200; i++)
            {
                trakeServers[i] = new int[10000];
            }
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
                        server.TotalServerCustomer = 0;
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
            int rnd = random.Next(1, 101);
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
            int rnd = random.Next(1, 101);
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
                //MessageBox.Show(interarrival.Key.ToString());
                if (StoppingCriteria == 2 && ac + interarrival.Key > system.StoppingNumber)
                    break;
                
                Customer customer = new Customer();
                customer.ArrivalTime = (customers.Count > 0) ? ac + interarrival.Key : 0;
                customer.InterArrivalTime = interarrival.Key;
                customer.RandomInterArrival = interarrival.Value;
                customer.CustomerNumber = customers.Count + 1;
                if (customers.Count > 0)
                    ac += interarrival.Key;
                customers.Add(customer);
            }
            //MessageBox.Show(ac.ToString());
            int idx = 0;
            for (int i = 0; i <= ac || queue.Count > 0; i++)
            {
                checkQueue(i);
                if (idx < customers.Count && customers[idx].ArrivalTime <= i)
                {
                    assignToServer(customers[idx], i); idx++;
                }
            }
        }

        void checkQueue(int CurTime)
        {
            while (queue.Count != 0)
            {
                bool st = false;
                foreach (Server s in system.Servers)
                {
                    if (s.FinishTime <= CurTime) st = true;
                }
                if (!st)
                    return;
                assignToServer(queue.Dequeue(), CurTime);
            }
        }
        void assignToServer(Customer customer, int CurTime)
        {
            int mn = 100000, selectedServer = -1;
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                if (system.Servers[i].FinishTime > CurTime)
                    continue;
                if (SelectedMethod == 3)
                {
                    if (mn > system.Servers[i].TotalWorkingTime)
                    {
                        mn = system.Servers[i].TotalWorkingTime;
                        selectedServer = i;
                    }
                }
                else
                {
                    selectedServer = i; break;
                }
            }
            if (selectedServer == -1)
            {
                queue.Enqueue(customer);
                system.PerformanceMeasures.MaxQueueLength = Math.Max(system.PerformanceMeasures.MaxQueueLength, queue.Count);
            }
            else addToServer(customer, selectedServer);
        }

        void addToServer(Customer customer, int selectedServer)
        {
            KeyValuePair<int, int> serviceTime = generateServiceTime(selectedServer);

            int start = Math.Max(customer.ArrivalTime, system.Servers[selectedServer].FinishTime);
            int end = start + serviceTime.Key;
            for (int i = start; i <= end; i++)
            {
                trakeServers[selectedServer][i] = 1;
            }
            totalSimulation = Math.Max(totalSimulation, end);
            int timeInQueue = Math.Max(0, system.Servers[selectedServer].FinishTime - customer.ArrivalTime);
            system.Servers[selectedServer].TotalServerCustomer++;

            system.Servers[selectedServer].TotalWorkingTime += serviceTime.Key;
            system.Servers[selectedServer].FinishTime = end;
            SimulationCase simcase = new SimulationCase();

            simcase.CustomerNumber = customer.CustomerNumber;
            simcase.RandomInterArrival = customer.RandomInterArrival;
            simcase.InterArrival = customer.InterArrivalTime;

            simcase.ArrivalTime = customer.ArrivalTime;
            simcase.RandomService = serviceTime.Value;
            simcase.ServiceTime = serviceTime.Key;

            simcase.AssignedServer = system.Servers[selectedServer];
            simcase.StartTime = start;
            simcase.EndTime = end;

            simcase.TimeInQueue = timeInQueue;
            system.SimulationTable.Add(simcase);
        }
        public void calcPreformance()
        {
            double totalq = 0, c = 0;
            foreach (SimulationCase s in system.SimulationTable) {
                totalq += s.TimeInQueue;
                if (s.TimeInQueue > 0) c++;
            }
            if (customers.Count > 0)
                system.PerformanceMeasures.AverageWaitingTime = ((decimal)totalq) / (customers.Count);
            if (customers.Count > 0)
                system.PerformanceMeasures.WaitingProbability = ((decimal)c) / (customers.Count);
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                if (system.Servers[i].TotalServerCustomer > 0)
                    system.Servers[i].AverageServiceTime = ((decimal)system.Servers[i].TotalWorkingTime) / system.Servers[i].TotalServerCustomer;
                system.Servers[i].IdleProbability = ((decimal)totalSimulation - system.Servers[i].TotalWorkingTime) / totalSimulation;
                system.Servers[i].Utilization = ((decimal)system.Servers[i].TotalWorkingTime) / totalSimulation;
            }
        }
    }

}
