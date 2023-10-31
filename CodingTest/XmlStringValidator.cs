namespace XmlStringValidator
{
    public class XMLStringValidator
    {
        private enum Type
        {
            InValid,
            OpeningTag,
            ClosingTag,
            Content,
        }

        /// <summary>
        /// Determine Xml string which is valid or not
        /// </summary>
        /// <param name="xml">xml input</param>
        /// <returns>Xml string is valid : true, not valid : false</returns>
        public bool DetermineXml(string xml)
        {
            bool isValid = true;
            int pointFocused = 0;
            string body = "";
            Type type = Type.InValid;
            Stack<string> openingTags = new Stack<string>();
            Stack<Type> types = new Stack<Type>();

            //check input null or not
            if (string.IsNullOrEmpty(xml)) return false;
            
            isValid  = AssignString(xml, ref pointFocused, ref type, ref body);
            if (isValid != true)    return false;

            //only openingTagBody must be first
            if (type != Type.OpeningTag)    return false;

            openingTags.Push(body);
            types.Push(type);

            while (openingTags.Count > 0)
            {
                pointFocused++;
                body = "";
                isValid = AssignString(xml, ref pointFocused,ref type, ref body);
                if (isValid != true) return false;
                Type typeBefore = types.Peek();

                switch (type)
                {
                    case Type.OpeningTag:
                        //Content must be nested for same tags
                        if (typeBefore == Type.Content) return false;
                        openingTags.Push(body);
                        break;

                    case Type.ClosingTag:
                        isValid = IsValidClosingTag(body, ref openingTags);
                        if (isValid != true) return false;    
                        break;

                    case Type.Content:
                        //Content must be nested for same tags
                        if (typeBefore != Type.OpeningTag) return false;                    
                        break;
                }
                types.Push(type);
            }

            //First tagBody must be pushed lastly
            if (pointFocused + 1 != xml.Length) return false;

            return true;
        }

        /// <summary>
        /// Match with closing tag and opening tag which is pushed lately
        /// </summary>
        /// <param name="closingTagBody"></param>
        /// <param name="stack">which is pushed only opening tag</param>
        /// <returns>no ploblem : true</returns>
        private bool IsValidClosingTag(string closingTagBody, ref Stack<string> stack)
        {
            string openingTagBody = stack.Pop();
            //only openingTagBody must be first
            if (openingTagBody != closingTagBody) return false;

            return true;
        }

        /// <summary>
        /// Assign string to tag or content 
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="pointFocused">number of letters focused now</param>
        /// <param name="type">type of string focused now</param>
        /// <param name="body">body of string focused now</param>
        /// <returns>no ploblem : true</returns>
        private bool AssignString(string xml, ref int pointFocused,ref Type type, ref string body)
        {
            //XML string must start with opening tagBody and finish with closing tagBody
            if (pointFocused >= xml.Length) return false;

            if (xml[pointFocused] == '<')
            {
                bool isTagTypeValid = AssignTagType(xml, ref pointFocused, ref type, ref body);
                if( isTagTypeValid != true )   return false;
            }
            else
            {

                bool isContentValid  = FindContent(xml, ref pointFocused, ref body);
                if (isContentValid != true) return false;
                type = Type.Content;
            }
            
            return true;
        }

        /// <summary>
        /// Assign tag to opening tag or closing tag
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="pointFocused"></param>
        /// <param name="tagType">type of tag focused now</param>
        /// <param name="tagBody">body of tag focused now</param>
        /// <returns>no ploblem : true</returns>
        private bool AssignTagType(string xml, ref int pointFocused, ref Type tagType, ref string tagBody)
        {
            bool isTagValid;

            if (xml[pointFocused + 1] == '/')
            {
                //tagBody doesn't include '<','>','/'
                pointFocused = pointFocused + 2;
                isTagValid = FindTag(xml, ref pointFocused, ref tagBody);
                if(isTagValid != true)  return false;
                tagType = Type.ClosingTag;

            }
            else
            {
                pointFocused++;
                isTagValid= FindTag(xml, ref pointFocused, ref tagBody);
                if (isTagValid != true) return false;
                tagType = Type.OpeningTag;

            }

            return true;
        }

        /// <summary>
        /// Find tag's body
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="pointFocused"></param>
        /// <param name="tagBody"></param>
        /// <returns>no ploblem : true</returns>
        private bool FindTag(string xml, ref int pointFocused, ref string tagBody)
        {
           
            while (xml[pointFocused] != '>')
            {
                //Tag must finish in '>'
                if (pointFocused >= xml.Length - 1) return false;

                tagBody += xml[pointFocused];
                pointFocused++;
                
            }

            return true;
        }

        /// <summary>
        /// Find content's body
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="pointFocused"></param>
        /// <param name="content"></param>
        /// <returns>no ploblem : true</returns>
        private bool FindContent(string xml, ref int pointFocused, ref string content)
        {
            while (xml[pointFocused] != '<')
            {
                //Content must not include '>' but '/' is ok
                if (xml[pointFocused] == '>') return false;
                content += xml[pointFocused];
                pointFocused++;
                //Content must not be end
                if (pointFocused >= xml.Length) return false;
            }

            //pointFocused is left on '<'
            pointFocused = pointFocused - 1;
            
            return true;
        }

    }
}
