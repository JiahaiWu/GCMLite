using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMDateMatcher : IMatcher
    {
        public DCMDateMatcher(char[] expChars, int from, int end)
        {
            if (end > from)
            {
                datePatternChars = new char[end - from];
                for (int i = from; i < end; i++)
                {
                    datePatternChars[i] = expChars[from + i];
                }
                parseformatPattern();
            }
        }

        private void parseformatPattern()
        {
            if (datePatternChars != null)
            {
                int head = filterWhiteSpace(0);
                char ch = datePatternChars[head];
                switch (ch)
                {
                    case (char)DCMDateDelimitToken.leftBrace:
                        datePattern = DCMDatePatternToken.Fuzzy;
                        break;
                    case (char)DCMDatePatternToken.Fuzzy:
                        head++;
                        datePattern = DCMDatePatternToken.Fuzzy;
                        break;
                    case (char)DCMDatePatternToken.Strict:
                        head++;
                        datePattern = DCMDatePatternToken.Strict;
                        break;
                    default:
                        throw new DCMExException("Synax error in identify date pattern tag.");
                }
                
                head = filterWhiteSpace(head);
                detectDateFormation(ref head);
                head = filterWhiteSpace(head);
                detectDateTransFormat(ref head);
            }
        }
        private void detectDateFormation(ref int head)
        {
            int newHead = head;
            StringBuilder sb=new StringBuilder();
            if (newHead < datePatternChars.Length)
            {
                char ch = datePatternChars[newHead];
                if (ch.Equals((char)DCMDateDelimitToken.leftBrace))
                {
                    newHead++;
                    while (newHead < datePatternChars.Length)
                    {
                        ch = datePatternChars[newHead];
                        switch (ch)
                        {
                            case (char)DCMDateDelimitToken.rightBrace:
                                head = newHead + 1;
                                formatPattern = sb.ToString();
                                return;
                            case (char)DCMDateDelimitToken.leftBrace:
                                throw new DCMExException("Synax error in identify date Delimit token.");
                            default:
                                sb.Append(ch);
                                break;
                        }
                        newHead++;
                    }
                }
                else if (ch.Equals((char)DCMTranformToken.TransTag))
                {
                    return;
                }
                else if (datePattern.Equals(DCMDatePatternToken.Strict))
                    throw new DCMExException("Synax error in identify date Delimit token.");
            }
            formatPattern = null;
        }

        private void detectDateTransFormat(ref int head)
        {
            if (head < datePatternChars.Length)
            {
                char ch = datePatternChars[head];
                if (ch.Equals((char)DCMTranformToken.TransTag))
                {
                    transformFormat = new string(datePatternChars, head + 1, datePatternChars.Length - head + 1).TrimEnd();
                }
                else
                {
                    throw new DCMExException("Synax error in identify date tranform formats.");
                }
            }
        }

        private int filterWhiteSpace(int head)
        {
            char ch;
            while (head < datePatternChars.Length)
            {
                ch = datePatternChars[head];
                if (ch.Equals((char)DelimitToken.EndLine) || ch.Equals((char)DelimitToken.NewLine) || ch.Equals((char)DelimitToken.WhiteSpace))
                {
                    ++head;
                    continue;
                }
                else
                    break;
            }
            return head;
        }

        public bool isMatch(string chars, params string[] groups)
        {
            string checkChars = chars;
            if (groupFetchTag != null)
            {
                groupFetchTag.tryFetchGroup(groups, ref checkChars);
            }
            bool res = false;
            DateTime dt = DateTime.Now;
            if (formatPattern != null && formatPattern.Length > 0)
            {
                res = DateTime.TryParseExact(checkChars, formatPattern, null, System.Globalization.DateTimeStyles.None, out dt);
                if (!res && datePattern.Equals(DCMDatePatternToken.Strict))
                {
                    res = false;
                }
            }
            res = DateTime.TryParse(checkChars, out dt);
            return res;
        }

        public void setGroupFetchTag(GroupFetcher fetchTag)
        {
            this.groupFetchTag = fetchTag;
        }

        public string[] getGroups()
        {
            return null;
        }

        private readonly char[] datePatternChars;
        private DCMDatePatternToken datePattern = DCMDatePatternToken.Fuzzy;
        private string formatPattern = null;
        public GroupFetcher groupFetchTag = null;
        private string transformFormat = null;
    }
}
