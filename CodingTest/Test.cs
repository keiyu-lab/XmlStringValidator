using System.ComponentModel.DataAnnotations;
using System;
using System.IO;
using System.Reflection.PortableExecutable;

namespace TakeHomeCodingTest
{
    public class Test
    {
        public void TestDeterminXml()
        {
            XMLStringValidator validator = new XMLStringValidator();

            int numberfailed = 0;
            string[] failCase = new string[10000];
            int numberTestCase = 0;

            //read file and test
            using (StreamReader reader = new StreamReader(@"..\..\..\TestCase.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');
                    bool valid;
                    if (values[1] == "valid")
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                    var testCase = (values[0], valid);

                    //test
                    bool result = validator.DetermineXml(testCase.Item1);
                    if (result != valid)
                    {
                        failCase[numberfailed] = testCase.Item1;
                        numberfailed++;
                    }
                    numberTestCase++;

                }
            }

            //show result
            int numberpassed = numberTestCase - numberfailed;
            Console.WriteLine("result : {0}/{1}", numberpassed, numberTestCase);
            if (numberfailed > 0)
            {
                Console.WriteLine("here is fail cases");
                for (int i = 0; i < numberfailed; i++)
                {
                    Console.WriteLine(failCase[i]);
                }
            }
        }
    }
}