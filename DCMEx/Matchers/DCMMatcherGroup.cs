using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMMatcherGroup : IMatcher
    {
        private List<KeyValuePair<LogicSymbol, IMatcher>> logicMatchers = new List<KeyValuePair<LogicSymbol, IMatcher>>();
        List<string> groups = null;
        private readonly char[] expChars;
        private int cursor;
        private readonly int expFrom;
        private readonly int expEnd;

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="expChars"></param>
        /// <param name="expFrom"></param>
        /// <param name="expEnd"></param>
        public DCMMatcherGroup(char[] expChars, int expFrom, int expEnd)
        {
            this.expChars = expChars;
            this.cursor = expFrom;
            this.expFrom = expFrom;
            this.expEnd = expEnd;
            buildLogicMatchers();
        }

        /// <summary>
        /// 对表达式解析生成验证逻辑对象集合
        /// </summary>
        private void buildLogicMatchers()
        {
            int head = cursor - 1; //探针标识
            int beg = cursor;    //二次探针标识
            char ch = (char)DelimitToken.WhiteSpace;
            LogicSymbol lSymbol = LogicSymbol.AND;
            PatternMode pMode = PatternMode.REGEX;
            //////////////////////////////////////////////////////////////////////////////////////////////
            //尝试获取初始的逻辑组合标记量
            if (!preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref head, ref beg))
            {
                if (detectNewPattern(head + 1, out pMode, out beg))
                {
                    cursor = beg + 1;
                    head = beg;
                    if (pMode.Equals(PatternMode.COMPLEX))
                    {
                        if (detectPairedBrace(head + 1, out beg, 1))
                        {
                            setLogicMatchers(lSymbol, PatternMode.COMPLEX, head + 1, beg - 1);
                            cursor = beg + 1;
                            head = beg;
                        }
                        else
                            throw new DCMExException("Synax error in matching the closing brace!");
                    }
                }
                else
                {
                    lSymbol = LogicSymbol.AND;
                    pMode = PatternMode.REGEX;
                }
            }
            head++;
            //////////////////////////////////////////////////////////////////////////////////////////////
            while (head < expEnd)
            {
                ch = expChars[head];
                switch (ch)
                {
                    case (char)DelimitToken.EndLine:
                    case (char)DelimitToken.NewLine:
                    case (char)DelimitToken.WhiteSpace:
                        /////////////////////////////////////////////////////////////////////////////////
                        //当遇到空白符时探测逻辑组合表达式的存在，如果存在缓存截取到的验证逻辑对象，并置入新的逻辑组合标记量
                        preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref head, ref beg);
                        ///////////////////////////////////////////////////////////////////////////////////
                        break;
                    case (char)DelimitToken.SubEx: //结尾界定符标志
                        if (!preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref head, ref beg))
                            throw new DCMExException("Synax error in matching the closing colon Tag!");
                        break;
                    case (char)DelimitToken.leftBrace: //条件子句快进
                        if (detectPairedBrace(head, out beg))
                        {
                            head = beg;
                        }
                        else
                            throw new DCMExException("Synax error in matching the closing brace!");
                        break;
                    case (char)DelimitToken.rightBrace:
                        throw new DCMExException("Illegal synax, can note parse the expression!");
                    case (char)EscapeToken.EscapeNext: //转义字符，向前跳跃一个字符位
                        ++head;
                        break;
                    default:
                        break;
                }
                ++head;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////
            //如果留有逻辑表达式序列，缓存截取到的验证逻辑对象
            if (cursor <= expEnd && cursor <= head)
            {
                setLogicMatchers(lSymbol, pMode, cursor, head);
                cursor = head;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////
        }

        private bool preDetectConnectiveLogicMatchers(ref LogicSymbol lSymbol, ref PatternMode pMode, ref int head, ref int beg)
        {
            LogicSymbol symbol;
            PatternMode mode;
            if (detectLogicSymbol(head + 1, out symbol, out beg) && detectNewPattern(beg, out mode, out beg))
            {
                setLogicMatchers(lSymbol, pMode, cursor, head);
                lSymbol = symbol;
                pMode = mode;
                cursor = beg + 1;
                head = beg;
                if (mode.Equals(PatternMode.COMPLEX))
                {
                    if (detectPairedBrace(head, out beg, 1))
                    {
                        setLogicMatchers(lSymbol, PatternMode.COMPLEX, head + 1, beg - 1);
                        cursor = beg + 1;
                        head = beg;
                    }
                    else
                        throw new DCMExException("Synax error in matching the closing brace!");
                }
                return true;
            }
            return false;
        }

        private bool detectPairedBrace(int head, out int beg, int initCount = 0)
        {
            char ch;
            int cc = initCount;
            while (head < expEnd)
            {
                ch = expChars[head];
                switch (ch)
                {
                    case (char)EscapeToken.EscapeNext:
                        ++head;
                        break;
                    case (char)DelimitToken.leftBrace:
                        ++cc;
                        break;
                    case (char)DelimitToken.rightBrace:
                        --cc;
                        if (cc == 0)
                        {
                            beg = head;
                            return true;
                        }
                        break;
                    default:
                        break;
                }
                ++head;
            }
            beg = -1;
            return false;
        }

        private int filterWhiteSpace(int head)
        {
            char ch;
            while (head < expEnd)
            {
                ch = expChars[head];
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

        private bool setLogicMatchers(LogicSymbol lSymbol, PatternMode pMode, int from, int end)
        {
            IMatcher matcher = DCMMatcherFactory.getMatcher(pMode, expChars, from > expChars.Length ? expChars.Length : from, end > expChars.Length ? expChars.Length : end);
            if (matcher != null)
            {
                logicMatchers.Add(new KeyValuePair<LogicSymbol, IMatcher>(lSymbol, matcher));
                return true;
            }
            return false;
        }

        private bool detectNewPattern(int head, out PatternMode mode, out int beg)
        {
            if (head < expEnd)
            {
                int newHead = filterWhiteSpace(head);
                char ch = expChars[newHead];
                int newBeg = -1;
                switch (ch)
                {
                    case (char)DelimitToken.leftBrace:
                        newBeg = filterWhiteSpace(newHead + 1);
                        beg = newBeg - 1;
                        mode = PatternMode.COMPLEX;
                        return true;
                    case 'R':
                        if (newHead < expEnd - 5)
                        {
                            if (expChars[newHead + 1].Equals('E') && expChars[newHead + 2].Equals('G') && expChars[newHead + 3].Equals('E') && expChars[newHead + 4].Equals('X'))
                            {
                                newBeg = filterWhiteSpace(newHead + 5);
                                if (expChars[newBeg].Equals((char)DelimitToken.SubEx))
                                {
                                    beg = newBeg;
                                    mode = PatternMode.REGEX;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'D':
                        if (newHead < expEnd - 4)
                        {
                            if (expChars[newHead + 1].Equals('A') && expChars[newHead + 2].Equals('T') && expChars[newHead + 3].Equals('E'))
                            {
                                newBeg = filterWhiteSpace(newHead + 4);
                                if (expChars[newBeg].Equals((char)DelimitToken.SubEx))
                                {
                                    beg = newBeg;
                                    mode = PatternMode.DATE;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'N':
                        if (newHead < expEnd - 6)
                        {
                            if (expChars[newHead + 1].Equals('U') && expChars[newHead + 2].Equals('M') && expChars[newHead + 3].Equals('B') && expChars[newHead + 4].Equals('E') && expChars[newHead + 5].Equals('R'))
                            {
                                newBeg = filterWhiteSpace(newHead + 6);
                                if (expChars[newBeg].Equals((char)DelimitToken.SubEx))
                                {
                                    beg = newBeg;
                                    mode = PatternMode.NUMBER;
                                    return true;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            mode = PatternMode.REGEX;
            beg = -1;
            return false;
        }

        private bool detectLogicSymbol(int head, out LogicSymbol symbol, out int beg)
        {
            if (head < expEnd)
            {
                int newHead = filterWhiteSpace(head);
                char ch = expChars[newHead];
                switch (ch)
                {
                    case (char)LogicSymbol.AND:
                    case (char)LogicSymbol.OR:
                    case (char)LogicSymbol.NOT:
                        if (newHead < expEnd)
                        {
                            int newBeg = filterWhiteSpace(newHead + 1);
                            ///////////////////////////////////////////
                            //if(newBeg>newHead)
                            //不适用空白符约束
                            ///////////////////////////////////////////
                            {
                                beg = newBeg - 1;
                                symbol = (LogicSymbol)ch;
                                return true;
                            }
                        }
                        break;
                    case 'A':
                        if (newHead < expEnd - 3)
                        {
                            if (expChars[newHead + 1].Equals('N') && expChars[newHead + 2].Equals('D'))
                            {
                                int newBeg = filterWhiteSpace(newHead + 3);
                                if (newBeg > newHead + 3)
                                {
                                    beg = newBeg;
                                    symbol = LogicSymbol.AND;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'O':
                        if (newHead < expEnd - 2)
                        {
                            if (expChars[newHead + 1].Equals('R'))
                            {
                                int newBeg = filterWhiteSpace(newHead + 2);
                                if (newBeg > newHead + 2)
                                {
                                    beg = newBeg;
                                    symbol = LogicSymbol.OR;
                                }
                            }
                        }
                        break;
                    case 'N':
                        if (newHead < expEnd - 3)
                        {
                            if (expChars[newHead + 1].Equals('O') && expChars[newHead + 2].Equals('T'))
                            {
                                int newBeg = filterWhiteSpace(newHead + 3);
                                if (newBeg > newHead + 3)
                                {
                                    beg = newBeg;
                                    symbol = LogicSymbol.NOT;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            symbol = LogicSymbol.AND;
            beg = -1;
            return false;
        }

        public bool isMatch(string chars, params string[] _groups)
        {
            bool res = true;

            foreach (KeyValuePair<LogicSymbol, IMatcher> pair in logicMatchers)
            {
                switch (pair.Key)
                {
                    case LogicSymbol.AND:
                        res = res & pair.Value.isMatch(chars);
                        if (res)
                            syncGroups(pair.Value.getGroups(), groups);
                        else
                            return false;
                        break;
                    case LogicSymbol.OR:
                        res = res | pair.Value.isMatch(chars);
                        if (res)
                            syncGroups(pair.Value.getGroups(), groups);
                        else
                            return false;
                        break;
                    case LogicSymbol.NOT:
                        res = res & !pair.Value.isMatch(chars);
                        if (res)
                            syncGroups(pair.Value.getGroups(), groups);
                        else
                            return false;
                        break;
                    default:
                        throw new DCMExException("Unsupported Logic Symbol!");
                }
            }
            return res;
        }

        private void syncGroups(string[] newGroups, List<string> groups)
        {
            if (newGroups != null && newGroups.Length > 0)
            {
                groups = new List<string>();
                groups.AddRange(newGroups);
            }
        }
        public string[] getGroups()
        {
            return groups == null ? null : groups.ToArray();
        }
    }
}
