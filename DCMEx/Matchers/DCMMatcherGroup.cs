using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx.Matchers
{
    class DCMMatcherGroup : IMatcher
    {
        #region Members

        private List<KeyValuePair<LogicSymbol, IMatcher>> logicMatchers = new List<KeyValuePair<LogicSymbol, IMatcher>>();
        List<string> groups = null;
        private readonly char[] expChars;
        private int cursor;
        private readonly int expFrom;
        private readonly int expEnd;
        public GroupFetcher _groupFetchTag = null;
        #endregion

        #region Methods

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
            string groupFetchTag = GroupTag.FetchTag.ToString();
            //////////////////////////////////////////////////////////////////////////////////////////////
            //尝试获取初始的逻辑组合标记量
            if (!preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref groupFetchTag, ref head, ref beg))
            {
                if (detectNewPattern(head + 1, out pMode, out beg))
                {
                    if (detectGroupTag(beg, out groupFetchTag, out beg))
                    {
                        cursor = beg + 1;
                        head = beg;
                        if (pMode.Equals(PatternMode.COMPLEX))
                        {
                            if (detectPairedBrace(head + 1, out beg, 1))
                            {
                                setLogicMatchers(lSymbol, PatternMode.COMPLEX, groupFetchTag, head + 1, beg - 1);
                                cursor = beg + 1;
                                head = beg;
                            }
                            else
                                throw new DCMExException("Synax error in matching the closing brace!");
                        }
                    }
                    else
                    {
                        throw new DCMExException("Synax error in identify group tag!");
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
            //MatchLoopStart:
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
                        preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref groupFetchTag, ref head, ref beg);
                        ///////////////////////////////////////////////////////////////////////////////////
                        break;
                    case (char)DelimitToken.SubEx: //结尾界定符标志
                        if (!preDetectConnectiveLogicMatchers(ref lSymbol, ref pMode, ref groupFetchTag, ref head, ref beg))
                        {
                            if (!detectEnd(head))
                            {
                                throw new DCMExException("Synax error in matching the closing colon Tag!");
                            }
                            goto MatchLoopFinish;
                        }
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
                        throw new DCMExException("Synax error in matching the closing brace, can note parse the expression!");
                    case (char)EscapeToken.EscapeNext: //转义字符，向前跳跃一个字符位
                        ++head;
                        break;
                    default:
                        break;
                }
                ++head;
            }
        MatchLoopFinish:
            //////////////////////////////////////////////////////////////////////////////////////////////
            //如果留有逻辑表达式序列，缓存截取到的验证逻辑对象
            if (cursor <= expEnd && cursor <= head)
            {
                setLogicMatchers(lSymbol, pMode, groupFetchTag, cursor, head);
                cursor = head + 1;
            }
            //////////////////////////////////////////////////////////////////////////////////////////////
        }

        private bool detectEnd(int head)
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
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 探测逻辑组合表达式的存在，如果存在缓存截取到的验证逻辑对象，并置入新的逻辑组合标记量。
        /// 说明：
        /// 1.如果逻辑表达词存在，则模式符也应该存在。
        /// </summary>
        /// <param name="lSymbol"></param>
        /// <param name="pMode"></param>
        /// <param name="groupFetchTag"></param>
        /// <param name="head"></param>
        /// <param name="beg"></param>
        /// <returns></returns>
        private bool preDetectConnectiveLogicMatchers(ref LogicSymbol lSymbol, ref PatternMode pMode, ref string groupFetchTag, ref int head, ref int beg)
        {
            LogicSymbol symbol;
            PatternMode mode;
            if (detectLogicSymbol(head + 1, out symbol, out beg))
            {
                if (detectNewPattern(beg, out mode, out beg))
                {
                    if (detectGroupTag(beg, out groupFetchTag, out beg))
                    {
                        //记录上一个逻辑校验表达式部分
                        setLogicMatchers(lSymbol, pMode, groupFetchTag, cursor, head);
                        //缓存当前逻辑校验表达式标记
                        lSymbol = symbol;
                        pMode = mode;
                        cursor = beg + 1;
                        head = beg;
                        if (mode.Equals(PatternMode.COMPLEX))
                        {
                            if (detectPairedBrace(head, out beg, 1))
                            {
                                //存在分组子句，插入复合式校验表达式验证逻辑
                                setLogicMatchers(lSymbol, PatternMode.COMPLEX, groupFetchTag, head + 1, beg - 1);
                                cursor = beg + 1;
                                head = beg;
                            }
                            else
                                throw new DCMExException("Synax error in matching the closing brace!");
                        }
                        return true;
                    }
                    else
                    {
                        throw new DCMExException("Synax error in identify group tag!");
                    }
                }
                else
                {
                    throw new DCMExException("Synax error in assemble the logic corporation!");
                }
            }
            return false;
        }
        /// <summary>
        /// 探测配对的右括号出现的位置，返回探测成功与否。
        /// </summary>
        /// <param name="head"></param>
        /// <param name="beg"></param>
        /// <param name="initCount"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 从当前游标位置过滤可能存在的空白字符序列，返回非空白字符位序。
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 根据指定的模式与字符序列，创建相应的校验对象。
        /// </summary>
        /// <param name="lSymbol"></param>
        /// <param name="pMode"></param>
        /// <param name="groupFetchTag"></param>
        /// <param name="from"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private bool setLogicMatchers(LogicSymbol lSymbol, PatternMode pMode, string groupFetchTag, int from, int end)
        {
            IMatcher matcher = DCMMatcherFactory.getMatcher(pMode, expChars, from > expChars.Length ? expChars.Length : from,
                end > expChars.Length ? expChars.Length : end);
            if (matcher != null)
            {
                if (groupFetchTag != null && groupFetchTag.Length > 0)
                    matcher.setGroupFetchTag(new GroupFetcher(groupFetchTag));
                logicMatchers.Add(new KeyValuePair<LogicSymbol, IMatcher>(lSymbol, matcher));
                return true;
            }
            return false;
        }
        /// <summary>
        /// 探测模式符识别之后存在合法的定界符
        /// </summary>
        /// <param name="head"></param>
        /// <returns></returns>
        private bool detectValidPatternDelimeter(int head)
        {
            if (head < expChars.Length)
            {
                char ch = expChars[head];
                if (ch.Equals((char)GroupTag.FetchTag) || ch.Equals((char)DelimitToken.SubEx) || ch.Equals((char)DelimitToken.EndLine) || ch.Equals((char)DelimitToken.NewLine) || ch.Equals((char)DelimitToken.WhiteSpace))
                {
                    return true;
                }
            }
            else
                return true;
            return false;
        }
        /// <summary>
        /// 探测可能存在的分组标记
        /// </summary>
        /// <param name="head"></param>
        /// <param name="groupFetchTag"></param>
        /// <param name="beg"></param>
        /// <returns></returns>
        private bool detectGroupTag(int head, out string groupFetchTag, out int beg)
        {
            string fetchTag = (char)GroupTag.FetchTag + "0";
            if (head < expEnd)
            {
                int newHead = filterWhiteSpace(head);
                char ch = expChars[newHead];
                if (ch.Equals((char)DelimitToken.SubEx))
                {
                    beg = newHead;
                    groupFetchTag = fetchTag;
                    return true;
                }
                else if (ch.Equals((char)GroupTag.FetchTag))
                {
                    fetchTag = ch.ToString();
                    while ((++newHead) < expEnd)
                    {
                        ch = expChars[newHead];
                        if (ch.Equals((char)DelimitToken.EndLine) || ch.Equals((char)DelimitToken.NewLine) || ch.Equals((char)DelimitToken.WhiteSpace) || ch.Equals((char)DelimitToken.SubEx))
                        {
                            newHead = filterWhiteSpace(newHead);
                            if (newHead < expEnd && expChars[newHead].Equals(DelimitToken.SubEx))
                            {
                                beg = newHead;
                                groupFetchTag = fetchTag;
                                return true;
                            }
                        }
                        fetchTag += ch.ToString();
                    }
                    if (newHead == expEnd)
                    {
                        beg = newHead;
                        groupFetchTag = fetchTag;
                        return true;
                    }
                }
            }
            beg = -1;
            groupFetchTag = null;
            return false;
        }

        /// <summary>
        /// 向前探测可能存在的模式符
        /// 说明：
        /// 1.如果存在有效的模式符（REGEX|DATE|NUMBER|URI|FILE|SIZE）则更新第二游标值，返回真。
        /// 2.如果匹配失败，使用默认正则模式符，返回假。
        /// </summary>
        /// <param name="head"></param>
        /// <param name="mode"></param>
        /// <param name="beg"></param>
        /// <returns></returns>
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
                                if (detectValidPatternDelimeter(newHead + 5))
                                {
                                    beg = filterWhiteSpace(newHead + 5);
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
                                if (detectValidPatternDelimeter(newHead + 4))
                                {
                                    beg = filterWhiteSpace(newHead + 4);
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
                                if (detectValidPatternDelimeter(newHead + 6))
                                {
                                    beg = filterWhiteSpace(newHead + 6);
                                    mode = PatternMode.NUMBER;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'U':
                        if (newHead < expEnd - 3)
                        {
                            if (expChars[newHead + 1].Equals('R') && expChars[newHead + 2].Equals('I'))
                            {
                                if (detectValidPatternDelimeter(newHead + 3))
                                {
                                    beg = filterWhiteSpace(newHead + 3);
                                    mode = PatternMode.URI;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'F':
                        if (newHead < expEnd - 4)
                        {
                            if (expChars[newHead + 1].Equals('I') && expChars[newHead + 2].Equals('L') && expChars[newHead + 3].Equals('E'))
                            {
                                if (detectValidPatternDelimeter(newHead + 4))
                                {
                                    beg = filterWhiteSpace(newHead + 4);
                                    mode = PatternMode.FILE;
                                    return true;
                                }
                            }
                        }
                        break;
                    case 'S':
                        if (newHead < expEnd - 4)
                        {
                            if (expChars[newHead + 1].Equals('I') && expChars[newHead + 2].Equals('Z') && expChars[newHead + 3].Equals('E'))
                            {
                                if (detectValidPatternDelimeter(newHead + 4))
                                {
                                    beg = filterWhiteSpace(newHead + 4);
                                    mode = PatternMode.SIZE;
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
        /// <summary>
        /// 探测可能存在的逻辑词
        /// 说明：
        /// 1.如果存在逻辑表达关键字，则更新第二游标，返回真。
        /// 2.如果匹配失败，使用默认的串联逻辑关系，返回假。
        /// </summary>
        /// <param name="head"></param>
        /// <param name="symbol"></param>
        /// <param name="beg"></param>
        /// <returns></returns>
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
                                    return true;
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
                                    return true;
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
                groups.Clear();
                groups.AddRange(newGroups);
            }
        }
        public void setGroupFetchTag(GroupFetcher groupFetchTag)
        {
            this._groupFetchTag = groupFetchTag;
        }
        public string[] getGroups()
        {
            return groups == null ? null : groups.ToArray();
        }
        #endregion

    }
}
