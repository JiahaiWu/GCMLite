using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    /// <summary>
    /// 数值匹配验证实现方法类
    /// 说明：
    /// 1.数值匹配验证表达式基本范式：
    /// [{"F" | "I"}]{[%d91 | %d40] number, number [%d91 | %d40]}[{>format}]
    /// 模式符分为浮点类型和整型两种，默认为浮点类型。
    /// 数值区间以圆括号为开区间，方括号为闭区间。
    /// 如伴随变换符则执行数值字符串格式化转换操作。
    /// </summary>
    class DCMNumberMatcher : IMatcher
    {
        public DCMNumberMatcher(char[] expChars, int from, int end)
        {
            if (end > from)
            {
                numPatternChars = new char[end - from];
                for (int i = from; i < end; i++)
                {
                    numPatternChars[i] = expChars[from + i];
                }
                parseformatPattern();
            }
        }

        private void parseformatPattern()
        {
            if (numPatternChars != null)
            {
                int head = filterWhiteSpace(0);
                char ch = numPatternChars[head];
                switch (ch)
                {
                    case (char)DCMNumberDelimitToken.leftClosed:
                    case (char)DCMNumberDelimitToken.leftOpen:
                    case (char)DCMNumberPatternToken.Float:
                        numberPattern = DCMNumberPatternToken.Float;
                        break;
                    case (char)DCMNumberPatternToken.Integer:
                        numberPattern = DCMNumberPatternToken.Integer;
                        break;
                    default:
                        throw new DCMExException("Synax error in identify number pattern tag.");
                }
                head++;
                head = filterWhiteSpace(head);
                detectNumberInterval(ref head);
                head = filterWhiteSpace(head);
                detectNumberTransFormat(ref head);
            }
        }

        private void detectNumberTransFormat(ref int head)
        {
            if (head < numPatternChars.Length)
            {
                char ch = numPatternChars[head];
                if (ch.Equals((char)DCMTranformToken.TransTag))
                {
                    transformFormat = new string(numPatternChars, head + 1, numPatternChars.Length - head + 1).TrimEnd();
                }
                else
                {
                    throw new DCMExException("Synax error in identify number tranform formats.");
                }
            }
        }

        private void detectNumberInterval(ref int head)
        {
            if (head < numPatternChars.Length)
            {
                char ch = numPatternChars[head];
                switch(ch)
                {
                    case (char)DCMNumberDelimitToken.leftOpen:
                        leftToken = DCMNumberDelimitToken.leftOpen;
                        head++;
                        leftNumber = indentifyNextNumber(ref head);
                        if(!numPatternChars[head].Equals((char)DCMNumberDelimitToken.separator))
                            throw new DCMExException("Synax error in identify number interval.");
                        head++;
                        rightNumber = indentifyNextNumber(ref head);
                        rightToken = indentifyIntervalRightToken(ref head);
                        break;
                    case (char)DCMNumberDelimitToken.leftClosed:
                        leftToken = DCMNumberDelimitToken.leftClosed;
                        head++;
                        leftNumber = indentifyNextNumber(ref head);
                        if(!numPatternChars[head].Equals((char)DCMNumberDelimitToken.separator))
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

        private DCMNumberDelimitToken indentifyIntervalRightToken(ref int head)
        {
            head = filterWhiteSpace(head);
            if (head < numPatternChars.Length)
            {
                char ch = numPatternChars[head];
                switch (ch)
                {
                    case (char)DCMNumberDelimitToken.rightClosed:
                        head++;
                        return DCMNumberDelimitToken.rightClosed;
                    case (char)DCMNumberDelimitToken.rightOpen:
                        head++;
                        return DCMNumberDelimitToken.rightOpen;
                    default:
                        throw new DCMExException("Synax error in identify number interval.");
                }
            }
            throw new DCMExException("Synax error in identify number interval.");
        }

        private double indentifyNextNumber(ref int head)
        {
            int newHead = head;
            StringBuilder sb=new StringBuilder();
            bool floatTag = false;
            bool efloatTag = false;
            while (newHead < numPatternChars.Length)
            {
                char ch = numPatternChars[newHead];
                switch (ch)
                {
                    case '+':
                    case '-':
                        if (sb.Length == 0)
                        {
                            sb.Append(ch);
                            break;
                        }
                        goto NumberLoopEnd;
                    case '.':
                        if (floatTag == false)
                        {
                            floatTag = true;
                            sb.Append(ch);
                            break;
                        }
                        goto NumberLoopEnd;
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
                    case 'E':
                    case 'e':
                        if (efloatTag == false)
                        {
                            if (newHead + 1 < numPatternChars.Length)
                            {
                                char nch = numPatternChars[newHead + 1];
                                if (nch.Equals('+') || nch.Equals('-'))
                                {
                                    if (newHead + 2 < numPatternChars.Length)
                                    {
                                        efloatTag = true;
                                        sb.Append(ch);
                                        sb.Append(nch);
                                        newHead++;
                                        break;
                                    }
                                }
                                else if (nch >= '0' && nch <= '9')
                                {
                                    efloatTag = true;
                                    sb.Append(ch);
                                    sb.Append(nch);
                                    newHead++;
                                    break;
                                }
                            }
                        }
                        goto NumberLoopEnd;
                    default:
                        goto NumberLoopEnd;
                }
                newHead++;
            }
            NumberLoopEnd:
            if (sb.Length > 0)
            {
                char lch = numPatternChars[newHead - 1];
                if (lch.Equals('.') || (lch >= '0' && lch <= '9'))
                {
                    double val = double.Parse(sb.ToString());
                    head = newHead;
                    return val;
                }
            }
            throw new DCMExException("Synax error in identify number characters.");
        }

        /// <summary>
        /// 从当前游标位置过滤可能存在的空白字符序列，返回非空白字符位序。
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private int filterWhiteSpace(int head)
        {
            char ch;
            while (head < numPatternChars.Length)
            {
                ch = numPatternChars[head];
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
            if (numberPattern.Equals(DCMNumberPatternToken.Float))
            {
                double dval;
                bool res = Double.TryParse(checkChars, out dval);
                if (res && !Double.IsNaN(leftNumber) && !double.IsNaN(rightNumber))
                {
                    res = leftToken.Equals(DCMNumberDelimitToken.leftClosed) ? dval >= leftNumber : dval > leftNumber;
                    if(res)
                        res=rightToken.Equals(DCMNumberDelimitToken.rightClosed)?dval<=rightNumber:dval<rightNumber;
                }
                return res;
            }
            else if (numberPattern.Equals(DCMNumberPatternToken.Integer))
            {
                Int64 val;
                bool res = Int64.TryParse(checkChars, out val);
                if (res && !Double.IsNaN(leftNumber) && !double.IsNaN(rightNumber))
                {
                    Int64 leftNum = Convert.ToInt64(leftNumber);
                    Int64 rightNum = Convert.ToInt64(rightNumber);
                    res = leftToken.Equals(DCMNumberDelimitToken.leftClosed) ? val >= leftNum : val > leftNum;
                    if (res)
                        res = rightToken.Equals(DCMNumberDelimitToken.rightClosed) ? val <= rightNum : val < rightNum;
                }
                return res;
            }
            Decimal decial;
            return Decimal.TryParse(checkChars, out decial);
        }

        public void setGroupFetchTag(GroupFetcher fetchTag)
        {
            this.groupFetchTag = fetchTag;
        }

        public string[] getGroups()
        {
            return null;
        }

        public GroupFetcher groupFetchTag = null;
        private readonly char[] numPatternChars;
        private DCMNumberPatternToken numberPattern = DCMNumberPatternToken.Float;
        private double leftNumber=Double.NaN;
        private double rightNumber=Double.NaN;
        private DCMNumberDelimitToken leftToken=DCMNumberDelimitToken.leftClosed;
        private DCMNumberDelimitToken rightToken = DCMNumberDelimitToken.rightClosed;
        private string transformFormat = null;
    }
}
