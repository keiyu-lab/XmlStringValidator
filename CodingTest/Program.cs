using TakeHomeCodingTest;

XMLStringValidator validator = new XMLStringValidator();
/*
Test test = new Test();
test.TestDeterminXml();
return;
*/
Console.WriteLine("input Xml string");
//string input = Console.ReadLine();
string input = "<kad>dfa/</kad>";
bool result = validator.DetermineXml(input);
if (result)
{
    Console.WriteLine("XML string input is valid");

}
else
{

    Console.WriteLine("XML string input is not valid");

}
return;
