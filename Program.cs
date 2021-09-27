using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;


namespace WiredBrainCoffeeSurveys.Reports
   
{
    class Program
    {

        static void Main(string[] args)
        {

            bool quitApp = false;

            do
            {
                Console.WriteLine("Please specify a report to run( rewards, comments, tasks, quit):");
                var selectedReport = Console.ReadLine();

                Console.WriteLine("Please specify which quarter of data: (q1, q2)");
                var selectedData = Console.ReadLine();

                var surveyResults = JsonConvert.DeserializeObject<SurveyResults>
                    (File.ReadAllText($"data/{selectedData}.json"));

                switch (selectedReport)
                {
                    case "rewards":
                        GenerateWinnerEmails(surveyResults);
                        break;
                    case "comments":
                        GenerateCommentsReport(surveyResults);
                        break;
                    case "tasks":
                        GenerateTasksReport(surveyResults);
                        break;
                    case "quit":
                        quitApp = true;
                        break;
                    default:
                        Console.WriteLine("Sorry, that's not a valid option.");
                        break;

                }

                Console.WriteLine();

            }
            while (!quitApp);


        }

        public static void GenerateWinnerEmails(SurveyResults results)
        {
            var selectedEmails = new List<string>();
            int counter = 0;

            Console.WriteLine(Environment.NewLine + "Selected Winners Output:");
            //While Loop:
            while (selectedEmails.Count < 2 && counter < results.Responses.Count)
            {
                var currentItem = results.Responses[counter];

                if (currentItem.FavoriteProduct == "Cappucino")
                {
                    selectedEmails.Add(currentItem.emailAdress);
                    Console.WriteLine(currentItem.emailAdress);
                }
                counter++;
            }

                
            File.WriteAllLines("SelectedEmails.csv", selectedEmails);

        }

        public static void GenerateTasksReport(SurveyResults results)
        {
            var tasks = new List<string>();

            double responseRate = results.NumberResponded / results.NumberSurveyed;
            double overallScore = (results.ServiceScore + results.CoffeeScore + results.FoodScore + results.PriceScore) / 4;

            //If statment
            if (results.CoffeeScore < results.FoodScore)
            {
                tasks.Add("Investigate coffee recipes and ingredients.");
            }

            // If else statment
            var newTask = overallScore > 8.0 ? "Work with leadership to reward staff" : "Work with employees for improvement ideas";
            tasks.Add(newTask);

           // !!!!!!!!!!!!!!This are two different ways to right!!!!!!!!!!!!!!!!!
            //If else statment
            tasks.Add(responseRate switch
            {
                var rate when rate < .33 => "Research options to improve response rate.",
                var rate when rate > .33 && rate < .66 => "Reward participants with free coffee cuppon.",
                var rate when rate > .66 => "Rewards participants with discount coffee coupon."
            });


            //Switch statment
            tasks.Add(results.AreaToImprove switch
            {
                "RewardsProgram" => "Revisit rewards deals.",
                "CleanlinessProgram" => "Contact the cleaning vendor.",
                "MobileApp" => "Contact the consulting firm about app.",
                _ => "Investigate individual comments for ideas."
            });
         
          

            Console.WriteLine(Environment.NewLine + "Tasks Output:");
            foreach (var task in tasks)
            {
                Console.WriteLine(task);
            }

            File.WriteAllLines("TasksReport.csv", tasks);
        }

        public static void GenerateCommentsReport(SurveyResults results)
            {

            var comments = new List<string>();

            Console.WriteLine(Environment.NewLine + "Selected comments Output:");
            //For Loop
            for (var i = 0; i < results.Responses.Count; i++)
                {
                    var currentResponse = results.Responses[i];

                    if (currentResponse.WouldRecommend < 7.0)
                    {
                        Console.WriteLine(currentResponse.comments);
                        comments.Add(currentResponse.comments);
                    }
                }

                //foreach loop
                foreach (var response in results.Responses)
                {
                    if (response.AreaToImprove == results.AreaToImprove)
                    {
                        Console.WriteLine(response.comments);
                    comments.Add(response.comments);
                    }
                }

            File.WriteAllLines("CommentsReport.csv", comments);
            }

        
       

    }
}
