
namespace TestProject
{
    static class M2L2
    {
        public static void RunRandomDecisionFlow()
        {
            Random random = new Random();
            int daysUntilExpiration = random.Next(12);


            if  (daysUntilExpiration <= 10)
            {
                if(daysUntilExpiration == 0)
                {
                    Console.WriteLine("Your subscription has expired.");
                } else if (daysUntilExpiration <= 1){
                    Console.WriteLine("Your subscription expires within a day!\nRenew now and save 20%");

                } else if (daysUntilExpiration <=5)
                {
                    Console.Write($"Your subscription expires in {daysUntilExpiration} days.\nRenew noew and save 10%!");

                }else
                {
                    Console.WriteLine("Your subscription will expire soon. Renew now!");
                }

            }
            else
            {
                Console.WriteLine("Your subscription is not about to expire.");
            }

           
        }
    }
}


