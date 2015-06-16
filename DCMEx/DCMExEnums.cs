using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMEx
{
    /// <summary>
    /// 前导符设定
    /// </summary>
    internal enum PrefixToken
    {
        DefaultMode = 0,  //默认的解析模式前导符（用户不可见）
        RegexOnly = '/',  //限定正则表达式前导符
        Deprecated = '*', //无匹配前导符，自动注释后续表达式描述的前导符
    }
    /// <summary>
    /// 逻辑符
    /// </summary>
    enum LogicSymbol
    {
        AND = '&',
        OR = '|',
        NOT = '~',
    }
    /// <summary>
    /// 模式符定义
    /// </summary>
    enum PatternMode
    {
        COMPLEX = 0, //默认的匹配模式（用户不可见）
        REGEX = 1,
        DATE = 2,
        NUMBER = 3,
        FILE = 4,
        URI = 5,
        SIZE = 6
    }
    /// <summary>
    /// 定界符
    /// </summary>
    enum DelimitToken
    {
        SubEx = ':',
        WhiteSpace = ' ',
        NewLine = '\r',
        EndLine = '\n',
        leftBrace = '(',
        rightBrace = ')'
    }
    /// <summary>
    /// 转义符
    /// </summary>
    enum EscapeToken
    {
        EscapeNext = '\\',
    }
    /// <summary>
    /// 分组获取符
    /// </summary>
    enum GroupTag
    {
        FetchTag = '$'
    }
    /// <summary>
    /// 变换符
    /// </summary>
    enum DCMTranformToken
    {
        TransTag='>'
    }
    /// <summary>
    /// 数值部分的模式符
    /// </summary>
    enum DCMNumberPatternToken
    {
        Float='F',
        Integer='I'
    }
    /// <summary>
    /// 数值部分的定界符
    /// </summary>
    enum DCMNumberDelimitToken
    {
        leftOpen='(',
        rightOpen=')',
        leftClosed='[',
        rightClosed=']',
        separator=','
    }
    /// <summary>
    /// 日期部分的模式符
    /// </summary>
    enum DCMDatePatternToken
    {
        Fuzzy='F',
        Strict='S'
    }
    /// <summary>
    /// 日期部分的定界符
    /// </summary>
    enum DCMDateDelimitToken
    {
        leftBrace = '(',
        rightBrace = ')'
    }
}
