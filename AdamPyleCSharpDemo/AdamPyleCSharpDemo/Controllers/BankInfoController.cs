using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdamPyleCSharpDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankInfoController : ControllerBase
    {

        private struct Bank
        {
            public String name;
            public int rating;
            public double totalAssets;
            public DateTime date;
            public bool approved;
            public double calculatedLimit;
        };
        List<Bank> banks = new List<Bank>();


        private readonly ILogger<BankInfoController> _logger;

        public BankInfoController(ILogger<BankInfoController> logger)
        {
            createBankDataset();
            _logger = logger;
        }

        [HttpGet]
        public List<Dictionary<String, String>> Get()
        {


            List<Dictionary<String, String>> json;
            json = new List<Dictionary<String, String>>();
            foreach (Bank bank in banks)
            {
                Dictionary<String, String> temp = new Dictionary<String, String>();
                temp.Add("Name", bank.name);
                temp.Add("Approved", bank.approved.ToString());
                temp.Add("Rating", bank.rating.ToString());
                temp.Add("TotalAssets", bank.totalAssets.ToString());
                temp.Add("Date", bank.date.ToString());
                temp.Add("CalculatedLimit", bank.calculatedLimit.ToString());
                json.Add(temp);
            };
            return json;
        }

        private double calculateLimit(int rating, double totalAssets)
        {
            double toReturn = 2000000;
            if (rating >= -5 && rating <= -3)
            {
                toReturn -= (toReturn * .12);
            }
            else if (rating >= -2 && rating <= 0)
            {
                toReturn -= (toReturn * .09);
            }
            else if (rating >= 1 && rating <= 3)
            {
                toReturn += (toReturn * .05);
            }
            else if (rating >= 4 && rating <= 6)
            {
                toReturn += (toReturn * .08);
            }
            else if (rating >= 7 && rating <= 10)
            {
                toReturn += (toReturn * .13);
            }
            else
            {
                toReturn = -1;
                _logger.LogError("Error! Rating is out of bounds.");
                return toReturn;
            }
            if (totalAssets > 3000000)
            {
                toReturn += (toReturn * .23);
            }

            return toReturn;
        }

        private void createBankDataset()
        {

            //If this were for real, this would instead contain the logic to connect to a dabase, and run a query, using prepared statements, to fetch this data from the database designed earlier in this
            //application. Assuming the following table structure and foreign keys, it would use the query "select bankName.Name, bankName.Approved, riskRating.risk,totalAssets.TotalAssets,calculatedLimit.CalculatedLimit,calculatedLimit.CurrentDate where bankName.Name=riskRating.Name AND bankName.Name=TotalAssets.Name AND bankName.Accepted=true AND calculatedLimit.CurrentDate=GETDATE();
            //
            //Below is how I'd lay out the table structure:
            //Table BankName
                //varchar Name, boolean Approved
            //Table RiskRating
                //varchar BankName, smallint Risk
            //Table TotalAssets
                //varchar BankName, decimal TotalAssets
            //Table CalculatedLimit
                //varchar BankName, decimal CalculatedLimit, DateTime CurrentDate
            //Any or all of these tables can be joined on the bank's name, and I would put a constraint on the last three tables that the bank name must already exist in table BankName.




            //Upon running the aforementioned database query, I'd put the resulting data into a structure I could easily move out in either xml or json. Note that while the calculated limit would likely be present in the database, here we will need to calculate it.
            //The following will simulate what the state would be after getting this data from the database.
            Bank bank = new Bank();
            bank.name = "Bank of America";
            bank.approved = true;
            bank.rating = 7;
            bank.totalAssets = 1234000;
            bank.date = DateTime.Parse("12/10/2020");
            bank.calculatedLimit = calculateLimit(bank.rating, bank.totalAssets);
            banks.Add(bank);

            bank = new Bank();
            bank.name = "Wells Fargo";
            bank.approved = true;
            bank.rating = -4;
            bank.totalAssets = 5657345;
            bank.date = DateTime.Parse("12/10/2020");
            bank.calculatedLimit = calculateLimit(bank.rating, bank.totalAssets);
            banks.Add(bank);

            bank = new Bank();
            bank.name = "Bank of Nova Scotia";
            bank.approved = false;
            bank.rating = 2;
            bank.totalAssets = 2999002;
            bank.date = DateTime.Parse("12/10/2020");
            bank.calculatedLimit = calculateLimit(bank.rating, bank.totalAssets);
            banks.Add(bank);

            bank = new Bank();
            bank.name = "Bank of Montreal";
            bank.approved = false;
            bank.rating = 9;
            bank.totalAssets = 15342679;
            bank.date = DateTime.Parse("12/10/2020");
            bank.calculatedLimit = calculateLimit(bank.rating, bank.totalAssets);
            banks.Add(bank);



        }
    }

}
