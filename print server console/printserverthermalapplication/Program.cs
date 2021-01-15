using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.Printing;

namespace printserverthermalapplication
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintDocument printdoc = new PrintDocument();

            TimeSpan span;
            DateTime startTime = DateTime.Now;
            int overTime = 1500;

            string printername = string.Empty;
            printername = "BT-T080(U) 1";

            printdoc.PrinterSettings.PrinterName = printername;

            string machinenames = Environment.MachineName;
            PrintServer myprintserver = new LocalPrintServer();
            PrintQueueCollection myprintqueue = myprintserver.GetPrintQueues();
            string printqueue = "my print queue : \n\n";
            bool isjobdone = false;
            Console.WriteLine(printqueue);
            foreach (PrintQueue pq in myprintqueue)
            {
                int flag = 0;
                pq.Refresh();
                //if (pq.Name != "Brother HL-L2360D series") continue;
                if (pq.Name != printername) continue;
                Console.WriteLine(pq.Name);
                do
                {
                    span = DateTime.Now - startTime;
                    if (overTime > 0 && span.TotalMilliseconds > overTime)
                    {
                        isjobdone = true;
                        Console.WriteLine("PRINTING SERVER TIME OUT");
                        Utility.WriteLog("Status printing in " + printername + " : printing server timeout", "step-action");
                        break;
                    }
                    PrintJobInfoCollection jobs = pq.GetPrintJobInfoCollection();
                    if (pq.NumberOfJobs == 0)
                    {
                        Console.WriteLine("JOB IN QUEUE IS EMPTY");
                        Utility.WriteLog("Status printing in " + printername + " : job queue is empty", "step-action");
                        isjobdone = true;
                        break;
                    }
                    Console.WriteLine(pq.QueueStatus);
                    Console.WriteLine("DO WHILE");
                    foreach (PrintSystemJobInfo job in jobs)
                    {
                        var aux = job;
                        if (aux.IsDeleted)
                        {
                            Console.WriteLine("JOB HAS DELETED");
                            Utility.WriteLog("Status printing in " + printername + " : job has deleted", "step-action");
                            isjobdone = true;
                            break;
                        }
                        if (aux.NumberOfPages == 0)
                        {
                            Console.WriteLine("NO PAGES LEFT");
                            Utility.WriteLog("Status printing in " + printername + " : no pages left", "step-action");
                            isjobdone = true;
                            break;
                        }
                        Console.WriteLine(aux.JobName);
                        Console.WriteLine(aux.JobStatus);
                        if (aux.IsPrinted)
                        {
                            Console.WriteLine("JOB HAS PRINTED");
                            Utility.WriteLog("Status printing in " + printername + " : job has printed", "step-action");
                            isjobdone = true;
                            break;
                            //Environment.Exit(0);
                        }
                        if (aux.IsDeleting)
                        {
                            Console.WriteLine("JOB HAS DELETING");
                            Utility.WriteLog("Status printing in " + printername + " : job has deleting", "step-action");
                            isjobdone = true;
                            break;
                            //flag += 1;
                        }
                        if (aux.IsCompleted)
                        {
                            Console.WriteLine("JOB HAS COMPLETED");
                            Utility.WriteLog("Status printing in " + printername + " : job has completed", "step-action");
                            isjobdone = true;
                            break;
                        }
                        /*if(flag > 2)
                        {
                            isjobdone = true;
                            Environment.Exit(0);
                        }*/
                        Task.Delay(100);
                    }
                } while (!isjobdone);
                if (pq.QueueStatus == PrintQueueStatus.None)
                {
                    Console.WriteLine("STATUS QUEUE IS NONE");
                    Utility.WriteLog("Status printing in " + printername + " : status queue is none", "step-action");
                    isjobdone = true;
                    break;
                }
            }
            Console.WriteLine("PRINTING DONE");
            Utility.WriteLog("Status printing in " + printername + " : printing done", "step-action");
            Environment.Exit(0);
        }
    }
}
