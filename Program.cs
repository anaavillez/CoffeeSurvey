using System;
using System.Collections.Generic;
using System.IO;

namespace WiredBrainCoffeeSurveys.Reports
{
    class Program
    {
        static void Main(string[] args)
        {

            GenerateWinnerEmails();
            GenerateTaskReports();
            GenerateCommentsReport();

        }

        public static void GenerateWinnerEmails()
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            Console.WriteLine(Environment.NewLine + "Selected Winners Output:");
            //While Loop:
            while (selectedEmails.Count < 2 && counter < Q1Results.Responses.Count)
            {
                var currentItem = Q1Results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.EmailAddress);
                    Console.WriteLine(currentItem.EmailAddress);
                }
                counter++;
            }

                
            File.WriteAllLines("SelectedEmails.csv", selectedEmails);

        }

        public static void GenerateTaskReports()
        {
            var tasks = new List<string>();

            // Calculated values
            double responseRate = Q1Results.NumberResponded / Q1Results.NumberSurveyed;
            double overallScore = (Q1Results.FoodScore + Q1Results.PriceScore + Q1Results.ServiceScore + Q1Results.CoffeeScore) / 4;


            //If Statment
            if (Q1Results.CoffeeScore < Q1Results.FoodScore)
            {
                tasks.Add("Investigate coffee recipes and ingrediens.");
            }

            //If else statment
            if (overallScore > 8.0)
            {
                tasks.Add("Work with leadership to reward staff!");
            }
            else
            {
                tasks.Add("Work with employees to find out how to improve sales!");
            }


            // Else if statment
            if (responseRate < .33)
            {
                tasks.Add("Research options to improve response rate");
            }
            else if (responseRate > .33 && responseRate < .66)
            {
                tasks.Add("Reward participants with a cuppon for a free coffee!");
            }
            else
            {
                tasks.Add("Reward participants with a discount cuppon");
            }


            //Switch Statment
            switch (Q1Results.AreaToImprove)
            {
                case "RewardsProgram":
                    tasks.Add("Revisit the rewards deals.");
                    break;
                case "Cleanliness":
                    tasks.Add("Contact the cleaning vendor");
                    break;
                case "MobileApp":
                    tasks.Add("Contact consulting firm about app.");
                    break;
                default:
                    tasks.Add("Investigate individual comments for ideas.");
                    break;
            }

            Console.WriteLine(Environment.NewLine + "Tasks Output:");
            foreach(var task in tasks)

            File.WriteAllLines("TasksReport.csv", tasks);


        }

        public static void GenerateCommentsReport()
            {

            var comments = new List<string>();

            Console.WriteLine(Environment.NewLine + "Selected comments Output:");
            //For Loop
            for (var i = 0; i < Q1Results.Responses.Count; i++)
                {
                    var currentResponse = Q1Results.Responses[i];

                    if (currentResponse.WouldRecommend < 7.0)
                    {
                        Console.WriteLine(currentResponse.Comments);
                        comments.Add(currentResponse.Comments);
                    }
                }

                //foreach loop
                foreach (var response in Q1Results.Responses)
                {
                    if (response.AreaToImprove == Q1Results.AreaToImprove)
                    {
                        Console.WriteLine(response.Comments);
                    comments.Add(response.Comments);
                    }
                }

            File.WriteAllLines("CommentsReport.csv", comments);
            }

        
       

    }
}
