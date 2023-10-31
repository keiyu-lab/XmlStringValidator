using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace TakeHomeCodingTest
{
    public class XMLStringValidator
    {
        private enum Type
        {
            InValid,
            StartingElement,
            EndingElement,
            Contain,
        }

        public bool DetermineXml(string xml)
        {
            bool isValid = true;
            int pointFocused = 0;
            string body = "";
            Type type = Type.InValid;
            Stack<string> startingElements = new Stack<string>();
            Stack<Type> types = new Stack<Type>();

            //check input
            if (string.IsNullOrEmpty(xml))
            {
                Console.WriteLine("XML string is null, so");
                return false;
            }

            //only tagHead must be first
            isValid  = JudgeType(xml, ref pointFocused, ref type, ref body);
            if (isValid != true) return false;

            if (type != Type.StartingElement)
            {
                Console.WriteLine("head must be first, so");
                return false;
            }
            startingElements.Push(body);
            types.Push(type);

            while (startingElements.Count > 0)
            {
                pointFocused++;
                isValid = JudgeType(xml, ref pointFocused,ref type, ref body);
                if (isValid != true) return false;
                Type typeBefore = types.Peek();

                switch (type)
                {
                    case Type.StartingElement:
                        //Contain must be between StartingElement and EndingElement
                        //have to change point
                        /*
                        if (typeBefore == Type.Contain)
                        {
                            Console.WriteLine("Contain must be between StartingElement and EndingElement , so");
                            return false;
                        }
                        */
                        startingElements.Push(body);
                        break;

                    case Type.EndingElement:
                        isValid = IsEndingElementValid(body, ref startingElements);
                        if (isValid != true) return false;    
                        break;

                    case Type.Contain:
                        //Contain must be between StartingElement and EndingElement
                        //have to change point
                        /*
                        if (typeBefore != Type.StartingElement)
                        {
                            Console.WriteLine("Contain must be between StartingElement and EndingElement, so");
                            return false;
                        }
                        */
                        break;
                }

                types.Push(type);
            }

            if (pointFocused + 1 != xml.Length)
            {
                Console.WriteLine("First tag must be pushed lastly, so");
                return false;
            }

            return true;
        }

        private bool IsEndingElementValid(string tagTail, ref Stack<string> stack)
        {
            string tagHead = stack.Pop();
            if (tagHead != tagTail)
            {
                Console.WriteLine("Starting element must match with ending element, so");
                return false;
            }
            return true;
        }

        private bool JudgeType(string xml, ref int pointFocused,ref Type type, ref string str)
        {
            bool isTypeValid;

            if (pointFocused >= xml.Length)
            {
                Console.WriteLine("XML string must start with head and finish with tail, so");
                return false;
            }

            if (xml[pointFocused] == '<')
            {

                isTypeValid = JudgeTagType(xml, ref pointFocused, ref type, ref str);
                if( isTypeValid != true )   return false;
            }
            else
            {

                isTypeValid  = FindContain(xml, ref pointFocused, ref str);
                if (isTypeValid != true) return false;
                type = Type.Contain;
            }

            return true;
        }

        private bool JudgeTagType(string xml, ref int pointFocused, ref Type tagType, ref string tag)
        {
            bool isTagValid;

            if (xml[pointFocused + 1] == '/')
            {
                //tag doesn't include <,>,/
                pointFocused += pointFocused + 2;
                isTagValid = FindTag(xml, ref pointFocused, ref tag);
                if(isTagValid != true)  return false;
                tagType = Type.EndingElement;

            }
            else
            {
                pointFocused++;
                isTagValid= FindTag(xml, ref pointFocused, ref tag);
                if (isTagValid != true) return false;
                tagType = Type.StartingElement;

            }

            return true;
        }

        private bool FindTag(string xml, ref int pointFocused, ref string tag)
        {
            while (xml[pointFocused] != '>')
            {
                //to deal with no '>'
                if (pointFocused >= xml.Length - 1)
                {
                    Console.WriteLine("Tag must finish with '>' , so");
                    return false;
                }
                tag += xml[pointFocused];
                pointFocused++;
            }

            //return the position of '>'
            return true;
        }

        private bool FindContain(string xml, ref int pointFocused, ref string contain)
        {
            while (xml[pointFocused] != '<')
            {
                contain += xml[pointFocused];
                pointFocused++;
                if (pointFocused >= xml.Length)
                {
                    Console.WriteLine("Contain must not be end, so");
                    return false;
                }
            }
            pointFocused = pointFocused - 1;
            //return the position next to '<'
            return true;
        }

  
    }
}
