using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1_Simulation
{
    class Calculation
    {
        public double GenerateRandomDouble()
        {
            double RandomDouble = 0;
            RandomDouble = DataBase.Rand.NextDouble();
            return RandomDouble;
        }
        public int GenerateRandomInt(int a, int b)
        {
            int RandomInt = 0;
            RandomInt = DataBase.Rand.Next(a, b);
            return RandomInt;
        }
        public int GetInterArrivalTime(double RandomNum)
        {
            int InterArrivalTime = 1;
            for (int i = 0; i < DataBase.InterArrivalDistribution.Count; i++)
            {
                if (DataBase.InterArrivalDistribution[i].CummProbability >= RandomNum)
                {
                    InterArrivalTime = DataBase.InterArrivalDistribution[i].Time;
                    break;
                }
            }
            return InterArrivalTime;
        }
        public int GetServiceTime(int ServerIndex, double RandomNum)
        {
            int ServiceTime = 0;
            for (int i = 0; i < DataBase.ServiceTimeDistribution.Count; i++)
            {
                if (DataBase.ServiceTimeDistribution[ServerIndex - 1][i].CummProbability >= RandomNum)
                {
                    ServiceTime = DataBase.ServiceTimeDistribution[ServerIndex - 1][i].Time;
                    break;
                }
            }
            if (ServiceTime == 0)
                ServiceTime =1;
            return ServiceTime;
        }
        public void SetServers()
        {
            Server Server;
            DataBase.Servers = new List<Server>();
            for (int i = 0; i < DataBase.Servernumber; i++)
            {
                Server = new Server();
                Server.available = true;
                Server.ServerNo = i + 1;
                Server.WorkedTime = Server.IdleTime = Server.CustormNumbers = 0;
                DataBase.Servers.Add(Server);
            }
        }
        public int ChooseServer()
        {
            int ServerIndex = -1;
            if (DataBase.ServerMethod == "Random")//Choose first random Empty Server
            {
                int RandInt = GenerateRandomInt(1, DataBase.Servernumber + 1);
                HashSet<int> set = new HashSet<int>();
                while (DataBase.Servers[RandInt - 1].available == false)
                {
                    set.Add(RandInt);
                    RandInt = GenerateRandomInt(1, DataBase.Servernumber + 1);

                    if (set.Count == DataBase.Servernumber)
                        return -1;
                }
                ServerIndex = RandInt;
            }
            else if (DataBase.ServerMethod == "Highest Priority") // Assume Lowest Index has a high Priority
            {
                for (int i = 0; i < DataBase.Servernumber; i++)
                {
                    if (DataBase.Servers[i].available)
                    {
                        ServerIndex = DataBase.Servers[i].ServerNo;
                        break;
                    }
                }
            }
            else // Choose the first server that has lowest worked time
            {
                DataBase.Servers = DataBase.Servers.OrderBy(x => x.WorkedTime).ToList();
                for (int i = 0; i < DataBase.Servernumber; i++)
                {
                    if (DataBase.Servers[i].available)
                    {
                        ServerIndex = DataBase.Servers[i].ServerNo;
                        break;
                    }
                }
            }
            return ServerIndex;
        }
    }
}
