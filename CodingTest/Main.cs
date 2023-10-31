using XmlStringValidator;

Console.WriteLine("Choose a mode");
Console.WriteLine("Use Xml validator : 1, Test Xml validator : 2");
string mode = Console.ReadLine();
if(mode == "1"){
    Console.WriteLine("Input Xml string");
    string input = Console.ReadLine();
    XMLStringValidator validator = new XMLStringValidator();
    bool result = validator.DetermineXml(input);
    if (result)
    {
        Console.WriteLine("XML string input is valid");

    }
    else
    {

        Console.WriteLine("XML string input is not valid");

    }
}else if(mode == "2"){
    Test test = new Test();
    test.TestDeterminXml();
    return;
}else{
    Console.WriteLine("Don't choose other");
    return;
}

