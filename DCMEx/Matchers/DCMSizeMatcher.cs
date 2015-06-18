using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    /// <summary>
    /// 长度匹配验证实现方法类
    /// 说明：
    /// 1.数值匹配验证表达式基本范式：
    /// {[%d91 | %d40] number, number [%d91 | %d40]}[{>format}]
    /// 数值区间以圆括号为开区间，方括号为闭区间。
    /// 如伴随变换符则执行长度截取转换操作。
    /// </summary>
    class DCMSizeMatcher : IMatcher
    {
        private readonly char[] sizePatternChars;
        public GroupFetcher groupFetchTag = null;
        private string transformFormat = null;
        List<string> groups = new List<string>();

        private Int32 leftNumber = Int32.MinValue;
        private Int32 rightNumber = Int32.MinValue;
        private DCMSizeDelimitToken leftToken = DCMSizeDelimitToken.leftClosed;
        private DCMSizeDelimitToken rightToken = DCMSizeDelimitToken.rightClosed;


        public DCMSizeMatcher(char[] expChars, int from, int end)
        {
            if (end > from)
            {
                sizePatternChars = new char[end - from];
                for (int i = from; i < end; i++)
                {
                    sizePatternChars[i] = expChars[from + i];
                }
                parseformatPattern();
            }
        }

        private void parseformatPattern()
        {
            if (sizePatternChars != null)
            {
                int head = filterWhiteSpace(0);
                detectSizeInterval(ref head);
                head = filterWhiteSpace(head);
                detectSizeTransFormat(ref head);
            }
        }
        private void detectSizeTransFormat(ref int head)
        {
            if (head < sizePatternChars.Length)
            {
                char ch = sizePatternChars[head];
                if (ch.Equals((char)DCMTranformToken.TransTag))
                {
                    transformFormat = new string(sizePatternChars, head + 1, sizePatternChars.Length - head + 1).TrimEnd();
                }
                else
                {
                    throw new DCMExException("Synax error in identify number tranform formats.");
                }
            }
        }
        private void detectSizeInterval(ref int head)
        {
            if (head < sizePatternChars.Length)
            {
                char ch = sizePatternChars[head];
                switch (ch)
                {
                    case (char)DCMSizeDelimitToken.leftOpen:
                        leftToken = DCMSizeDelimitToken.leftOpen;
                        head++;
                        leftNumber = indentifyNextNumber(ref head);
                        if (!sizePatternChars[head].Equals((char)DCMSizeDelimitToken.separator))
                            throw new DCMExException("Synax error in identify size interval.");
                        head++;
                        rightNumber = indentifyNextNumber(ref head);
                        rightToken = indentifyIntervalRightToken(ref head);
                        break;
                    case (char)DCMSizeDelimitToken.leftClosed:
                        leftToken = DCMSizeDelimitToken.leftClosed;
                        head++;
                        leftNumber = indentifyNextNumber(ref head);
                        if (!sizePatternChars[head].Equals((char)DCMSizeDelimitToken.separator))
                            throw new DCMExException("Synax error in identify number interval.");
                        head++;
                        rightNumber = indentifyNextNumber(ref head);
                        rightToken = indentifyIntervalRightToken(ref head);
                        break;
                    case (char)DCMTranformToken.TransTag:
                        return;
                    default:
                        throw new DCMExException("Synax error in identify number interval.");
                }
            }
        }
        private DCMSizeDelimitToken indentifyIntervalRightToken(ref int head)
        {
            head = filterWhiteSpace(head);
            if (head < sizePatternChars.Length)
            {
                char ch = sizePatternChars[head];
                switch (ch)
                {
                    case (char)DCMSizeDelimitToken.rightClosed:
                        head++;
                        return DCMSizeDelimitToken.rightClosed;
                    case (char)DCMSizeDelimitToken.rightOpen:
                        head++;
                        return DCMSizeDelimitToken.rightOpen;
                    default:
                        throw new DCMExException("Synax error in identify size interval.");
                }
            }
            throw new DCMExException("Synax error in identify size interval.");
        }

        private Int32 indentifyNextNumber(ref int head)
        {
            int newHead = head;
            StringBuilder sb = new StringBuilder();
            while (newHead < sizePatternChars.Length)
            {
                char ch = sizePatternChars[newHead];
                switch (ch)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        sb.Append(ch);
                        break;
                    default:
                        goto NumberLoopEnd;
                }
                newHead++;
            }
        NumberLoopEnd:
            if (sb.Length > 0)
            {
                char lch = sizePatternChars[newHead - 1];
                if (lch.Equals('.') || (lch >= '0' && lch <= '9'))
                {
                    Int32 val = Int32.Parse(sb.ToString());
                    head = newHead;
                    return val;
                }
            }
        throw new DCMExException("Synax error in identify size characters.");
        }
        /// <summary>
        /// 从当前游标位置过滤可能存在的空白字符序列，返回非空白字符位序。
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private int filterWhiteSpace(int head)
        {
            char ch;
            while (head < sizePatternChars.Length)
            {
                ch = sizePatternChars[head];
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
            int size = checkChars.Length;
            if (leftNumber>-1 && rightNumber>-1)
            {
                bool res = leftToken.Equals(DCMSizeDelimitToken.leftClosed) ? size >= leftNumber : size > leftNumber;
                if (res)
                    res = rightToken.Equals(DCMSizeDelimitToken.rightClosed) ? size <= rightNumber : size < rightNumber;
                return res;
            }
            return true;
        }
        public void setGroupFetchTag(GroupFetcher groupFetchTag)
        {
            this.groupFetchTag = groupFetchTag;
        }
        public string[] getGroups()
        {
            return null;
        }
    }
}
